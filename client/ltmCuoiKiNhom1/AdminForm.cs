using System;
using System.Linq;
using System.Text.Json.Nodes;
using System.Windows.Forms;

namespace ltmCuoiKiNhom1
{
    public partial class AdminForm : Form
    {
        private NetworkManager? _net;

        public AdminForm()
        {
            InitializeComponent();
            Text = "Admin Dashboard - Nhóm 1";
            UpdateUi(false);
        }

        private void Log(string msg)
        {
            txtAdminLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {msg}\r\n");
        }

        private void UpdateUi(bool connected)
        {
            btnAdminConnect.Enabled = !connected;
            btnRefreshClients.Enabled = connected;
            btnKick.Enabled = connected;
            btnCreateElection.Enabled = connected;
        }

        private void AdminForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try { _net?.Dispose(); } catch { }
        }

        private void btnAdminConnect_Click(object sender, EventArgs e)
        {
            try
            {
                string host = txtServer.Text.Trim();
                if (!int.TryParse(txtPort.Text.Trim(), out int port)) { MessageBox.Show("Port không hợp lệ"); return; }

                string key = txtAdminKey.Text.Trim();
                if (string.IsNullOrWhiteSpace(key)) { MessageBox.Show("Nhập admin_key"); return; }

                _net?.Dispose();
                _net = new NetworkManager();

                _net.OnConnected += () => BeginInvoke(new Action(() => Log("Admin đã kết nối TLS.")));
                _net.OnDisconnected += why => BeginInvoke(new Action(() =>
                {
                    Log("Mất kết nối: " + why);
                    UpdateUi(false);
                }));
                _net.OnMessage += msg => BeginInvoke(new Action(() => HandleMsg(msg)));

                _net.Connect(host, port, acceptAnyCert: true);

                _net.Send(new JsonObject
                {
                    ["type"] = "ADMIN_HELLO",
                    ["admin_key"] = key
                });

                Log(">> ADMIN_HELLO");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log("ERROR: " + ex);
                UpdateUi(false);
            }
        }

        private void HandleMsg(JsonObject msg)
        {
            var type = (string?)msg["type"] ?? "";
            Log("<< " + msg.ToJsonString());

            if (type == "ADMIN_WELCOME")
            {
                UpdateUi(true);
                LoadClientList(msg["clients"] as JsonArray);
                return;
            }

            if (type == "LOG")
            {
                Log((string?)msg["message"] ?? "");
                return;
            }

            if (type == "CLIENT_LIST")
            {
                LoadClientList(msg["clients"] as JsonArray);
                return;
            }

            if (type == "ERR")
            {
                MessageBox.Show($"{(string?)msg["code"]}\n{(string?)msg["message"]}", "Admin Error");
                return;
            }
        }

        private void LoadClientList(JsonArray? arr)
        {
            lstClients.Items.Clear();
            if (arr == null) return;

            foreach (var x in arr)
            {
                var o = x as JsonObject;
                if (o == null) continue;
                string id = (string?)o["client_id"] ?? "";
                string addr = (string?)o["addr"] ?? "";
                lstClients.Items.Add($"{id} ({addr})");
            }
        }

        private void btnRefreshClients_Click(object sender, EventArgs e)
        {
            _net?.Send(new JsonObject { ["type"] = "ADMIN_LIST" });
        }

        private void btnKick_Click(object sender, EventArgs e)
        {
            if (_net == null) return;
            if (lstClients.SelectedIndex < 0) { MessageBox.Show("Chọn client để kick"); return; }

            string line = lstClients.SelectedItem.ToString() ?? "";
            string id = line.Split(' ').FirstOrDefault() ?? "";
            if (string.IsNullOrWhiteSpace(id)) return;

            _net.Send(new JsonObject
            {
                ["type"] = "ADMIN_KICK",
                ["client_id"] = id
            });
        }

        private void btnCreateElection_Click(object sender, EventArgs e)
        {
            if (_net == null) return;

            var topic = txtTopic.Text.Trim();
            var lines = txtOptions.Lines.Select(x => x.Trim()).Where(x => x.Length > 0).ToArray();
            int limit = (int)numLimit.Value;

            if (string.IsNullOrWhiteSpace(topic)) { MessageBox.Show("Nhập topic"); return; }
            if (lines.Length < 1) { MessageBox.Show("Nhập ít nhất 1 lựa chọn"); return; }

            var arr = new JsonArray();
            foreach (var s in lines) arr.Add(s);

            _net.Send(new JsonObject
            {
                ["type"] = "ADMIN_NEW_ELECTION",
                ["topic"] = topic,
                ["options"] = arr,
                ["limit_time"] = limit
            });
        }
    }
}
