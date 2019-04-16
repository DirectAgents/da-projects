using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg.Facebook;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CakeExtracter.Etl.SocialMarketing.EntitiesLoaders
{
    public class BaseFacebookEntityLoader<T> where T : FbEntity
    {
        protected void AddUpdateDependentItems(List<T> items, EntityIdStorage<T> fbAdEntityIdStorage, object lockObject)
        {
            using (var db = new ClientPortalProgContext())
            {
                var itemsToBeAdded = new List<T>();
                var itemsToBeUpdated = new List<T>();
                var itemsToProcess = items.Where(item => !fbAdEntityIdStorage.IsEntityInStorage(item));
                foreach (var item in itemsToProcess)
                {
                    var existingItem = db.Set<T>().FirstOrDefault(ad => ad.ExternalId == item.ExternalId && ad.AccountId == item.AccountId);
                    if (existingItem == null)
                    {
                        itemsToBeAdded.Add(item);
                    }
                    else
                    {
                        if (UpdateExistingDbAdPropertiesIfNecessary(existingItem, item))
                        {
                            itemsToBeUpdated.Add(existingItem);
                        }
                    }
                }
                AddMissedDbItems(itemsToBeAdded, lockObject);
                UpdateOutdatedDbItems(itemsToBeUpdated);
                AddItemsToEntityIdStorage(items, fbAdEntityIdStorage);
            }
        }

        protected void AssignIdToItems(List<T> items, EntityIdStorage<T> fbAdEntityIdStorage)
        {
            items.ForEach(item =>
            {
                if (fbAdEntityIdStorage.IsEntityInStorage(item))
                {
                    item.Id = fbAdEntityIdStorage.GetEntityIdFromStorage(item);
                }
                else
                {
                    Console.WriteLine("Test stuff");
                }
            });
        }

        private bool UpdateExistingDbAdPropertiesIfNecessary(T existingDbAd, T latestAdFromApi)
        {
            return false;
        }

        private void AddMissedDbItems(List<T> itemsToBeAdded, object lockObject)
        {
            SafeContextWrapper.TryMakeTransactionWithLock<ClientPortalProgContext>(dbContext =>
            {
                dbContext.BulkInsert(itemsToBeAdded);
            }, lockObject, "BulkInserting");
        }

        private void UpdateOutdatedDbItems(List<T> itemsToBeUpdated)
        {
        }

        private void AddItemsToEntityIdStorage(List<T> campaigns, EntityIdStorage<T> fbAdEntityIdStorage)
        {
            campaigns.ForEach(c => { fbAdEntityIdStorage.AddEntityIdToStorage(c); });
        }
    }
}
