using Amazon;
using Amazon.Entities.Summaries;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;
using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;

namespace CakeExtracter.Etl.TradingDesk.Extracters.AmazonExtractors.AmazonApiExtractors
{
    public class AmazonApiSearchTermExtractor : BaseAmazonExtractor<SearchTermSummary>
    {
        public AmazonApiSearchTermExtractor(AmazonUtility amazonUtility, DateRange dateRange, ExtAccount account, bool clearBeforeLoad, string campaignFilter = null, string campaignFilterOut = null)
            : base(amazonUtility, dateRange, account, clearBeforeLoad, campaignFilter, campaignFilterOut)
        { }

        protected override void Extract()
        {
            Logger.Info(accountId, "Extracting SearchTermSummaries from Amazon API for ({0}) from {1:d} to {2:d}",
                clientId, dateRange.FromDate, dateRange.ToDate);

            var accountTermIds = GetAccountSearchTermIds();
            foreach (var date in dateRange.Dates)
            {
                Extract(accountTermIds, date);
            }
            End();
        }

        private void Extract(IEnumerable<int> accountTermIds, DateTime date)
        {
            var sums = ExtractSummaries(date);
            var items = TransformSummaries(sums, date);
            if (ClearBeforeLoad)
            {
                RemoveOldData(date, accountTermIds);
            }

            Add(items);
        }

        public IEnumerable<AmazonSearchTermDailySummary> ExtractSummaries(DateTime date)
        {
            var sums = _amazonUtility.ReportSearchTerms(date, clientId, true);
            var filteredSums = FilterByCampaigns(sums, x => x.CampaignName);
            return filteredSums.ToList();
        }

        private IEnumerable<SearchTermSummary> TransformSummaries(IEnumerable<AmazonSearchTermDailySummary> searchTermStats, DateTime date)
        {
            var notEmptyStats = searchTermStats.Where(x => !x.AllZeros()).ToList();
            var summaries = notEmptyStats.Select(stat => CreateSummary(stat, date));
            return summaries.ToList();
        }

        private SearchTermSummary CreateSummary(AmazonSearchTermDailySummary stat, DateTime date)
        {
            var sum = new SearchTermSummary
            {
                SearchTermName = stat.Query,
                KeywordEid = stat.KeywordId,
                KeywordName = stat.KeywordText,
                AdSetEid = stat.AdGroupId,
                AdSetName = stat.AdGroupName,
                StrategyEid = stat.CampaignId,
                StrategyName = stat.CampaignName,
            };
            SetCPProgStats(sum, stat, date);
            return sum;
        }

        private IEnumerable<int> GetAccountSearchTermIds()
        {
            using (var db = new ClientPortalProgContext())
            {
                var ids = db.SearchTerms.Where(x => x.AccountId == accountId).Select(x => x.Id);
                return ids.ToList();
            }
        }

        private void RemoveOldData(DateTime date, IEnumerable<int> accountTermIds)
        {
            using (var db = new ClientPortalProgContext())
            {
                var items = db.SearchTermSummaries.Where(x => x.Date == date && accountTermIds.Contains(x.SearchTermId)).ToList();
                var metrics = db.SearchTermSummaryMetrics.Where(x => x.Date == date && accountTermIds.Contains(x.EntityId)).ToList();
                db.SearchTermSummaryMetrics.RemoveRange(metrics);
                db.SearchTermSummaries.RemoveRange(items);
                var numChanges = SafeContextWrapper.TrySaveChanges(db);
                Logger.Info(accountId, "{0} - SearchTermSummaries for account ({1}) was cleaned. Count of deleted objects: {2}", date, accountId, numChanges);
            }
        }
    }
}
