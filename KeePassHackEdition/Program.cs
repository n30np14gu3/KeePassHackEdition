using System;
using System.Text;
using System.Windows.Forms;
using KeePassHackEdition.SDK.Crypto;
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
            try
            {
                LicenseManager manager = new LicenseManager("test.kpdblic");
                manager.LoadLicense();
                manager.ValidateLicense();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
