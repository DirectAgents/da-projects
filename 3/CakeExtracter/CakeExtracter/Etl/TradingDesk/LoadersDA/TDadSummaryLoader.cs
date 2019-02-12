using AutoMapper;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CakeExtracter.Common;
using CakeExtracter.SimpleRepositories;
using CakeExtracter.SimpleRepositories.Interfaces;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA
{
    public class TDadSummaryLoader : Loader<TDadSummary>
    {
        public readonly int AccountId; // only used in AddUpdateDependentTDads()
        public static EntityIdStorage<TDad> TDadStorage;

        private readonly TDAdSetSummaryLoader adSetLoader;
        private readonly EntityIdStorage<AdSet> adSetStorage;
        private readonly SummaryMetricLoader metricLoader;
        private readonly ISimpleRepository<EntityType> typeRepository;

        static TDadSummaryLoader()
        {
            TDadStorage = new EntityIdStorage<TDad>(x => x.Id, x => $"{x.AccountId} {x.AdSetId} {x.Name} {x.ExternalId}", x => $"{x.AccountId} {x.Name} {x.ExternalId}");
        }

        public TDadSummaryLoader(int accountId = -1)
        {
            AccountId = accountId;
            adSetLoader = new TDAdSetSummaryLoader(accountId);
            adSetStorage = TDAdSetSummaryLoader.AdSetStorage;
            metricLoader = new SummaryMetricLoader();
            typeRepository = new TypeRepository();
        }

        protected override int Load(List<TDadSummary> items)
        {
            Logger.Info(AccountId, "Loading {0} DA-TD AdSummaries..", items.Count);
            PrepareData(items);
            AddUpdateDependentAdSets(items);
            AddUpdateDependentTDads(items);
            AssignTDadIdToItems(items);
            var count = UpsertDailySummaries(items);
            return count;
        }

        public void PrepareData(List<TDadSummary> items)
        {
            items.ForEach(x =>
            {
                if (x.TDad == null)
                {
                    x.TDad = new TDad();
                }
                Mapper.Map(x, x.TDad);
            });
        }

        public void AssignTDadIdToItems(List<TDadSummary> items)
        {
            foreach (var item in items)
            {
                if (!TDadStorage.IsEntityInStorage(item.TDad))
                {
                    continue;
                }

                item.TDadId = TDadStorage.GetEntityIdFromStorage(item.TDad);
                item.TDad = null;
                // otherwise it will get skipped; no TDad to use for the foreign key
            }
        }

        public int UpsertDailySummaries(List<TDadSummary> items)
        {
            var progress = new LoadingProgress();
            using (var db = new ClientPortalProgContext())
            {
                var itemTDadIds = items.Select(i => i.TDadId).Distinct().ToArray();
                var tdAdIdsInDb = db.TDads.Select(a => a.Id).Where(i => itemTDadIds.Contains(i)).ToArray();

                foreach (var item in items)
                {
                    var target = db.Set<TDadSummary>().Find(item.Date, item.TDadId);
                    if (target == null)
                    {
                        TryToAddSummary(db, item, tdAdIdsInDb, progress);
                    }
                    else // TDadSummary already exists
                    {
                        TryToUpdateSummary(db, item, target, progress);
                    }

                    progress.ItemCount++;
                }

                SafeContextWrapper.TrySaveChanges(db);
            }

            Logger.Info(AccountId, "Saving {0} TDadSummaries ({1} updates, {2} additions, {3} duplicates, {4} deleted, {5} already-deleted, {6} skipped)",
                progress.ItemCount, progress.UpdatedCount, progress.AddedCount, progress.DuplicateCount, progress.DeletedCount, progress.AlreadyDeletedCount, progress.SkippedCount);
            if (progress.DuplicateCount > 0)
            {
                Logger.Warn(AccountId, "Encountered {0} duplicates which were skipped", progress.DuplicateCount);
            }
            return progress.ItemCount;
        }

        public void AddUpdateDependentAdSets(List<TDadSummary> items)
        {
            var adSets = items.Select(x => x.TDad.AdSet).ToList();
            adSetLoader.AddUpdateDependentAdSets(adSets);
            AssignAdSetIdToItems(items);
        }

        public void AddUpdateDependentTDads(List<TDadSummary> items)
        {
            var tdAds = items.Select(x => x.TDad).ToList();
            AddUpdateDependentTDads(tdAds);
        }

        public void AddUpdateDependentTDads(List<TDad> items)
        {
            var notNullableItems = items.Where(x => x != null).ToList();
            notNullableItems.ForEach(x => x.AccountId = AccountId);
            var tdAds = notNullableItems
                .GroupBy(x => new { x.AccountId, x.AdSetId, x.Name, x.ExternalId })
                .Select(x => x.First())
                .ToList();
            AddUpdateDependentTDadsInDb(tdAds);
        }

        private void AddUpdateDependentTDadsInDb(List<TDad> items)
        {
            using (var db = new ClientPortalProgContext())
            {
                var extIds = items.Where(x => x.ExternalIds != null).SelectMany(x => x.ExternalIds).ToList();
                AddDependentExternalIdTypes(db, extIds);
                foreach (var tdAd in items)
                {
                    if (TDadStorage.IsEntityInStorage(tdAd))
                    {
                        continue; // already encountered this TDad
                    }

                    SafeContextWrapper.Lock(SafeContextWrapper.AdLocker, () =>
                    {
                        var tdAdsInDbList = GetTDads(db, tdAd);
                        if (!tdAdsInDbList.Any())
                        {
                            // TDad doesn't exist in the db; so create it and put an entry in the lookup
                            var tdAdInDb = AddTDad(db, tdAd);
                            Logger.Info(AccountId, "Saved new TDad: {0} ({1}), ExternalId={2}", tdAdInDb.Name, tdAdInDb.Id, tdAdInDb.ExternalId);
                            TDadStorage.AddEntityIdToStorage(tdAdInDb);
                        }
                        else
                        {
                            // Update & put existing TDad in the lookup
                            // There should only be one matching TDad in the db, but just in case...
                            var numUpdates = UpdateTDads(db, tdAdsInDbList, tdAd);
                            if (numUpdates > 0)
                            {
                                Logger.Info(AccountId, "Updated TDad: {0}, Eid={1}", tdAd.Name, tdAd.ExternalId);
                                if (numUpdates > 1)
                                {
                                    Logger.Warn(AccountId, "Multiple entities in db ({0})", numUpdates);
                                }
                            }
                            TDadStorage.AddEntityIdToStorage(tdAdsInDbList.First());
                        }
                    });
                }
            }
        }

        private void AssignAdSetIdToItems(IEnumerable<TDadSummary> items)
        {
            foreach (var item in items)
            {
                if (adSetStorage.IsEntityInStorage(item.TDad.AdSet))
                {
                    item.TDad.AdSetId = adSetStorage.GetEntityIdFromStorage(item.TDad.AdSet);
                }
                item.TDad.AdSet = null;
            }
        }

        private void AddDependentExternalIdTypes(ClientPortalProgContext db, IEnumerable<TDadExternalId> externalIds)
        {
            var notStoredTypes = externalIds.Select(x => x.Type).Where(x => x?.Name != null).DistinctBy(x => x.Name).ToList();
            typeRepository.AddItems(db, notStoredTypes);
            AssignTypeIdToExternalIds(externalIds);
        }

        private void AssignTypeIdToExternalIds(IEnumerable<TDadExternalId> externalIds)
        {
            foreach (var id in externalIds)
            {
                if (typeRepository.IdStorage.IsEntityInStorage(id.Type))
                {
                    id.TypeId = typeRepository.IdStorage.GetEntityIdFromStorage(id.Type);
                    id.Type = null;
                }
            }
        }

        private void TryToAddSummary(ClientPortalProgContext db, TDadSummary item, IEnumerable<int> tdAdIdsInDb, LoadingProgress progress)
        {
            if (item.AllZeros())
            {
                progress.AlreadyDeletedCount++;
                return;
            }
            if (!tdAdIdsInDb.Contains(item.TDadId))
            {
                Logger.Warn(AccountId, "Skipping load of item. TDad with id {0} does not exist.", item.TDadId);
                progress.SkippedCount++;
                return;
            }
            db.TDadSummaries.Add(item);
            UpsertSummaryMetrics(db, item);
            progress.AddedCount++;
        }

        private void TryToUpdateSummary(ClientPortalProgContext db, TDadSummary item, TDadSummary target, LoadingProgress progress)
        {
            var entry = db.Entry(target);
            if (entry.State != EntityState.Unchanged)
            {
                Logger.Warn(AccountId, "Encountered duplicate for {0:d} - TDad {1}", item.Date, item.TDadId);
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

        private void UpsertSummaryMetrics(ClientPortalProgContext db, TDadSummary item)
        {
            item.InitialMetrics = GetItemMetrics(item);
            if (item.InitialMetrics == null || !item.InitialMetrics.Any())
            {
                return;
            }
            item.InitialMetrics.ForEach(x => x.EntityId = item.TDadId);
            metricLoader.AddDependentMetricTypes(item.InitialMetrics);
            metricLoader.AssignMetricTypeIdToItems(item.InitialMetrics);
            metricLoader.UpsertSummaryMetrics<TDadSummaryMetric>(db, item.InitialMetrics);
        }

        private IEnumerable<SummaryMetric> GetItemMetrics(TDadSummary item)
        {
            var metrics = item.InitialMetrics == null
                ? item.Metrics
                : item.Metrics == null
                    ? item.InitialMetrics
                    : item.InitialMetrics.Concat(item.Metrics);
            return metrics?.ToList();
        }

        private void RemoveOldSummaryMetrics(ClientPortalProgContext db, TDadSummary item, TDadSummary target)
        {
            var deletedMetrics = item.InitialMetrics == null
                ? target.Metrics
                : target.Metrics.Where(x => item.InitialMetrics.All(m => m.MetricTypeId != x.MetricTypeId));
            metricLoader.RemoveMetrics(db, deletedMetrics);
        }

        private List<TDad> GetTDads(ClientPortalProgContext db, TDad tdAd)
        {
            IQueryable<TDad> tdAdsInDb;
            if (!string.IsNullOrWhiteSpace(tdAd.ExternalId))
            {
                // See if a TDad with that ExternalId exists
                tdAdsInDb = db.TDads.Where(a => a.AccountId == tdAd.AccountId && a.ExternalId == tdAd.ExternalId);
                // If not, check for a match by name where ExternalId == null
                if (!tdAdsInDb.Any())
                {
                    tdAdsInDb = db.TDads.Where(x => x.AccountId == tdAd.AccountId && x.ExternalId == null && x.Name == tdAd.Name);
                    if (tdAd.AdSetId.HasValue)
                    {
                        tdAdsInDb = tdAdsInDb.Where(x => x.AdSetId == tdAd.AdSetId);
                    }
                }
            }
            else
            {
                // Check by TDad name
                tdAdsInDb = db.TDads.Where(x => x.AccountId == tdAd.AccountId && x.Name == tdAd.Name);
                if (tdAd.AdSetId.HasValue)
                {
                    tdAdsInDb = tdAdsInDb.Where(x => x.AdSetId == tdAd.AdSetId);
                }
            }
            //Note: If we're grouping by ad name, then the ads in the db and the ads to-be-loaded shouldn't have externalIds filled in.
            return tdAdsInDb.ToList();
        }

        private TDad AddTDad(ClientPortalProgContext db, TDad tdAdProps)
        {
            var tdAd = new TDad
            {
                AccountId = tdAdProps.AccountId,
                AdSetId = tdAdProps.AdSetId,
                ExternalId = tdAdProps.ExternalId,
                Name = tdAdProps.Name,
                ExternalIds = tdAdProps.ExternalIds
            };
            db.TDads.Add(tdAd);
            SafeContextWrapper.TrySaveChanges(db);
            return tdAd;
        }

        private int UpdateTDads(ClientPortalProgContext db, IEnumerable<TDad> tdAdInDbList, TDad tdAdProps)
        {
            var numChanges = 0;
            foreach (var tdAd in tdAdInDbList)
            {
                if (!string.IsNullOrWhiteSpace(tdAdProps.ExternalId) && tdAd.ExternalId != tdAdProps.ExternalId)
                {
                    tdAd.ExternalId = tdAdProps.ExternalId;
                    numChanges++;
                }
                if (!string.IsNullOrWhiteSpace(tdAdProps.Name) && tdAd.Name != tdAdProps.Name)
                {
                    tdAd.Name = tdAdProps.Name;
                    numChanges++;
                }
                if (tdAdProps.AdSetId.HasValue && tdAd.AdSetId != tdAdProps.AdSetId)
                {
                    tdAd.AdSetId = tdAdProps.AdSetId;
                    numChanges++;
                }
                numChanges += UpdateTDadExternalIds(tdAd, tdAdProps);
            }
            if (numChanges > 0)
            {
                SafeContextWrapper.TrySaveChanges(db);
            }
            return numChanges;
        }

        private int UpdateTDadExternalIds(TDad tdAd, TDad tdAdProps)
        {
            var numChanges = 0;
            if (tdAdProps.ExternalIds == null)
            {
                return numChanges;
            }
            foreach (var idProp in tdAdProps.ExternalIds)
            {
                var externalId = tdAd.ExternalIds.FirstOrDefault(x => x.TypeId == idProp.TypeId);
                if (externalId != null)
                {
                    if (externalId.ExternalId != idProp.ExternalId)
                    {
                        externalId.ExternalId = idProp.ExternalId;
                        numChanges++;
                    }
                }
                else
                {
                    tdAd.ExternalIds.Add(idProp);
                    numChanges++;
                }
            }
            return numChanges;
        }
    }
}
