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
    public class AdformTDadSummaryExtractor : AdformApiBaseExtractor<AdfBannerSummary>
    {
        private readonly bool byOrder;

        public AdformTDadSummaryExtractor(AdformUtility adformUtility, DateRange dateRange, ExtAccount account, bool byOrder)
            : base(adformUtility, dateRange, account)
        {
            this.byOrder = byOrder;
        }

        protected override void Extract()
        {
            Logger.Info(AccountId, $"Extracting TDadSummaries from Adform API for ({ClientId}) from {DateRange.FromDate:d} to {DateRange.ToDate:d}");
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
            var dimensions = new List<Dimension>
            {
                Dimension.BannerId,
                Dimension.Banner,
                Dimension.LineItemId,
            };
            SetDimensionsForReportSettings(dimensions, settings);
            //settings.Dimensions.Add(byOrder ? Dimension.OrderId : Dimension.CampaignId);
            var parameters = AfUtility.CreateReportParams(settings);
            var allReportData = AfUtility.GetReportDataWithLimits(parameters);
            var adFormSums = allReportData.SelectMany(TransformReportData).ToList();
            return adFormSums;
        }

        private IEnumerable<AdformSummary> TransformReportData(ReportData reportData)
        {
            var adFormTransformer = new AdformTransformer(reportData, byLineItem: true, byBanner: true);
            var afSums = adFormTransformer.EnumerateAdformSummaries();
            return afSums;
        }

        private IEnumerable<AdfBannerSummary> GroupSummaries(IEnumerable<AdformSummary> adFormSums)
        {
            var sums = EnumerateRows(adFormSums);
            var resultSums = AdjustItems(sums);
            return resultSums;
        }

        private IEnumerable<AdfBannerSummary> EnumerateRows(IEnumerable<AdformSummary> afSums)
        {
            //var bannerGroups = afSums.GroupBy(x => new { x.CampaignId, x.Campaign, x.LineItemId, x.LineItem, x.BannerId, x.Banner, x.Date, x.MediaId, x.Media });
            var bannerGroups = afSums.GroupBy(x => new { x.LineItemId, x.BannerId, x.Banner, x.Date, x.MediaId });
            foreach (var bannerGroup in bannerGroups)
            {
                var sum = new AdfBannerSummary
                {
                    Date = bannerGroup.Key.Date,
                    MediaType = new AdfMediaType
                    {
                        ExternalId = bannerGroup.Key.MediaId,
                        //Name = bannerGroup.Key.Media,
                    },
                    Banner = new AdfBanner
                    {
                        ExternalId = bannerGroup.Key.BannerId,
                        Name = bannerGroup.Key.Banner,
                        LineItem = new AdfLineItem
                        {
                            ExternalId = bannerGroup.Key.LineItemId,
                            //Name = bannerGroup.Key.LineItem,
                            //Campaign = new AdfCampaign
                            //{
                            //    ExternalId = bannerGroup.First().CampaignId,
                            //    Name = bannerGroup.First().Campaign,
                            //},
                        },
                    },
                };
                SetStats(sum, bannerGroup);
                yield return sum;
            }
        }
    }
}