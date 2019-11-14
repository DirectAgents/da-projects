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
    public class AdformStrategySummaryExtractor : AdformApiBaseExtractor<AdfCampaignSummary>
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
            Logger.Info(AccountId, $"Extracting StrategySummaries from Adform API for ({ClientId}) from {DateRange.FromDate:d} to {DateRange.ToDate:d} - {additionInfo}");
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
            settings.Dimensions.Add(byOrder ? Dimension.Order : Dimension.Campaign);
            settings.Dimensions.Add(byOrder ? Dimension.OrderId : Dimension.CampaignId);
            var parameters = AfUtility.CreateReportParams(settings);
            var allReportData = AfUtility.GetReportDataWithLimits(parameters);
            var adFormSums = allReportData.SelectMany(TransformReportData).ToList();
            return adFormSums;
        }

        private IEnumerable<AdformSummary> TransformReportData(ReportData reportData)
        {
            var adFormTransformer = new AdformTransformer(reportData, byCampaign: !byOrder, byOrder: byOrder);
            var afSums = adFormTransformer.EnumerateAdformSummaries();
            return afSums;
        }

        private IEnumerable<AdfCampaignSummary> GroupSummaries(IEnumerable<AdformSummary> adFormSums)
        {
            var sums = EnumerateRows(adFormSums);
            var resultSums = AdjustItems(sums);
            return resultSums;
        }

        private IEnumerable<AdfCampaignSummary> EnumerateRows(IEnumerable<AdformSummary> afSums)
        {
            var campDateGroups = afSums.GroupBy(x => new { x.Campaign, x.CampaignId, x.Order, x.OrderId, x.Date, x.MediaId, x.Media });
            foreach (var campDateGroup in campDateGroups)
            {
                var sum = new AdfCampaignSummary
                {
                    Date = campDateGroup.Key.Date,
                    Campaign = new AdfCampaign
                    {
                        Name = byOrder ? campDateGroup.Key.Order : campDateGroup.Key.Campaign,
                        ExternalId = byOrder ? campDateGroup.Key.OrderId : campDateGroup.Key.CampaignId,
                    },
                    MediaType = new AdfMediaType
                    {
                        ExternalId = campDateGroup.Key.MediaId,
                        Name = campDateGroup.Key.Media,
                    },
                };
                SetStats(sum, campDateGroup/*, campDateGroup.Key.Date*/);
                yield return sum;
            }
        }
    }
}