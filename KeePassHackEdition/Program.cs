﻿using System;
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
            LicenseManager manager = new LicenseManager("test.kpdblic");
            try
            {
                manager.LoadLicense();
                manager.ValidateLicense();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
