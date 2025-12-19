using System;
using System.Windows.Forms;

namespace ltmCuoiKiNhom1
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}
