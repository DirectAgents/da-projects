using System;
using System.Collections.Generic;
using System.Linq;
using Adform.Entities;
using Adform.Entities.ReportEntities;
using Adform.Entities.ReportEntities.ReportParameters;
using Adform.Enums;
using Adform.Utilities;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Adform;
using DirectAgents.Domain.Entities.CPProg.Adform.Summaries;

namespace CakeExtracter.Etl.Adform.Extractors
{
    /// <inheritdoc />
    /// <summary>
    /// Adform Campaign summary extractor.
    /// </summary>
    public class AdformStrategySummaryExtractor : AdformApiBaseExtractor<AdfCampaignSummary>
    {
        private readonly bool byOrder;

        /// <inheritdoc cref="AdformApiBaseExtractor{T}"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="AdformStrategySummaryExtractor"/> class.
        /// </summary>
        /// <param name="adformUtility">API utility.</param>
        /// <param name="dateRange">Date range.</param>
        /// <param name="account">Account.</param>
        /// <param name="byOrder">Flag indicates to extract order dimension instead campaign dimension.</param>
        public AdformStrategySummaryExtractor(AdformUtility adformUtility, DateRange dateRange, ExtAccount account, bool byOrder = false)
            : base(adformUtility, dateRange, account)
        {
            this.byOrder = byOrder;
        }

        protected override void Extract()
        {
            var additionInfo = byOrder ? "Orders" : "Campaigns";
            Logger.Info(AccountId, $"Extracting CampaignSummaries from Adform API for ({ClientId}) from {DateRange.FromDate:d} to {DateRange.ToDate:d} - {additionInfo}");
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

        private IEnumerable<AdformReportSummary> ExtractData()
        {
            var reportData = GetReportData();
            return reportData.SelectMany(TransformReportData).ToList();
        }

        private IEnumerable<AdformReportSummary> TransformReportData(ReportData reportData)
        {
            var adFormTransformer = new AdformReportDataTransformer(reportData, byCampaign: !byOrder, byOrder: byOrder);
            var afSums = adFormTransformer.EnumerateAdformSummaries();
            return afSums;
        }

        private IEnumerable<AdfCampaignSummary> GroupSummaries(IEnumerable<AdformReportSummary> adFormSums)
        {
            var sums = EnumerateRows(adFormSums);
            var resultSums = AdjustItems(sums);
            return resultSums;
        }

        private IEnumerable<AdfCampaignSummary> EnumerateRows(IEnumerable<AdformReportSummary> afSums)
        {
            var campaignGroups = afSums.GroupBy(x => new { x.Campaign, x.CampaignId, x.Order, x.OrderId, x.Date, x.MediaId });
            foreach (var campaignGroup in campaignGroups)
            {
                var sum = new AdfCampaignSummary
                {
                    Date = campaignGroup.Key.Date,
                    Campaign = new AdfCampaign
                    {
                        Name = byOrder ? campaignGroup.Key.Order : campaignGroup.Key.Campaign,
                        ExternalId = byOrder ? campaignGroup.Key.OrderId : campaignGroup.Key.CampaignId,
                    },
                    MediaType = new AdfMediaType
                    {
                        ExternalId = campaignGroup.Key.MediaId,
                    },
                };
                SetStats(sum, campaignGroup);
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
            };
            SetDimensionsForReportSettings(dimensions, settings);
            return AfUtility.CreateReportParams(settings);
        }
    }
}