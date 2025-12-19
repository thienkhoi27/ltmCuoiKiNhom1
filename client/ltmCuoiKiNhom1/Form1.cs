using CuoiKiLTM;
using System;
using System.Windows.Forms;

namespace ltmCuoiKiNhom1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Text = "Nhóm 1 - Lập trình mạng";
        }

        private void btnJoin_Click(object sender, EventArgs e)
        {
            using var f = new VoteForm();
            Hide();
            f.ShowDialog();
            Show();
        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            using var f = new AdminForm();
            Hide();
            f.ShowDialog();
            Show();
        }
    }
}
