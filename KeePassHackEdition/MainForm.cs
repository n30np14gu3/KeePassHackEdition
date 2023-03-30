using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Xml;
using KeePassHackEdition.SDK;
using KeePassHackEdition.SDK.License;
using KeePassHackEdition.SDK.PassDb;

namespace KeePassHackEdition
{
    public partial class MainForm : Form
    {
        private Dictionary<int, KeyValuePair<string, DatabaseEntryType>> _openData = null;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (!File.Exists("config.xml"))
                return;

            try
            {
                using (StreamReader sr = new StreamReader("config.xml"))
                {
                    string xml = sr.ReadToEnd();
                    using (StringReader ssr = new StringReader(xml))
                    {
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(LicenseConfig));
                        Config.LicenseInfo = xmlSerializer.Deserialize(ssr) as LicenseConfig;
                        if (Config.LicenseInfo == null)
                            return;

                        if(string.IsNullOrWhiteSpace(Config.LicenseInfo.LicensePath))
                            return;

                        if (!File.Exists(Config.LicenseInfo.LicensePath))
                        {
                            Config.LicenseInfo.LicensePath = null;
                            throw new Exception("Can't load license file");
                        }

                        InitLicenseManager();

                    }
                }


                ActivateLicense();

                if (Config.Activated)
                {
                    bCreateDatabase.Enabled = true;
                    bOpenDatabase.Enabled = true;
                    bSaveDatabase.Enabled = true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"LICENSE ACTIVATION ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (Config.LicenseInfo != null)
                {
                    Config.LicenseInfo.ActivationResponse = null;
                    Config.LicenseInfo.LicensePath = null;
                }
            }

        }

        private void bActivateButton_Click(object sender, EventArgs e)
        {
            if (Config.Activated)
                return;

            if (Config.LicenseInfo == null)
                Config.LicenseInfo = new LicenseConfig();

            if (string.IsNullOrWhiteSpace(Config.LicenseInfo.LicensePath))
            {
                openLicenseDialog.FileName = "";
                if (openLicenseDialog.ShowDialog() != DialogResult.OK)
                    return;

                Config.LicenseInfo.LicensePath = openLicenseDialog.FileName;
            }

            try
            {
                InitLicenseManager();
                ActivateLicense();
                Config.Activated = true;
                bCreateDatabase.Enabled = true;
                bOpenDatabase.Enabled = true;
                bSaveDatabase.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"LICENSE ACTIVATION ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Config.LicenseInfo.ActivationResponse = null;
                Config.LicenseInfo.LicensePath = null;
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Config.LicenseInfo == null)
                return;

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(LicenseConfig));
            using (StringWriter sw = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sw))
                {
                    xmlSerializer.Serialize(writer, Config.LicenseInfo);
                    using (StreamWriter ssw = new StreamWriter("config.xml"))
                    {
                        ssw.Write(sw.ToString());
                    }
                }
            }
        }

        private void InitLicenseManager()
        {
            Config.LicManager = new LicenseManager(Config.LicenseInfo.LicensePath);
            Config.LicManager.LoadLicense();
            Config.LicManager.ValidateLicense();
            MessageBox.Show(
                $@"Thank you for buying our software! Here's your discount coupon for other products that we produce: {Config.LicManager.GetLicensePromocode()}", 
                @"INFO", 
                MessageBoxButtons.OK, 
                MessageBoxIcon.Information);
        }

        private void ActivateLicense()
        {
            if (string.IsNullOrWhiteSpace(Config.LicenseInfo.ActivationResponse))
            {
                if (new ActivationForm().ShowDialog() != DialogResult.OK)
                    return;
            }

            Config.LicManager.ValidateActivationResponse(Config.LicenseInfo.ActivationResponse);
            Config.Activated = true;
        }

        private void bSaveDatabase_Click(object sender, EventArgs e)
        {
            if (Config.Database == null)
                return;

            Config.Database.Save();
            MessageBox.Show(@"Saved", @"OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void UpdateList()
        {
            passList.Items.Clear();
            _openData?.Clear();
            _openData = Config.Database.GetOpenData();
            foreach (var item in _openData)
            {
                string[] row = { (item.Key + 1).ToString(), item.Value.Key, item.Value.Value == DatabaseEntryType.EntryTypePassword ? "*****" : "[FILE]" };
                passList.Items.Add(new ListViewItem(row));
            }
        }

        private void bOpenDatabase_Click(object sender, EventArgs e)
        {
            if (Config.Database != null)
                return;
            try
            {
                openDbDialog.FileName = "";
                if (openDbDialog.ShowDialog() != DialogResult.OK)
                    return;

                PasswordForm passForm = new PasswordForm();
                if (passForm.ShowDialog() != DialogResult.OK)
                    return;

                Config.Database = new PassDb(openDbDialog.FileName, passForm.Password);
                Config.Database.LoadDb();
                _openData = Config.Database.GetOpenData();
                UpdateList();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"OPEN DATABASE ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Config.Database = null;
            }

        }

        private void bCreateDatabase_Click(object sender, EventArgs e)
        {
            try
            {
                saveDbDialog.FileName = "";
                if (saveDbDialog.ShowDialog() != DialogResult.OK)
                    return;

                PasswordForm passForm = new PasswordForm();
                if (passForm.ShowDialog() != DialogResult.OK)
                    return;

                Config.Database = new PassDb(saveDbDialog.FileName, passForm.Password);
                Config.Database.InitDb("KEEPASS HACKER EDTION DATABASE");
                Config.Database.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Create DATABASE ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Config.Database = null;
            }
        }

        private void listContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Config.Database == null)
            {
                e.Cancel = true;
                return;
            }
            bShowPassword.Enabled = passList.SelectedItems.Count != 0;
            bSaveFile.Enabled = passList.SelectedItems.Count != 0;

            if (passList.SelectedIndices.Count != 0)
            {
                bShowPassword.Enabled = _openData[int.Parse(passList.SelectedItems[0].Text) - 1].Value ==
                                        DatabaseEntryType.EntryTypePassword;

                bSaveFile.Enabled = _openData[int.Parse(passList.SelectedItems[0].Text) - 1].Value ==
                                        DatabaseEntryType.EntryTypeFile;
            }
        }

        private void bExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void bAddEntry_Click(object sender, EventArgs e)
        {
            if(Config.Database == null)
                return;

            NewEntryForm entry = new NewEntryForm();
            if(entry.ShowDialog() != DialogResult.OK)
                return;

            Config.Database.AddPassword(entry.EntryName, entry.EntryPassword);
            UpdateList();
        }

        private void bShowPassword_Click(object sender, EventArgs e)
        {
            if (Config.Database == null)
                return;
            try
            {
                MessageBox.Show(Config.Database.GetPassword(int.Parse(passList.SelectedItems[0].Text) - 1),
                    @"Your password (Press Cntrl + C to copy)", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bAddFile_Click(object sender, EventArgs e)
        {
            if (Config.Database == null)
                return;

            addFileDialog.FileName = "";
            if(addFileDialog.ShowDialog() != DialogResult.OK)
                return;

            Config.Database.AddFile(addFileDialog.SafeFileName, addFileDialog.FileName);
            UpdateList();
        }

        private void bSaveFile_Click(object sender, EventArgs e)
        {
            if (Config.Database == null)
                return;

            exporFileDialog.FileName = "";
            if(exporFileDialog.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                Config.Database.ExportFile(int.Parse(passList.SelectedItems[0].Text) - 1, exporFileDialog.FileName);
                MessageBox.Show(@"Saved", @"INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bAoutButton_Click(object sender, EventArgs e) => new AboutForm().ShowDialog();
    }
}
