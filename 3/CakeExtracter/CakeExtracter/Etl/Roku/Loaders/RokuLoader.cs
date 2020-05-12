using System.Collections.Generic;
using System.Data.Entity;

using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg.Roku;
using Quartz.Util;

namespace CakeExtracter.Etl.Roku.Loaders
{
    public class RokuLoader : Loader<RokuSummary>
    {
        protected override int Load(List<RokuSummary> items)
        {
            Logger.Info($"Loading {items.Count} RokuSummaries (to add)...");
            if (items.Count == 0)
            {
                return 0;
            }
            var count = UpsertSearchRokuSummaries(items);
            return count;
        }

        private int UpsertSearchRokuSummaries(List<RokuSummary> rokuSummaries)
        {
            var progress = new LoadingProgress();
            using (var db = new ClientPortalProgContext())
            {
                foreach (var rokuStat in rokuSummaries)
                {
                    var target = db.Set<RokuSummary>().Find(rokuStat.Id);
                    if (target == null)
                    {
                        db.RokuSummaries.Add(rokuStat);
                        progress.AddedCount++;
                    }
                    else
                    {
                        var entry = db.Entry(target);
                        if (entry.State != EntityState.Unchanged)
                        {
                            Logger.Warn($"Encountered duplicate for {rokuStat.Id} - {rokuStat.OrderName}");
                            progress.DuplicateCount++;
                        }
                        else
                        {
                            entry.State = EntityState.Detached;
                            AutoMapper.Mapper.Map(rokuStat, target);
                            entry.State = EntityState.Modified;
                            progress.UpdatedCount++;
                        }
                    }
                    progress.ItemCount++;
                }
                SafeContextWrapper.TrySaveChanges(db);
            }
            Logger.Info($"Saving {progress.ItemCount} RokuSummaries ({progress.UpdatedCount} updates, {progress.AddedCount} additions, {progress.DuplicateCount} duplicates)");
            return rokuSummaries.Count;
        }
    }
}