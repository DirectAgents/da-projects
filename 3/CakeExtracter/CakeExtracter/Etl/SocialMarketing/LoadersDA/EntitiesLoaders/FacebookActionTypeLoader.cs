using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg.Facebook;
using System.Collections.Generic;
using System.Linq;

namespace CakeExtracter.Etl.SocialMarketing.LoadersDA.EntitiesLoaders
{
    public class FacebookActionTypeLoader
    {
        private static EntityIdStorage<FbActionType> fbActionTypesEntityIdStorage = new EntityIdStorage<FbActionType>(x => x.Id,
          x => $"{x.Code}");

        private static object actionTypesLock = new object();

        public void EnsureActionType(List<FbActionType> actionTypes)
        {
            var uniqueItems = actionTypes.GroupBy(item => item.Code).Select(gr => gr.First()).ToList();
            AddMissedActionTypes(uniqueItems);
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
