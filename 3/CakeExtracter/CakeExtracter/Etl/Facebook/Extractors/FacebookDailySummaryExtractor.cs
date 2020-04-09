using System;
using System.Linq;
using CakeExtracter.Commands;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Facebook.Daily;
using FacebookAPI.Entities;
using FacebookAPI.Providers;

namespace CakeExtracter.Etl.Facebook.Extractors
{
    /// <inheritdoc />
    /// <summary>
    /// Facebook daily summary extractor.
    /// </summary>
    public class FacebookDailySummaryExtractor : FacebookApiExtractor<FbDailySummary, FacebookInsightsDataProvider>
    {
        /// <inheritdoc cref="FacebookApiExtractor{T,TProvider}"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookDailySummaryExtractor"/> class.
        /// </summary>
        /// <param name="dateRange">The date range.</param>
        /// <param name="account">The account.</param>
        /// <param name="fbUtility">The fb utility.</param>
        public FacebookDailySummaryExtractor(DateRange dateRange, ExtAccount account, FacebookInsightsDataProvider fbUtility)
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
            Logger.Info(
                AccountId,
                $"Extracting DailySummaries from Facebook API for ({FbAccountId}) from {DateRange?.FromDate:d} to {DateRange?.ToDate:d}");
            try
            {
                var fbSums = FbUtility.GetDailyStats("act_" + FbAccountId, DateRange.Value.FromDate, DateRange.Value.ToDate);
                var fbDailySummaryItems = fbSums.Select(CreateFbDailySummary);
                Add(fbDailySummaryItems);
            }
            catch (Exception ex)
            {
                OnProcessFailedExtraction(
                    this.DateRange?.FromDate,
                    this.DateRange?.ToDate,
                    this.AccountId,
                    FbStatsTypeAgg.DailyArg,
                    ex);
                Logger.Error(AccountId, ex);
            }
            End();
        }

        private FbDailySummary CreateFbDailySummary(FBSummary item)
        {
            var sum = new FbDailySummary
            {
                Date = item.Date,
                AccountId = AccountId,
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
