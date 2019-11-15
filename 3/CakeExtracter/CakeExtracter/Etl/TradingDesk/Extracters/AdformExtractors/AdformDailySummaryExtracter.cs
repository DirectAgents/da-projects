using System;
using System.Collections.Generic;
using System.Linq;
using Adform;
using Adform.Entities.ReportEntities;
using Adform.Enums;
using Adform.Utilities;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Adform;
using DirectAgents.Domain.Entities.CPProg.Adform.Summaries;

namespace CakeExtracter.Etl.TradingDesk.Extracters.AdformExtractors
{
    public class AdformDailySummaryExtractor : AdformApiBaseExtractor<AdfDailySummary>
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
                var data = ExtractData();
                var daySums = EnumerateRows(data);
                daySums = AdjustItems(daySums);
                Add(daySums);
            }
            catch (Exception ex)
            {
                Logger.Error(AccountId, ex);
            }
            End();
        }

        private IEnumerable<AdformSummary> ExtractData()
        {
            var settings = GetBaseSettings();
            var dimensions = new List<Dimension> { Dimension.Media };
            SetDimensionsForReportSettings(dimensions, settings);
            var parameters = AfUtility.CreateReportParams(settings);
            var allReportData = AfUtility.GetReportDataWithLimits(parameters);
            var adFormSums = allReportData.SelectMany(TransformReportData).ToList();
            return adFormSums;
        }

        private IEnumerable<AdfDailySummary> EnumerateRows(IEnumerable<AdformSummary> afSums)
        {
            var dailyGroups = afSums.GroupBy(x => new { x.Date, x.MediaId, x.Media }).ToList();
            foreach (var dailyGroup in dailyGroups)
            {
                var daySum = new AdfDailySummary
                {
                    Date = dailyGroup.Key.Date,
                    MediaType = new AdfMediaType
                    {
                        ExternalId = dailyGroup.Key.MediaId,
                        Name = dailyGroup.Key.Media,
                    },
                };
                SetStats(daySum, dailyGroup);
                yield return daySum;
            }
        }

        private IEnumerable<AdformSummary> TransformReportData(ReportData reportData)
        {
            var dayStatsTransformer = new AdformTransformer(reportData);
            return dayStatsTransformer.EnumerateAdformSummaries();
        }
    }
}