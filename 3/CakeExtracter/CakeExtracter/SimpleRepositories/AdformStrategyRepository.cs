﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CakeExtracter.Helpers;
using CakeExtracter.SimpleRepositories.Interfaces;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.SimpleRepositories
{
    /// <summary>
    /// Adform Strategy Repository for Adform ETL:
    /// uses instead of the simple Strategy Repository,
    /// because it needs other keys when working with Entity Storage
    /// </summary>
    class AdformStrategyRepository : ISimpleRepository<Strategy>
    {
        private static readonly EntityIdStorage<Strategy> StrategyStorage;

        static AdformStrategyRepository()
        {
            StrategyStorage = new EntityIdStorage<Strategy>(x => x.Id,
                x => (x.ExternalId == null && x.Name == null) ? null : $"{x.AccountId} {x.Name} {x.ExternalId}",
                x => (x.ExternalId == null) ? null : $"{x.AccountId} {x.ExternalId}",
                x => (x.Name == null) ? null : $"{x.AccountId} {x.Name}");
        }

        public EntityIdStorage<Strategy> IdStorage => StrategyStorage;

        public List<Strategy> GetItems<TContext>(TContext db, Strategy itemToCompare) where TContext : DbContext, new()
        {
            IEnumerable<Strategy> strategiesInDb;
            if (string.IsNullOrWhiteSpace(itemToCompare.ExternalId))
            {
                strategiesInDb = db.Set<Strategy>().Where(s => s.AccountId == itemToCompare.AccountId && s.Name == itemToCompare.Name);
                return strategiesInDb.ToList();
            }
            strategiesInDb = db.Set<Strategy>().Where(s => s.AccountId == itemToCompare.AccountId && s.ExternalId == itemToCompare.ExternalId);
            if (!strategiesInDb.Any())
            {
                strategiesInDb = db.Set<Strategy>().Where(s => s.AccountId == itemToCompare.AccountId && s.ExternalId == null && s.Name == itemToCompare.Name);
            }
            return strategiesInDb.ToList();
        }

        public Strategy AddItem<TContext>(TContext db, Strategy sourceItem) where TContext : DbContext, new()
        {
            db.Set<Strategy>().Add(sourceItem);
            var numChanges = SafeContextWrapper.TrySaveChanges(db);
            if (numChanges == 0)
            {
                return null;
            }

            StrategyStorage.AddEntityIdToStorage(sourceItem);
            return sourceItem;
        }

        public int UpdateItem<TContext>(TContext db, Strategy sourceItem, Strategy targetItemInDb)
            where TContext : DbContext, new()
        {
            UpdateStrategy(sourceItem, targetItemInDb);
            var numChanges = SafeContextWrapper.TrySaveChanges(db);
            StrategyStorage.AddEntityIdToStorage(targetItemInDb);
            return numChanges;
        }

        public void AddItems(IEnumerable<Strategy> items)
        {
            using (var db = new ClientPortalProgContext())
            {
                AddItems(db, items);
            }
        }

        public void AddItems<TContext>(TContext db, IEnumerable<Strategy> items) where TContext : DbContext, new()
        {
            throw new NotImplementedException();
        }

        private void UpdateStrategy(Strategy sourceItem, Strategy targetItemInDb)
        {
            if (!string.IsNullOrWhiteSpace(sourceItem.ExternalId))
            {
                targetItemInDb.ExternalId = sourceItem.ExternalId;
            }
            if (!string.IsNullOrWhiteSpace(sourceItem.Name))
            {
                targetItemInDb.Name = sourceItem.Name;
            }
            if (sourceItem.TargetingTypeId.HasValue)
            {
                targetItemInDb.TargetingTypeId = sourceItem.TargetingTypeId;
            }
            if (sourceItem.TypeId.HasValue)
            {
                targetItemInDb.TypeId = sourceItem.TypeId;
            }
        }
    }
}
