using System;
using System.Collections.Generic;
using System.Data.Entity;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.YAM.Outdated
{
    [Obsolete]
    public class TDDailyConValLoader : Loader<DailySummary>
    {
        [Obsolete]
        public TDDailyConValLoader(int accountId) : base(accountId) { }

        protected override int Load(List<DailySummary> items)
        {
            Logger.Info(accountId, "Loading {0} DA-TD DailySummary ConVals..", items.Count);
            var count = UpsertDailySummaryConVals(items);
            return count;
        }

        [Obsolete]
        public int UpsertDailySummaryConVals(List<DailySummary> items)
        {
            var progress = new LoadingProgress();
            using (var db = new ClientPortalProgContext())
            {
                foreach (var item in items)
                {
                    {
                        var target = db.Set<DailySummary>().Find(item.Date, accountId);
                        if (target == null)
                        {
                            if (item.PostClickRev != 0 || item.PostViewRev != 0)
                            {
                                var ds = new DailySummary
                                {
                                    Date = item.Date,
                                    AccountId = accountId,
                                    PostClickRev = item.PostClickRev,
                                    PostViewRev = item.PostViewRev
                                };
                                db.DailySummaries.Add(ds);
                                progress.AddedCount++;
                            }
                            else
                            {
                                progress.AlreadyDeletedCount++;
                            }
                        }
                        else // DailySummary already exists
                        {
                            var entry = db.Entry(target);
                            if (entry.State == EntityState.Unchanged)
                            {
                                target.PostClickRev = item.PostClickRev;
                                target.PostViewRev = item.PostViewRev;
                                progress.UpdatedCount++;
                            }
                            else
                            {
                                Logger.Warn(accountId, "Encountered duplicate for {0:d} - Acct {1}", item.Date,
                                    item.AccountId);
                                progress.DuplicateCount++;
                            }
                        }

                        progress.ItemCount++;
                    }
                }

                db.SaveChanges();
            }

            Logger.Info(accountId, "Saving {0} DailySummary ConVals ({1} updates, {2} additions, {3} duplicates, {4} already-deleted)",
                progress.ItemCount, progress.UpdatedCount, progress.AddedCount, progress.DuplicateCount, progress.AlreadyDeletedCount);
            if (progress.DuplicateCount > 0)
            {
                Logger.Warn(accountId, "Encountered {0} duplicates which were skipped", progress.DuplicateCount);
            }
            return progress.ItemCount;
        }

    }
}
