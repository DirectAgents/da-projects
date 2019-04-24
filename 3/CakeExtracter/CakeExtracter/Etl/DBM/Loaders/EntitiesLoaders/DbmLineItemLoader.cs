using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Entities.CPProg.DBM.Entities;

namespace CakeExtracter.Etl.DBM.Loaders.EntitiesLoaders
{
    public class DbmLineItemLoader : DbmBaseEntityLoader<DbmLineItem>
    {
        private readonly DbmAdvertiserLoader advertiserLoader;
        private readonly DbmCampaignLoader campaignLoader;
        private readonly DbmInsertionOrderLoader insertionOrderLoader;

        /// <summary>
        /// Entity id storage of already updated entity.
        /// </summary>
        private static readonly EntityIdStorage<DbmLineItem> lineItemIdStorage =
            new EntityIdStorage<DbmLineItem>(x => x.Id, x => $"{x.Name} {x.ExternalId}");

        private static readonly object lockObject = new object();

        public DbmLineItemLoader(DbmAdvertiserLoader advertiserLoader, DbmCampaignLoader campaignLoader,
            DbmInsertionOrderLoader insertionOrderLoader)
        {
            this.advertiserLoader = advertiserLoader;
            this.campaignLoader = campaignLoader;
            this.insertionOrderLoader = insertionOrderLoader;
        }

        /// <summary>
        /// Adds or updates dependent entities.
        /// </summary>
        /// <param name="items">The items.</param>
        public void AddUpdateDependentEntities(List<DbmLineItem> items)
        {
            var uniqueItems = items.GroupBy(item => item.ExternalId).Select(gr => gr.First()).ToList();
            EnsureInsertionOrdersData(uniqueItems);
            //EnsureCampaignsData(uniqueItems);
            //EnsureAdvertisersData(uniqueItems);
            AddUpdateDependentItems(uniqueItems, lineItemIdStorage, lockObject);
            AssignIdToItems(items, lineItemIdStorage);
        }

        /// <summary>
        /// Updates the existing database item properties if necessary.
        /// </summary>
        /// <param name="existingDbItem">The existing database item.</param>
        /// <param name="latestItemFromApi">The latest item from API.</param>
        /// <returns></returns>
        protected override bool UpdateExistingDbItemPropertiesIfNecessary(DbmLineItem existingDbItem, DbmLineItem latestItemFromApi)
        {
            if (existingDbItem.Name == latestItemFromApi.Name &&
                existingDbItem.InsertionOrderId == latestItemFromApi.InsertionOrderId &&
                existingDbItem.Status == latestItemFromApi.Status &&
                existingDbItem.Type == latestItemFromApi.Type)
            {
                return false;
            }
            existingDbItem.Name = latestItemFromApi.Name;
            existingDbItem.InsertionOrderId = latestItemFromApi.InsertionOrderId;
            existingDbItem.Status = latestItemFromApi.Status;
            existingDbItem.Type = latestItemFromApi.Type;
            return true;
        }

        private void EnsureInsertionOrdersData(List<DbmLineItem> items)
        {
            var relatedInsOrders = items.Select(item => item.InsertionOrder).Where(item => item != null).ToList();
            insertionOrderLoader.AddUpdateDependentEntities(relatedInsOrders);
            items.ForEach(item => item.InsertionOrderId = item.InsertionOrder?.Id);
        }

        private void EnsureCampaignsData(List<DbmLineItem> items)
        {
            var relatedCampaigns = items.Select(item => item.InsertionOrder.Campaign).Where(item => item != null).ToList();
            campaignLoader.AddUpdateDependentEntities(relatedCampaigns);
            items.ForEach(item => item.InsertionOrder.CampaignId = item.InsertionOrder.Campaign?.Id);
        }

        private void EnsureAdvertisersData(List<DbmLineItem> items)
        {
            var relatedAdvertisers = items.Select(item => item.InsertionOrder.Campaign?.Advertiser).Where(item => item != null).ToList();
            advertiserLoader.AddUpdateDependentEntities(relatedAdvertisers);
            items.ForEach(item => item.InsertionOrder.Campaign.AdvertiserId = item.InsertionOrder.Campaign.Advertiser?.Id);
        }
    }
}
