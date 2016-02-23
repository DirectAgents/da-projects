using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Etl.TradingDesk.Extracters;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.TD;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA
{
    public class DbmCreativeSummaryLoader : Loader<DbmRowBase>
    {
        private TDadSummaryLoader tdAdSummaryLoader;
        private Dictionary<int, int> accountIdLookupByIOid = new Dictionary<int, int>();
        private Dictionary<string, int> tdAdIdLookupByCreativeId_IOid = new Dictionary<string, int>();

        public DbmCreativeSummaryLoader()
        {
            this.tdAdSummaryLoader = new TDadSummaryLoader();
        }

        protected override int Load(List<DbmRowBase> items)
        {
            Logger.Info("Loading {0} Creative DailySummaries..", items.Count);
            AddUpdateDependentAccounts(items);
            AddUpdateDependentTDads(items);
            var sItems = items.Select(i => CreateTDadSummary(i, tdAdIdLookupByCreativeId_IOid[((DbmRowWithCreative)i).CreativeID + "_" + i.InsertionOrderID])).ToList();
            var count = tdAdSummaryLoader.UpsertDailySummaries(sItems);
            return count;
        }

        public static TDadSummary CreateTDadSummary(DbmRowBase item, int tdAdId)
        {
            var sum = new TDadSummary
            {
                TDadId = tdAdId,
                Date = item.Date,
                Impressions = int.Parse(item.Impressions),
                Clicks = int.Parse(item.Clicks),
                PostClickConv = (int)decimal.Parse(item.PostClickConversions),
                PostViewConv = (int)decimal.Parse(item.PostViewConversions),
                Cost = decimal.Parse(item.Revenue)
            };
            return sum;
        }

        public void AddUpdateDependentAccounts(List<DbmRowBase> items)
        {
            var ioTuples = items.Select(i => Tuple.Create(i.InsertionOrderID, i.InsertionOrder)).Distinct();
            DbmDailySummaryLoader.AddUpdateAccounts(ioTuples, accountIdLookupByIOid);
        }

        private void AddUpdateDependentTDads(List<DbmRowBase> items)
        {
            var tuples = items.Select(i => Tuple.Create(((DbmRowWithCreative)i).CreativeID, ((DbmRowWithCreative)i).Creative, i.InsertionOrderID)).Distinct();

            using (var db = new DATDContext())
            {
                foreach (var tuple in tuples)
                {
                    string creativeID = tuple.Item1;
                    string creativeName = tuple.Item2;
                    int ioID = tuple.Item3;

                    if (tdAdIdLookupByCreativeId_IOid.ContainsKey(creativeID + "_" + ioID))
                        continue; // already encountered this creative
                    if (!accountIdLookupByIOid.ContainsKey(ioID))
                        continue; // should have been added in AddUpdateDependentAccounts()
                    int accountId = accountIdLookupByIOid[ioID];

                    var tdAdsInDb = db.TDads.Where(a => a.AccountId == accountId && a.ExternalId == creativeID);
                    if (!tdAdsInDb.Any())
                    {   // TDad doesn't exist in the db; so create it and put an entry in the lookup
                        var tdAd = new TDad
                        {
                            AccountId = accountId,
                            ExternalId = creativeID,
                            Name = creativeName
                            // other properties...
                        };
                        db.TDads.Add(tdAd);
                        db.SaveChanges();
                        Logger.Info("Saved new TDad: {0} ({1}), ExternalId={2}", tdAd.Name, tdAd.Id, tdAd.ExternalId);
                        tdAdIdLookupByCreativeId_IOid[creativeID + "_" + ioID] = tdAd.Id;
                    }
                    else
                    {   // Update & put existing TDad in the lookup
                        // There should only be one matching TDad in the db, but just in case...
                        foreach (var tdAd in tdAdsInDb)
                        {
                            if (!string.IsNullOrWhiteSpace(creativeName))
                                tdAd.Name = creativeName;
                            // other properties...
                        }
                        int numUpdates = db.SaveChanges();
                        if (numUpdates > 0)
                        {
                            Logger.Info("Updated TDad: {0}, Eid={1}", creativeName, creativeID);
                            if (numUpdates > 1)
                                Logger.Warn("Multiple entities in db ({0})", numUpdates);
                        }
                        tdAdIdLookupByCreativeId_IOid[creativeID + "_" + ioID] = tdAdsInDb.First().Id;
                    }
                }
            }
        }

    }
}
