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
    // AdSet(Ad group)
    public class AmazonApiAdSetExtractor : BaseAmazonExtractor<AdSetSummary>
    {
        public AmazonApiAdSetExtractor(AmazonUtility amazonUtility, DateRange dateRange, ExtAccount account, bool clearBeforeLoad, string campaignFilter = null, string campaignFilterOut = null)
            : base(amazonUtility, dateRange, account, clearBeforeLoad, campaignFilter, campaignFilterOut)
        { }

        protected override void Extract()
        {
            Logger.Info(accountId, "Extracting AdSetSummaries from Amazon API for ({0}) from {1:d} to {2:d}",
                clientId, dateRange.FromDate, dateRange.ToDate);

            var accountAdSetIds = GetAccountAdSetIds();
            foreach (var date in dateRange.Dates)
            {
                Extract(accountAdSetIds, date);
            }
            End();
        }

        private void Extract(IEnumerable<int> accountAdSetIds, DateTime date)
        {
            var sums = ExtractSummaries(date);
            var items = TransformSummaries(sums, date);
            if (ClearBeforeLoad)
            {
                RemoveOldData(date, accountAdSetIds);
            }

            Add(items);
        }

        public IEnumerable<AmazonAdGroupSummary> ExtractSummaries(DateTime date)
        {
            var spSums = _amazonUtility.ReportAdGroups(CampaignType.SponsoredProducts, date, clientId, true);
            var sbSums = _amazonUtility.ReportAdGroups(CampaignType.SponsoredBrands, date, clientId, true);
            var sums = spSums.Concat(sbSums);
            var filteredSums = FilterByCampaigns(sums, x => x.CampaignName);
            return filteredSums.ToList();
        }

        private IEnumerable<AdSetSummary> TransformSummaries(IEnumerable<AmazonAdGroupSummary> adGroupStats, DateTime date)
        {
            var notEmptyStats = adGroupStats.Where(x => !x.AllZeros()).ToList();
            var summaries = notEmptyStats.Select(stat => CreateSummary(stat, date));
            return summaries.ToList();
        }

        private AdSetSummary CreateSummary(AmazonAdGroupSummary adGroupStat, DateTime date)
        {
            var sum = new AdSetSummary
            {
                AdSetEid = adGroupStat.AdGroupId,
                AdSetName = adGroupStat.AdGroupName,
                StrategyEid = adGroupStat.CampaignId,
                StrategyName = adGroupStat.CampaignName
            };
            SetCPProgStats(sum, adGroupStat, date);
            return sum;
        }

        private IEnumerable<int> GetAccountAdSetIds()
        {
            using (var db = new ClientPortalProgContext())
            {
                var ids = db.AdSets.Where(x => x.AccountId == accountId).Select(x => x.Id);
                return ids.ToList();
            }
        }

        private void RemoveOldData(DateTime date, IEnumerable<int> accountAdSetIds)
        {
            using (var db = new ClientPortalProgContext())
            {
                var items = db.AdSetSummaries.Where(x => x.Date == date && accountAdSetIds.Contains(x.AdSetId)).ToList();
                var metrics = db.AdSetSummaryMetrics.Where(x => x.Date == date && accountAdSetIds.Contains(x.EntityId)).ToList();
                db.AdSetSummaryMetrics.RemoveRange(metrics);
                db.AdSetSummaries.RemoveRange(items);
                var numChanges = SafeContextWrapper.TrySaveChanges(db);
                Logger.Info(accountId, "{0} - AdSetSummaries for account ({1}) was cleaned. Count of deleted objects: {2}", date, accountId, numChanges);
            }
        }
    }
}
