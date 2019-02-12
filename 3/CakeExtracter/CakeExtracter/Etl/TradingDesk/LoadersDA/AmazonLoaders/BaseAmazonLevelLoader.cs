using CakeExtracter.Helpers;
using CakeExtracter.Logging.TimeWatchers;
using CakeExtracter.Logging.TimeWatchers.Amazon;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using System.Collections.Generic;
using System.Linq;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA.AmazonLoaders
{
    public abstract class BaseAmazonLevelLoader<TSummaryLevelEntity, TSummaryMetricLevelEntity> : Loader<TSummaryLevelEntity>
        where TSummaryLevelEntity : DatedStatsSummary
        where TSummaryMetricLevelEntity : SummaryMetric, new()
    {
        private const int loadingBatchesSize = 500;

        private readonly AmazonSummaryMetricLoader<TSummaryMetricLevelEntity> summaryMetricsItemsLoader;

        protected abstract string LevelName
        {
            get;
        }

        protected abstract object LockerObject
        {
            get;
        }

        protected BaseAmazonLevelLoader(int accountId)
            : base(accountId, loadingBatchesSize)
        {
            summaryMetricsItemsLoader = new AmazonSummaryMetricLoader<TSummaryMetricLevelEntity>();
        }

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

        protected abstract void EnsureRelatedItems(List<TSummaryLevelEntity> items);

        protected abstract void SetSummaryMetricEntityId(TSummaryLevelEntity summary, SummaryMetric summaryMetric);

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

        protected void UpsertSummaryItems(List<TSummaryLevelEntity> items)
        {
            AmazonTimeTracker.Instance.ExecuteWithTimeTracking(() =>
            {
                SafeContextWrapper.Lock(LockerObject, () =>
                {
                    using (var db = new ClientPortalProgContext())
                    {
                        db.BulkInsert(items);
                    }
                });
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
