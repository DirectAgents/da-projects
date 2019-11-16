﻿using System;
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
            { AdInteractionType.UniqueImpressions, "None" },
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
            SetClickAndViewStats(stats, adformStats);
            SetConversionMetrics(stats, adformStats);
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
            stats.PostClickRev = clickThroughs.Sum(x => x.SalesAll);
            stats.PostViewRev = viewThroughs.Sum(x => x.SalesAll);
        }

        protected static void SetConversionMetrics(T stats, IEnumerable<AdformSummary> adformStats)
        {
            var clickThroughs = adformStats.Where(x => x.AdInteractionType == AdInteractions[AdInteractionType.Clicks]);
            var viewThroughs = adformStats.Where(x => x.AdInteractionType == AdInteractions[AdInteractionType.Impressions]);
            var uniqImpressions = adformStats.Where(x => x.AdInteractionType == AdInteractions[AdInteractionType.UniqueImpressions]);
            stats.ConversionsConvType1Clicks = clickThroughs.Sum(x => x.ConversionsConvType1);
            stats.ConversionsConvType2Clicks = clickThroughs.Sum(x => x.ConversionsConvType2);
            stats.ConversionsConvType3Clicks = clickThroughs.Sum(x => x.ConversionsConvType3);
            stats.SalesConvType1Clicks = clickThroughs.Sum(x => x.SalesConvType1);
            stats.SalesConvType2Clicks = clickThroughs.Sum(x => x.SalesConvType2);
            stats.SalesConvType3Clicks = clickThroughs.Sum(x => x.SalesConvType3);

            stats.ConversionsConvType1Impressions = viewThroughs.Sum(x => x.ConversionsConvType1);
            stats.ConversionsConvType2Impressions = viewThroughs.Sum(x => x.ConversionsConvType2);
            stats.ConversionsConvType3Impressions = viewThroughs.Sum(x => x.ConversionsConvType3);
            stats.SalesConvType1Impressions = viewThroughs.Sum(x => x.SalesConvType1);
            stats.SalesConvType2Impressions = viewThroughs.Sum(x => x.SalesConvType2);
            stats.SalesConvType3Impressions = viewThroughs.Sum(x => x.SalesConvType3);

            stats.UniqueImpressions = uniqImpressions.Sum(x => x.UniqueImpressions);
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
