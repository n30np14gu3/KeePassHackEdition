using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeePassHackEdition
{
    public partial class PasswordForm : Form
    {
        public string Password { get; private set; }

        public PasswordForm()
        {
            Password = "";
            InitializeComponent();
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(tPassword.Text))
                return;

            Password = tPassword.Text;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
