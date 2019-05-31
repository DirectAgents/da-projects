﻿using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Common;
using CakeExtracter.Etl.TradingDesk.Extracters.AmazonExtractors;
using CakeExtracter.Etl.AmazonSelenium.PDA.Enums;
using CakeExtracter.Etl.AmazonSelenium.PDA.Models;
using CakeExtracter.Etl.AmazonSelenium.PDA.Models.ConsoleManagerUtilityModels;
using CakeExtracter.Etl.AmazonSelenium.PDA.Models.ReportModels;
using CakeExtracter.Etl.AmazonSelenium.PDA.Extractors.CsvExtractors;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.AmazonSelenium.PDA.Extractors
{
    internal class AmazonPdaCampaignExtractor : BaseAmazonExtractor<StrategySummary>
    {
        public readonly AmazonPdaExtractor PdaExtractor;

        public AmazonPdaCampaignExtractor(ExtAccount account, DateRange dateRange) 
            : base(null, dateRange, account)
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

        public IEnumerable<StrategySummary> ExtractCampaignDailySummaries(IEnumerable<AmazonCmApiCampaignSummary> campaignsInfo, string campaignUrl, int campaignNumber)
        {
            try
            {
                Logger.Info(accountId, "Retrieving information about campaign [{0}]...", campaignNumber);
                return RetrieveSummaries(campaignsInfo, campaignUrl);
            }
            catch (Exception exc)
            {
                Logger.Error(accountId, new Exception($"Processing of campaign info is failed: {exc.Message}", exc));
            }
            return new List<StrategySummary>();
        }

        private void ExtractCampaignSummaries(List<string> campaignsUrls)
        {
            var campaignsSummaries = PdaExtractor.ExtractCampaignApiTruncatedSummaries(dateRange);
            for (var i = 0; i < campaignsUrls.Count; i++)
            {
                var data = ExtractCampaignDailySummaries(campaignsSummaries, campaignsUrls[i], i + 1);
                Add(data);
            }
        }

        private IEnumerable<StrategySummary> RetrieveSummaries(IEnumerable<AmazonCmApiCampaignSummary> campaignsSummaries, string campaignUrl)
        {
            var campaignInfo = PdaExtractor.ExtractCampaignInfo(campaignUrl, dateRange);
            if (string.IsNullOrEmpty(campaignInfo.ReportPath))
            {
                return new List<StrategySummary>();
            }

            var campaignSummaries = campaignsSummaries.Where(x => x.Id == campaignInfo.Id);
            var data = TransformCampaignInfo(campaignInfo, campaignSummaries);
            return data;
        }

        private IEnumerable<StrategySummary> TransformCampaignInfo(CampaignInfo info, IEnumerable<AmazonCmApiCampaignSummary> campaignSummaries)
        {
            var extractor = new AmazonReportCsvExtractor(info.ReportPath);
            var summaries = extractor.EnumerateRows();
            var summariesWithValidDate = summaries.Where(x => dateRange.Dates.Contains(x.Date) && !x.AllZeros());
            var groupedByDateSummaries = summariesWithValidDate.GroupBy(x => x.Date);
            var data = groupedByDateSummaries.Select(x => CreateSummary(x, campaignSummaries, info, x.Key));
            return data.ToList();
        }

        private StrategySummary CreateSummary(IEnumerable<AmazonReportSummary> stat, IEnumerable<AmazonCmApiCampaignSummary> campaignSummaries, CampaignInfo campaign, DateTime date)
        {
            var apiSummaryForDate = campaignSummaries.FirstOrDefault(x => x.Date == date);
            var sum = CreateSummary(campaign, date);
            SetStats(sum, stat, apiSummaryForDate, date);
            return sum;
        }

        private StrategySummary CreateSummary(CampaignInfo campaign, DateTime date)
        {
            var sum = new StrategySummary
            {
                StrategyEid = campaign.Id,
                StrategyName = campaign.Name,
                StrategyTargetingType = campaign.Targeting,
                StrategyType = campaign.Type
            };
            return sum;
        }

        private void SetStats(StrategySummary sum, IEnumerable<AmazonReportSummary> stat, AmazonCmApiCampaignSummary campaignApiSummaries, DateTime date)
        {
            SetCPProgStats(sum, stat, date);
            var reportMetrics = GetReportMetrics(stat, campaignApiSummaries, date);
            sum.InitialMetrics = sum.InitialMetrics.Concat(reportMetrics).ToList();
        }

        private IEnumerable<SummaryMetric> GetReportMetrics(IEnumerable<AmazonReportSummary> amazonStats, AmazonCmApiCampaignSummary campaignApiSummaries, DateTime date)
        {
            var metrics = new List<SummaryMetric>();
            AddMetric(metrics, AmazonReportMetrics.DetailPageViews, date, amazonStats.Sum(x => x.DetailPageViews));
            AddMetric(metrics, AmazonReportMetrics.UnitsSold, date, amazonStats.Sum(x => x.UnitsSold));
            AddMetric(metrics, AmazonCmApiMetrics.Orders, date, campaignApiSummaries.Orders);
            return metrics;
        }

        private void AddMetric(List<SummaryMetric> metrics, AmazonReportMetrics type, DateTime date, decimal metricValue)
        {
            AddMetric(metrics, type.ToString(), date, metricValue);
        }

        private void AddMetric(List<SummaryMetric> metrics, AmazonCmApiMetrics type, DateTime date, decimal metricValue)
        {
            AddMetric(metrics, type.ToString(), date, metricValue);
        }
    }
}
