using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg.Facebook;
using System.Collections.Generic;
using System.Linq;

namespace CakeExtracter.Etl.SocialMarketing.EntitiesLoaders
{
    public abstract class BaseFacebookEntityLoader<T> where T : FbEntity
    {
        protected void AddUpdateDependentItems(List<T> items, EntityIdStorage<T> fbAdEntityIdStorage, object lockObject)
        {
            using (var db = new ClientPortalProgContext())
            {
                var itemsToBeAdded = new List<T>();
                var itemsToBeUpdated = new List<T>();
                var itemsToProcess = items.Where(item => !fbAdEntityIdStorage.IsEntityInStorage(item)).ToList();
                foreach (var item in itemsToProcess)
                {
                    var existingItem = db.Set<T>().FirstOrDefault(ad => ad.ExternalId == item.ExternalId && ad.AccountId == item.AccountId);
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
                AddItemsToEntityIdStorage(itemsToProcess, fbAdEntityIdStorage);
            }
        }

        protected abstract bool UpdateExistingDbItemPropertiesIfNecessary(T existingDbItem, T latestItemFromApi);

        protected void AssignIdToItems(List<T> items, EntityIdStorage<T> fbAdEntityIdStorage)
        {
            items.ForEach(item =>
            {
                if (fbAdEntityIdStorage.IsEntityInStorage(item))
                {
                    item.Id = fbAdEntityIdStorage.GetEntityIdFromStorage(item);
                }
            });
        }

        private void AddMissedDbItems(List<T> itemsToBeAdded, object lockObject)
        {
            SafeContextWrapper.TryMakeTransactionWithLock<ClientPortalProgContext>(dbContext =>
            {
                dbContext.BulkInsert(itemsToBeAdded);
            }, lockObject, "BulkInserting");
        }

        private void UpdateOutdatedDbItems(List<T> itemsToBeUpdated, object lockObject)
        {
            SafeContextWrapper.TryMakeTransactionWithLock<ClientPortalProgContext>(dbContext =>
            {
                dbContext.BulkUpdate(itemsToBeUpdated);
            }, lockObject, "BulkUpdating");
        }

        private void AddItemsToEntityIdStorage(List<T> items, EntityIdStorage<T> entityIdStorage)
        {
            items.ForEach(c => { entityIdStorage.AddEntityIdToStorage(c); });
        }
    }
}
