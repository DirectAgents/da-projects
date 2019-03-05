using Amazon;
using Amazon.Entities.Summaries;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;
using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using CakeExtracter.Logging.TimeWatchers;
using CakeExtracter.Logging.TimeWatchers.Amazon;

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

            foreach (var date in dateRange.Dates)
            {
                try
                {
                    Extract(date);
                }
                catch (Exception e)
                {
                    Logger.Error(accountId, e);
                }
            }

            End();
        }

        private void Extract(DateTime date)
        {
            IEnumerable<SearchTermSummary> items = null;
            AmazonTimeTracker.Instance.ExecuteWithTimeTracking(() =>
            {
                items = GetSearchTermSummariesFromApi(date);
            }, accountId, AmazonJobLevels.searchTerm, AmazonJobOperations.reportExtracting);
            if (ClearBeforeLoad)
            {
                RemoveOldData(date);
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

        private void RemoveOldData(DateTime date)
        {
            Logger.Info(accountId, "The cleaning of SearchTermSummaries for account ({0}) has begun - {1}.", accountId, date);
            AmazonTimeTracker.Instance.ExecuteWithTimeTracking(() =>
            {
                SafeContextWrapper.TryMakeTransaction((ClientPortalProgContext db) =>
                {
                    db.SearchTermSummaryMetrics.Where(x => x.Date == date && x.SearchTerm.AccountId == accountId).DeleteFromQuery();
                    db.SearchTermSummaries.Where(x => x.Date == date && x.SearchTerm.AccountId == accountId).DeleteFromQuery();
                }, "DeleteFromQuery");
            }, accountId, AmazonJobLevels.searchTerm, AmazonJobOperations.cleanExistingData);
            Logger.Info(accountId, "The cleaning of SearchTermSummaries for account ({0}) is over - {1}.", accountId, date);
        }
    }
}
