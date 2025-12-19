namespace ltmCuoiKiNhom1
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TableLayoutPanel mainLayout;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Button btnJoin;
        private System.Windows.Forms.Button btnAdmin;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.mainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.btnJoin = new System.Windows.Forms.Button();
            this.btnAdmin = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // 
            // Form1 Settings
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 500);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cổng thông tin - Hệ thống Bỏ phiếu An toàn";
            this.BackColor = System.Drawing.Color.WhiteSmoke; // Màu nền dịu nhẹ

            // 
            // mainLayout (TableLayoutPanel)
            // 
            this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayout.ColumnCount = 3;
            // Chia cột: 20% lề trái - 60% nội dung chính - 20% lề phải (giúp nút bấm nằm giữa)
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));

            this.mainLayout.RowCount = 5;
            // Chia dòng: Tiêu đề - Phụ đề - Nút Vote (To) - Nút Admin (Nhỏ) - Đệm dưới
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F)); // Title
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));  // Subtitle
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F)); // Nút Vote
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F)); // Nút Admin
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F)); // Bottom Padding

            // 
            // lblTitle
            // 
            this.lblTitle.Text = "HỆ THỐNG BỎ PHIẾU AN TOÀN";
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(40, 40, 40); // Màu xám đen chuyên nghiệp

            // 
            // lblSubtitle
            // 
            this.lblSubtitle.Text = "Nhóm 1 - Lập trình mạng";
            this.lblSubtitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSubtitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic);
            this.lblSubtitle.ForeColor = System.Drawing.Color.Gray;

            // 
            // btnJoin (Nút Chính)
            // 
            this.btnJoin.Text = "🗳️ THAM GIA BỎ PHIẾU\n(Dành cho Client)";
            this.btnJoin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnJoin.Margin = new System.Windows.Forms.Padding(10, 20, 10, 20); // Tạo khoảng cách cho nút
            this.btnJoin.BackColor = System.Drawing.Color.DodgerBlue; // Màu xanh nổi bật
            this.btnJoin.ForeColor = System.Drawing.Color.White;
            this.btnJoin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnJoin.FlatAppearance.BorderSize = 0;
            this.btnJoin.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.btnJoin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnJoin.Click += new System.EventHandler(this.btnJoin_Click);

            // 
            // btnAdmin (Nút Phụ)
            // 
            this.btnAdmin.Text = "🛠️ QUẢN TRỊ VIÊN\n(Dành cho Admin)";
            this.btnAdmin.Dock = System.Windows.Forms.DockStyle.Fill;
            // Margin lớn hơn để nút Admin nhỏ hơn nút Join một chút
            this.btnAdmin.Margin = new System.Windows.Forms.Padding(50, 5, 50, 30);
            this.btnAdmin.BackColor = System.Drawing.Color.DimGray;
            this.btnAdmin.ForeColor = System.Drawing.Color.White;
            this.btnAdmin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdmin.FlatAppearance.BorderSize = 0;
            this.btnAdmin.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnAdmin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdmin.Click += new System.EventHandler(this.btnAdmin_Click);

            // 
            // Add Controls to Layout
            // 
            // Thêm vào cột thứ 2 (Index 1) để canh giữa
            this.mainLayout.Controls.Add(this.lblTitle, 1, 0);
            this.mainLayout.Controls.Add(this.lblSubtitle, 1, 1);
            this.mainLayout.Controls.Add(this.btnJoin, 1, 2);
            this.mainLayout.Controls.Add(this.btnAdmin, 1, 3);

            this.Controls.Add(this.mainLayout);
            this.ResumeLayout(false);
        }
    }
}