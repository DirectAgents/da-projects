using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EomApp1.Screens.Advertiser2.Controls
{
    public partial class Advertisers2Control : UserControl
    {
        public Advertisers2Control()
        {
            InitializeComponent();
        }

        private void advertiserBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.advertiserBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.advertiser2DataSet);
        }

        private void Advertisers2Control_Load(object sender, EventArgs e)
        {
            DoFill();
        }

        public void DoFill()
        {
            advertiserTableAdapter.Fill(advertiser2DataSet.Advertiser);
        }
    }
}
