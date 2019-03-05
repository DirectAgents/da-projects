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
                var settings = GetBaseSettings();
                settings.Dimensions.Add(byOrder ? Dimension.Order : Dimension.Campaign);
                var parms = AfUtility.CreateReportParams(settings);
                foreach (var reportData in AfUtility.GetReportDataWithPaging(parms))
                {
                    var sums = EnumerateRows(reportData);
                    sums = AdjustItems(sums);
                    Add(sums);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            End();
        }

        private IEnumerable<StrategySummary> EnumerateRows(ReportData reportData)
        {
            var adformTransformer = new AdformTransformer(reportData, byCampaign: !byOrder, byOrder: byOrder);
            var afSums = adformTransformer.EnumerateAdformSummaries();
            var campDateGroups = afSums.GroupBy(x => new { x.Campaign, x.Order, x.Date });
            foreach (var campDateGroup in campDateGroups)
            {
                var sum = new StrategySummary
                {
                    StrategyName = byOrder ? campDateGroup.Key.Order : campDateGroup.Key.Campaign,
                    Date = campDateGroup.Key.Date,
                    Cost = campDateGroup.Sum(x => x.Cost),
                    Impressions = campDateGroup.Sum(x => x.Impressions),
                    Clicks = campDateGroup.Sum(x => x.Clicks)
                };
                var clickThroughs = campDateGroup.Where(x => x.AdInteractionType == "Click");
                sum.PostClickConv = clickThroughs.Sum(x => x.Conversions);
                sum.PostClickRev = clickThroughs.Sum(x => x.Sales);

                var viewThroughs = campDateGroup.Where(x => x.AdInteractionType == "Impression");
                sum.PostViewConv = viewThroughs.Sum(x => x.Conversions);
                sum.PostViewRev = viewThroughs.Sum(x => x.Sales);

                yield return sum;
            }
        }
    }
}