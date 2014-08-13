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
                            AutoMapper.Mapper.Map(source, target);
                            db.Entry(target).State = EntityState.Modified;
                            updatedCount++;
                        }
                        itemCount++;
                    }
                }
                Logger.Info("Saving {0} AdDailySummaries ({1} updates, {2} additions)", itemCount, updatedCount, addedCount);
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
