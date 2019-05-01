using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Entities.CPProg.YAM;
using DirectAgents.Domain.Entities.CPProg.YAM.Summaries;

namespace CakeExtracter.Etl.YAM.Loaders
{
    internal class YamLineSummaryLoader : Loader<YamLineSummary>
    {
        private static readonly MergeHelper LineMergeHelper = new MergeHelper
        {
            EntitiesName = "Lines",
            Locker = new object()
        };
        private static readonly MergeHelper LineSummariesMergeHelper = new MergeHelper
        {
            EntitiesName = "Line Summaries",
            Locker = new object()
        };

        private readonly BaseYamSummaryLoader baseLoader;
        private readonly YamCampaignSummaryLoader campaignLoader;

        public YamLineSummaryLoader(int accountId = -1) : base(accountId)
        {
            baseLoader = new BaseYamSummaryLoader(accountId);
            campaignLoader = new YamCampaignSummaryLoader(accountId);
        }

        protected override int Load(List<YamLineSummary> items)
        {
            Logger.Info(accountId, "Loading {0} YamLineSummaries..", items.Count);
            MergeItemsWithExisted(items);
            return items.Count;
        }

        public void MergeDependentLines(List<YamLine> items)
        {
            var campaigns = items.Select(x => x.Campaign).ToList();
            campaignLoader.MergeDependentCampaigns(campaigns);
            baseLoader.MergeDependentEntitiesWithExisted(items, LineMergeHelper, line => line.CampaignId = line.Campaign.Id);
        }

        private void MergeItemsWithExisted(List<YamLineSummary> items)
        {
            var entities = items.Select(x => x.Line).ToList();
            MergeDependentLines(entities);
            baseLoader.MergeSummariesWithExisted(items, LineSummariesMergeHelper, x => x.EntityId = x.Line.Id);
        }
    }
}
