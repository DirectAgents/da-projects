using CakeExtracter.Helpers;
using DirectAgents.Domain.Entities.CPProg.Facebook.AdSet;
using System.Collections.Generic;
using System.Linq;

namespace CakeExtracter.Etl.SocialMarketing.EntitiesLoaders
{
    /// <summary>
    /// Facebook adsets entities loader.
    /// </summary>
    /// <seealso cref="CakeExtracter.Etl.SocialMarketing.EntitiesLoaders.BaseFacebookEntityLoader{DirectAgents.Domain.Entities.CPProg.Facebook.AdSet.FbAdSet}" />
    public class FacebookAdSetsLoader : BaseFacebookEntityLoader<FbAdSet>
    {
        private readonly FacebookCampaignsLoader campaignsLoader;

        public FacebookAdSetsLoader(FacebookCampaignsLoader campaignsLoader)
        {
            this.campaignsLoader = campaignsLoader;
        }

        /// <summary>
        /// Entity id storage of already updated ads.
        /// </summary>
        private static EntityIdStorage<FbAdSet> fbAdEntityIdStorage = new EntityIdStorage<FbAdSet>(x => x.Id,
            x => $"{x.AccountId} {x.ExternalId}");

        private static object lockObject = new object();

        /// <summary>
        /// Adds or updates dependent entities.
        /// </summary>
        /// <param name="items">The items.</param>
        public void AddUpdateDependentEntities(List<FbAdSet> items)
        {
            var uniqueItems = items.GroupBy(item => item.ExternalId).Select(gr => gr.First()).ToList();
            EnsureCampaignsData(uniqueItems);
            AddUpdateDependentItems(uniqueItems, fbAdEntityIdStorage, lockObject);
            AssignIdToItems(items, fbAdEntityIdStorage);
        }

        /// <summary>
        /// Updates the existing database item properties if necessary.
        /// </summary>
        /// <param name="existingDbItem">The existing database item.</param>
        /// <param name="latestItemFromApi">The latest item from API.</param>
        /// <returns></returns>
        protected override bool UpdateExistingDbItemPropertiesIfNecessary(FbAdSet existingDbItem, FbAdSet latestItemFromApi)
        {
            if (existingDbItem.Name != latestItemFromApi.Name || existingDbItem.CampaignId != latestItemFromApi.CampaignId)
            {
                existingDbItem.Name = latestItemFromApi.Name;
                existingDbItem.CampaignId = latestItemFromApi.CampaignId;
                return true;
            }
            return false;
        }

        private void EnsureCampaignsData(List<FbAdSet> items)
        {
            var relatedCampaigns = items.Select(item => item.Campaign).Where(item => item != null).ToList();
            campaignsLoader.AddUpdateDependentEntities(relatedCampaigns);
            items.ForEach(item => item.CampaignId = item.Campaign?.Id);
        }
    }
}
