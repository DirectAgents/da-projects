using QuickBooks.UI.Models;
using QuickBooks.UI.Views;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace QuickBooks.UI.Controls
{
    public partial class QuickBooksSynchControl : UserControlBase, IQuickBooksSynchView
    {
        public QuickBooksSynchControl()
        {
            InitializeComponent();
            new Presenters.QuickBooksSynchPresenter(this);

            FillCompanyFiles();
            SetSelectedCompanyFile();
        }

        void FillCompanyFiles()
        {
            if (this.GetCompanyFiles != null)
            {
                var eventArgs = new GetCompanyFilesEventArgs();
                this.GetCompanyFiles(this, eventArgs);

                if (eventArgs.CompanyFiles != null)
                    foreach (var item in eventArgs.CompanyFiles)
                        this.companyFileBindingSource.Add(item);
            }
        }

        void SetSelectedCompanyFile()
        {
            if (this.GetQuickBooksSynchSettings != null)
            {
                var eventArgs = new QuickBooksSynchSettingsEventArgs();
                this.GetQuickBooksSynchSettings(this, eventArgs);

                if (eventArgs.QuickBooksSynchSettings != null)
                    for (int i = 0; i < this.comboBox1.Items.Count; i++)
                        if (((CompanyFile)this.comboBox1.Items[i]).Id == eventArgs.QuickBooksSynchSettings.CompanyFileId)
                            this.comboBox1.SelectedIndex = i;
            }
        }

        public event System.EventHandler<GetCompanyFilesEventArgs> GetCompanyFiles;
        public event System.EventHandler<QuickBooksSynchSettingsEventArgs> GetQuickBooksSynchSettings;
        public event System.EventHandler Go;

        public QuickBooksSynchTargets[] SynchTargets
        {
            get 
            {
                var synchTargets = new List<QuickBooksSynchTargets>();
                foreach (string item in this.targetsCheckBoxList.CheckedItems)
                {
                    switch (item)
                    {
                        case "Customer":
                            synchTargets.Add(QuickBooksSynchTargets.Customer);
                            break;
                        case "Invoice":
                            synchTargets.Add(QuickBooksSynchTargets.Invoice);
                            break;
                        case "Payment":
                            synchTargets.Add(QuickBooksSynchTargets.Payment);
                            break;
                        case "Credit Memo":
                            synchTargets.Add(QuickBooksSynchTargets.CreditMemo);
                            break;
                        default:
                            break;
                    }
                }
                return synchTargets.ToArray();
            }
        }

        private void goButton_Click(object sender, System.EventArgs e)
        {
            if (this.Go != null)
                this.Go(this, EventArgs.Empty);
        }
    }
}
