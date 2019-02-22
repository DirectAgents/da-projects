using CakeExtracter.Helpers;
using CakeExtracter.Logging.TimeWatchers;
using CakeExtracter.Logging.TimeWatchers.Amazon;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using System.Collections.Generic;
using System.Linq;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA.AmazonLoaders
{
    /// <summary>
    /// Base amazon level loader. Contains common for all amazon level loaders operations.
    /// </summary>
    /// <typeparam name="TSummaryLevelEntity">The type of the summary level entity.</typeparam>
    /// <typeparam name="TSummaryMetricLevelEntity">The type of the summary metric level entity.</typeparam>
    /// <seealso cref="CakeExtracter.Etl.Loader{TSummaryLevelEntity}" />
    public abstract class BaseAmazonLevelLoader<TSummaryLevelEntity, TSummaryMetricLevelEntity> : Loader<TSummaryLevelEntity>
        where TSummaryLevelEntity : DatedStatsSummary
        where TSummaryMetricLevelEntity : SummaryMetric, new()
    {
        private const int loadingBatchesSize = 500;

        private readonly AmazonSummaryMetricLoader<TSummaryMetricLevelEntity> summaryMetricsItemsLoader;

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
            : base(accountId, loadingBatchesSize)
        {
            summaryMetricsItemsLoader = new AmazonSummaryMetricLoader<TSummaryMetricLevelEntity>();
        }

        /// <summary>
        /// Loads the specified metrics and related summary metric items.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        protected override int Load(List<TSummaryLevelEntity> items)
        {
            Logger.Info(accountId, "Loading {0} {1}..", items.Count, LevelName);
            AmazonTimeTracker.Instance.ExecuteWithTimeTracking(() =>
            {
                EnsureRelatedItems(items);
            }, accountId, LevelName, AmazonJobOperations.ensureRelatedEntities);
            UpsertSummaryItems(items);
            var summaryMetricItems = GetSummaryMetricsToInsert(items);
            UpsertSummaryMetricItems(summaryMetricItems);
            return items.Count;
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
        /// Upserts the summary metric items into db.
        /// </summary>
        /// <param name="items">The items.</param>
        protected void UpsertSummaryMetricItems(List<SummaryMetric> items)
        {
            AmazonTimeTracker.Instance.ExecuteWithTimeTracking(() =>
            {
                SafeContextWrapper.Lock(LockerObject, () =>
                {
                    summaryMetricsItemsLoader.UpsertSummaryMetrics(items);
                });
            }, accountId, LevelName, AmazonJobOperations.loadSummaryMetricItemsData);
        }

        /// <summary>
        /// Upserts the summary items into db.
        /// </summary>
        /// <param name="items">The items.</param>
        protected void UpsertSummaryItems(List<TSummaryLevelEntity> items)
        {
            AmazonTimeTracker.Instance.ExecuteWithTimeTracking(() =>
            {
                SafeContextWrapper.TryBulkInsert<ClientPortalProgContext, TSummaryLevelEntity>(LockerObject, items);
            }, accountId, LevelName, AmazonJobOperations.loadSummaryItemsData);
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
    }
}
