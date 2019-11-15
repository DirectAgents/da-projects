using System.Collections.Generic;
using System.Linq;
using CakeExtracter.SimpleRepositories.BaseRepositories.Interfaces;
using DirectAgents.Domain.Contexts;
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
            AdfCampaignSummaryLoader campaignLoader,
            AdfMediaTypeLoader mediaTypeLoader)
            : base(accountId, entityRepository, summaryRepository, mediaTypeLoader)
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
            return MergeDependentEntitiesWithExisted(items);
        }

        public AdfLineItem GetLineItemByExternalId(string externalId)
        {
            using (var db = new ClientPortalProgContext())
            {
                return db.AdfLineItems.FirstOrDefault(lineItem => lineItem.ExternalId == externalId);
            }
        }

        protected override int Load(List<AdfLineItemSummary> items)
        {
            Logger.Info(accountId, "Loading {0} AdfLineItemSummaries..", items.Count);
            var result = MergeItemsWithExisted(items);
            return result ? items.Count : 0;
        }

        protected override void SetEntityParents(AdfLineItem entity)
        {
            var dbCampaign = campaignLoader.GetCampaignByExternalId(entity.Campaign.ExternalId);
            entity.CampaignId = dbCampaign.Id;
        }

        protected override void SetSummaryParents(AdfLineItemSummary summary)
        {
            base.SetSummaryParents(summary);
            summary.EntityId = summary.LineItem.Id;
        }
    }
}
