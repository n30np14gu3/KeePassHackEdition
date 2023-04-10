using System;
using System.Windows.Forms;
using KeePassHackEdition.SDK.License;

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
#if DEBUG
            LicenseManager manager = new LicenseManager("license.kpdblic");
            manager.GenerateValidLicense();
#endif
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
