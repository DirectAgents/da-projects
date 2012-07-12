using System;
using System.Windows.Forms;
using System.Data;

namespace EomApp1.Screens.Extra
{
    public partial class ExtraItemsForm : Form
    {
        public ExtraItemsForm()
        {
            InitializeComponent();
        }

        private void FormLoaded(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                // Reminder of how to do this (blog it?)
                //this.accountManagersDataSet.AccountManager.AddAccountManagerRow("default");
                //accountManagerTableAdapter.ClearBeforeFill = false;

                this.accountManagerTableAdapter.Fill(this.accountManagersDataSet.AccountManager);
                extraItemsUserControl.Initialize();
            }
        }

        private void RefreshButtonClicked(object sender, EventArgs e)
        {
            extraItemsUserControl.SaveAndFilterByAccountManager(comboBox1.Text);
        }
    }
}
