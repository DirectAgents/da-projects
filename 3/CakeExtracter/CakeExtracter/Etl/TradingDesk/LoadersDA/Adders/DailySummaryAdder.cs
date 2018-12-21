using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using CakeExtracter.Etl.TradingDesk.LoadersDA.Interfaces;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA.Adders
{
    /// <summary>
    /// This loader adds daily summary values to existing summaries in the database or creates new ones if they do not already exist.
    /// </summary>
    class DailySummaryAdder : Loader<DailySummary>, IDailySummaryLoader
    {
        private readonly ISummaryMetricLoader metricLoader;
        private readonly TDDailySummaryLoader tdDailySummaryLoader;

        public DailySummaryAdder(int accountId) : base(accountId)
        {
            metricLoader = new SummaryMetricAdder();
            tdDailySummaryLoader = new TDDailySummaryLoader(accountId);
        }

        protected override int Load(List<DailySummary> items)
        {
            Logger.Info(accountId, "Loading {0} DailySummaries (to add)...", items.Count);
            AssignIdsToItems(items);
            var count = UpsertDailySummaries(items);
            return count;
        }

        public void AssignIdsToItems(List<DailySummary> items)
        {
            tdDailySummaryLoader.AssignIdsToItems(items);
        }

        public int UpsertDailySummaries(List<DailySummary> items)
        {
            var progress = new LoadingProgress();
            using (var db = new ClientPortalProgContext())
            {
                foreach (var item in items)
                {
                    TryProcessSummary(db, item, progress);
                }

                SafeContextWrapper.TrySaveChanges(db);
            }

            LogResultOfSummaryProcessing(progress);
            return progress.ItemCount;
        }

        private void TryProcessSummary(ClientPortalProgContext db, DailySummary item, LoadingProgress progress)
        {
            if (item.AllZeros())
            {
                progress.AlreadyDeletedCount++;
            }
            else
            {
                ProcessSummary(db, item, progress);
            }

            progress.ItemCount++;
        }

        private void LogResultOfSummaryProcessing(LoadingProgress progress)
        {
            Logger.Info(accountId, "Saving {0} DailySummaries ({1} updates, {2} additions, {3} duplicates, {4} already-deleted)",
                progress.ItemCount, progress.UpdatedCount, progress.AddedCount, progress.DuplicateCount, progress.AlreadyDeletedCount);
            if (progress.DuplicateCount > 0)
            {
                Logger.Warn(accountId, "Encountered {0} duplicates which were skipped", progress.DuplicateCount);
            }
        }

        private void ProcessSummary(ClientPortalProgContext db, DailySummary item, LoadingProgress progress)
        {
            var target = db.Set<DailySummary>().Find(item.Date, item.AccountId);
            if (target == null)
            {
                TryToAddSummary(db, item, progress);
            }
            else
            {
                TryToUpdateSummary(db, item, target, progress);
            }
        }

        private void TryToAddSummary(ClientPortalProgContext db, DailySummary item, LoadingProgress progress)
        {
            db.DailySummaries.Add(item);
            UpsertSummaryMetrics(db, item);
            progress.AddedCount++;
        }

        private void TryToUpdateSummary(ClientPortalProgContext db, DailySummary item, DailySummary target, LoadingProgress progress)
        {
            var entry = db.Entry(target);
            if (entry.State != EntityState.Unchanged)
            {
                Logger.Warn(accountId, "Encountered duplicate for {0:d} - Acct {1}", item.Date, item.AccountId);
                progress.DuplicateCount++;
                return;
            }
            UpdateSummary(entry, item, target);
            UpsertSummaryMetrics(db, item);
            progress.UpdatedCount++;
        }

        private void UpdateSummary(DbEntityEntry<DailySummary> entry, DailySummary item, DailySummary target)
        {
            entry.State = EntityState.Detached;
            target.AddStats(item);
            entry.State = EntityState.Modified;
        }

        private void UpsertSummaryMetrics(ClientPortalProgContext db, DailySummary item)
        {
            if (item.InitialMetrics == null || !item.InitialMetrics.Any())
            {
                return;
            }
            item.InitialMetrics.ForEach(x => x.EntityId = item.AccountId);
            metricLoader.AddDependentMetricTypes(item.InitialMetrics);
            metricLoader.AssignMetricTypeIdToItems(item.InitialMetrics);
            metricLoader.UpsertSummaryMetrics<DailySummaryMetric>(db, item.InitialMetrics);
        }
    }
}
