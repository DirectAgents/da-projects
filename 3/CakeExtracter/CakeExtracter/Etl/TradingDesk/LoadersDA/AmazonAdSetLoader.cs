using Amazon.Entities;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA
{
    public class AmazonAdSetLoader : Loader<AmazonAdSet>
    {
        private readonly int accountId;
        private Dictionary<string, int> _strategyIdLookup = new Dictionary<string, int>(); // by StrategyEid and DB Id
        private Dictionary<string, int> adsetIdLookup = new Dictionary<string, int>(); // by StrategyEid + StrategyName + AdSetEid + AdSetName

        public AmazonAdSetLoader(int accountId = -1)
        {
            this.accountId = accountId;
        }

        protected override int Load(List<AmazonAdSet> items)
        {
            Logger.Info("Loading {0} Amazon AdSet data:", items.Count);
            LoadStrategy();

            AddUpdateDependentAdSets(items);
            //AssignAdSetIdToItems(items);
            //var count = UpsertDailySummaries(items);
            return items.Count;
        }
        private void LoadStrategy()
        {
            using (var db = new ClientPortalProgContext())
            {
                _strategyIdLookup = db.Strategies.ToDictionary(y => y.ExternalId, x => x.Id);
            }
        }
        //update strategies table if Adset returns a campaign id 
        public void AddUpdateDependentStrategies(List<AmazonAdSet> items)
        {
            //var strategyNameEids = items.GroupBy(i => new { i.StrategyName, i.StrategyEid })
            //    .Select(g => new NameEid { Name = g.Key.StrategyName, Eid = g.Key.StrategyEid })
            //    .Where(x => !string.IsNullOrWhiteSpace(x.Name) || !string.IsNullOrWhiteSpace(x.Eid));
            //TDStrategySummaryLoader.AddUpdateDependentStrategies(strategyNameEids, this.accountId, this.strategyIdLookup);
        }

        //public void AssignAdSetIdToItems(List<AmazonAdSet> items)
        //{
        //    foreach (var item in items)
        //    {
        //        string adsetKey = item.StrategyEid + item.StrategyName + item.AdSetEid + item.AdSetName;
        //        if (adsetIdLookup.ContainsKey(adsetKey))
        //        {
        //            item.AdSetId = adsetIdLookup[adsetKey];
        //        }
        //        otherwise it will get skipped; no AdSet to use for the foreign key
        //    }
        //}

        //public int UpsertDailySummaries(List<AmazonAdSet> items)
        //{
        //    var addedCount = 0;
        //    var updatedCount = 0;
        //    var duplicateCount = 0;
        //    var deletedCount = 0;
        //    var alreadyDeletedCount = 0;
        //    var skippedCount = 0;
        //    var itemCount = 0;
        //    using (var db = new ClientPortalProgContext())
        //    {
        //        var itemAdSetIds = items.Select(i => i.AdSetId).Distinct().ToArray();
        //        var adsetIdsInDb = db.AdSets.Select(s => s.Id).Where(i => itemAdSetIds.Contains(i)).ToArray();
        //        foreach (var item in items)
        //        {
        //            var target = db.Set<AmazonAdSet>().Find(item.Date, item.AdSetId);
        //            if (target == null)
        //            {
        //                if (item.AllZeros())
        //                {
        //                    alreadyDeletedCount++;
        //                }
        //                else
        //                {
        //                    if (adsetIdsInDb.Contains(item.AdSetId))
        //                    {
        //                        db.AdSetSummaries.Add(item);
        //                        addedCount++;
        //                    }
        //                    else
        //                    {
        //                        Logger.Warn("Skipping load of item. AdSet with id {0} does not exist.", item.AdSetId);
        //                        skippedCount++;
        //                    }
        //                }
        //            }
        //            else // AmazonAdSet already exists
        //            {
        //                var entry = db.Entry(target);
        //                if (entry.State == EntityState.Unchanged)
        //                {
        //                    if (!item.AllZeros())
        //                    {
        //                        entry.State = EntityState.Detached;
        //                        AutoMapper.Mapper.Map(item, target);
        //                        entry.State = EntityState.Modified;
        //                        updatedCount++;
        //                    }
        //                    else
        //                    {
        //                        entry.State = EntityState.Deleted;
        //                        deletedCount++;
        //                    }
        //                }
        //                else
        //                {
        //                    Logger.Warn("Encountered duplicate for {0:d} - AdSet {1}", item.Date, item.AdSetId);
        //                    duplicateCount++;
        //                }
        //            }
        //            itemCount++;
        //        }
        //        Logger.Info("Saving {0} AdSetSummaries ({1} updates, {2} additions, {3} duplicates, {4} deleted, {5} already-deleted, {6} skipped)",
        //                    itemCount, updatedCount, addedCount, duplicateCount, deletedCount, alreadyDeletedCount, skippedCount);
        //        if (duplicateCount > 0)
        //            Logger.Warn("Encountered {0} duplicates which were skipped", duplicateCount);
        //        int numChanges = db.SaveChanges();
        //    }
        //    return itemCount;
        //}


        public void AddUpdateDependentAdSets(List<AmazonAdSet> items)
        {
            using (var db = new ClientPortalProgContext())
            {
                // Find the unique adsets by grouping
                //var itemGroups = items.GroupBy(i => new { i.StrategyName, i.StrategyEid, i.AdSetName, i.AdSetEid });
                foreach (var amazonAdSetItem in items)
                {
                    string adsetKey = amazonAdSetItem.KeywordId + amazonAdSetItem.KeywordText;
                    if (adsetIdLookup.ContainsKey(adsetKey))
                        continue; // already encountered this adset

                    //var stratKey = group.Key.StrategyEid + group.Key.StrategyName;
                    //int? stratId = strategyIdLookup.ContainsKey(stratKey) ? strategyIdLookup[stratKey] : (int?)null;
                    int? stratId = _strategyIdLookup.ContainsKey(amazonAdSetItem.CampaignId) ? _strategyIdLookup[amazonAdSetItem.CampaignId] : (int?)null;

                    ////TODO: Check this logic for finding an existing adset...
                    ////      The main concern is when uploading stats from a csv and AdSetEids aren't included

                    // First see if an AdSet with that ExternalId exists
                    var adsetsInDb = db.AdSets.Where(x => x.AccountId == accountId && x.ExternalId == amazonAdSetItem.KeywordId);

                    #region Check if adset already exists
                    if (!adsetsInDb.Any())
                    {
                        //If not, check for a match by name where ExternalId == null
                        adsetsInDb = db.AdSets.Where(x => x.AccountId == accountId && x.ExternalId == null && x.Name == amazonAdSetItem.KeywordText);
                        if (stratId.HasValue)
                            adsetsInDb = adsetsInDb.Where(x => x.StrategyId == stratId.Value);
                    }
                    else
                    {
                        // Check by adset name
                        adsetsInDb = db.AdSets.Where(x => x.AccountId == accountId && x.Name == amazonAdSetItem.KeywordText);
                        if (stratId.HasValue)
                            adsetsInDb = adsetsInDb.Where(x => x.StrategyId == stratId.Value);
                    }

                    adsetsInDb = db.AdSets.Where(x => x.AccountId == accountId && x.ExternalId == amazonAdSetItem.KeywordId && x.Name == amazonAdSetItem.KeywordText);
                    #endregion

                    var adsetsInDbList = adsetsInDb.ToList();
                    if (!adsetsInDbList.Any())
                    {   // AdSet doesn't exist in the db; so create it and put an entry in the lookup
                        #region add adset to db
                        var adset = new AdSet
                        {
                            AccountId = this.accountId,
                            StrategyId = stratId,
                            ExternalId = amazonAdSetItem.KeywordId,
                            Name = amazonAdSetItem.KeywordText
                        };
                        db.AdSets.Add(adset);
                        db.SaveChanges();
                        Logger.Info("Saved new AdSet: {0} ({1}), ExternalId={2}", adset.Name, adset.Id, adset.ExternalId);
                        adsetIdLookup[adsetKey] = adset.Id; 
                        #endregion
                    }
                    else
                    {
                        #region update adset in db
                        // Update & put existing AdSet in the lookup
                        // There should only be one matching AdSet in the db, but just in case...
                        foreach (var adset in adsetsInDbList)
                        {
                            if (!string.IsNullOrWhiteSpace(amazonAdSetItem.KeywordId))
                                adset.ExternalId = amazonAdSetItem.KeywordId;
                            if (!string.IsNullOrWhiteSpace(amazonAdSetItem.KeywordText))
                                adset.Name = amazonAdSetItem.KeywordText;
                            if (stratId.HasValue)
                                adset.StrategyId = stratId.Value;
                            // other properties...
                        }
                        int numUpdates = db.SaveChanges();
                        if (numUpdates > 0)
                        {
                            Logger.Info("Updated AdSet: {0}, Eid={1}", amazonAdSetItem.KeywordText, amazonAdSetItem.KeywordId);
                            if (numUpdates > 1)
                                Logger.Warn("Multiple entities in db ({0})", numUpdates);
                        }
                        adsetIdLookup[adsetKey] = adsetsInDbList.First().Id; 
                        #endregion
                    }
                }
            }
        }

    }
}
