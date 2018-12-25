using Amazon;
using Amazon.Entities.Summaries;
using Amazon.Enums;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;
using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;

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

            var accountKeywordIds = GetAccountKeywordIds();
            foreach (var date in dateRange.Dates)
            {
                Extract(accountKeywordIds, date);
            }
            End();
        }

        private void Extract(IEnumerable<int> accountKeywordIds, DateTime date)
        {
            var sums = ExtractSummaries(date);
            var items = TransformSummaries(sums, date);
            if (ClearBeforeLoad)
            {
                RemoveOldData(date, accountKeywordIds);
            }

            Add(items);
        }

        public IEnumerable<AmazonKeywordDailySummary> ExtractSummaries(DateTime date)
        {
            var spSums = _amazonUtility.ReportKeywords(CampaignType.SponsoredProducts, date, clientId, true);
            var sbSums = _amazonUtility.ReportKeywords(CampaignType.SponsoredBrands, date, clientId, true);
            var sums = spSums.Concat(sbSums);
            var filteredSums = FilterByCampaigns(sums, x => x.CampaignName);
            return filteredSums.ToList();
        }

        private IEnumerable<KeywordSummary> TransformSummaries(IEnumerable<AmazonKeywordDailySummary> keywordStats, DateTime date)
        {
            var notEmptyStats = keywordStats.Where(x => !x.AllZeros()).ToList();
            var summaries = notEmptyStats.Select(stat => CreateSummary(stat, date));
            return summaries.ToList();
        }

        private KeywordSummary CreateSummary(AmazonKeywordDailySummary keywordStat, DateTime date)
        {
            var sum = new KeywordSummary
            {
                KeywordEid = keywordStat.KeywordId,
                KeywordName = keywordStat.KeywordText,
                AdSetEid = keywordStat.AdGroupId,
                AdSetName = keywordStat.AdGroupName,
                StrategyEid = keywordStat.CampaignId,
                StrategyName = keywordStat.CampaignName,
            };
            SetCPProgStats(sum, keywordStat, date);
            return sum;
        }

        private IEnumerable<int> GetAccountKeywordIds()
        {
            using (var db = new ClientPortalProgContext())
            {
                var ids = db.Keywords.Where(x => x.AccountId == accountId).Select(x => x.Id);
                return ids.ToList();
            }
        }

        private void RemoveOldData(DateTime date, IEnumerable<int> accountKeywordIds)
        {
            using (var db = new ClientPortalProgContext())
            {
                var items = db.KeywordSummaries.Where(x => x.Date == date && accountKeywordIds.Contains(x.KeywordId)).ToList();
                var metrics = db.KeywordSummaryMetrics.Where(x => x.Date == date && accountKeywordIds.Contains(x.EntityId)).ToList();
                db.KeywordSummaryMetrics.RemoveRange(metrics);
                db.KeywordSummaries.RemoveRange(items);
                var numChanges = SafeContextWrapper.TrySaveChanges(db);
                Logger.Info(accountId, "{0} - KeywordSummaries for account ({1}) was cleaned. Count of deleted objects: {2}", date, accountId, numChanges);
            }
        }
    }
}
