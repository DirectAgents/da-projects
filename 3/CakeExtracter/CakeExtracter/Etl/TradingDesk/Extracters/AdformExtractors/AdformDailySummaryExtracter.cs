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
        private readonly int accountId;

        public AdformDailySummaryExtractor(AdformUtility adformUtility, DateRange dateRange, ExtAccount account)
            : base(adformUtility, dateRange, account)
        {
            accountId = account.Id;
        }

        protected override void Extract()
        {
            Logger.Info(accountId, $"Extracting DailySummaries from Adform API for ({ClientId}) from {DateRange.FromDate:d} to {DateRange.ToDate:d}");
            try
            {
                var basicStatsReportData = GetBasicStatsReportData();
                var convStatsReportData = GetConvStatsReportData();
                var daysums = EnumerateRows(basicStatsReportData, convStatsReportData);
                daysums = AdjustItems(daysums);
                Add(daysums);
            }
            catch (Exception ex)
            {
                Logger.Error(accountId, ex);
            }
            End();
        }

        private ReportData GetBasicStatsReportData()
        {
            var settings = GetBaseSettings();
            settings.ConvMetrics = false;
            settings.Dimensions = null;
            var parameters = AfUtility.CreateReportParams(settings);
            var dataLocationPath = AfUtility.ProcessDataReport(parameters);
            var basicStatsReportData = AfUtility.TryGetReportData(dataLocationPath);

            return basicStatsReportData;
        }

        private ReportData GetConvStatsReportData()
        {
            var settings = GetBaseSettings();
            settings.BasicMetrics = false;
            var parameters = AfUtility.CreateReportParams(settings);
            var dataLocationPath = AfUtility.ProcessDataReport(parameters);
            var convStatsReportData = AfUtility.TryGetReportData(dataLocationPath);

            return convStatsReportData;
        }

        private IEnumerable<DailySummary> EnumerateRows(ReportData basicStatsReportData, ReportData convStatsReportData)
        {
            var dayStatsTransformer = new AdformTransformer(basicStatsReportData, basicStatsOnly: true);
            var daySumDict = dayStatsTransformer.EnumerateAdformSummaries().ToDictionary(x => x.Date);

            var convStatsTransformer = new AdformTransformer(convStatsReportData, convStatsOnly: true);
            var convSums = convStatsTransformer.EnumerateAdformSummaries();
            var convsumGroups = convSums.GroupBy(x => x.Date);
            // Steps:
            // loop through convsumGroups; get daysum or create blank one
            // then go through daysums that didn't have a convsumGroup
            foreach (var csGroup in convsumGroups)
            {
                var daysum = new DailySummary {Date = csGroup.Key};
                if (daySumDict.ContainsKey(csGroup.Key))
                {
                    SetBaseStats(daysum, daySumDict[csGroup.Key]);
                }

                SetConversionStats(daysum, csGroup, csGroup.Key);
                yield return daysum;
            }

            var convsumDates = convsumGroups.Select(x => x.Key).ToArray();
            var remainingDaySums = daySumDict.Values.Where(ds => !convsumDates.Contains(ds.Date));
            foreach (var adfsum in remainingDaySums) // the daily summaries that didn't have any conversion summaries
            {
                var daysum = new DailySummary {Date = adfsum.Date};
                SetBaseStats(daysum, adfsum);
                yield return daysum;
            }
        }
    }
}