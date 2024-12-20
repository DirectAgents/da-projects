﻿using System;
using System.Collections.Generic;
using System.Linq;
using Adform.Entities;
using Adform.Entities.ReportEntities;
using Adform.Entities.ReportEntities.ReportParameters;
using Adform.Enums;
using Adform.Utilities;
using CakeExtracter.Commands;
using CakeExtracter.Common;
using CakeExtracter.Etl.Adform.Exceptions;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Adform;
using DirectAgents.Domain.Entities.CPProg.Adform.Summaries;

namespace CakeExtracter.Etl.Adform.Extractors
{
    /// <inheritdoc />
    /// <summary>
    /// Adform Line Item summary extractor.
    /// </summary>
    public class AdformAdSetSummaryExtractor : AdformApiBaseExtractor<AdfLineItemSummary>
    {
        private readonly bool byOrder;

        /// <inheritdoc cref="AdformApiBaseExtractor{T}"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="AdformAdSetSummaryExtractor"/> class.
        /// </summary>
        /// <param name="adformUtility">API utility.</param>
        /// <param name="dateRange">Date range.</param>
        /// <param name="account">Account.</param>
        /// <param name="byOrder">Flag indicates to extract order dimension instead campaign dimension.</param>
        public AdformAdSetSummaryExtractor(AdformUtility adformUtility, DateRange dateRange, ExtAccount account, bool byOrder)
            : base(adformUtility, dateRange, account)
        {
            this.byOrder = byOrder;
        }

        /// <inheritdoc/>
        protected override void Extract()
        {
            Logger.Info(AccountId, $"Extracting LineItemSummaries from Adform API for ({ClientId}) from {DateRange.FromDate:d} to {DateRange.ToDate:d}");
            //TODO: Do X days at a time...?
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
            return reportData.SelectMany(TransformReportData).ToList();
        }

        private IEnumerable<AdformReportSummary> TransformReportData(ReportData reportData)
        {
            var adFormTransformer = new AdformReportDataTransformer(reportData, byLineItem: true, byCampaign: !byOrder, byOrder: byOrder);
            var afSums = adFormTransformer.EnumerateAdformSummaries();
            return afSums;
        }

        private IEnumerable<AdfLineItemSummary> GroupSummaries(IEnumerable<AdformReportSummary> adFormSums)
        {
            var sums = EnumerateRows(adFormSums);
            var resultSums = AdjustItems(sums);
            return resultSums;
        }

        private IEnumerable<AdfLineItemSummary> EnumerateRows(IEnumerable<AdformReportSummary> afSums)
        {
            var lineItemGroups = afSums.GroupBy(x => new { x.Campaign, x.CampaignId, x.OrderId, x.LineItemId, x.LineItem, x.Date, x.MediaId });
            foreach (var lineItemGroup in lineItemGroups)
            {
                var sum = new AdfLineItemSummary
                {
                     Date = lineItemGroup.Key.Date,
                     MediaType = new AdfMediaType
                     {
                         ExternalId = lineItemGroup.Key.MediaId,
                     },
                     LineItem = new AdfLineItem
                     {
                         ExternalId = lineItemGroup.Key.LineItemId,
                         Name = lineItemGroup.Key.LineItem,
                         Campaign = new AdfCampaign
                         {
                             ExternalId = byOrder ? lineItemGroup.First().OrderId : lineItemGroup.First().CampaignId,
                             Name = byOrder ? lineItemGroup.First().Order : lineItemGroup.First().Campaign,
                         },
                     },
                };
                SetStats(sum, lineItemGroup);
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
            };
            SetDimensionsForReportSettings(dimensions, settings);
            return AfUtility.CreateReportParams(settings);
        }

        private void ProcessFailedStatsExtraction(Exception e, DateTime fromDate, DateTime toDate)
        {
            Logger.Error(AccountId, e);
            var exception = new AdformFailedStatsLoadingException(fromDate, toDate, AccountId, e, StatsTypeAgg.AdSetArg);
            InvokeProcessFailedExtractionHandlers(exception);
        }
    }
}