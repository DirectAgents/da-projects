using System;
using System.Linq;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Facebook.Campaign;
using FacebookAPI.Entities;
using FacebookAPI.Providers;

namespace CakeExtracter.Etl.Facebook.Extractors
{
    /// <inheritdoc />
    /// <summary>
    /// Facebook campaigns summary extractor.
    /// </summary>
    public class FacebookCampaignSummaryExtractor : FacebookApiExtractor<FbCampaignSummary, FacebookInsightsDataProvider>
    {
        /// <inheritdoc cref="FacebookApiExtractor{T,TProvider}"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookCampaignSummaryExtractor"/> class.
        /// </summary>
        /// <param name="dateRange">The date range.</param>
        /// <param name="account">The account.</param>
        /// <param name="fbUtility">The fb utility.</param>
        public FacebookCampaignSummaryExtractor(DateRange dateRange, ExtAccount account, FacebookInsightsDataProvider fbUtility)
            : base(fbUtility, dateRange, account)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// The derived class implements this method, which calls Add() for each item
        /// extracted and then calls End() when complete.
        /// </summary>
        protected override void Extract()
        {
            Logger.Info(AccountId, "Extracting Campaign Summaries from Facebook API for ({0}) from {1:d} to {2:d}",
                        this.FbAccountId, this.DateRange.Value.FromDate, this.DateRange.Value.ToDate);
            try
            {
                var fbSums = FbUtility.GetDailyCampaignStats("act_" + FbAccountId, DateRange.Value.FromDate, DateRange.Value.ToDate);
                var fbCampaignSummaryItems = fbSums.Select(CreateFbCampaignSummary);
                Add(fbCampaignSummaryItems);
            }
            catch (Exception ex)
            {
                Logger.Error(AccountId, ex);
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
                    AccountId = AccountId,
                    Name = item.CampaignName,
                    ExternalId = item.CampaignId,
                },
                Impressions = item.Impressions,
                AllClicks = item.AllClicks,
                Clicks = item.LinkClicks,
                PostClickConv = item.Conversions_click,
                PostViewConv = item.Conversions_view,
                PostClickRev = item.ConVal_click,
                PostViewRev = item.ConVal_view,
                Cost = item.Spend,
            };
            return sum;
        }
    }
}
