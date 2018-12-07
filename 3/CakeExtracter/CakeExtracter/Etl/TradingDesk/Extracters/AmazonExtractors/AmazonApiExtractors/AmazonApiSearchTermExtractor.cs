using System;
using System.Collections.Generic;
using System.Linq;
using Amazon;
using Amazon.Entities;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.Extracters.AmazonExtractors.AmazonApiExtractors
{
    public class AmazonApiSearchTermExtracter : BaseAmazonExtractor<SearchTermSummary>
    {
        public AmazonApiSearchTermExtracter(AmazonUtility amazonUtility, DateRange dateRange, ExtAccount account, string campaignFilter = null, string campaignFilterOut = null)
            : base(amazonUtility, dateRange, account, campaignFilter: campaignFilter, campaignFilterOut: campaignFilterOut)
        { }

        protected override void Extract()
        {
            Logger.Info(accountId, "Extracting SearchTermSummaries from Amazon API for ({0}) from {1:d} to {2:d}",
                this.clientId, this.dateRange.FromDate, this.dateRange.ToDate);

            foreach (var date in dateRange.Dates)
            {
                var sums = ExtractSummaries(date);
                var items = TransformSummaries(sums, date);
                Add(items);
            }
            End();
        }

        public IEnumerable<AmazonSearchTermDailySummary> ExtractSummaries(DateTime date)
        {
            var sums = _amazonUtility.ReportSearchTerms(date, clientId, true);
            var filteredSums = FilterByCampaigns(sums, x => x.campaignName);
            return filteredSums.ToList();
        }

        private IEnumerable<SearchTermSummary> TransformSummaries(IEnumerable<AmazonSearchTermDailySummary> searchTermStats, DateTime date)
        {
            searchTermStats = searchTermStats.Where(x => !x.AllZeros()).ToList();
            foreach (var stat in searchTermStats)
            {
                var sum = new SearchTermSummary
                {
                    SearchTermName = stat.query,
                    KeywordEid = stat.keywordId,
                    KeywordName = stat.keywordText,
                    AdSetEid = stat.adGroupId,
                    AdSetName = stat.adGroupName,
                    StrategyEid = stat.campaignId,
                    StrategyName = stat.campaignName,
                };
                SetCPProgStats(sum, stat, date);
                yield return sum;
            }
        }
    }
}
