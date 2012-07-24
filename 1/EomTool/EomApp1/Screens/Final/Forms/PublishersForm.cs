using System;
using System.Windows.Forms;

namespace EomApp1.Screens.Final.Forms
{
    public partial class PublishersForm : Form
    {
        private static string WindowTitleFormat = "Publishers for {0} ({1})";
        private Presenters.PublishersPresenter presenter;
        private FinalizeForm1 parent;
        private Mode mode;
        private string initialFilter;

        public enum Mode { Finalize, Verify }

        public PublishersForm()
        {
            InitializeComponent();
        }

        public PublishersForm(FinalizeForm1 finalizeForm, int pid, Mode mode, string initialFilter)
        {
            InitializeComponent();
            this.parent = finalizeForm;
            this.mode = mode;
            this.initialFilter = initialFilter;
            InitializeMVP(pid);
        }

        private void InitializeMVP(int pid)
        {
            var model = new Models.PublishersModel(pid);
            this.presenter = new Presenters.PublishersPresenter(this, model);
            this.Shown += new EventHandler(FormShown);
            this.finalizePublishersView.PublishersActionInvoked += new EventHandler<PublishersEventArgs>(finalizePublishersView_PublishersActionInvoked);
            this.verifyPublishersView.PublishersActionInvoked += new EventHandler<PublishersEventArgs>(verifyPublishersView_PublishersActionInvoked);
        }

        void finalizePublishersView_PublishersActionInvoked(object sender, PublishersEventArgs e)
        {
            if (PublishersToFinalizeSelected != null)
            {
                PublishersToFinalizeSelected(this, new PublishersEventArgs(e.AffIds));
            }
        }

        void verifyPublishersView_PublishersActionInvoked(object sender, PublishersEventArgs e)
        {
            if (PublishersToFinalizeSelected != null)
            {
                PublishersToVerifySelected(this, new PublishersEventArgs(e.AffIds));
            }
        }

        void FormShown(object sender, EventArgs e)
        {
            this.presenter.Present();
            if (mode == Mode.Finalize)
            {
                this.finalizePublishersView.SelectAll();
                this.finalizePublishersView.SetNetTermFilter(this.initialFilter);

                this.verifyPublishersView.SelectNone();
            }
            else
            {
                this.finalizePublishersView.SelectNone();

                this.verifyPublishersView.SelectAll();
                this.verifyPublishersView.SetNetTermFilter(this.initialFilter);
            }
        }

        public void SetWindowText(string campaignName, string advertiserName)
        {
            this.Text = string.Format(WindowTitleFormat, campaignName, advertiserName);
        }

        public Data.PublishersDataSet.PublishersDataTable PublishersToFinalize
        {
            get { return this.finalizePublishersView.PublishersDataSet.Publishers; } 
        }

        public Data.PublishersDataSet.PublishersDataTable PublishersToVerify
        {
            get { return this.verifyPublishersView.PublishersDataSet.Publishers; }
        }

        public void RefreshCampaigns()
        {
            this.parent.RefreshCampaigns();
        }

        public void InitializeNetTermsFilter()
        {
            this.finalizePublishersView.InitializeNetTermsDropdown();
            this.verifyPublishersView.InitializeNetTermsDropdown();
        }

        public event EventHandler<PublishersEventArgs> PublishersToFinalizeSelected;
        public event EventHandler<PublishersEventArgs> PublishersToVerifySelected;
    }
}
