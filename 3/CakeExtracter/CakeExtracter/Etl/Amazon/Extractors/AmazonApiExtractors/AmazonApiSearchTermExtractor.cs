using System;
using System.Collections.Generic;
using System.Linq;
using Amazon;
using Amazon.Entities;
using Amazon.Entities.Summaries;
using Amazon.Enums;
using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement;
using CakeExtracter.Etl.Amazon.Exceptions;
using CakeExtracter.Etl.TradingDesk.Extracters.AmazonExtractors;
using CakeExtracter.Helpers;
using CakeExtracter.Logging.TimeWatchers.Amazon;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.Amazon.Extractors.AmazonApiExtractors
{
    /// <summary>
    /// API extractor for the Search term level.
    /// </summary>
    internal class AmazonApiSearchTermExtractor : BaseAmazonExtractor<SearchTermSummary>
    {
        private readonly AmazonCampaignMetadataExtractor campaignMetadataExtractor;

        /// <inheritdoc cref="BaseAmazonExtractor{SearchTermSummary}"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonApiSearchTermExtractor" /> class.
        /// </summary>
        /// <param name="amazonUtility">The amazon utility.</param>
        /// <param name="dateRange">Extraction date range.</param>
        /// <param name="account">Account data.</param>
        /// <param name="campaignFilter">Campaign filter value.</param>
        /// <param name="campaignFilterOut">Campaign filter value out.</param>
        public AmazonApiSearchTermExtractor(
            AmazonUtility amazonUtility,
            DateRange dateRange,
            ExtAccount account,
            string campaignFilter = null,
            string campaignFilterOut = null)
            : base(amazonUtility, dateRange, account, campaignFilter, campaignFilterOut)
        {
            campaignMetadataExtractor = new AmazonCampaignMetadataExtractor(amazonUtility);
        }

        /// <summary>
        /// Removing old data before insert fresh data.
        /// </summary>
        /// <param name="date">Date for removing.</param>
        public virtual void RemoveOldData(DateTime date)
        {
            Logger.Info(accountId, "The cleaning of SearchTermSummaries for account ({0}) has begun - {1}.", accountId, date);
            AmazonTimeTracker.Instance.ExecuteWithTimeTracking(
                () =>
                {
                    SafeContextWrapper.TryMakeTransaction(
                        (ClientPortalProgContext db) =>
                        {
                            db.SearchTermSummaryMetrics.Where(x => x.Date == date && x.SearchTerm.AccountId == accountId)
                                .DeleteFromQuery();
                            db.SearchTermSummaries.Where(x => x.Date == date && x.SearchTerm.AccountId == accountId)
                                .DeleteFromQuery();
                        }, "DeleteFromQuery");
                },
                accountId,
                AmazonJobLevels.SearchTerm,
                AmazonJobOperations.CleanExistingData);
            Logger.Info(accountId, "The cleaning of SearchTermSummaries for account ({0}) is over - {1}.", accountId, date);
        }

        /// <inheritdoc/>
        /// <summary>
        /// Extracting data of the Search term level from API.
        /// </summary>
        protected override void Extract()
        {
            Logger.Info(
                accountId,
                "Extracting SearchTermSummaries from Amazon API for ({0}) from {1:d} to {2:d}",
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

        private void ExtractDataForDays(List<AmazonCampaign> campaignsData)
        {
            foreach (var date in dateRange.Dates)
            {
                try
                {
                    ExtractDaily(date, campaignsData);
                }
                catch (Exception e)
                {
                    ProcessFailedStatsExtraction(e, date, date);
                }
            }
        }

        private void ExtractDaily(DateTime date, List<AmazonCampaign> campaignsData)
        {
            CommandExecutionContext.Current?.SetJobExecutionStateInHistory($"SearchTerm Level- {date.ToString()}", accountId);
            IEnumerable<SearchTermSummary> items = null;
            AmazonTimeTracker.Instance.ExecuteWithTimeTracking(
                () =>
                {
                    items = GetSearchTermSummariesFromApi(date, campaignsData);
                },
                accountId,
                AmazonJobLevels.SearchTerm,
                AmazonJobOperations.ReportExtracting);
            RemoveOldData(date);
            Add(items);
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
                AmazonJobLevels.SearchTerm,
                AmazonJobOperations.LoadCampaignMetadata);
            return campaignsData;
        }

        private void ProcessFailedStatsExtraction(Exception e, DateTime fromDate, DateTime toDate)
        {
            Logger.Error(accountId, e);
            var exception = new FailedStatsLoadingException(fromDate, toDate, accountId, e, bySearchTerm: true);
            InvokeProcessFailedExtractionHandlers(exception);
        }

        private IEnumerable<SearchTermSummary> GetSearchTermSummariesFromApi(DateTime date, List<AmazonCampaign> campaignsData)
        {
            var searchTermSums = ExtractSearchTermSummaries(date);
            var searchTermItems = TransformSummaries(searchTermSums, date, campaignsData);
            searchTermItems = searchTermItems
                .GroupBy(p => new
                {
                    p.Date,
                    p.AdSetEid,
                    p.AdSetName,
                    p.KeywordEid,
                    p.KeywordName,
                    p.SearchTermName,
                    p.StrategyEid,
                    p.StrategyName,
                    p.StrategyTargetingType,
                    p.StrategyType,
                    p.SearchTermMediaType,
                })
                .Select(g => new SearchTermSummary
                {
                    Date = g.Key.Date,
                    AdSetEid = g.Key.AdSetEid,
                    AdSetName = g.Key.AdSetName,
                    KeywordEid = g.Key.KeywordEid,
                    KeywordName = g.Key.KeywordName,
                    SearchTermName = g.Key.SearchTermName,
                    StrategyEid = g.Key.StrategyEid,
                    StrategyName = g.Key.StrategyName,
                    StrategyTargetingType = g.Key.StrategyTargetingType,
                    StrategyType = g.Key.StrategyType,
                    SearchTermMediaType = g.Key.SearchTermMediaType,
                    Impressions = g.Sum(p => p.Impressions),
                    Clicks = g.Sum(p => p.Clicks),
                    Cost = g.Sum(p => p.Cost),
                    AllClicks = g.Sum(p => p.AllClicks),
                    PostClickConv = g.Sum(p => p.AllClicks),
                    PostClickRev = g.Sum(p => p.PostClickRev),
                    PostViewConv = g.Sum(p => p.PostViewConv),
                    PostViewRev = g.Sum(p => p.PostViewRev),
                });
            var targetSums = ExtractTargetSummaries(date);
            var targetItems = TransformSummaries(targetSums, date, campaignsData);
            var items = searchTermItems.Concat(targetItems);
            return items.ToList();
        }

        private IEnumerable<AmazonSearchTermDailySummary> ExtractSearchTermSummaries(DateTime date)
        {
            var spSums = _amazonUtility.ReportSearchTerms(CampaignType.SponsoredProducts, date, clientId, true);
            var sbSums = _amazonUtility.ReportSearchTerms(CampaignType.SponsoredBrands, date, clientId, true);
            var sums = spSums.Concat(sbSums);
            var filteredSums = FilterByCampaigns(sums, x => x.CampaignName);
            return filteredSums.ToList();
        }

        private IEnumerable<AmazonTargetSearchTermDailySummary> ExtractTargetSummaries(DateTime date)
        {
            var sums = _amazonUtility.ReportTargetSearchTerms(date, clientId, true);
            var filteredSums = FilterByCampaigns(sums, x => x.CampaignName);
            return filteredSums.ToList();
        }

        private IEnumerable<SearchTermSummary> TransformSummaries(
            IEnumerable<AmazonSearchTermDailySummary> searchTermStats,
            DateTime date,
            List<AmazonCampaign> campaignsData)
        {
            var notEmptyStats = searchTermStats.Where(x => !x.AllZeros()).ToList();
            var summaries = notEmptyStats.Select(stat =>
                CreateSummary(stat, date, campaignsData.FirstOrDefault(c => c.CampaignId == stat.CampaignId)));
            return summaries.ToList();
        }

        private IEnumerable<SearchTermSummary> TransformSummaries(
            IEnumerable<AmazonTargetSearchTermDailySummary> targetStats,
            DateTime date,
            List<AmazonCampaign> campaignsData)
        {
            var notEmptyStats = targetStats.Where(x => !x.AllZeros()).ToList();
            var summaries = notEmptyStats.Select(stat =>
                CreateSummary(stat, date, campaignsData.FirstOrDefault(c => c.CampaignId == stat.CampaignId)));
            return summaries.ToList();
        }

        private SearchTermSummary CreateSummary(
            AmazonSearchTermDailySummary searchTermStats,
            DateTime date,
            AmazonCampaign relatedCampaign)
        {
            var sum = CreateSummary(searchTermStats as AmazonStatSummary, date, relatedCampaign);
            sum.KeywordEid = searchTermStats.KeywordId;
            sum.KeywordName = searchTermStats.KeywordText;
            sum.SearchTermName = searchTermStats.Query;
            sum.SearchTermMediaType = searchTermStats.MatchType;
            return sum;
        }

        private SearchTermSummary CreateSummary(
            AmazonTargetSearchTermDailySummary targetStat,
            DateTime date,
            AmazonCampaign relatedCampaign)
        {
            var sum = CreateSummary(targetStat as AmazonStatSummary, date, relatedCampaign);
            sum.KeywordEid = targetStat.TargetId;
            sum.KeywordName = targetStat.TargetingText;
            sum.SearchTermName = targetStat.Query;
            return sum;
        }

        private SearchTermSummary CreateSummary(AmazonStatSummary stat, DateTime date, AmazonCampaign relatedCampaign)
        {
            var sum = new SearchTermSummary
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
