using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using CakeExtracter.Etl.TradingDesk.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA
{
    public class TDStrategySummaryLoader : Loader<StrategySummary>
    {
        // Note accountId is only used in AddUpdateDependentStrategies() (i.e. not by AdrollCampaignSummaryLoader)
        public readonly int AccountId;
        public static readonly EntityIdStorage<Strategy> StrategyStorage;
        private static readonly EntityIdStorage<EntityType> typeStorage;
        private readonly bool preLoadStrategies;
        private readonly SummaryMetricLoader metricLoader;

        static TDStrategySummaryLoader()
        {
            StrategyStorage = new EntityIdStorage<Strategy>(x => x.Id, x => $"{x.Name}{x.ExternalId}");
            typeStorage = new EntityIdStorage<EntityType>(x => x.Id, x => x.Name);
        }

        public TDStrategySummaryLoader(int accountId = -1, bool preLoadStrategies = false)
        {
            this.AccountId = accountId;
            this.preLoadStrategies = preLoadStrategies;
            this.metricLoader = new SummaryMetricLoader();
        }

        protected override int Load(List<StrategySummary> items)
        {
            Logger.Info(AccountId, "Loading {0} DA-TD StrategySummaries..", items.Count);
            if (!preLoadStrategies)
            {
                PrepareData(items);
                AddUpdateDependentStrategies(items);
            }
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
                Mapper.Map(x, x.Strategy);
            });
        }

        public void AssignStrategyIdToItems(List<StrategySummary> items)
        {
            foreach (var item in items)
            {
                if (StrategyStorage.IsEntityInStorage(item.Strategy))
                {
                    item.StrategyId = StrategyStorage.GetEntityIdFromStorage(item.Strategy);
                    item.Strategy = null;
                }
                // otherwise it will get skipped; no strategy to use for the foreign key
            }
        }

        public int UpsertDailySummaries(List<StrategySummary> items)
        {
            var progress = new LoadingProgress();
            using (var db = new ClientPortalProgContext())
            {
                var itemStrategyIds = items.Select(i => i.StrategyId).Distinct().ToArray();
                var strategyIdsInDb = db.Strategies.Select(s => s.Id).Where(i => itemStrategyIds.Contains(i)).ToArray();

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
                Logger.Info(AccountId, "Saving {0} StrategySummaries ({1} updates, {2} additions, {3} duplicates, {4} deleted, {5} already-deleted, {6} skipped)",
                    progress.ItemCount, progress.UpdatedCount, progress.AddedCount, progress.DuplicateCount, progress.DeletedCount, progress.AlreadyDeletedCount, progress.SkippedCount);
                if (progress.DuplicateCount > 0)
                {
                    Logger.Warn(AccountId, "Encountered {0} duplicates which were skipped", progress.DuplicateCount);
                }
                var numChanges = db.SaveChanges();
            }
            return progress.ItemCount;
        }

        public void AddUpdateDependentStrategies(IEnumerable<StrategySummary> items)
        {
            var strategies = items
                .GroupBy(i => new { i.Strategy.Name, i.Strategy.ExternalId })
                .Select(x => x.First().Strategy)
                .ToList();
            AddUpdateDependentStrategies(strategies, AccountId);
        }

        public void AddUpdateDependentStrategies(IEnumerable<Strategy> items)
        {
            AddUpdateDependentStrategies(items, AccountId);
        }

        public void AddUpdateDependentStrategies(IEnumerable<Strategy> strategies, int accountId)
        {
            using (var db = new ClientPortalProgContext())
            {
                AddDependentStrategyTypes(db, strategies);
                foreach (var strategyProps in strategies)
                {
                    if (StrategyStorage.IsEntityInStorage(strategyProps))
                    {
                        continue; // already encountered this strategy
                    }

                    var stratsInDbList = GetStrategies(db, strategyProps, accountId);
                    if (!stratsInDbList.Any())
                    {
                        var strategy = AddStrategy(db, strategyProps, accountId);   // Strategy doesn't exist in the db; so create it and put an entry in the lookup
                        Logger.Info(accountId, "Saved new Strategy: {0} ({1}), ExternalId={2}", strategy.Name, strategy.Id, strategy.ExternalId);
                        StrategyStorage.AddEntityIdToStorage(strategy);
                    }
                    else
                    {   // Update & put existing Strategy in the lookup
                        // There should only be one matching Strategy in the db, but just in case...
                        var numUpdates = UpdateStrategies(db, stratsInDbList, strategyProps);
                        if (numUpdates > 0)
                        {
                            Logger.Info(accountId, "Updated Strategy: {0}, Eid={1}", strategyProps.Name, strategyProps.ExternalId);
                            if (numUpdates > 1)
                            {
                                Logger.Warn(accountId, "Multiple entities in db ({0})", numUpdates);
                            }
                        }
                        StrategyStorage.AddEntityIdToStorage(stratsInDbList.First());
                    }
                }
            }
        }

        private void AddDependentStrategyTypes(ClientPortalProgContext db, IEnumerable<Strategy> strategies)
        {
            var notStoredTypes = strategies.GroupBy(x => x.Type?.Name)
                .Select(x => x.First().Type)
                .Where(x => x != null && !string.IsNullOrEmpty(x.Name) && !typeStorage.IsEntityInStorage(x))
                .ToList();
            AddDependentTypes(db, notStoredTypes);
            AssignTypeIdToStrategies(strategies);
        }

        private void AddDependentTypes(ClientPortalProgContext db, IEnumerable<EntityType> types)
        {
            var newTypes = new List<EntityType>();
            foreach (var type in types)
            {
                var typeInDB = db.Types.FirstOrDefault(x => type.Name == x.Name);
                if (typeInDB == null)
                {
                    newTypes.Add(type);
                }
                else
                {
                    typeStorage.AddEntityIdToStorage(typeInDB);
                }
            }
            db.Types.AddRange(newTypes);
            db.SaveChanges();
            newTypes.ForEach(typeStorage.AddEntityIdToStorage);
        }

        private void AssignTypeIdToStrategies(IEnumerable<Strategy> strategies)
        {
            foreach (var strategy in strategies)
            {
                if (typeStorage.IsEntityInStorage(strategy.Type))
                {
                    strategy.TypeId = typeStorage.GetEntityIdFromStorage(strategy.Type);
                }
                strategy.Type = null;
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
            progress.UpdatedCount++;
        }

        private void UpsertSummaryMetrics(ClientPortalProgContext db, StrategySummary item)
        {
            if (item.Metrics == null || !item.Metrics.Any())
            {
                return;
            }
            item.Metrics.ForEach(x => x.EntityId = item.StrategyId);
            metricLoader.AddDependentMetricTypes(item.Metrics);
            metricLoader.AssignMetricTypeIdToItems(item.Metrics);
            metricLoader.UpsertSummaryMetrics<StrategySummaryMetric>(db, item.Metrics);
        }

        private List<Strategy> GetStrategies(ClientPortalProgContext db, Strategy strategy, int accountId)
        {
            IEnumerable<Strategy> stratsInDb;
            if (string.IsNullOrWhiteSpace(strategy.ExternalId))
            {
                stratsInDb = db.Strategies.Where(s => s.AccountId == accountId && s.Name == strategy.Name);
                return stratsInDb.ToList();
            }
            // First see if a Strategy with that ExternalId exists
            stratsInDb = db.Strategies.Where(s => s.AccountId == accountId && s.ExternalId == strategy.ExternalId);
            if (!stratsInDb.Any()) // If not, check for a match by name where ExternalId == null
            {
                stratsInDb = db.Strategies.Where(s => s.AccountId == accountId && s.ExternalId == null && s.Name == strategy.Name);
            }
            return stratsInDb.ToList();
        }

        private Strategy AddStrategy(ClientPortalProgContext db, Strategy strategyProps, int accountId)
        {
            var strategy = new Strategy
            {
                AccountId = accountId,
                ExternalId = strategyProps.ExternalId,
                Name = strategyProps.Name,
                TypeId = strategyProps.TypeId
            };
            db.Strategies.Add(strategy);
            db.SaveChanges();
            return strategy;
        }

        private int UpdateStrategies(ClientPortalProgContext db, IEnumerable<Strategy> stratsInDbList, Strategy strategyProps)
        {
            foreach (var strategy in stratsInDbList)
            {
                if (!string.IsNullOrWhiteSpace(strategyProps.ExternalId))
                {
                    strategy.ExternalId = strategyProps.ExternalId;
                }
                if (!string.IsNullOrWhiteSpace(strategyProps.Name))
                {
                    strategy.Name = strategyProps.Name;
                }
                if (strategyProps.TypeId.HasValue)
                {
                    strategy.TypeId = strategyProps.TypeId;
                }
            }
            return db.SaveChanges();
        }
    }
}
