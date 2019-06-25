using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg.Facebook;

namespace CakeExtracter.Etl.Facebook.Loaders.EntitiesLoaders
{
    /// <summary>
    /// Facebook action types loader.
    /// </summary>
    public class FacebookActionTypeLoader
    {
        private static EntityIdStorage<FbActionType> fbActionTypesEntityIdStorage = new EntityIdStorage<FbActionType>(x => x.Id,
          x => $"{x.Code}");

        private static object actionTypesLock = new object();

        /// <summary>
        /// Ensures the type of the action entity in database.
        /// </summary>
        /// <param name="actionTypes">The action types.</param>
        public void EnsureActionType(List<FbActionType> actionTypes)
        {
            var uniqueItems = actionTypes.GroupBy(item => item.Code).Select(gr => gr.First()).ToList();
            lock (actionTypesLock)
            {
                AddMissedActionTypes(uniqueItems);
            }
            AssignIdToItems(actionTypes);
        }

        private void AddMissedActionTypes(List<FbActionType> items)
        {
            using (var db = new ClientPortalProgContext())
            {
                var itemsToBeAdded = new List<FbActionType>();
                var itemsToProcess = items.Where(item => !fbActionTypesEntityIdStorage.IsEntityInStorage(item)).ToList();
                foreach (var item in itemsToProcess)
                {
                    var existingItem = db.FbActionTypes.FirstOrDefault(at => at.Code == item.Code);
                    if (existingItem == null)
                    {
                        itemsToBeAdded.Add(item);
                    }
                    else
                    {
                        item.Id = existingItem.Id;
                    }
                }
                AddMissedDbItems(itemsToBeAdded);
                AddItemsToEntityIdStorage(itemsToProcess);
            }
        }

        private void AssignIdToItems(List<FbActionType> items)
        {
            items.ForEach(item =>
            {
                if (fbActionTypesEntityIdStorage.IsEntityInStorage(item))
                {
                    item.Id = fbActionTypesEntityIdStorage.GetEntityIdFromStorage(item);
                }
            });
        }

        private void AddMissedDbItems(List<FbActionType> itemsToBeAdded)
        {
            SafeContextWrapper.TryMakeTransactionWithLock<ClientPortalProgContext>(dbContext =>
            {
                dbContext.BulkInsert(itemsToBeAdded);
            }, actionTypesLock, "BulkInserting");
        }

        private void AddItemsToEntityIdStorage(List<FbActionType> items)
        {
            items.ForEach(c => { fbActionTypesEntityIdStorage.AddEntityIdToStorage(c); });
        }
    }
}
