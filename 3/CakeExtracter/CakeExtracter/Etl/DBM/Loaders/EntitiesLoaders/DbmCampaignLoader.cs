using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Entities.CPProg.DBM.Entities;

namespace CakeExtracter.Etl.DBM.Loaders.EntitiesLoaders
{
    public class DbmCampaignLoader : DbmBaseEntityLoader<DbmCampaign>
    {
        private readonly DbmAdvertiserLoader advertiserLoader;

        /// <summary>
        /// Entity id storage of already updated entity.
        /// </summary>
        private static readonly EntityIdStorage<DbmCampaign> campaignIdStorage = 
            new EntityIdStorage<DbmCampaign>(x => x.Id, x => $"{x.Name} {x.ExternalId}");

        private static readonly object lockObject = new object();

        public DbmCampaignLoader(DbmAdvertiserLoader advertiserLoader)
        {
            this.advertiserLoader = advertiserLoader;
        }

        /// <summary>
        /// Adds or updates dependent entities.
        /// </summary>
        /// <param name="items">The items.</param>
        public void AddUpdateDependentEntities(List<DbmCampaign> items)
        {
            var uniqueItems = items.GroupBy(item => item.ExternalId).Select(gr => gr.First()).ToList();
            EnsureAdvertisersData(uniqueItems);
            AddUpdateDependentItems(uniqueItems, campaignIdStorage, lockObject);
            AssignIdToItems(items, campaignIdStorage);
        }

        /// <summary>
        /// Updates the existing database item properties if necessary.
        /// </summary>
        /// <param name="existingDbItem">The existing database item.</param>
        /// <param name="latestItemFromApi">The latest item from API.</param>
        /// <returns></returns>
        protected override bool UpdateExistingDbItemPropertiesIfNecessary(DbmCampaign existingDbItem, DbmCampaign latestItemFromApi)
        {
            if (existingDbItem.Name == latestItemFromApi.Name &&
                existingDbItem.AdvertiserId == latestItemFromApi.AdvertiserId)
            {
                return false;
            }
            existingDbItem.Name = latestItemFromApi.Name;
            existingDbItem.AdvertiserId = latestItemFromApi.AdvertiserId;
            return true;
        }

        private void EnsureAdvertisersData(List<DbmCampaign> items)
        {
            var relatedAdvertisers = items.Select(item => item.Advertiser).Where(item => item != null).ToList();
            advertiserLoader.AddUpdateDependentEntities(relatedAdvertisers);
            items.ForEach(item => item.AdvertiserId = item.Advertiser?.Id);
        }
    }
}
