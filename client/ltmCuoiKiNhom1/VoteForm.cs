using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.Json.Nodes;
using System.Windows.Forms;

namespace ltmCuoiKiNhom1
{
    public partial class VoteForm : Form
    {
        private NetworkManager? _net;
        private VoteChart? _chart;

        private string _sessionId = "";
        private int _remainSeconds = 0;
        private bool _timeExpired = false;
        private bool _hasVoted = false;

        public VoteForm()
        {
            InitializeComponent();
            Text = "Tham gia Vote - Nhóm 1";

            voteTimer.Interval = 1000;
            voteTimer.Tick += voteTimer_Tick;

            UpdateUiDisconnected();
        }

        private void VoteForm_Load(object sender, EventArgs e)
        {
            _chart = new VoteChart();
            panelChart.Controls.Add(_chart.Control);

            lstOptions.DrawMode = DrawMode.OwnerDrawFixed;
            lstOptions.ItemHeight = 34;
            lstOptions.DrawItem += lstOptions_DrawItem;
        }

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

        private void CloseNet(string reason = "")
        {
            try { _net?.Dispose(); } catch { }
            _net = null;

            voteTimer.Stop();
            _timeExpired = true;
            lblTimer.Text = "Chưa kết nối";
            if (!string.IsNullOrWhiteSpace(reason)) Log(reason);

            UpdateUiDisconnected();
        }

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

                string clientId = txtClientId.Text.Trim();
                if (string.IsNullOrWhiteSpace(clientId))
                {
                    MessageBox.Show("Vui lòng nhập MSSV / ID");
                    return;
                }

                _net?.Dispose();
                _net = new NetworkManager();

                _net.OnConnected += () => BeginInvoke(new Action(() => Log("Đã kết nối TLS tới server.")));
                _net.OnError += err => BeginInvoke(new Action(() => Log("ERROR: " + err)));
                _net.OnDisconnected += why => BeginInvoke(new Action(() =>
                {
                    Log("Mất kết nối: " + why);
                    CloseNet();
                }));
                _net.OnMessage += msg => BeginInvoke(new Action(() => HandleServerMessage(msg)));

                _net.Connect(host, port, acceptAnyCert: true);

                _net.Send(new JsonObject
                {
                    ["type"] = "HELLO",
                    ["client_id"] = clientId
                });
                Log($">> HELLO (client_id={clientId})");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi kết nối: " + ex.Message);
                Log("Lỗi: " + ex);
                CloseNet();
            }
        }

        private void ApplySession(JsonObject msg)
        {
            _sessionId = (string?)msg["session_id"] ?? "";
            lblTopic.Text = (string?)msg["topic"] ?? "";

            lstOptions.Items.Clear();
            var opts = msg["options"] as JsonArray;
            if (opts != null)
            {
                foreach (var o in opts) lstOptions.Items.Add((string?)o ?? "");
            }

            _remainSeconds = (int?)msg["remaining"] ?? (int?)msg["limit_time"] ?? 0;
            _timeExpired = (_remainSeconds <= 0);
            lblTimer.Text = _timeExpired ? "Hết thời gian!" : $"{_remainSeconds}s";

            _hasVoted = false;
            voteTimer.Stop();
            if (!_timeExpired) voteTimer.Start();

            // init chart = 0
            if (_chart != null)
            {
                var initCounts = new Dictionary<string, int>();
                foreach (var item in lstOptions.Items)
                    initCounts[item?.ToString() ?? ""] = 0;
                _chart.Update(initCounts);
            }

            UpdateUiConnected();
        }

        private void HandleServerMessage(JsonObject msg)
        {
            var type = (string?)msg["type"] ?? "";
            Log($"<< {msg.ToJsonString()}");

            if (type == "WELCOME")
            {
                ApplySession(msg);
                return;
            }

            if (type == "NEW_ELECTION")
            {
                MessageBox.Show("Admin đã tạo cuộc bầu cử mới. Giao diện sẽ được reset.", "Thông báo");
                ApplySession(msg);
                return;
            }

            if (type == "KICKED")
            {
                MessageBox.Show((string?)msg["message"] ?? "Bạn đã bị kick", "Thông báo");
                CloseNet("Bị kick khỏi server.");
                return;
            }

            if (type == "OK")
            {
                if ((string?)msg["result"] == "VOTED")
                {
                    _hasVoted = true;
                    var opt = (string?)msg["option_text"] ?? "";
                    MessageBox.Show("Bạn đã bỏ phiếu cho: " + opt, "Thành công");
                }
                _remainSeconds = (int?)msg["remaining"] ?? _remainSeconds;
                return;
            }

            if (type == "ERR")
            {
                var code = (string?)msg["code"] ?? "UNKNOWN";
                var message = (string?)msg["message"] ?? "";

                if (code == "ALREADY_VOTED") _hasVoted = true;

                if (code == "TIME_EXPIRED")
                {
                    _timeExpired = true;
                    voteTimer.Stop();
                    lblTimer.Text = "Hết thời gian!";
                }

                MessageBox.Show($"{code}\n{message}", "Lỗi");
                return;
            }

            if (type == "RESULT" || type == "RESULT_UPDATE")
            {
                _remainSeconds = (int?)msg["remaining"] ?? _remainSeconds;
                if (_remainSeconds <= 0)
                {
                    _timeExpired = true;
                    voteTimer.Stop();
                    lblTimer.Text = "Hết thời gian!";
                }

                var data = msg["data"] as JsonObject;
                if (data != null && _chart != null)
                {
                    var counts = new Dictionary<string, int>();
                    foreach (var kv in data)
                    {
                        int v = 0;
                        if (kv.Value != null && int.TryParse(kv.Value.ToString(), out var parsed)) v = parsed;
                        counts[kv.Key] = v;
                    }
                    _chart.Update(counts);
                }
                return;
            }
        }

        private void btnVote_Click(object sender, EventArgs e)
        {
            try
            {
                if (_net == null) { MessageBox.Show("Chưa kết nối server."); return; }
                if (_timeExpired) { MessageBox.Show("Đã hết thời gian bỏ phiếu!"); return; }
                if (_hasVoted) { MessageBox.Show("Bạn đã bỏ phiếu rồi."); return; }
                if (lstOptions.SelectedIndex < 0) { MessageBox.Show("Vui lòng chọn một lựa chọn."); return; }

                int idx = lstOptions.SelectedIndex + 1;

                _net.Send(new JsonObject
                {
                    ["type"] = "VOTE",
                    ["session_id"] = _sessionId,
                    ["index"] = idx
                });

                Log($">> VOTE (index={idx})");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi bỏ phiếu: " + ex.Message);
                Log("Lỗi vote: " + ex);
            }
        }

        private void btnResult_Click(object sender, EventArgs e)
        {
            try
            {
                if (_net == null) { MessageBox.Show("Chưa kết nối server."); return; }
                _net.Send(new JsonObject { ["type"] = "RESULT" });
                Log(">> RESULT");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xem kết quả: " + ex.Message);
                Log("Lỗi result: " + ex);
            }
        }

        private void voteTimer_Tick(object? sender, EventArgs e)
        {
            if (_remainSeconds > 0)
            {
                _remainSeconds--;
                lblTimer.Text = $"{_remainSeconds}s";
            }
            else
            {
                _timeExpired = true;
                voteTimer.Stop();
                lblTimer.Text = "Hết thời gian!";
            }
        }

        private void VoteForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            CloseNet();
        }

        private void lstOptions_DrawItem(object? sender, DrawItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= lstOptions.Items.Count) return;

            string text = lstOptions.Items[e.Index]?.ToString() ?? "";
            int number = e.Index + 1;
            string displayText = $"Lựa chọn {number}:   {text}";

            bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            var backColor = isSelected ? Color.FromArgb(220, 235, 255) : Color.White;
            var foreColor = Color.Black;

            using (var backBrush = new SolidBrush(backColor))
                e.Graphics.FillRectangle(backBrush, e.Bounds);

            var textRect = new Rectangle(e.Bounds.X + 10, e.Bounds.Y + 7, e.Bounds.Width - 20, e.Bounds.Height - 14);

            using (var textBrush = new SolidBrush(foreColor))
            using (var font = new Font("Segoe UI", 10, FontStyle.Regular))
                e.Graphics.DrawString(displayText, font, textBrush, textRect);

            using (var pen = new Pen(Color.Gainsboro, 1))
                e.Graphics.DrawLine(pen, e.Bounds.Left + 6, e.Bounds.Bottom - 1, e.Bounds.Right - 6, e.Bounds.Bottom - 1);

            e.DrawFocusRectangle();
        }
    }
}
