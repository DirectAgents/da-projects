using System;
using System.Collections.Generic;
using System.Linq;
using Adform.Outdated.Entities;
using Adform.Outdated.Entities.ReportEntities;
using Adform.Outdated.Utilities;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.Extracters.AdformExtractors
{
    public class AdformDailySummaryExtractor : AdformApiBaseExtractor<DailySummary>
    {
        private bool areUniqueImpressionsForAllMediaTypes;

        public AdformDailySummaryExtractor(
            AdformUtility adformUtility,
            DateRange dateRange,
            ExtAccount account,
            bool rtbMediaOnly,
            bool areUniqueImpressionsForAllMediaTypes,
            bool areAllStatsForAllMediaTypes)
            : base(adformUtility, dateRange, account, rtbMediaOnly, areAllStatsForAllMediaTypes)
        {
            this.areUniqueImpressionsForAllMediaTypes = areUniqueImpressionsForAllMediaTypes;
        }

        protected override void Extract()
        {
            Logger.Info(AccountId, $"Extracting DailySummaries from Adform API for ({ClientId}) from {DateRange.FromDate:d} to {DateRange.ToDate:d}");
            try
            {
                var basicStatsReportData = GetBasicStatsReportData();
                var convStatsReportData = GetConvStatsReportData();
                var uniqueImpressionStatsReportData = areUniqueImpressionsForAllMediaTypes
                    ? GetUniqueImpressionStatsReportData()
                    : null;
                var daySums = EnumerateRows(basicStatsReportData, convStatsReportData, uniqueImpressionStatsReportData);
                daySums = AdjustItems(daySums);
                Add(daySums);
            }
            catch (Exception ex)
            {
                Logger.Error(AccountId, ex);
            }
            End();
        }

        private ReportData GetBasicStatsReportData()
        {
            var settings = GetBaseSettings();
            settings.ConvMetrics = false;
            settings.Dimensions = null;
            var parameters = AfUtility.CreateReportParams(settings);
            var basicStatsReportData = AfUtility.TryGetReportData(parameters);
            return basicStatsReportData;
        }

        private ReportData GetConvStatsReportData()
        {
            var settings = GetBaseSettings();
            settings.BasicMetrics = false;
            var parameters = AfUtility.CreateReportParams(settings);
            var convStatsReportData = AfUtility.TryGetReportData(parameters);
            return convStatsReportData;
        }

        private ReportData GetUniqueImpressionStatsReportData()
        {
            var settings = GetUniqueImpressionSettings();
            var parameters = AfUtility.CreateReportParams(settings);
            var uniqueImpressionReportData = AfUtility.TryGetReportData(parameters);
            return uniqueImpressionReportData;
        }

        private IEnumerable<DailySummary> EnumerateRows(
            ReportData basicStatsReportData,
            ReportData convStatsReportData,
            ReportData uniqueImpressionReportData)
        {
            var dailyStats = TransformDailyReportData(basicStatsReportData);
            var dailyStatsByDate = dailyStats.ToDictionary(x => x.Date);
            var conversionStats = TransformConversionReportData(convStatsReportData);
            var conversionStatsGroups = conversionStats.GroupBy(x => x.Date).ToList();

            IEnumerable<AdformSummary> uniqueImpressionStats = new List<AdformSummary>();
            if (areUniqueImpressionsForAllMediaTypes)
            {
                uniqueImpressionStats = TransformUniqueImpressionReportData(uniqueImpressionReportData);
            }
            // Steps:
            // loop through convSumGroups; get daySum or create blank one
            // then go through daySums that didn't have a convSumGroup
            foreach (var conversionStatsGroup in conversionStatsGroups)
            {
                var daySum = new DailySummary { Date = conversionStatsGroup.Key };
                if (dailyStatsByDate.ContainsKey(conversionStatsGroup.Key))
                {
                    SetBaseStats(daySum, dailyStatsByDate[conversionStatsGroup.Key]);
                }
                SetClickAndViewStats(daySum, conversionStatsGroup);
                if (areUniqueImpressionsForAllMediaTypes)
                {
                    var uniqImprStatsForDay = uniqueImpressionStats.Where(s => s.Date == conversionStatsGroup.Key).ToList();
                    SetConversionMetricsWithUniqImprForAllMedia(daySum, conversionStatsGroup.Key, conversionStatsGroup, uniqImprStatsForDay);
                }
                else
                {
                    SetConversionMetrics(daySum, conversionStatsGroup.Key, conversionStatsGroup);
                }
                yield return daySum;
            }
            var convSumDates = conversionStatsGroups.Select(x => x.Key).ToArray();
            var remainingDaySums = dailyStatsByDate.Values.Where(ds => !convSumDates.Contains(ds.Date));
            foreach (var adfSum in remainingDaySums) // the daily summaries that didn't have any conversion summaries
            {
                var daySum = new DailySummary { Date = adfSum.Date };
                SetBaseStats(daySum, adfSum);
                yield return daySum;
            }
        }

        private ReportSettings GetUniqueImpressionSettings()
        {
            var settings = GetBaseSettings();
            settings.BasicMetrics = false;
            settings.ConvMetrics = false;
            settings.Dimensions = null;
            settings.UniqueImpressionsMetricForAllMediaTypes = true;
            return settings;
        }

        private IEnumerable<AdformSummary> TransformDailyReportData(ReportData reportData)
        {
            var dayStatsTransformer = new AdformTransformer(reportData, basicStatsOnly: true);
            return dayStatsTransformer.EnumerateAdformSummaries();
        }

        private IEnumerable<AdformSummary> TransformConversionReportData(ReportData reportData)
        {
            var convStatsTransformer = new AdformTransformer(reportData, convStatsOnly: true);
            return convStatsTransformer.EnumerateAdformSummaries();
        }

        private IEnumerable<AdformSummary> TransformUniqueImpressionReportData(ReportData reportData)
        {
            var uniqueImpressionStatsTransformer = new AdformTransformer(reportData, uniqueImpressionsOnly: true);
            return uniqueImpressionStatsTransformer.EnumerateAdformSummaries().ToList();
        }
    }
}