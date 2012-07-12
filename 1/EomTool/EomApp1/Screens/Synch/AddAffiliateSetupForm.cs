using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EomApp1.Screens.Synch
{
    public partial class AddAffiliateSetupForm : Form
    {
        public AddAffiliateSetupForm()
        {
            InitializeComponent();
        }

        public int CakeAffiliateId
        {
            get
            {
                return int.Parse(cakeAffiliateIDTextBox.Text);
            }
        }

        public int AffiliateId
        {
            get
            {
                return int.Parse(affiliateIDTextBox.Text);
            }
        }

        public bool CreateAffiliateWithSafeId
        {
            get
            {
                return this.checkBox1.Enabled && this.checkBox1.Checked;
            }
        }

        private void okClick(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void cancelClick(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void textChanged(object sender, EventArgs e)
        {
            int number;
            bool cakeAffiliateIDHasNumber = int.TryParse(this.cakeAffiliateIDTextBox.Text, out number);
            bool affiliateIDHasNumber = int.TryParse(this.affiliateIDTextBox.Text, out number);
            if (cakeAffiliateIDHasNumber)
            {
                okButton.Enabled = true;
            }
            if (affiliateIDHasNumber)
            {
                checkBox1.Enabled = false;
            }
            else
            {
                checkBox1.Enabled = true;
            }
        }
    }
}
