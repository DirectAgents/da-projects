using System.Collections.Generic;
using System.Linq;
using CakeExtracter.SimpleRepositories.BaseRepositories.Interfaces;
using DirectAgents.Domain.Entities.CPProg.Adform;
using DirectAgents.Domain.Entities.CPProg.Adform.Summaries;

namespace CakeExtracter.Etl.Adform.Loaders
{
    internal class AdfBannerSummaryLoader : AdfBaseSummaryLoader<AdfBanner, AdfBannerSummary>
    {
        private readonly AdfLineItemSummaryLoader lineItemLoader;

        public AdfBannerSummaryLoader(
            int accountId,
            IBaseRepository<AdfBanner> entityRepository,
            IBaseRepository<AdfBannerSummary> summaryRepository,
            AdfLineItemSummaryLoader lineItemLoader,
            AdfMediaTypeLoader mediaTypeLoader)
            : base(accountId, entityRepository, summaryRepository, mediaTypeLoader)
        {
            this.lineItemLoader = lineItemLoader;
        }

        public bool MergeItemsWithExisted(List<AdfBannerSummary> items)
        {
            var entities = items.Select(x => x.Banner).ToList();
            var result = MergeDependentBanners(entities);
            if (result)
            {
                result = MergeSummariesWithExisted(items);
            }
            return result;
        }

        public bool MergeDependentBanners(List<AdfBanner> items)
        {
            return MergeDependentEntitiesWithExisted(items);
        }

        protected override int Load(List<AdfBannerSummary> items)
        {
            Logger.Info(accountId, "Loading {0} AdfBannerSummaries..", items.Count);
            var result = MergeItemsWithExisted(items);
            return result ? items.Count : 0;
        }

        protected override void SetEntityParents(AdfBanner entity)
        {
            var dbLineItem = lineItemLoader.GetLineItemByExternalId(entity.LineItem.ExternalId);
            entity.LineItemId = dbLineItem.Id;
        }

        protected override void SetSummaryParents(AdfBannerSummary summary)
        {
            base.SetSummaryParents(summary);
            summary.EntityId = summary.Banner.Id;
        }
    }
}
