﻿using System;
using System.Linq;
using EomApp1.Screens.Final.Models;

namespace EomApp1.Screens.Final.UI
{
    public class PublishersPresenter
    {
        private MediaBuyerApprovalStatusId? mediaBuyerApprovalStatus;

        public PublishersPresenter(PublishersForm view, Models.CampaignPublishers model, MediaBuyerApprovalStatusId? mediaBuyerApprovalStatus)
        {
            this.mediaBuyerApprovalStatus = mediaBuyerApprovalStatus;
            View = view;
            Model = model;
            View.PublishersToFinalizeSelected += new EventHandler<PublishersEventArgs>(view_PublishersToFinalizeSelected);
            View.PublishersToVerifySelected += new EventHandler<PublishersEventArgs>(view_PublishersToVerifySelected);
        }

        private bool MarginCheck(PublishersEventArgs e)
        {
            return FinalizeUtility.CheckFinalizationMargins(Model.Pid, Model.Currency, e.AffIds, e.CostCurrs);
        }

        void view_PublishersToFinalizeSelected(object sender, PublishersEventArgs e)
        {
            if (!MarginCheck(e)) return;

            Model.ChangeCampaignStatus(CampaignStatusId.Default, CampaignStatusId.Finalized, e.AffIds, e.CostCurrs);
            View.PublishersToFinalize.Clear();
            Model.Fill(View.PublishersToFinalize, CampaignStatusId.Default, null);
            View.PublishersToVerify.Clear();
            Model.Fill(View.PublishersToVerify, CampaignStatusId.Finalized, this.mediaBuyerApprovalStatus);
            View.RefreshCampaigns();
        }

        void view_PublishersToVerifySelected(object sender, PublishersEventArgs e)
        {
            Model.ChangeCampaignStatus(CampaignStatusId.Finalized, CampaignStatusId.Verified, e.AffIds, e.CostCurrs, MediaBuyerApprovalStatusId.Approved);
            View.PublishersToVerify.Clear();
            Model.Fill(View.PublishersToVerify, CampaignStatusId.Finalized, this.mediaBuyerApprovalStatus);
            View.RefreshCampaigns();
        }

        public void Present()
        {
            View.SetWindowText(Model.CampaignName, Model.AdvertiserName);
            Model.Fill(View.PublishersToFinalize, CampaignStatusId.Default, null);
            Model.Fill(View.PublishersToVerify, CampaignStatusId.Finalized, this.mediaBuyerApprovalStatus);
            View.InitializeNetTermsFilter();

            // Security
            DisableActions();
        }

        private void DisableActions()
        {
            if (!Security.User.Current.CanFinalizeForAccountManager(Model.AccountManagerName))
            {
                View.DisableFinalize();
            }
            if (!Security.User.Current.CanDoWorkflowVerify || this.mediaBuyerApprovalStatus != MediaBuyerApprovalStatusId.Approved)
            {
                View.DisableVerify();
            }
        }

        private PublishersForm View { get; set; }
        private CampaignPublishers Model { get; set; }
    }
}
