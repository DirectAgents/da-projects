using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Entities.CPProg.DBM.Entities;

namespace CakeExtracter.Etl.DBM.Loaders.EntitiesLoaders
{
    /// <inheritdoc />
    /// <summary>
    /// Loader for DBM insertion orders.
    /// </summary>
    public class DbmInsertionOrderLoader : DbmBaseEntityLoader<DbmInsertionOrder>
    {
        /// <summary>
        /// Entity id storage of already updated entity.
        /// </summary>
        private static readonly EntityIdStorage<DbmInsertionOrder> InsertionOrderIdStorage =
            new EntityIdStorage<DbmInsertionOrder>(x => x.Id, x => $"{x.Name} {x.ExternalId}");

        private static readonly object LockObject = new object();

        private readonly DbmCampaignLoader campaignLoader;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbmInsertionOrderLoader"/> class.
        /// </summary>
        /// <param name="campaignLoader">Loader for campaigns.</param>
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
            AssignAccountIdToItems(items);
            var uniqueItems = items.GroupBy(item => item.ExternalId).Select(gr => gr.First()).ToList();
            EnsureCampaignsData(uniqueItems);
            AddUpdateDependentItems(uniqueItems, InsertionOrderIdStorage, LockObject);
            AssignIdToItems(items, InsertionOrderIdStorage);
        }

        /// <inheritdoc/>
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