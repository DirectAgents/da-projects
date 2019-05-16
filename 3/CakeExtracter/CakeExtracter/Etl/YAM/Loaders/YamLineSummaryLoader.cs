using System.Collections.Generic;
using System.Linq;
using CakeExtracter.SimpleRepositories.BaseRepositories.Interfaces;
using DirectAgents.Domain.Entities.CPProg.YAM;
using DirectAgents.Domain.Entities.CPProg.YAM.Summaries;

namespace CakeExtracter.Etl.YAM.Loaders
{
    internal class YamLineSummaryLoader : BaseYamSummaryLoader<YamLine, YamLineSummary>
    {
        private readonly YamCampaignSummaryLoader campaignLoader;

        public YamLineSummaryLoader(int accountId, IBaseRepository<YamLine> entityRepository,
            IBaseRepository<YamLineSummary> summaryRepository, YamCampaignSummaryLoader campaignLoader)
            : base(accountId, entityRepository, summaryRepository)
        {
            this.campaignLoader = campaignLoader;
        }

        public bool MergeItemsWithExisted(List<YamLineSummary> items)
        {
            var entities = items.Select(x => x.Line).ToList();
            var result = MergeDependentLines(entities);
            if (result)
            {
                result = MergeSummariesWithExisted(items);
            }

            return result;
        }

        public bool MergeDependentLines(List<YamLine> items)
        {
            var campaigns = items.Select(x => x.Campaign).ToList();
            var result = campaignLoader.MergeDependentCampaigns(campaigns);
            if (result)
            {
                result = MergeDependentEntitiesWithExisted(items);
            }

            return result;
        }

        protected override int Load(List<YamLineSummary> items)
        {
            Logger.Info(accountId, "Loading {0} YamLineSummaries..", items.Count);
            var result = MergeItemsWithExisted(items);
            return result ? items.Count : 0;
        }

        protected override void SetEntityParents(YamLine entity)
        {
            entity.CampaignId = entity.Campaign.Id;
        }

        protected override void SetSummaryParents(YamLineSummary summary)
        {
            summary.EntityId = summary.Line.Id;
        }
    }
}
