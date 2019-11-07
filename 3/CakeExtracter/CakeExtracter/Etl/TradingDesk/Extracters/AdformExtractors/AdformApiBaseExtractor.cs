using System;
using System.Collections.Generic;
using System.Linq;
using Adform;
using Adform.Entities;
using Adform.Enums;
using Adform.Utilities;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.Extracters.AdformExtractors
{
    public abstract class AdformApiBaseExtractor<T> : Extracter<T>
        where T : DatedStatsSummary
    {
        protected readonly AdformUtility AfUtility;
        protected readonly DateRange DateRange;
        protected readonly int ClientId;

        /// <summary>
        /// Internal identifier of account.
        /// </summary>
        protected readonly int AccountId;

        protected Dictionary<DateTime, decimal> MonthlyCostMultipliers = new Dictionary<DateTime, decimal>();

        private static readonly Dictionary<AdInteractionType, string> AdInteractions = new Dictionary<AdInteractionType, string>
        {
            { AdInteractionType.Clicks, "Click" },
            { AdInteractionType.Impressions, "Impression" },
            { AdInteractionType.UniqueImpressions, "None" },
        };

        private static readonly Dictionary<AdInteractionType, Dictionary<ConversionMetric, string>> MetricNames =
            new Dictionary<AdInteractionType, Dictionary<ConversionMetric, string>>
            {
                {
                    AdInteractionType.Clicks, new Dictionary<ConversionMetric, string>
                    {
                        {ConversionMetric.ConversionsConversionType1, "Adform-Conversions-ConvType1-Clicks"},
                        {ConversionMetric.ConversionsConversionType2, "Adform-Conversions-ConvType2-Clicks"},
                        {ConversionMetric.ConversionsConversionType3, "Adform-Conversions-ConvType3-Clicks"},
                        {ConversionMetric.SalesConversionType1, "Adform-Sales-ConvType1-Clicks"},
                        {ConversionMetric.SalesConversionType2, "Adform-Sales-ConvType2-Clicks"},
                        {ConversionMetric.SalesConversionType3, "Adform-Sales-ConvType3-Clicks"},
                    }
                },
                {
                    AdInteractionType.Impressions, new Dictionary<ConversionMetric, string>
                    {
                        {ConversionMetric.ConversionsConversionType1, "Adform-Conversions-ConvType1-Impressions"},
                        {ConversionMetric.ConversionsConversionType2, "Adform-Conversions-ConvType2-Impressions"},
                        {ConversionMetric.ConversionsConversionType3, "Adform-Conversions-ConvType3-Impressions"},
                        {ConversionMetric.SalesConversionType1, "Adform-Sales-ConvType1-Impressions"},
                        {ConversionMetric.SalesConversionType2, "Adform-Sales-ConvType2-Impressions"},
                        {ConversionMetric.SalesConversionType3, "Adform-Sales-ConvType3-Impressions"},
                    }
                },
                {
                    AdInteractionType.UniqueImpressions, new Dictionary<ConversionMetric, string>
                    {
                        { ConversionMetric.UniqueImpressions, "Adform-Unique-Impressions" },
                    }
                },
            };

        private readonly bool rtbMediaOnly;

        protected AdformApiBaseExtractor(AdformUtility adformUtility, DateRange dateRange, ExtAccount account, bool rtbMediaOnly)
        {
            AfUtility = adformUtility;
            DateRange = dateRange;
            ClientId = int.Parse(account.ExternalId);
            AccountId = account.Id;
            if (account.Campaign != null)
            {
                SetMonthlyCostMultipliers(account, dateRange);
            }
            this.rtbMediaOnly = rtbMediaOnly;
        }

        protected ReportSettings GetBaseSettings()
        {
            return new ReportSettings
            {
                StartDate = DateRange.FromDate,
                EndDate = DateRange.ToDate,
                ClientId = ClientId,
                BasicMetrics = true,
                ConvMetrics = true,
                UniqueImpressionsMetricForAllMediaTypes = false,
                RtbMediaOnly = rtbMediaOnly,
                TrackingIds = AfUtility.TrackingIds,
                Dimensions = new List<Dimension> { Dimension.AdInteractionType },
            };
        }

        protected IEnumerable<T> AdjustItems(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                var monthStart = new DateTime(item.Date.Year, item.Date.Month, 1);
                if (MonthlyCostMultipliers.ContainsKey(monthStart))
                {
                    item.Cost *= MonthlyCostMultipliers[monthStart];
                }

                yield return item;
            }
        }

        protected void SetStats(T stats, IEnumerable<AdformSummary> adformStats, DateTime date)
        {
            stats.Date = date;
            SetBaseStats(stats, adformStats);
            SetClickAndViewStats(stats, adformStats);
            SetConversionMetrics(stats, date, adformStats);
        }

        protected void SetBaseStats(T stats, AdformSummary adformStat)
        {
            SetBaseStats(stats, new[] {adformStat});
        }

        protected void SetBaseStats(T stats, IEnumerable<AdformSummary> adformStats)
        {
            stats.Cost = adformStats.Sum(x => x.Cost);
            stats.Impressions = adformStats.Sum(x => x.Impressions);
            stats.Clicks = adformStats.Sum(x => x.Clicks);
        }

        protected static void SetClickAndViewStats(T stats, IEnumerable<AdformSummary> adformStats)
        {
            var clickThroughs = adformStats.Where(x => x.AdInteractionType == AdInteractions[AdInteractionType.Clicks]);
            var viewThroughs = adformStats.Where(x => x.AdInteractionType == AdInteractions[AdInteractionType.Impressions]);
            stats.PostClickConv = clickThroughs.Sum(x => x.ConversionsAll);
            stats.PostViewConv = viewThroughs.Sum(x => x.ConversionsAll);
            var rev = stats as DatedStatsSummaryWithRev;
            if (rev != null)
            {
                rev.PostClickRev = clickThroughs.Sum(x => x.SalesAll);
                rev.PostViewRev = viewThroughs.Sum(x => x.SalesAll);
            }
        }

        protected static void SetConversionMetrics(T stats, DateTime date, IEnumerable<AdformSummary> adformStats)
        {
            var metrics = new List<SummaryMetric>();
            SetConversionMetrics(metrics, adformStats, date);
            SetUniqueImpressionMetric(metrics, adformStats, date);
            stats.InitialMetrics = metrics;
        }

        protected static void SetConversionMetricsWithUniqImprForAllMedia(
            T stats, DateTime date, IEnumerable<AdformSummary> adformStats, IEnumerable<AdformSummary> uniqueImpressionStats)
        {
            var metrics = new List<SummaryMetric>();
            SetConversionMetrics(metrics, adformStats, date);
            SetUniqueImpressionMetricForAllMedia(metrics, uniqueImpressionStats, date);
            stats.InitialMetrics = metrics;
        }

        private static void SetConversionMetrics(List<SummaryMetric> metrics, IEnumerable<AdformSummary> adformStats, DateTime date)
        {
            var clickThroughs = adformStats.Where(x => x.AdInteractionType == AdInteractions[AdInteractionType.Clicks]);
            var viewThroughs = adformStats.Where(x => x.AdInteractionType == AdInteractions[AdInteractionType.Impressions]);
            AddMetrics(metrics, clickThroughs, AdInteractionType.Clicks, date);
            AddMetrics(metrics, viewThroughs, AdInteractionType.Impressions, date);
        }

        private static void SetUniqueImpressionMetric(
            List<SummaryMetric> metrics, IEnumerable<AdformSummary> adformStats, DateTime date)
        {
            var uniqueImpressionSum = adformStats
                .Where(x => x.AdInteractionType == AdInteractions[AdInteractionType.UniqueImpressions])
                .Sum(x => x.UniqueImpressions);
            AddMetric(metrics, AdInteractionType.UniqueImpressions, ConversionMetric.UniqueImpressions, date, uniqueImpressionSum);
        }

        private static void SetUniqueImpressionMetricForAllMedia(
            List<SummaryMetric> metrics, IEnumerable<AdformSummary> uniqueImpressionStats, DateTime date)
        {
            var uniqueImpressionSum = uniqueImpressionStats.Sum(x => x.UniqueImpressions);
            AddMetric(metrics, AdInteractionType.UniqueImpressions, ConversionMetric.UniqueImpressions, date, uniqueImpressionSum);
        }

        private static void AddMetrics(List<SummaryMetric> metrics, IEnumerable<AdformSummary> adInteractionStats, AdInteractionType adInteractionType, DateTime date)
        {
            AddMetric(metrics, adInteractionType, ConversionMetric.ConversionsConversionType1, date, adInteractionStats.Sum(x => x.ConversionsConvType1));
            AddMetric(metrics, adInteractionType, ConversionMetric.ConversionsConversionType2, date, adInteractionStats.Sum(x => x.ConversionsConvType2));
            AddMetric(metrics, adInteractionType, ConversionMetric.ConversionsConversionType3, date, adInteractionStats.Sum(x => x.ConversionsConvType3));
            AddMetric(metrics, adInteractionType, ConversionMetric.SalesConversionType1, date, adInteractionStats.Sum(x => x.SalesConvType1));
            AddMetric(metrics, adInteractionType, ConversionMetric.SalesConversionType2, date, adInteractionStats.Sum(x => x.SalesConvType2));
            AddMetric(metrics, adInteractionType, ConversionMetric.SalesConversionType3, date, adInteractionStats.Sum(x => x.SalesConvType3));
        }

        private static void AddMetric(List<SummaryMetric> metrics, AdInteractionType adInteractionType, ConversionMetric metricType, DateTime date, decimal metricValue)
        {
            if (metricValue == 0.0M)
            {
                return;
            }

            var metric = GetMetric(MetricNames[adInteractionType][metricType], null, date, metricValue);
            metrics.Add(metric);
        }

        private static SummaryMetric GetMetric(string metricName, int? daysInterval, DateTime date, decimal metricValue)
        {
            var metricType = new MetricType(metricName, daysInterval);
            var metric = new SummaryMetric(date, metricType, metricValue);
            return metric;
        }

        private void SetMonthlyCostMultipliers(ExtAccount account, DateRange dateRange)
        {
            var clientCostConversionStart = new DateTime(2018, 2, 1); // Starting 2/1/18, Adform pulls in "client cost" rather than "DA cost" so we have to convert back to "client cost"
            var clientCostConversionStop = new DateTime(2019, 1, 1); // Starting 1/1/2019, we treat "da cost" == "client cost" so stop doing the conversion

            for (var firstOfMonth = new DateTime(dateRange.FromDate.Year, dateRange.FromDate.Month, 1);
                firstOfMonth <= dateRange.ToDate; firstOfMonth = firstOfMonth.AddMonths(1))
            {
                if (firstOfMonth >= clientCostConversionStart && firstOfMonth < clientCostConversionStop)
                {
                    SetMonthlyCostMultiply(account, firstOfMonth);
                }
            }
        }

        private void SetMonthlyCostMultiply(ExtAccount account, DateTime firstOfMonth)
        {
            var pbi = account.Campaign.PlatformBudgetInfoFor(firstOfMonth, account.PlatformId, useParentValsIfNone: true);
            if (pbi.CostGoesThruDA())
            {
                MonthlyCostMultipliers[firstOfMonth] = pbi.MediaSpendToRevMultiplier * pbi.RevToCostMultiplier;
            }
            // If cost doesn't go through DA, that means "DACost" is 0. In that case don't set a multiplier so the cost won't get adjusted.
        }
    }
}
