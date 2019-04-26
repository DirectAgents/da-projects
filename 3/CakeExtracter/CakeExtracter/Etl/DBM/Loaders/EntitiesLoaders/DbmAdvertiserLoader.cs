using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Entities.CPProg.DBM.Entities;

namespace CakeExtracter.Etl.DBM.Loaders.EntitiesLoaders
{
    public class DbmAdvertiserLoader : DbmBaseEntityLoader<DbmAdvertiser>
    {
        /// <summary>
        /// Entity id storage of already updated entity.
        /// </summary>
        private static readonly EntityIdStorage<DbmAdvertiser> advertiserEntityIdStorage =
            new EntityIdStorage<DbmAdvertiser>(x => x.Id, x => $"{x.Name} {x.ExternalId}");

        private static readonly object lockObject = new object();

        /// <summary>
        /// Adds or updates dependent entities.
        /// </summary>
        /// <param name="items">The items.</param>
        public void AddUpdateDependentEntities(List<DbmAdvertiser> items)
        {
            AssignAccountIdToItems(items);
            var uniqueItems = items.GroupBy(item => item.ExternalId).Select(gr => gr.First()).ToList();
            AddUpdateDependentItems(uniqueItems, advertiserEntityIdStorage, lockObject);
            AssignIdToItems(items, advertiserEntityIdStorage);
        }

        /// <summary>
        /// Updates the existing database item properties if necessary.
        /// </summary>
        /// <param name="existingDbItem">The existing database item.</param>
        /// <param name="latestItemFromApi">The latest item from API.</param>
        /// <returns></returns>
        protected override bool UpdateExistingDbItemPropertiesIfNecessary(DbmAdvertiser existingDbItem, DbmAdvertiser latestItemFromApi)
        {
            if (existingDbItem.Name == latestItemFromApi.Name &&
                existingDbItem.AccountId == latestItemFromApi.AccountId)
            {
                return false;
            }
            existingDbItem.Name = latestItemFromApi.Name;
            existingDbItem.AccountId = latestItemFromApi.AccountId;
            return true;
        }
    }
}
