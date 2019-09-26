using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Entities.CPProg.DBM.Entities;

namespace CakeExtracter.Etl.DBM.Loaders.EntitiesLoaders
{
    /// <inheritdoc />
    /// <summary>
    /// Loader for DBM creatives.
    /// </summary>
    public class DbmCreativeLoader : DbmBaseEntityLoader<DbmCreative>
    {
        /// <summary>
        /// Entity id storage of already updated entity.
        /// </summary>
        private static readonly EntityIdStorage<DbmCreative> CreativeIdStorage =
            new EntityIdStorage<DbmCreative>(x => x.Id, x => $"{x.Name} {x.ExternalId}");

        private static readonly object LockObject = new object();

        private readonly DbmAdvertiserLoader advertiserLoader;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbmCreativeLoader"/> class.
        /// </summary>
        /// <param name="advertiserLoader">Loader for advertisers.</param>
        public DbmCreativeLoader(DbmAdvertiserLoader advertiserLoader)
        {
            this.advertiserLoader = advertiserLoader;
        }

        /// <summary>
        /// Adds or updates dependent entities.
        /// </summary>
        /// <param name="items">The items.</param>
        public void AddUpdateDependentEntities(List<DbmCreative> items)
        {
            AssignAccountIdToItems(items);
            var uniqueItems = items.GroupBy(item => item.ExternalId).Select(gr => gr.First()).ToList();
            EnsureAdvertisersData(uniqueItems);
            AddUpdateDependentItems(uniqueItems, CreativeIdStorage, LockObject);
            AssignIdToItems(items, CreativeIdStorage);
        }

        /// <inheritdoc/>
        protected override bool UpdateExistingDbItemPropertiesIfNecessary(DbmCreative existingDbItem, DbmCreative latestItemFromApi)
        {
            if (existingDbItem.Name == latestItemFromApi.Name &&
                existingDbItem.AdvertiserId == latestItemFromApi.AdvertiserId &&
                existingDbItem.Height == latestItemFromApi.Height &&
                existingDbItem.Width == latestItemFromApi.Width &&
                existingDbItem.Size == latestItemFromApi.Size &&
                existingDbItem.Type == latestItemFromApi.Type)
            {
                return false;
            }
            existingDbItem.Name = latestItemFromApi.Name;
            existingDbItem.AdvertiserId = latestItemFromApi.AdvertiserId;
            existingDbItem.Height = latestItemFromApi.Height;
            existingDbItem.Width = latestItemFromApi.Width;
            existingDbItem.Size = latestItemFromApi.Size;
            existingDbItem.Type = latestItemFromApi.Type;
            return true;
        }

        private void EnsureAdvertisersData(List<DbmCreative> items)
        {
            var relatedAdvertisers = items.Select(item => item.Advertiser).Where(item => item != null).ToList();
            advertiserLoader.AddUpdateDependentEntities(relatedAdvertisers);
            items.ForEach(item => item.AdvertiserId = item.Advertiser?.Id);
        }
    }
}