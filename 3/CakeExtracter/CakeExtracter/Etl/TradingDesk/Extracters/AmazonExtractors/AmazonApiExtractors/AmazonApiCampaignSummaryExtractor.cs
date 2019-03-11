using Amazon;
using Amazon.Entities;
using Amazon.Entities.Summaries;
using Amazon.Enums;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;
using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.Helpers;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using CakeExtracter.Logging.TimeWatchers.Amazon;
using CakeExtracter.Logging.TimeWatchers;

namespace CakeExtracter.Etl.TradingDesk.Extracters.AmazonExtractors.AmazonApiExtractors
{
    //Campaign / Strategy
    public class AmazonApiCampaignSummaryExtractor : BaseAmazonExtractor<StrategySummary>
    {
        private AmazonCampaignMetadataExtractor campaignMetadataExtractor;

        private readonly string[] campaignTypesFromApi =
        {
            AmazonApiHelper.GetCampaignTypeName(CampaignType.SponsoredProducts),
            AmazonApiHelper.GetCampaignTypeName(CampaignType.SponsoredBrands)
        };

        public AmazonApiCampaignSummaryExtractor(AmazonUtility amazonUtility, DateRange dateRange, ExtAccount account, bool clearBeforeLoad, string campaignFilter = null, string campaignFilterOut = null)
            : base(amazonUtility, dateRange, account, clearBeforeLoad, campaignFilter, campaignFilterOut)
        {
            campaignMetadataExtractor = new AmazonCampaignMetadataExtractor(amazonUtility);
        }

        public IEnumerable<AmazonDailySummary> ExtractSummaries(DateTime date)
        {
            var spSums = _amazonUtility.ReportCampaigns(CampaignType.SponsoredProducts, date, clientId, false);
            var sbSums = _amazonUtility.ReportCampaigns(CampaignType.SponsoredBrands, date, clientId, false);
            var sums = spSums.Concat(sbSums);
            return sums.ToList();
        }

        protected override void Extract()
        {
            Logger.Info(accountId, "Extracting StrategySummaries from Amazon API for ({0}) from {1:d} to {2:d}",
                clientId, dateRange.FromDate, dateRange.ToDate);
            var campaigns = campaignMetadataExtractor.LoadCampaignsMetadata(accountId, clientId);
            if (campaigns != null)
            {
                Extract(campaigns);
            }
            End();
        }

        private void Extract(IEnumerable<AmazonCampaign> campaigns)
        {
            foreach (var date in dateRange.Dates)
            {
                try
                {
                    Extract(campaigns, date);
                }
                catch (Exception e)
                {
                    Logger.Error(accountId, e);
                }
            }
        }

        //TODO? Request the SP and HSA reports in parallel... ?Okay for two threads to call Add at the same time?
        //TODO? Do multiple dates in parallel

        private void Extract(IEnumerable<AmazonCampaign> campaigns, DateTime date)
        {
            IEnumerable<StrategySummary> items = null;
            AmazonTimeTracker.Instance.ExecuteWithTimeTracking(() =>
            {
                var sums = ExtractSummaries(date);
                items = TransformSummaries(sums, campaigns, date);
            }, accountId, AmazonJobLevels.strategy, AmazonJobOperations.reportExtracting);
            if (ClearBeforeLoad)
            {
                RemoveOldData(date);
            }
            Add(items);
        }

        private IEnumerable<StrategySummary> TransformSummaries(IEnumerable<AmazonDailySummary> dailyStats, IEnumerable<AmazonCampaign> campaigns, DateTime date)
        {
            var campaignIds = campaigns.Select(x => x.CampaignId).ToList();
            var notEmptyStats = dailyStats.Where(x => campaignIds.Contains(x.CampaignId) && !x.AllZeros());
            var summaries = notEmptyStats.GroupBy(x => x.CampaignId).Select(stat =>
            {
                var campaign = campaigns.First(x => x.CampaignId == stat.Key);
                return CreateSummary(stat, campaign, date);
            });
            return summaries.ToList();
        }

        private StrategySummary CreateSummary(IEnumerable<AmazonDailySummary> stat, AmazonCampaign campaign, DateTime date)
        {
            var sum = new StrategySummary
            {
                StrategyEid = campaign.CampaignId,
                StrategyName = campaign.Name,
                StrategyTargetingType = campaign.TargetingType,
                StrategyType = stat.FirstOrDefault().CampaignType
            };
            SetCPProgStats(sum, stat, date); // most likely there's just one dailyStat in the group, but this covers everything...
            return sum;
        }

        private void RemoveOldData(DateTime date)
        {
            Logger.Info(accountId, "The cleaning of StrategySummaries for account ({0}) has begun - {1}.", accountId, date);
            AmazonTimeTracker.Instance.ExecuteWithTimeTracking(() =>
            {
                SafeContextWrapper.TryMakeTransaction((ClientPortalProgContext db) =>
                {
                    db.StrategySummaryMetrics.Where(x => x.Date == date && x.Strategy.AccountId == accountId && campaignTypesFromApi.Contains(x.Strategy.Type.Name)).DeleteFromQuery();
                    db.StrategySummaries.Where(x => x.Date == date && x.Strategy.AccountId == accountId && campaignTypesFromApi.Contains(x.Strategy.Type.Name)).DeleteFromQuery();
                }, "DeleteFromQuery");
            }, accountId, AmazonJobLevels.strategy, AmazonJobOperations.cleanExistingData);
            Logger.Info(accountId, "The cleaning of StrategySummaries for account ({0}) is over - {1}.", accountId, date);
        }
    }
}
