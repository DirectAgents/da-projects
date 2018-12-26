using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter;
using CakeExtracter.Common;
using CakeExtracter.Etl.TradingDesk.Extracters.AmazonExtractors;
using CakeExtractor.SeleniumApplication.Enums;
using CakeExtractor.SeleniumApplication.Models;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.AmazonReportCsvExtractors;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtractor.SeleniumApplication.SeleniumExtractors.AmazonPdaExtractors
{
    class AmazonPdaCampaignExtractor : BaseAmazonExtractor<StrategySummary>
    {
        public readonly AmazonPdaExtractor PdaExtractor;

        public AmazonPdaCampaignExtractor(ExtAccount account, DateRange dateRange) : base(null, dateRange, account)
        {
            PdaExtractor = new AmazonPdaExtractor(account);
        }

        protected override void Extract()
        {
            Logger.Info(accountId, "Extracting CampaignSummaries (PDA) from Amazon Platform for ({0}) from {1:d} to {2:d}",
                clientId, dateRange.FromDate, dateRange.ToDate);
            PdaExtractor.Extract(ExtractCampaignSummaries);
            End();
        }

        public IEnumerable<StrategySummary> ExtractCampaignDailySummaries(string campaignUrl, int campaignNumber)
        {
            try
            {
                Logger.Info(accountId, "Retrieving information about campaign [{0}]...", campaignNumber);
                return RetrieveSummaries(campaignUrl);
            }
            catch (Exception exc)
            {
                Logger.Error(accountId, new Exception($"Processing of campaign info is failed: {exc.Message}", exc));
            }
            return new List<StrategySummary>();
        }

        private void ExtractCampaignSummaries(List<string> campaignsUrls)
        {
            for (var i = 0; i < campaignsUrls.Count; i++)
            {
                var data = ExtractCampaignDailySummaries(campaignsUrls[i], i + 1);
                Add(data);
            }
        }

        private IEnumerable<StrategySummary> RetrieveSummaries(string campaignUrl)
        {
            var campaignInfo = PdaExtractor.ExtractCampaignInfo(campaignUrl, dateRange);
            if (string.IsNullOrEmpty(campaignInfo.ReportPath))
            {
                return new List<StrategySummary>();
            }
            var data = TransformCampaignInfo(campaignInfo);
            return data;
        }

        private IEnumerable<StrategySummary> TransformCampaignInfo(CampaignInfo info)
        {
            var extractor = new AmazonReportCsvExtractor(info.ReportPath);
            var summaries = extractor.EnumerateRows();
            var summariesWithValidDate = summaries.Where(x => dateRange.Dates.Contains(x.Date) && !x.AllZeros());
            var groupedByDateSummaries = summariesWithValidDate.GroupBy(x => x.Date);
            var data = groupedByDateSummaries.Select(x => CreateSummary(x, info, x.Key));
            return data.ToList();
        }

        private StrategySummary CreateSummary(IEnumerable<AmazonReportSummary> stat, CampaignInfo campaign, DateTime date)
        {
            var sum = new StrategySummary
            {
                StrategyEid = campaign.Id,
                StrategyName = campaign.Name,
                StrategyTargetingType = campaign.Targeting,
                StrategyType = campaign.Type
            };
            SetStats(sum, stat, date);
            return sum;
        }

        private void SetStats(StrategySummary sum, IEnumerable<AmazonReportSummary> stat, DateTime date)
        {
            SetCPProgStats(sum, stat, date);
            var reportMetrics = GetReportMetrics(stat, date);
            sum.InitialMetrics = sum.InitialMetrics.Concat(reportMetrics).ToList();
        }

        private IEnumerable<SummaryMetric> GetReportMetrics(IEnumerable<AmazonReportSummary> amazonStats, DateTime date)
        {
            var metrics = new List<SummaryMetric>();
            AddMetric(metrics, AmazonReportMetrics.DetailPageViews, date, amazonStats.Sum(x => x.DetailPageViews));
            AddMetric(metrics, AmazonReportMetrics.UnitsSold, date, amazonStats.Sum(x => x.UnitsSold));
            return metrics;
        }

        private void AddMetric(List<SummaryMetric> metrics, AmazonReportMetrics type, DateTime date, decimal metricValue)
        {
            AddMetric(metrics, type.ToString(), date, metricValue);
        }
    }
}
