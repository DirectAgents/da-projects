using AutoMapper;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CakeExtracter.Common;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA
{
    public class TDAdSetSummaryLoader : Loader<AdSetSummary>
    {
        public static EntityIdStorage<AdSet> DefaultAdSetStorage = StorageCollection.AdSetWithEidStorage;

        public readonly int AccountId;

        private readonly EntityIdStorage<AdSet> adSetStorage;
        private readonly EntityIdStorage<Strategy> strategyStorage;
        private readonly TDStrategySummaryLoader strategyLoader;
        private readonly SummaryMetricLoader metricLoader;

        public TDAdSetSummaryLoader(int accountId = -1, EntityIdStorage<AdSet> adSetStorage = null, EntityIdStorage<Strategy> strategyStorage = null)
        {
            this.strategyStorage = strategyStorage ?? TDStrategySummaryLoader.DefaultStrategyStorage;
            this.adSetStorage = adSetStorage ?? DefaultAdSetStorage;
            AccountId = accountId;
            strategyLoader = new TDStrategySummaryLoader(accountId, this.strategyStorage);
            metricLoader = new SummaryMetricLoader();
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
                if (!adSetStorage.IsEntityInStorage(item.AdSet))
                {
                    continue;
                }

                item.AdSetId = adSetStorage.GetEntityIdFromStorage(item.AdSet);
                item.AdSet = null;
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

                SafeContextWrapper.TrySaveChanges(db);
            }

            Logger.Info(AccountId, "Saving {0} AdSetSummaries ({1} updates, {2} additions, {3} duplicates, {4} deleted, {5} already-deleted, {6} skipped)",
                progress.ItemCount, progress.UpdatedCount, progress.AddedCount, progress.DuplicateCount, progress.DeletedCount, progress.AlreadyDeletedCount, progress.SkippedCount);
            if (progress.DuplicateCount > 0)
            {
                Logger.Warn(AccountId, "Encountered {0} duplicates which were skipped", progress.DuplicateCount);
            }
            return progress.ItemCount;
        }

        public void AddUpdateDependentStrategies(List<AdSetSummary> items)
        {
            var strategies = items.Select(x => x.AdSet.Strategy).ToList();
            strategyLoader.AddUpdateDependentStrategies(strategies);
            AssignStrategyIdToItems(items);
        }

        public void AddUpdateDependentAdSets(IEnumerable<AdSetSummary> items)
        {
            var adSets = items.Select(x => x.AdSet).ToList();
            AddUpdateDependentAdSets(adSets);
        }

        public void AddUpdateDependentAdSets(IEnumerable<AdSet> items)
        {
            var notNullableItems = items.Where(x => x != null).ToList();
            notNullableItems.ForEach(x => x.AccountId = AccountId);
            var adSets = notNullableItems
                .Where(x => !string.IsNullOrWhiteSpace(x.Name) || !string.IsNullOrWhiteSpace(x.ExternalId))
                .GroupBy(x => new { x.AccountId, x.StrategyId, x.Name, x.ExternalId })
                .Select(x => x.First())
                .ToList();
            AddUpdateDependentAdSetsInDb(adSets);
        }

        private void AddUpdateDependentAdSetsInDb(IEnumerable<AdSet> items)
        {
            using (var db = new ClientPortalProgContext())
            {
                foreach (var adSet in items)
                {
                    if (adSetStorage.IsEntityInStorage(adSet))
                    {
                        continue; // already encountered this adset
                    }

                    SafeContextWrapper.Lock(SafeContextWrapper.AdSetLocker, () =>
                    {
                        var adSetsInDbList = GetAdSets(db, adSet);
                        if (!adSetsInDbList.Any())
                        {
                            // AdSet doesn't exist in the db; so create it and put an entry in the lookup
                            var adSetInDB = AddAdSet(db, adSet);
                            Logger.Info(accountId, "Saved new AdSet: {0} ({1}), ExternalId={2}", adSetInDB.Name,
                                adSetInDB.Id, adSetInDB.ExternalId);
                            adSetStorage.AddEntityIdToStorage(adSetInDB);
                        }
                        else
                        {
                            // Update & put existing AdSet in the lookup
                            // There should only be one matching AdSet in the db, but just in case...
                            var numUpdates = UpdateAdSets(db, adSetsInDbList, adSet);
                            if (numUpdates > 0)
                            {
                                Logger.Info(accountId, "Updated AdSet: {0}, Eid={1}", adSet.Name, adSet.ExternalId);
                                if (numUpdates > 1)
                                {
                                    Logger.Warn(accountId, "Multiple entities in db ({0})", numUpdates);
                                }
                            }
                            adSetStorage.AddEntityIdToStorage(adSetsInDbList.First());
                        }
                    });
                }
            }
        }

        private void AssignStrategyIdToItems(IEnumerable<AdSetSummary> items)
        {
            foreach (var item in items)
            {
                if (strategyStorage.IsEntityInStorage(item.AdSet.Strategy))
                {
                    item.AdSet.StrategyId = strategyStorage.GetEntityIdFromStorage(item.AdSet.Strategy);
                }
                item.AdSet.Strategy = null;
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
            RemoveOldSummaryMetrics(db, item, target);
            progress.UpdatedCount++;
        }

        private void UpsertSummaryMetrics(ClientPortalProgContext db, AdSetSummary item)
        {
            item.InitialMetrics = GetItemMetrics(item);
            if (item.InitialMetrics == null || !item.InitialMetrics.Any())
            {
                return;
            }
            item.InitialMetrics.ForEach(x => x.EntityId = item.AdSetId);
            metricLoader.AddDependentMetricTypes(item.InitialMetrics);
            metricLoader.AssignMetricTypeIdToItems(item.InitialMetrics);
            metricLoader.UpsertSummaryMetrics<AdSetSummaryMetric>(db, item.InitialMetrics);
        }

        private IEnumerable<SummaryMetric> GetItemMetrics(AdSetSummary item)
        {
            var metrics = item.InitialMetrics == null
                ? item.Metrics
                : item.Metrics == null
                    ? item.InitialMetrics
                    : item.InitialMetrics.Concat(item.Metrics);
            return metrics?.ToList();
        }

        private void RemoveOldSummaryMetrics(ClientPortalProgContext db, AdSetSummary item, AdSetSummary target)
        {
            var deletedMetrics = item.InitialMetrics == null
                ? target.Metrics
                : target.Metrics.Where(x => item.InitialMetrics.All(m => m.MetricTypeId != x.MetricTypeId));
            metricLoader.RemoveMetrics(db, deletedMetrics);
        }

        private List<AdSet> GetAdSets(ClientPortalProgContext db, AdSet adSet)
        {
            //TODO: Check this logic for finding an existing adset...
            //      The main concern is when uploading stats from a csv and AdSetEids aren't included

            IEnumerable<AdSet> adSetsInDb;
            if (!string.IsNullOrWhiteSpace(adSet.ExternalId))
            {
                // First see if an AdSet with that ExternalId exists
                adSetsInDb = db.AdSets.Where(x => x.AccountId == adSet.AccountId && x.ExternalId == adSet.ExternalId);

                // If not, check for a match by name where ExternalId == null
                if (!adSetsInDb.Any())
                {
                    adSetsInDb = db.AdSets.Where(x => x.AccountId == adSet.AccountId && x.ExternalId == null && x.Name == adSet.Name);
                    if (adSet.StrategyId.HasValue)
                    {
                        adSetsInDb = adSetsInDb.Where(x => x.StrategyId == adSet.StrategyId);
                    }
                }
            }
            else
            {
                // Check by adset name
                adSetsInDb = db.AdSets.Where(x => x.AccountId == adSet.AccountId && x.Name == adSet.Name);
                if (adSet.StrategyId.HasValue)
                {
                    adSetsInDb = adSetsInDb.Where(x => x.StrategyId == adSet.StrategyId);
                }
            }

            return adSetsInDb.ToList();
        }

        private AdSet AddAdSet(ClientPortalProgContext db, AdSet adSetProps)
        {
            var adSet = new AdSet
            {
                AccountId = adSetProps.AccountId,
                StrategyId = adSetProps.StrategyId,
                ExternalId = adSetProps.ExternalId,
                Name = adSetProps.Name
            };
            db.AdSets.Add(adSet);
            SafeContextWrapper.TrySaveChanges(db);
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
                if (adSetProps.StrategyId.HasValue)
                {
                    adSet.StrategyId = adSetProps.StrategyId;
                }
            }
            return SafeContextWrapper.TrySaveChanges(db);
        }
    }
}
