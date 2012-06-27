using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EomApp1.Formss.Advertiser
{
    public partial class AdvertiserMapping2 : Form
    {
        public AdvertiserMapping2()
        {
            InitializeComponent();
            advertisersDataSet1.EnforceConstraints = false;
        }

        private void advertiserBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.advertiserBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.advertisersDataSet1);

        }

        private void AdvertiserMapping2_Load(object sender, EventArgs e)
        {
            this.campaignTableAdapter.Fill(this.advertisersDataSet1.Campaign);
            this.advertiserTableAdapter.Fill(this.advertisersDataSet1.Advertiser);

        }
    }
}
