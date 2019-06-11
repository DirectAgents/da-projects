using System;
using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Entities.CPProg;
using CakeExtracter.Common;
using SeleniumDataBrowser.PDA.Exceptions;
using CakeExtracter.Etl.AmazonSelenium.PDA.Enums;
using CakeExtracter.Etl.AmazonSelenium.PDA.Models;
using SeleniumDataBrowser.PDA;

namespace CakeExtracter.Etl.AmazonSelenium.PDA.Extractors
{
    /// <inheritdoc cref="AmazonPdaExtractor{T}"/>
    /// <summary>
    /// Daily data extractor for Amazon PDA stats (from Campaign data).
    /// </summary>
    internal class AmazonPdaDailyRequestExtractor : AmazonPdaExtractor<DailySummary>
    {
        /// <inheritdoc cref="AmazonPdaExtractor{T}"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonPdaDailyRequestExtractor"/> class.
        /// </summary>
        public AmazonPdaDailyRequestExtractor(ExtAccount account, DateRange dateRange, AmazonConsoleManagerUtility amazonPdaUtility)
            : base(account, dateRange, amazonPdaUtility)
        {
        }

        /// <inheritdoc/>
        /// <summary>
        /// Extracts daily stats using HTTP requests from Amazon Platform.
        /// </summary>
        protected override void Extract()
        {
            Logger.Info(accountId, "Extracting DailySummaries (PDA) using HTTP requests from Amazon Platform for ({0}) from {1:d} to {2:d}",
                clientId, dateRange.FromDate, dateRange.ToDate);
            try
            {
                var sums = ExtractSummaries();
                var items = TransformSummaries(sums);
                Add(items);
            }
            catch (AccountDoesNotHaveProfileException ex)
            {
                Logger.Warn(accountId, ex.Message);
            }
            catch (Exception e)
            {
                var exception = new Exception("Could not extract DailySummaries (PDA).", e);
                throw exception;
            }
            End();
        }

        private IEnumerable<AmazonPdaCampaignSummary> ExtractSummaries()
        {
            var sums = ExtractPdaCampaignSummaries();
            var filteredSums = FilterByCampaigns(sums, x => x.Name);
            return filteredSums;
        }

        private IEnumerable<DailySummary> TransformSummaries(IEnumerable<AmazonPdaCampaignSummary> dailyStats)
        {
            var notEmptyStats = dailyStats.Where(x => !x.AllZeros());
            var summaries = notEmptyStats.GroupBy(x => x.Date).Select(x => CreateSummary(x, x.Key.Date));
            return summaries.ToList();
        }

        private DailySummary CreateSummary(IEnumerable<AmazonPdaCampaignSummary> campaignSummaries, DateTime date)
        {
            var sum = new DailySummary();
            SetStats(sum, campaignSummaries, date);
            return sum;
        }

        private void SetStats(DailySummary sum, IEnumerable<AmazonPdaCampaignSummary> campaignSummaries, DateTime date)
        {
            SetCPProgStats(sum, campaignSummaries, date);
            var reportMetrics = GetReportMetrics(campaignSummaries, date);
            sum.InitialMetrics = sum.InitialMetrics.Concat(reportMetrics).ToList();
        }

        private IEnumerable<SummaryMetric> GetReportMetrics(IEnumerable<AmazonPdaCampaignSummary> campaignSummaries, DateTime date)
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
    }
}
