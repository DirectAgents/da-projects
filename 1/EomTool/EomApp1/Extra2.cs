using System;
using System.Windows.Forms;

namespace EomApp1
{
    public partial class Extra2 : Form
    {
        public Extra2()
        {
            InitializeComponent();
        }

        private void extraBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.extraBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.extra2DataSet);
            
        }

        private void Extra2_Load(object sender, EventArgs e)
        {
            this.extraTableAdapter.Fill(this.extra2DataSet.Extra);
            this.affiliateTableAdapter.Fill(this.extra2DataSet.Affiliate);
            this.unitTypeTableAdapter.Fill(this.extra2DataSet.UnitType);
            this.currencyTableAdapter.Fill(this.extra2DataSet.Currency);
            this.advertiserTableAdapter.Fill(this.extra2DataSet.Advertiser);
            this.campaignTableAdapter.Fill(this.extra2DataSet.Campaign);
            this.sourceTableAdapter.Fill(this.extra2DataSet.Source);

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            bool b = !toolStripButton2.Checked;
            extraDataGridView.Columns["dataGridViewTextBoxColumnRep"].Visible = b;
            extraDataGridView.Columns["dataGridViewTextBoxColumnAdmanager"].Visible = b;
            extraDataGridView.Columns["dataGridViewTextBoxColumnAM"].Visible = b;
            toolStripButton2.Checked = b;
        }

        private void extraDataGridView_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            //if (e == null || e.Value == null) { return; }
            //if (this.extraDataGridView.Columns[e.ColumnIndex].Name == "dataGridViewTextBoxColumn5")
            //{
            //    try
            //    {
            //        if (extraDataGridView[e.ColumnIndex, e.RowIndex].Value.ToString() == "CPM")
            //        {
            //            e.Value = 999;
            //        }
            //    }
            //    catch (FormatException)
            //    {
            //        e.ParsingApplied = false;
            //    }
            //}
        }
    }
}
