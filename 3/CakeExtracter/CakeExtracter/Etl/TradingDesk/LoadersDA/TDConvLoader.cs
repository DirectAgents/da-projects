using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.TD;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA
{
    public class TDConvLoader : Loader<Conv>
    {
        //public TDConvLoader() { }

        protected override int Load(List<Conv> items)
        {
            Logger.Info("Loading {0} DA-TD Convs..", items.Count);
            var count = UpsertConvs(items);
            return count;
        }

        public int UpsertConvs(List<Conv> items)
        {
            var addedCount = 0;
            var updatedCount = 0;
            var duplicateCount = 0;
            var unmatchedCount = 0;
            var itemCount = 0;
            using (var db = new DATDContext())
            {
                foreach (var item in items)
                {
                    var targets = db.Convs.Where(c => c.AccountId == item.AccountId && c.Time == item.Time);

                    if (targets.Count() > 1) // found more than one
                    {
                        unmatchedCount++;
                    }
                    else
                    {
                        var target = targets.FirstOrDefault();
                        if (target == null) // add...
                        {
                            db.Convs.Add(item);
                            addedCount++;
                        }
                        else // update...
                        {
                            var entry = db.Entry(target);
                            if (entry.State == EntityState.Unchanged)
                            {
                                item.Id = target.Id; // so the target's Id won't get zeroed out

                                entry.State = EntityState.Detached;
                                AutoMapper.Mapper.Map(item, target);
                                entry.State = EntityState.Modified;
                                updatedCount++;
                            }
                            else
                            {
                                Logger.Warn("Encountered duplicate for {0:d} - Strategy {1} | TDad {2}", item.Time, item.StrategyId, item.TDadId);
                                duplicateCount++;
                            }
                        }
                    }
                    itemCount++;
                }
                Logger.Info("Processing {0} Conversions ({1} updates, {2} additions, {3} duplicates, {4} unmatched)",
                            itemCount, updatedCount, addedCount, duplicateCount, unmatchedCount);
                if (duplicateCount > 0)
                    Logger.Warn("Encountered {0} duplicates which were skipped", duplicateCount);
                int numChanges = db.SaveChanges();
            }
            return itemCount;
        }
    }
}
