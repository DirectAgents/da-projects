using System.Collections.Generic;
using System.Data.Entity;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.TD;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA
{
    public class TDSiteSummaryLoader : Loader<SiteSummary>
    {
        //public TDSiteSummaryLoader() { }

        protected override int Load(List<SiteSummary> items)
        {
            Logger.Info("Loading {0} DA-TD SiteSummaries..", items.Count);
            var count = UpsertDailySummaries(items);
            return count;
        }

        public int UpsertDailySummaries(List<SiteSummary> items)
        {
            var addedCount = 0;
            var updatedCount = 0;
            var duplicateCount = 0;
            var deletedCount = 0;
            var alreadyDeletedCount = 0;
            var itemCount = 0;
            using (var db = new DATDContext())
            {
                foreach (var item in items)
                {
                    var target = db.Set<SiteSummary>().Find(item.Date, item.SiteId, item.AccountId);
                    if (target == null)
                    {
                        if (!item.AllZeros())
                        {
                            db.SiteSummaries.Add(item);
                            addedCount++;
                        }
                        else
                            alreadyDeletedCount++;
                    }
                    else // Summary already exists
                    {
                        var entry = db.Entry(target);
                        if (entry.State == EntityState.Unchanged)
                        {
                            if (!item.AllZeros())
                            {
                                entry.State = EntityState.Detached;
                                AutoMapper.Mapper.Map(item, target);
                                entry.State = EntityState.Modified;
                                updatedCount++;
                            }
                            else
                                entry.State = EntityState.Deleted;
                        }
                        else
                        {
                            Logger.Warn("Encountered duplicate for {0:d} - Site {1}, Account {2}", item.Date, item.Site, item.AccountId);
                            duplicateCount++;
                        }
                    }
                    itemCount++;
                }
                Logger.Info("Saving {0} SiteSummaries ({1} updates, {2} additions, {3} duplicates, {4} deleted, {5} already-deleted)",
                            itemCount, updatedCount, addedCount, duplicateCount, deletedCount, alreadyDeletedCount);
                if (duplicateCount > 0)
                    Logger.Warn("Encountered {0} duplicates which were skipped", duplicateCount);
                int numChanges = db.SaveChanges();
            }
            return itemCount;
        }
    }
}
