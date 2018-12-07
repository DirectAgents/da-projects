using System;
using System.Collections.Generic;
using System.Linq;
using Amazon;
using Amazon.Enums;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;
using StatSummary = Amazon.Entities.StatSummary;

namespace CakeExtracter.Etl.TradingDesk.Extracters.AmazonExtractors
{
    public abstract class BaseAmazonExtractor<T> : Extracter<T>
        where T : DatedStatsSummary
    {
        protected readonly AmazonUtility _amazonUtility;
        protected readonly DateRange dateRange;
        protected readonly int accountId;   // in our db
        protected readonly string clientId; // external id
        protected readonly string campaignFilter;
        protected readonly string campaignFilterOut;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseAmazonExtractor{T}"/> class.
        /// </summary>
        /// <param name="amazonUtility">The amazon utility.</param>
        /// <param name="date">The date.</param>
        /// <param name="clientId">The client identifier.</param>
        public BaseAmazonExtractor(AmazonUtility amazonUtility, DateRange dateRange, ExtAccount account, string campaignFilter = null, string campaignFilterOut = null)
        {
            this._amazonUtility = amazonUtility;
            this.dateRange = dateRange;
            this.accountId = account.Id;
            this.clientId = account.ExternalId;
            this.campaignFilter = campaignFilter;
            this.campaignFilterOut = campaignFilterOut;
        }

        protected IEnumerable<TStat> FilterByCampaigns<TStat>(IEnumerable<TStat> reportEntities, Func<TStat, string> getFilterProp)
        {
            if (!String.IsNullOrEmpty(campaignFilter))
            {
                reportEntities = reportEntities.Where(x => getFilterProp(x).Contains(campaignFilter));
            }
            if (!String.IsNullOrEmpty(campaignFilterOut))
            {
                reportEntities = reportEntities.Where(x => !getFilterProp(x).Contains(campaignFilterOut));
            }
            return reportEntities.ToList();
        }

        protected static void SetCPProgStats(T stats, StatSummary amazonStat, DateTime date)
        {
            SetCPProgStats(stats, new[] { amazonStat }, date);
        }

        protected static void SetCPProgStats(T stats, IEnumerable<StatSummary> amazonStats, DateTime date)
        {
            stats.Date = date;
            if (amazonStats == null || !amazonStats.Any())
            {
                return; //note: not setting stats to 0 if !any
            }

            stats.Cost = amazonStats.Sum(x => x.cost);
            stats.Impressions = amazonStats.Sum(x => x.impressions);
            stats.Clicks = amazonStats.Sum(x => x.clicks);
            stats.PostClickConv = amazonStats.Sum(x => x.attributedConversions14d);
            var rev = stats as DatedStatsSummaryWithRev;
            if (rev != null)
            {
                rev.PostClickRev = amazonStats.Sum(x => x.attributedSales14d);
            }
            stats.InitialMetrics = GetMetrics(amazonStats, date);
        }

        private static List<SummaryMetric> GetMetrics(IEnumerable<StatSummary> amazonStats, DateTime date)
        {
            var metrics = new List<SummaryMetric>();
            AddMetric(metrics, AttributedMetricType.attributedConversions, AttributedMetricDaysInterval.Days1, date, amazonStats.Sum(x => x.attributedConversions1d));
            AddMetric(metrics, AttributedMetricType.attributedConversions, AttributedMetricDaysInterval.Days7, date, amazonStats.Sum(x => x.attributedConversions7d));
            AddMetric(metrics, AttributedMetricType.attributedConversions, AttributedMetricDaysInterval.Days14, date, amazonStats.Sum(x => x.attributedConversions14d));
            AddMetric(metrics, AttributedMetricType.attributedConversions, AttributedMetricDaysInterval.Days30, date, amazonStats.Sum(x => x.attributedConversions30d));
            AddMetric(metrics, AttributedMetricType.attributedConversionsSameSKU, AttributedMetricDaysInterval.Days1, date, amazonStats.Sum(x => x.attributedConversions1dSameSKU));
            AddMetric(metrics, AttributedMetricType.attributedConversionsSameSKU, AttributedMetricDaysInterval.Days7, date, amazonStats.Sum(x => x.attributedConversions7dSameSKU));
            AddMetric(metrics, AttributedMetricType.attributedConversionsSameSKU, AttributedMetricDaysInterval.Days14, date, amazonStats.Sum(x => x.attributedConversions14dSameSKU));
            AddMetric(metrics, AttributedMetricType.attributedConversionsSameSKU, AttributedMetricDaysInterval.Days30, date, amazonStats.Sum(x => x.attributedConversions30dSameSKU));
            AddMetric(metrics, AttributedMetricType.attributedSales, AttributedMetricDaysInterval.Days1, date, amazonStats.Sum(x => x.attributedSales1d));
            AddMetric(metrics, AttributedMetricType.attributedSales, AttributedMetricDaysInterval.Days7, date, amazonStats.Sum(x => x.attributedSales7d));
            AddMetric(metrics, AttributedMetricType.attributedSales, AttributedMetricDaysInterval.Days14, date, amazonStats.Sum(x => x.attributedSales14d));
            AddMetric(metrics, AttributedMetricType.attributedSales, AttributedMetricDaysInterval.Days30, date, amazonStats.Sum(x => x.attributedSales30d));
            AddMetric(metrics, AttributedMetricType.attributedSalesSameSKU, AttributedMetricDaysInterval.Days1, date, amazonStats.Sum(x => x.attributedSales1dSameSKU));
            AddMetric(metrics, AttributedMetricType.attributedSalesSameSKU, AttributedMetricDaysInterval.Days7, date, amazonStats.Sum(x => x.attributedSales7dSameSKU));
            AddMetric(metrics, AttributedMetricType.attributedSalesSameSKU, AttributedMetricDaysInterval.Days14, date, amazonStats.Sum(x => x.attributedSales14dSameSKU));
            AddMetric(metrics, AttributedMetricType.attributedSalesSameSKU, AttributedMetricDaysInterval.Days30, date, amazonStats.Sum(x => x.attributedSales30dSameSKU));
            AddMetric(metrics, AttributedMetricType.attributedUnitsOrdered, AttributedMetricDaysInterval.Days1, date, amazonStats.Sum(x => x.attributedUnitsOrdered1d));
            AddMetric(metrics, AttributedMetricType.attributedUnitsOrdered, AttributedMetricDaysInterval.Days7, date, amazonStats.Sum(x => x.attributedUnitsOrdered7d));
            AddMetric(metrics, AttributedMetricType.attributedUnitsOrdered, AttributedMetricDaysInterval.Days14, date, amazonStats.Sum(x => x.attributedUnitsOrdered14d));
            AddMetric(metrics, AttributedMetricType.attributedUnitsOrdered, AttributedMetricDaysInterval.Days30, date, amazonStats.Sum(x => x.attributedUnitsOrdered30d));
            return metrics;
        }

        private static void AddMetric(List<SummaryMetric> metrics, AttributedMetricType type, AttributedMetricDaysInterval daysInterval, DateTime date, decimal metricValue)
        {
            if (metricValue == 0.0M)
            {
                return;
            }

            var metric = new SummaryMetric()
            {
                Date = date,
                MetricType = new MetricType
                {
                    Name = type.ToString(),
                    DaysInterval = (int)daysInterval
                },
                Value = metricValue
            };
            metrics.Add(metric);
        }
    }
}
