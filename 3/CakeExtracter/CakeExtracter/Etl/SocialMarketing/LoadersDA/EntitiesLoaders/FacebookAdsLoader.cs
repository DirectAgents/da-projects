using CakeExtracter.Etl.SocialMarketing.EntitiesLoaders;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Entities.CPProg.Facebook.Ad;
using System.Collections.Generic;
using System.Linq;

namespace CakeExtracter.Etl.SocialMarketing.EntitiesStorage
{
    /// <summary>
    /// Insure existence of facebook ads, creatives, and validate it's metadata.
    /// </summary>
    public class FacebookAdsLoader : BaseFacebookEntityLoader<FbAd>
    {
        private readonly FacebookAdSetsLoader adSetsLoader;

        private readonly FacebookCampaignsLoader campaignsLoader;

        private readonly FacebookCreativesLoader creativesLoader;

        private static EntityIdStorage<FbAd> fbAdEntityIdStorage = new EntityIdStorage<FbAd>(x => x.Id,
            x => $"{x.AccountId} {x.ExternalId}");

        private static object lockObject = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookAdsLoader"/> class.
        /// </summary>
        /// <param name="adSetsLoader">The ad sets loader.</param>
        /// <param name="campaignsLoader">The campaigns loader.</param>
        /// <param name="creativesLoader">The creatives loader.</param>
        public FacebookAdsLoader(FacebookAdSetsLoader adSetsLoader, FacebookCampaignsLoader campaignsLoader, FacebookCreativesLoader creativesLoader)
        {
            this.adSetsLoader = adSetsLoader;
            this.campaignsLoader = campaignsLoader;
            this.creativesLoader = creativesLoader;
        }

        /// <summary>
        /// Adds or updates dependent entities.
        /// </summary>
        /// <param name="items">The items.</param>
        public void AddUpdateDependentEntities(List<FbAd> items)
        {
            var uniqueItems = items.GroupBy(item => item.ExternalId).Select(gr => gr.First()).ToList();
            EnsureCreativesData(uniqueItems);
            EnsureAdSetsData(uniqueItems);
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
        protected override bool UpdateExistingDbItemPropertiesIfNecessary(FbAd existingDbItem, FbAd latestItemFromApi)
        {
            if (existingDbItem.Name != latestItemFromApi.Name ||
                existingDbItem.Status != latestItemFromApi.Status ||
                existingDbItem.CampaignId != latestItemFromApi.CampaignId ||
                existingDbItem.AdSetId != latestItemFromApi.AdSetId ||
                existingDbItem.CreativeId != latestItemFromApi.CreativeId)
            {
                existingDbItem.Name = latestItemFromApi.Name;
                existingDbItem.CampaignId = latestItemFromApi.CampaignId;
                existingDbItem.AdSetId = latestItemFromApi.AdSetId;
                existingDbItem.CreativeId = latestItemFromApi.CreativeId;
                existingDbItem.Status = latestItemFromApi.Status;
                return true;
            }
            return false;
        }

        private void EnsureCampaignsData(List<FbAd> items)
        {
            var relatedCampaigns = items.Select(item => item.Campaign).ToList();
            campaignsLoader.AddUpdateDependentEntities(relatedCampaigns);
            items.ForEach(item => item.CampaignId = item.Campaign.Id);
            items.ForEach(item => item.AdSet.CampaignId = item.Campaign?.Id);
        }

        private void EnsureAdSetsData(List<FbAd> items)
        {
            var relatedAdSets = items.Select(item => item.AdSet).ToList();
            adSetsLoader.AddUpdateDependentEntities(relatedAdSets);
            items.ForEach(item => item.AdSetId = item.AdSet?.Id);
        }

        private void EnsureCreativesData(List<FbAd> items)
        {
            var relatedCreatives = items.Select(item => item.Creative).Where(item=>item!=null).ToList();
            creativesLoader.AddUpdateDependentEntities(relatedCreatives);
            items.ForEach(item => item.CreativeId = item.Creative?.Id);
        }
    }
}
