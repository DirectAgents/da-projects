using System.Collections.Generic;
using System.Linq;
using CakeExtracter.SimpleRepositories.BaseRepositories.Interfaces;
using DirectAgents.Domain.Entities.CPProg.Adform;
using DirectAgents.Domain.Entities.CPProg.Adform.Summaries;

namespace CakeExtracter.Etl.Adform.Loaders
{
    internal class AdfCampaignSummaryLoader : AdfBaseSummaryLoader<AdfCampaign, AdfCampaignSummary>
    {
        public AdfCampaignSummaryLoader(
            int accountId,
            IBaseRepository<AdfCampaign> entityRepository,
            IBaseRepository<AdfCampaignSummary> summaryRepository)
            : base(accountId, entityRepository, summaryRepository)
        {
        }

        public bool MergeItemsWithExisted(List<AdfCampaignSummary> items)
        {
            var entities = items.Select(x => x.Campaign).ToList();
            var result = MergeDependentCampaigns(entities);
            if (result)
            {
                result = MergeSummariesWithExisted(items);
            }
            return result;
        }

        public bool MergeDependentCampaigns(List<AdfCampaign> items)
        {
            return MergeDependentEntitiesWithExisted(items);
        }

        protected override int Load(List<AdfCampaignSummary> items)
        {
            Logger.Info(accountId, "Loading {0} AdfCampaignSummaries..", items.Count);
            var result = MergeItemsWithExisted(items);
            return result ? items.Count : 0;
        }

        protected override void SetEntityParents(AdfCampaign entity)
        {
            entity.AccountId = accountId;
        }

        protected override void SetSummaryParents(AdfCampaignSummary summary)
        {
            base.SetSummaryParents(summary);
            summary.EntityId = summary.Campaign.Id;
        }
    }
}
