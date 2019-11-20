using System.Collections.Generic;
using System.Linq;
using CakeExtracter.SimpleRepositories.BaseRepositories.Interfaces;
using DirectAgents.Domain.Entities.CPProg.Adform;
using DirectAgents.Domain.Entities.CPProg.Adform.Summaries;

namespace CakeExtracter.Etl.Adform.Loaders
{
    /// <inheritdoc />
    /// <summary>
    /// Adform loader of Line item level summaries.
    /// </summary>
    internal class AdfLineItemSummaryLoader : AdfBaseSummaryLoader<AdfLineItem, AdfLineItemSummary>
    {
        private readonly AdfCampaignSummaryLoader campaignLoader;

        /// <inheritdoc cref="AdfBaseSummaryLoader{AdfLineItem, AdfLineItemSummary}"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="AdfLineItemSummaryLoader" /> class.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="entityRepository"></param>
        /// <param name="summaryRepository"></param>
        /// <param name="campaignLoader">Campaign summary loader.</param>
        /// <param name="mediaTypeLoader"></param>
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

        /// <summary>
        /// Merges (inserts or updates) dependent line items with existed in DB.
        /// </summary>
        /// <param name="items">Line items for merge.</param>
        /// <returns>True if successfully.</returns>
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

        /// <inheritdoc />
        /// <summary>
        /// Merges (inserts or updates) Line item summaries with existed in DB.
        /// </summary>
        /// <param name="items">Line item summary items for merge.</param>
        /// <returns>True if successfully merged.</returns>
        protected override bool MergeItemsWithExisted(List<AdfLineItemSummary> items)
        {
            var entities = items.Select(x => x.LineItem).ToList();
            var result = MergeDependentLineItems(entities);
            if (result)
            {
                result = MergeSummariesWithExisted(items);
            }
            return result;
        }

        /// <inheritdoc/>
        protected override int Load(List<AdfLineItemSummary> items)
        {
            Logger.Info(accountId, "Loading {0} Adform LineItem Summaries..", items.Count);
            var result = MergeItemsWithExisted(items);
            return result ? items.Count : 0;
        }

        /// <inheritdoc />
        /// <summary>
        /// Sets DB identifier of parent Campaign for Line item.
        /// </summary>
        /// <param name="entity">Line item for which the parent Campaign ID will be set.</param>
        protected override void SetEntityParents(AdfLineItem entity)
        {
            entity.CampaignId = entity.Campaign.Id;
        }

        /// <inheritdoc />
        /// <summary>
        /// Sets DB identifiers of parent Line item and Media type for Line item summary.
        /// </summary>
        /// <param name="summary">Line item summary.</param>
        protected override void SetSummaryParents(AdfLineItemSummary summary)
        {
            base.SetSummaryParents(summary);
            summary.EntityId = summary.LineItem.Id;
        }
    }
}
