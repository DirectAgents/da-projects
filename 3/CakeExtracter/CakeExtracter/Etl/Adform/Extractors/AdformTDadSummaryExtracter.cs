using System;
using System.Collections.Generic;
using System.Linq;
using Adform.Entities;
using Adform.Entities.ReportEntities;
using Adform.Entities.ReportEntities.ReportParameters;
using Adform.Enums;
using Adform.Utilities;
using CakeExtracter.Common;
using CakeExtracter.Etl.Adform.Exceptions;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Adform;
using DirectAgents.Domain.Entities.CPProg.Adform.Summaries;

namespace CakeExtracter.Etl.Adform.Extractors
{
    /// <inheritdoc />
    /// <summary>
    /// Adform Banner summary extractor.
    /// </summary>
    public class AdformTDadSummaryExtractor : AdformApiBaseExtractor<AdfBannerSummary>
    {
        /// <inheritdoc cref="AdformApiBaseExtractor{T}"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="AdformTDadSummaryExtractor"/> class.
        /// </summary>
        /// <param name="adformUtility">API utility.</param>
        /// <param name="dateRange">Date range.</param>
        /// <param name="account">Account.</param>
        public AdformTDadSummaryExtractor(AdformUtility adformUtility, DateRange dateRange, ExtAccount account)
            : base(adformUtility, dateRange, account)
        {
        }

        /// <inheritdoc/>
        protected override void Extract()
        {
            Logger.Info(AccountId, $"Extracting BannerSummaries from Adform API for ({ClientId}) from {DateRange.FromDate:d} to {DateRange.ToDate:d}");
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
            var adFormTransformer = new AdformReportDataTransformer(reportData, byBanner: true);
            var afSums = adFormTransformer.EnumerateAdformSummaries();
            return afSums;
        }

        private IEnumerable<AdfBannerSummary> GroupSummaries(IEnumerable<AdformReportSummary> adFormSums)
        {
            var sums = EnumerateRows(adFormSums);
            var resultSums = AdjustItems(sums);
            return resultSums;
        }

        private IEnumerable<AdfBannerSummary> EnumerateRows(IEnumerable<AdformReportSummary> afSums)
        {
            var bannerGroups = afSums.GroupBy(x => new { x.BannerId, x.Banner, x.Date, x.MediaId });
            foreach (var bannerGroup in bannerGroups)
            {
                var sum = new AdfBannerSummary
                {
                    Date = bannerGroup.Key.Date,
                    MediaType = new AdfMediaType
                    {
                        ExternalId = bannerGroup.Key.MediaId,
                    },
                    Banner = new AdfBanner
                    {
                        ExternalId = bannerGroup.Key.BannerId,
                        Name = bannerGroup.Key.Banner,
                    },
                };
                SetStats(sum, bannerGroup);
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
                Dimension.BannerId,
                Dimension.Banner,
            };
            SetDimensionsForReportSettings(dimensions, settings);
            return AfUtility.CreateReportParams(settings);
        }

        private void ProcessFailedStatsExtraction(Exception e, DateTime fromDate, DateTime toDate)
        {
            Logger.Error(AccountId, e);
            var exception = new AdformFailedStatsLoadingException(fromDate, toDate, AccountId, e, byBanner: true);
            InvokeProcessFailedExtractionHandlers(exception);
        }
    }
}