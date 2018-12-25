using Amazon;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;
using Amazon.Entities.Summaries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CakeExtracter.Etl.TradingDesk.Extracters.AmazonExtractors.AmazonApiExtractors
{
    public class AmazonApiTargetKeywordExtractor : BaseAmazonExtractor<KeywordSummary>
    {
        public AmazonApiTargetKeywordExtractor(AmazonUtility amazonUtility, DateRange dateRange, ExtAccount account,
            string campaignFilter = null, string campaignFilterOut = null)
            : base(amazonUtility, dateRange, account, false, campaignFilter, campaignFilterOut)
        {
        }

        protected override void Extract()
        {
            Logger.Info(accountId, "Extracting TargetKeywordSummaries from Amazon API for ({0}) from {1:d} to {2:d}",
                clientId, dateRange.FromDate, dateRange.ToDate);

            foreach (var date in dateRange.Dates)
            {
                var sums = ExtractSummaries(date);
                var items = TransformSummaries(sums, date);
                Add(items);
            }
            End();
        }

        public IEnumerable<AmazonTargetKeywordDailySummary> ExtractSummaries(DateTime date)
        {
            var sums = _amazonUtility.ReportTargetKeywords(date, clientId, true);
            var filteredSums = FilterByCampaigns(sums, x => x.CampaignName);
            return filteredSums.ToList();
        }

        private IEnumerable<KeywordSummary> TransformSummaries(IEnumerable<AmazonTargetKeywordDailySummary> keywordStats, DateTime date)
        {
            var notEmptyStats = keywordStats.Where(x => !x.AllZeros()).ToList();
            var summaries = notEmptyStats.Select(stat => CreateSummary(stat, date));
            return summaries.ToList();
        }

        private KeywordSummary CreateSummary(AmazonTargetKeywordDailySummary keywordStat, DateTime date)
        {
            var sum = new KeywordSummary
            {
                KeywordEid = keywordStat.TargetId,
                KeywordName = keywordStat.TargetingText,
                AdSetEid = keywordStat.AdGroupId,
                AdSetName = keywordStat.AdGroupName,
                StrategyEid = keywordStat.CampaignId,
                StrategyName = keywordStat.CampaignName,
            };
            SetCPProgStats(sum, keywordStat, date);
            return sum;
        }
    }
}
