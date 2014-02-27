using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EomApp1.Screens.Campaign
{
    public partial class CampaignsUC : UserControl
    {
        private int[] _requiredCols;
        private int[] RequiredCols
        {
            get
            {
                if (_requiredCols == null)
                    _requiredCols = new int[] { colAccountManager.Index, colAdManager.Index, colAdvertiser.Index, colPID.Index, colName.Index };
                return _requiredCols;
            }
        }
        private int[] _integerCols;
        private int[] IntegerCols
        {
            get
            {
                if (_integerCols == null)
                    _integerCols = new int[] { colPID.Index };
                return _integerCols;
            }
        }

        public CampaignsUC()
        {
            InitializeComponent();
        }

        internal void Fill()
        {
            campaignTableAdapter.Fill(campaignDataSet.Campaign);
            accountManagerTableAdapter.Fill(campaignDataSet.AccountManager);
            adManagerTableAdapter.Fill(campaignDataSet.AdManager);
            advertiserTableAdapter.Fill(campaignDataSet.Advertiser);
            campaignStatusTableAdapter.Fill(campaignDataSet.CampaignStatus);
            dTCampaignStatusTableAdapter.Fill(campaignDataSet.DTCampaignStatus);
            //dTCampaignTypeTableAdapter.Fill(campaignDataSet.DTCampaignType);
        }

        private void campaignBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.campaignBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.campaignDataSet);
        }

        private void CampaignsUC_Load(object sender, EventArgs e)
        {

        }

        private void campaignDataGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == colAccountManager.Index)
            {
                Sort(colAccountManagerName, colAccountManager);
            }
            else if (e.ColumnIndex == colAdManager.Index)
            {
                Sort(colAdManagerName, colAdManager);
            }
            else if (e.ColumnIndex == colAdvertiser.Index)
            {
                Sort(colAdvertiserName, colAdvertiser);
            }
        }

        private void Sort(DataGridViewColumn sortColumn, DataGridViewColumn displayColumn)
        {
            ListSortDirection direction = ListSortDirection.Ascending;
            if (campaignDataGridView.SortedColumn != null && campaignDataGridView.SortedColumn.Index == sortColumn.Index && campaignDataGridView.SortOrder == SortOrder.Ascending)
            {
                direction = ListSortDirection.Descending;
            }
            campaignDataGridView.Sort(sortColumn, direction);
            displayColumn.HeaderCell.SortGlyphDirection = (direction == ListSortDirection.Ascending) ? SortOrder.Ascending : SortOrder.Descending;
        }

        private void campaignDataGridView_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            foreach (var requiredCol in this.RequiredCols)
            {
                var cell = campaignDataGridView.Rows[e.RowIndex].Cells[requiredCol];
                if (cell.FormattedValue.ToString() == string.Empty)
                {
                    cell.ErrorText = "Mandatory";
                    e.Cancel = true;
                }
                else
                {
                    cell.ErrorText = string.Empty;
                }
            }
        }

        private void campaignDataGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            int val;
            if (this.IntegerCols.Contains(e.ColumnIndex))
            {
                var cell = campaignDataGridView[e.ColumnIndex, e.RowIndex];
                if (!int.TryParse(e.FormattedValue.ToString(), out val))
                {
                    cell.ErrorText = "Must be an integer";
                    e.Cancel = true;
                }
                else
                {
                    cell.ErrorText = string.Empty;
                }
            }
        }
    }
}
