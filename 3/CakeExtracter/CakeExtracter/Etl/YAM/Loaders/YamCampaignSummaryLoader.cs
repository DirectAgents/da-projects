using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Entities.CPProg.YAM;
using DirectAgents.Domain.Entities.CPProg.YAM.Summaries;

namespace CakeExtracter.Etl.YAM.Loaders
{
    internal class YamCampaignSummaryLoader : Loader<YamCampaignSummary>
    {
        private static readonly MergeHelper CampaignMergeHelper = new MergeHelper
        {
            EntitiesName = "Campaigns",
            Locker = new object()
        };
        private static readonly MergeHelper CampaignSummariesMergeHelper = new MergeHelper
        {
            EntitiesName = "Campaign Summaries",
            Locker = new object()
        };

        private readonly BaseYamSummaryLoader baseLoader;

        public YamCampaignSummaryLoader(int accountId = -1) : base(accountId)
        {
            baseLoader = new BaseYamSummaryLoader(accountId);
        }

        protected override int Load(List<YamCampaignSummary> items)
        {
            Logger.Info(accountId, "Loading {0} YamCampaignSummaries..", items.Count);
            MergeItemsWithExisted(items);
            return items.Count;
        }

        public void MergeDependentCampaigns(List<YamCampaign> items)
        {
            baseLoader.MergeDependentEntitiesWithExisted(items, CampaignMergeHelper, campaign => campaign.AccountId = accountId);
        }

        private void MergeItemsWithExisted(List<YamCampaignSummary> items)
        {
            var entities = items.Select(x => x.Campaign).ToList();
            MergeDependentCampaigns(entities);
            baseLoader.MergeSummariesWithExisted(items, CampaignSummariesMergeHelper, x => x.EntityId = x.Campaign.Id);
        }
    }
}
