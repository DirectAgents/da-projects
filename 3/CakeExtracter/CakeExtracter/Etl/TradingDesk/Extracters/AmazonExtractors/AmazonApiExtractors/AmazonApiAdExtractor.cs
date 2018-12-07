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
    // Ad (ProductAd)
    public class AmazonApiAdExtrater : BaseAmazonExtractor<TDadSummary>
    {
        //NOTE: We can only get ad stats for SponsoredProduct campaigns, for these reasons:
        // - the get-ProductAds call only returns SP ads
        // - for Sponsored Brands reports, recordType call only be campaigns, adGroups or keywords
        // - a productAdId metric is not available anyway

        public AmazonApiAdExtrater(AmazonUtility amazonUtility, DateRange dateRange, ExtAccount account, string campaignFilter = null, string campaignFilterOut = null)
            : base(amazonUtility, dateRange, account, campaignFilter: campaignFilter, campaignFilterOut: campaignFilterOut)
        { }

        protected override void Extract()
        {
            Logger.Info(accountId, "Extracting TDadSummaries from Amazon API for ({0}) from {1:d} to {2:d}",
                this.clientId, this.dateRange.FromDate, this.dateRange.ToDate);

            foreach (var date in dateRange.Dates)
            {
                var sums = ExtractSummaries(date);
                var items = TransformSummaries(sums, date);
                Add(items);
            }
            End();
        }

        private IEnumerable<AmazonAdDailySummary> ExtractSummaries(DateTime date)
        {
            var sums = _amazonUtility.ReportProductAds(date, clientId, true);
            var filteredSums = FilterByCampaigns(sums, x => x.campaignName);
            return filteredSums.ToList();
        }

        private IEnumerable<TDadSummary> TransformSummaries(IEnumerable<AmazonAdDailySummary> adStats, DateTime date)
        {
            adStats = adStats.Where(x => !x.AllZeros()).ToList();
            foreach (var adSum in adStats)
            {
                var sum = new TDadSummary
                {
                    TDadEid = adSum.adId,
                    TDadName = adSum.adGroupName,
                    AdSetEid = adSum.adGroupId,
                    AdSetName = adSum.adGroupName
                };
                var externalIds = new List<TDadExternalId>();
                AddAdExternalId(externalIds, ProductAdIdType.ASIN, adSum.asin);
                //AddAdExternalId(externalIds, ProductAdIdType.SKU, adSum.sku);
                sum.ExternalIds = externalIds;
                SetCPProgStats(sum, adSum, date);
                yield return sum;
            }
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
    }
}
