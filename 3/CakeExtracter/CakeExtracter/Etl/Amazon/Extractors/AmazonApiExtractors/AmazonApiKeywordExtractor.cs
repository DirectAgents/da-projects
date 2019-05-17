using Amazon;
using Amazon.Entities.Summaries;
using Amazon.Enums;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;
using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using CakeExtracter.Logging.TimeWatchers.Amazon;
using CakeExtracter.Logging.TimeWatchers;
using Amazon.Entities;
using CakeExtracter.Common.JobExecutionManagement;
using CakeExtracter.Etl.Amazon.Exceptions;

namespace CakeExtracter.Etl.TradingDesk.Extracters.AmazonExtractors.AmazonApiExtractors
{
    /// <summary>
    /// Keyword data extractor for amazon job.
    /// </summary>
    /// <seealso cref="CakeExtracter.Etl.TradingDesk.Extracters.AmazonExtractors.BaseAmazonExtractor{DirectAgents.Domain.Entities.CPProg.KeywordSummary}" />
    public class AmazonApiKeywordExtractor : BaseAmazonExtractor<KeywordSummary>
    {
        private readonly AmazonCampaignMetadataExtractor campaignMetadataExtractor;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonApiKeywordExtractor"/> class.
        /// </summary>
        /// <param name="amazonUtility">The amazon utility.</param>
        /// <param name="dateRange">Extraction date range.</param>
        /// <param name="account">Account data.</param>
        /// <param name="clearBeforeLoad">Parameter value. Indicates whether clean before update needed.</param>
        /// <param name="campaignFilter">Campaign filter value.</param>
        /// <param name="campaignFilterOut">Campaign filter value out.</param>
        public AmazonApiKeywordExtractor(AmazonUtility amazonUtility, DateRange dateRange, ExtAccount account, string campaignFilter = null, string campaignFilterOut = null)
            : base(amazonUtility, dateRange, account, campaignFilter, campaignFilterOut)
        {
            campaignMetadataExtractor = new AmazonCampaignMetadataExtractor(amazonUtility);
        }

        /// <summary>
        /// The derived class implements this method, which calls Add() for each item
        /// extracted and then calls End() when complete.
        /// </summary>
        protected override void Extract()
        {
            Logger.Info(accountId, "Extracting KeywordSummaries from Amazon API for ({0}) from {1:d} to {2:d}",
                clientId, dateRange.FromDate, dateRange.ToDate);
            var campaignsData = GetCampaignInfo();
            foreach (var date in dateRange.Dates)
            {
                try
                {
                    ExtractDaily(date, campaignsData);
                }
                catch (Exception e)
                {
                    ProcessFailedStatsExtraction(e, date);
                }
            }
            End();
        }

        private void ExtractDaily(DateTime date, List<AmazonCampaign> campaignsData)
        {
            CommandExecutionContext.Current?.SetJobExecutionStateInHistory($"Keywords Level- {date.ToString()}", accountId);
            IEnumerable<KeywordSummary> items = null;
            AmazonTimeTracker.Instance.ExecuteWithTimeTracking(() =>
            {
                items = GetKeywordSummariesFromApi(date, campaignsData);
            }, accountId, AmazonJobLevels.keyword, AmazonJobOperations.reportExtracting);
            RemoveOldData(date);
            Add(items);
        }

        private List<AmazonCampaign> GetCampaignInfo()
        {
            List<AmazonCampaign> campaignsData = null;
            AmazonTimeTracker.Instance.ExecuteWithTimeTracking(() =>
            {
                campaignsData = campaignMetadataExtractor.LoadCampaignsMetadata(accountId, clientId).ToList();
            }, accountId, AmazonJobLevels.keyword, AmazonJobOperations.loadCampaignMetadata);
            return campaignsData;
        }

        private void ProcessFailedStatsExtraction(Exception e, DateTime date)
        {
            Logger.Error(accountId, e);
            var exception = new FailedStatsExtractionException(date, date, accountId, e, byKeyword: true);
            InvokeProcessFailedExtractionHandlers(exception);
        }

        private IEnumerable<KeywordSummary> GetKeywordSummariesFromApi(DateTime date, List<AmazonCampaign> campaignsData)
        {
            var keywordSums = ExtractKeywordSummaries(date);
            var keywordItems = TransformSummaries(keywordSums, date, campaignsData);
            var targetSums = ExtractTargetSummaries(date);
            var targetItems = TransformSummaries(targetSums, date, campaignsData);
            var items = keywordItems.Concat(targetItems);
            return items.ToList();
        }

        private IEnumerable<AmazonKeywordDailySummary> ExtractKeywordSummaries(DateTime date)
        {
            var spSums = _amazonUtility.ReportKeywords(CampaignType.SponsoredProducts, date, clientId, true);
            var sbSums = _amazonUtility.ReportKeywords(CampaignType.SponsoredBrands, date, clientId, true);
            var sums = spSums.Concat(sbSums);
            var filteredSums = FilterByCampaigns(sums, x => x.CampaignName);
            return filteredSums.ToList();
        }

        private IEnumerable<AmazonTargetKeywordDailySummary> ExtractTargetSummaries(DateTime date)
        {
            var sums = _amazonUtility.ReportTargetKeywords(date, clientId, true);
            var filteredSums = FilterByCampaigns(sums, x => x.CampaignName);
            return filteredSums.ToList();
        }

        private IEnumerable<KeywordSummary> TransformSummaries(IEnumerable<AmazonKeywordDailySummary> keywordStats, DateTime date, List<AmazonCampaign> campaignsData)
        {
            var notEmptyStats = keywordStats.Where(x => !x.AllZeros()).ToList();
            var summaries = notEmptyStats.Select(stat => CreateSummary(stat, date,
                campaignsData.FirstOrDefault(c => c.CampaignId == stat.CampaignId)));
            return summaries.ToList();
        }

        private IEnumerable<KeywordSummary> TransformSummaries(IEnumerable<AmazonTargetKeywordDailySummary> targetStats, DateTime date, List<AmazonCampaign> campaignsData)
        {
            var notEmptyStats = targetStats.Where(x => !x.AllZeros()).ToList();
            var summaries = notEmptyStats.Select(stat => CreateSummary(stat, date,
                campaignsData.FirstOrDefault(c => c.CampaignId == stat.CampaignId)));
            return summaries.ToList();
        }

        private KeywordSummary CreateSummary(AmazonKeywordDailySummary keywordStat, DateTime date, AmazonCampaign relatedCampaign)
        {
            var sum = CreateSummary(keywordStat as AmazonStatSummary, date, relatedCampaign);
            sum.KeywordEid = keywordStat.KeywordId;
            sum.KeywordName = keywordStat.KeywordText;
            return sum;
        }

        private KeywordSummary CreateSummary(AmazonTargetKeywordDailySummary targetStat, DateTime date, AmazonCampaign relatedCampaign)
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
                StrategyTargetingType = relatedCampaign != null ? relatedCampaign.TargetingType : null
            };
            SetCPProgStats(sum, stat, date);
            return sum;
        }

        private void RemoveOldData(DateTime date)
        {
            Logger.Info(accountId, "The cleaning of KeywordSummaries for account ({0}) has begun - {1}.", accountId, date);
            AmazonTimeTracker.Instance.ExecuteWithTimeTracking(() =>
            {
                SafeContextWrapper.TryMakeTransaction((ClientPortalProgContext db) =>
                {
                    db.KeywordSummaryMetrics.Where(x => x.Date == date && x.Keyword.AccountId == accountId).DeleteFromQuery();
                    db.KeywordSummaries.Where(x => x.Date == date && x.Keyword.AccountId == accountId).DeleteFromQuery();
                }, "DeleteFromQuery");
            }, accountId, AmazonJobLevels.keyword, AmazonJobOperations.cleanExistingData);
            Logger.Info(accountId, "The cleaning of KeywordSummaries for account ({0}) is over - {1}.", accountId, date);
        }
    }
}
