using System;
using System.Linq;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Facebook.Daily;
using FacebookAPI;
using FacebookAPI.Entities;

namespace CakeExtracter.Etl.SocialMarketing.Extracters
{
    public class FacebookDailySummaryExtracter : FacebookApiExtracter<FbDailySummary>
    {
        public FacebookDailySummaryExtracter(DateRange dateRange, ExtAccount account, FacebookInsightsDataProvider fbUtility)
            : base(fbUtility, dateRange, account)
        { }

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
                Cost = item.Spend
            };
            return sum;
        }
    }
}
