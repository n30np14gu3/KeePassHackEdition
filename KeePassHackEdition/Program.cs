using System;
using System.Text;
using System.Windows.Forms;
using KeePassHackEdition.SDK.License;
using KeePassHackEdition.SDK.PassDb;

namespace KeePassHackEdition
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
