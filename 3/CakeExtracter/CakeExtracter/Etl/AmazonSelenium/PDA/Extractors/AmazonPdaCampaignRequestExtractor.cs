using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.Enums;
using Amazon.Helpers;
using CakeExtracter.Common;
using CakeExtracter.Etl.AmazonSelenium.PDA.Enums;
using CakeExtracter.Etl.AmazonSelenium.PDA.Models;
using SeleniumDataBrowser.PDA.Exceptions;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Contexts;
using CakeExtracter.Helpers;
using SeleniumDataBrowser.PDA;

namespace CakeExtracter.Etl.AmazonSelenium.PDA.Extractors
{
    /// <inheritdoc cref="AmazonPdaExtractor{T}"/>
    /// <summary>
    /// Campaigns data extractor for Amazon PDA stats.
    /// </summary>
    internal class AmazonPdaCampaignRequestExtractor : AmazonPdaExtractor<StrategySummary>
    {
        private readonly string[] campaignTypesFromApi =
        {
            AmazonApiHelper.GetCampaignTypeName(CampaignType.ProductDisplay),
        };

        /// <inheritdoc/>
        public override string SummariesDisplayName { get; } = "Pda Campaign Summaries";

        /// <inheritdoc cref="AmazonPdaExtractor{T}"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonPdaCampaignRequestExtractor"/> class.
        /// </summary>
        public AmazonPdaCampaignRequestExtractor(
            ExtAccount account,
            DateRange dateRange,
            AmazonConsoleManagerUtility amazonPdaUtility)
            : base(account, dateRange, amazonPdaUtility)
        {
        }

        /// <inheritdoc/>
        /// <summary>
        /// Extracts campaign stats using HTTP requests from Amazon Platform.
        /// </summary>
        protected override void Extract()
        {
            Logger.Info(
                accountId,
                "Extracting CampaignSummaries (PDA) using HTTP requests from Amazon Platform for ({0}) from {1:d} to {2:d}",
                clientId,
                dateRange.FromDate,
                dateRange.ToDate);
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
                throw new Exception("Could not extract CampaignSummaries (PDA).", e);
            }
            End();
        }

        private IEnumerable<AmazonPdaCampaignSummary> ExtractSummaries()
        {
            var sums = ExtractPdaCampaignSummaries();
            var filteredSums = FilterByCampaigns(sums, x => x.Name);
            return filteredSums;
        }

        private IEnumerable<StrategySummary> TransformSummaries(IEnumerable<AmazonPdaCampaignSummary> dailyStats)
        {
            var notEmptyStats = dailyStats.Where(x => !x.AllZeros());
            var statsGroupsByIdAndDate = notEmptyStats.GroupBy(x => new { x.Id, x.Date });
            var summaries = statsGroupsByIdAndDate.Select(x => CreateSummary(x, x.Key.Date));
            return summaries.ToList();
        }

        private StrategySummary CreateSummary(IEnumerable<AmazonPdaCampaignSummary> summaries, DateTime date)
        {
            var campaignSummaries = summaries.ToList();
            var campaign = campaignSummaries.First();
            var sum = new StrategySummary
            {
                StrategyEid = campaign.Id,
                StrategyName = campaign.Name,
                StrategyTargetingType = campaign.TargetingType,
                StrategyType = campaign.Type,
            };
            SetStats(sum, campaignSummaries, date);
            return sum;
        }

        private void SetStats(StrategySummary sum, IEnumerable<AmazonPdaCampaignSummary> summaries, DateTime date)
        {
            var campaignSummaries = summaries.ToList();
            SetCPProgStats(sum, campaignSummaries, date);
            var reportMetrics = GetReportMetrics(campaignSummaries, date);
            sum.InitialMetrics = sum.InitialMetrics.Concat(reportMetrics).ToList();
        }

        private IEnumerable<SummaryMetric> GetReportMetrics(IEnumerable<AmazonPdaCampaignSummary> summaries, DateTime date)
        {
            var campaignSummaries = summaries.ToList();
            var metrics = new List<SummaryMetric>();
            var detailPageViewsSum = campaignSummaries.Sum(x => x.DetailPageViews);
            var unitsSoldSum = campaignSummaries.Sum(x => x.UnitsSold);
            var ordersSum = campaignSummaries.Sum(x => x.Orders);
            AddMetric(metrics, AmazonReportMetrics.DetailPageViews, date, detailPageViewsSum);
            AddMetric(metrics, AmazonReportMetrics.UnitsSold, date, unitsSoldSum);
            AddMetric(metrics, AmazonCmApiMetrics.Orders, date, ordersSum);
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
            Logger.Info(
                accountId,
                $"The cleaning of StrategySummaries for account ({accountId}) has begun - from {fromDate} to {toDate}.");
            SafeContextWrapper.TryMakeTransaction(
                (ClientPortalProgContext db) =>
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
            Logger.Info(
                accountId,
                $"The cleaning of StrategySummaries for account ({accountId}) is over - from {fromDate} to {toDate}.");
        }
    }
}
