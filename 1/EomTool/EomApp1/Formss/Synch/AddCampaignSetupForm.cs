using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EomApp1.Formss.Synch
{
    public partial class AddCampaignSetupForm : Form
    {

        public AddCampaignSetupForm()
        {
            InitializeComponent();
        }

        public int OfferId
        {
            get
            {
                return int.Parse(offerIDTextBox.Text);
            }
        }

        public int PID
        {
            get
            {
                return int.Parse(pidTextBox.Text);
            }
        }

        private void okButtonClick(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void cancelButtonClick(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void textChanged(object sender, EventArgs e)
        {
            int number;
            if (int.TryParse(offerIDTextBox.Text, out number) && int.TryParse(pidTextBox.Text, out number))
            {
                okButton.Enabled = true;
            }
        }
    }
}
