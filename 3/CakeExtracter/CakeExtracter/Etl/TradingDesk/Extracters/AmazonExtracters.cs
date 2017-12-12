using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;
using Amazon;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Amazon.Entities;

namespace CakeExtracter.Etl.TradingDesk.Extracters
{
    public abstract class AmazonApiExtracter<T> : Extracter<T>
    {
        protected readonly AmazonUtility _amazonUtility;
        protected readonly DateRange dateRange;
        protected readonly string clientId;

        public AmazonApiExtracter(AmazonUtility amazonUtility, DateRange dateRange, string clientId)
        {
            this._amazonUtility = amazonUtility;
            this.dateRange = dateRange;
            this.clientId = clientId;
        }

        public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }
    }

    //The daily extracter will load data based on date range and sum up totals of each campaign
    public class AmazonDailySummaryExtracter : AmazonApiExtracter<AmazonDailySummary>
    {
        public AmazonDailySummaryExtracter(AmazonUtility amazonUtility, DateRange dateRange, string clientId)
            : base(amazonUtility, dateRange, clientId)
        { }
        protected override void Extract()
        {
            Logger.Info("Extracting DailySummaries from Amazon API for ({0}) from {1:d} to {2:d}",
                        this.clientId, this.dateRange.FromDate, this.dateRange.ToDate);
            try
            {
                List<AmazonDailySummary> dailyStats = new List<AmazonDailySummary>();

                //loop through the dates
                foreach (DateTime day in EachDay(this.dateRange.FromDate, this.dateRange.ToDate))
                {
                    string reportDate = day.ToString("yyyyMMdd");
                    var parms = _amazonUtility.CreateAmazonApiReportParams(reportDate);
                    var submissionReportResponse = _amazonUtility.SubmitReport(parms, "campaigns");

                    //report could take awhile to be generated, therefore,we are looping until we get status SUCCESS
                    while (true)
                    {
                        var downloadInfo = _amazonUtility.RequestReport(submissionReportResponse.reportId, this.clientId.ToString());

                        if (downloadInfo!=null && !string.IsNullOrWhiteSpace(downloadInfo.location))
                        {
                            var json = _amazonUtility.GetJsonStringFromDownloadFile(downloadInfo.location);
                            dailyStats = JsonConvert.DeserializeObject<List<AmazonDailySummary>>(json);
                            break;
                        }                       
                    }
                    foreach (var item in dailyStats)
                        item.date = day;

                    //total up the stats and set the date
                    var dailySums = EnumerateRows(dailyStats);

                    Add(dailySums);
                }
                
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            End();
        }
        private IEnumerable<AmazonDailySummary> EnumerateRows(List<AmazonDailySummary> dailyItems)
        {
            var campDateGroups = dailyItems.GroupBy(x => new { x.date });
            foreach (var campDateGroup in campDateGroups)
            {
                var sum = new AmazonDailySummary
                {
                    date = campDateGroup.Key.date,
                    cost = campDateGroup.Sum(x => x.cost),
                    impressions = campDateGroup.Sum(x => x.impressions),
                    clicks = campDateGroup.Sum(x => x.clicks)
                };
                yield return sum;
            }
        }
    }

    public class AmazonCampaignSummaryExtracter : AmazonApiExtracter<AmazonCampaignSummary>
    {
        protected readonly string campaignEid;

        public AmazonCampaignSummaryExtracter(AmazonUtility amazonUtility, DateRange dateRange, string clientId, string campaignEid = null)
            : base(amazonUtility, dateRange, clientId)
        { }

        protected override void Extract()
        {
            Logger.Info("Extracting StrategySummaries from Amazon API for ({0}) from {1:d} to {2:d}",
                        this.clientId, this.dateRange.FromDate, this.dateRange.ToDate);
            try
            {
                var json = _amazonUtility.GetCampaings(clientId);                
                var items = JsonConvert.DeserializeObject<List<AmazonCampaignSummary>>(json);

                foreach (var item in items)
                {
                    foreach (DateTime day in EachDay(this.dateRange.FromDate, this.dateRange.ToDate))
                    {
                        //loop through for each campaign
                        var campSums = _amazonUtility.AmazonCampaignDailySummaries(day, clientId, this.campaignEid);
                        if (campSums != null)
                            Add(campSums);
                    }
                }
                Add(items);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            End();
        }
    }

    public class AmazonAdSetSummaryExtracter : AmazonApiExtracter<AdSetSummary>
    {
        public AmazonAdSetSummaryExtracter(AmazonUtility amazonUtility, DateRange dateRange, string clientId)
            : base(amazonUtility, dateRange, clientId)
        { }

        protected override void Extract()
        {
            Logger.Info("Extracting AdSetSummaries from Amazon API for ({0}) from {1:d} to {2:d}",
                        this.clientId, this.dateRange.FromDate, this.dateRange.ToDate);
            //TODO: Do X days at a time...?
            try
            {
                //var parms = _amazonUtility.CreateReportParams(dateRange.FromDate, dateRange.ToDate, clientId, byLineItem: true, RTBonly: true,
                //                                          basicMetrics: true, convMetrics: true, byAdInteractionType: true);
                //var reportData = _amazonUtility.GetReportData(parms);
                //if (reportData != null)
                //{
                //    var sums = EnumerateRows(reportData);
                //    Add(sums);
                //}
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            End();
        }
        private IEnumerable<AdSetSummary> EnumerateRows(ReportData reportData)
        {
            var adformTransformer = new AmazonTransformer(reportData, byLineItem: true);
            var afSums = adformTransformer.EnumerateAmazonSummaries();
            var liDateGroups = afSums.GroupBy(x => new { x.LineItem, x.Date });
            foreach (var liDateGroup in liDateGroups)
            {
                var sum = new AdSetSummary
                {
                    //StrategyName = ?
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

    public class AmazonTDadSummaryExtracter : AmazonApiExtracter<TDadSummary>
    {
        public AmazonTDadSummaryExtracter(AmazonUtility amazonUtility, DateRange dateRange, string clientId)
            : base(amazonUtility, dateRange, clientId)
        { }

        protected override void Extract()
        {
            Logger.Info("Extracting TDadSummaries from Amazon API for ({0}) from {1:d} to {2:d}",
                        this.clientId, this.dateRange.FromDate, this.dateRange.ToDate);
            //TODO: Do X days at a time...?
            try
            {
                //var parms = _amazonUtility.CreateReportParams(dateRange.FromDate, dateRange.ToDate, clientId, byBanner: true, RTBonly: true,
                //                                          basicMetrics: true, convMetrics: true, byAdInteractionType: true);
                //var reportData = _amazonUtility.GetReportData(parms);
                //if (reportData != null)
                //{
                //    var sums = EnumerateRows(reportData);
                //    Add(sums);
                //}
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            End();
        }
        private IEnumerable<TDadSummary> EnumerateRows(ReportData reportData)
        {
            var adformTransformer = new AmazonTransformer(reportData, byBanner: true);
            var afSums = adformTransformer.EnumerateAmazonSummaries();
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
