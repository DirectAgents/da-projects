using System;
using System.Collections.Generic;
using CakeExtracter.Etl.Amazon.Exceptions;
using CakeExtracter.Helpers;
using CakeExtracter.Logging.TimeWatchers;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA.AmazonLoaders
{
    /// <summary>
    /// Summary loader for amazon Compaign level.
    /// </summary>
    /// <seealso cref="CakeExtracter.Etl.TradingDesk.LoadersDA.AmazonLoaders.BaseAmazonLevelLoader{DirectAgents.Domain.Entities.CPProg.StrategySummary, DirectAgents.Domain.Entities.CPProg.StrategySummaryMetric}" />
    public class AmazonCampaignSummaryLoader : BaseAmazonLevelLoader<StrategySummary, StrategySummaryMetric>
    {
        private readonly TDStrategySummaryLoader summaryItemsLoader;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonCampaignSummaryLoader"/> class.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        public AmazonCampaignSummaryLoader(int accountId)
            : base(accountId)
        {
            summaryItemsLoader = new TDStrategySummaryLoader(accountId);
        }

        /// <summary>
        /// Gets the name of amazon job information level.
        /// </summary>
        /// <value>
        /// The name of the level.
        /// </value>
        protected override string LevelName => AmazonJobLevels.strategy;

        /// <summary>
        /// Gets the locker object for multithreading operations.
        /// </summary>
        /// <value>
        /// The locker object.
        /// </value>
        protected override object LockerObject => SafeContextWrapper.StrategyLocker;

        /// <summary>
        /// Ensures the related items.
        /// </summary>
        /// <param name="summaryItems">The summary items.</param>
        protected override void EnsureRelatedItems(List<StrategySummary> summaryItems)
        {
            summaryItemsLoader.PrepareData(summaryItems);
            summaryItemsLoader.AddUpdateDependentStrategies(summaryItems);
            summaryItemsLoader.AssignStrategyIdToItems(summaryItems);
        }

        /// <summary>
        /// Sets the summary metric entity identifier.
        /// </summary>
        /// <param name="summary">The summary.</param>
        /// <param name="summaryMetric">The summary metric.</param>
        protected override void SetSummaryMetricEntityId(StrategySummary summary, SummaryMetric summaryMetric)
        {
            summaryMetric.EntityId = summary.StrategyId;
        }

        protected override FailedStatsLoadingException GetFailedStatsLoadingException(Exception e, List<StrategySummary> items)
        {
            var exception = base.GetFailedStatsLoadingException(e, items);
            exception.ByCampaign = true;
            return exception;
        }
    }
}
