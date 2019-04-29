using System;
using System.Linq;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Facebook.Daily;
using FacebookAPI;
using FacebookAPI.Entities;

namespace CakeExtracter.Etl.SocialMarketing.Extracters.V2
{
    /// <summary>
    /// Facebook daily summary extractor.
    /// </summary>
    /// <seealso cref="CakeExtracter.Etl.SocialMarketing.Extracters.FacebookApiExtracter{DirectAgents.Domain.Entities.CPProg.Facebook.Daily.FbDailySummary}" />
    public class FacebookDailySummaryExtractorV2 : FacebookApiExtractorV2<FbDailySummary>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookDailySummaryExtracter"/> class.
        /// </summary>
        /// <param name="dateRange">The date range.</param>
        /// <param name="account">The account.</param>
        /// <param name="fbUtility">The fb utility.</param>
        public FacebookDailySummaryExtractorV2(DateRange dateRange, ExtAccount account, FacebookInsightsDataProvider fbUtility)
            : base(fbUtility, dateRange, account)
        { }

        /// <summary>
        /// The derived class implements this method, which calls Add() for each item
        /// extracted and then calls End() when complete.
        /// </summary>
        protected override void Extract()
        {
            Logger.Info(accountId, "Extracting DailySummaries from Facebook API for ({0}) from {1:d} to {2:d}",
                        this.fbAccountId, this.dateRange.Value.FromDate, this.dateRange.Value.ToDate);
            try
            {
                var fbSums = _fbUtility.GetDailyStats("act_" + fbAccountId, dateRange.Value.FromDate, dateRange.Value.ToDate);
                var fbDailySummaryItems = fbSums.Select(CreateFbDailySummary);
                Add(fbDailySummaryItems);
            }
            catch (Exception ex)
            {
                Logger.Error(accountId, ex);
            }
            End();
        }

        private FbDailySummary CreateFbDailySummary(FBSummary item)
        {
            var sum = new FbDailySummary
            {
                Date = item.Date,
                AccountId = accountId,
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
