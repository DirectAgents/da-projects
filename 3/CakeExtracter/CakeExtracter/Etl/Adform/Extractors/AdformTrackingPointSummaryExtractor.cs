using System;
using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Entities.CPProg.Adform.Summaries;
using Adform.Utilities;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;
using Adform.Entities;
using Adform.Entities.ReportEntities;
using DirectAgents.Domain.Entities.CPProg.Adform;
using Adform.Enums;
using Adform.Entities.ReportEntities.ReportParameters;
using CakeExtracter.Etl.Adform.Exceptions;
using CakeExtracter.Commands;

namespace CakeExtracter.Etl.Adform.Extractors
{
    /// <inheritdoc />
    /// <summary>
    /// Adform Tracking Point summary extractor.
    /// </summary>
    public class AdformTrackingPointSummaryExtractor : AdformApiBaseExtractor<AdfTrackingPointSummary>
    {
        private readonly bool byOrder;

        /// <inheritdoc cref="AdformApiBaseExtractor{T}"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="AdformTrackingPointSummaryExtractor"/> class.
        /// </summary>
        /// <param name="adformUtility">API utility.</param>
        /// <param name="dateRange">Date range.</param>
        /// <param name="account">Account.</param>
        /// <param name="byOrder">Flag indicates to extract order dimension instead of campaign dimension.</param>
        public AdformTrackingPointSummaryExtractor(AdformUtility adformUtility, DateRange dateRange, ExtAccount account, bool byOrder)
            : base(adformUtility, dateRange, account)
        {
            this.byOrder = byOrder;
        }

        /// <inheritdoc/>
        protected override void Extract()
        {
            Logger.Info(AccountId, $"Extracting TrackingPointSummaries from Adform API for ({ClientId}) from {DateRange.FromDate:d} to {DateRange.ToDate:d}");
            try
            {
                var data = ExtractData();
                var sums = GroupSummaries(data);
                var s = data.Where(x => x.TrackingPoint != null);
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
            return reportData.SelectMany(TransformReportData).ToList();
        }

        private IEnumerable<AdformReportSummary> TransformReportData(ReportData reportData)
        {
            var adFormTransformer = new AdformReportDataTransformer(reportData, byLineItem: true, byCampaign: !byOrder, byOrder: byOrder, byTrackingPoint: true, filterInteractionType: true);
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
            var trackingPointGroups = afSums.GroupBy(x => new { x.LineItem, x.LineItemId, x.OrderId, x.TrackingPointId, x.TrackingPoint, x.Date, x.MediaId });
            foreach (var trackingPointGroup in trackingPointGroups)
            {
                var sum = new AdfTrackingPointSummary
                {
                    Date = trackingPointGroup.Key.Date,
                    MediaType = new AdfMediaType
                    {
                        ExternalId = trackingPointGroup.Key.MediaId,
                    },
                    TrackingPoint = new AdfTrackingPoint
                    {
                        ExternalId = trackingPointGroup.Key.TrackingPointId,
                        Name = trackingPointGroup.Key.TrackingPoint,
                        LineItem = new AdfLineItem
                        {
                            ExternalId = byOrder ? trackingPointGroup.FirstOrDefault()?.OrderId : trackingPointGroup.FirstOrDefault()?.LineItemId,
                            Name = byOrder ? trackingPointGroup.FirstOrDefault()?.Order : trackingPointGroup.FirstOrDefault()?.LineItem,
                        },
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
                byOrder ? Dimension.Order : Dimension.Campaign,
                byOrder ? Dimension.OrderId : Dimension.CampaignId,
                Dimension.LineItem,
                Dimension.LineItemId,
                Dimension.TrackingPoint,
                Dimension.TrackingPointId,
            };
            SetDimensionsForReportSettings(dimensions, settings);
            var reportParams = AfUtility.CreateReportParams(settings);
            reportParams.Dimensions = reportParams.Dimensions.Where(x => !x.Equals("adInteractionType")).ToArray();
            return reportParams;
        }

        private void ProcessFailedStatsExtraction(Exception e, DateTime fromDate, DateTime toDate)
        {
            Logger.Error(AccountId, e);
            var exception = new AdformFailedStatsLoadingException(fromDate, toDate, AccountId, e, AdfStatsTypeAgg.TrackingPointArg);
            InvokeProcessFailedExtractionHandlers(exception);
        }
    }
}
