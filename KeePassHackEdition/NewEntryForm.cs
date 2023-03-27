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
    public partial class NewEntryForm : Form
    {
        public string EntryName { get; private set; }
        public string EntryPassword { get; private set; }

        public NewEntryForm()
        {
            EntryName = "";
            EntryPassword = "";
            InitializeComponent();
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(tName.Text) || string.IsNullOrWhiteSpace(tPassword.Text))
                return;

            EntryName = tName.Text;
            EntryPassword = tPassword.Text;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
