﻿using System;
using System.Windows.Forms;
using EomApp1.Screens.Final.Models;

namespace EomApp1.Screens.Final.UI
{
    public partial class PublishersForm : Form
    {
        private static string WindowTitleFormat = "Publishers for {0} ({1})";
        private PublishersPresenter presenter;
        private FinalizeForm1 parent;
        private Mode mode;
        private string initialFilter;

        public enum Mode { Finalize, Verify }

        public PublishersForm()
        {
            InitializeComponent();
        }

        public PublishersForm(FinalizeForm1 finalizeForm, int pid, string currency, Mode mode, string initialFilter, MediaBuyerApprovalStatusId? mediaBuyerApprovalStatus)
        {
            InitializeComponent();
            this.parent = finalizeForm;
            this.mode = mode;
            this.initialFilter = initialFilter;
            InitializeMVP(pid, currency, mediaBuyerApprovalStatus);
        }

        private void InitializeMVP(int pid, string currency, MediaBuyerApprovalStatusId? mediaBuyerApprovalStatus)
        {
            var model = new CampaignPublishers(pid, currency);
            this.presenter = new PublishersPresenter(this, model, mediaBuyerApprovalStatus);
            this.Shown += new EventHandler(FormShown);
            this.finalizePublishersView.PublishersActionInvoked += new EventHandler<PublishersEventArgs>(finalizePublishersView_PublishersActionInvoked);
            this.verifyPublishersView.PublishersActionInvoked += new EventHandler<PublishersEventArgs>(verifyPublishersView_PublishersActionInvoked);
        }

        void finalizePublishersView_PublishersActionInvoked(object sender, PublishersEventArgs e)
        {
            if (PublishersToFinalizeSelected != null)
                PublishersToFinalizeSelected(this, e);
        }

        void verifyPublishersView_PublishersActionInvoked(object sender, PublishersEventArgs e)
        {
            if (PublishersToFinalizeSelected != null)
                PublishersToVerifySelected(this, e);
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

        public Data PublishersToFinalize
        {
            get { return this.finalizePublishersView.PublishersDataSet; }
        }

        public Data PublishersToVerify
        {
            get { return this.verifyPublishersView.PublishersDataSet; }
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

        public void DisableFinalize()
        {
            this.finalizePublishersView.ActionButtonEnabled = false;
        }

        public void DisableVerify()
        {
            this.verifyPublishersView.ActionButtonEnabled = false;
        }

        public event EventHandler<PublishersEventArgs> PublishersToFinalizeSelected;
        public event EventHandler<PublishersEventArgs> PublishersToVerifySelected;
    }
}
