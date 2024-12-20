﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using CakeExtracter.Etl.TradingDesk.LoadersDA;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.YAM.Outdated
{
    [Obsolete]
    public class TDStrategyConValLoader : Loader<StrategySummary>
    {
        private readonly TDStrategySummaryLoader strategySummaryLoader;

        [Obsolete]
        public TDStrategyConValLoader(int accountId) : base(accountId)
        {
            strategySummaryLoader = new TDStrategySummaryLoader(accountId);
        }

        protected override int Load(List<StrategySummary> items)
        {
            Logger.Info(accountId, "Loading {0} DA-TD StrategySummary ConVals..", items.Count);
            strategySummaryLoader.PrepareData(items);
            strategySummaryLoader.AddUpdateDependentStrategies(items);
            strategySummaryLoader.AssignStrategyIdToItems(items);
            var count = UpsertStrategySummaryConVals(items);
            return count;
        }

        [Obsolete]
        public int UpsertStrategySummaryConVals(List<StrategySummary> items)
        {
            var progress = new LoadingProgress();
            using (var db = new ClientPortalProgContext())
            {
                foreach (var item in items)
                {
                    var target = db.Set<StrategySummary>().Find(item.Date, item.StrategyId);
                    if (target == null)
                    {
                        if (item.PostClickRev != 0 || item.PostViewRev != 0)
                        {
                            var ds = new StrategySummary
                            {
                                Date = item.Date,
                                StrategyId = item.StrategyId,
                                PostClickRev = item.PostClickRev,
                                PostViewRev = item.PostViewRev
                            };
                            db.StrategySummaries.Add(ds);
                            progress.AddedCount++;
                        }
                        else
                        {
                            progress.AlreadyDeletedCount++;
                        }
                    }
                    else // StrategySummary already exists
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
                            Logger.Warn(accountId, "Encountered duplicate for {0:d} - StrategyId {1}", item.Date, item.StrategyId);
                            progress.DuplicateCount++;
                        }
                    }

                    progress.ItemCount++;
                }

                db.SaveChanges();
            }

            Logger.Info(accountId, "Saving {0} StrategySummary ConVals ({1} updates, {2} additions, {3} duplicates, {4} already-deleted)",
                progress.ItemCount, progress.UpdatedCount, progress.AddedCount, progress.DuplicateCount, progress.AlreadyDeletedCount);
            if (progress.DuplicateCount > 0)
            {
                Logger.Warn(accountId, "Encountered {0} duplicates which were skipped", progress.DuplicateCount);
            }
            return progress.ItemCount;
        }

    }
}
