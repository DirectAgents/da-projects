using CakeExtracter.Etl.TradingDesk.Extracters;
using ClientPortal.Data.Entities.TD;
using ClientPortal.Data.Entities.TD.DBM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CakeExtracter.Etl.TradingDesk.Loaders
{
    public class DbmDailySummaryLoader : Loader<DbmRowBase>
    {

        protected override int Load(List<DbmRowBase> items)
        {
            Logger.Info("Loading {0} DailySummaries..", items.Count);
            AddDependentInsertionOrders(items);
            var count = UpsertDailySummaries(items);
            return count;
        }

        private int UpsertDailySummaries(List<DbmRowBase> items)
        {
            var addedCount = 0;
            var updatedCount = 0;
            var itemCount = 0;
            using (var db = new TDContext())
            {
                foreach(var item in items)
                {
                    DateTime date = DateTime.Parse(item.Date);
                    int ioID = int.Parse(item.InsertionOrderID);
                    var source = new DailySummary
                    {
                        Date = date,
                        InsertionOrderID = ioID,
                        AdvertiserCurrency = item.AdvertiserCurrency,
                        Impressions = int.Parse(item.Impressions),
                        Clicks = int.Parse(item.Clicks),
                        Conversions = (int)decimal.Parse(item.TotalConversions),
                        Revenue = decimal.Parse(item.Revenue)
                    };
                    var target = db.Set<DailySummary>().Find(date, ioID);
                    if (target == null)
                    {
                        db.DailySummaries.Add(source);
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
                Logger.Info("Saving {0} DailySummaries ({1} updates, {2} additions)", itemCount, updatedCount, addedCount);
                db.SaveChanges();
            }
            return itemCount;
        }

        public static void AddDependentInsertionOrders(List<DbmRowBase> items)
        {
            using (var db = new TDContext())
            {
                var ioTuples = items.Select(i => Tuple.Create(i.InsertionOrderID, i.InsertionOrder)).Distinct();
                foreach (var ioTuple in ioTuples)
                {
                    int insertionOrderID;
                    if (Int32.TryParse(ioTuple.Item1, out insertionOrderID) && !db.InsertionOrders.Any(io => io.InsertionOrderID == insertionOrderID))
                    {
                        var io = new InsertionOrder
                        {
                            InsertionOrderID = insertionOrderID,
                            InsertionOrderName = ioTuple.Item2
                        };
                        db.InsertionOrders.Add(io);
                        Logger.Info("Saving new InsertionOrder: {0} ({1})", io.InsertionOrderName, io.InsertionOrderID);
                        db.SaveChanges();
                    }
                }
            }
        }
    }
}
