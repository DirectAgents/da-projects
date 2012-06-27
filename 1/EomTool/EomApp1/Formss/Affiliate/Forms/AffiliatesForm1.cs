using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EomApp1.Formss.Affiliate.Data;

namespace EomApp1.Formss.Affiliate.Forms
{
    public partial class AffiliatesForm1 : Form
    {
        public AffiliatesForm1()
        {
            InitializeComponent();
        }

        private void affiliateBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.affiliateBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.affiliatesDataSet2);

        }

        private void AffiliatesForm1_Load(object sender, EventArgs e)
        {
            var db = new AffilaiteDataClasses1DataContext(DAgents.Common.Properties.Settings.Default.ConnStr);
            affiliateTableAdapter.Fill(this.affiliatesDataSet2.Affiliate);
            dataItemBindingSource.DataSource = (new DataItemList(db.MediaBuyers)).TheList;
            dataItemBindingSource1.DataSource = (new DataItemList(db.Currencies)).TheList;
            netTermTypeBindingSource.DataSource = db.NetTermTypes.Select(c => c).ToList();
            affiliatePaymentMethodBindingSource.DataSource = db.AffiliatePaymentMethods.Select(c => c).ToList();
        }
    }
}
