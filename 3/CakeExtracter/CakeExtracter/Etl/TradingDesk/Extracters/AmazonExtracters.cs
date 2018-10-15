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
        protected readonly int accountId;   // in our db
        protected readonly string clientId; // external id
        protected readonly string campaignFilter;
        protected readonly string campaignFilterOut;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonApiExtracter{T}"/> class.
        /// </summary>
        /// <param name="amazonUtility">The amazon utility.</param>
        /// <param name="date">The date.</param>
        /// <param name="clientId">The client identifier.</param>
        public AmazonApiExtracter(AmazonUtility amazonUtility, DateRange dateRange, ExtAccount account, string campaignFilter = null, string campaignFilterOut = null)
        {
            this._amazonUtility = amazonUtility;
            this.dateRange = dateRange;
            this.accountId = account.Id;
            this.clientId = account.ExternalId;
            this.campaignFilter = campaignFilter;
            this.campaignFilterOut = campaignFilterOut;
        }

        protected IEnumerable<TStat> FilterByCampaigns<TStat>(IEnumerable<TStat> reportEntities, Func<TStat, string> getFilterProp)
        {
            if (!String.IsNullOrEmpty(campaignFilter))
            {
                reportEntities = reportEntities.Where(x => getFilterProp(x).Contains(campaignFilter));
            }
            if (!String.IsNullOrEmpty(campaignFilterOut))
            {
                reportEntities = reportEntities.Where(x => !getFilterProp(x).Contains(campaignFilterOut));
            }
            return reportEntities.ToList();
        }

        protected IEnumerable<TEntity> GetEntities<TEntity>(EntitesType entitesType, CampaignType campaignType)
        {
            var entities = _amazonUtility.GetEntities<TEntity>(entitesType, campaignType, this.clientId);
            return entities.ToList();
        }

        //By using snapshot API calls
        //This method returns an internal Amazon server error.

        //protected IEnumerable<TEntity> GetEntities<TEntity>(EntitesType entitesType, CampaignType campaignType)
        //{
        //    var parms = _amazonUtility.CreateAmazonApiSnapshotParams(campaignType);
        //    var submitReportResponse = _amazonUtility.SubmitSnapshot(parms, campaignType, entitesType, this.clientId);
        //    if (submitReportResponse != null)
        //    {
        //        var json = _amazonUtility.WaitForSnapshotAndDownload(submitReportResponse.snapshotId, this.clientId);
        //        if (json != null)
        //        {
        //            var stats = JsonConvert.DeserializeObject<List<TEntity>>(json);
        //            return stats;
        //        }
        //    }
        //    return new List<TEntity>();
        //}

        protected IEnumerable<TStat> GetReportInfo<TStat>(EntitesType reportType, CampaignType campaignType, DateTime date, bool includeCampaignName = false)
        {
            var parms = _amazonUtility.CreateAmazonApiReportParams(reportType, date, includeCampaignName);
            var submitReportResponse = _amazonUtility.SubmitReport(parms, campaignType, reportType, this.clientId);
            if (submitReportResponse != null)
            {
                var json = _amazonUtility.WaitForReportAndDownload(submitReportResponse.reportId, this.clientId);
                if (json != null)
                {
                    var stats = JsonConvert.DeserializeObject<List<TStat>>(json);
                    return stats;
                }
            }
            return new List<TStat>();
        }

        protected static void SetCPProgStats(StatsSummary cpProgStats, IEnumerable<StatSummary> amazonStats)
        {
            var any = amazonStats != null && amazonStats.Any();
            if (!any)
            {
                return;   //note: not setting stats to 0 if !any
            }
            cpProgStats.Cost = amazonStats.Sum(x => x.cost);
            cpProgStats.Impressions = amazonStats.Sum(x => x.impressions);
            cpProgStats.Clicks = amazonStats.Sum(x => x.clicks);
            cpProgStats.PostClickConv = amazonStats.Sum(x => x.attributedConversions14d);
            var rev = cpProgStats as DatedStatsSummaryWithRev;
            if (rev != null)
            {
                rev.PostClickRev = amazonStats.Sum(x => x.attributedSales14d);
            }
        }
    }

    #region Daily
    //The daily extracter will load data based on date range and sum up the total of all campaigns
    public class AmazonDailySummaryExtracter : AmazonApiExtracter<AmazonDailySummary>
    {
        public AmazonDailySummaryExtracter(AmazonUtility amazonUtility, DateRange dateRange, ExtAccount account, string campaignFilter = null, string campaignFilterOut = null)
            : base(amazonUtility, dateRange, account, campaignFilter: campaignFilter, campaignFilterOut: campaignFilterOut)
        { }

        protected override void Extract()
        {
            Logger.Info(accountId, "Extracting DailySummaries from Amazon API for ({0}) from {1:d} to {2:d}",
                this.clientId, this.dateRange.FromDate, this.dateRange.ToDate);

            foreach (var date in dateRange.Dates)
            {
                var sums = ExtractSummaries(date);
                var dailySum = TransformSummaries(sums, date);
                Add(dailySum);
            }
            End();
        }

        private IEnumerable<AmazonDailySummary> ExtractSummaries(DateTime date)
        {
            var spSums = GetReportInfo<AmazonDailySummary>(EntitesType.Campaigns, CampaignType.SponsoredProducts, date, true);
            var sbSums = GetReportInfo<AmazonDailySummary>(EntitesType.Campaigns, CampaignType.SponsoredBrands, date, true);
            var sums = spSums.Concat(sbSums);
            sums = FilterByCampaigns(sums, x => x.campaignName);
            return sums;
        }

        private AmazonDailySummary TransformSummaries(IEnumerable<AmazonDailySummary> sums, DateTime date)
        {
            var dailySum = new AmazonDailySummary
            {
                date = date,
            };
            dailySum.SetStatTotals(sums);
            return dailySum;
        }
    }
    #endregion

    #region Campaign/Strategy
    public class AmazonCampaignSummaryExtracter : AmazonApiExtracter<StrategySummary>
    {
        public AmazonCampaignSummaryExtracter(AmazonUtility amazonUtility, DateRange dateRange, ExtAccount account, string campaignFilter = null, string campaignFilterOut = null)
            : base(amazonUtility, dateRange, account, campaignFilter: campaignFilter, campaignFilterOut: campaignFilterOut)
        { }

        protected override void Extract()
        {
            Logger.Info(accountId, "Extracting StrategySummaries from Amazon API for ({0}) from {1:d} to {2:d}",
                this.clientId, this.dateRange.FromDate, this.dateRange.ToDate);

            foreach (var date in dateRange.Dates)
            {
                var sums = ExtractSummaries(date);
                var items = TransformSummaries(sums, date);
                Add(items);
            }
            End();
        }

        //TODO? Request the SP and HSA reports in parallel... ?Okay for two threads to call Add at the same time?
        //TODO? Do multiple dates in parallel

        public IEnumerable<AmazonDailySummary> ExtractSummaries(DateTime date)
        {
            var spSums = GetReportInfo<AmazonDailySummary>(EntitesType.Campaigns, CampaignType.SponsoredProducts, date, true);
            var sbSums = GetReportInfo<AmazonDailySummary>(EntitesType.Campaigns, CampaignType.SponsoredBrands, date, true);
            var sums = spSums.Concat(sbSums);
            sums = FilterByCampaigns(sums, x => x.campaignName);
            return sums;
        }

        private IEnumerable<StrategySummary> TransformSummaries(IEnumerable<AmazonDailySummary> dailyStats, DateTime date)
        {
            dailyStats = dailyStats.Where(x => !x.AllZeros()).ToList();
            var groupedStats = dailyStats.GroupBy(x => x.campaignId);
            foreach (var stat in groupedStats)
            {
                var sum = new StrategySummary
                {
                    Date = date,
                    StrategyEid = stat.Key.ToString(),
                    StrategyName = stat.FirstOrDefault()?.campaignName
                };
                SetCPProgStats(sum, stat); // most likely there's just one dailyStat in the group, but this covers everything...
                yield return sum;
            }
        }
    }

    #endregion

    #region AdSet (Keyword)
    public class AmazonAdSetExtracter : AmazonApiExtracter<AdSetSummary>
    {
        public AmazonAdSetExtracter(AmazonUtility amazonUtility, DateRange dateRange, ExtAccount account, string campaignFilter = null, string campaignFilterOut = null)
            : base(amazonUtility, dateRange, account, campaignFilter: campaignFilter, campaignFilterOut: campaignFilterOut)
        { }

        //Note: The API only returns keywords for sponsoredProduct campaigns, and they are at the adgroup level.  So presumably two Keyword objects
        // could have the same text but be for two different adgroups in the same campaign.

        //TODO: Instead of keyword stats, get adgroup level stats here.  If needed, establish a new stats level in the db: Keyword, KeywordSummary
        // (where a Keyword is actually a set of keywords)

        protected override void Extract()
        {
            Logger.Info(accountId, "Extracting AdSetSummaries from Amazon API for ({0}) from {1:d} to {2:d}",
                this.clientId, this.dateRange.FromDate, this.dateRange.ToDate);

            var campaigns = LoadCampaignsFromAmazonApi();
            var adSets = LoadAdSetsFromAmazonApi(campaigns);
            var keywordGroups = adSets.GroupBy(x => x.KeywordText);

            foreach (var date in dateRange.Dates)
            {
                var sums = ExtractSummaries(date);
                var items = TransformSummaries(sums, date, keywordGroups);
                Add(items);
            }
            End();
        }

        private IEnumerable<AmazonCampaign> LoadCampaignsFromAmazonApi()
        {
            var campaigns = GetEntities<AmazonCampaign>(EntitesType.Campaigns, CampaignType.SponsoredProducts);
            campaigns = FilterByCampaigns(campaigns, x => x.name);
            return campaigns;
        }

        private IEnumerable<AmazonAdSet> LoadAdSetsFromAmazonApi(IEnumerable<AmazonCampaign> campaigns)
        {
            var keywords = GetEntities<AmazonKeyword>(EntitesType.Keywords, CampaignType.SponsoredProducts); // only for sponsoredProduct
            var campaignIds = campaigns.Select(x => x.campaignId).ToArray();
            var filteredKeywords = keywords.Where(x => campaignIds.Contains(x.CampaignId));
            var adSets = filteredKeywords.Select(x =>
                new AmazonAdSet
                {
                    KeywordText = x.KeywordText,
                    CampaignId = x.CampaignId.ToString(),
                    KeywordId = x.KeywordId.ToString()
                });
            return adSets.ToList();
        }

        //private List<string> LoadDistinctKeywordsfromAmazonAPI()
        //{
        //    var keywords = _amazonUtility.GetKeywords(clientId);
        //    if (keywords == null) return null;

        //    var uniqueKeywords = keywords.Select(x => x.KeywordText).Distinct().ToList();
        //    return uniqueKeywords;
        //}

        public IEnumerable<AmazonKeywordDailySummary> ExtractSummaries(DateTime date)
        {
            var sums = GetReportInfo<AmazonKeywordDailySummary>(EntitesType.Keywords, CampaignType.SponsoredProducts, date);
            return sums;
        }

        private IEnumerable<AdSetSummary> TransformSummaries(IEnumerable<AmazonKeywordDailySummary> dailyStats, DateTime date,
            IEnumerable<IGrouping<string, AmazonAdSet>> keywordGroups)
        {
            foreach (var keywordGroup in keywordGroups)
            {
                var keywordIds = keywordGroup.Select(x => x.KeywordId).ToArray();
                var statsGroup = dailyStats.Where(x => keywordIds.Contains(x.KeywordId) && !x.AllZeros());
                if (statsGroup.Any())
                {
                    var sum = new AdSetSummary
                    {
                        Date = date,
                        AdSetName = keywordGroup.Key
                    };
                    SetCPProgStats(sum, statsGroup);
                    yield return sum;
                }
            }
        }
    }
    #endregion

    #region Ad (ProductAd)
    public class AmazonAdExtrater : AmazonApiExtracter<TDadSummary>
    {
        //NOTE: We can only get ad stats for SponsoredProduct campaigns, for these reasons:
        // - the get-ProductAds call only returns SP ads
        // - for HSA reports, recordType call only be campaigns, adGroups or keywords
        // - a productAdId metric is not available anyway
        // (as of v.20180314)

        public AmazonAdExtrater(AmazonUtility amazonUtility, DateRange dateRange, ExtAccount account, string campaignFilter = null, string campaignFilterOut = null)
            : base(amazonUtility, dateRange, account, campaignFilter: campaignFilter, campaignFilterOut: campaignFilterOut)
        { }

        protected override void Extract()
        {
            Logger.Info(accountId, "Extracting TDadSummaries from Amazon API for ({0}) from {1:d} to {2:d}",
                this.clientId, this.dateRange.FromDate, this.dateRange.ToDate);

            // This didn't work. The stats (e.g. spend) were larger than what we got at the campaign/keyword levels.
            var campaigns = LoadCampaignsFromAmazonApi();
            var productAds = LoadAdsFromAmazonApi(campaigns);
            var adNameGroups = productAds.GroupBy(x => x.Name);

            foreach (var date in dateRange.Dates)
            {
                var sums = ExtractSummaries(date);
                var items = TransformSummaries(sums, date, adNameGroups);
                Add(items);
            }
            End();
        }

        private IEnumerable<AmazonCampaign> LoadCampaignsFromAmazonApi()
        {
            var campaigns = GetEntities<AmazonCampaign>(EntitesType.Campaigns, CampaignType.SponsoredProducts);
            campaigns = FilterByCampaigns(campaigns, x => x.name);
            return campaigns;
        }

        //NOTE: This only retrieves SponsoredProduct ads
        private IEnumerable<TDad> LoadAdsFromAmazonApi(IEnumerable<AmazonCampaign> campaigns)
        {
            var productAds = GetEntities<AmazonProductAd>(EntitesType.ProductAds, CampaignType.SponsoredProducts); // only for sponsoredProduct
            if (productAds == null)
            {
                return null;
            }
            var campaignIds = campaigns.Select(x => x.campaignId).ToArray();
            productAds = productAds.Where(x => campaignIds.Contains(x.CampaignId));
            var ads = productAds.GroupBy(x => x.AdId).Select(MapProductAdGroup);
            return ads.ToList();
        }

        private TDad MapProductAdGroup(IGrouping<string, AmazonProductAd> productAdGroup)
        {
            var ad = new TDad
            {
                ExternalId = productAdGroup.Key,
                Name = productAdGroup.First().Asin
            };
            if (productAdGroup.Count() <= 1)
            {
                return ad;
            }
            Logger.Info("Multiple ads for {0}", productAdGroup.Key);
            var names = productAdGroup.Select(x => x.Asin).ToArray();
            ad.Name = String.Join(",", names);
            return ad;
        }

        //private List<string> LoadDistinctAdNamesFromAmazonAPI()
        //{
        //    var productAds = _amazonUtility.GetProductAds(clientId);
        //    if (productAds == null) return null;

        //    var uniqueAdNames = productAds.Select(x => x.Asin).Distinct().ToList();
        //    return uniqueAdNames;
        //}

        private IEnumerable<AmazonAdDailySummary> ExtractSummaries(DateTime date)
        {
            var sums = GetReportInfo<AmazonAdDailySummary>(EntitesType.ProductAds, CampaignType.SponsoredProducts, date);
            return sums;
        }

        private IEnumerable<TDadSummary> TransformSummaries(IEnumerable<AmazonAdDailySummary> productAdsDailyStats, DateTime date, 
            IEnumerable<IGrouping<string, TDad>> adNameGroups)
        {
            foreach (var adNameGroup in adNameGroups)
            {
                var adIds = adNameGroup.Select(x => x.ExternalId).ToArray();
                var statsGroup = productAdsDailyStats.Where(x => adIds.Contains(x.adId) && !x.AllZeros());
                if (statsGroup.Any())
                {
                    var sum = new TDadSummary
                    {
                        Date = date,
                        TDadName = adNameGroup.Key,
                    };
                    SetCPProgStats(sum, statsGroup);
                    yield return sum;
                }
            }
        }
    }
    #endregion

}
