using Amazon;
using Amazon.Entities.Summaries;
using Amazon.Enums;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;
using System;
using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Contexts;
using CakeExtracter.Logging.TimeWatchers.Amazon;
using CakeExtracter.Logging.TimeWatchers;

namespace CakeExtracter.Etl.TradingDesk.Extracters.AmazonExtractors.AmazonApiExtractors
{
    public class AmazonApiKeywordExtractor : BaseAmazonExtractor<KeywordSummary>
    {
        public AmazonApiKeywordExtractor(AmazonUtility amazonUtility, DateRange dateRange, ExtAccount account, bool clearBeforeLoad, string campaignFilter = null, string campaignFilterOut = null)
            : base(amazonUtility, dateRange, account, clearBeforeLoad, campaignFilter, campaignFilterOut)
        { }

        protected override void Extract()
        {
            Logger.Info(accountId, "Extracting KeywordSummaries from Amazon API for ({0}) from {1:d} to {2:d}",
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
            IEnumerable<KeywordSummary> items = null;
            AmazonTimeTracker.Instance.ExecuteWithTimeTracking(() => {
                items = GetKeywordSummariesFromApi(date);
            }, accountId, AmazonJobLevels.keyword, AmazonJobOperations.reportExtracting);
            if (ClearBeforeLoad)
            {
                RemoveOldData(date);
            }
            Add(items);
        }

        public IEnumerable<KeywordSummary> GetKeywordSummariesFromApi(DateTime date)
        {
            var keywordSums = ExtractKeywordSummaries(date);
            var keywordItems = TransformSummaries(keywordSums, date);
            var targetSums = ExtractTargetSummaries(date);
            var targetItems = TransformSummaries(targetSums, date);
            var items = keywordItems.Concat(targetItems);
            return items.ToList();
        }

        public IEnumerable<AmazonKeywordDailySummary> ExtractKeywordSummaries(DateTime date)
        {
            var spSums = _amazonUtility.ReportKeywords(CampaignType.SponsoredProducts, date, clientId, true);
            var sbSums = _amazonUtility.ReportKeywords(CampaignType.SponsoredBrands, date, clientId, true);
            var sums = spSums.Concat(sbSums);
            var filteredSums = FilterByCampaigns(sums, x => x.CampaignName);
            return filteredSums.ToList();
        }

        public IEnumerable<AmazonTargetKeywordDailySummary> ExtractTargetSummaries(DateTime date)
        {
            var sums = _amazonUtility.ReportTargetKeywords(date, clientId, true);
            var filteredSums = FilterByCampaigns(sums, x => x.CampaignName);
            return filteredSums.ToList();
        }

        private IEnumerable<KeywordSummary> TransformSummaries(IEnumerable<AmazonKeywordDailySummary> keywordStats, DateTime date)
        {
            var notEmptyStats = keywordStats.Where(x => !x.AllZeros()).ToList();
            var summaries = notEmptyStats.Select(stat => CreateSummary(stat, date));
            return summaries.ToList();
        }

        private IEnumerable<KeywordSummary> TransformSummaries(IEnumerable<AmazonTargetKeywordDailySummary> targetStats, DateTime date)
        {
            var notEmptyStats = targetStats.Where(x => !x.AllZeros()).ToList();
            var summaries = notEmptyStats.Select(stat => CreateSummary(stat, date));
            return summaries.ToList();
        }

        private KeywordSummary CreateSummary(AmazonKeywordDailySummary keywordStat, DateTime date)
        {
            var sum = CreateSummary(keywordStat as AmazonAdGroupSummary, date);
            sum.KeywordEid = keywordStat.KeywordId;
            sum.KeywordName = keywordStat.KeywordText;
            return sum;
        }

        private KeywordSummary CreateSummary(AmazonTargetKeywordDailySummary targetStat, DateTime date)
        {
            var sum = CreateSummary(targetStat as AmazonAdGroupSummary, date);
            sum.KeywordEid = targetStat.TargetId;
            sum.KeywordName = targetStat.TargetingText;
            return sum;
        }

        private KeywordSummary CreateSummary(AmazonAdGroupSummary stat, DateTime date)
        {
            var sum = new KeywordSummary
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
            Logger.Info(accountId, "The cleaning of KeywordSummaries for account ({0}) has begun - {1}.", accountId, date);
            AmazonTimeTracker.Instance.ExecuteWithTimeTracking(() => {
                using (var db = new ClientPortalProgContext())
                {
                    db.KeywordSummaryMetrics.Where(x => x.Date == date && x.Keyword.AccountId == accountId).DeleteFromQuery();
                    db.KeywordSummaries.Where(x => x.Date == date && x.Keyword.AccountId == accountId).DeleteFromQuery();
                }
            }, accountId, AmazonJobLevels.keyword, AmazonJobOperations.cleanExistingData);
            Logger.Info(accountId, "The cleaning of KeywordSummaries for account ({0}) is over - {1}.", accountId, date);
        }
    }
}
