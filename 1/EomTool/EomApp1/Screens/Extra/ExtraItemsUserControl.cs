using System;
using System.ComponentModel;
using System.Windows.Forms;
using DAgents.Common;
using EomAppControls;
using EomAppControls.Filtering;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Resources;
using System.Linq;

namespace EomApp1.Screens.Extra
{
    public partial class ExtraItemsUserControl : UserControlBase
    {
        private BindingSourceFilter itemFilter;
        private BindingSource campaignFilterBindingSource;

        public ExtraItemsUserControl()
        {
            InitializeComponent();
            extraItems.EnforceConstraints = false; // Prevent errors from popping up - need to look into why constraints get violated in first place...
            this.itemFilter = new BindingSourceFilter(this.itemBindingSource);

            // Security
            itemsGrid.Sorted += (s, e) => DisableRows();
            itemBindingSource.ListChanged += (s, e) => DisableRows();
            itemsGrid.KeyDown += itemsGrid_KeyDown;
            itemsGrid.MultiSelect = false;
        }

        // Security
        void itemsGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyValue == 46 && itemsGrid.CurrentRow.ReadOnly)
                e.Handled = true;
        }

        // Security
        private void DisableRows()
        {
            using (var db = EomApp1.Screens.Final.Models.Eom.Create())
            {
                var amByPID = db.Campaigns.ToDictionary(c => c.pid, c => c.AccountManager.name);
                if (itemsGrid.Rows.Count > 0)
                    foreach (var item in from row in itemsGrid.Rows.Cast<DataGridViewRow>()
                                         where !row.IsNewRow
                                         let pid = ((row.DataBoundItem as DataRowView).Row as ExtraItemDataSet.ItemRow).pid
                                         let am = amByPID.ContainsKey(pid) ? amByPID[pid] : null
                                         where am != null
                                         select new { Row = row, AM = am })
                    {
                        if (!Security.User.Current.CanFinalizeForAccountManager(item.AM))
                        {
                            item.Row.DefaultCellStyle.BackColor = DisabledBackColor;
                            item.Row.ReadOnly = true;
                        }
                    }
            }
        }

        private static Color DisabledBackColor
        {
            get { return Color.Gray;}
        }

        public void Initialize()
        {
            if (Running)
            {
                FillTableAdapters();
                SetupFilters();
                SetupCampaignBindings();
            }
        }

        private void SetupCampaignBindings()
        {
            DataView dv = new DataView(extraItems.Campaign);
            campaignFilterBindingSource = new BindingSource();
            campaignFilterBindingSource.DataSource = dv;
        }

        private void itemsGrid_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == colCampaign.Index && itemsGrid[colAdvertiser.Index, e.RowIndex].Value != null)
            {
                DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)itemsGrid[e.ColumnIndex, e.RowIndex];
                int advertiser_id = (int)itemsGrid[colAdvertiser.Index, e.RowIndex].Value;

                if (advertiser_id == -1)
                {   // if advertiser not selected, show all campaigns
                    cell.DataSource = campaignBindingSource;
                }
                else
                {
                    cell.DataSource = campaignFilterBindingSource;
                    cell.ValueMember = "pid";
                    cell.DisplayMember = "campaign_name";
                    campaignFilterBindingSource.Filter = "advertiser_id=" + advertiser_id;
                }
                CheckValidCampaignForRow(e.RowIndex);
            }
        }

        private void itemsGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == colCampaign.Index)
            {
                campaignFilterBindingSource.RemoveFilter();
                DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)itemsGrid[e.ColumnIndex, e.RowIndex];
                cell.DataSource = campaignBindingSource;
            }
        }

        private void itemsGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                // Make sure the edited row is always displayed
                var cell = itemsGrid[colID.Index, e.RowIndex];
                int id = (int)cell.Value;
                if (id >= 0)
                    this.itemFilter.EditedIds.Add(id);

                if (e.ColumnIndex == colAdvertiser.Index)
                {
                    CheckValidCampaignForRow(e.RowIndex);
                }
                else if (e.ColumnIndex == colCampaign.Index)
                {
                    CheckSetAdvertiserForRow(e.RowIndex);
                }
                this.itemFilter.Apply();
            }
        }

        private void CheckSetAdvertiserForRow(int rowIndex)
        {
            int curr_advertiser_id = (int)itemsGrid[colAdvertiser.Index, rowIndex].Value;
            if (curr_advertiser_id == -1)
            {
                int pid = (int)itemsGrid[colCampaign.Index, rowIndex].Value;
                var rows = extraItems.Campaign.Select("pid=" + pid);
                if (rows.Length > 0)
                {
                    itemsGrid[colAdvertiser.Index, rowIndex].Value = rows[0]["advertiser_id"];
                }
            }
        }

        // See if the row's pid is valid based on the row's advertiser id. If not, give it a valid one.
        private void CheckValidCampaignForRow(int rowIndex)
        {
            if (itemsGrid[colAdvertiser.Index, rowIndex] != null)
            {
                int advertiser_id = (int)itemsGrid[colAdvertiser.Index, rowIndex].Value;
                if (advertiser_id > 0)
                {
                    int pid = (int)itemsGrid[colCampaign.Index, rowIndex].Value;
                    var rows = extraItems.Campaign.Select("advertiser_id=" + advertiser_id + " AND pid=" + pid);
                    if (rows.Length == 0)
                    {
                        rows = extraItems.Campaign.Select("advertiser_id=" + advertiser_id);
                        if (rows.Length > 0)
                        {
                            itemsGrid[colCampaign.Index, rowIndex].Value = rows[0]["pid"];
                        }
                        // if no campaigns for the advertiser, do nothing
                    }
                }
            }
        }

        private void SetupFilters()
        {
            SetupComboBoxColumnFilter(colAdvertiser, colAdvertiserName);
            SetupComboBoxColumnFilter(colCampaign, colCampaignName);
            SetupComboBoxColumnFilter(colPublisher, colPublisherName);
        }

        private void SetupComboBoxColumnFilter(DataGridViewComboBoxColumn comboBoxColumn, DataGridViewTextBoxColumn textBoxColumn)
        {
            var filterHeaderCell = new DataGridViewTextBoxFilterColumnHeaderCell(comboBoxColumn.HeaderCell);
            filterHeaderCell.FilterChanged += (s, e) => this.itemFilter[textBoxColumn.DataPropertyName] = e.FilterText;
            comboBoxColumn.HeaderCell = filterHeaderCell;
        }

        private void FillTableAdapters()
        {
            currencyTableAdapter.Fill(extraItems.Currency);
            unitTypeTableAdapter.Fill(extraItems.UnitType);
            FillAdvertisers(null);
            campaignTableAdapter.Fill(extraItems.Campaign);
            affiliateTableAdapter.Fill(extraItems.Affiliate);
            sourceTableAdapter.Fill(extraItems.Source);
            itemReportingStatusTableAdapter.Fill(extraItems.ItemReportingStatus);
            itemTableAdapter.Fill(extraItems.Item);
        }

        private void FillAdvertisers(string accountManagerName)
        {
            extraItems.Advertiser.Clear();
            var row = extraItems.Advertiser.NewAdvertiserRow();
            row.id = -1; row.name = "-";
            extraItems.Advertiser.AddAdvertiserRow(row);

            advertiserTableAdapter.ClearBeforeFill = false;
            if (accountManagerName == null)
                advertiserTableAdapter.Fill(extraItems.Advertiser);
            else
                advertiserTableAdapter.FillByAM(extraItems.Advertiser, accountManagerName);
        }

        private void SaveClicked(object sender, EventArgs e)
        {
            SaveData();
            this.itemFilter.Apply();
        }

        public void SaveAndFilterByAccountManager(string accountManagerName)
        {
            Validate();
            this.itemBindingSource.EndEdit();

            var items = extraItems.Item;
            var changedItems = items.Where(c => c.RowState == DataRowState.Added || c.RowState == DataRowState.Deleted || c.RowState == DataRowState.Modified);
            int changeCount = changedItems.Count();
            if (changeCount > 0)
            {
                var result = MessageBox.Show(string.Format("Save changes to {0} items?", changeCount), "Save Changes", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    SaveData();
                }
                else
                {
                    this.itemFilter.EditedIds.Clear();
                }
            }

            if (accountManagerName == "default")
            {
                itemTableAdapter.Fill(extraItems.Item);
                FillAdvertisers(null);
                campaignTableAdapter.Fill(extraItems.Campaign);
            }
            else
            {
                itemTableAdapter.FillBy(extraItems.Item, accountManagerName);
                FillAdvertisers(accountManagerName);
                campaignTableAdapter.FillBy(extraItems.Campaign, accountManagerName);
            }
            if (extraItems.Advertiser.Rows.Count > 0 && extraItems.Campaign.Rows.Count > 0)
            {
                itemsGrid.AllowUserToAddRows = true;
                bindingNavigatorAddNewItem.Enabled = true;
                extraItems.Item.pidColumn.DefaultValue = extraItems.Campaign[0][extraItems.Campaign.pidColumn.Ordinal];
                // ? if removing AM filter, go back to default campaign with pid=99999 ?
            }
            else
            {
                itemsGrid.AllowUserToAddRows = false;
                bindingNavigatorAddNewItem.Enabled = false;
            }

            this.itemFilter.Apply();
        }

        private void SaveData()
        {
            Validate();
            this.itemBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.extraItems);
            this.itemFilter.EditedIds.Clear();
        }

        private void itemsGrid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == colCampaign.Index)
            {
                Sort(colCampaignName, colCampaign);
            }
            else if (e.ColumnIndex == colPublisher.Index)
            {
                Sort(colPublisherName, colPublisher);
            }
            else if (e.ColumnIndex == colAdvertiser.Index)
            {
                Sort(colAdvertiserName, colAdvertiser);
            }
        }
        private void Sort(DataGridViewColumn sortColumn, DataGridViewColumn displayColumn)
        {
            ListSortDirection direction = ListSortDirection.Ascending;
            if (itemsGrid.SortedColumn != null && itemsGrid.SortedColumn.Index == sortColumn.Index && itemsGrid.SortOrder == SortOrder.Ascending)
            {
                direction = ListSortDirection.Descending;
            }
            itemsGrid.Sort(sortColumn, direction);
            displayColumn.HeaderCell.SortGlyphDirection = (direction == ListSortDirection.Ascending) ? SortOrder.Ascending : SortOrder.Descending;
        }

        private void itemsGrid_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (NeedsEditIcon(e.RowIndex))
                e.Graphics.DrawImage(Properties.Resources.editing, e.RowBounds.Left + 2, e.RowBounds.Top + 2);
        }

        private bool NeedsEditIcon(int rowIndex)
        {
            int rowID = (int)(itemsGrid.Rows[rowIndex].Cells[colID.Index].Value ?? 0);

            bool rowIsEdited = this.itemFilter.EditedIds.Contains(rowID);
            bool rowIsNew = rowID < 0;

            return rowIsEdited || rowIsNew;
        }

        private void itemsGrid_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (colID.Value<int>(e) == -1)
            {
                itemsGrid.BeginEdit(true);
            }
        }
    }
}
