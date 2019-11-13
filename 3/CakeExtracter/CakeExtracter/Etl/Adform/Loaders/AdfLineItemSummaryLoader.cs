using System.Collections.Generic;
using System.Linq;
using CakeExtracter.SimpleRepositories.BaseRepositories.Interfaces;
using DirectAgents.Domain.Entities.CPProg.Adform;
using DirectAgents.Domain.Entities.CPProg.Adform.Summaries;

namespace CakeExtracter.Etl.Adform.Loaders
{
    internal class AdfLineItemSummaryLoader : AdfBaseSummaryLoader<AdfLineItem, AdfLineItemSummary>
    {
        private readonly AdfCampaignSummaryLoader campaignLoader;

        public AdfLineItemSummaryLoader(
            int accountId,
            IBaseRepository<AdfLineItem> entityRepository,
            IBaseRepository<AdfLineItemSummary> summaryRepository,
            AdfCampaignSummaryLoader campaignLoader)
            : base(accountId, entityRepository, summaryRepository)
        {
            this.campaignLoader = campaignLoader;
        }

        public bool MergeItemsWithExisted(List<AdfLineItemSummary> items)
        {
            var entities = items.Select(x => x.LineItem).ToList();
            var result = MergeDependentLineItems(entities);
            if (result)
            {
                result = MergeSummariesWithExisted(items);
            }

            return result;
        }

        public bool MergeDependentLineItems(List<AdfLineItem> items)
        {
            var campaigns = items.Select(x => x.Campaign).ToList();
            var result = campaignLoader.MergeDependentCampaigns(campaigns);
            if (result)
            {
                result = MergeDependentEntitiesWithExisted(items);
            }
            return result;
        }

        protected override int Load(List<AdfLineItemSummary> items)
        {
            Logger.Info(accountId, "Loading {0} AdfLineItemSummaries..", items.Count);
            var result = MergeItemsWithExisted(items);
            return result ? items.Count : 0;
        }

        protected override void SetEntityParents(AdfLineItem entity)
        {
            entity.CampaignId = entity.Campaign.Id;
        }

        protected override void SetSummaryParents(AdfLineItemSummary summary)
        {
            summary.EntityId = summary.LineItem.Id;
        }
    }
}
