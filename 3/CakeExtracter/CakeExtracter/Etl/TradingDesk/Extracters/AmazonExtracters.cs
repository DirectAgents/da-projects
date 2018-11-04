using System;
using System.Collections.Generic;
using System.Linq;
using Amazon;
using Amazon.Entities;
using Amazon.Enums;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;
using StatSummary = Amazon.Entities.StatSummary;

namespace CakeExtracter.Etl.TradingDesk.Extracters
{
    public abstract class AmazonApiExtracter<T> : Extracter<T>
        where T : DatedStatsSummary
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

        protected static void SetCPProgStats(T stats, StatSummary amazonStat, DateTime date)
        {
            SetCPProgStats(stats, new[] { amazonStat }, date);
        }

        protected static void SetCPProgStats(T stats, IEnumerable<StatSummary> amazonStats, DateTime date)
        {
            stats.Date = date;
            if (amazonStats == null || !amazonStats.Any())
            {
                return; //note: not setting stats to 0 if !any
            }

            stats.Cost = amazonStats.Sum(x => x.cost);
            stats.Impressions = amazonStats.Sum(x => x.impressions);
            stats.Clicks = amazonStats.Sum(x => x.clicks);
            stats.PostClickConv = amazonStats.Sum(x => x.attributedConversions14d);
            var rev = stats as DatedStatsSummaryWithRev;
            if (rev != null)
            {
                rev.PostClickRev = amazonStats.Sum(x => x.attributedSales14d);
            }
            stats.InitialMetrics = GetMetrics(amazonStats, date);
        }

        private static List<SummaryMetric> GetMetrics(IEnumerable<StatSummary> amazonStats, DateTime date)
        {
            var metrics = new List<SummaryMetric>();
            AddMetric(metrics, AttributedMetricType.attributedConversions, AttributedMetricDaysInterval.Days1, date, amazonStats.Sum(x => x.attributedConversions1d));
            AddMetric(metrics, AttributedMetricType.attributedConversions, AttributedMetricDaysInterval.Days7, date, amazonStats.Sum(x => x.attributedConversions7d));
            AddMetric(metrics, AttributedMetricType.attributedConversions, AttributedMetricDaysInterval.Days14, date, amazonStats.Sum(x => x.attributedConversions14d));
            AddMetric(metrics, AttributedMetricType.attributedConversions, AttributedMetricDaysInterval.Days30, date, amazonStats.Sum(x => x.attributedConversions30d));
            AddMetric(metrics, AttributedMetricType.attributedConversionsSameSKU, AttributedMetricDaysInterval.Days1, date, amazonStats.Sum(x => x.attributedConversions1dSameSKU));
            AddMetric(metrics, AttributedMetricType.attributedConversionsSameSKU, AttributedMetricDaysInterval.Days7, date, amazonStats.Sum(x => x.attributedConversions7dSameSKU));
            AddMetric(metrics, AttributedMetricType.attributedConversionsSameSKU, AttributedMetricDaysInterval.Days14, date, amazonStats.Sum(x => x.attributedConversions14dSameSKU));
            AddMetric(metrics, AttributedMetricType.attributedConversionsSameSKU, AttributedMetricDaysInterval.Days30, date, amazonStats.Sum(x => x.attributedConversions30dSameSKU));
            AddMetric(metrics, AttributedMetricType.attributedSales, AttributedMetricDaysInterval.Days1, date, amazonStats.Sum(x => x.attributedSales1d));
            AddMetric(metrics, AttributedMetricType.attributedSales, AttributedMetricDaysInterval.Days7, date, amazonStats.Sum(x => x.attributedSales7d));
            AddMetric(metrics, AttributedMetricType.attributedSales, AttributedMetricDaysInterval.Days14, date, amazonStats.Sum(x => x.attributedSales14d));
            AddMetric(metrics, AttributedMetricType.attributedSales, AttributedMetricDaysInterval.Days30, date, amazonStats.Sum(x => x.attributedSales30d));
            AddMetric(metrics, AttributedMetricType.attributedSalesSameSKU, AttributedMetricDaysInterval.Days1, date, amazonStats.Sum(x => x.attributedSales1dSameSKU));
            AddMetric(metrics, AttributedMetricType.attributedSalesSameSKU, AttributedMetricDaysInterval.Days7, date, amazonStats.Sum(x => x.attributedSales7dSameSKU));
            AddMetric(metrics, AttributedMetricType.attributedSalesSameSKU, AttributedMetricDaysInterval.Days14, date, amazonStats.Sum(x => x.attributedSales14dSameSKU));
            AddMetric(metrics, AttributedMetricType.attributedSalesSameSKU, AttributedMetricDaysInterval.Days30, date, amazonStats.Sum(x => x.attributedSales30dSameSKU));
            AddMetric(metrics, AttributedMetricType.attributedUnitsOrdered, AttributedMetricDaysInterval.Days1, date, amazonStats.Sum(x => x.attributedUnitsOrdered1d));
            AddMetric(metrics, AttributedMetricType.attributedUnitsOrdered, AttributedMetricDaysInterval.Days7, date, amazonStats.Sum(x => x.attributedUnitsOrdered7d));
            AddMetric(metrics, AttributedMetricType.attributedUnitsOrdered, AttributedMetricDaysInterval.Days14, date, amazonStats.Sum(x => x.attributedUnitsOrdered14d));
            AddMetric(metrics, AttributedMetricType.attributedUnitsOrdered, AttributedMetricDaysInterval.Days30, date, amazonStats.Sum(x => x.attributedUnitsOrdered30d));
            return metrics;
        }

        private static void AddMetric(List<SummaryMetric> metrics, AttributedMetricType type, AttributedMetricDaysInterval daysInterval, DateTime date, decimal metricValue)
        {
            if (metricValue == 0.0M)
            {
                return;
            }

            var metric = new SummaryMetric()
            {
                Date = date,
                MetricType = new MetricType
                {
                    Name = type.ToString(),
                    DaysInterval = (int)daysInterval
                },
                Value = metricValue
            };
            metrics.Add(metric);
        }
    }

    #region Daily
    //The daily extracter will load data based on date range and sum up the total of all campaigns
    public class AmazonDailySummaryExtracter : AmazonApiExtracter<DailySummary>
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
            var spSums = _amazonUtility.ReportCampaigns(CampaignType.SponsoredProducts, date, clientId, true);
            var sbSums = _amazonUtility.ReportCampaigns(CampaignType.SponsoredBrands, date, clientId, true);
            var sums = spSums.Concat(sbSums);
            sums = FilterByCampaigns(sums, x => x.campaignName);
            return sums.ToList();
        }

        private DailySummary TransformSummaries(IEnumerable<AmazonDailySummary> sums, DateTime date)
        {
            var dailySum = new DailySummary();
            SetCPProgStats(dailySum, sums, date);
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

            var campaigns = LoadCampaignsFromAmazonApi();
            foreach (var date in dateRange.Dates)
            {
                var sums = ExtractSummaries(date);
                var items = TransformSummaries(sums, campaigns, date);
                Add(items);
            }
            End();
        }

        //TODO? Request the SP and HSA reports in parallel... ?Okay for two threads to call Add at the same time?
        //TODO? Do multiple dates in parallel

        private IEnumerable<AmazonCampaign> LoadCampaignsFromAmazonApi()
        {
            var spCampaigns = _amazonUtility.GetCampaigns(CampaignType.SponsoredProducts, clientId);
            var sbCampaigns = _amazonUtility.GetCampaigns(CampaignType.SponsoredBrands, clientId);
            var campaigns = spCampaigns.Concat(sbCampaigns);
            var filteredCampaigns = FilterByCampaigns(campaigns, x => x.name);
            return filteredCampaigns.ToList();
        }

        public IEnumerable<AmazonDailySummary> ExtractSummaries(DateTime date)
        {
            var spSums = _amazonUtility.ReportCampaigns(CampaignType.SponsoredProducts, date, clientId, false);
            var sbSums = _amazonUtility.ReportCampaigns(CampaignType.SponsoredBrands, date, clientId, false);
            var sums = spSums.Concat(sbSums);
            return sums.ToList();
        }

        private IEnumerable<StrategySummary> TransformSummaries(IEnumerable<AmazonDailySummary> dailyStats, IEnumerable<AmazonCampaign> campaigns, DateTime date)
        {
            var campaignIds = campaigns.Select(x => x.campaignId);
            dailyStats = dailyStats.Where(x => campaignIds.Contains(x.campaignId) && !x.AllZeros());
            var groupedStats = dailyStats.GroupBy(x => x.campaignId);
            foreach (var stat in groupedStats)
            {
                var campaign = campaigns.First(x => x.campaignId == stat.Key);
                var sum = new StrategySummary
                {
                    StrategyEid = campaign.campaignId,
                    StrategyName = campaign.name,
                    StrategyType = campaign.targetingType
                };
                SetCPProgStats(sum, stat, date); // most likely there's just one dailyStat in the group, but this covers everything...
                yield return sum;
            }
        }
    }

    #endregion

    #region AdSet (Ad group)
    public class AmazonAdSetExtracter : AmazonApiExtracter<AdSetSummary>
    {
        public AmazonAdSetExtracter(AmazonUtility amazonUtility, DateRange dateRange, ExtAccount account, string campaignFilter = null, string campaignFilterOut = null)
            : base(amazonUtility, dateRange, account, campaignFilter: campaignFilter, campaignFilterOut: campaignFilterOut)
        { }

        protected override void Extract()
        {
            Logger.Info(accountId, "Extracting AdSetSummaries from Amazon API for ({0}) from {1:d} to {2:d}",
                this.clientId, this.dateRange.FromDate, this.dateRange.ToDate);

            foreach (var date in dateRange.Dates)
            {
                var sums = ExtractSummaries(date);
                var items = TransformSummaries(sums, date);
                Add(items);
            }
            End();
        }

        public IEnumerable<AmazonAdGroupSummary> ExtractSummaries(DateTime date)
        {
            var spSums = _amazonUtility.ReportAdGroups(CampaignType.SponsoredProducts, date, clientId, true);
            var sbSums = _amazonUtility.ReportAdGroups(CampaignType.SponsoredBrands, date, clientId, true);
            var sums = spSums.Concat(sbSums);
            var filteredSums = FilterByCampaigns(sums, x => x.campaignName);
            return filteredSums.ToList();
        }

        private IEnumerable<AdSetSummary> TransformSummaries(IEnumerable<AmazonAdGroupSummary> adGroupStats, DateTime date)
        {
            adGroupStats = adGroupStats.Where(x => !x.AllZeros()).ToList();
            foreach (var adGroupStat in adGroupStats)
            {
                var sum = new AdSetSummary
                {
                    AdSetEid = adGroupStat.adGroupId,
                    AdSetName = adGroupStat.adGroupName,
                    StrategyEid = adGroupStat.campaignId,
                    StrategyName = adGroupStat.campaignName
                };
                SetCPProgStats(sum, adGroupStat, date);
                yield return sum;
            }
        }
    }
    #endregion

    #region Ad (ProductAd)
    public class AmazonAdExtrater : AmazonApiExtracter<TDadSummary>
    {
        //NOTE: We can only get ad stats for SponsoredProduct campaigns, for these reasons:
        // - the get-ProductAds call only returns SP ads
        // - for Sponsored Brands reports, recordType call only be campaigns, adGroups or keywords
        // - a productAdId metric is not available anyway

        public AmazonAdExtrater(AmazonUtility amazonUtility, DateRange dateRange, ExtAccount account, string campaignFilter = null, string campaignFilterOut = null)
            : base(amazonUtility, dateRange, account, campaignFilter: campaignFilter, campaignFilterOut: campaignFilterOut)
        { }

        protected override void Extract()
        {
            Logger.Info(accountId, "Extracting TDadSummaries from Amazon API for ({0}) from {1:d} to {2:d}",
                this.clientId, this.dateRange.FromDate, this.dateRange.ToDate);

            foreach (var date in dateRange.Dates)
            {
                var sums = ExtractSummaries(date);
                var items = TransformSummaries(sums, date);
                Add(items);
            }
            End();
        }

        private IEnumerable<AmazonAdDailySummary> ExtractSummaries(DateTime date)
        {
            var sums = _amazonUtility.ReportProductAds(date, clientId, true);
            var filteredSums = FilterByCampaigns(sums, x => x.campaignName);
            return filteredSums.ToList();
        }

        private IEnumerable<TDadSummary> TransformSummaries(IEnumerable<AmazonAdDailySummary> adStats, DateTime date)
        {
            adStats = adStats.Where(x => !x.AllZeros()).ToList();
            foreach (var adSum in adStats)
            {
                var sum = new TDadSummary
                {
                    TDadEid = adSum.adId,
                    TDadName = adSum.adGroupName,
                    AdSetEid = adSum.adGroupId,
                    AdSetName = adSum.adGroupName
                };
                var externalIds = new List<TDadExternalId>();
                AddAdExternalId(externalIds, ProductAdIdType.ASIN, adSum.asin);
                //AddAdExternalId(externalIds, ProductAdIdType.SKU, adSum.sku);
                sum.ExternalIds = externalIds;
                SetCPProgStats(sum, adSum, date);
                yield return sum;
            }
        }

        private void AddAdExternalId(List<TDadExternalId> ids, ProductAdIdType type, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            var id = new TDadExternalId
            {
                Type = new EntityType {Name = type.ToString()},
                ExternalId = value
            };
            ids.Add(id);
        }
    }
    #endregion

    #region Keyword
    public class AmazonKeywordExtracter : AmazonApiExtracter<KeywordSummary>
    {
        public AmazonKeywordExtracter(AmazonUtility amazonUtility, DateRange dateRange, ExtAccount account, string campaignFilter = null, string campaignFilterOut = null)
            : base(amazonUtility, dateRange, account, campaignFilter: campaignFilter, campaignFilterOut: campaignFilterOut)
        { }

        protected override void Extract()
        {
            Logger.Info(accountId, "Extracting KeywordSummaries from Amazon API for ({0}) from {1:d} to {2:d}",
                this.clientId, this.dateRange.FromDate, this.dateRange.ToDate);
            
            foreach (var date in dateRange.Dates)
            {
                var sums = ExtractSummaries(date);
                var items = TransformSummaries(sums, date);
                Add(items);
            }
            End();
        }

        public IEnumerable<AmazonKeywordDailySummary> ExtractSummaries(DateTime date)
        {
            var spSums = _amazonUtility.ReportKeywords(CampaignType.SponsoredProducts, date, clientId, true);
            var sbSums = _amazonUtility.ReportKeywords(CampaignType.SponsoredBrands, date, clientId, true);
            var sums = spSums.Concat(sbSums);
            var filteredSums = FilterByCampaigns(sums, x => x.campaignName);
            return filteredSums.ToList();
        }
        
        private IEnumerable<KeywordSummary> TransformSummaries(IEnumerable<AmazonKeywordDailySummary> keywordStats, DateTime date)
        {
            keywordStats = keywordStats.Where(x => !x.AllZeros()).ToList();
            foreach (var keywordStat in keywordStats)
            {
                var sum = new KeywordSummary
                {
                    KeywordEid = keywordStat.keywordId,
                    KeywordName = keywordStat.keywordText,
                    AdSetEid = keywordStat.adGroupId,
                    AdSetName = keywordStat.adGroupName,
                    StrategyEid = keywordStat.campaignId,
                    StrategyName = keywordStat.campaignName,
                };
                SetCPProgStats(sum, keywordStat, date);
                yield return sum;
            }
        }
    }
    #endregion

    #region Search term
    public class AmazonSearchTermExtracter : AmazonApiExtracter<SearchTermSummary>
    {
        public AmazonSearchTermExtracter(AmazonUtility amazonUtility, DateRange dateRange, ExtAccount account, string campaignFilter = null, string campaignFilterOut = null)
            : base(amazonUtility, dateRange, account, campaignFilter: campaignFilter, campaignFilterOut: campaignFilterOut)
        { }

        protected override void Extract()
        {
            Logger.Info(accountId, "Extracting SearchTermSummaries from Amazon API for ({0}) from {1:d} to {2:d}",
                this.clientId, this.dateRange.FromDate, this.dateRange.ToDate);

            foreach (var date in dateRange.Dates)
            {
                var sums = ExtractSummaries(date);
                var items = TransformSummaries(sums, date);
                Add(items);
            }
            End();
        }

        public IEnumerable<AmazonSearchTermDailySummary> ExtractSummaries(DateTime date)
        {
            var sums = _amazonUtility.ReportSearchTerms(date, clientId, true);
            var filteredSums = FilterByCampaigns(sums, x => x.campaignName);
            return filteredSums.ToList();
        }

        private IEnumerable<SearchTermSummary> TransformSummaries(IEnumerable<AmazonSearchTermDailySummary> searchTermStats, DateTime date)
        {
            searchTermStats = searchTermStats.Where(x => !x.AllZeros()).ToList();
            foreach (var stat in searchTermStats)
            {
                var sum = new SearchTermSummary
                {
                    Query = stat.query,
                    KeywordEid = stat.keywordId,
                    KeywordName = stat.keywordText,
                    AdSetEid = stat.adGroupId,
                    AdSetName = stat.adGroupName,
                    StrategyEid = stat.campaignId,
                    StrategyName = stat.campaignName,
                };
                SetCPProgStats(sum, stat, date);
                yield return sum;
            }
        }
    }
    #endregion
}
