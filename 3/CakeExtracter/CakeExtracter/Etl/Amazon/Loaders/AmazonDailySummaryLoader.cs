using System;
using System.Collections.Generic;
using CakeExtracter.Etl.Amazon.Exceptions;
using CakeExtracter.Etl.TradingDesk.LoadersDA;
using CakeExtracter.Helpers;
using CakeExtracter.Logging.TimeWatchers.Amazon;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.Amazon.Loaders
{
    /// <summary>
    /// Summary loader for amazon Account level.
    /// </summary>
    /// <seealso cref="BaseAmazonLevelLoader{DailySummary, DailySummaryMetric}" />
    public class AmazonDailySummaryLoader : BaseAmazonLevelLoader<DailySummary,DailySummaryMetric>
    {
        private readonly TDDailySummaryLoader summaryItemsLoader;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonDailySummaryLoader"/> class.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        public AmazonDailySummaryLoader(int accountId)
            : base(accountId)
        {
            summaryItemsLoader = new TDDailySummaryLoader(accountId);
        }

        /// <summary>
        /// Gets the name of amazon job information level.
        /// </summary>
        /// <value>
        /// The name of the level.
        /// </value>
        protected override string LevelName => AmazonJobLevels.Account;

        /// <summary>
        /// Gets the locker object for multithreading operations.
        /// </summary>
        /// <value>
        /// The locker object.
        /// </value>
        protected override object LockerObject => SafeContextWrapper.DailyLocker;

        /// <summary>
        /// Ensures the related items.
        /// </summary>
        /// <param name="summaryItems">The summary items.</param>
        protected override void EnsureRelatedItems(List<DailySummary> summaryItems)
        {
            summaryItemsLoader.AssignIdsToItems(summaryItems);
        }

        /// <summary>
        /// Sets the summary metric entity identifier.
        /// </summary>
        /// <param name="summary">The summary.</param>
        /// <param name="summaryMetric">The summary metric.</param>
        protected override void SetSummaryMetricEntityId(DailySummary summary, SummaryMetric summaryMetric)
        {
            summaryMetric.EntityId = summary.AccountId;
        }

        protected override FailedStatsLoadingException GetFailedStatsLoadingException(Exception e, List<DailySummary> items)
        {
            var exception = base.GetFailedStatsLoadingException(e, items);
            exception.ByDaily = true;
            return exception;
        }
    }
}
