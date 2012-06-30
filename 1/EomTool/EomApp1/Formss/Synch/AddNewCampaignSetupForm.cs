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
    public partial class AddNewCampaignSetupForm : Form
    {
        public AddNewCampaignSetupForm()
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

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
