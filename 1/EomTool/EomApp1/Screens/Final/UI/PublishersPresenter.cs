﻿using System;
using EomApp1.Screens.Final.Models;

namespace EomApp1.Screens.Final.UI
{
    public class PublishersPresenter
    {
        public PublishersPresenter(PublishersForm view, Models.CampaignPublishers model)
        {
            View = view;
            Model = model;
            View.PublishersToFinalizeSelected += new EventHandler<PublishersEventArgs>(view_PublishersToFinalizeSelected);
            View.PublishersToVerifySelected += new EventHandler<PublishersEventArgs>(view_PublishersToVerifySelected);
        }

        void view_PublishersToFinalizeSelected(object sender, PublishersEventArgs e)
        {
            Model.ChangeCampaignStatus(CampaignStatusId.Default, CampaignStatusId.Finalized, e.AffIds, e.CostCurrs);
            View.PublishersToFinalize.Clear();
            Model.Fill(View.PublishersToFinalize, CampaignStatusId.Default);
            View.PublishersToVerify.Clear();
            Model.Fill(View.PublishersToVerify, CampaignStatusId.Finalized);
            View.RefreshCampaigns();
        }

        void view_PublishersToVerifySelected(object sender, PublishersEventArgs e)
        {
            Model.ChangeCampaignStatus(CampaignStatusId.Finalized, CampaignStatusId.Verified, e.AffIds, e.CostCurrs);
            View.PublishersToVerify.Clear();
            Model.Fill(View.PublishersToVerify, CampaignStatusId.Finalized);
            View.RefreshCampaigns();
        }

        public void Present()
        {
            View.SetWindowText(Model.CampaignName, Model.AdvertiserName);
            Model.Fill(View.PublishersToFinalize, CampaignStatusId.Default);
            Model.Fill(View.PublishersToVerify, CampaignStatusId.Finalized);
            View.InitializeNetTermsFilter();

            // Security
            DisableActions();
        }

        private void DisableActions()
        {
            if (!Security.User.Current.CanDoWorkflowFinalize(Model.AccountManagerName))
            {
                View.DisableFinalize();
            }
            if (!Security.User.Current.CanDoWorkflowVerify)
            {
                View.DisableVerify();
            }
        }

        private PublishersForm View { get; set; }
        private CampaignPublishers Model { get; set; }
    }
}
