using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Etl.Amazon.Exceptions;
using CakeExtracter.Helpers;
using CakeExtracter.Logging.TimeWatchers.Amazon;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.Amazon.Loaders
{
    /// <summary>
    /// Base amazon level loader. Contains common for all amazon level loaders operations.
    /// </summary>
    /// <typeparam name="TSummaryLevelEntity">The type of the summary level entity.</typeparam>
    /// <typeparam name="TSummaryMetricLevelEntity">The type of the summary metric level entity.</typeparam>
    /// <seealso cref="Etl.Loader{TSummaryLevelEntity}" />
    public abstract class BaseAmazonLevelLoader<TSummaryLevelEntity, TSummaryMetricLevelEntity> : Loader<TSummaryLevelEntity>
        where TSummaryLevelEntity : DatedStatsSummary
        where TSummaryMetricLevelEntity : SummaryMetric, new()
    {
        private const int LoadingBatchesSize = 1000;

        private readonly AmazonSummaryMetricLoader<TSummaryMetricLevelEntity> summaryMetricsItemsLoader;

        public event Action<FailedStatsLoadingException> ProcessFailedExtraction;

        /// <summary>
        /// Gets the name of amazon job information level.
        /// </summary>
        /// <value>
        /// The name of the level.
        /// </value>
        protected abstract string LevelName
        {
            get;
        }

        /// <summary>
        /// Gets the locker object for multithreading operations.
        /// </summary>
        /// <value>
        /// The locker object.
        /// </value>
        protected abstract object LockerObject
        {
            get;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseAmazonLevelLoader{TSummaryLevelEntity, TSummaryMetricLevelEntity}"/> class.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        protected BaseAmazonLevelLoader(int accountId)
            : base(accountId, LoadingBatchesSize)
        {
            summaryMetricsItemsLoader = new AmazonSummaryMetricLoader<TSummaryMetricLevelEntity>();
        }

        public virtual void LoadItems(List<TSummaryLevelEntity> items)
        {
            AmazonTimeTracker.Instance.ExecuteWithTimeTracking(
                () => { EnsureRelatedItems(items); },
                accountId,
                LevelName,
                AmazonJobOperations.EnsureRelatedEntities);
            MergeSummaryItems(items);
            var summaryMetricItems = GetSummaryMetricsToInsert(items);
            MergeSummaryMetricItems(summaryMetricItems);
        }

        /// <summary>
        /// Loads the specified metrics and related summary metric items.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns>Count of items.</returns>
        protected override int Load(List<TSummaryLevelEntity> items)
        {
            try
            {
                Logger.Info(accountId, "Loading {0} {1}..", items.Count, LevelName);
                LoadItems(items);
                return items.Count;
            }
            catch (Exception e)
            {
                ProcessFailedStatsExtraction(e, items);
                return items.Count;
            }
        }

        protected virtual FailedStatsLoadingException GetFailedStatsLoadingException(Exception e, List<TSummaryLevelEntity> items)
        {
            var fromDate = items.Min(x => x.Date);
            var toDate = items.Max(x => x.Date);
            var fromDateArg = fromDate == default(DateTime) ? null : (DateTime?)fromDate;
            var toDateArg = toDate == default(DateTime) ? null : (DateTime?)toDate;
            var exception = new FailedStatsLoadingException(fromDateArg, toDateArg, accountId, e);
            return exception;
        }

        /// <summary>
        /// Ensures the related entities in db by it's summary items.
        /// </summary>
        /// <param name="items">The items.</param>
        protected abstract void EnsureRelatedItems(List<TSummaryLevelEntity> items);

        /// <summary>
        /// Sets the summary metric entity identifier.
        /// </summary>
        /// <param name="summary">The summary.</param>
        /// <param name="summaryMetric">The summary metric.</param>
        protected abstract void SetSummaryMetricEntityId(TSummaryLevelEntity summary, SummaryMetric summaryMetric);

        /// <summary>
        /// Merge the summary metric items into db.
        /// </summary>
        /// <param name="items">The items.</param>
        protected void MergeSummaryMetricItems(List<SummaryMetric> items)
        {
            AmazonTimeTracker.Instance.ExecuteWithTimeTracking(
                () =>
                {
                    SafeContextWrapper.Lock(
                        LockerObject,
                        () => { summaryMetricsItemsLoader.MergeSummaryMetrics(items); });
                },
                accountId,
                LevelName,
                AmazonJobOperations.LoadSummaryMetricItemsData);
        }

        /// <summary>
        /// Merge the summary items into db.
        /// </summary>
        /// <param name="items">The items.</param>
        protected void MergeSummaryItems(List<TSummaryLevelEntity> items)
        {
            AmazonTimeTracker.Instance.ExecuteWithTimeTracking(
                () =>
                {
                    SafeContextWrapper.TryMakeTransactionWithLock(
                        (ClientPortalProgContext db) => db.BulkMerge(items),
                        LockerObject,
                        "BulkMerge");
                },
                accountId,
                LevelName,
                AmazonJobOperations.LoadSummaryItemsData);
        }

        private List<SummaryMetric> GetSummaryMetricsToInsert(List<TSummaryLevelEntity> summaries)
        {
            var summaryMetricsToInsert = new List<SummaryMetric>();
            summaries.ForEach(summary =>
            {
                if (summary.InitialMetrics != null)
                {
                    summary.InitialMetrics.ToList().ForEach(summaryMetric => SetSummaryMetricEntityId(summary, summaryMetric));
                    summaryMetricsToInsert.AddRange(summary.InitialMetrics);
                }
            });
            return summaryMetricsToInsert;
        }

        private void ProcessFailedStatsExtraction(Exception e, List<TSummaryLevelEntity> items)
        {
            Logger.Error(accountId, e);
            var exception = GetFailedStatsLoadingException(e, items);
            ProcessFailedExtraction?.Invoke(exception);
        }
    }
}
