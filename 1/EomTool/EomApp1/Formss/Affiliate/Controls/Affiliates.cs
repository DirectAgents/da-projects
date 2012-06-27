using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EomApp1.Formss.Affiliate.Controls
{
    public partial class Affiliates : UserControl
    {
        public Affiliates()
        {
            InitializeComponent();
            var ident = System.Security.Principal.WindowsIdentity.GetCurrent();

            UserName = ident.Name;

            //MessageBox.Show(UserName);

            if (UserName.ToLower() == @"directagents\jbhawsar")
            {
                dataGridViewTextBoxColumn2.ReadOnly = true;
            }
        }

        private void affiliateBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.affiliateBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.affiliatesDataSet);

        }

        private void Affiliates_Load(object sender, EventArgs e)
        {
            DoFill();
        }

        private void DoFill()
        {
            currencyTableAdapter.Fill(affiliatesDataSet.Currency);
            mediaBuyerTableAdapter.Fill(affiliatesDataSet.MediaBuyer);
            affiliateTableAdapter.Fill(affiliatesDataSet.Affiliate);
            netTermTypeTableAdapter.Fill(affiliatesDataSet.NetTermType);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //var logger = new Common.LoggerForm();
            //logger.DoIt(this, () =>
            //{
            foreach (DataGridViewRow item in affiliateDataGridView.SelectedRows)
            {
                int id = (int)item.Cells["colAffId"].Value;
                DAgents.Synch.SynchUtility.SynchAffiliateToMediaBuyer(id, null);
            }
            DoRefresh();
            //});
        }

        private void DoRefresh()
        {
            Validate();
            affiliateBindingSource.EndEdit();
            tableAdapterManager.UpdateAll(this.affiliatesDataSet);
            DoFill();
        }

        public string UserName { get; set; }
    }
}
