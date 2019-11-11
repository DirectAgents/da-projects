using System;
using System.Collections.Generic;
using System.Linq;
using Adform;
using Adform.Entities.ReportEntities;
using Adform.Utilities;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.Extracters.AdformExtractors
{
    public class AdformDailySummaryExtractor : AdformApiBaseExtractor<DailySummary>
    {
        public AdformDailySummaryExtractor(AdformUtility adformUtility, DateRange dateRange, ExtAccount account)
            : base(adformUtility, dateRange, account)
        {
        }

        protected override void Extract()
        {
            Logger.Info(AccountId, $"Extracting DailySummaries from Adform API for ({ClientId}) from {DateRange.FromDate:d} to {DateRange.ToDate:d}");
            try
            {
                var basicStatsReportData = GetBasicStatsReportData();
                var convStatsReportData = GetConvStatsReportData();
                var daySums = EnumerateRows(basicStatsReportData, convStatsReportData);
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

        private IEnumerable<DailySummary> EnumerateRows(ReportData basicStatsReportData, ReportData convStatsReportData)
        {
            var dailyStats = TransformDailyReportData(basicStatsReportData);
            var dailyStatsByDate = dailyStats.ToDictionary(x => x.Date);
            var conversionStats = TransformConversionReportData(convStatsReportData);
            var conversionStatsGroups = conversionStats.GroupBy(x => x.Date).ToList();
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
                SetConversionMetrics(daySum, conversionStatsGroup.Key, conversionStatsGroup);
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
    }
}