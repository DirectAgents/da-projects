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
    //Campaign / Strategy
    public class AmazonApiCampaignSummaryExtracter : BaseAmazonExtractor<StrategySummary>
    {
        public AmazonApiCampaignSummaryExtracter(AmazonUtility amazonUtility, DateRange dateRange, ExtAccount account, string campaignFilter = null, string campaignFilterOut = null)
            : base(amazonUtility, dateRange, account, campaignFilter: campaignFilter, campaignFilterOut: campaignFilterOut)
        { }

        protected override void Extract()
        {
            Logger.Info(accountId, "Extracting StrategySummaries from Amazon API for ({0}) from {1:d} to {2:d}",
                this.clientId, this.dateRange.FromDate, this.dateRange.ToDate);

            var campaigns = LoadCampaignsFromAmazonApi();
            foreach (var date in dateRange.Dates)
            {
                var sums = ExtractSummaries(date);
                var items = TransformSummaries(sums, campaigns, date);
                Add(items);
            }
            End();
        }

        //TODO? Request the SP and HSA reports in parallel... ?Okay for two threads to call Add at the same time?
        //TODO? Do multiple dates in parallel

        private IEnumerable<AmazonCampaign> LoadCampaignsFromAmazonApi()
        {
            var spCampaigns = _amazonUtility.GetCampaigns(CampaignType.SponsoredProducts, clientId);
            var sbCampaigns = _amazonUtility.GetCampaigns(CampaignType.SponsoredBrands, clientId);
            var campaigns = spCampaigns.Concat(sbCampaigns);
            var filteredCampaigns = FilterByCampaigns(campaigns, x => x.name);
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
            var campaignIds = campaigns.Select(x => x.campaignId);
            dailyStats = dailyStats.Where(x => campaignIds.Contains(x.campaignId) && !x.AllZeros());
            var groupedStats = dailyStats.GroupBy(x => x.campaignId);
            foreach (var stat in groupedStats)
            {
                var campaign = campaigns.First(x => x.campaignId == stat.Key);
                var sum = new StrategySummary
                {
                    StrategyEid = campaign.campaignId,
                    StrategyName = campaign.name,
                    StrategyType = campaign.targetingType
                };
                SetCPProgStats(sum, stat, date); // most likely there's just one dailyStat in the group, but this covers everything...
                yield return sum;
            }
        }
    }
}
