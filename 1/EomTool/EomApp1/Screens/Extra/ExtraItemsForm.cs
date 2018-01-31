using System;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace EomApp1.Screens.Extra
{
    public partial class ExtraItemsForm : Form
    {
        public ExtraItemsForm()
        {
            InitializeComponent();

            var finalizeConcat = (!Security.User.Current.FinalizeList.Any() ? "''" :
                String.Join(",", Security.User.Current.FinalizeList.Select(x => "'" + x + "'")));
            this.accountManagerBindingSource.Filter = "name in (" + finalizeConcat + ")";
        }

        private void FormLoaded(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                // Reminder of how to do this (blog it?)
                //this.accountManagersDataSet.AccountManager.AddAccountManagerRow("default");
                //accountManagerTableAdapter.ClearBeforeFill = false;

                this.accountManagerTableAdapter.Fill(this.accountManagersDataSet.AccountManager);
                extraItemsUserControl.Initialize(comboBox1.Text); // pass in account manager
            }
        }

        private void RefreshButtonClicked(object sender, EventArgs e)
        {
            extraItemsUserControl.CheckSave();
            extraItemsUserControl.FilterByAccountManager(comboBox1.Text);
        }
    }
}
