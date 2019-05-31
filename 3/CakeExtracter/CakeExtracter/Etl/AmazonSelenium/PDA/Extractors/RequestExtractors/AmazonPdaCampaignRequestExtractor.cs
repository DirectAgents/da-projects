using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.Enums;
using Amazon.Helpers;
using CakeExtracter.Common;
using CakeExtracter.Etl.TradingDesk.Extracters.AmazonExtractors;
using CakeExtracter.Etl.AmazonSelenium.PDA.Enums;
using CakeExtracter.Etl.AmazonSelenium.PDA.Exceptions;
using CakeExtracter.Etl.AmazonSelenium.PDA.Models.ConsoleManagerUtilityModels;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Contexts;
using CakeExtracter.Helpers;

namespace CakeExtracter.Etl.AmazonSelenium.PDA.Extractors.RequestExtractors
{
    internal class AmazonPdaCampaignRequestExtractor : BaseAmazonExtractor<StrategySummary>
    {
        public readonly AmazonPdaExtractor PdaExtractor;

        private readonly string[] campaignTypesFromApi =
        {
            AmazonApiHelper.GetCampaignTypeName(CampaignType.ProductDisplay)
        };

        public AmazonPdaCampaignRequestExtractor(ExtAccount account, DateRange dateRange) : base(null, dateRange, account)
        {
            PdaExtractor = new AmazonPdaExtractor(account);
        }

        protected override void Extract()
        {
            Logger.Info(accountId, "Extracting CampaignSummaries (PDA) using HTTP requests from Amazon Platform for ({0}) from {1:d} to {2:d}",
                clientId, dateRange.FromDate, dateRange.ToDate);
            try
            {
                var sums = ExtractSummaries();
                var items = TransformSummaries(sums);
                RemoveOldData(dateRange.FromDate, dateRange.ToDate);
                Add(items);
            }
            catch (AccountDoesNotHaveProfileException ex)
            {
                Logger.Warn(accountId, ex.Message);
            }
            catch (Exception e)
            {
                Logger.Error(new Exception("Could not extract CampaignSummaries (PDA).", e));
            }
            End();
        }

        private IEnumerable<AmazonCmApiCampaignSummary> ExtractSummaries()
        {
            var sums = PdaExtractor.ExtractCampaignApiFullSummaries(dateRange);
            var filteredSums = FilterByCampaigns(sums, x => x.Name);
            return filteredSums;
        }

        private IEnumerable<StrategySummary> TransformSummaries(IEnumerable<AmazonCmApiCampaignSummary> dailyStats)
        {
            var notEmptyStats = dailyStats.Where(x => !x.AllZeros());
            var summaries = notEmptyStats.GroupBy(x => new {x.Id, x.Date}).Select(x => CreateSummary(x, x.Key.Date));
            return summaries.ToList();
        }

        private StrategySummary CreateSummary(IEnumerable<AmazonCmApiCampaignSummary> campaignSummaries, DateTime date)
        {
            var campaign = campaignSummaries.First();
            var sum = new StrategySummary
            {
                StrategyEid = campaign.Id,
                StrategyName = campaign.Name,
                StrategyTargetingType = campaign.TargetingType,
                StrategyType = campaign.Type
            };
            SetStats(sum, campaignSummaries, date);
            return sum;
        }

        private void SetStats(StrategySummary sum, IEnumerable<AmazonCmApiCampaignSummary> campaignSummaries, DateTime date)
        {
            SetCPProgStats(sum, campaignSummaries, date);
            var reportMetrics = GetReportMetrics(campaignSummaries, date);
            sum.InitialMetrics = sum.InitialMetrics.Concat(reportMetrics).ToList();
        }

        private IEnumerable<SummaryMetric> GetReportMetrics(IEnumerable<AmazonCmApiCampaignSummary> campaignSummaries, DateTime date)
        {
            var metrics = new List<SummaryMetric>();
            AddMetric(metrics, AmazonReportMetrics.DetailPageViews, date, campaignSummaries.Sum(x => x.DetailPageViews));
            AddMetric(metrics, AmazonReportMetrics.UnitsSold, date, campaignSummaries.Sum(x => x.UnitsSold));
            AddMetric(metrics, AmazonCmApiMetrics.Orders, date, campaignSummaries.Sum(x => x.Orders));
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

        private void RemoveOldData(DateTime fromDate, DateTime toDate)
        {
            Logger.Info(accountId, $"The cleaning of StrategySummaries for account ({accountId}) has begun - from {fromDate} to {toDate}.");

            SafeContextWrapper.TryMakeTransaction((ClientPortalProgContext db) =>
            {
                db.StrategySummaryMetrics.Where(x =>
                    x.Date >= fromDate && 
                    x.Date <= toDate && 
                    x.Strategy.AccountId == accountId &&
                    campaignTypesFromApi.Contains(x.Strategy.Type.Name)).DeleteFromQuery();
                db.StrategySummaries.Where(x =>
                    x.Date >= fromDate && 
                    x.Date <= toDate && 
                    x.Strategy.AccountId == accountId &&
                    campaignTypesFromApi.Contains(x.Strategy.Type.Name)).DeleteFromQuery();
            }, "DeleteFromQuery");

            Logger.Info(accountId, $"The cleaning of StrategySummaries for account ({accountId}) is over - from {fromDate} to {toDate}.");
        }
    }
}
