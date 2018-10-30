using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using CakeExtracter.Etl.TradingDesk.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA
{
    public class TDAdSetSummaryLoader : Loader<AdSetSummary>
    {
        public readonly int AccountId;
        public static EntityIdStorage<AdSet> AdSetStorage;

        private readonly TDStrategySummaryLoader strategyLoader;
        private readonly EntityIdStorage<Strategy> strategyStorage;
        private readonly SummaryMetricLoader metricLoader;

        static TDAdSetSummaryLoader()
        {
            AdSetStorage = new EntityIdStorage<AdSet>(x => x.Id, x => $"{x.StrategyId}{x.Name}{x.ExternalId}");
        }

        public TDAdSetSummaryLoader(int accountId = -1)
        {
            this.AccountId = accountId;
            this.strategyLoader = new TDStrategySummaryLoader(accountId);
            this.strategyStorage = TDStrategySummaryLoader.StrategyStorage;
            this.metricLoader = new SummaryMetricLoader();
        }

        protected override int Load(List<AdSetSummary> items)
        {
            Logger.Info(AccountId, "Loading {0} DA-TD AdSetSummaries..", items.Count);
            PrepareData(items);
            AddUpdateDependentStrategies(items);
            AddUpdateDependentAdSets(items);
            AssignAdSetIdToItems(items);
            var count = UpsertDailySummaries(items);
            return count;
        }

        public void PrepareData(List<AdSetSummary> items)
        {
            items.ForEach(x =>
            {
                if (x.AdSet == null)
                {
                    x.AdSet = new AdSet();
                }
                Mapper.Map(x, x.AdSet);
            });
        }

        public void AssignAdSetIdToItems(List<AdSetSummary> items)
        {
            foreach (var item in items)
            {
                if (AdSetStorage.IsEntityInStorage(item.AdSet))
                {
                    item.AdSetId = AdSetStorage.GetEntityIdFromStorage(item.AdSet);
                    item.AdSet = null;
                }
                // otherwise it will get skipped; no AdSet to use for the foreign key
            }
        }

        public int UpsertDailySummaries(List<AdSetSummary> items)
        {
            var progress = new LoadingProgress();
            using (var db = new ClientPortalProgContext())
            {
                var itemAdSetIds = items.Select(i => i.AdSetId).Distinct().ToArray();
                var adSetIdsInDb = db.AdSets.Select(s => s.Id).Where(i => itemAdSetIds.Contains(i)).ToArray();

                foreach (var item in items)
                {
                    var target = db.Set<AdSetSummary>().Find(item.Date, item.AdSetId);
                    if (target == null)
                    {
                        TryToAddSummary(db, item, adSetIdsInDb, progress);
                    }
                    else // AdSetSummary already exists
                    {
                        TryToUpdateSummary(db, item, target, progress);
                    }
                    progress.ItemCount++;
                }
                Logger.Info(AccountId, "Saving {0} AdSetSummaries ({1} updates, {2} additions, {3} duplicates, {4} deleted, {5} already-deleted, {6} skipped)",
                            progress.ItemCount, progress.UpdatedCount, progress.AddedCount, progress.DuplicateCount, progress.DeletedCount, progress.AlreadyDeletedCount, progress.SkippedCount);
                if (progress.DuplicateCount > 0)
                {
                    Logger.Warn(AccountId, "Encountered {0} duplicates which were skipped", progress.DuplicateCount);
                }
                var numChanges = db.SaveChanges();
            }
            return progress.ItemCount;
        }

        public void AddUpdateDependentStrategies(List<AdSetSummary> items)
        {
            var strategies = items
                .Select(x => x.AdSet.Strategy)
                .Where(x => !string.IsNullOrWhiteSpace(x.Name) || !string.IsNullOrWhiteSpace(x.ExternalId))
                .ToList();
            strategyLoader.AddUpdateDependentStrategies(strategies);
            AssignStrategyIdToItems(items);
        }

        public void AddUpdateDependentAdSets(List<AdSetSummary> items)
        {
            using (var db = new ClientPortalProgContext())
            {
                // Find the unique adsets by grouping
                var itemGroups = items.GroupBy(i => new { i.AdSet.StrategyId, i.AdSet.Name, i.AdSet.ExternalId });
                foreach (var group in itemGroups)
                {
                    var adSet = group.First().AdSet;
                    if (AdSetStorage.IsEntityInStorage(adSet))
                    {
                        continue; // already encountered this adset
                    }

                    var adSetsInDbList = GetAdSets(db, adSet, AccountId);
                    if (!adSetsInDbList.Any())
                    {   // AdSet doesn't exist in the db; so create it and put an entry in the lookup
                        var adSetInDB = AddAdSet(db, adSet, AccountId);
                        Logger.Info(AccountId, "Saved new AdSet: {0} ({1}), ExternalId={2}", adSetInDB.Name, adSetInDB.Id, adSetInDB.ExternalId);
                        AdSetStorage.AddEntityIdToStorage(adSetInDB);
                    }
                    else
                    {   // Update & put existing AdSet in the lookup
                        // There should only be one matching AdSet in the db, but just in case...
                        var numUpdates = UpdateAdSets(db, adSetsInDbList, adSet);
                        if (numUpdates > 0)
                        {
                            Logger.Info(AccountId, "Updated AdSet: {0}, Eid={1}", adSet.Name, adSet.ExternalId);
                            if (numUpdates > 1)
                            {
                                Logger.Warn(AccountId, "Multiple entities in db ({0})", numUpdates);
                            }
                        }
                        AdSetStorage.AddEntityIdToStorage(adSetsInDbList.First());
                    }
                }
            }
        }

        private void AssignStrategyIdToItems(List<AdSetSummary> items)
        {
            foreach (var item in items)
            {
                if (strategyStorage.IsEntityInStorage(item.AdSet.Strategy))
                {
                    item.AdSet.StrategyId = strategyStorage.GetEntityIdFromStorage(item.AdSet.Strategy);
                    item.AdSet.Strategy = null;
                }
            }
        }

        private void TryToAddSummary(ClientPortalProgContext db, AdSetSummary item, IEnumerable<int> adSetIdsInDb, LoadingProgress progress)
        {
            if (item.AllZeros())
            {
                progress.AlreadyDeletedCount++;
                return;
            }
            if (!adSetIdsInDb.Contains(item.AdSetId))
            {
                Logger.Warn(AccountId, "Skipping load of item. AdSet with id {0} does not exist.", item.AdSetId);
                progress.SkippedCount++;
                return;
            }
            db.AdSetSummaries.Add(item);
            UpsertSummaryMetrics(db, item);
            progress.AddedCount++;
        }

        private void TryToUpdateSummary(ClientPortalProgContext db, AdSetSummary item, AdSetSummary target, LoadingProgress progress)
        {
            var entry = db.Entry(target);
            if (entry.State != EntityState.Unchanged)
            {
                Logger.Warn(AccountId, "Encountered duplicate for {0:d} - AdSet {1}", item.Date, item.AdSetId);
                progress.DuplicateCount++;
                return;
            }

            if (item.AllZeros())
            {
                entry.State = EntityState.Deleted;
                progress.DeletedCount++;
                return;
            }
            entry.State = EntityState.Detached;
            Mapper.Map(item, target);
            entry.State = EntityState.Modified;
            UpsertSummaryMetrics(db, item);
            progress.UpdatedCount++;
        }

        private void UpsertSummaryMetrics(ClientPortalProgContext db, AdSetSummary item)
        {
            item.Metrics.ForEach(x => x.EntityId = item.AdSetId);
            metricLoader.UpsertSummaryMetrics<AdSetSummaryMetric>(db, item.Metrics);
        }

        private List<AdSet> GetAdSets(ClientPortalProgContext db, AdSet adSet, int accountId)
        {
            //TODO: Check this logic for finding an existing adset...
            //      The main concern is when uploading stats from a csv and AdSetEids aren't included

            IEnumerable<AdSet> adSetsInDb;
            if (!string.IsNullOrWhiteSpace(adSet.ExternalId))
            {
                // First see if an AdSet with that ExternalId exists
                adSetsInDb = db.AdSets.Where(x => x.AccountId == accountId && x.ExternalId == adSet.ExternalId);

                // If not, check for a match by name where ExternalId == null
                if (!adSetsInDb.Any())
                {
                    adSetsInDb = db.AdSets.Where(x => x.AccountId == accountId && x.ExternalId == null && x.Name == adSet.Name);
                    if (adSet.StrategyId > 0)
                    {
                        adSetsInDb = adSetsInDb.Where(x => x.StrategyId == adSet.StrategyId);
                    }
                }
            }
            else
            {
                // Check by adset name
                adSetsInDb = db.AdSets.Where(x => x.AccountId == accountId && x.Name == adSet.Name);
                if (adSet.StrategyId > 0)
                {
                    adSetsInDb = adSetsInDb.Where(x => x.StrategyId == adSet.StrategyId);
                }
            }

            return adSetsInDb.ToList();
        }

        private AdSet AddAdSet(ClientPortalProgContext db, AdSet adSetProps, int accountId)
        {
            var adSet = new AdSet
            {
                AccountId = accountId,
                StrategyId = adSetProps.StrategyId,
                ExternalId = adSetProps.ExternalId,
                Name = adSetProps.Name
            };
            db.AdSets.Add(adSet);
            db.SaveChanges();
            return adSet;
        }

        private int UpdateAdSets(ClientPortalProgContext db, IEnumerable<AdSet> adSetsInDbList, AdSet adSetProps)
        {
            foreach (var adSet in adSetsInDbList)
            {
                if (!string.IsNullOrWhiteSpace(adSetProps.ExternalId))
                {
                    adSet.ExternalId = adSetProps.ExternalId;
                }
                if (!string.IsNullOrWhiteSpace(adSetProps.Name))
                {
                    adSet.Name = adSetProps.Name;
                }
                if (adSetProps.StrategyId > 0)
                {
                    adSet.StrategyId = adSetProps.StrategyId;
                }
            }
            return db.SaveChanges();
        }
    }
}
