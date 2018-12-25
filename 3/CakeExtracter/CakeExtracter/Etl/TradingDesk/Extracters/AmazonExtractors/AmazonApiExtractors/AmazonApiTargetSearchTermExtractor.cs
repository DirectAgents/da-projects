using System;
using System.Collections.Generic;
using System.Linq;
using Amazon;
using Amazon.Entities.Summaries;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.Extracters.AmazonExtractors.AmazonApiExtractors
{
    public class AmazonApiTargetSearchTermExtractor : BaseAmazonExtractor<SearchTermSummary>
    {
        public AmazonApiTargetSearchTermExtractor(AmazonUtility amazonUtility, DateRange dateRange, ExtAccount account, string campaignFilter = null, string campaignFilterOut = null)
            : base(amazonUtility, dateRange, account, false, campaignFilter, campaignFilterOut)
        { }

        protected override void Extract()
        {
            Logger.Info(accountId, "Extracting TargetSearchTermSummaries from Amazon API for ({0}) from {1:d} to {2:d}",
                clientId, dateRange.FromDate, dateRange.ToDate);

            foreach (var date in dateRange.Dates)
            {
                var sums = ExtractSummaries(date);
                var items = TransformSummaries(sums, date);
                Add(items);
            }
            End();
        }
        
        public IEnumerable<AmazonTargetSearchTermDailySummary> ExtractSummaries(DateTime date)
        {
            var sums = _amazonUtility.ReportTargetSearchTerms(date, clientId, true);
            var filteredSums = FilterByCampaigns(sums, x => x.CampaignName);
            return filteredSums.ToList();
        }

        private IEnumerable<SearchTermSummary> TransformSummaries(IEnumerable<AmazonTargetSearchTermDailySummary> searchTermStats, DateTime date)
        {
            var notEmptyStats = searchTermStats.Where(x => !x.AllZeros()).ToList();
            var summaries = notEmptyStats.Select(stat => CreateSummary(stat, date));
            return summaries.ToList();
        }

        private SearchTermSummary CreateSummary(AmazonTargetSearchTermDailySummary stat, DateTime date)
        {
            var sum = new SearchTermSummary
            {
                SearchTermName = stat.Query,
                KeywordEid = stat.TargetId,
                KeywordName = stat.TargetingText,
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
