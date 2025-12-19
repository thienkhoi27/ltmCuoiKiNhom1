namespace ltmCuoiKiNhom1
{
    partial class AdminForm
    {
        private System.ComponentModel.IContainer components = null;

        // Các control giữ nguyên tên để không ảnh hưởng code logic
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox txtAdminKey;
        private System.Windows.Forms.Button btnAdminConnect;

        private System.Windows.Forms.TextBox txtAdminLog;

        private System.Windows.Forms.ListBox lstClients;
        private System.Windows.Forms.Button btnRefreshClients;
        private System.Windows.Forms.Button btnKick;

        private System.Windows.Forms.TextBox txtTopic;
        private System.Windows.Forms.TextBox txtOptions;
        private System.Windows.Forms.NumericUpDown numLimit;
        private System.Windows.Forms.Button btnCreateElection;

        // Thêm các GroupBox để gom nhóm giao diện
        private System.Windows.Forms.GroupBox grpConnection;
        private System.Windows.Forms.GroupBox grpClients;
        private System.Windows.Forms.GroupBox grpLog;
        private System.Windows.Forms.GroupBox grpNewElection;
        private System.Windows.Forms.TableLayoutPanel mainLayout;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            txtServer = new TextBox();
            txtPort = new TextBox();
            txtAdminKey = new TextBox();
            btnAdminConnect = new Button();
            txtAdminLog = new TextBox();
            lstClients = new ListBox();
            btnRefreshClients = new Button();
            btnKick = new Button();
            txtTopic = new TextBox();
            txtOptions = new TextBox();
            numLimit = new NumericUpDown();
            btnCreateElection = new Button();
            grpConnection = new GroupBox();
            panelConn = new FlowLayoutPanel();
            lblServer = new Label();
            lblPort = new Label();
            lblKey = new Label();
            grpClients = new GroupBox();
            layoutClients = new TableLayoutPanel();
            panelClientBtns = new FlowLayoutPanel();
            grpLog = new GroupBox();
            grpNewElection = new GroupBox();
            layoutElection = new TableLayoutPanel();
            panelLeftElec = new Panel();
            lblTopic = new Label();
            panelRightElec = new Panel();
            lblLimit = new Label();
            mainLayout = new TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)numLimit).BeginInit();
            grpConnection.SuspendLayout();
            panelConn.SuspendLayout();
            grpClients.SuspendLayout();
            layoutClients.SuspendLayout();
            panelClientBtns.SuspendLayout();
            grpLog.SuspendLayout();
            grpNewElection.SuspendLayout();
            layoutElection.SuspendLayout();
            panelLeftElec.SuspendLayout();
            panelRightElec.SuspendLayout();
            mainLayout.SuspendLayout();
            SuspendLayout();
            // 
            // txtServer
            // 
            txtServer.Location = new Point(119, 23);
            txtServer.Name = "txtServer";
            txtServer.Size = new Size(120, 30);
            txtServer.TabIndex = 1;
            txtServer.Text = "127.0.0.1";
            // 
            // txtPort
            // 
            txtPort.Location = new Point(351, 23);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(60, 30);
            txtPort.TabIndex = 3;
            txtPort.Text = "8443";
            // 
            // txtAdminKey
            // 
            txtAdminKey.Location = new Point(523, 23);
            txtAdminKey.Name = "txtAdminKey";
            txtAdminKey.Size = new Size(120, 30);
            txtAdminKey.TabIndex = 5;
            txtAdminKey.Text = "admin";
            txtAdminKey.UseSystemPasswordChar = true;
            // 
            // btnAdminConnect
            // 
            btnAdminConnect.BackColor = Color.Teal;
            btnAdminConnect.Cursor = Cursors.Hand;
            btnAdminConnect.FlatStyle = FlatStyle.Flat;
            btnAdminConnect.ForeColor = Color.White;
            btnAdminConnect.Location = new Point(666, 23);
            btnAdminConnect.Margin = new Padding(20, 3, 0, 0);
            btnAdminConnect.Name = "btnAdminConnect";
            btnAdminConnect.Size = new Size(100, 30);
            btnAdminConnect.TabIndex = 6;
            btnAdminConnect.Text = "KẾT NỐI";
            btnAdminConnect.UseVisualStyleBackColor = false;
            btnAdminConnect.Click += btnAdminConnect_Click;
            // 
            // txtAdminLog
            // 
            txtAdminLog.BackColor = Color.Black;
            txtAdminLog.BorderStyle = BorderStyle.FixedSingle;
            txtAdminLog.Dock = DockStyle.Fill;
            txtAdminLog.Font = new Font("Consolas", 9.5F);
            txtAdminLog.ForeColor = Color.LimeGreen;
            txtAdminLog.Location = new Point(3, 26);
            txtAdminLog.Multiline = true;
            txtAdminLog.Name = "txtAdminLog";
            txtAdminLog.ReadOnly = true;
            txtAdminLog.ScrollBars = ScrollBars.Vertical;
            txtAdminLog.Size = new Size(625, 235);
            txtAdminLog.TabIndex = 0;
            // 
            // lstClients
            // 
            lstClients.BackColor = Color.White;
            lstClients.BorderStyle = BorderStyle.FixedSingle;
            lstClients.Dock = DockStyle.Fill;
            lstClients.Font = new Font("Segoe UI", 10F);
            lstClients.ItemHeight = 23;
            lstClients.Location = new Point(8, 8);
            lstClients.Name = "lstClients";
            lstClients.Size = new Size(315, 489);
            lstClients.TabIndex = 0;
            // 
            // btnRefreshClients
            // 
            btnRefreshClients.BackColor = Color.SteelBlue;
            btnRefreshClients.FlatStyle = FlatStyle.Flat;
            btnRefreshClients.ForeColor = Color.White;
            btnRefreshClients.Location = new Point(3, 3);
            btnRefreshClients.Name = "btnRefreshClients";
            btnRefreshClients.Size = new Size(100, 35);
            btnRefreshClients.TabIndex = 0;
            btnRefreshClients.Text = "🔄 Refresh";
            btnRefreshClients.UseVisualStyleBackColor = false;
            btnRefreshClients.Click += btnRefreshClients_Click;
            // 
            // btnKick
            // 
            btnKick.BackColor = Color.IndianRed;
            btnKick.FlatStyle = FlatStyle.Flat;
            btnKick.ForeColor = Color.White;
            btnKick.Location = new Point(116, 3);
            btnKick.Margin = new Padding(10, 3, 0, 0);
            btnKick.Name = "btnKick";
            btnKick.Size = new Size(80, 35);
            btnKick.TabIndex = 1;
            btnKick.Text = "🚫 Kick";
            btnKick.UseVisualStyleBackColor = false;
            btnKick.Click += btnKick_Click;
            // 
            // txtTopic
            // 
            txtTopic.Dock = DockStyle.Top;
            txtTopic.Font = new Font("Segoe UI", 10F);
            txtTopic.Location = new Point(0, 0);
            txtTopic.Name = "txtTopic";
            txtTopic.Size = new Size(417, 30);
            txtTopic.TabIndex = 2;
            // 
            // txtOptions
            // 
            txtOptions.Dock = DockStyle.Fill;
            txtOptions.Font = new Font("Segoe UI", 10F);
            txtOptions.Location = new Point(0, 30);
            txtOptions.Multiline = true;
            txtOptions.Name = "txtOptions";
            txtOptions.ScrollBars = ScrollBars.Vertical;
            txtOptions.Size = new Size(417, 229);
            txtOptions.TabIndex = 0;
            txtOptions.Text = "A\r\nB\r\nC";
            // 
            // numLimit
            // 
            numLimit.Dock = DockStyle.Top;
            numLimit.Font = new Font("Segoe UI", 10F);
            numLimit.Location = new Point(10, 0);
            numLimit.Maximum = new decimal(new int[] { 3600, 0, 0, 0 });
            numLimit.Minimum = new decimal(new int[] { 5, 0, 0, 0 });
            numLimit.Name = "numLimit";
            numLimit.Size = new Size(166, 30);
            numLimit.TabIndex = 1;
            numLimit.Value = new decimal(new int[] { 60, 0, 0, 0 });
            // 
            // btnCreateElection
            // 
            btnCreateElection.BackColor = Color.DarkOrange;
            btnCreateElection.Cursor = Cursors.Hand;
            btnCreateElection.Dock = DockStyle.Bottom;
            btnCreateElection.FlatStyle = FlatStyle.Flat;
            btnCreateElection.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnCreateElection.ForeColor = Color.White;
            btnCreateElection.Location = new Point(10, 209);
            btnCreateElection.Name = "btnCreateElection";
            btnCreateElection.Size = new Size(166, 50);
            btnCreateElection.TabIndex = 0;
            btnCreateElection.Text = "📢 BẮT ĐẦU BẦU CỬ";
            btnCreateElection.UseVisualStyleBackColor = false;
            btnCreateElection.Click += btnCreateElection_Click;
            // 
            // grpConnection
            // 
            mainLayout.SetColumnSpan(grpConnection, 2);
            grpConnection.Controls.Add(panelConn);
            grpConnection.Dock = DockStyle.Fill;
            grpConnection.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            grpConnection.Location = new Point(13, 13);
            grpConnection.Name = "grpConnection";
            grpConnection.Size = new Size(974, 84);
            grpConnection.TabIndex = 0;
            grpConnection.TabStop = false;
            grpConnection.Text = "Cấu hình Kết nối Admin";
            // 
            // panelConn
            // 
            panelConn.Controls.Add(lblServer);
            panelConn.Controls.Add(txtServer);
            panelConn.Controls.Add(lblPort);
            panelConn.Controls.Add(txtPort);
            panelConn.Controls.Add(lblKey);
            panelConn.Controls.Add(txtAdminKey);
            panelConn.Controls.Add(btnAdminConnect);
            panelConn.Dock = DockStyle.Fill;
            panelConn.Location = new Point(3, 26);
            panelConn.Name = "panelConn";
            panelConn.Padding = new Padding(10, 20, 0, 0);
            panelConn.Size = new Size(968, 55);
            panelConn.TabIndex = 0;
            // 
            // lblServer
            // 
            lblServer.Location = new Point(13, 20);
            lblServer.Name = "lblServer";
            lblServer.Size = new Size(100, 23);
            lblServer.TabIndex = 0;
            lblServer.Text = "IP:";
            // 
            // lblPort
            // 
            lblPort.Location = new Point(245, 20);
            lblPort.Name = "lblPort";
            lblPort.Size = new Size(100, 23);
            lblPort.TabIndex = 2;
            lblPort.Text = "Port:";
            // 
            // lblKey
            // 
            lblKey.Location = new Point(417, 20);
            lblKey.Name = "lblKey";
            lblKey.Size = new Size(100, 23);
            lblKey.TabIndex = 4;
            lblKey.Text = "Key:";
            // 
            // grpClients
            // 
            grpClients.Controls.Add(layoutClients);
            grpClients.Dock = DockStyle.Fill;
            grpClients.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            grpClients.Location = new Point(13, 103);
            grpClients.Name = "grpClients";
            mainLayout.SetRowSpan(grpClients, 2);
            grpClients.Size = new Size(337, 584);
            grpClients.TabIndex = 1;
            grpClients.TabStop = false;
            grpClients.Text = "Danh sách Client Online";
            // 
            // layoutClients
            // 
            layoutClients.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            layoutClients.Controls.Add(lstClients, 0, 0);
            layoutClients.Controls.Add(panelClientBtns, 0, 1);
            layoutClients.Dock = DockStyle.Fill;
            layoutClients.Location = new Point(3, 26);
            layoutClients.Name = "layoutClients";
            layoutClients.Padding = new Padding(5);
            layoutClients.RowCount = 2;
            layoutClients.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            layoutClients.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            layoutClients.Size = new Size(331, 555);
            layoutClients.TabIndex = 0;
            // 
            // panelClientBtns
            // 
            panelClientBtns.Controls.Add(btnRefreshClients);
            panelClientBtns.Controls.Add(btnKick);
            panelClientBtns.Dock = DockStyle.Fill;
            panelClientBtns.Location = new Point(8, 503);
            panelClientBtns.Name = "panelClientBtns";
            panelClientBtns.Size = new Size(315, 44);
            panelClientBtns.TabIndex = 1;
            // 
            // grpLog
            // 
            grpLog.Controls.Add(txtAdminLog);
            grpLog.Dock = DockStyle.Fill;
            grpLog.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            grpLog.Location = new Point(356, 103);
            grpLog.Name = "grpLog";
            grpLog.Size = new Size(631, 264);
            grpLog.TabIndex = 2;
            grpLog.TabStop = false;
            grpLog.Text = "Nhật ký Hoạt động (System Log)";
            // 
            // grpNewElection
            // 
            grpNewElection.BackColor = Color.White;
            grpNewElection.Controls.Add(layoutElection);
            grpNewElection.Dock = DockStyle.Fill;
            grpNewElection.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            grpNewElection.Location = new Point(356, 373);
            grpNewElection.Name = "grpNewElection";
            grpNewElection.Size = new Size(631, 314);
            grpNewElection.TabIndex = 3;
            grpNewElection.TabStop = false;
            grpNewElection.Text = "Tạo Cuộc Bầu Cử Mới (Broadcast)";
            // 
            // layoutElection
            // 
            layoutElection.ColumnCount = 2;
            layoutElection.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            layoutElection.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            layoutElection.Controls.Add(panelLeftElec, 0, 0);
            layoutElection.Controls.Add(panelRightElec, 1, 0);
            layoutElection.Dock = DockStyle.Fill;
            layoutElection.Location = new Point(3, 26);
            layoutElection.Name = "layoutElection";
            layoutElection.Padding = new Padding(10);
            layoutElection.RowCount = 3;
            layoutElection.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            layoutElection.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            layoutElection.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            layoutElection.Size = new Size(625, 285);
            layoutElection.TabIndex = 0;
            // 
            // panelLeftElec
            // 
            panelLeftElec.Controls.Add(txtOptions);
            panelLeftElec.Controls.Add(txtTopic);
            panelLeftElec.Controls.Add(lblTopic);
            panelLeftElec.Dock = DockStyle.Fill;
            panelLeftElec.Location = new Point(13, 13);
            panelLeftElec.Name = "panelLeftElec";
            layoutElection.SetRowSpan(panelLeftElec, 3);
            panelLeftElec.Size = new Size(417, 259);
            panelLeftElec.TabIndex = 0;
            // 
            // lblTopic
            // 
            lblTopic.Location = new Point(0, 0);
            lblTopic.Name = "lblTopic";
            lblTopic.Size = new Size(100, 23);
            lblTopic.TabIndex = 3;
            // 
            // panelRightElec
            // 
            panelRightElec.Controls.Add(btnCreateElection);
            panelRightElec.Controls.Add(numLimit);
            panelRightElec.Controls.Add(lblLimit);
            panelRightElec.Dock = DockStyle.Fill;
            panelRightElec.Location = new Point(436, 13);
            panelRightElec.Name = "panelRightElec";
            panelRightElec.Padding = new Padding(10, 0, 0, 0);
            layoutElection.SetRowSpan(panelRightElec, 3);
            panelRightElec.Size = new Size(176, 259);
            panelRightElec.TabIndex = 1;
            // 
            // lblLimit
            // 
            lblLimit.Location = new Point(0, 0);
            lblLimit.Name = "lblLimit";
            lblLimit.Size = new Size(100, 23);
            lblLimit.TabIndex = 2;
            // 
            // mainLayout
            // 
            mainLayout.ColumnCount = 2;
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
            mainLayout.Controls.Add(grpConnection, 0, 0);
            mainLayout.Controls.Add(grpClients, 0, 1);
            mainLayout.Controls.Add(grpLog, 1, 1);
            mainLayout.Controls.Add(grpNewElection, 1, 2);
            mainLayout.Dock = DockStyle.Fill;
            mainLayout.Location = new Point(0, 0);
            mainLayout.Name = "mainLayout";
            mainLayout.Padding = new Padding(10);
            mainLayout.RowCount = 3;
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 90F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 320F));
            mainLayout.Size = new Size(1000, 700);
            mainLayout.TabIndex = 0;
            // 
            // AdminForm
            // 
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(1000, 700);
            Controls.Add(mainLayout);
            Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Name = "AdminForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Admin Dashboard - Hệ Thống Bỏ Phiếu An Toàn";
            ((System.ComponentModel.ISupportInitialize)numLimit).EndInit();
            grpConnection.ResumeLayout(false);
            panelConn.ResumeLayout(false);
            panelConn.PerformLayout();
            grpClients.ResumeLayout(false);
            layoutClients.ResumeLayout(false);
            panelClientBtns.ResumeLayout(false);
            grpLog.ResumeLayout(false);
            grpLog.PerformLayout();
            grpNewElection.ResumeLayout(false);
            layoutElection.ResumeLayout(false);
            panelLeftElec.ResumeLayout(false);
            panelLeftElec.PerformLayout();
            panelRightElec.ResumeLayout(false);
            mainLayout.ResumeLayout(false);
            ResumeLayout(false);
        }

        private TableLayoutPanel layoutClients;
        private FlowLayoutPanel panelClientBtns;
        private TableLayoutPanel layoutElection;
        private Panel panelLeftElec;
        private Label lblTopic;
        private Panel panelRightElec;
        private Label lblLimit;
        private FlowLayoutPanel panelConn;
        private Label lblServer;
        private Label lblPort;
        private Label lblKey;
    }
}