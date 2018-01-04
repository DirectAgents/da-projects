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
using System.Threading;

namespace CakeExtracter.Etl.TradingDesk.Extracters
{
    public abstract class AmazonApiExtracter<T> : Extracter<T>
    {
        protected readonly AmazonUtility _amazonUtility;
        protected readonly DateRange dateRange;
        protected readonly string clientId;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonApiExtracter{T}"/> class.
        /// </summary>
        /// <param name="amazonUtility">The amazon utility.</param>
        /// <param name="date">The date.</param>
        /// <param name="clientId">The client identifier.</param>
        public AmazonApiExtracter(AmazonUtility amazonUtility, DateRange dateRange, string clientId)
        {
            this._amazonUtility = amazonUtility;
            this.dateRange = dateRange;
            this.clientId = clientId;
        }

    }

    #region Daily
    //The daily extracter will load data based on date range and sum up totals of each campaign
    public class AmazonDailySummaryExtracter : AmazonApiExtracter<AmazonDailySummary>
    {
        public AmazonDailySummaryExtracter(AmazonUtility amazonUtility, DateRange dateRange, string clientId)
            : base(amazonUtility, dateRange, clientId)
        { }
        protected override void Extract()
        {
            foreach (var date in dateRange.Dates)
            {
                Logger.Info("Extracting DailySummaries from Amazon API for client id: ({0}) report date {1:d}", this.clientId, date);
                var items = EnumerateRows(date);
                Add(items);
                End();
            }
        }
        private IEnumerable<AmazonDailySummary> EnumerateRows(DateTime date)
        {
            List<AmazonDailySummary> dailyStats = new List<AmazonDailySummary>();
            string reportDate = date.ToString("yyyyMMdd");
            var parms = _amazonUtility.CreateAmazonApiReportParams(reportDate);
            var submissionReportResponse = _amazonUtility.SubmitReport(parms, "campaigns", this.clientId);

            //report could take awhile to be generated, therefore,we are looping until we get status SUCCESS
            while (true)
            {
                var downloadInfo = _amazonUtility.RequestReport(submissionReportResponse.reportId, this.clientId.ToString());

                if (downloadInfo != null && !string.IsNullOrWhiteSpace(downloadInfo.location))
                {
                    var json = _amazonUtility.GetJsonStringFromDownloadFile(downloadInfo.location, this.clientId);
                    dailyStats = JsonConvert.DeserializeObject<List<AmazonDailySummary>>(json);
                    break;
                }
            }
            foreach (var item in dailyStats)
                item.date = date;

            //total up the stats and set the date
            return AggregateDailyStats(dailyStats);
        }
        private IEnumerable<AmazonDailySummary> AggregateDailyStats(List<AmazonDailySummary> dailyItems)
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
    #endregion

    #region Campaign/Strategy
    public class AmazonCampaignSummaryExtracter : AmazonApiExtracter<StrategySummary>
    {
        protected readonly string campaignEid;

        public AmazonCampaignSummaryExtracter(AmazonUtility amazonUtility, DateRange dateRange, string clientId, string campaignEid = null)
            : base(amazonUtility, dateRange, clientId)
        { }

        protected override void Extract()
        {
            foreach (var date in dateRange.Dates)
            {
                Logger.Info("Extracting StrategySummaries from Amazon API for ({0}) for {1:d}", this.clientId, date);
                var items = EnumerateRows(date);
                Add(items);
                End();
            }
        }
        public IEnumerable<StrategySummary> EnumerateRows(DateTime date)
        {
            IEnumerable<AmazonCampaignSummary> campaignSummaries = null;
            var json = _amazonUtility.GetCampaigns(clientId);
            campaignSummaries = JsonConvert.DeserializeObject<List<AmazonCampaignSummary>>(json);


            string reportDate = date.ToString("yyyyMMdd");
            var parms = _amazonUtility.CreateAmazonApiReportParams(reportDate);
            var submissionReportResponse = _amazonUtility.SubmitReport(parms, "campaigns", this.clientId);
            List<AmazonDailySummary> dailyStats = new List<AmazonDailySummary>();
            //report could take awhile to be generated, therefore,we are looping until we get status SUCCESS
            while (true)
            {
                var downloadInfo = _amazonUtility.RequestReport(submissionReportResponse.reportId, this.clientId.ToString());

                if (downloadInfo != null && !string.IsNullOrWhiteSpace(downloadInfo.location))
                {
                    var dailyStatsJson = _amazonUtility.GetJsonStringFromDownloadFile(downloadInfo.location, this.clientId);
                    dailyStats = JsonConvert.DeserializeObject<List<AmazonDailySummary>>(dailyStatsJson);
                    break;
                }
            }
            foreach (var sum in GroupAndEnumerate(dailyStats, campaignSummaries, date))
                yield return sum;


        }

        private IEnumerable<StrategySummary> GroupAndEnumerate(List<AmazonDailySummary> dailyStats, IEnumerable<AmazonCampaignSummary> campaignSummaries, DateTime day)
        {
            //var result = (from cs in campaignSummaries
            //              join ds in dailyStats
            //              on cs.campaignId equals ds.campaignId into q
            //              select new StrategySummary
            //              {
            //                  Date = day,
            //                  StrategyEid = cs.campaignId.ToString(),
            //                  StrategyName = cs.name,
            //                  Impressions = q.Sum(g => g.impressions),
            //                  Clicks = q.Sum(g => g.clicks),
            //                  Cost = q.Sum(g => g.cost)
            //              });

            //yield return result;
            foreach (var campaign in campaignSummaries)
            {
                var groupedRows = dailyStats.GroupBy(x => x.campaignId == campaign.campaignId);
                foreach (var group in groupedRows)
                {
                    var sum = new StrategySummary
                    {
                        Date = day,
                        StrategyEid = campaign.campaignId.ToString(),
                        StrategyName = campaign.name,
                        Impressions = group.Sum(g => g.impressions),
                        Clicks = group.Sum(g => g.clicks),
                        Cost = group.Sum(g => g.cost)
                    };
                    yield return sum;
                }
            }

            // if StrategyEid's aren't all filled in...
            // if (campaignSummaries.Any(r => string.IsNullOrWhiteSpace(r.campaignId.ToString())))
            //{
            //var groupedRows = dailyStats.GroupBy(r => new { r.date, r.campaignId});
            //foreach (var group in groupedRows)
            //{
            //    var sum = new StrategySummary
            //    {
            //        Date = group.Key.date,
            //        StrategyEid = group.Key.campaignId.ToString(),
            //        StrategyName = group.Key.StrategyName,
            //        Impressions = group.Sum(g => g.Impressions),
            //        Clicks = group.Sum(g => g.Clicks),
            //        Cost = group.Sum(g => g.Cost)
            //    };
            //    yield return sum;
            //}
            //}
            //else // if all StrategyEid's are filled in...
            //{
            //    var groupedRows = dailyStats.GroupBy(r => new { r.date, r.campaignId });
            //    foreach (var group in groupedRows)
            //    {
            //        string stratName = null;
            //        if (group.Count() == 1)
            //            stratName = group.First().StrategyName;
            //        var sum = new StrategySummary
            //        {
            //            Date = group.Key.Date,
            //            StrategyEid = group.Key.StrategyEid,
            //            StrategyName = stratName,
            //            Impressions = group.Sum(g => g.Impressions),
            //            Clicks = group.Sum(g => g.Clicks),
            //            Cost = group.Sum(g => g.Cost)
            //        };
            //        yield return sum;
            //    }
            //}
        }
    }
    #endregion

    #region AdSet & AdSet Summary
    public class AmazonAdSetExtracter : AmazonApiExtracter<AdSetSummary>
    {
        public AmazonAdSetExtracter(AmazonUtility amazonUtility, DateRange dateRange, string clientId)
            : base(amazonUtility, dateRange, clientId)
        { }

        protected override void Extract()
        {
            foreach (var date in dateRange.Dates)
            {
                Logger.Info("Extracting AdSetSummaries from Amazon API for ({0}) for reporting date: {1:d}", this.clientId, date);
                var items = EnumerateRows(date);
                Add(items);
                End();
            }
        }

        private IEnumerable<AdSetSummary> EnumerateRows(DateTime date)
        {
            List<AmazonAdSet> adsets = new List<AmazonAdSet>();
            IEnumerable<AmazonKeyword> keywords = null;

            var json = _amazonUtility.GetKeywords(clientId);
            keywords = JsonConvert.DeserializeObject<List<AmazonKeyword>>(json);

            foreach (var keyword in keywords)
            {
                adsets.Add(new AmazonAdSet
                {
                    KeywordText = keyword.KeywordText,
                    CampaignId = keyword.CampaignId.ToString(),
                    KeywordId = keyword.KeywordId.ToString()
                });
            }

            string reportDate = date.ToString("yyyyMMdd");
            var parms = _amazonUtility.CreateAmazonApiReportParams(reportDate);
            var submissionReportResponse = _amazonUtility.SubmitReport(parms, "keywords", this.clientId);
            List<AmazonKeywordMetric> dailyStats = new List<AmazonKeywordMetric>();

            //report could take awhile to be generated, therefore,we are looping until we get status SUCCESS
            while (true)
            {
                var downloadInfo = _amazonUtility.RequestReport(submissionReportResponse.reportId, this.clientId.ToString());

                if (downloadInfo != null && !string.IsNullOrWhiteSpace(downloadInfo.location))
                {
                    var dailyStatsJson = _amazonUtility.GetJsonStringFromDownloadFile(downloadInfo.location, this.clientId);
                    dailyStats = JsonConvert.DeserializeObject<List<AmazonKeywordMetric>>(dailyStatsJson);
                    break;
                }
            }
            foreach (var sum in GroupAndEnumerate(dailyStats, adsets, date))
                yield return sum;
        }
        private IEnumerable<AdSetSummary> GroupAndEnumerate(List<AmazonKeywordMetric> dailyStats, IEnumerable<AmazonAdSet> adsets, DateTime day)
        {
            foreach (var ad in adsets)
            {
                var groupedRows = dailyStats.GroupBy(x => x.KeywordId == ad.KeywordId);
                foreach (var group in groupedRows)
                {
                    var sum = new AdSetSummary
                    {
                        Date = day,
                        AdSetEid = ad.KeywordId.ToString(),
                        AdSetName = ad.KeywordText,
                        Impressions = group.Sum(g => g.Impressions),
                        Clicks = group.Sum(g => g.Clicks),
                        Cost = group.Sum(g => g.Cost)
                    };
                    yield return sum;
                }
            }
        }

    }
    #endregion

    #region Ad and Ad Summary / Creative
    public class AmazonAdExtrater : AmazonApiExtracter<TDadSummary>
    {
        public AmazonAdExtrater(AmazonUtility amazonUtility, DateRange dateRange, string clientId)
            : base(amazonUtility, dateRange, clientId)
        { }

        protected override void Extract()
        {
            foreach (var date in dateRange.Dates)
            {
                Logger.Info("Extracting AdSetSummaries from Amazon API for ({0}) for reporting date: {1:d}", this.clientId, this.dateRange);
                var items = EnumerateRows(date);
                Add(items);
                End();
            }
        }
        private IEnumerable<TDadSummary> EnumerateRows(DateTime date)
        {
            List<TDad> ads = new List<TDad>();
            IEnumerable<AmazonProductAds> productAds = null;

            var json = _amazonUtility.GetProductAds(clientId);
            productAds = JsonConvert.DeserializeObject<List<AmazonProductAds>>(json);

            foreach (var productAd in productAds)
            {
                ads.Add(new TDad
                {
                    ExternalId = productAd.AdId,
                    Name = productAd.Asin
                });
            }
            string reportDate = date.ToString("yyyyMMdd");
            var parms = _amazonUtility.CreateAmazonApiReportParams(reportDate);
            var submissionReportResponse = _amazonUtility.SubmitReport(parms, "productAds", this.clientId);
            List<AmazonAdDailySummary> dailyStats = new List<AmazonAdDailySummary>();
            //report could take awhile to be generated, therefore,we are looping until we get status SUCCESS
            while (true)
            {
                var downloadInfo = _amazonUtility.RequestReport(submissionReportResponse.reportId, this.clientId.ToString());

                if (downloadInfo != null && !string.IsNullOrWhiteSpace(downloadInfo.location))
                {
                    var dailyStatsJson = _amazonUtility.GetJsonStringFromDownloadFile(downloadInfo.location, this.clientId);
                    dailyStats = JsonConvert.DeserializeObject<List<AmazonAdDailySummary>>(dailyStatsJson);
                    break;
                }
            }
            foreach (var sum in GroupAndEnumerate(dailyStats, ads, date))
                yield return sum;
        }
        private IEnumerable<TDadSummary> GroupAndEnumerate(List<AmazonAdDailySummary> productAdsDailyStats, IEnumerable<TDad> productAds, DateTime day)
        {
            foreach (var ad in productAds)
            {
                var groupedRows = productAdsDailyStats.GroupBy(x => x.adId == ad.ExternalId);
                foreach (var group in groupedRows)
                {
                    var sum = new TDadSummary
                    {
                        Date = day,
                        TDadEid = ad.ExternalId.ToString(),
                        TDadName = ad.Name,
                        Impressions = group.Sum(g => g.impressions),
                        Clicks = group.Sum(g => g.clicks),
                        Cost = group.Sum(g => g.cost)
                    };
                    yield return sum;
                }
            }
        }
    }
    #endregion

}
