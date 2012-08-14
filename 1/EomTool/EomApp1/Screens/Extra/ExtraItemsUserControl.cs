using System;
using System.ComponentModel;
using System.Windows.Forms;
using DAgents.Common;
using EomAppControls;
using EomAppControls.Filtering;

namespace EomApp1.Screens.Extra
{
    public partial class ExtraItemsUserControl : UserControlBase
    {
        private BindingSourceFilter itemFilter;

        public ExtraItemsUserControl()
        {
            InitializeComponent();
            extraItems.EnforceConstraints = false; // Prevent errors from popping up - need to look into why constraints get violated in first place...
            this.itemFilter = new BindingSourceFilter(this.itemBindingSource);
        }

        public void Initialize()
        {
            if (Running)
            {
                FillTableAdapters();
                SetupFilters();
            }
        }

        private void SetupFilters()
        {
            // TODO: Publisers
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
            campaignTableAdapter.Fill(extraItems.Campaign);
            affiliateTableAdapter.Fill(extraItems.Affiliate);
            sourceTableAdapter.Fill(extraItems.Source);
            itemReportingStatusTableAdapter.Fill(extraItems.ItemReportingStatus);
            itemTableAdapter.Fill(extraItems.Item);
        }

        private void SaveClicked(object sender, EventArgs e)
        {
            SaveData();
        }

        public void SaveAndFilterByAccountManager(string accountManagerName)
        {
            SaveData();

            if (accountManagerName == "default")
            {
                itemTableAdapter.Fill(extraItems.Item);
                campaignTableAdapter.Fill(extraItems.Campaign);
            }
            else
            {
                itemTableAdapter.FillBy(extraItems.Item, accountManagerName);
                campaignTableAdapter.FillBy(extraItems.Campaign, accountManagerName);
            }
            if (extraItems.Campaign.Rows.Count > 0)
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
        }

        private void SaveData()
        {
            Validate();
            this.itemBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.extraItems);
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
    }
}
