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
    public class AdformAdSetSummaryExtractor : AdformApiBaseExtractor<AdfLineItemSummary>
    {
        private readonly bool byOrder;

        public AdformAdSetSummaryExtractor(AdformUtility adformUtility, DateRange dateRange, ExtAccount account, bool byOrder)
            : base(adformUtility, dateRange, account)
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
            settings.Dimensions.Add(Dimension.LineItemId);
            settings.Dimensions.Add(byOrder ? Dimension.Order : Dimension.Campaign);
            settings.Dimensions.Add(byOrder ? Dimension.OrderId : Dimension.CampaignId);
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

        private IEnumerable<AdfLineItemSummary> GroupSummaries(IEnumerable<AdformSummary> adFormSums)
        {
            var sums = EnumerateRows(adFormSums);
            var resultSums = AdjustItems(sums);
            return resultSums;
        }

        private IEnumerable<AdfLineItemSummary> EnumerateRows(IEnumerable<AdformSummary> afSums)
        {
            var liDateGroups = afSums.GroupBy(x => new { x.CampaignId, x.Campaign, x.LineItemId, x.LineItem, x.Date, x.MediaId, x.Media });
            foreach (var liDateGroup in liDateGroups)
            {
                var sum = new AdfLineItemSummary
                {
                     Date = liDateGroup.Key.Date,
                     MediaType = new AdfMediaType
                     {
                         ExternalId = liDateGroup.Key.MediaId,
                         Name = liDateGroup.Key.Media,
                     },
                     LineItem = new AdfLineItem
                     {
                         ExternalId = liDateGroup.Key.LineItemId,
                         Name = liDateGroup.Key.LineItem,
                         Campaign = new AdfCampaign
                         {
                             ExternalId = byOrder ? liDateGroup.First().OrderId : liDateGroup.First().CampaignId,
                             Name = byOrder ? liDateGroup.First().Order : liDateGroup.First().Campaign,
                         },
                     },
                };
                SetStats(sum, liDateGroup/*, liDateGroup.Key.Date*/);
                yield return sum;
            }
        }
    }
}