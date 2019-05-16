using System.Collections.Generic;
using System.Linq;
using CakeExtracter.SimpleRepositories.BaseRepositories.Interfaces;
using DirectAgents.Domain.Entities.CPProg.YAM;
using DirectAgents.Domain.Entities.CPProg.YAM.Summaries;

namespace CakeExtracter.Etl.YAM.Loaders
{
    internal class YamCampaignSummaryLoader : BaseYamSummaryLoader<YamCampaign, YamCampaignSummary>
    {
        public YamCampaignSummaryLoader(int accountId, IBaseRepository<YamCampaign> entityRepository,
            IBaseRepository<YamCampaignSummary> summaryRepository)
            : base(accountId, entityRepository, summaryRepository)
        {
        }

        public bool MergeItemsWithExisted(List<YamCampaignSummary> items)
        {
            var entities = items.Select(x => x.Campaign).ToList();
            var result = MergeDependentCampaigns(entities);
            if (result)
            {
                result = MergeSummariesWithExisted(items);
            }

            return result;
        }

        public bool MergeDependentCampaigns(List<YamCampaign> items)
        {
            return MergeDependentEntitiesWithExisted(items);
        }

        protected override int Load(List<YamCampaignSummary> items)
        {
            Logger.Info(accountId, "Loading {0} YamCampaignSummaries..", items.Count);
            var result = MergeItemsWithExisted(items);
            return result ? items.Count : 0;
        }

        protected override void SetEntityParents(YamCampaign entity)
        {
            entity.AccountId = accountId;
        }

        protected override void SetSummaryParents(YamCampaignSummary summary)
        {
            summary.EntityId = summary.Campaign.Id;
        }
    }
}
