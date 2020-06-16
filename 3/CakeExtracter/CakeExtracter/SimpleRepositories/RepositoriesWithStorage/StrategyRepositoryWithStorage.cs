using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.Enums;
using CakeExtracter.Helpers;
using CakeExtracter.SimpleRepositories.RepositoriesWithStorage.Interfaces;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.SimpleRepositories.RepositoriesWithStorage
{
    /// <inheritdoc />
    /// <summary>
    /// The repository for working with Strategy objects in the database.
    /// </summary>
    internal class StrategyRepositoryWithStorage : IRepositoryWithStorage<Strategy, ClientPortalProgContext>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="StrategyRepositoryWithStorage"/>.
        /// </summary>
        /// <param name="storage">The storage for entity IDs.</param>
        public StrategyRepositoryWithStorage(EntityIdStorage<Strategy> storage)
        {
            IdStorage = storage;
        }

        /// <inheritdoc />
        public EntityIdStorage<Strategy> IdStorage { get; }

        /// <inheritdoc />
        public List<Strategy> GetItems(ClientPortalProgContext db, Strategy itemToCompare)
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

        /// <inheritdoc />
        public Strategy AddItem(ClientPortalProgContext db, Strategy sourceItem)
        {
            db.Set<Strategy>().Add(sourceItem);
            var numChanges = SafeContextWrapper.TrySaveChanges(db);
            if (numChanges == 0)
            {
                return null;
            }

            IdStorage.AddEntityIdToStorage(sourceItem);
            return sourceItem;
        }

        /// <inheritdoc />
        public int UpdateItem(ClientPortalProgContext db, Strategy sourceItem, Strategy targetItemInDb)
        {
            UpdateStrategy(sourceItem, targetItemInDb);
            var numChanges = SafeContextWrapper.TrySaveChanges(db);
            IdStorage.AddEntityIdToStorage(targetItemInDb);
            return numChanges;
        }

        /// <inheritdoc />
        public void AddItems(IEnumerable<Strategy> items)
        {
            using (var db = new ClientPortalProgContext())
            {
                AddItems(db, items);
            }
        }

        /// <inheritdoc />
        public void AddItems(ClientPortalProgContext db, IEnumerable<Strategy> items)
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
            if (sourceItem.TypeId.HasValue && (targetItemInDb.Type?.Name != CampaignType.ProductDisplay.ToString()))
            {
                targetItemInDb.TypeId = sourceItem.TypeId;
            }
        }
    }
}
