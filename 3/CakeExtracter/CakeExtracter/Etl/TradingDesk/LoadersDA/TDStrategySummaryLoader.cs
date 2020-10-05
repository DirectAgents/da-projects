using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using CakeExtracter.Common;
using CakeExtracter.Helpers;
using CakeExtracter.SimpleRepositories.RepositoriesWithStorage;
using CakeExtracter.SimpleRepositories.RepositoriesWithStorage.Interfaces;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;


namespace CakeExtracter.Etl.TradingDesk.LoadersDA
{
    public class TDStrategySummaryLoader : Loader<StrategySummary>
    {
        public static EntityIdStorage<Strategy> DefaultStrategyStorage = StorageCollection.StrategyWithEidStorage;

        // Note accountId is only used in AddUpdateDependentStrategies() (i.e. not by AdrollCampaignSummaryLoader)
        public int AccountId;
        private readonly SummaryMetricLoader metricLoader;
        private readonly IRepositoryWithStorage<EntityType, ClientPortalProgContext> typeRepositoryWithStorage;
        private readonly IRepositoryWithStorage<Strategy, ClientPortalProgContext> strategyRepositoryWithStorage;

        public TDStrategySummaryLoader(int accountId = -1, EntityIdStorage<Strategy> strategyStorage = null)
        {
            AccountId = accountId;
            metricLoader = new SummaryMetricLoader();
            typeRepositoryWithStorage = new TypeRepositoryWithStorage(StorageCollection.TypeStorage);
            strategyRepositoryWithStorage = new StrategyRepositoryWithStorage(strategyStorage ?? DefaultStrategyStorage);
        }

        /// <inheritdoc/>
        protected override int Load(List<StrategySummary> items)
        {
            Logger.Info(AccountId, "Loading {0} DA-TD StrategySummaries..", items.Count);
            PrepareData(items);
            AddUpdateDependentStrategies(items);
            AssignStrategyIdToItems(items);
            var count = UpsertDailySummaries(items);
            return count;
        }

        public void PrepareData(List<StrategySummary> items)
        {
            items.ForEach(x =>
            {
                if (x.Strategy == null)
                {
                    x.Strategy = new Strategy();
                }

                x.Strategy.AccountId = AccountId;
                Mapper.Map(x, x.Strategy);
            });
        }

        public void AssignStrategyIdToItems(List<StrategySummary> items)
        {
            foreach (var item in items)
            {
                if (strategyRepositoryWithStorage.IdStorage.IsEntityInStorage(item.Strategy))
                {
                    item.StrategyId = strategyRepositoryWithStorage.IdStorage.GetEntityIdFromStorage(item.Strategy);
                    item.Strategy = null;
                    // otherwise it will get skipped; no strategy to use for the foreign key
                }
            }
        }

        public int UpsertDailySummaries(List<StrategySummary> items)
        {
            var progress = new LoadingProgress();
            using (var db = new ClientPortalProgContext())
            {
                var itemStrategyIds = items.Select(i => i.StrategyId).Distinct().ToArray();
                var strategyIdsInDb = db.Strategies.Select(s => s.Id).Where(i => itemStrategyIds.Contains(i))
                    .ToArray();

                foreach (var item in items)
                {
                    var target = db.Set<StrategySummary>().Find(item.Date, item.StrategyId);
                    if (target == null)
                    {
                        TryToAddSummary(db, item, strategyIdsInDb, progress);
                    }
                    else // StrategySummary already exists
                    {
                        TryToUpdateSummary(db, item, target, progress);
                    }

                    progress.ItemCount++;
                }

                SafeContextWrapper.TrySaveChanges(db);
            }

            Logger.Info(AccountId, "Saving {0} StrategySummaries ({1} updates, {2} additions, {3} duplicates, {4} deleted, {5} already-deleted, {6} skipped)",
                progress.ItemCount, progress.UpdatedCount, progress.AddedCount, progress.DuplicateCount, progress.DeletedCount, progress.AlreadyDeletedCount, progress.SkippedCount);
            if (progress.DuplicateCount > 0)
            {
                Logger.Warn(AccountId, "Encountered {0} duplicates which were skipped", progress.DuplicateCount);
            }
            return progress.ItemCount;
        }

        public void AddUpdateDependentStrategies(IEnumerable<StrategySummary> items)
        {
            var strategies = items.Select(x => x.Strategy).ToList();
            AddUpdateDependentStrategies(strategies);
        }

        public void AddUpdateDependentStrategies(IEnumerable<Strategy> items)
        {
            var notNullableItems = items.Where(x => x != null).ToList();
            notNullableItems.ForEach(x => x.AccountId = AccountId);
            var strategies = notNullableItems
                .Where(x => !string.IsNullOrWhiteSpace(x.Name) || !string.IsNullOrWhiteSpace(x.ExternalId))
                .GroupBy(x => new {x.AccountId, x.Name, x.ExternalId})
                .Select(x => x.First())
                .ToList();
            AddUpdateDependentStrategiesInDb(strategies);
        }

        private void AddUpdateDependentStrategiesInDb(IEnumerable<Strategy> strategies)
        {
            using (var db = new ClientPortalProgContext())
            {
                AddDependentStrategyTypes(db, strategies);
                foreach (var strategyProps in strategies)
                {
                    if (strategyRepositoryWithStorage.IdStorage.IsEntityInStorage(strategyProps))
                    {
                        continue; // already encountered this strategy
                    }

                    SafeContextWrapper.Lock(SafeContextWrapper.StrategyLocker, () =>
                    {
                        var strategiesInDbList = strategyRepositoryWithStorage.GetItems(db, strategyProps);
                        if (!strategiesInDbList.Any())
                        {
                            TryToAddStrategy(db, strategyProps);
                        }
                        else
                        {
                            // Update & put existing Strategy in the lookup
                            // There should only be one matching Strategy in the db, but just in case...
                            TryToUpdateStrategies(db, strategiesInDbList, strategyProps);
                        }
                    });
                }
            }
        }

        private void AddDependentStrategyTypes(ClientPortalProgContext db, IEnumerable<Strategy> strategies)
        {
            var targetingTypes = strategies.Select(x => x.TargetingType);
            var types = strategies.Select(x => x.Type).Concat(targetingTypes);
            var notStoredTypes = types.Where(x => x?.Name != null).DistinctBy(x => x.Name).ToList();
            typeRepositoryWithStorage.AddItems(db, notStoredTypes);
            AssignTypeIdToStrategies(strategies);
        }

        private void AssignTypeIdToStrategies(IEnumerable<Strategy> strategies)
        {
            foreach (var strategy in strategies)
            {
                if (typeRepositoryWithStorage.IdStorage.IsEntityInStorage(strategy.TargetingType))
                {
                    strategy.TargetingTypeId = typeRepositoryWithStorage.IdStorage.GetEntityIdFromStorage(strategy.TargetingType);
                }

                if (typeRepositoryWithStorage.IdStorage.IsEntityInStorage(strategy.Type))
                {
                    strategy.TypeId = typeRepositoryWithStorage.IdStorage.GetEntityIdFromStorage(strategy.Type);
                }

                strategy.Type = null;
                strategy.TargetingType = null;
            }
        }

        private void TryToAddSummary(ClientPortalProgContext db, StrategySummary item, IEnumerable<int> strategyIdsInDb, LoadingProgress progress)
        {
            if (item.AllZeros())
            {
                progress.AlreadyDeletedCount++;
                return;
            }
            if (!strategyIdsInDb.Contains(item.StrategyId))
            {
                Logger.Warn(AccountId, "Skipping load of item. Strategy with id {0} does not exist.", item.StrategyId);
                progress.SkippedCount++;
                return;
            }
            db.StrategySummaries.Add(item);
            UpsertSummaryMetrics(db, item);
            progress.AddedCount++;
        }

        private void TryToUpdateSummary(ClientPortalProgContext db, StrategySummary item, StrategySummary target, LoadingProgress progress)
        {
            var entry = db.Entry(target);
            if (entry.State != EntityState.Unchanged)
            {
                Logger.Warn(AccountId, "Encountered duplicate for {0:d} - Strategy {1}", item.Date, item.StrategyId);
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

        private void UpsertSummaryMetrics(ClientPortalProgContext db, StrategySummary item)
        {
            item.InitialMetrics = GetItemMetrics(item);
            if (item.InitialMetrics == null || !item.InitialMetrics.Any())
            {
                return;
            }
            item.InitialMetrics.ForEach(x => x.EntityId = item.StrategyId);
            metricLoader.AddDependentMetricTypes(item.InitialMetrics);
            metricLoader.AssignMetricTypeIdToItems(item.InitialMetrics);
            metricLoader.UpsertSummaryMetrics<StrategySummaryMetric>(db, item.InitialMetrics);
        }

        private IEnumerable<SummaryMetric> GetItemMetrics(StrategySummary item)
        {
            var metrics = item.InitialMetrics == null
                ? item.Metrics
                : item.Metrics == null
                    ? item.InitialMetrics
                    : item.InitialMetrics.Concat(item.Metrics);
            return metrics?.ToList();
        }

        private void RemoveOldSummaryMetrics(ClientPortalProgContext db, StrategySummary item, StrategySummary target)
        {
            var deletedMetrics = item.InitialMetrics == null
                ? target.Metrics
                : target.Metrics.Where(x => item.InitialMetrics.All(m => m.MetricTypeId != x.MetricTypeId));
            metricLoader.RemoveMetrics(db, deletedMetrics);
        }

        private void TryToAddStrategy(ClientPortalProgContext db, Strategy strategyProps)
        {
            var strategy = strategyRepositoryWithStorage.AddItem(db, strategyProps);
            if (strategy == null)
            {
                return;
            }

            Logger.Info(accountId, "Saved new Strategy: {0} ({1}), ExternalId={2}", strategy.Name, strategy.Id, strategy.ExternalId);
        }

        private void TryToUpdateStrategies(ClientPortalProgContext db, IEnumerable<Strategy> strategiesInDbList, Strategy strategyProps)
        {
            foreach (var strategy in strategiesInDbList)
            {
                TryToUpdateStrategy(db, strategy, strategyProps);
            }
        }

        private void TryToUpdateStrategy(ClientPortalProgContext db, Strategy strategy, Strategy strategyProps)
        {
            var numUpdates = strategyRepositoryWithStorage.UpdateItem(db, strategyProps, strategy);
            if (numUpdates <= 0)
            {
                return;
            }

            Logger.Info(accountId, "Updated Strategy: {0}, Eid={1}", strategyProps.Name, strategyProps.ExternalId);
            if (numUpdates > 1)
            {
                Logger.Warn(accountId, "Multiple entities in db ({0})", numUpdates);
            }
        }

        protected void ProcessFailedStatsLoading(Exception e, List<StrategySummary> items)
        {
        }
    }
}
