using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter;
using CakeExtracter.Common;
using CakeExtracter.Etl.TradingDesk.Extracters.AmazonExtractors;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtractor.SeleniumApplication.SeleniumExtractors.AmazonPdaExtractors
{
    class AmazonPdaDailyExtractor : BaseAmazonExtractor<DailySummary>
    {
        private readonly AmazonPdaCampaignExtractor campaignExtractor;

        public AmazonPdaDailyExtractor(ExtAccount account, DateRange dateRange) : base(null, dateRange, account)
        {
            campaignExtractor = new AmazonPdaCampaignExtractor(account, dateRange);
        }

        protected override void Extract()
        {
            Logger.Info(accountId, "Extracting DailySummaries (PDA) from Amazon Platform for ({0}) from {1:d} to {2:d}",
                clientId, dateRange.FromDate, dateRange.ToDate);
            campaignExtractor.PdaExtractor.Extract(ExtractDailySummaries);
        }

        private void ExtractDailySummaries(List<string> campaignsUrls)
        {
            var campaignSummaries = GetCampaignSummaries(campaignsUrls);
            var dailySummaries = TransformCampaignSummariesToDaily(campaignSummaries);
            Add(dailySummaries);
            End();
        }

        private IEnumerable<StrategySummary> GetCampaignSummaries(List<string> campaignsUrls)
        {
            var campaignsSummaries = new List<StrategySummary>();
            for (var i = 0; i < campaignsUrls.Count; i++)
            {
                var campaignSummaries = campaignExtractor.ExtractCampaignDailySummaries(campaignsUrls[i], i + 1);
                campaignsSummaries.AddRange(campaignSummaries);
            }

            return campaignsSummaries;
        }

        private IEnumerable<DailySummary> TransformCampaignSummariesToDaily(IEnumerable<StrategySummary> campaignSummaries)
        {
            var groupedByDateSummaries = campaignSummaries.GroupBy(x => x.Date);
            var dailySummaries = groupedByDateSummaries.Select(x => CreateSummary(x, x.Key));
            return dailySummaries.ToList();
        }

        private DailySummary CreateSummary(IEnumerable<StrategySummary> strategySummaries, DateTime date)
        {
            var dailySummary = new DailySummary();
            dailySummary.SetStats(date, strategySummaries);
            return dailySummary;
        }
    }
}
