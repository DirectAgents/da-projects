using System;
using EomApp1.Screens.Final.Models;

namespace EomApp1.Screens.Final.Presenters
{
    public class PublishersPresenter
    {
        private Forms.PublishersForm view;
        private Models.PublishersModel model;

        public PublishersPresenter(Forms.PublishersForm view, Models.PublishersModel model)
        {
            this.view = view;
            this.model = model;

            this.view.PublishersToFinalizeSelected += new EventHandler<PublishersEventArgs>(view_PublishersToFinalizeSelected);
            this.view.PublishersToVerifySelected += new EventHandler<PublishersEventArgs>(view_PublishersToVerifySelected);
        }

        void view_PublishersToFinalizeSelected(object sender, PublishersEventArgs e)
        {
            this.model.ChangePublisherItems(e.AffIds, CampaignStatusId.Default, CampaignStatusId.Finalized);

            this.view.PublishersToFinalize.Clear();
            this.model.FillPublishers(this.view.PublishersToFinalize, CampaignStatusId.Default);

            this.view.PublishersToVerify.Clear();
            this.model.FillPublishers(this.view.PublishersToVerify, CampaignStatusId.Finalized);

            this.view.RefreshCampaigns();
        }

        void view_PublishersToVerifySelected(object sender, PublishersEventArgs e)
        {
            this.model.ChangePublisherItems(e.AffIds, CampaignStatusId.Finalized, CampaignStatusId.Verified);

            this.view.PublishersToVerify.Clear();
            this.model.FillPublishers(this.view.PublishersToVerify, CampaignStatusId.Finalized);

            this.view.RefreshCampaigns();
        }

        public void Present()
        {
            this.view.SetWindowText(this.model.CampaignName, this.model.AdvertiserName);
            this.model.FillPublishers(this.view.PublishersToFinalize, CampaignStatusId.Default);
            this.model.FillPublishers(this.view.PublishersToVerify, CampaignStatusId.Finalized);
            this.view.InitializeNetTermsFilter();
        }
    }
}
