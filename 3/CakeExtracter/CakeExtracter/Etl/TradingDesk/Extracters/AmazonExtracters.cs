using System;
using System.Collections.Generic;
using System.Linq;
using Amazon;
using Amazon.Entities;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;
using Newtonsoft.Json;

namespace CakeExtracter.Etl.TradingDesk.Extracters
{
    public abstract class AmazonApiExtracter<T> : Extracter<T>
    {
        protected readonly AmazonUtility _amazonUtility;
        protected readonly DateRange dateRange;
        protected readonly string clientId;
        protected readonly string campaignFilter;
        protected readonly string campaignFilterOut;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonApiExtracter{T}"/> class.
        /// </summary>
        /// <param name="amazonUtility">The amazon utility.</param>
        /// <param name="date">The date.</param>
        /// <param name="clientId">The client identifier.</param>
        public AmazonApiExtracter(AmazonUtility amazonUtility, DateRange dateRange, string clientId, string campaignFilter = null, string campaignFilterOut = null)
        {
            this._amazonUtility = amazonUtility;
            this.dateRange = dateRange;
            this.clientId = clientId;
            this.campaignFilter = campaignFilter;
            this.campaignFilterOut = campaignFilterOut;
        }

        protected List<AmazonCampaign> LoadCampaignsFromAmazonAPI()
        {
            List<AmazonCampaign> campaigns = _amazonUtility.GetCampaigns(clientId);
            if (!String.IsNullOrEmpty(campaignFilter))
                campaigns = campaigns.Where(x => x.name.Contains(campaignFilter)).ToList();
            if (!String.IsNullOrEmpty(campaignFilterOut))
                campaigns = campaigns.Where(x => !x.name.Contains(campaignFilterOut)).ToList();

            return campaigns;
        }

    }

    #region Daily
    //The daily extracter will load data based on date range and sum up totals of each campaign
    public class AmazonDailySummaryExtracter : AmazonApiExtracter<AmazonDailySummary>
    {
        private long[] campaignIds;

        public AmazonDailySummaryExtracter(AmazonUtility amazonUtility, DateRange dateRange, string clientId, string campaignFilter = null, string campaignFilterOut = null)
            : base(amazonUtility, dateRange, clientId, campaignFilter: campaignFilter, campaignFilterOut: campaignFilterOut)
        { }
        protected override void Extract()
        {
            Logger.Info("Extracting DailySummaries from Amazon API for ({0}) from {1:d} to {2:d}",
                this.clientId, this.dateRange.FromDate, this.dateRange.ToDate);

            var campaigns = LoadCampaignsFromAmazonAPI();
            this.campaignIds = campaigns.Select(x => x.campaignId).ToArray();

            foreach (var date in dateRange.Dates)
            {
                var items = EnumerateRows(date);
                Add(items);
            }
            End();
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
            if (this.campaignIds != null)
                dailyItems = dailyItems.Where(x => this.campaignIds.Contains(x.campaignId)).ToList();

            var campDateGroups = dailyItems.GroupBy(x => new { x.date });
            foreach (var campDateGroup in campDateGroups)
            {
                var sum = new AmazonDailySummary
                {
                    date = campDateGroup.Key.date,
                    cost = campDateGroup.Sum(x => x.cost),
                    impressions = campDateGroup.Sum(x => x.impressions),
                    clicks = campDateGroup.Sum(x => x.clicks),
                    attributedConversions14d = campDateGroup.Sum(g => g.attributedConversions14d),
                    attributedSales14d = campDateGroup.Sum(g => g.attributedSales14d)
                };
                yield return sum;
            }
        }
    }
    #endregion

    #region Campaign/Strategy
    public class AmazonCampaignSummaryExtracter : AmazonApiExtracter<StrategySummary>
    {
        public AmazonCampaignSummaryExtracter(AmazonUtility amazonUtility, DateRange dateRange, string clientId, string campaignFilter = null, string campaignFilterOut = null)
            : base(amazonUtility, dateRange, clientId, campaignFilter: campaignFilter, campaignFilterOut: campaignFilterOut)
        { }

        protected override void Extract()
        {
            Logger.Info("Extracting StrategySummaries from Amazon API for ({0}) from {1:d} to {2:d}", 
                this.clientId, this.dateRange.FromDate, this.dateRange.ToDate);

            List<AmazonCampaign> campaigns = LoadCampaignsFromAmazonAPI();
            if (campaigns != null)
            {
                foreach (var date in dateRange.Dates)
                {
                    var items = EnumerateRows(date, campaigns);
                    Add(items);
                }
            }
            End();
        }

        public IEnumerable<StrategySummary> EnumerateRows(DateTime date, IEnumerable<AmazonCampaign> campaigns)
        {
            string reportDate = date.ToString("yyyyMMdd");
            var parms = _amazonUtility.CreateAmazonApiReportParams(reportDate);
            var submitReportResponse = _amazonUtility.SubmitReport(parms, "campaigns", this.clientId);
            if (!String.IsNullOrWhiteSpace(submitReportResponse.reportId))
            {
                List<AmazonDailySummary> dailyStats = new List<AmazonDailySummary>();
                //report could take awhile to be generated, therefore,we are looping until we get status SUCCESS
                while (true)
                {
                    var downloadInfo = _amazonUtility.RequestReport(submitReportResponse.reportId, this.clientId.ToString());

                    if (downloadInfo != null && !string.IsNullOrWhiteSpace(downloadInfo.location))
                    {
                        var dailyStatsJson = _amazonUtility.GetJsonStringFromDownloadFile(downloadInfo.location, this.clientId);
                        dailyStats = JsonConvert.DeserializeObject<List<AmazonDailySummary>>(dailyStatsJson);
                        break;
                    }
                }
                foreach (var sum in GroupAndEnumerate(dailyStats, campaigns, date))
                    yield return sum;
            }
        }

        private IEnumerable<StrategySummary> GroupAndEnumerate(List<AmazonDailySummary> dailyStats, IEnumerable<AmazonCampaign> campaigns, DateTime day)
        {
            foreach (var campaign in campaigns)
            {
                var group = dailyStats.Where(x => x.campaignId == campaign.campaignId);
                if (group.Any())
                {
                    var sum = new StrategySummary
                    {
                        Date = day,
                        StrategyEid = campaign.campaignId.ToString(),
                        StrategyName = campaign.name,
                        Impressions = group.Sum(g => g.impressions),
                        Clicks = group.Sum(g => g.clicks),
                        Cost = group.Sum(g => g.cost),
                        PostClickConv = group.Sum(g => g.attributedConversions14d),
                        PostClickRev = group.Sum(g => g.attributedSales14d)
                    };
                    yield return sum;
                }
            }
        }
    }
    #endregion

    #region AdSet (Keyword)
    public class AmazonAdSetExtracter : AmazonApiExtracter<AdSetSummary>
    {
        public AmazonAdSetExtracter(AmazonUtility amazonUtility, DateRange dateRange, string clientId, string campaignFilter = null, string campaignFilterOut = null)
            : base(amazonUtility, dateRange, clientId, campaignFilter: campaignFilter, campaignFilterOut: campaignFilterOut)
        { }

        protected override void Extract()
        {            
            Logger.Info("Extracting AdSetSummaries from Amazon API for ({0}) from {1:d} to {2:d}",
                this.clientId, this.dateRange.FromDate, this.dateRange.ToDate);

            var campaigns = LoadCampaignsFromAmazonAPI();
            var campIds = campaigns.Select(x => x.campaignId).ToArray();
            List<AmazonAdSet> adsets = LoadAdSetsfromAmazonAPI(campaignIds: campIds);

            if (adsets != null)
            {
                foreach (var date in dateRange.Dates)
                {
                    var items = EnumerateRows(date, adsets);
                    Add(items);
                }
            }
            End();
        }

        private List<AmazonAdSet> LoadAdSetsfromAmazonAPI(long[] campaignIds = null)
        {
            List<AmazonAdSet> adsets = new List<AmazonAdSet>();
            var keywords = _amazonUtility.GetKeywords(clientId);

            if (keywords == null) return null;

            foreach (var keyword in keywords)
            {
                if (campaignIds == null || campaignIds.Contains(keyword.CampaignId))
                {
                    adsets.Add(new AmazonAdSet
                    {
                        KeywordText = keyword.KeywordText,
                        CampaignId = keyword.CampaignId.ToString(),
                        KeywordId = keyword.KeywordId.ToString()
                    });
                }
            }
            return adsets;
        }
        //private List<string> LoadDistinctKeywordsfromAmazonAPI()
        //{
        //    var keywords = _amazonUtility.GetKeywords(clientId);
        //    if (keywords == null) return null;

        //    var uniqueKeywords = keywords.Select(x => x.KeywordText).Distinct().ToList();
        //    return uniqueKeywords;
        //}

        private IEnumerable<AdSetSummary> EnumerateRows(DateTime date, List<AmazonAdSet> adsets)
        {
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
            var keywordGroups = adsets.GroupBy(x => x.KeywordText); //TODO: do this outside of loop-by-day (above)

            foreach (var keywordGroup in keywordGroups)
            {
                var keywordIds = keywordGroup.Select(x => x.KeywordId).ToArray();
                var statsGroup = dailyStats.Where(x => keywordIds.Contains(x.KeywordId));
                if (statsGroup.Any())
                {
                    var sum = new AdSetSummary
                    {
                        Date = day,
                        AdSetName = keywordGroup.Key,
                        Impressions = statsGroup.Sum(g => g.Impressions),
                        Clicks = statsGroup.Sum(g => g.Clicks),
                        Cost = statsGroup.Sum(g => g.Cost),
                        PostClickConv = statsGroup.Sum(g => g.AttributedConversions30d),
                        PostClickRev = statsGroup.Sum(g => g.AttributedSales30d)
                    };
                    yield return sum;
                }
            }
        }

    }
    #endregion

    #region Ad (ProductAd)
    public class AmazonAdExtrater : AmazonApiExtracter<TDadSummary>
    {
        //TODO: Allow to use campaignFilter

        public AmazonAdExtrater(AmazonUtility amazonUtility, DateRange dateRange, string clientId, string campaignFilter = null, string campaignFilterOut = null)
            : base(amazonUtility, dateRange, clientId, campaignFilter: campaignFilter, campaignFilterOut: campaignFilterOut)
        { }

        protected override void Extract()
        {
            Logger.Info("Extracting TDadSummaries from Amazon API for ({0}) from {1:d} to {2:d}",
                this.clientId, this.dateRange.FromDate, this.dateRange.ToDate);

            // This didn't work. The stats (e.g. spend) were larger than what we got at the campaign/keyword levels.
            var campaigns = LoadCampaignsFromAmazonAPI();
            var campIds = campaigns.Select(x => x.campaignId).ToArray();
            List<TDad> ads = LoadAdsFromAmazonAPI(campaignIds: campIds);

            //List<TDad> ads = LoadAdsFromAmazonAPI();

            if (ads != null)
            {
                foreach (var date in dateRange.Dates)
                {
                    var items = EnumerateRows(date, ads);
                    Add(items);
                }
            }
            End();
        }

        private List<TDad> LoadAdsFromAmazonAPI(long[] campaignIds = null)
        {
            List<TDad> ads = new List<TDad>();            
            var productAds = _amazonUtility.GetProductAds(clientId);
            if (productAds == null) return null;
            if (campaignIds != null)
                productAds = productAds.Where(x => campaignIds.Contains(x.CampaignId)).ToList();

            foreach (var productAdGroup in productAds.GroupBy(x => x.AdId))
            {
                var ad = new TDad
                {
                    ExternalId = productAdGroup.Key,
                    Name = productAdGroup.First().Asin
                };
                if (productAdGroup.Count() > 1)
                {
                    Logger.Info("Multiple ads for {0}", productAdGroup.Key);
                    var names = productAdGroup.Select(x => x.Asin).ToArray();
                    ad.Name = String.Join(",", names);
                }
                ads.Add(ad);
            }
            return ads;
        }
        //private List<string> LoadDistinctAdNamesFromAmazonAPI()
        //{
        //    var productAds = _amazonUtility.GetProductAds(clientId);
        //    if (productAds == null) return null;

        //    var uniqueAdNames = productAds.Select(x => x.Asin).Distinct().ToList();
        //    return uniqueAdNames;
        //}

        private IEnumerable<TDadSummary> EnumerateRows(DateTime date, List<TDad> ads)
        {
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
            var adNameGroups = productAds.GroupBy(x => x.Name); //TODO: do this outside of loop-by-day (above)

            foreach (var adNameGroup in adNameGroups)
            {
                var adIds = adNameGroup.Select(x => x.ExternalId).ToArray();
                var statsGroup = productAdsDailyStats.Where(x => adIds.Contains(x.adId));
                if (statsGroup.Any())
                {
                    var sum = new TDadSummary
                    {
                        Date = day,
                        //TDadEid =
                        TDadName = adNameGroup.Key,
                        Impressions = statsGroup.Sum(g => g.impressions),
                        Clicks = statsGroup.Sum(g => g.clicks),
                        Cost = statsGroup.Sum(g => g.cost),
                        PostClickConv = statsGroup.Sum(g => g.attributedConversions14d),
                    };
                    yield return sum;
                }
            }
        }
    }
    #endregion

}
