using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EomApp1
{
    public partial class AdvertiserMappingForm : Form
    {
        public AdvertiserMappingForm()
        {
            InitializeComponent();
        }

        private void AdvertiserMappingForm_Load(object sender, EventArgs e)
        {
            this.advertiserTableAdapter.FillBysorted(this.dataSet1.Advertiser);
            this.campaignTableAdapter.Fill(this.dataSet1.Campaign);
        }

        private void campaignBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            Save();
            campaignBindingNavigatorSaveItem.Enabled = false;
        }

        private void Save()
        {
            this.Validate();
            this.campaignBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dataSet1);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.advertiserTableAdapter.FillBysorted(this.dataSet1.Advertiser);
            this.campaignTableAdapter.Fill(this.dataSet1.Campaign);
        }

        private void campaignDataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            campaignBindingNavigatorSaveItem.Enabled = true;
        }
    }
}
