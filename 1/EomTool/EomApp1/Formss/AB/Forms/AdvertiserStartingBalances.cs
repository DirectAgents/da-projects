using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EomApp1.Formss.AB.Data;

namespace EomApp1.Formss.AB.Forms
{
    public partial class AdvertiserStartingBalances : Form
    {
        public AdvertiserStartingBalances()
        {
            InitializeComponent();
        }

        private void advertiserStartingBalanceBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.advertiserStartingBalanceBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dAMain1DataSet);

        }

        private void AdvertiserStartingBalances_Load(object sender, EventArgs e)
        {
            this.advertiserStartingBalanceTableAdapter.Fill(this.dAMain1DataSet.AdvertiserStartingBalance);
            this.advertiserTableAdapter1.Fill(this.dAMain1DataSet.Advertiser);
        }
    }
}
