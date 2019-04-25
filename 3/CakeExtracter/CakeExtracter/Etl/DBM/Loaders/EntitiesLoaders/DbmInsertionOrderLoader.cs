using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Entities.CPProg.DBM.Entities;

namespace CakeExtracter.Etl.DBM.Loaders.EntitiesLoaders
{
    public class DbmInsertionOrderLoader : DbmBaseEntityLoader<DbmInsertionOrder>
    {
        private readonly DbmCampaignLoader campaignLoader;

        /// <summary>
        /// Entity id storage of already updated entity.
        /// </summary>
        private static readonly EntityIdStorage<DbmInsertionOrder> insertionOrderIdStorage =
            new EntityIdStorage<DbmInsertionOrder>(x => x.Id, x => $"{x.Name} {x.ExternalId}");

        private static readonly object lockObject = new object();

        public DbmInsertionOrderLoader(DbmCampaignLoader campaignLoader)
        {
            this.campaignLoader = campaignLoader;
        }

        /// <summary>
        /// Adds or updates dependent entities.
        /// </summary>
        /// <param name="items">The items.</param>
        public void AddUpdateDependentEntities(List<DbmInsertionOrder> items)
        {
            var uniqueItems = items.GroupBy(item => item.ExternalId).Select(gr => gr.First()).ToList();
            EnsureCampaignsData(uniqueItems);
            AddUpdateDependentItems(uniqueItems, insertionOrderIdStorage, lockObject);
            AssignIdToItems(items, insertionOrderIdStorage);
        }

        /// <summary>
        /// Updates the existing database item properties if necessary.
        /// </summary>
        /// <param name="existingDbItem">The existing database item.</param>
        /// <param name="latestItemFromApi">The latest item from API.</param>
        /// <returns></returns>
        protected override bool UpdateExistingDbItemPropertiesIfNecessary(DbmInsertionOrder existingDbItem, DbmInsertionOrder latestItemFromApi)
        {
            if (existingDbItem.Name == latestItemFromApi.Name &&
                existingDbItem.CampaignId == latestItemFromApi.CampaignId)
            {
                return false;
            }
            existingDbItem.Name = latestItemFromApi.Name;
            existingDbItem.CampaignId = latestItemFromApi.CampaignId;
            return true;
        }

        private void EnsureCampaignsData(List<DbmInsertionOrder> items)
        {
            var relatedCampaigns = items.Select(item => item.Campaign).Where(item => item != null).ToList();
            campaignLoader.AddUpdateDependentEntities(relatedCampaigns);
            items.ForEach(item => item.CampaignId = item.Campaign?.Id);
        }
    }
}
