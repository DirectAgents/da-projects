using Amazon;
using Amazon.Entities.Summaries;
using Amazon.Enums;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;
using System;
using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Contexts;
using Z.EntityFramework.Plus;

namespace CakeExtracter.Etl.TradingDesk.Extracters.AmazonExtractors.AmazonApiExtractors
{
    //The daily extracter will load data based on date range and sum up the total of all campaigns
    public class AmazonApiDailySummaryExtractor : BaseAmazonExtractor<DailySummary>
    {
        public AmazonApiDailySummaryExtractor(AmazonUtility amazonUtility, DateRange dateRange, ExtAccount account, bool clearBeforeLoad, string campaignFilter = null, string campaignFilterOut = null)
            : base(amazonUtility, dateRange, account, clearBeforeLoad, campaignFilter, campaignFilterOut)
        { }

        protected override void Extract()
        {
            Logger.Info(accountId, "Extracting DailySummaries from Amazon API for ({0}) from {1:d} to {2:d}", clientId,
                dateRange.FromDate, dateRange.ToDate);

            foreach (var date in dateRange.Dates)
            {
                Extract(date);
            }
            End();
        }

        private void Extract(DateTime date)
        {
            var sums = ExtractSummaries(date);
            var dailySum = TransformSummaries(sums, date);
            if (ClearBeforeLoad)
            {
                RemoveOldData(date);
            }

            Add(dailySum);
        }

        private IEnumerable<AmazonDailySummary> ExtractSummaries(DateTime date)
        {
            var spSums = _amazonUtility.ReportCampaigns(CampaignType.SponsoredProducts, date, clientId, true);
            var sbSums = _amazonUtility.ReportCampaigns(CampaignType.SponsoredBrands, date, clientId, true);
            var sums = spSums.Concat(sbSums);
            sums = FilterByCampaigns(sums, x => x.CampaignName);
            return sums.ToList();
        }

        private DailySummary TransformSummaries(IEnumerable<AmazonDailySummary> sums, DateTime date)
        {
            var dailySum = new DailySummary();
            SetCPProgStats(dailySum, sums, date);
            return dailySum;
        }

        private void RemoveOldData(DateTime date)
        {
            Logger.Info(accountId, "The cleaning of DailySummaries for account ({0}) has begun - {1}.", accountId, date);
            using (var db = new ClientPortalProgContext())
            {
                var removedMetrics = db.DailySummaryMetrics
                    .Where(x => x.EntityId == accountId && x.Date == date)
                    .Delete();

                var removedSummaries = db.DailySummaries
                    .Where(x => x.AccountId == accountId && x.Date == date)
                    .Delete();

                int numChanges = removedMetrics + removedSummaries;
                Logger.Info(accountId, "The cleaning of DailySummaries for account ({0}) is over - {1}. Count of deleted objects: {2}", accountId, date, numChanges);
            }
        }
    }
}
