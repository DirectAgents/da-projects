using CakeExtracter.Helpers;
using DirectAgents.Domain.Entities.CPProg.Facebook.Campaign;
using System.Collections.Generic;
using System.Linq;

namespace CakeExtracter.Etl.SocialMarketing.EntitiesLoaders
{
    /// <summary>
    /// Facebook Campaigns Loader
    /// </summary>
    public class FacebookCampaignsLoader : BaseFacebookEntityLoader<FbCampaign>
    {
        /// <summary>
        /// Entity id storage of already updated ads.
        /// </summary>
        private static EntityIdStorage<FbCampaign> fbAdEntityIdStorage = new EntityIdStorage<FbCampaign>(x => x.Id,
            x => $"{x.AccountId} {x.ExternalId}");

        private static object lockObject = new object();

        public void AddUpdateDependentEntities(List<FbCampaign> items)
        {
            var uniqueItems = items.GroupBy(item => item.ExternalId).Select(gr => gr.First()).ToList();
            AddUpdateDependentItems(uniqueItems, fbAdEntityIdStorage, lockObject);
            AssignIdToItems(items, fbAdEntityIdStorage);
        }

        protected override bool UpdateExistingDbItemPropertiesIfNecessary(FbCampaign existingDbItem, FbCampaign latestItemFromApi)
        {
            if (existingDbItem.Name != latestItemFromApi.Name)
            {
                existingDbItem.Name = latestItemFromApi.Name;
                return true;
            }
            return false;
        }
    }
}
