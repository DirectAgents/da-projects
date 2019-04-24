using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Entities.CPProg.DBM.Entities;

namespace CakeExtracter.Etl.DBM.Loaders.EntitiesLoaders
{
    public class DbmCreativeLoader : DbmBaseEntityLoader<DbmCreative>
    {
        private readonly DbmAdvertiserLoader advertiserLoader;
        private readonly DbmCampaignLoader campaignLoader;
        private readonly DbmInsertionOrderLoader insertionOrderLoader;
        private readonly DbmLineItemLoader lineItemLoader;

        /// <summary>
        /// Entity id storage of already updated entity.
        /// </summary>
        private static readonly EntityIdStorage<DbmCreative> creativeIdStorage =
            new EntityIdStorage<DbmCreative>(x => x.Id, x => $"{x.Name} {x.ExternalId}");

        private static readonly object lockObject = new object();

        public DbmCreativeLoader(DbmAdvertiserLoader advertiserLoader, DbmCampaignLoader campaignLoader,
            DbmInsertionOrderLoader insertionOrderLoader, DbmLineItemLoader lineItemLoader)
        {
            this.advertiserLoader = advertiserLoader;
            this.campaignLoader = campaignLoader;
            this.insertionOrderLoader = insertionOrderLoader;
            this.lineItemLoader = lineItemLoader;
        }

        /// <summary>
        /// Adds or updates dependent entities.
        /// </summary>
        /// <param name="items">The items.</param>
        public void AddUpdateDependentEntities(List<DbmCreative> items)
        {
            var uniqueItems = items.GroupBy(item => item.ExternalId).Select(gr => gr.First()).ToList();
            EnsureLineItemsData(uniqueItems);
            //EnsureInsertionOrdersData(uniqueItems);
            //EnsureCampaignsData(uniqueItems);
            //EnsureAdvertisersData(uniqueItems);
            AddUpdateDependentItems(uniqueItems, creativeIdStorage, lockObject);
            AssignIdToItems(items, creativeIdStorage);
        }

        /// <summary>
        /// Updates the existing database item properties if necessary.
        /// </summary>
        /// <param name="existingDbItem">The existing database item.</param>
        /// <param name="latestItemFromApi">The latest item from API.</param>
        /// <returns></returns>
        protected override bool UpdateExistingDbItemPropertiesIfNecessary(DbmCreative existingDbItem, DbmCreative latestItemFromApi)
        {
            if (existingDbItem.Name == latestItemFromApi.Name &&
                existingDbItem.LineItemId == latestItemFromApi.LineItemId &&
                existingDbItem.Height == latestItemFromApi.Height &&
                existingDbItem.Width == latestItemFromApi.Width &&
                existingDbItem.Size == latestItemFromApi.Size &&
                existingDbItem.Type == latestItemFromApi.Type)
            {
                return false;
            }
            existingDbItem.Name = latestItemFromApi.Name;
            existingDbItem.LineItemId = latestItemFromApi.LineItemId;
            existingDbItem.Height = latestItemFromApi.Height;
            existingDbItem.Width = latestItemFromApi.Width;
            existingDbItem.Size = latestItemFromApi.Size;
            existingDbItem.Type = latestItemFromApi.Type;
            return true;
        }

        private void EnsureLineItemsData(List<DbmCreative> items)
        {
            var relatedLineItems = items.Select(item => item.LineItem).Where(item => item != null).ToList();
            lineItemLoader.AddUpdateDependentEntities(relatedLineItems);
            items.ForEach(item => item.LineItemId = item.LineItem?.Id);
        }

        private void EnsureInsertionOrdersData(List<DbmCreative> items)
        {
            var relatedInsOrders = items.Select(item => item.LineItem.InsertionOrder).Where(item => item != null).ToList();
            insertionOrderLoader.AddUpdateDependentEntities(relatedInsOrders);
            items.ForEach(item => item.LineItem.InsertionOrderId = item.LineItem.InsertionOrder?.Id);
        }

        private void EnsureCampaignsData(List<DbmCreative> items)
        {
            var relatedCampaigns = items.Select(item => item.LineItem.InsertionOrder.Campaign).Where(item => item != null).ToList();
            campaignLoader.AddUpdateDependentEntities(relatedCampaigns);
            items.ForEach(item => item.LineItem.InsertionOrder.CampaignId = item.LineItem.InsertionOrder.Campaign?.Id);
        }

        private void EnsureAdvertisersData(List<DbmCreative> items)
        {
            var relatedAdvertisers = items.Select(item => item.LineItem.InsertionOrder.Campaign?.Advertiser).Where(item => item != null).ToList();
            advertiserLoader.AddUpdateDependentEntities(relatedAdvertisers);
            items.ForEach(item => item.LineItem.InsertionOrder.Campaign.AdvertiserId = item.LineItem.InsertionOrder.Campaign.Advertiser?.Id);
        }
    }
}
