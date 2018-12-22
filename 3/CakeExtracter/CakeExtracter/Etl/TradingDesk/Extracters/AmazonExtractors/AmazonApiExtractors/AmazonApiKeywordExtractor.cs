using Amazon;
using Amazon.Entities.Summaries;
using Amazon.Enums;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CakeExtracter.Etl.TradingDesk.Extracters.AmazonExtractors.AmazonApiExtractors
{
    public class AmazonApiKeywordExtractor : BaseAmazonExtractor<KeywordSummary>
    {
        public AmazonApiKeywordExtractor(AmazonUtility amazonUtility, DateRange dateRange, ExtAccount account, string campaignFilter = null, string campaignFilterOut = null)
            : base(amazonUtility, dateRange, account, campaignFilter, campaignFilterOut)
        { }

        protected override void Extract()
        {
            Logger.Info(accountId, "Extracting KeywordSummaries from Amazon API for ({0}) from {1:d} to {2:d}",
                clientId, dateRange.FromDate, dateRange.ToDate);

            foreach (var date in dateRange.Dates)
            {
                var items = GetKeywordSummariesFromApi(date);
                Add(items);
            }
            End();
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
            };
            SetCPProgStats(sum, stat, date);
            return sum;
        }
    }
}
