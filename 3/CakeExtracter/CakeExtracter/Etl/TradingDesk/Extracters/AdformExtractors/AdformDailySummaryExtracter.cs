using System;
using System.Collections.Generic;
using System.Linq;
using Adform;
using Adform.Entities.ReportEntities;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.Extracters.AdformExtractors
{
    public class AdformDailySummaryExtractor : AdformApiBaseExtractor<DailySummary>
    {
        public AdformDailySummaryExtractor(AdformUtility adformUtility, DateRange dateRange, ExtAccount account)
            : base(adformUtility, dateRange, account)
        { }

        protected override void Extract()
        {
            Logger.Info($"Extracting DailySummaries from Adform API for ({ClientId}) from {DateRange.FromDate:d} to {DateRange.ToDate:d}");
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
                Logger.Error(ex);
            }
            End();
        }

        private ReportData GetBasicStatsReportData()
        {
            var settings = GetBaseSettings();
            settings.ConvMetrics = false;
            settings.Dimensions = null;
            var parameters = AfUtility.CreateReportParams(settings);
            var basicStatsReportData = AfUtility.GetReportData(parameters);
            return basicStatsReportData;
        }

        private ReportData GetConvStatsReportData()
        {
            var settings = GetBaseSettings();
            settings.BasicMetrics = false;
            var parameters = AfUtility.CreateReportParams(settings);
            var convStatsReportData = AfUtility.GetReportData(parameters);
            return convStatsReportData;
        }

        private IEnumerable<DailySummary> EnumerateRows(ReportData basicStatsReportData, ReportData convStatsReportData)
        {
            var rowConverter = new AdformRowConverter(basicStatsReportData, includeConvMetrics: false);
            var daysumDict = rowConverter.EnumerateDailySummaries().ToDictionary(x => x.Date);

            var convStatsTransformer = new AdformTransformer(convStatsReportData, convStatsOnly: true);
            var convSums = convStatsTransformer.EnumerateAdformSummaries();
            var convsumGroups = convSums.GroupBy(x => x.Date);
            // Steps:
            // loop through convsumGroups; get daysum or create blank one
            // then go through daysums that didn't have a convsumGroup
            foreach (var csGroup in convsumGroups)
            {
                DailySummary daysum;
                if (daysumDict.ContainsKey(csGroup.Key))
                    daysum = daysumDict[csGroup.Key];
                else
                    daysum = new DailySummary
                    {
                        Date = csGroup.Key
                    };
                var clickThroughs = csGroup.Where(x => x.AdInteractionType == "Click");
                daysum.PostClickConv = clickThroughs.Sum(x => x.Conversions);
                daysum.PostClickRev = clickThroughs.Sum(x => x.Sales);

                var viewThroughs = csGroup.Where(x => x.AdInteractionType == "Impression");
                daysum.PostViewConv = viewThroughs.Sum(x => x.Conversions);
                daysum.PostViewRev = viewThroughs.Sum(x => x.Sales);

                yield return daysum;
            }
            var convsumDates = convsumGroups.Select(x => x.Key).ToArray();
            var remainingDaySums = daysumDict.Values.Where(ds => !convsumDates.Contains(ds.Date));
            foreach (var daysum in remainingDaySums) // the daily summaries that didn't have any conversion summaries
                yield return daysum;
        }
    }
}