using System;
using System.Collections.Generic;
using System.Linq;
using Adform.Entities.ReportEntities;
using Adform.Utilities;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.Extracters.AdformExtractors
{
    public class AdformDailySummaryExtractor : AdformApiBaseExtractor<DailySummary>
    {
        public AdformDailySummaryExtractor(AdformUtility adformUtility, DateRange dateRange, ExtAccount account, bool rtbMediaOnly)
            : base(adformUtility, dateRange, account, rtbMediaOnly)
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
            var dayStatsTransformer = new AdformTransformer(basicStatsReportData, basicStatsOnly: true);
            var daySumDict = dayStatsTransformer.EnumerateAdformSummaries().ToDictionary(x => x.Date);

            var convStatsTransformer = new AdformTransformer(convStatsReportData, convStatsOnly: true);
            var convSums = convStatsTransformer.EnumerateAdformSummaries();
            var convSumGroups = convSums.GroupBy(x => x.Date).ToList();
            // Steps:
            // loop through convSumGroups; get daySum or create blank one
            // then go through daySums that didn't have a convSumGroup
            foreach (var csGroup in convSumGroups)
            {
                var daySum = new DailySummary { Date = csGroup.Key };
                if (daySumDict.ContainsKey(csGroup.Key))
                {
                    SetBaseStats(daySum, daySumDict[csGroup.Key]);
                }

                SetConversionStats(daySum, csGroup, csGroup.Key);
                yield return daySum;
            }

            var convSumDates = convSumGroups.Select(x => x.Key).ToArray();
            var remainingDaySums = daySumDict.Values.Where(ds => !convSumDates.Contains(ds.Date));
            foreach (var adfSum in remainingDaySums) // the daily summaries that didn't have any conversion summaries
            {
                var daySum = new DailySummary { Date = adfSum.Date };
                SetBaseStats(daySum, adfSum);
                yield return daySum;
            }
        }
    }
}