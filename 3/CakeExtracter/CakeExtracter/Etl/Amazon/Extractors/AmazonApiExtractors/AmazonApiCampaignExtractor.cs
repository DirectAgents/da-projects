using System;
using System.Collections.Generic;
using System.Linq;
using Amazon;
using Amazon.Entities;
using Amazon.Entities.Summaries;
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
    /// API extractor for the Campaign  level.
    /// </summary>
    internal class AmazonApiCampaignExtractor : BaseAmazonExtractor<StrategySummary>
    {
        private readonly AmazonSdMetadataExtractor campaignMetadataExtractor;

        /// <inheritdoc/>
        protected override string AmazonJobLevel => AmazonJobLevels.Strategy;

        /// <inheritdoc cref="BaseAmazonExtractor{CampaignSummary}"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonApiCampaignExtractor" /> class.
        /// </summary>
        /// <param name="amazonUtility">The amazon utility.</param>
        /// <param name="dateRange">Extraction date range.</param>
        /// <param name="account">Account data.</param>
        /// <param name="campaignFilter">Campaign filter value.</param>
        /// <param name="campaignFilterOut">Campaign filter value out.</param>
        public AmazonApiCampaignExtractor(
            AmazonUtility amazonUtility,
            DateRange dateRange,
            ExtAccount account,
            string campaignFilter = null,
            string campaignFilterOut = null)
            : base(amazonUtility, dateRange, account, campaignFilter, campaignFilterOut)
        {
            campaignMetadataExtractor = new AmazonSdMetadataExtractor(amazonUtility);
        }

        /// <summary>
        /// Removing old data before insert fresh data.
        /// </summary>
        /// <param name="date">Date for removing.</param>
        public override void RemoveOldData(DateTime date)
        {
            const int productDisplayCampaignTypeId = 8;
            Logger.Info(accountId, "The cleaning of CampaignSummaries for account ({0}) has begun - {1}.", accountId, date);
            AmazonTimeTracker.Instance.ExecuteWithTimeTracking(
                () =>
                {
                    SafeContextWrapper.TryMakeTransaction(
                        (ClientPortalProgContext db) =>
                        {
                            db.StrategySummaryMetrics.Where(x =>
                                    x.Date == date && x.Strategy.AccountId == accountId &&
                                    x.Strategy.TypeId == productDisplayCampaignTypeId)
                                .DeleteFromQuery();
                            db.StrategySummaries.Where(x =>
                                    x.Date == date && x.Strategy.AccountId == accountId &&
                                    x.Strategy.TypeId == productDisplayCampaignTypeId)
                                .DeleteFromQuery();
                        }, "DeleteFromQuery");
                },
                accountId,
                AmazonJobLevel,
                AmazonJobOperations.CleanExistingData);
            Logger.Info(accountId, "The cleaning of StrategySummaries for account ({0}) is over - {1}.", accountId, date);
        }

        /// <inheritdoc/>
        /// <summary>
        /// Extracting data of the Campaign level from API.
        /// </summary>
        protected override void Extract()
        {
            Logger.Info(
                accountId,
                "Extracting StrategySummaries from Amazon API for ({0}) from {1:d} to {2:d}",
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
        protected override IEnumerable<StrategySummary> GetDataFromApi(DateTime date, List<AmazonCampaign> campaignsData)
        {
            var strategySums = ExtractStrategySummaries(date);
            var strategyItems = TransformSummaries(strategySums, date, campaignsData);
            return strategyItems.ToList();
        }

        /// <inheritdoc/>
        protected override void ProcessFailedStatsExtraction(Exception e, DateTime fromDate, DateTime toDate)
        {
            Logger.Error(accountId, e);
            var exception = new FailedStatsLoadingException(fromDate, toDate, accountId, e, byCampaign: true);
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

        private IEnumerable<AmazonStrategyDailySummary> ExtractStrategySummaries(DateTime date)
        {
            var sums = _amazonUtility.ReportStrategy(date, clientId, true);
            var filteredSums = FilterByCampaigns(sums, x => x.CampaignName);
            return filteredSums.ToList();
        }

        private IEnumerable<StrategySummary> TransformSummaries(
            IEnumerable<AmazonStrategyDailySummary> strategyStats,
            DateTime date,
            List<AmazonCampaign> campaignsData)
        {
            var notEmptyStats = strategyStats.Where(x => !x.AllZeros()).ToList();
            var summaries = notEmptyStats.Select(stat =>
                CreateSummary(stat, date, campaignsData.FirstOrDefault(c => c.CampaignId == stat.CampaignId)));
            return summaries.ToList();
        }

        private StrategySummary CreateSummary(
            AmazonStrategyDailySummary strategyStats,
            DateTime date,
            AmazonCampaign relatedCampaign)
        {
            var sum = CreateSummary(strategyStats as AmazonStatSummary, date, relatedCampaign);
            return sum;
        }

        private StrategySummary CreateSummary(AmazonStatSummary stat, DateTime date, AmazonCampaign relatedCampaign)
        {
            var sum = new StrategySummary
            {
                StrategyEid = stat.CampaignId,
                StrategyName = stat.CampaignName,
                StrategyType = stat.CampaignType,
            };
            SetCPProgStats(sum, stat, date);
            return sum;
        }
    }
}