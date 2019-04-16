using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Etl.SocialMarketing.EntitiesLoaders;
using CakeExtracter.Etl.SocialMarketing.EntitiesStorage;
using CakeExtracter.Etl.TradingDesk.LoadersDA;
using DirectAgents.Domain.Entities.CPProg.Facebook.Ad;

namespace CakeExtracter.Etl.SocialMarketing.LoadersDA
{
    /// <summary>
    /// Facebook Ad Summary loader
    /// </summary>
    /// <seealso cref="CakeExtracter.Etl.Loader{DirectAgents.Domain.Entities.CPProg.Facebook.Ad.FbAdSummary}" />
    public class FacebookAdSummaryLoader : Loader<FbAdSummary>
    {
        private readonly TDadSummaryLoader tdAdSummaryLoader;

        private readonly FacebookAdsLoader fbAdsLoader;

        private readonly FacebookAdSetsLoader fbAdSetsLoader;

        private readonly FacebookCampaignsLoader fbCampaignsLoader;

        private readonly FacebookCreativesLoader fbCreativesLoader;

        public FacebookAdSummaryLoader(int accountId)
            : base(accountId)
        {
            fbCreativesLoader = new FacebookCreativesLoader();
            fbCampaignsLoader = new FacebookCampaignsLoader();
            fbAdSetsLoader = new FacebookAdSetsLoader(fbCampaignsLoader);
            fbAdsLoader = new FacebookAdsLoader(fbAdSetsLoader, fbCampaignsLoader, fbCreativesLoader);
        }

        protected override int Load(List<FbAdSummary> items)
        {
            EnsureAdEntitiesData(items);
            return items.Count;
        }

        private void EnsureAdEntitiesData(List<FbAdSummary> items)
        {
            var fbAds = items.Select(item => item.Ad).ToList();
            fbAdsLoader.AddUpdateDependentEntities(fbAds);
            items.ForEach(item => item.AdId = item.Ad.Id);
        }
    }
}
