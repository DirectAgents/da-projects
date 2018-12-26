using Amazon;
using Amazon.Entities;
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
    //Campaign / Strategy
    public class AmazonApiCampaignSummaryExtractor : BaseAmazonExtractor<StrategySummary>
    {
        public AmazonApiCampaignSummaryExtractor(AmazonUtility amazonUtility, DateRange dateRange, ExtAccount account, bool clearBeforeLoad, string campaignFilter = null, string campaignFilterOut = null)
            : base(amazonUtility, dateRange, account, clearBeforeLoad, campaignFilter, campaignFilterOut)
        { }

        protected override void Extract()
        {
            Logger.Info(accountId, "Extracting StrategySummaries from Amazon API for ({0}) from {1:d} to {2:d}",
                clientId, dateRange.FromDate, dateRange.ToDate);

            var accountStrategyIds = GetAccountStrategyIds();
            var campaigns = LoadCampaignsFromAmazonApi();
            foreach (var date in dateRange.Dates)
            {
                Extract(campaigns, accountStrategyIds, date);
            }
            End();
        }

        //TODO? Request the SP and HSA reports in parallel... ?Okay for two threads to call Add at the same time?
        //TODO? Do multiple dates in parallel

        private void Extract(IEnumerable<AmazonCampaign> campaigns, IEnumerable<int> accountStrategyIds, DateTime date)
        {
            var sums = ExtractSummaries(date);
            var items = TransformSummaries(sums, campaigns, date);
            if (ClearBeforeLoad)
            {
                RemoveOldData(date, accountStrategyIds);
            }

            Add(items);
        }

        public IEnumerable<AmazonCampaign> LoadCampaignsFromAmazonApi()
        {
            var spCampaigns = _amazonUtility.GetCampaigns(CampaignType.SponsoredProducts, clientId);
            var sbCampaigns = _amazonUtility.GetCampaigns(CampaignType.SponsoredBrands, clientId);
            var campaigns = spCampaigns.Concat(sbCampaigns);
            var filteredCampaigns = FilterByCampaigns(campaigns, x => x.Name);
            return filteredCampaigns.ToList();
        }

        public IEnumerable<AmazonDailySummary> ExtractSummaries(DateTime date)
        {
            var spSums = _amazonUtility.ReportCampaigns(CampaignType.SponsoredProducts, date, clientId, false);
            var sbSums = _amazonUtility.ReportCampaigns(CampaignType.SponsoredBrands, date, clientId, false);
            var sums = spSums.Concat(sbSums);
            return sums.ToList();
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

        private IEnumerable<int> GetAccountStrategyIds()
        {
            using (var db = new ClientPortalProgContext())
            {
                var ids = db.Strategies.Where(x => x.AccountId == accountId).Select(x => x.Id);
                return ids.ToList();
            }
        }

        private void RemoveOldData(DateTime date, IEnumerable<int> accountStrategyIds)
        {
            using (var db = new ClientPortalProgContext())
            {
                var items = db.StrategySummaries.Where(x => x.Date == date && accountStrategyIds.Contains(x.StrategyId)).ToList();
                var metrics = db.StrategySummaryMetrics.Where(x => x.Date == date && accountStrategyIds.Contains(x.EntityId)).ToList();
                db.StrategySummaryMetrics.RemoveRange(metrics);
                db.StrategySummaries.RemoveRange(items);
                var numChanges = SafeContextWrapper.TrySaveChanges(db);
                Logger.Info(accountId, "{0} - StrategySummaries for account ({1}) was cleaned. Count of deleted objects: {2}", date, accountId, numChanges);
            }
        }
    }
}
