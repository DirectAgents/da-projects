﻿using System;
using System.Collections.Generic;
using System.Linq;
using Amazon;
using Amazon.Entities;
using Amazon.Entities.Summaries;
using Amazon.Enums;
using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement;
using CakeExtracter.Helpers;
using CakeExtracter.Logging.TimeWatchers;
using CakeExtracter.Logging.TimeWatchers.Amazon;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.Extracters.AmazonExtractors.AmazonApiExtractors
{
    // Ad (ProductAd)
    public class AmazonApiAdExtrator : BaseAmazonExtractor<TDadSummary>
    {
        private readonly AmazonCampaignMetadataExtractor campaignMetadataExtractor;

        //NOTE: We can only get ad stats for SponsoredProduct campaigns, for these reasons:
        public AmazonApiAdExtrator(AmazonUtility amazonUtility, DateRange dateRange, ExtAccount account, bool clearBeforeLoad, string campaignFilter = null, string campaignFilterOut = null)
            : base(amazonUtility, dateRange, account, clearBeforeLoad, campaignFilter, campaignFilterOut)
        {
            campaignMetadataExtractor = new AmazonCampaignMetadataExtractor(amazonUtility);
        }

        protected override void Extract()
        {
            Logger.Info(accountId, "Extracting TDadSummaries from Amazon API for ({0}) from {1:d} to {2:d}",
                clientId, dateRange.FromDate, dateRange.ToDate);
            var campaignsData = GetCampaignInfo();
            foreach (var date in dateRange.Dates)
            {
                try
                {
                    Extract(date, campaignsData);
                }
                catch (Exception e)
                {
                    Logger.Error(accountId, e);
                }
            }
            End();
        }

        private void Extract(DateTime date, List<AmazonCampaign> campaignInfo)
        {
            CommandExecutionContext.Current?.SetJobExecutionStateInHistory($"Ad Level - {date.ToString()}", accountId);
            IEnumerable<TDadSummary> items = null;
            AmazonTimeTracker.Instance.ExecuteWithTimeTracking(() => {
                var productAdSums = ExtractProductAdSummaries(date);
                var asinSums = ExtractAsinSummaries(date);
                items = TransformSummaries(productAdSums, asinSums, date, campaignInfo);
            }, accountId, AmazonJobLevels.creative, AmazonJobOperations.reportExtracting);
            if (ClearBeforeLoad)
            {
                RemoveOldData(date);
            }
            Add(items);
        }

        private List<AmazonCampaign> GetCampaignInfo()
        {
            List<AmazonCampaign> campaignsData = null;
            AmazonTimeTracker.Instance.ExecuteWithTimeTracking(() =>
            {
                campaignsData = campaignMetadataExtractor.LoadCampaignsMetadata(accountId, clientId).ToList();
            }, accountId, AmazonJobLevels.creative, AmazonJobOperations.loadCampaignMetadata);
            return campaignsData;
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
            IEnumerable<AmazonAsinSummaries> asinStats, DateTime date, List<AmazonCampaign> campaignInfo)
        {
            var summaries = adStats.Select(stat => CreateSummary(stat, asinStats, date, campaignInfo));
            var notEmptySummaries = summaries.Where(x => x != null && !x.AllZeros()).ToList();
            return notEmptySummaries;
        }

        private TDadSummary CreateSummary(AmazonAdDailySummary adStat, IEnumerable<AmazonAsinSummaries> asinStats, 
            DateTime date, List<AmazonCampaign> campaignInfo)
        {
            var asinStatsForProduct = asinStats.Where(x => x.Asin == adStat.Asin && x.AdGroupId == adStat.AdGroupId).ToList();
            if (!asinStatsForProduct.Any() && adStat.AllZeros())
            {
                return null;
            }
            var relatedCampaign = campaignInfo.FirstOrDefault(c => c.CampaignId == adStat.CampaignId);
            var sum = CreateSummary(adStat, date, relatedCampaign);
            AddAsinMetrics(sum, asinStatsForProduct);
            return sum;
        }

        private TDadSummary CreateSummary(AmazonAdDailySummary adSum, DateTime date, AmazonCampaign relatedCampaign)
        {
            var sum = new TDadSummary
            {
                TDadEid = adSum.AdId,
                TDadName = adSum.AdGroupName,
                AdSetEid = adSum.AdGroupId,
                AdSetName = adSum.AdGroupName,
                StrategyEid = adSum.CampaignId,
                StrategyName = adSum.CampaignName,
                StrategyType = adSum.CampaignType,
                StrategyTargetingType = relatedCampaign!=null ? relatedCampaign.TargetingType : null
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
            AmazonTimeTracker.Instance.ExecuteWithTimeTracking(() => 
            {
                SafeContextWrapper.TryMakeTransaction((ClientPortalProgContext db) =>
                {
                    db.TDadSummaryMetrics.Where(x => x.Date == date && x.TDad.AccountId == accountId).DeleteFromQuery();
                    db.TDadSummaries.Where(x => x.Date == date && x.TDad.AccountId == accountId).DeleteFromQuery();
                }, "DeleteFromQuery");
            }, accountId, AmazonJobLevels.creative, AmazonJobOperations.cleanExistingData);
            Logger.Info(accountId, "The cleaning of AdSummaries for account ({0}) is over - {1}.", accountId, date);
        }
    }
}
