using System;
using System.Collections.Generic;
using System.Linq;
using Adform;
using Adform.Entities.ReportEntities;
using Adform.Enums;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.Extracters.AdformExtractors
{
    public class AdformStrategySummaryExtractor : AdformApiBaseExtractor<StrategySummary>
    {
        private readonly bool byOrder;

        public AdformStrategySummaryExtractor(AdformUtility adformUtility, DateRange dateRange, ExtAccount account, bool byOrder = false)
            : base(adformUtility, dateRange, account)
        {
            this.byOrder = byOrder;
        }

        protected override void Extract()
        {
            var additionInfo = byOrder ? "Orders" : "Campaigns";
            Logger.Info($"Extracting StrategySummaries from Adform API for ({ClientId}) from {DateRange.FromDate:d} to {DateRange.ToDate:d} - {additionInfo}");
            //TODO: Do X days at a time...?
            try
            {
                var data = ExtractData();
                var sums = GroupSummaries(data);
                Add(sums);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            End();
        }

        private IEnumerable<AdformSummary> ExtractData()
        {
            var settings = GetBaseSettings();
            settings.Dimensions.Add(byOrder ? Dimension.Order : Dimension.Campaign);
            var parms = AfUtility.CreateReportParams(settings);
            var allReportData = AfUtility.GetReportDataWithPaging(parms);
            var adFormSums = allReportData.SelectMany(TransformReportData).ToList();
            return adFormSums;
        }

        private IEnumerable<AdformSummary> TransformReportData(ReportData reportData)
        {
            var adFormTransformer = new AdformTransformer(reportData, byCampaign: !byOrder, byOrder: byOrder);
            var afSums = adFormTransformer.EnumerateAdformSummaries();
            return afSums;
        }

        private IEnumerable<StrategySummary> GroupSummaries(IEnumerable<AdformSummary> adFormSums)
        {
            var sums = EnumerateRows(adFormSums);
            var resultSums = AdjustItems(sums);
            return resultSums;
        }

        private IEnumerable<StrategySummary> EnumerateRows(IEnumerable<AdformSummary> afSums)
        {
            var campDateGroups = afSums.GroupBy(x => new {x.Campaign, x.Order, x.Date});
            foreach (var campDateGroup in campDateGroups)
            {
                var sum = new StrategySummary
                {
                    StrategyName = byOrder ? campDateGroup.Key.Order : campDateGroup.Key.Campaign
                };
                SetStats(sum, campDateGroup, campDateGroup.Key.Date);
                yield return sum;
            }
        }
    }
}