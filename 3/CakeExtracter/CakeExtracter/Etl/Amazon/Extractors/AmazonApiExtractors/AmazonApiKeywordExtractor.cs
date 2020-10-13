using System;
using System.Collections.Generic;
using System.Linq;
using Amazon;
using Amazon.Entities;
using Amazon.Entities.Summaries;
using Amazon.Enums;
using CakeExtracter.Common;
using CakeExtracter.Etl.Amazon.Exceptions;
using CakeExtracter.Etl.TradingDesk.Extracters.AmazonExtractors;
using CakeExtracter.Helpers;
using CakeExtracter.Logging.TimeWatchers.Amazon;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.Amazon.Extractors.AmazonApiExtractors
{
    /// <summary>
    /// Keyword data extractor for amazon job.
    /// </summary>
    /// <seealso cref="BaseAmazonExtractor{T}" />
    public class AmazonApiKeywordExtractor : BaseAmazonExtractor<KeywordSummary>
    {
        private readonly AmazonCampaignMetadataExtractor campaignMetadataExtractor;

        /// <inheritdoc/>
        protected override string AmazonJobLevel => AmazonJobLevels.Keyword;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonApiKeywordExtractor"/> class.
        /// </summary>
        /// <param name="amazonUtility">The amazon utility.</param>
        /// <param name="dateRange">Extraction date range.</param>
        /// <param name="account">Account data.</param>
        /// <param name="campaignFilter">Campaign filter value.</param>
        /// <param name="campaignFilterOut">Campaign filter value out.</param>
        public AmazonApiKeywordExtractor(
            AmazonUtility amazonUtility,
            DateRange dateRange,
            ExtAccount account,
            string campaignFilter = null,
            string campaignFilterOut = null)
            : base(amazonUtility, dateRange, account, campaignFilter, campaignFilterOut)
        {
            campaignMetadataExtractor = new AmazonCampaignMetadataExtractor(amazonUtility);
        }

        public override void RemoveOldData(DateTime date)
        {
            Logger.Info(accountId, "The cleaning of KeywordSummaries for account ({0}) has begun - {1}.", accountId, date);
            AmazonTimeTracker.Instance.ExecuteWithTimeTracking(
                () =>
                {
                    SafeContextWrapper.TryMakeTransaction(
                        (ClientPortalProgContext db) =>
                        {
                            db.KeywordSummaryMetrics.Where(x => x.Date == date && x.Keyword.AccountId == accountId)
                                .DeleteFromQuery();
                            db.KeywordSummaries.Where(x => x.Date == date && x.Keyword.AccountId == accountId)
                                .DeleteFromQuery();
                        }, "DeleteFromQuery");
                },
                accountId,
                AmazonJobLevel,
                AmazonJobOperations.CleanExistingData);
            Logger.Info(accountId, "The cleaning of KeywordSummaries for account ({0}) is over - {1}.", accountId, date);
        }

        /// <summary>
        /// The derived class implements this method, which calls Add() for each item
        /// extracted and then calls End() when complete.
        /// </summary>
        protected override void Extract()
        {
            Logger.Info(
                accountId,
                "Extracting KeywordSummaries from Amazon API for ({0}) from {1:d} to {2:d}",
                clientId,
                dateRange.FromDate,
                dateRange.ToDate);
            try
            {
                var campaignsData = GetCampaignInfo();
                ExtractDataForDays(campaignsData);
            }
            catch (Exception e)
            {
                ProcessFailedStatsExtraction(e, dateRange.FromDate, dateRange.ToDate);
            }
            finally
            {
                End();
            }
        }

        /// <inheritdoc/>
        protected override IEnumerable<KeywordSummary> GetDataFromApi(DateTime date, List<AmazonCampaign> campaignsData)
        {
            var keywordSums = ExtractKeywordSummaries(date);
            var keywordItems = TransformSummaries(keywordSums, date, campaignsData);
            var targetSums = ExtractTargetSummaries(date);
            var targetItems = TransformSummaries(targetSums, date, campaignsData);
            targetItems = targetItems
                .GroupBy(p => new
                {
                    p.Date,
                    p.AdSetEid,
                    p.AdSetName,
                    p.KeywordEid,
                    p.KeywordName,
                    p.StrategyEid,
                    p.StrategyName,
                    p.StrategyTargetingType,
                    p.StrategyType,
                })
                .Select(g => new KeywordSummary
                {
                    Date = g.Key.Date,
                    AdSetEid = g.Key.AdSetEid,
                    AdSetName = g.Key.AdSetName,
                    KeywordEid = g.Key.KeywordEid,
                    KeywordName = g.Key.KeywordName,
                    StrategyEid = g.Key.StrategyEid,
                    StrategyName = g.Key.StrategyName,
                    StrategyTargetingType = g.Key.StrategyTargetingType,
                    StrategyType = g.Key.StrategyType,
                    Impressions = g.Sum(p => p.Impressions),
                    Clicks = g.Sum(p => p.Clicks),
                    Cost = g.Sum(p => p.Cost),
                    AllClicks = g.Sum(p => p.AllClicks),
                    PostClickConv = g.Sum(p => p.AllClicks),
                    PostClickRev = g.Sum(p => p.PostClickRev),
                    PostViewConv = g.Sum(p => p.PostViewConv),
                    PostViewRev = g.Sum(p => p.PostViewRev),
                });
            var items = keywordItems.Concat(targetItems);
            return items.ToList();
        }

        /// <inheritdoc/>
        protected override void ProcessFailedStatsExtraction(Exception e, DateTime fromDate, DateTime toDate)
        {
            Logger.Error(accountId, e);
            var exception = new FailedStatsLoadingException(fromDate, toDate, accountId, e, byKeyword: true);
            InvokeProcessFailedExtractionHandlers(exception);
        }

        private List<AmazonCampaign> GetCampaignInfo()
        {
            List<AmazonCampaign> campaignsData = null;
            AmazonTimeTracker.Instance.ExecuteWithTimeTracking(
                () =>
                {
                    campaignsData = campaignMetadataExtractor.LoadCampaignsMetadata(accountId, clientId).ToList();
                },
                accountId,
                AmazonJobLevel,
                AmazonJobOperations.LoadCampaignMetadata);
            return campaignsData;
        }

        private IEnumerable<AmazonKeywordDailySummary> ExtractKeywordSummaries(DateTime date)
        {
            var spSums = _amazonUtility.ReportKeywords(CampaignType.SponsoredProducts, date, clientId, true);
            var sbSums = _amazonUtility.ReportKeywords(CampaignType.SponsoredBrands, date, clientId, true);
            var sbvSums = _amazonUtility.ReportKeywords(CampaignType.SponsoredBrandsVideo, date, clientId, true);
            var sums = spSums.Concat(sbSums).Concat(sbvSums);
            var filteredSums = FilterByCampaigns(sums, x => x.CampaignName);
            return filteredSums.ToList();
        }

        private IEnumerable<AmazonTargetKeywordDailySummary> ExtractTargetSummaries(DateTime date)
        {
            var spSums = _amazonUtility.ReportTargetKeywords(CampaignType.SponsoredProducts, date, clientId, true);
            var sbSums = _amazonUtility.ReportTargetKeywords(CampaignType.SponsoredBrands, date, clientId, true);
            var sums = spSums.Concat(sbSums);
            var filteredSums = FilterByCampaigns(sums, x => x.CampaignName);
            return filteredSums.ToList();
        }

        private IEnumerable<KeywordSummary> TransformSummaries(
            IEnumerable<AmazonKeywordDailySummary> keywordStats,
            DateTime date,
            List<AmazonCampaign> campaignsData)
        {
            var notEmptyStats = keywordStats.Where(x => !x.AllZeros()).ToList();
            var summaries = notEmptyStats.Select(stat =>
                CreateSummary(stat, date, campaignsData.FirstOrDefault(c => c.CampaignId == stat.CampaignId)));
            return summaries.ToList();
        }

        private IEnumerable<KeywordSummary> TransformSummaries(
            IEnumerable<AmazonTargetKeywordDailySummary> targetStats,
            DateTime date,
            List<AmazonCampaign> campaignsData)
        {
            var notEmptyStats = targetStats.Where(x => !x.AllZeros()).ToList();
            var summaries = notEmptyStats.Select(stat =>
                CreateSummary(stat, date, campaignsData.FirstOrDefault(c => c.CampaignId == stat.CampaignId)));
            return summaries.ToList();
        }

        private KeywordSummary CreateSummary(
            AmazonKeywordDailySummary keywordStat,
            DateTime date,
            AmazonCampaign relatedCampaign)
        {
            var sum = CreateSummary(keywordStat as AmazonStatSummary, date, relatedCampaign);
            sum.KeywordEid = keywordStat.KeywordId;
            sum.KeywordName = keywordStat.KeywordText;
            sum.KeywordMediaType = keywordStat.MatchType;
            return sum;
        }

        private KeywordSummary CreateSummary(
            AmazonTargetKeywordDailySummary targetStat,
            DateTime date,
            AmazonCampaign relatedCampaign)
        {
            var sum = CreateSummary(targetStat as AmazonStatSummary, date, relatedCampaign);
            sum.KeywordEid = targetStat.TargetId;
            sum.KeywordName = targetStat.TargetingText;
            return sum;
        }

        private KeywordSummary CreateSummary(AmazonStatSummary stat, DateTime date, AmazonCampaign relatedCampaign)
        {
            var sum = new KeywordSummary
            {
                AdSetEid = stat.AdGroupId,
                AdSetName = stat.AdGroupName,
                StrategyEid = stat.CampaignId,
                StrategyName = stat.CampaignName,
                StrategyType = stat.CampaignType,
                StrategyTargetingType = relatedCampaign?.TargetingType,
            };
            SetCPProgStats(sum, stat, date);
            return sum;
        }
    }
}
