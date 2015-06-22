using System.Collections.Generic;
using System.Data.Entity;
using AdRoll.Entities;
using DirectAgents.Domain.Entities.AdRoll;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA
{
    public class AdrollAdvertisableStatsLoader : Loader<DailySummary>
    {
        private readonly int advertisableId;

        public AdrollAdvertisableStatsLoader(int advertisableId)
        {
            this.advertisableId = advertisableId;
        }

        protected override int Load(List<DailySummary> items)
        {
            Logger.Info("Loading {0} Advertisable DailySummaries..", items.Count);
            var count = UpsertDailySummaries(items);
            return count;
        }

        private int UpsertDailySummaries(List<DailySummary> items)
        {
            var addedCount = 0;
            var updatedCount = 0;
            var duplicateCount = 0;
            var itemCount = 0;
            using (var db = new DirectAgents.Domain.Contexts.DAContext())
            {
                foreach (var item in items)
                {
                    var source = new AdvertisableStat
                    {
                        Date = item.date,
                        AdvertisableId = advertisableId,
                        Impressions = item.impressions,
                        Clicks = item.clicks,
                        CTC = item.click_through_conversions,
                        VTC = item.view_through_conversions,
                        Cost = (decimal)item.cost_USD,
                        Prospects = item.prospects
                    };
                    var target = db.Set<AdvertisableStat>().Find(item.date, advertisableId);
                    if (target == null)
                    {
                        db.AdvertisableStats.Add(source);
                        addedCount++;
                    }
                    else
                    {
                        var entry = db.Entry(target);
                        if (entry.State == EntityState.Unchanged)
                        {
                            entry.State = EntityState.Detached;
                            AutoMapper.Mapper.Map(source, target);
                            entry.State = EntityState.Modified;
                            updatedCount++;
                        }
                        else
                        {
                            duplicateCount++;
                        }
                    }
                    itemCount++;
                }
                Logger.Info("Saving {0} AdvertisableStats ({1} updates, {2} additions, {3} duplicates)", itemCount, updatedCount, addedCount, duplicateCount);
                int numChanges = db.SaveChanges();
            }
            return itemCount;
        }

    }
}
