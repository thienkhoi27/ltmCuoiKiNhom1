namespace ltmCuoiKiNhom1
{
    partial class VoteForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Timer voteTimer;
        // Các control cũ giữ nguyên, thêm GroupBox để gom nhóm cho đẹp
        private System.Windows.Forms.GroupBox grpConnection;
        private System.Windows.Forms.GroupBox grpVoting;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox txtClientId;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnVote;
        private System.Windows.Forms.Button btnResult;
        private System.Windows.Forms.Label lblTopic;
        private System.Windows.Forms.Label lblTimer;
        private System.Windows.Forms.ListBox lstOptions;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Panel panelChart;
        private System.Windows.Forms.Label lblTitleLog;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            // --- CÀI ĐẶT MÀU SẮC CHỦ ĐẠO ---
            System.Drawing.Color primaryColor = System.Drawing.Color.FromArgb(0, 122, 204); // Xanh Visual Studio
            System.Drawing.Color successColor = System.Drawing.Color.FromArgb(40, 167, 69); // Xanh lá
            System.Drawing.Color warningColor = System.Drawing.Color.FromArgb(255, 193, 7); // Vàng cam
            System.Drawing.Color bgColor = System.Drawing.Color.FromArgb(245, 247, 250); // Xám nhạt hiện đại
            System.Drawing.Font mainFont = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular);
            System.Drawing.Font boldFont = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);

            this.voteTimer = new System.Windows.Forms.Timer(this.components);

            // Khởi tạo các GroupBox để gom nhóm giao diện
            this.grpConnection = new System.Windows.Forms.GroupBox();
            this.grpVoting = new System.Windows.Forms.GroupBox();

            this.txtServer = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtClientId = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnVote = new System.Windows.Forms.Button();
            this.btnResult = new System.Windows.Forms.Button();
            this.lblTopic = new System.Windows.Forms.Label();
            this.lblTimer = new System.Windows.Forms.Label();
            this.lstOptions = new System.Windows.Forms.ListBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.panelChart = new System.Windows.Forms.Panel();
            this.lblTitleLog = new System.Windows.Forms.Label();

            // --- MAIN LAYOUT ---
            var root = new System.Windows.Forms.TableLayoutPanel();
            root.Dock = System.Windows.Forms.DockStyle.Fill;
            root.Padding = new System.Windows.Forms.Padding(15);
            root.ColumnCount = 2;
            root.RowCount = 4; // Tăng thêm 1 row nhỏ cho tiêu đề Log
            root.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F)); // Cột trái nhỏ hơn chút
            root.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));

            // Row 0: Connection (Top)
            root.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            // Row 1: Main Content (Vote + Chart)
            root.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            // Row 2: Title Log (Nhỏ)
            root.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            // Row 3: Log content
            root.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));

            // --- TOP SECTION (CONNECTION) ---
            this.grpConnection.Text = "Cấu hình kết nối";
            this.grpConnection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpConnection.Font = boldFont;
            this.grpConnection.ForeColor = primaryColor; // Màu chữ tiêu đề group

            var topLayout = new System.Windows.Forms.TableLayoutPanel();
            topLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            topLayout.ColumnCount = 7;
            topLayout.RowCount = 1;
            // Chia tỉ lệ cột cho đẹp
            topLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize)); // Label IP
            topLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F)); // Text IP
            topLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize)); // Label Port
            topLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F)); // Text Port
            topLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize)); // Label ID
            topLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F)); // Text ID
            topLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F)); // Button Connect

            var lb1 = new System.Windows.Forms.Label { Text = "IP Server:", Dock = System.Windows.Forms.DockStyle.Fill, TextAlign = System.Drawing.ContentAlignment.MiddleRight, Font = mainFont, ForeColor = System.Drawing.Color.Black };
            var lb2 = new System.Windows.Forms.Label { Text = "Port:", Dock = System.Windows.Forms.DockStyle.Fill, TextAlign = System.Drawing.ContentAlignment.MiddleRight, Font = mainFont, ForeColor = System.Drawing.Color.Black };
            var lb3 = new System.Windows.Forms.Label { Text = "MSSV/Tên:", Dock = System.Windows.Forms.DockStyle.Fill, TextAlign = System.Drawing.ContentAlignment.MiddleRight, Font = mainFont, ForeColor = System.Drawing.Color.Black };

            // TextBoxes Styling
            this.txtServer.Dock = System.Windows.Forms.DockStyle.Fill; this.txtServer.Text = "127.0.0.1"; this.txtServer.Font = mainFont; this.txtServer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPort.Dock = System.Windows.Forms.DockStyle.Fill; this.txtPort.Text = "8443"; this.txtPort.Font = mainFont; this.txtPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtClientId.Dock = System.Windows.Forms.DockStyle.Fill; this.txtClientId.Font = mainFont; this.txtClientId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            // Button Connect Styling
            this.btnConnect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnConnect.Text = "KẾT NỐI";
            this.btnConnect.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnConnect.BackColor = primaryColor;
            this.btnConnect.ForeColor = System.Drawing.Color.White;
            this.btnConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConnect.FlatAppearance.BorderSize = 0;
            this.btnConnect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);

            topLayout.Controls.Add(lb1, 0, 0); topLayout.Controls.Add(this.txtServer, 1, 0);
            topLayout.Controls.Add(lb2, 2, 0); topLayout.Controls.Add(this.txtPort, 3, 0);
            topLayout.Controls.Add(lb3, 4, 0); topLayout.Controls.Add(this.txtClientId, 5, 0);
            topLayout.Controls.Add(this.btnConnect, 6, 0);

            // Add padding to inputs inside layout
            topLayout.Padding = new System.Windows.Forms.Padding(5, 15, 5, 5);
            this.grpConnection.Controls.Add(topLayout);

            // --- MIDDLE LEFT SECTION (VOTING) ---
            var leftPanel = new System.Windows.Forms.TableLayoutPanel();
            leftPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            leftPanel.RowCount = 4;
            leftPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F)); // Topic Header
            leftPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F)); // Timer
            leftPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F)); // ListBox
            leftPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F)); // Buttons

            this.lblTopic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTopic.Text = "Chủ đề: Đang chờ máy chủ...";
            this.lblTopic.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblTopic.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.lblTopic.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.lblTimer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTimer.Text = "--:--";
            this.lblTimer.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Italic);
            this.lblTimer.ForeColor = System.Drawing.Color.Red;
            this.lblTimer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.lstOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstOptions.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lstOptions.ItemHeight = 30; // Giãn dòng cho dễ đọc
            this.lstOptions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstOptions.BackColor = System.Drawing.Color.White;

            // Buttons Vote & Result
            var btnRow = new System.Windows.Forms.TableLayoutPanel();
            btnRow.Dock = System.Windows.Forms.DockStyle.Fill;
            btnRow.ColumnCount = 2;
            btnRow.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            btnRow.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            btnRow.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);

            this.btnVote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnVote.Text = "✅ GỬI BỎ PHIẾU";
            this.btnVote.Font = boldFont;
            this.btnVote.BackColor = successColor;
            this.btnVote.ForeColor = System.Drawing.Color.White;
            this.btnVote.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVote.FlatAppearance.BorderSize = 0;
            this.btnVote.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnVote.Click += new System.EventHandler(this.btnVote_Click);

            this.btnResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnResult.Text = "📊 KẾT QUẢ";
            this.btnResult.Font = boldFont;
            this.btnResult.BackColor = warningColor;
            this.btnResult.ForeColor = System.Drawing.Color.Black;
            this.btnResult.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResult.FlatAppearance.BorderSize = 0;
            this.btnResult.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.btnResult.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnResult.Click += new System.EventHandler(this.btnResult_Click);

            btnRow.Controls.Add(this.btnVote, 0, 0);
            btnRow.Controls.Add(this.btnResult, 1, 0);

            leftPanel.Controls.Add(this.lblTopic, 0, 0);
            leftPanel.Controls.Add(this.lblTimer, 0, 1);
            leftPanel.Controls.Add(this.lstOptions, 0, 2);
            leftPanel.Controls.Add(btnRow, 0, 3);

            // Bọc phần Vote trong GroupBox
            this.grpVoting.Controls.Add(leftPanel);
            this.grpVoting.Text = "Khu vực bỏ phiếu";
            this.grpVoting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpVoting.Font = mainFont;

            // --- MIDDLE RIGHT SECTION (CHART) ---
            this.panelChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelChart.BackColor = System.Drawing.Color.White;
            this.panelChart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle; // Viền mỏng
            // Bạn nên thêm label "Biểu đồ trực quan" ở đây nếu muốn

            // --- BOTTOM SECTION (LOG) ---
            this.lblTitleLog.Text = "> System Logs / Terminal Output:";
            this.lblTitleLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblTitleLog.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold);
            this.lblTitleLog.ForeColor = System.Drawing.Color.DimGray;

            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Multiline = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.ReadOnly = true;
            this.txtLog.BackColor = System.Drawing.Color.FromArgb(30, 30, 30); // Nền tối
            this.txtLog.ForeColor = System.Drawing.Color.FromArgb(0, 255, 0); // Chữ xanh hacker
            this.txtLog.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            // --- ADD TO ROOT ---
            // Row 0: Connection spans both columns
            root.Controls.Add(this.grpConnection, 0, 0);
            root.SetColumnSpan(this.grpConnection, 2);

            // Row 1: Left (Vote) + Right (Chart)
            root.Controls.Add(this.grpVoting, 0, 1);
            root.Controls.Add(this.panelChart, 1, 1);

            // Row 2: Title Log
            root.Controls.Add(this.lblTitleLog, 0, 2);
            root.SetColumnSpan(this.lblTitleLog, 2);

            // Row 3: Log content
            root.Controls.Add(this.txtLog, 0, 3);
            root.SetColumnSpan(this.txtLog, 2);

            // --- FORM SETTINGS ---
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 750);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hệ thống Bỏ phiếu Trực tuyến - Nhóm 1";
            this.BackColor = bgColor; // Màu nền form
            this.Controls.Add(root);

            this.Load += new System.EventHandler(this.VoteForm_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.VoteForm_FormClosed);
        }
    }
}