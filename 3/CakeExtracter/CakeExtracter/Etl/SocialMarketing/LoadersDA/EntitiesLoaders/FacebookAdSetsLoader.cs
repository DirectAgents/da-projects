using CakeExtracter.Helpers;
using DirectAgents.Domain.Entities.CPProg.Facebook.AdSet;
using System.Collections.Generic;
using System.Linq;

namespace CakeExtracter.Etl.SocialMarketing.EntitiesLoaders
{
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

        public void AddUpdateDependentEntities(List<FbAdSet> items)
        {
            var uniqueItems = items.GroupBy(item => item.ExternalId).Select(gr => gr.First()).ToList();
            EnsureCampaignsData(uniqueItems);
            AddUpdateDependentItems(uniqueItems, fbAdEntityIdStorage, lockObject);
            AssignIdToItems(items, fbAdEntityIdStorage);
        }

        private void EnsureCampaignsData(List<FbAdSet> items)
        {
            var relatedCampaigns = items.Select(item => item.Campaign).ToList();
            campaignsLoader.AddUpdateDependentEntities(relatedCampaigns);
            items.ForEach(item => item.CampaignId = item.Campaign.Id);
        }
    }
}
