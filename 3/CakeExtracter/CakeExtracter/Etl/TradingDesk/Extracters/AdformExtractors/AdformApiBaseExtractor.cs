using System;
using System.Collections.Generic;
using System.Linq;
using Adform;
using Adform.Entities;
using Adform.Enums;
using Adform.Utilities;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Adform.Summaries;

namespace CakeExtracter.Etl.TradingDesk.Extracters.AdformExtractors
{
    public abstract class AdformApiBaseExtractor<T> : Extracter<T>
        where T : AdfBaseSummary
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
            { AdInteractionType.None, "None" },
        };

        protected AdformApiBaseExtractor(AdformUtility adformUtility, DateRange dateRange, ExtAccount account)
        {
            AfUtility = adformUtility;
            DateRange = dateRange;
            ClientId = int.Parse(account.ExternalId);
            AccountId = account.Id;
            if (account.Campaign != null)
            {
                SetMonthlyCostMultipliers(account, dateRange);
            }
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
                TrackingIds = AfUtility.TrackingIds,
                Dimensions = new List<Dimension> { Dimension.AdInteractionType },
            };
        }

        protected void SetDimensionsForReportSettings(IEnumerable<Dimension> dimensions, ReportSettings settings)
        {
            settings.Dimensions.AddRange(dimensions);
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

        protected void SetStats(T stats, IEnumerable<AdformSummary> adformStats/*, DateTime date*/)
        {
            SetBaseStats(stats, adformStats);
            SetConversionStats(stats, adformStats);
        }

        private static void SetBaseStats(T stats, IEnumerable<AdformSummary> adformStats)
        {
            stats.Cost = adformStats.Sum(x => x.Cost);
            stats.Impressions = adformStats.Sum(x => x.Impressions);
            stats.Clicks = adformStats.Sum(x => x.Clicks);
        }

        private static void SetConversionStats(T stats, IEnumerable<AdformSummary> adformStats)
        {
            SetClickAdInteractionMetrics(stats, adformStats);
            SetImpressionAdInteractionMetrics(stats, adformStats);
            SetUniqueImpressionMetric(stats, adformStats);
        }

        private static void SetClickAdInteractionMetrics(T stats, IEnumerable<AdformSummary> adformStats)
        {
            var clickAdInteractionStats = adformStats.Where(x => x.AdInteractionType == AdInteractions[AdInteractionType.Clicks]);
            SetClickConversionMetrics(stats, clickAdInteractionStats);
            SetClickSaleMetrics(stats, clickAdInteractionStats);
        }

        private static void SetImpressionAdInteractionMetrics(T stats, IEnumerable<AdformSummary> adformStats)
        {
            var viewAdInteractionStats = adformStats.Where(x => x.AdInteractionType == AdInteractions[AdInteractionType.Impressions]);
            SetImpressionConversionMetrics(stats, viewAdInteractionStats);
            SetImpressionSaleMetrics(stats, viewAdInteractionStats);
        }

        private static void SetUniqueImpressionMetric(T stats, IEnumerable<AdformSummary> adformStats)
        {
            var uniqueImpressionStats = adformStats.Where(x => x.AdInteractionType == AdInteractions[AdInteractionType.None]);
            stats.UniqueImpressions = uniqueImpressionStats.Sum(x => x.UniqueImpressions);
        }

        private static void SetClickConversionMetrics(T stats, IEnumerable<AdformSummary> clickAdInteractionStats)
        {
            stats.ClickConversionsConvTypeAll = clickAdInteractionStats.Sum(x => x.ConversionsAll);
            stats.ClickConversionsConvType1 = clickAdInteractionStats.Sum(x => x.ConversionsConvType1);
            stats.ClickConversionsConvType2 = clickAdInteractionStats.Sum(x => x.ConversionsConvType2);
            stats.ClickConversionsConvType3 = clickAdInteractionStats.Sum(x => x.ConversionsConvType3);
        }

        private static void SetClickSaleMetrics(T stats, IEnumerable<AdformSummary> clickAdInteractionStats)
        {
            stats.ClickSalesConvTypeAll = clickAdInteractionStats.Sum(x => x.SalesAll);
            stats.ClickSalesConvType1 = clickAdInteractionStats.Sum(x => x.SalesConvType1);
            stats.ClickSalesConvType2 = clickAdInteractionStats.Sum(x => x.SalesConvType2);
            stats.ClickSalesConvType3 = clickAdInteractionStats.Sum(x => x.SalesConvType3);
        }

        private static void SetImpressionConversionMetrics(T stats, IEnumerable<AdformSummary> viewAdInteractionStats)
        {
            stats.ImpressionConversionsConvTypeAll = viewAdInteractionStats.Sum(x => x.ConversionsAll);
            stats.ImpressionConversionsConvType1 = viewAdInteractionStats.Sum(x => x.ConversionsConvType1);
            stats.ImpressionConversionsConvType2 = viewAdInteractionStats.Sum(x => x.ConversionsConvType2);
            stats.ImpressionConversionsConvType3 = viewAdInteractionStats.Sum(x => x.ConversionsConvType3);
        }

        private static void SetImpressionSaleMetrics(T stats, IEnumerable<AdformSummary> viewAdInteractionStats)
        {
            stats.ImpressionSalesConvTypeAll = viewAdInteractionStats.Sum(x => x.SalesAll);
            stats.ImpressionSalesConvType1 = viewAdInteractionStats.Sum(x => x.SalesConvType1);
            stats.ImpressionSalesConvType2 = viewAdInteractionStats.Sum(x => x.SalesConvType2);
            stats.ImpressionSalesConvType3 = viewAdInteractionStats.Sum(x => x.SalesConvType3);
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
