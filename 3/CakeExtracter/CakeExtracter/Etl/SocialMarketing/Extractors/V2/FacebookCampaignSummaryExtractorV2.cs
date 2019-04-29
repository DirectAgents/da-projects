using System;
using System.Linq;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Facebook.Campaign;
using FacebookAPI;
using FacebookAPI.Entities;

namespace CakeExtracter.Etl.SocialMarketing.Extractors.V2
{
    /// <summary>
    /// Facebook campaigns summary extractor.
    /// </summary>
    /// <seealso cref="CakeExtracter.Etl.SocialMarketing.Extractors.V2.FacebookApiExtractorV2{DirectAgents.Domain.Entities.CPProg.Facebook.Campaign.FbCampaignSummary}" />
    public class FacebookCampaignSummaryExtractorV2 : FacebookApiExtractorV2<FbCampaignSummary>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookCampaignSummaryExtractorV2"/> class.
        /// </summary>
        /// <param name="dateRange">The date range.</param>
        /// <param name="account">The account.</param>
        /// <param name="fbUtility">The fb utility.</param>
        public FacebookCampaignSummaryExtractorV2(DateRange dateRange, ExtAccount account, FacebookInsightsDataProvider fbUtility)
            : base(fbUtility, dateRange, account)
        { }

        /// <summary>
        /// The derived class implements this method, which calls Add() for each item
        /// extracted and then calls End() when complete.
        /// </summary>
        protected override void Extract()
        {
            Logger.Info(accountId, "Extracting Campaign Summaries from Facebook API for ({0}) from {1:d} to {2:d}",
                        this.fbAccountId, this.dateRange.Value.FromDate, this.dateRange.Value.ToDate);
            try
            {
                var fbSums = _fbUtility.GetDailyCampaignStats("act_" + fbAccountId, dateRange.Value.FromDate, dateRange.Value.ToDate);
                var fbCampaignSummaryItems = fbSums.Select(CreateFbCampaignSummary);
                Add(fbCampaignSummaryItems);
            }
            catch (Exception ex)
            {
                Logger.Error(accountId, ex);
            }
            End();
        }

        private FbCampaignSummary CreateFbCampaignSummary(FBSummary item)
        {
            var sum = new FbCampaignSummary
            {
                Date = item.Date,
                Campaign = new FbCampaign
                {
                    AccountId = accountId,
                    Name = item.CampaignName,
                    ExternalId = item.CampaignId
                },
                Impressions = item.Impressions,
                AllClicks = item.AllClicks,
                Clicks = item.LinkClicks,
                PostClickConv = item.Conversions_click,
                PostViewConv = item.Conversions_view,
                PostClickRev = item.ConVal_click,
                PostViewRev = item.ConVal_view,
                Cost = item.Spend
            };
            return sum;
        }
    }
}
