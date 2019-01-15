﻿using System;
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

        public AmazonApiAdExtrator(AmazonUtility amazonUtility, DateRange dateRange, ExtAccount account, bool clearBeforeLoad, string campaignFilter = null, string campaignFilterOut = null)
            : base(amazonUtility, dateRange, account, clearBeforeLoad, campaignFilter, campaignFilterOut)
        { }

        protected override void Extract()
        {
            Logger.Info(accountId, "Extracting TDadSummaries from Amazon API for ({0}) from {1:d} to {2:d}",
                clientId, dateRange.FromDate, dateRange.ToDate);
            
            foreach (var date in dateRange.Dates)
            {
                Extract(date);
            }
            End();
        }

        private void Extract(DateTime date)
        {
            var productAdSums = ExtractProductAdSummaries(date);
            var asinSums = ExtractAsinSummaries(date);
            var items = TransformSummaries(productAdSums, asinSums, date);
            if (ClearBeforeLoad)
            {
                RemoveOldData(date);
            }

            Add(items);
        }

        private IEnumerable<AmazonAdDailySummary> ExtractProductAdSummaries(DateTime date)
        {
            var sums = _amazonUtility.ReportProductAds(date, clientId, true);
            var filteredSums = FilterByCampaigns(sums, x => x.CampaignName);
            return filteredSums.ToList();
        }

        private IEnumerable<AmazonAsinSummaries> ExtractAsinSummaries(DateTime date)
        {
            var sums = _amazonUtility.ReportAsins(date, clientId);
            return sums.ToList();
        }

        private IEnumerable<TDadSummary> TransformSummaries(IEnumerable<AmazonAdDailySummary> adStats,
            IEnumerable<AmazonAsinSummaries> asinStats, DateTime date)
        {
            var notEmptyStats = adStats.Where(x => !x.AllZeros()).ToList();
            var summaries = notEmptyStats.Select(stat => CreateSummary(stat, asinStats, date));
            return summaries.ToList();
        }

        private TDadSummary CreateSummary(AmazonAdDailySummary adStat, IEnumerable<AmazonAsinSummaries> asinStats, DateTime date)
        {
            var sum = CreateSummary(adStat, date);
            var asinStatsForProduct = asinStats.Where(x => x.Asin == adStat.Asin && x.AdGroupId == adStat.AdGroupId).ToList();
            AddAsinMetrics(sum, asinStatsForProduct);
            return sum;
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

        private void RemoveOldData(DateTime date)
        {
            Logger.Info(accountId, "The cleaning of AdSummaries for account ({0}) has begun - {1}.", accountId, date);
            using (var db = new ClientPortalProgContext())
            {
                var items = db.TDadSummaries.Where(x => x.Date == date && x.TDad.AccountId == accountId);
                var metrics = db.TDadSummaryMetrics.Where(x => x.Date == date && x.TDad.AccountId == accountId);
                db.TDadSummaryMetrics.RemoveRange(metrics);
                db.TDadSummaries.RemoveRange(items);
                var numChanges = SafeContextWrapper.TrySaveChanges(db);
                Logger.Info(accountId, "The cleaning of AdSummaries for account ({0}) is over - {1}. Count of deleted objects: {2}", accountId, date, numChanges);
            }
        }
    }
}
