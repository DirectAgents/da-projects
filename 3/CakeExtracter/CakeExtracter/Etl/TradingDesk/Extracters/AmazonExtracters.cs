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
        protected readonly Int64 clientId;

        public AmazonApiExtracter(AmazonUtility amazonUtility, DateRange dateRange, string clientId)
        {
            this._amazonUtility = amazonUtility;
            this.dateRange = dateRange;
            this.clientId = Int64.Parse(clientId);
        }
    }

    public class AmazonDailySummaryExtracter : AmazonApiExtracter<AmazonDailySummary>
    {
        public AmazonDailySummaryExtracter(AmazonUtility amazonUtility, DateRange dateRange, string clientId)
            : base(amazonUtility, dateRange, clientId)
        { }
        private readonly ColumnMapping columnMapping;
        protected override void Extract()
        {
            Logger.Info("Extracting DailySummaries from Amazon API for ({0}) from {1:d} to {2:d}",
                        this.clientId, this.dateRange.FromDate, this.dateRange.ToDate);
            try
            {

                string campaignType = "sponsoredProducts";
                string segment = "";
                string reportDate = "20171204";

                var parms = _amazonUtility.CreateReportParams2("sponsoredProducts", reportDate);
                var reportData = _amazonUtility.GetReportData(parms, "campaigns");

                string reportId = reportData.reportId;

                var downloadInfo = _amazonUtility.SubmitReport(reportId);
                if (downloadInfo!=null)
                    _amazonUtility.GetReport(downloadInfo);
                

                //var daysums = EnumerateRows(basicStatsReportData, convStatsReportData);
                //Add(daysums);

                if (!string.IsNullOrWhiteSpace(downloadInfo.location))
                {
                    var json = _amazonUtility.GetJsonStringFromDownloadFile(downloadInfo.location);
                    var items = JsonConvert.DeserializeObject<List<AmazonDailySummary>>(json);                    
                    Add(items);
                }
                
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            End();
        }
    }

    public class AmazonCampaignSummaryExtracter : AmazonApiExtracter<AmazonCampaignSummary>
    {
        public AmazonCampaignSummaryExtracter(AmazonUtility amazonUtility, DateRange dateRange, string clientId)
            : base(amazonUtility, dateRange, clientId)
        { }

        protected override void Extract()
        {
            Logger.Info("Extracting StrategySummaries from Amazon API for ({0}) from {1:d} to {2:d}",
                        this.clientId, this.dateRange.FromDate, this.dateRange.ToDate);
            try
            {

                var json = _amazonUtility.GetCampaings();                
                var items = JsonConvert.DeserializeObject<List<AmazonCampaignSummary>>(json);

                if (items != null)
                {
                    //var sums = EnumerateRows(items);
                    Add(items);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            End();
        }
        //private IEnumerable<StrategySummary> EnumerateRows(List<AmazonCampaign> reportData)
        //{
        //    var adformTransformer = new AmazonTransformer(reportData, byCampaign: true);
        //    var afSums = adformTransformer.EnumerateAmazonSummaries();
        //    var campDateGroups = afSums.GroupBy(x => new { x.Campaign, x.Date });
        //    foreach (var campDateGroup in campDateGroups)
        //    {
        //        var sum = new StrategySummary
        //        {
        //            StrategyName = campDateGroup.Key.Campaign,
        //            Date = campDateGroup.Key.Date,
        //            Cost = campDateGroup.Sum(x => x.Cost),
        //            Impressions = campDateGroup.Sum(x => x.Impressions),
        //            Clicks = campDateGroup.Sum(x => x.Clicks)
        //        };
        //        var clickThroughs = campDateGroup.Where(x => x.AdInteractionType == "Click");
        //        sum.PostClickConv = clickThroughs.Sum(x => x.Conversions);
        //        sum.PostClickRev = clickThroughs.Sum(x => x.Sales);

        //        var viewThroughs = campDateGroup.Where(x => x.AdInteractionType == "Impression");
        //        sum.PostViewConv = viewThroughs.Sum(x => x.Conversions);
        //        sum.PostViewRev = viewThroughs.Sum(x => x.Sales);

        //        yield return sum;
        //    }
        //}
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
