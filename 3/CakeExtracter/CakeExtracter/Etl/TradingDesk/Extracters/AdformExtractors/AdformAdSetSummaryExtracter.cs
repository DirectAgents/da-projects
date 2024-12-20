﻿using System;
using System.Collections.Generic;
using System.Linq;
using Adform.Outdated.Entities.ReportEntities;
using Adform.Outdated.Entities;
using Adform.Outdated.Enums;
using Adform.Outdated.Utilities;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.Extracters.AdformExtractors
{
    public class AdformAdSetSummaryExtractor : AdformApiBaseExtractor<AdSetSummary>
    {
        private readonly bool byOrder;

        public AdformAdSetSummaryExtractor(
            AdformUtility adformUtility,
            DateRange dateRange,
            ExtAccount account,
            bool rtbMediaOnly,
            bool areAllStatsForAllMediaTypes,
            bool byOrder)
            : base(adformUtility, dateRange, account, rtbMediaOnly, areAllStatsForAllMediaTypes)
        {
            this.byOrder = byOrder;
        }

        protected override void Extract()
        {
            Logger.Info(AccountId, $"Extracting AdSetSummaries from Adform API for ({ClientId}) from {DateRange.FromDate:d} to {DateRange.ToDate:d}");
            //TODO: Do X days at a time...?
            try
            {
                var data = ExtractData();
                var sums = GroupSummaries(data);
                Add(sums);
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
            settings.Dimensions.Add(Dimension.LineItem);
            settings.Dimensions.Add(byOrder ? Dimension.Order : Dimension.Campaign);
            var parameters = AfUtility.CreateReportParams(settings);
            var allReportData = AfUtility.GetReportDataWithLimits(parameters);
            var adFormSums = allReportData.SelectMany(TransformReportData).ToList();
            return adFormSums;
        }

        private IEnumerable<AdformSummary> TransformReportData(ReportData reportData)
        {
            var adFormTransformer = new AdformTransformer(reportData, byLineItem: true, byCampaign: !byOrder, byOrder: byOrder);
            var afSums = adFormTransformer.EnumerateAdformSummaries();
            return afSums;
        }

        private IEnumerable<AdSetSummary> GroupSummaries(IEnumerable<AdformSummary> adFormSums)
        {
            var sums = EnumerateRows(adFormSums);
            var resultSums = AdjustItems(sums);
            return resultSums;
        }

        private IEnumerable<AdSetSummary> EnumerateRows(IEnumerable<AdformSummary> afSums)
        {
            var liDateGroups = afSums.GroupBy(x => new { x.LineItem, x.Date });
            foreach (var liDateGroup in liDateGroups)
            {
                var sum = new AdSetSummary
                {
                    StrategyName = byOrder ? liDateGroup.First().Order : liDateGroup.First().Campaign,
                    AdSetName = liDateGroup.Key.LineItem,
                };
                SetStats(sum, liDateGroup, liDateGroup.Key.Date);
                yield return sum;
            }
        }
    }
}