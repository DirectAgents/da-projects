using System;
using System.Collections.Generic;
using System.Linq;
using Amazon;
using Amazon.Entities.Summaries;
using Amazon.Enums;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.Extracters.AmazonExtractors
{
    public abstract class BaseAmazonExtractor<T> : Extracter<T>
        where T : DatedStatsSummary
    {
        private const int collectionBoundedCapacity = 20000;

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
        protected BaseAmazonExtractor(AmazonUtility amazonUtility, DateRange dateRange, ExtAccount account,
            string campaignFilter = null, string campaignFilterOut = null)
            : base(collectionBoundedCapacity)
        {
            _amazonUtility = amazonUtility;
            this.dateRange = dateRange;
            this.accountId = account.Id;
            this.clientId = account.ExternalId;
            this.campaignFilter = campaignFilter;
            this.campaignFilterOut = campaignFilterOut;
        }

        protected IEnumerable<TStat> FilterByCampaigns<TStat>(IEnumerable<TStat> reportEntities, Func<TStat, string> getFilterProp)
        {
            if (!string.IsNullOrEmpty(campaignFilter))
            {
                reportEntities = reportEntities.Where(x => getFilterProp(x).Contains(campaignFilter));
            }
            if (!string.IsNullOrEmpty(campaignFilterOut))
            {
                reportEntities = reportEntities.Where(x => !getFilterProp(x).Contains(campaignFilterOut));
            }
            return reportEntities.ToList();
        }

        protected static void SetCPProgStats(T stats, AmazonStatSummary amazonStat, DateTime date)
        {
            SetCPProgStats(stats, new[] { amazonStat }, date);
        }

        protected static void SetCPProgStats(T stats, IEnumerable<AmazonStatSummary> amazonStats, DateTime date)
        {
            stats.Date = date;
            if (amazonStats == null || !amazonStats.Any())
            {
                return; //note: not setting stats to 0 if !any
            }

            stats.Cost = amazonStats.Sum(x => x.Cost);
            stats.Impressions = amazonStats.Sum(x => x.Impressions);
            stats.Clicks = amazonStats.Sum(x => x.Clicks);
            stats.PostClickConv = amazonStats.Sum(x => x.AttributedConversions14D);
            var rev = stats as DatedStatsSummaryWithRev;
            if (rev != null)
            {
                rev.PostClickRev = amazonStats.Sum(x => x.AttributedSales14D);
            }
            stats.InitialMetrics = GetMetrics(amazonStats, date);
        }

        protected static void AddAsinMetrics(T stats, IEnumerable<AmazonAsinSummaries> asinStats)
        {
            var metrics = stats.InitialMetrics.ToList();
            var asinMetrics = GetAsinMetrics(asinStats, stats.Date);
            metrics.AddRange(asinMetrics);
            stats.InitialMetrics = metrics;
        }

        protected static void AddMetric(List<SummaryMetric> metrics, string metricName, DateTime date, decimal metricValue)
        {
            AddMetric(metrics, metricName, null, date, metricValue);
        }

        protected static void AddMetric(List<SummaryMetric> metrics, AttributedMetricType type, AttributedMetricDaysInterval daysInterval, DateTime date, decimal metricValue)
        {
            AddMetric(metrics, type.ToString(), (int)daysInterval, date, metricValue);
        }

        private static IEnumerable<SummaryMetric> GetAsinMetrics(IEnumerable<AmazonAsinSummaries> asinStats, DateTime date)
        {
            var metrics = new List<SummaryMetric>();
            AddMetric(metrics, AttributedMetricType.attributedSalesOtherSKU, AttributedMetricDaysInterval.Days1, date, asinStats.Sum(x => x.AttributedSales1DOtherSku));
            AddMetric(metrics, AttributedMetricType.attributedSalesOtherSKU, AttributedMetricDaysInterval.Days7, date, asinStats.Sum(x => x.AttributedSales7DOtherSku));
            AddMetric(metrics, AttributedMetricType.attributedSalesOtherSKU, AttributedMetricDaysInterval.Days14, date, asinStats.Sum(x => x.AttributedSales14DOtherSku));
            AddMetric(metrics, AttributedMetricType.attributedSalesOtherSKU, AttributedMetricDaysInterval.Days30, date, asinStats.Sum(x => x.AttributedSales30DOtherSku));
            AddMetric(metrics, AttributedMetricType.attributedUnitsOrderedOtherSKU, AttributedMetricDaysInterval.Days1, date, asinStats.Sum(x => x.AttributedUnitsOrdered1DOtherSku));
            AddMetric(metrics, AttributedMetricType.attributedUnitsOrderedOtherSKU, AttributedMetricDaysInterval.Days7, date, asinStats.Sum(x => x.AttributedUnitsOrdered7DOtherSku));
            AddMetric(metrics, AttributedMetricType.attributedUnitsOrderedOtherSKU, AttributedMetricDaysInterval.Days14, date, asinStats.Sum(x => x.AttributedUnitsOrdered14DOtherSku));
            AddMetric(metrics, AttributedMetricType.attributedUnitsOrderedOtherSKU, AttributedMetricDaysInterval.Days30, date, asinStats.Sum(x => x.AttributedUnitsOrdered30DOtherSku));
            return metrics;
        }

        private static IEnumerable<SummaryMetric> GetMetrics(IEnumerable<AmazonStatSummary> amazonStats, DateTime date)
        {
            var metrics = new List<SummaryMetric>();
            AddMetric(metrics, AttributedMetricType.attributedConversions, AttributedMetricDaysInterval.Days1, date, amazonStats.Sum(x => x.AttributedConversions1D));
            AddMetric(metrics, AttributedMetricType.attributedConversions, AttributedMetricDaysInterval.Days7, date, amazonStats.Sum(x => x.AttributedConversions7D));
            AddMetric(metrics, AttributedMetricType.attributedConversions, AttributedMetricDaysInterval.Days14, date, amazonStats.Sum(x => x.AttributedConversions14D));
            AddMetric(metrics, AttributedMetricType.attributedConversions, AttributedMetricDaysInterval.Days30, date, amazonStats.Sum(x => x.AttributedConversions30D));
            AddMetric(metrics, AttributedMetricType.attributedConversionsSameSKU, AttributedMetricDaysInterval.Days1, date, amazonStats.Sum(x => x.AttributedConversions1DSameSku));
            AddMetric(metrics, AttributedMetricType.attributedConversionsSameSKU, AttributedMetricDaysInterval.Days7, date, amazonStats.Sum(x => x.AttributedConversions7DSameSku));
            AddMetric(metrics, AttributedMetricType.attributedConversionsSameSKU, AttributedMetricDaysInterval.Days14, date, amazonStats.Sum(x => x.AttributedConversions14DSameSku));
            AddMetric(metrics, AttributedMetricType.attributedConversionsSameSKU, AttributedMetricDaysInterval.Days30, date, amazonStats.Sum(x => x.AttributedConversions30DSameSku));
            AddMetric(metrics, AttributedMetricType.attributedSales, AttributedMetricDaysInterval.Days1, date, amazonStats.Sum(x => x.AttributedSales1D));
            AddMetric(metrics, AttributedMetricType.attributedSales, AttributedMetricDaysInterval.Days7, date, amazonStats.Sum(x => x.AttributedSales7D));
            AddMetric(metrics, AttributedMetricType.attributedSales, AttributedMetricDaysInterval.Days14, date, amazonStats.Sum(x => x.AttributedSales14D));
            AddMetric(metrics, AttributedMetricType.attributedSales, AttributedMetricDaysInterval.Days30, date, amazonStats.Sum(x => x.AttributedSales30D));
            AddMetric(metrics, AttributedMetricType.attributedSalesSameSKU, AttributedMetricDaysInterval.Days1, date, amazonStats.Sum(x => x.AttributedSales1DSameSku));
            AddMetric(metrics, AttributedMetricType.attributedSalesSameSKU, AttributedMetricDaysInterval.Days7, date, amazonStats.Sum(x => x.AttributedSales7DSameSku));
            AddMetric(metrics, AttributedMetricType.attributedSalesSameSKU, AttributedMetricDaysInterval.Days14, date, amazonStats.Sum(x => x.AttributedSales14DSameSku));
            AddMetric(metrics, AttributedMetricType.attributedSalesSameSKU, AttributedMetricDaysInterval.Days30, date, amazonStats.Sum(x => x.AttributedSales30DSameSku));
            AddMetric(metrics, AttributedMetricType.attributedUnitsOrdered, AttributedMetricDaysInterval.Days1, date, amazonStats.Sum(x => x.AttributedUnitsOrdered1D));
            AddMetric(metrics, AttributedMetricType.attributedUnitsOrdered, AttributedMetricDaysInterval.Days7, date, amazonStats.Sum(x => x.AttributedUnitsOrdered7D));
            AddMetric(metrics, AttributedMetricType.attributedUnitsOrdered, AttributedMetricDaysInterval.Days14, date, amazonStats.Sum(x => x.AttributedUnitsOrdered14D));
            AddMetric(metrics, AttributedMetricType.attributedUnitsOrdered, AttributedMetricDaysInterval.Days30, date, amazonStats.Sum(x => x.AttributedUnitsOrdered30D));
            return metrics;
        }

        private static void AddMetric(List<SummaryMetric> metrics, string metricName, int? daysInterval, DateTime date, decimal metricValue)
        {
            if (metricValue == 0.0M)
            {
                return;
            }

            var metric = GetMetric(metricName, daysInterval, date, metricValue);
            metrics.Add(metric);
        }

        private static SummaryMetric GetMetric(string metricName, int? daysInterval, DateTime date, decimal metricValue)
        {
            var metricType = new MetricType(metricName, daysInterval);
            var metric = new SummaryMetric(date, metricType, metricValue);
            return metric;
        }
    }
}
