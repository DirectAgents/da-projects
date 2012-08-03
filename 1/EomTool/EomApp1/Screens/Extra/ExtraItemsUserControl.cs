using System;
using System.Windows.Forms;
using EomAppControls;

namespace EomApp1.Screens.Extra
{
    public partial class ExtraItemsUserControl : UserControlBase
    {
        public ExtraItemsUserControl()
        {
            InitializeComponent();
            extraItems.EnforceConstraints = false; // Prevent errors from popping up - need to look into why constraints get violated in first place...
        }

        public void Initialize()
        {
            if (Running)
            {
                FillTableAdapters();
            }
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
            }
            else
            {
                itemTableAdapter.FillBy(extraItems.Item, accountManagerName);
            }
        }

        private void SaveData()
        {
            Validate();
            this.itemBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.extraItems);
        }
    }
}
