using System;
using System.Collections.Generic;
using System.Linq;
using Amazon;
using Amazon.Entities.Summaries;
using Amazon.Enums;
using CakeExtracter.Common;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.Extracters.AmazonExtractors.AmazonApiExtractors
{
    // Ad (ProductAd)
    public class AmazonApiAdExtrator : BaseAmazonExtractor<TDadSummary>
    {
        //NOTE: We can only get ad stats for SponsoredProduct campaigns, for these reasons:
        // - the get-ProductAds call only returns SP ads
        // - for Sponsored Brands reports, recordType call only be campaigns, adGroups or keywords
        // - a productAdId metric is not available anyway

        public AmazonApiAdExtrator(AmazonUtility amazonUtility, DateRange dateRange, ExtAccount account, bool clearBeforeLoad, string campaignFilter = null, string campaignFilterOut = null)
            : base(amazonUtility, dateRange, account, clearBeforeLoad, campaignFilter, campaignFilterOut)
        { }

        protected override void Extract()
        {
            Logger.Info(accountId, "Extracting TDadSummaries from Amazon API for ({0}) from {1:d} to {2:d}",
                clientId, dateRange.FromDate, dateRange.ToDate);

            var accountAdIds = GetAccountAdIds();
            foreach (var date in dateRange.Dates)
            {
                Extract(accountAdIds, date);
            }
            End();
        }

        private void Extract(IEnumerable<int> accountAdIds, DateTime date)
        {
            var sums = ExtractSummaries(date);
            var items = TransformSummaries(sums, date);
            if (ClearBeforeLoad)
            {
                RemoveOldData(date, accountAdIds);
            }

            Add(items);
        }

        private IEnumerable<AmazonAdDailySummary> ExtractSummaries(DateTime date)
        {
            var sums = _amazonUtility.ReportProductAds(date, clientId, true);
            var filteredSums = FilterByCampaigns(sums, x => x.CampaignName);
            return filteredSums.ToList();
        }

        private IEnumerable<TDadSummary> TransformSummaries(IEnumerable<AmazonAdDailySummary> adStats, DateTime date)
        {
            var notEmptyStats = adStats.Where(x => !x.AllZeros()).ToList();
            var summaries = notEmptyStats.Select(stat => CreateSummary(stat, date));
            return summaries.ToList();
        }

        private TDadSummary CreateSummary(AmazonAdDailySummary adSum, DateTime date)
        {
            var sum = new TDadSummary
            {
                TDadEid = adSum.AdId,
                TDadName = adSum.AdGroupName,
                AdSetEid = adSum.AdGroupId,
                AdSetName = adSum.AdGroupName
            };
            var externalIds = new List<TDadExternalId>();
            AddAdExternalId(externalIds, ProductAdIdType.ASIN, adSum.Asin);
            //AddAdExternalId(externalIds, ProductAdIdType.SKU, adSum.sku);
            sum.ExternalIds = externalIds;
            SetCPProgStats(sum, adSum, date);
            return sum;
        }

        private void AddAdExternalId(List<TDadExternalId> ids, ProductAdIdType type, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            var id = new TDadExternalId
            {
                Type = new EntityType { Name = type.ToString() },
                ExternalId = value
            };
            ids.Add(id);
        }

        private IEnumerable<int> GetAccountAdIds()
        {
            using (var db = new ClientPortalProgContext())
            {
                var ids = db.TDads.Where(x => x.AccountId == accountId).Select(x => x.Id);
                return ids.ToList();
            }
        }

        private void RemoveOldData(DateTime date, IEnumerable<int> accountAdIds)
        {
            using (var db = new ClientPortalProgContext())
            {
                var items = db.TDadSummaries.Where(x => x.Date == date && accountAdIds.Contains(x.TDadId)).ToList();
                var metrics = db.TDadSummaryMetrics.Where(x => x.Date == date && accountAdIds.Contains(x.EntityId)).ToList();
                db.TDadSummaryMetrics.RemoveRange(metrics);
                db.TDadSummaries.RemoveRange(items);
                var numChanges = SafeContextWrapper.TrySaveChanges(db);
                Logger.Info(accountId, "{0} - AdSummaries for account ({1}) was cleaned. Count of deleted objects: {2}", date, accountId, numChanges);
            }
        }
    }
}
