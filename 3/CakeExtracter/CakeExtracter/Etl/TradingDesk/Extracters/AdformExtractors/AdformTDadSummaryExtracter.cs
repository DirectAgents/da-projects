using System;
using System.Collections.Generic;
using System.Linq;
using Adform;
using Adform.Entities.ReportEntities;
using Adform.Enums;
using Adform.Utilities;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.Extracters.AdformExtractors
{
    public class AdformTDadSummaryExtractor : AdformApiBaseExtractor<TDadSummary>
    {
        public AdformTDadSummaryExtractor(AdformUtility adformUtility, DateRange dateRange, ExtAccount account, bool rtbMediaOnly)
            : base(adformUtility, dateRange, account, rtbMediaOnly)
        {
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
            settings.Dimensions.Add(Dimension.Banner);
            var parameters = AfUtility.CreateReportParams(settings);
            var allReportData = AfUtility.GetReportDataWithLimits(parameters);
            var adFormSums = allReportData.SelectMany(TransformReportData).ToList();
            return adFormSums;
        }

        private IEnumerable<AdformSummary> TransformReportData(ReportData reportData)
        {
            var adFormTransformer = new AdformTransformer(reportData, byBanner: true);
            var afSums = adFormTransformer.EnumerateAdformSummaries();
            return afSums;
        }

        private IEnumerable<TDadSummary> GroupSummaries(IEnumerable<AdformSummary> adFormSums)
        {
            var sums = EnumerateRows(adFormSums);
            var resultSums = AdjustItems(sums);
            return resultSums;
        }

        private IEnumerable<TDadSummary> EnumerateRows(IEnumerable<AdformSummary> afSums)
        {
            var bannerDateGroups = afSums.GroupBy(x => new { x.Banner, x.Date });
            foreach (var bdGroup in bannerDateGroups)
            {
                var sum = new TDadSummary
                {
                    TDadName = bdGroup.Key.Banner,
                };
                SetStats(sum, bdGroup, bdGroup.Key.Date);
                yield return sum;
            }
        }
    }
}