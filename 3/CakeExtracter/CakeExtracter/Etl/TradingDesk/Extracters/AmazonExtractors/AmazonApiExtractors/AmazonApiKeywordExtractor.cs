using System;
using System.Collections.Generic;
using System.Linq;
using Amazon;
using Amazon.Entities;
using Amazon.Enums;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.Extracters.AmazonExtractors.AmazonApiExtractors
{
    public class AmazonApiKeywordExtracter : BaseAmazonExtractor<KeywordSummary>
    {
        public AmazonApiKeywordExtracter(AmazonUtility amazonUtility, DateRange dateRange, ExtAccount account, string campaignFilter = null, string campaignFilterOut = null)
            : base(amazonUtility, dateRange, account, campaignFilter: campaignFilter, campaignFilterOut: campaignFilterOut)
        { }

        protected override void Extract()
        {
            Logger.Info(accountId, "Extracting KeywordSummaries from Amazon API for ({0}) from {1:d} to {2:d}",
                this.clientId, this.dateRange.FromDate, this.dateRange.ToDate);

            foreach (var date in dateRange.Dates)
            {
                var sums = ExtractSummaries(date);
                var items = TransformSummaries(sums, date);
                Add(items);
            }
            End();
        }

        public IEnumerable<AmazonKeywordDailySummary> ExtractSummaries(DateTime date)
        {
            var spSums = _amazonUtility.ReportKeywords(CampaignType.SponsoredProducts, date, clientId, true);
            var sbSums = _amazonUtility.ReportKeywords(CampaignType.SponsoredBrands, date, clientId, true);
            var sums = spSums.Concat(sbSums);
            var filteredSums = FilterByCampaigns(sums, x => x.campaignName);
            return filteredSums.ToList();
        }

        private IEnumerable<KeywordSummary> TransformSummaries(IEnumerable<AmazonKeywordDailySummary> keywordStats, DateTime date)
        {
            keywordStats = keywordStats.Where(x => !x.AllZeros()).ToList();
            foreach (var keywordStat in keywordStats)
            {
                var sum = new KeywordSummary
                {
                    KeywordEid = keywordStat.keywordId,
                    KeywordName = keywordStat.keywordText,
                    AdSetEid = keywordStat.adGroupId,
                    AdSetName = keywordStat.adGroupName,
                    StrategyEid = keywordStat.campaignId,
                    StrategyName = keywordStat.campaignName,
                };
                SetCPProgStats(sum, keywordStat, date);
                yield return sum;
            }
        }
    }
}
