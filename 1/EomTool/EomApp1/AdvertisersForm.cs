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
    public partial class AdvertisersForm : Form
    {
        public AdvertisersForm()
        {
            InitializeComponent();
            advertiserDataGridView.CurrentCellDirtyStateChanged += new EventHandler(advertiserDataGridView_CurrentCellDirtyStateChanged);
        }

        void advertiserDataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            advertiserBindingNavigatorSaveItem.Enabled = true;
        }

        private void advertiserBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            Save();
            advertiserBindingNavigatorSaveItem.Enabled = false;
        }

        private void Save()
        {
            this.Validate();
            this.advertiserBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dataSet1);
        }

        private void AdvertisersForm_Load(object sender, EventArgs e)
        {
            this.advertiserTableAdapter.Fill(this.dataSet1.Advertiser);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.advertiserTableAdapter.Fill(this.dataSet1.Advertiser);
        }
    }
}
