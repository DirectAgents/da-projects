using System.Collections.Generic;
using System.Linq;
using CakeExtracter.SimpleRepositories.BaseRepositories.Interfaces;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg.Adform;
using DirectAgents.Domain.Entities.CPProg.Adform.Summaries;

namespace CakeExtracter.Etl.Adform.Loaders
{
    internal class AdfCampaignSummaryLoader : AdfBaseSummaryLoader<AdfCampaign, AdfCampaignSummary>
    {
        public AdfCampaignSummaryLoader(
            int accountId,
            IBaseRepository<AdfCampaign> entityRepository,
            IBaseRepository<AdfCampaignSummary> summaryRepository,
            AdfMediaTypeLoader mediaTypeLoader)
            : base(accountId, entityRepository, summaryRepository, mediaTypeLoader)
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

        public AdfCampaign GetCampaignByExternalId(string externalId)
        {
            using (var db = new ClientPortalProgContext())
            {
                return db.AdfCampaigns.FirstOrDefault(campaign => campaign.ExternalId == externalId);
            }
        }

        protected override int Load(List<AdfCampaignSummary> items)
        {
            Logger.Info(accountId, "Loading {0} Adform Campaign Summaries..", items.Count);
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
