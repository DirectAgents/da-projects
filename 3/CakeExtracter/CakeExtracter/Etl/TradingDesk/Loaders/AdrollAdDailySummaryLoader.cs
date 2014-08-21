using CakeExtracter.Etl.TradingDesk.Extracters;
using ClientPortal.Data.Entities.TD;
using ClientPortal.Data.Entities.TD.AdRoll;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace CakeExtracter.Etl.TradingDesk.Loaders
{
    public class AdrollAdDailySummaryLoader : Loader<AdrollRow>
    {
        private readonly int adrollProfileId;
        private Dictionary<string, int> adIdLookupByName = new Dictionary<string, int>();

        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
        public Dictionary<int, string> AdsAffected = new Dictionary<int, string>();

        public AdrollAdDailySummaryLoader(int adrollProfileId)
        {
            this.adrollProfileId = adrollProfileId;
        }

        protected override int Load(List<AdrollRow> items)
        {
            Logger.Info("Loading {0} AdrollAdDailySummaries..", items.Count);
            AddDependentAds(items);
            var count = UpsertAdDailySummaries(items);
            return count;
        }

        private int UpsertAdDailySummaries(List<AdrollRow> items)
        {
            var addedCount = 0;
            var updatedCount = 0;
            var skippedCount = 0;
            var itemCount = 0;
            using (var db = new TDContext())
            {
                foreach (var item in items)
                {
                    int? adId = null;
                    if (adIdLookupByName.ContainsKey(item.AdName))
                    {
                        DateTime date = DateTime.Parse(item.Date);
                        adId = adIdLookupByName[item.AdName];
                        var source = new AdDailySummary
                        {
                            Date = date,
                            AdRollAdId = adIdLookupByName[item.AdName],
                            Impressions = int.Parse(item.Impressions, NumberStyles.Number),
                            Clicks = int.Parse(item.Clicks, NumberStyles.Number),
                            Conversions = int.Parse(item.TotalConversions, NumberStyles.Number),
                            Spend = decimal.Parse(item.Spend, NumberStyles.Currency)
                        };
                        var target = db.Set<AdDailySummary>().Find(date, adId);
                        if (target == null)
                        {
                            db.AdDailySummaries.Add(source);
                            addedCount++;
                        }
                        else
                        {
                            var entry = db.Entry(target);
                            if (entry.State == EntityState.Unchanged)
                            {
                                AutoMapper.Mapper.Map(source, target);
                                entry.State = EntityState.Modified;
                                updatedCount++;
                            }
                            else
                            {
                                skippedCount++;
                            }
                            // Note: In the case that the csv has two identical rows and it's a new entry, we now avoid problems on the second row,
                            //       because Find will return the same "target". We don't want to change state from Added to Modified in that case.
                        }
                        itemCount++;

                        if (!MinDate.HasValue || date < MinDate.Value)
                            MinDate = date;
                        if (!MaxDate.HasValue || date > MaxDate.Value)
                            MaxDate = date;
                        if (!AdsAffected.ContainsKey(source.AdRollAdId))
                            AdsAffected[source.AdRollAdId] = item.AdName;
                    }
                }
                Logger.Info("Saving {0} AdDailySummaries ({1} updates, {2} additions, {3} duplicates)", itemCount, updatedCount, addedCount, skippedCount);
                db.SaveChanges();
            }
            return itemCount;
        }

        private void AddDependentAds(List<AdrollRow> items)
        {
            using (var db = new TDContext())
            {
                // Find the unique AdNames by grouping
                var itemGroups = items.GroupBy(i => i.AdName);
                foreach (var group in itemGroups)
                {   // See if an AdRollAd with that name exists
                    var ads = db.AdRollAds.Where(a => a.Name == group.Key);
                    if (ads.Count() == 0)
                    {   // Create new AdRollAd
                        var row = group.First(); // assume all other ad properties are the same for each group
                        var ad = new AdRollAd
                        {
                            AdRollProfileId = adrollProfileId,
                            Name = row.AdName,
                            Size = row.Size,
                            Type = row.Type,
                        };
                        DateTime createdDate;
                        if (DateTime.TryParse(row.CreateDate, out createdDate))
                            ad.CreatedDate = createdDate;
                        db.AdRollAds.Add(ad);
                        db.SaveChanges();
                        Logger.Info("Saving new AdRollAd: {0} ({1})", ad.Name, ad.Id);
                        adIdLookupByName[ad.Name] = ad.Id;
                    }
                    else
                    {   // Put existing Ad id in the lookup
                        adIdLookupByName[group.Key] = ads.First().Id;
                    }
                }
            }
        }

    }
}
