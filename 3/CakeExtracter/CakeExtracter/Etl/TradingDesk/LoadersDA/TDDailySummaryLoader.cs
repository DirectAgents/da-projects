using System.Collections.Generic;
using System.Data.Entity;
using AutoMapper;
using CakeExtracter.Etl.TradingDesk.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA
{
    public class TDDailySummaryLoader : Loader<DailySummary>
    {
        private readonly SummaryMetricLoader metricLoader;

        public TDDailySummaryLoader(int accountId = -1) : base(accountId)
        {
            metricLoader = new SummaryMetricLoader();
        }

        protected override int Load(List<DailySummary> items)
        {
            Logger.Info(accountId, "Loading {0} DA-TD DailySummaries..", items.Count);
            AssignIdsToItems(items);
            var count = UpsertDailySummaries(items);
            return count;
        }

        public void AssignIdsToItems(List<DailySummary> items)
        {
            foreach (var item in items)
            {
                item.AccountId = accountId;
            }
        }

        public int UpsertDailySummaries(List<DailySummary> items)
        {
            var progress = new LoadingProgress();
            using (var db = new ClientPortalProgContext())
            {
                foreach (var item in items)
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
                    progress.ItemCount++;
                }

                Logger.Info(accountId, "Saving {0} DailySummaries ({1} updates, {2} additions, {3} duplicates, {4} deleted, {5} already-deleted)",
                    progress.ItemCount, progress.UpdatedCount, progress.AddedCount, progress.DuplicateCount, progress.DeletedCount, progress.AlreadyDeletedCount);
                if (progress.DuplicateCount > 0)
                {
                    Logger.Warn(accountId, "Encountered {0} duplicates which were skipped", progress.DuplicateCount);
                }
                var numChanges = db.SaveChanges();
            }
            return progress.ItemCount;
        }

        private void TryToAddSummary(ClientPortalProgContext db, DailySummary item, LoadingProgress progress)
        {
            if (item.AllZeros())
            {
                progress.AlreadyDeletedCount++;
                return;
            }
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
            if (item.AllZeros())
            {
                entry.State = EntityState.Deleted;
                progress.DeletedCount++;
                return;
            }
            entry.State = EntityState.Detached;
            Mapper.Map(item, target);
            entry.State = EntityState.Modified;
            UpsertSummaryMetrics(db, item);
            progress.UpdatedCount++;
        }

        private void UpsertSummaryMetrics(ClientPortalProgContext db, DailySummary item)
        {
            item.Metrics.ForEach(x => x.EntityId = item.AccountId);
            metricLoader.UpsertSummaryMetrics<DailySummaryMetric>(db, item.Metrics);
        }
    }
}
