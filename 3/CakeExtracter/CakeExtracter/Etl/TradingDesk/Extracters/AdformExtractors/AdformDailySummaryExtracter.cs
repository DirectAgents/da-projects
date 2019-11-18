using System;
using System.Collections.Generic;
using System.Linq;
using Adform.Entities;
using Adform.Entities.ReportEntities;
using Adform.Entities.ReportEntities.ReportParameters;
using Adform.Enums;
using Adform.Utilities;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Adform;
using DirectAgents.Domain.Entities.CPProg.Adform.Summaries;

namespace CakeExtracter.Etl.TradingDesk.Extracters.AdformExtractors
{
    /// <inheritdoc />
    /// <summary>
    /// Adform Daily summary extractor.
    /// </summary>
    public class AdformDailySummaryExtractor : AdformApiBaseExtractor<AdfDailySummary>
    {
        /// <inheritdoc cref="AdformApiBaseExtractor{T}"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="AdformDailySummaryExtractor" /> class.
        /// </summary>
        /// <param name="adformUtility">API utility.</param>
        /// <param name="dateRange">Date range.</param>
        /// <param name="account">Account.</param>
        public AdformDailySummaryExtractor(AdformUtility adformUtility, DateRange dateRange, ExtAccount account)
            : base(adformUtility, dateRange, account)
        {
        }

        /// <inheritdoc />
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

        private IEnumerable<AdformReportSummary> ExtractData()
        {
            var reportData = GetReportData();
            return reportData.SelectMany(TransformReportData).ToList();
        }

        private IEnumerable<AdfDailySummary> EnumerateRows(IEnumerable<AdformReportSummary> afSums)
        {
            var dailyGroups = afSums.GroupBy(x => new { x.Date, x.MediaId, x.Media });
            foreach (var dailyGroup in dailyGroups)
            {
                var dailySummary = new AdfDailySummary
                {
                    Date = dailyGroup.Key.Date,
                    MediaType = new AdfMediaType
                    {
                        ExternalId = dailyGroup.Key.MediaId,
                        Name = dailyGroup.Key.Media,
                    },
                };
                SetStats(dailySummary, dailyGroup);
                yield return dailySummary;
            }
        }

        private IEnumerable<ReportData> GetReportData()
        {
            var parameters = GetReportParameters();
            return AfUtility.GetReportDataWithLimits(parameters);
        }

        private ReportParams GetReportParameters()
        {
            var settings = GetBaseSettings();
            var dimensions = new List<Dimension> { Dimension.Media };
            SetDimensionsForReportSettings(dimensions, settings);
            return AfUtility.CreateReportParams(settings);
        }

        private IEnumerable<AdformReportSummary> TransformReportData(ReportData reportData)
        {
            var dayStatsTransformer = new AdformTransformer(reportData);
            return dayStatsTransformer.EnumerateAdformSummaries();
        }
    }
}