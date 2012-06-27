using System;
using System.Windows.Forms;

namespace EomApp1.Formss.Advertiser
{
    public partial class Advertisers : Form
    {
        public Advertisers()
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

        private void Advertisers_Load(object sender, EventArgs e)
        {
            this.advertiserTableAdapter.Fill(this.advertisersDataSet1.Advertiser);
        }
    }
}
