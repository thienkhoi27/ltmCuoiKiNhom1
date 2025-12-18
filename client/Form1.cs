using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net.Security;
using System.Security.Authentication;
using System.Text;
using System.Windows.Forms;


namespace CuoiKiLTM
{
    public partial class Form1 : Form
    {
        private TcpClient? tcpClient;
        private SslStream? sslStream;
        private StreamReader? reader;
        private StreamWriter? writer;
        private bool hasVoted = false;
        private string? clientId;
        private int remainSeconds = 0;
        private bool timeExpired = false;

        public Form1()
        {
            InitializeComponent();
            UpdateUiDisconnected();

            lstOptions.DrawMode = DrawMode.OwnerDrawFixed;
            lstOptions.ItemHeight = 33;
            lstOptions.DrawItem += lstOptions_DrawItem;

            voteTimer.Interval = 1000;                 // 1 giây
            voteTimer.Tick += voteTimer_Tick;          // gắn event Tick
        }

        // =========================
        //   Tiện ích UI / Log
        // =========================

        private void Log(string msg)
        {
            txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {msg}\r\n");
        }

        private void UpdateUiConnected()
        {
            btnConnect.Enabled = false;
            btnVote.Enabled = true;
            btnResult.Enabled = true;
        }

        private void UpdateUiDisconnected()
        {
            btnConnect.Enabled = true;
            btnVote.Enabled = false;
            btnResult.Enabled = false;
        }

        // =========================
        //   Gửi / Nhận qua TLS
        // =========================

        private void SendLine(string text)
        {
            if (writer == null) return;
            writer.WriteLine(text);
            writer.Flush();
            Log(">> " + text);
        }

        private string? ReadLine()
        {
            if (reader == null) return null;
            string? line = reader.ReadLine();
            if (line != null)
            {
                Log("<< " + line);
            }
            return line;
        }

        // =========================
        //   Nút Kết nối & Lấy đề tài
        // =========================

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                string host = txtServer.Text.Trim();
                if (!int.TryParse(txtPort.Text.Trim(), out int port))
                {
                    MessageBox.Show("Port không hợp lệ");
                    return;
                }

                clientId = txtClientId.Text.Trim();
                if (string.IsNullOrEmpty(clientId))
                {
                    MessageBox.Show("Vui lòng nhập MSSV / ID");
                    return;
                }

                // Tạo TCP client
                tcpClient = new TcpClient(host, port);

                // Tạo SSL stream (chấp nhận mọi chứng chỉ self-signed -> dùng cho lab)
                sslStream = new SslStream(
                    tcpClient.GetStream(),
                    false,
                    (sender2, certificate, chain, errors) => true // bỏ qua validate cert
                );

                // Bắt tay TLS
                sslStream.AuthenticateAsClient(host, null,
                    SslProtocols.Tls12 | SslProtocols.Tls13, false);

                reader = new StreamReader(sslStream, Encoding.UTF8);
                writer = new StreamWriter(sslStream, Encoding.UTF8) { AutoFlush = true };

                Log("Đã kết nối TLS tới server.");

                // Gửi HELLO|<client_id>
                SendLine($"HELLO|{clientId}");

                // Nhận WELCOME|topic|opt1,opt2,...
                string? resp = ReadLine();
                if (resp == null)
                {
                    MessageBox.Show("Server đóng kết nối.");
                    CloseConnection();
                    return;
                }

                string[] parts = resp.Split('|');
                if (parts.Length != 4 || parts[0] != "WELCOME")
                {
                    MessageBox.Show("Server trả về sai định dạng:\n" + resp);
                    CloseConnection();
                    return;
                }

                string topic = parts[1];
                string[] options = parts[2].Split(',');
                int limitTime = int.Parse(parts[3]); // NEW

                remainSeconds = limitTime;
                timeExpired = false;
                lblTimer.Text = $"{remainSeconds}s";
                voteTimer.Start();



                lblTopic.Text = topic;
                lstOptions.Items.Clear();
                foreach (var opt in options)
                {
                    lstOptions.Items.Add(opt);
                }

                hasVoted = false;
                UpdateUiConnected();
                Log("Nhận chủ đề và danh sách lựa chọn từ server.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi kết nối: " + ex.Message);
                Log("Lỗi khi kết nối: " + ex);
                CloseConnection();
            }
        }

        // =========================
        //   Nút Bỏ phiếu
        // =========================

        private void btnVote_Click(object sender, EventArgs e)
        {
            try
            {
                if (sslStream == null || !sslStream.CanWrite)
                {
                    MessageBox.Show("Chưa kết nối server.");
                    return;
                }

                if (lstOptions.SelectedIndex < 0)
                {
                    MessageBox.Show("Vui lòng chọn một lựa chọn.");
                    return;
                }

                if (hasVoted)
                {
                    MessageBox.Show("Bạn đã bỏ phiếu rồi (client).");
                    return;
                }

                if (timeExpired)
                {
                    MessageBox.Show("Đã hết thời gian bỏ phiếu!", "Thông báo");
                    return;
                }


                int idx = lstOptions.SelectedIndex + 1;

                // Gửi VOTE|<index>
                SendLine($"VOTE|{idx}");

                string? resp = ReadLine();
                if (resp == null)
                {
                    MessageBox.Show("Server đóng kết nối sau khi vote.");
                    CloseConnection();
                    return;
                }

                string[] parts = resp.Split('|');
                if (parts[0] == "OK" && parts.Length >= 3 && parts[1] == "VOTED")
                {
                    string optText = parts[2];
                    hasVoted = true;
                    MessageBox.Show("Bạn đã bỏ phiếu cho: " + optText, "Thành công");
                }
                else if (parts[0] == "ERR")
                {
                    string code = parts.Length > 1 ? parts[1] : "";
                    if (code == "ALREADY_VOTED")
                    {
                        hasVoted = true;
                        MessageBox.Show("Server báo: Bạn đã bỏ phiếu trước đó.", "Thông báo");
                    }
                    else if (code == "INVALID_OPTION")
                    {
                        MessageBox.Show("Lựa chọn không hợp lệ.", "Lỗi");
                    }
                    else if (code == "NOT_AUTH")
                    {
                        MessageBox.Show("Chưa HELLO mà đã VOTE (lỗi giao thức).", "Lỗi");
                    }
                    else
                    {
                        MessageBox.Show("Lỗi từ server: " + resp, "Lỗi");
                    }
                }
                else
                {
                    MessageBox.Show("Phản hồi không mong đợi từ server: " + resp);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi bỏ phiếu: " + ex.Message);
                Log("Lỗi khi bỏ phiếu: " + ex);
                CloseConnection();
            }
        }

        // =========================
        //   Nút Xem kết quả
        // =========================

        private void btnResult_Click(object sender, EventArgs e)
        {
            try
            {
                if (sslStream == null || !sslStream.CanWrite)
                {
                    MessageBox.Show("Chưa kết nối server.");
                    return;
                }

                SendLine("RESULT?");
                string? resp = ReadLine();
                if (resp == null)
                {
                    MessageBox.Show("Server đóng kết nối.");
                    CloseConnection();
                    return;
                }

                if (!resp.StartsWith("RESULT|"))
                {
                    MessageBox.Show("Server trả về kết quả không đúng định dạng:\n" + resp);
                    return;
                }

                string data = resp.Substring("RESULT|".Length);
                if (string.IsNullOrEmpty(data))
                {
                    MessageBox.Show("Chưa có phiếu nào.");
                    return;
                }

                var parts = data.Split(';');
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Kết quả hiện tại:");
                foreach (var p in parts)
                {
                    if (string.IsNullOrWhiteSpace(p)) continue;
                    var kv = p.Split(':');
                    if (kv.Length == 2)
                    {
                        sb.AppendLine($"- {kv[0]}: {kv[1]} phiếu");
                    }
                }

                MessageBox.Show(sb.ToString(), "Kết quả");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xem kết quả: " + ex.Message);
                Log("Lỗi khi xem kết quả: " + ex);
                CloseConnection();
            }
        }

        private void lstOptions_DrawItem(object? sender, DrawItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= lstOptions.Items.Count)
                return;

            string text = lstOptions.Items[e.Index]?.ToString() ?? "";

            int number = e.Index + 1;
            string displayText = $"Lựa chọn {number}:   {text}";



            bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            var backColor = isSelected ? Color.FromArgb(220, 235, 255) : Color.White;
            var foreColor = Color.Black;

            using (var backBrush = new SolidBrush(backColor))
                e.Graphics.FillRectangle(backBrush, e.Bounds);

            var textRect = new Rectangle(e.Bounds.X + 8, e.Bounds.Y + 7, e.Bounds.Width - 16, e.Bounds.Height - 14);

            using (var textBrush = new SolidBrush(foreColor))
            using (var font = new Font(e.Font.FontFamily, 10, FontStyle.Regular))
                e.Graphics.DrawString(displayText, font, textBrush, textRect);

            using (var pen = new Pen(Color.Gainsboro, 1))
                e.Graphics.DrawLine(pen, e.Bounds.Left + 4, e.Bounds.Bottom - 1, e.Bounds.Right - 4, e.Bounds.Bottom - 1);

            e.DrawFocusRectangle();
        }

        private void voteTimer_Tick(object sender, EventArgs e)
        {
            if (remainSeconds > 0)
            {
                remainSeconds--;
                lblTimer.Text = $"{remainSeconds}s";
            }
            else
            {
                timeExpired = true;
                voteTimer.Stop();
                lblTimer.Text = "Hết thời gian!";
                btnVote.Enabled = true;  // vẫn cho click để hiện popup
            }
        }


        // =========================
        //   Đóng kết nối
        // =========================

        private void CloseConnection()
        {
            try { reader?.Dispose(); } catch { }
            try { writer?.Dispose(); } catch { }
            try { sslStream?.Close(); } catch { }
            try { tcpClient?.Close(); } catch { }

            reader = null;
            writer = null;
            sslStream = null;
            tcpClient = null;

            UpdateUiDisconnected();
            Log("Đã đóng kết nối.");
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            CloseConnection();
        }

        private void lblTopic_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
