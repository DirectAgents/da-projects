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
    public partial class Extras : Form
    {
        public Extras()
        {
            InitializeComponent();
        }

        private void Extras_Load(object sender, EventArgs e)
        {
            this.unitTypeTableAdapter.Fill(this.dataSetForExtra1.UnitType);
            this.sourceTableAdapter.Fill(this.dataSetForExtra1.Source);
            this.currencyTableAdapter.Fill(this.dataSetForExtra1.Currency);
            this.advertiserTableAdapter.Fill(this.dataSetForExtra1.Advertiser);
            this.campaignTableAdapter.Fill(this.dataSetForExtra1.Campaign);
            this.affiliateTableAdapter.Fill(this.dataSetForExtra1.Affiliate);
            this.extraTableAdapter.Fill(this.dataSetForExtra1.Extra);
        }

        private void fillBySourceIdToolStripButton_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    this.extraTableAdapter.FillBySourceId(this.dataSetForExtra.Extra, ((int)(System.Convert.ChangeType(_1ToolStripTextBox.Text, typeof(int)))));
            //}
            //catch (System.Exception ex)
            //{
            //    System.Windows.Forms.MessageBox.Show(ex.Message);
            //}
        }

        private void extraDataGridView_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void extraBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.extraBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dataSetForExtra1);
        }
    }
}
