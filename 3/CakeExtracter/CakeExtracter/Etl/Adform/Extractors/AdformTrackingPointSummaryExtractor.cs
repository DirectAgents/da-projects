using System;
using System.Collections.Generic;
using System.Linq;
using Adform.Enums;
using Adform.Entities;
using Adform.Entities.ReportEntities;
using Adform.Entities.ReportEntities.ReportParameters;
using Adform.Utilities;
using CakeExtracter.Common;
using CakeExtracter.Etl.Adform.Exceptions;
using DirectAgents.Domain.Entities.CPProg.Adform.Summaries;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Adform;

namespace CakeExtracter.Etl.Adform.Extractors
{
    /// <inheritdoc />
    /// <summary>
    /// Adform Tracking Point summary extractor.
    /// </summary>
    public class AdformTrackingPointSummaryExtractor : AdformApiBaseExtractor<AdfTrackingPointSummary>
    {
        /// <inheritdoc cref="AdformApiBaseExtractor{T}"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="AdformTrackingPointSummaryExtractor"/> class.
        /// </summary>
        /// <param name="adformUtility">API utility.</param>
        /// <param name="dateRange">Date range.</param>
        /// <param name="account">Account.</param>
        public AdformTrackingPointSummaryExtractor(AdformUtility adformUtility, DateRange dateRange, ExtAccount account)
            : base(adformUtility, dateRange, account)
        {
        }

        /// <inheritdoc/>
        protected override void Extract()
        {
            Logger.Info(
                AccountId,
                $"Extracting TrackingPointSummaries from Adform API for ({ClientId}) from {DateRange.FromDate:d} to {DateRange.ToDate:d}");
            try
            {
                var data = ExtractData();
                var sums = GroupSummaries(data);
                Add(sums);
            }
            catch (Exception e)
            {
                ProcessFailedStatsExtraction(e, DateRange.FromDate, DateRange.ToDate);
            }
            finally
            {
                End();
            }
        }

        private IEnumerable<AdformReportSummary> ExtractData()
        {
            var reportData = GetReportData();
            return reportData.SelectMany(TransformReportData);
        }

        private IEnumerable<AdformReportSummary> TransformReportData(ReportData reportData)
        {
            var adFormTransformer = new AdformReportDataTransformer(reportData, byLineItem: true, byTrackingPoint: true);
            var afSums = adFormTransformer.EnumerateAdformSummaries();
            return afSums;
        }

        private IEnumerable<AdfTrackingPointSummary> GroupSummaries(IEnumerable<AdformReportSummary> adFormSums)
        {
            var sums = EnumerateRows(adFormSums);
            var resultSums = AdjustItems(sums);
            return resultSums;
        }

        private IEnumerable<AdfTrackingPointSummary> EnumerateRows(IEnumerable<AdformReportSummary> afSums)
        {
            var unsupportedName = new[] { "n/a", "Unknown" };
            var trackingPointGroups = afSums.GroupBy(
                x => new { x.LineItem, x.LineItemId, x.TrackingPointId, x.TrackingPoint, x.Date, x.MediaId, });

            foreach (var trackingPointGroup in trackingPointGroups)
            {
                if (unsupportedName.Any(x =>
                        string.Equals(x, trackingPointGroup.Key.LineItem, StringComparison.InvariantCultureIgnoreCase)
                        || string.Equals(x, trackingPointGroup.Key.TrackingPoint, StringComparison.InvariantCultureIgnoreCase)))
                {
                    continue;
                }

                var sum = new AdfTrackingPointSummary
                {
                    Date = trackingPointGroup.Key.Date,
                    MediaType = new AdfMediaType
                    {
                        ExternalId = trackingPointGroup.Key.MediaId,
                    },
                    LineItem = new AdfLineItem
                    {
                        ExternalId = trackingPointGroup.Key.LineItemId,
                    },
                    TrackingPoint = new AdfTrackingPoint
                    {
                        ExternalId = trackingPointGroup.Key.TrackingPointId,
                        Name = trackingPointGroup.Key.TrackingPoint,
                    },
                };
                SetStats(sum, trackingPointGroup);
                yield return sum;
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
            var dimensions = new List<Dimension>
            {
                Dimension.LineItemId,
                Dimension.TrackingPoint,
                Dimension.TrackingPointId,
            };
            SetDimensionsForReportSettings(dimensions, settings);
            return AfUtility.CreateReportParams(settings);
        }

        private void ProcessFailedStatsExtraction(Exception e, DateTime fromDate, DateTime toDate)
        {
            Logger.Error(AccountId, e);
            var exception = new AdformFailedStatsLoadingException(fromDate, toDate, AccountId, e, AdfStatsTypeAgg.TrackingPointArg);
            InvokeProcessFailedExtractionHandlers(exception);
        }
    }
}
