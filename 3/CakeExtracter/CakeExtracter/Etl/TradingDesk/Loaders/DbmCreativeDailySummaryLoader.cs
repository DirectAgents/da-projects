using CakeExtracter.Etl.TradingDesk.Extracters;
using ClientPortal.Data.Entities.TD;
using ClientPortal.Data.Entities.TD.DBM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CakeExtracter.Etl.TradingDesk.Loaders
{
    public class DbmCreativeDailySummaryLoader : Loader<DbmRowBase>
    {

        protected override int Load(List<DbmRowBase> items)
        {
            Logger.Info("Loading {0} CreativeDailySummaries..", items.Count);
            DbmDailySummaryLoader.AddDependentInsertionOrders(items);
            AddDependentCreatives(items);
            var count = UpsertCreativeDailySummaries(items);
            return count;
        }

        private int UpsertCreativeDailySummaries(List<DbmRowBase> items)
        {
            var addedCount = 0;
            var updatedCount = 0;
            var itemCount = 0;
            using (var db = new TDContext())
            {
                foreach (var item in items)
                {
                    DateTime date = DateTime.Parse(item.Date);
                    int creativeID = int.Parse(((DbmRowWithCreative)item).CreativeID);
                    var source = new CreativeDailySummary
                    {
                        Date = date,
                        CreativeID = creativeID,
                        Impressions = int.Parse(item.Impressions),
                        Clicks = int.Parse(item.Clicks),
                        Conversions = (int)decimal.Parse(item.TotalConversions),
                        Revenue = decimal.Parse(item.Revenue)
                    };
                    var target = db.Set<CreativeDailySummary>().Find(date, creativeID);
                    if (target == null)
                    {
                        db.CreativeDailySummaries.Add(source);
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
                Logger.Info("Saving {0} CreativeDailySummaries ({1} updates, {2} additions)", itemCount, updatedCount, addedCount);
                db.SaveChanges();
            }
            return itemCount;
        }

        private void AddDependentCreatives(List<DbmRowBase> items)
        {
            using (var db = new TDContext())
            {
                var tuples = items.Select(i => Tuple.Create(((DbmRowWithCreative)i).CreativeID, ((DbmRowWithCreative)i).Creative, i.InsertionOrderID)).Distinct();
                foreach (var tuple in tuples)
                {
                    int creativeID, insertionOrderID;
                    if (int.TryParse(tuple.Item1, out creativeID) && int.TryParse(tuple.Item3, out insertionOrderID) && !db.Creatives.Any(c => c.CreativeID == creativeID))
                    {
                        var creative = new Creative
                        {
                            CreativeID = creativeID,
                            CreativeName = tuple.Item2,
                            InsertionOrderID = insertionOrderID
                        };
                        db.Creatives.Add(creative);
                        Logger.Info("Saving new Creative: {0} ({1})", creative.CreativeName, creative.CreativeID);
                        db.SaveChanges();
                    }
                }
            }
        }
    }
}
