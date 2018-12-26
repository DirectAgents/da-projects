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
            var items = GetSearchTermSummariesFromApi(date);
            if (ClearBeforeLoad)
            {
                RemoveOldData(date, accountTermIds);
            }

            Add(items);
        }

        public IEnumerable<SearchTermSummary> GetSearchTermSummariesFromApi(DateTime date)
        {
            var searchTermSums = ExtractSearchTermSummaries(date);
            var searchTermItems = TransformSummaries(searchTermSums, date);
            var targetSums = ExtractTargetSummaries(date);
            var targetItems = TransformSummaries(targetSums, date);
            var items = searchTermItems.Concat(targetItems);
            return items.ToList();
        }

        public IEnumerable<AmazonSearchTermDailySummary> ExtractSearchTermSummaries(DateTime date)
        {
            var sums = _amazonUtility.ReportSearchTerms(date, clientId, true);
            var filteredSums = FilterByCampaigns(sums, x => x.CampaignName);
            return filteredSums.ToList();
        }

        public IEnumerable<AmazonTargetSearchTermDailySummary> ExtractTargetSummaries(DateTime date)
        {
            var sums = _amazonUtility.ReportTargetSearchTerms(date, clientId, true);
            var filteredSums = FilterByCampaigns(sums, x => x.CampaignName);
            return filteredSums.ToList();
        }

        private IEnumerable<SearchTermSummary> TransformSummaries(IEnumerable<AmazonSearchTermDailySummary> searchTermStats, DateTime date)
        {
            var notEmptyStats = searchTermStats.Where(x => !x.AllZeros()).ToList();
            var summaries = notEmptyStats.Select(stat => CreateSummary(stat, date));
            return summaries.ToList();
        }

        private IEnumerable<SearchTermSummary> TransformSummaries(IEnumerable<AmazonTargetSearchTermDailySummary> searchTermStats, DateTime date)
        {
            var notEmptyStats = searchTermStats.Where(x => !x.AllZeros()).ToList();
            var summaries = notEmptyStats.Select(stat => CreateSummary(stat, date));
            return summaries.ToList();
        }

        private SearchTermSummary CreateSummary(AmazonSearchTermDailySummary stat, DateTime date)
        {
            var sum = CreateSummary(stat as AmazonAdGroupSummary, date);
            sum.SearchTermName = stat.Query;
            sum.KeywordEid = stat.KeywordId;
            sum.KeywordName = stat.KeywordText;
            return sum;
        }

        private SearchTermSummary CreateSummary(AmazonTargetSearchTermDailySummary targetStat, DateTime date)
        {
            var sum = CreateSummary(targetStat as AmazonAdGroupSummary, date);
            sum.SearchTermName = targetStat.Query;
            sum.KeywordEid = targetStat.TargetId;
            sum.KeywordName = targetStat.TargetingText;
            return sum;
        }

        private SearchTermSummary CreateSummary(AmazonAdGroupSummary stat, DateTime date)
        {
            var sum = new SearchTermSummary
            {
                AdSetEid = stat.AdGroupId,
                AdSetName = stat.AdGroupName,
                StrategyEid = stat.CampaignId,
                StrategyName = stat.CampaignName,
                StrategyType = stat.CampaignType
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
                var items = db.SearchTermSummaries.Where(x => x.Date == date && accountTermIds.Contains(x.SearchTermId));
                var metrics = db.SearchTermSummaryMetrics.Where(x => x.Date == date && accountTermIds.Contains(x.EntityId));
                db.SearchTermSummaryMetrics.RemoveRange(metrics);
                db.SearchTermSummaries.RemoveRange(items);
                var numChanges = SafeContextWrapper.TrySaveChanges(db);
                Logger.Info(accountId, "{0} - SearchTermSummaries for account ({1}) was cleaned. Count of deleted objects: {2}", date, accountId, numChanges);
            }
        }
    }
}
