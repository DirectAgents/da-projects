﻿using System;
using System.Collections.Generic;
using System.Linq;
using Adform;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.Extracters
{
    public abstract class AdformApiExtracter<T> : Extracter<T>
    {
        protected readonly AdformUtility _afUtility;
        protected readonly DateRange dateRange;
        protected readonly int clientId;

        public AdformApiExtracter(AdformUtility adformUtility, DateRange dateRange, string clientId)
        {
            this._afUtility = adformUtility;
            this.dateRange = dateRange;
            this.clientId = Int32.Parse(clientId);
        }
    }

    public class AdformDailySummaryExtracter : AdformApiExtracter<DailySummary>
    {
        public AdformDailySummaryExtracter(AdformUtility adformUtility, DateRange dateRange, string clientId)
            : base(adformUtility, dateRange, clientId)
        { }

        protected override void Extract()
        {
            Logger.Info("Extracting DailySummaries from Adform API for ({0}) from {1:d} to {2:d}",
                        this.clientId, this.dateRange.FromDate, this.dateRange.ToDate);
            try
            {
                bool rtbOnly = true; // real-time bidding campaigns only
                var parms = _afUtility.CreateReportParams(dateRange.FromDate, dateRange.ToDate, clientId, RTBonly: rtbOnly);
                var basicStatsReportData = _afUtility.GetReportData(parms);
                parms = _afUtility.CreateReportParams(dateRange.FromDate, dateRange.ToDate, clientId, RTBonly: rtbOnly, basicMetrics: false, convMetrics: true, byAdInteractionType: true);
                var convStatsReportData = _afUtility.GetReportData(parms);

                var daysums = EnumerateRows(basicStatsReportData, convStatsReportData);
                Add(daysums);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            End();
        }
        private IEnumerable<DailySummary> EnumerateRows(ReportData basicStatsReportData, ReportData convStatsReportData)
        {
            var rowConverter = new AdformRowConverter(basicStatsReportData, includeConvMetrics: false);
            var daysumDict = rowConverter.EnumerateDailySummaries().ToDictionary(x => x.Date);

            var convStatsTransformer = new AdformTransformer(convStatsReportData, convStatsOnly: true);
            var convSums = convStatsTransformer.EnumerateAdformSummaries();
            var convsumGroups = convSums.GroupBy(x => x.Date);
            // Steps:
            // loop through convsumGroups; get daysum or create blank one
            // then go through daysums that didn't have a convsumGroup
            foreach (var csGroup in convsumGroups)
            {
                DailySummary daysum;
                if (daysumDict.ContainsKey(csGroup.Key))
                    daysum = daysumDict[csGroup.Key];
                else
                    daysum = new DailySummary
                    {
                        Date = csGroup.Key
                    };
                var clickThroughs = csGroup.Where(x => x.AdInteractionType == "Click");
                daysum.PostClickConv = clickThroughs.Sum(x => x.Conversions);
                daysum.PostClickRev = clickThroughs.Sum(x => x.Sales);

                var viewThroughs = csGroup.Where(x => x.AdInteractionType == "Impression");
                daysum.PostViewConv = viewThroughs.Sum(x => x.Conversions);
                daysum.PostViewRev = viewThroughs.Sum(x => x.Sales);

                yield return daysum;
            }
            var convsumDates = convsumGroups.Select(x => x.Key).ToArray();
            var remainingDaySums = daysumDict.Values.Where(ds => !convsumDates.Contains(ds.Date));
            foreach (var daysum in remainingDaySums) // the daily summaries that didn't have any conversion summaries
                yield return daysum;
        }
    }

    public class AdformStrategySummaryExtracter : AdformApiExtracter<StrategySummary>
    {
        public AdformStrategySummaryExtracter(AdformUtility adformUtility, DateRange dateRange, string clientId)
            : base(adformUtility, dateRange, clientId)
        { }

        protected override void Extract()
        {
            Logger.Info("Extracting StrategySummaries from Adform API for ({0}) from {1:d} to {2:d}",
                        this.clientId, this.dateRange.FromDate, this.dateRange.ToDate);
            //TODO: Do X days at a time...?
            try
            {
                var parms = _afUtility.CreateReportParams(dateRange.FromDate, dateRange.ToDate, clientId, byLineItem: true, RTBonly: true,
                                                          basicMetrics: true, convMetrics: true, byAdInteractionType: true);
                var reportData = _afUtility.GetReportData(parms);
                if (reportData != null)
                {
                    var sums = EnumerateRows(reportData);
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
            var adformTransformer = new AdformTransformer(reportData, byLineItem: true);
            var afSums = adformTransformer.EnumerateAdformSummaries();
            var liDateGroups = afSums.GroupBy(x => new { x.LineItem, x.Date });
            foreach (var liDateGroup in liDateGroups)
            {
                var sum = new StrategySummary
                {
                    StrategyName = liDateGroup.Key.LineItem,
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

    public class AdformTDadSummaryExtracter : AdformApiExtracter<TDadSummary>
    {
        public AdformTDadSummaryExtracter(AdformUtility adformUtility, DateRange dateRange, string clientId)
            : base(adformUtility, dateRange, clientId)
        { }

        protected override void Extract()
        {
            Logger.Info("Extracting TDadSummaries from Adform API for ({0}) from {1:d} to {2:d}",
                        this.clientId, this.dateRange.FromDate, this.dateRange.ToDate);
            //TODO: Do X days at a time...?
            try
            {
                var parms = _afUtility.CreateReportParams(dateRange.FromDate, dateRange.ToDate, clientId, byBanner: true, RTBonly: true,
                                                          basicMetrics: true, convMetrics: true, byAdInteractionType: true);
                var reportData = _afUtility.GetReportData(parms);
                if (reportData != null)
                {
                    var sums = EnumerateRows(reportData);
                    Add(sums);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            End();
        }
        private IEnumerable<TDadSummary> EnumerateRows(ReportData reportData)
        {
            var adformTransformer = new AdformTransformer(reportData, byBanner: true);
            var afSums = adformTransformer.EnumerateAdformSummaries();
            var bannerDateGroups = afSums.GroupBy(x => new { x.Banner, x.Date });
            foreach (var bdGroup in bannerDateGroups)
            {
                var sum = new TDadSummary
                {
                    TDadName = bdGroup.Key.Banner,
                    Date = bdGroup.Key.Date,
                    Cost = bdGroup.Sum(x => x.Cost),
                    Impressions = bdGroup.Sum(x => x.Impressions),
                    Clicks = bdGroup.Sum(x => x.Clicks)
                };
                var clickThroughs = bdGroup.Where(x => x.AdInteractionType == "Click");
                sum.PostClickConv = clickThroughs.Sum(x => x.Conversions);
                //sum.PostClickRev = clickThroughs.Sum(x => x.Sales);

                var viewThroughs = bdGroup.Where(x => x.AdInteractionType == "Impression");
                sum.PostViewConv = viewThroughs.Sum(x => x.Conversions);
                //sum.PostViewRev = viewThroughs.Sum(x => x.Sales);

                yield return sum;
            }
        }
    }

}
