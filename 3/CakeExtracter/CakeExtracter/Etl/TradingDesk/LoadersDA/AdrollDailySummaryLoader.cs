using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AdRoll.Entities;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.TD;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA
{
    public class AdrollDailySummaryLoader : Loader<AdrollDailySummary>
    {
        private readonly int accountId;

        public AdrollDailySummaryLoader(string advertisableEid)
        {
            using (var db = new DATDContext())
            {
                var account = db.Accounts.Where(a => a.ExternalId == advertisableEid && a.Platform.Code == Platform.Code_AdRoll)
                                            .FirstOrDefault();
                if (account != null)
                    accountId = account.Id;
                else
                    accountId = -1;
            }
        }

        public bool FoundAccount()
        {
            return (accountId > -1);
        }

        protected override int Load(List<AdrollDailySummary> items)
        {
            Logger.Info("Loading {0} AdRoll DailySummaries..", items.Count);
            var count = UpsertDailySummaries(items);
            return count;
        }

        private int UpsertDailySummaries(List<AdrollDailySummary> items)
        {
            var addedCount = 0;
            var updatedCount = 0;
            var duplicateCount = 0;
            var itemCount = 0;
            using (var db = new DATDContext())
            {
                foreach (var item in items)
                {
                    //TODO: Check/delete if all zeros

                    var source = new DailySummary
                    {
                        Date = item.date,
                        AccountId = accountId,
                        Impressions = item.impressions,
                        Clicks = item.clicks,
                        PostClickConv = item.click_through_conversions,
                        PostViewConv = item.view_through_conversions,
                        Cost = (decimal)item.cost_USD
                    };
                    var target = db.Set<DailySummary>().Find(item.date, accountId);
                    if (target == null)
                    {
                        db.DailySummaries.Add(source);
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
                Logger.Info("Saving {0} DailySummaries ({1} updates, {2} additions, {3} duplicates)", itemCount, updatedCount, addedCount, duplicateCount);
                int numChanges = db.SaveChanges();
            }
            return itemCount;
        }

    }
}
