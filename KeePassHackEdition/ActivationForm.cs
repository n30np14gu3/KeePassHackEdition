using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KeePassHackEdition.SDK;

namespace KeePassHackEdition
{
    public partial class ActivationForm : Form
    {
        public ActivationForm()
        {
            InitializeComponent();
            bDev.Visible = true;
#if DEBUG
            bDev.Visible = true;
#endif
        }

        private void bActivateOnline_CheckedChanged(object sender, EventArgs e)
        {
            pOfflineActivation.Visible = bActivateOffline.Checked;
        }

        private void bActivateOffline_CheckedChanged(object sender, EventArgs e)
        {
            pOfflineActivation.Visible = bActivateOffline.Checked;
            tActivationRequest.Text = Config.LicManager.GenerateActivationRequest();
        }


        private void bDev_Click(object sender, EventArgs e)
        {
#if DEBUG
            tActivationResponse.Text = Config.LicManager.GenerateValidActivationResponse();
#else
            throw new Exception("Nice try xD");
#endif
        }

        private void bActivate_Click(object sender, EventArgs e)
        {
            if (bActivateOffline.Checked)
            {
                if (string.IsNullOrWhiteSpace(tActivationResponse.Text))
                    return;

                Config.LicenseInfo.ActivationResponse = tActivationResponse.Text;
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                try
                {
                    var values = new Dictionary<string, string>
                    {
                        { "license", Config.LicManager.GenerateActivationRequest() },
                    };
                    HttpClient client = new HttpClient();
                    var content = new FormUrlEncodedContent(values);
                    var result = client.PostAsync("http://www.keepass-hack-edition.su/api/activation", content).Result;
                    Config.LicenseInfo.ActivationResponse = result.Content.ReadAsStringAsync().Result;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, @"ACTIVATION ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
