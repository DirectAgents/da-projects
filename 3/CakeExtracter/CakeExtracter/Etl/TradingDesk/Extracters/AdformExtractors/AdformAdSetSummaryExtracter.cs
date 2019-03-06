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
    public class AdformAdSetSummaryExtractor : AdformApiBaseExtractor<AdSetSummary>
    {
        private readonly bool byOrder;

        public AdformAdSetSummaryExtractor(AdformUtility adformUtility, DateRange dateRange, ExtAccount account,
            bool byOrder)
            : base(adformUtility, dateRange, account)
        {
            this.byOrder = byOrder;
        }

        protected override void Extract()
        {
            Logger.Info($"Extracting AdSetSummaries from Adform API for ({ClientId}) from {DateRange.FromDate:d} to {DateRange.ToDate:d}");
            //TODO: Do X days at a time...?
            try
            {
                var settings = GetBaseSettings();
                settings.Dimensions.Add(Dimension.LineItem);
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

        private IEnumerable<AdSetSummary> EnumerateRows(ReportData reportData)
        {
            var adformTransformer = new AdformTransformer(reportData, byLineItem: true, byCampaign: !byOrder, byOrder: byOrder);
            var afSums = adformTransformer.EnumerateAdformSummaries();
            var liDateGroups = afSums.GroupBy(x => new { x.LineItem, x.Date });
            foreach (var liDateGroup in liDateGroups)
            {
                var sum = new AdSetSummary
                {
                    StrategyName = byOrder ? liDateGroup.First().Order : liDateGroup.First().Campaign,
                    AdSetName = liDateGroup.Key.LineItem,
                    Date = liDateGroup.Key.Date,
                    Cost = liDateGroup.Sum(x => x.Cost),
                    Impressions = liDateGroup.Sum(x => x.Impressions),
                    Clicks = liDateGroup.Sum(x => x.Clicks)
                };
                var clickThroughs = liDateGroup.Where(x => x.AdInteractionType == "Click");
                sum.PostClickConv = clickThroughs.Sum(x => x.Conversions);
                sum.PostClickRev = clickThroughs.Sum(x => x.Sales);

                var viewThroughs = liDateGroup.Where(x => x.AdInteractionType == "Impression");
                sum.PostViewConv = viewThroughs.Sum(x => x.Conversions);
                sum.PostViewRev = viewThroughs.Sum(x => x.Sales);

                yield return sum;
            }
        }
    }
}