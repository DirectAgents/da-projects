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
    public partial class AdvertiserMapping : Form
    {
        public AdvertiserMapping()
        {
            InitializeComponent();
            advertisersDataSet1.EnforceConstraints = false;
        }

        private void campaignBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.campaignBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.advertisersDataSet1);
        }

        private void AdvertiserMapping_Load(object sender, EventArgs e)
        {
            this.advertiserTableAdapter.Fill(this.advertisersDataSet1.Advertiser);
            this.campaignTableAdapter.Fill(this.advertisersDataSet1.Campaign);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            var a = new Advertisers();
            a.ShowDialog();

            Validate();
            campaignBindingSource.EndEdit();
            tableAdapterManager.UpdateAll(this.advertisersDataSet1);
            this.advertiserTableAdapter.Fill(this.advertisersDataSet1.Advertiser);
            this.campaignTableAdapter.Fill(this.advertisersDataSet1.Campaign);
        }
    }
}
