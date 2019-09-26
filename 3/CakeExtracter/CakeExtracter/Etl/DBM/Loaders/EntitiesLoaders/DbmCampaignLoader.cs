using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Entities.CPProg.DBM.Entities;

namespace CakeExtracter.Etl.DBM.Loaders.EntitiesLoaders
{
    /// <inheritdoc />
    /// <summary>
    /// Loader for DBM campaigns.
    /// </summary>
    public class DbmCampaignLoader : DbmBaseEntityLoader<DbmCampaign>
    {
        /// <summary>
        /// Entity id storage of already updated entity.
        /// </summary>
        private static readonly EntityIdStorage<DbmCampaign> CampaignIdStorage =
            new EntityIdStorage<DbmCampaign>(x => x.Id, x => $"{x.Name} {x.ExternalId}");

        private static readonly object LockObject = new object();

        private readonly DbmAdvertiserLoader advertiserLoader;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbmCampaignLoader"/> class.
        /// </summary>
        /// <param name="advertiserLoader">Loader for advertisers.</param>
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
            AssignAccountIdToItems(items);
            var uniqueItems = items.GroupBy(item => item.ExternalId).Select(gr => gr.First()).ToList();
            EnsureAdvertisersData(uniqueItems);
            AddUpdateDependentItems(uniqueItems, CampaignIdStorage, LockObject);
            AssignIdToItems(items, CampaignIdStorage);
        }

        /// <inheritdoc />
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