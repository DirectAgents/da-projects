using System;
using System.Windows.Forms;

namespace EomApp1.Screens.Final.Forms
{
    public partial class PublishersForm : Form
    {
        private static string WindowTitleFormat = "Publishers for {0} ({1})";
        private Presenters.PublishersPresenter presenter;
        private FinalizeForm1 parent;

        public PublishersForm()
        {
            InitializeComponent();
        }

        public PublishersForm(FinalizeForm1 finalizeForm, int pid)
        {
            InitializeComponent();
            this.parent = finalizeForm;
            InitializeMVP(pid);
        }

        private void InitializeMVP(int pid)
        {
            var model = new Models.PublishersModel(pid);
            this.presenter = new Presenters.PublishersPresenter(this, model);
            this.Shown += new EventHandler(FormShown);
            this.finalizePublishersView.PublishersSelected += new EventHandler<PublishersEventArgs>(finalizePublishersView_PublishersSelected);
            this.verifyPublishersView.PublishersSelected += new EventHandler<PublishersEventArgs>(verifyPublishersView_PublishersSelected);
        }

        void finalizePublishersView_PublishersSelected(object sender, PublishersEventArgs e)
        {
            if (PublishersToFinalizeSelected != null)
            {
                PublishersToFinalizeSelected(this, new PublishersEventArgs(e.AffIds));
            }
        }

        void verifyPublishersView_PublishersSelected(object sender, PublishersEventArgs e)
        {
            if (PublishersToFinalizeSelected != null)
            {
                PublishersToVerifySelected(this, new PublishersEventArgs(e.AffIds));
            }
        }

        void FormShown(object sender, EventArgs e)
        {
            this.presenter.Present();
            this.finalizePublishersView.ClearSelection();
            this.verifyPublishersView.ClearSelection();
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

        public event EventHandler<PublishersEventArgs> PublishersToFinalizeSelected;
        public event EventHandler<PublishersEventArgs> PublishersToVerifySelected;
    }
}
