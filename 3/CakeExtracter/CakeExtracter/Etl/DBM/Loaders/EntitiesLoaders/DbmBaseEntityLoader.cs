using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg.DBM.Entities;

namespace CakeExtracter.Etl.DBM.Loaders.EntitiesLoaders
{
    public abstract class DbmBaseEntityLoader<T> where T : DbmEntity
    {
        /// <summary>
        /// Adds or updates dependent items.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="dbmEntityIdStorage">The dbm entity identifier storage.</param>
        /// <param name="lockObject">The lock object.</param>
        protected void AddUpdateDependentItems(List<T> items, EntityIdStorage<T> dbmEntityIdStorage, object lockObject)
        {
            using (var db = new ClientPortalProgContext())
            {
                var itemsToBeAdded = new List<T>();
                var itemsToBeUpdated = new List<T>();
                var itemsToProcess = items.Where(item => !dbmEntityIdStorage.IsEntityInStorage(item)).ToList();
                foreach (var item in itemsToProcess)
                {
                    var existingItem = db.Set<T>().FirstOrDefault(x => x.ExternalId == item.ExternalId && x.Name == item.Name);
                    if (existingItem == null)
                    {
                        itemsToBeAdded.Add(item);
                    }
                    else
                    {
                        item.Id = existingItem.Id;
                        if (UpdateExistingDbItemPropertiesIfNecessary(existingItem, item))
                        {
                            itemsToBeUpdated.Add(existingItem);
                        }
                    }
                }
                AddMissedDbItems(itemsToBeAdded, lockObject);
                UpdateOutdatedDbItems(itemsToBeUpdated, lockObject);
                AddItemsToEntityIdStorage(itemsToProcess, dbmEntityIdStorage);
            }
        }

        /// <summary>
        /// Updates the existing database item properties if necessary.
        /// </summary>
        /// <param name="existingDbItem">The existing database item.</param>
        /// <param name="latestItemFromApi">The latest item from API.</param>
        /// <returns></returns>
        protected abstract bool UpdateExistingDbItemPropertiesIfNecessary(T existingDbItem, T latestItemFromApi);

        /// <summary>
        /// Assigns the identifier to items.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="dbmEntityIdStorage">The dbm entity identifier storage.</param>
        protected void AssignIdToItems(List<T> items, EntityIdStorage<T> dbmEntityIdStorage)
        {
            items.ForEach(item =>
            {
                if (dbmEntityIdStorage.IsEntityInStorage(item))
                {
                    item.Id = dbmEntityIdStorage.GetEntityIdFromStorage(item);
                }
            });
        }

        private void AddMissedDbItems(IReadOnlyCollection<T> itemsToBeAdded, object lockObject)
        {
            SafeContextWrapper.TryMakeTransactionWithLock<ClientPortalProgContext>(dbContext =>
            {
                dbContext.BulkInsert(itemsToBeAdded);
            }, lockObject, "BulkInserting");
        }

        private void UpdateOutdatedDbItems(IReadOnlyCollection<T> itemsToBeUpdated, object lockObject)
        {
            SafeContextWrapper.TryMakeTransactionWithLock<ClientPortalProgContext>(dbContext =>
            {
                dbContext.BulkUpdate(itemsToBeUpdated);
            }, lockObject, "BulkUpdating");
        }

        private void AddItemsToEntityIdStorage(List<T> items, EntityIdStorage<T> entityIdStorage)
        {
            items.ForEach(entityIdStorage.AddEntityIdToStorage);
        }
    }
}
