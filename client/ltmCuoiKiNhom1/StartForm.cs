using ltmCuoiKiNhom1;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CuoiKiLTM
{
    public sealed class StartForm : Form
    {
        public StartForm()
        {
            Text = "Nhóm 1 - Lập trình mạng | Secure Voting Client";
            Width = 520;
            Height = 280;
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.White;

            var title = new Label
            {
                Text = "SecureVote",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(30, 25)
            };

            var sub = new Label
            {
                Text = "Chọn chế độ để tiếp tục",
                Font = new Font("Segoe UI", 10),
                AutoSize = true,
                Location = new Point(32, 65),
                ForeColor = Color.DimGray
            };

            var btnVote = new Button
            {
                Text = "Tham gia vote",
                Width = 200,
                Height = 44,
                Location = new Point(30, 110)
            };
            btnVote.Click += (_, __) =>
            {
                Hide();
                var f = new Form1(); // VoteForm của bạn
                f.FormClosed += (s, e) => Close();
                f.Show();
            };

            var btnAdmin = new Button
            {
                Text = "Admin Dashboard",
                Width = 200,
                Height = 44,
                Location = new Point(250, 110)
            };
            btnAdmin.Click += (_, __) =>
            {
                Hide();
                var f = new AdminForm();
                f.FormClosed += (s, e) => Close();
                f.Show();
            };

            Controls.Add(title);
            Controls.Add(sub);
            Controls.Add(btnVote);
            Controls.Add(btnAdmin);
        }
    }
}
