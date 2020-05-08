using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Commands;
using CakeExtracter.SimpleRepositories.BaseRepositories.Interfaces;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg.Adform;
using DirectAgents.Domain.Entities.CPProg.Adform.Summaries;

namespace CakeExtracter.Etl.Adform.Loaders
{
    /// <inheritdoc />
    /// <summary>
    /// Adform loader of Campaign level summaries.
    /// </summary>
    internal class AdfCampaignSummaryLoader : AdfBaseSummaryLoader<AdfCampaign, AdfCampaignSummary>
    {
        /// <inheritdoc cref="AdfBaseSummaryLoader{AdfCampaign, AdfCampaignSummary}"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="AdfCampaignSummaryLoader" /> class.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="entityRepository"></param>
        /// <param name="summaryRepository"></param>
        /// <param name="mediaTypeLoader"></param>
        public AdfCampaignSummaryLoader(
            int accountId,
            IBaseRepository<AdfCampaign> entityRepository,
            IBaseRepository<AdfCampaignSummary> summaryRepository,
            AdfMediaTypeLoader mediaTypeLoader)
            : base(accountId, entityRepository, summaryRepository, mediaTypeLoader)
        {
        }

        /// <summary>
        /// Merges (inserts or updates) dependent campaigns with existed in DB.
        /// </summary>
        /// <param name="items">Campaigns for merge.</param>
        /// <returns>True if successfully.</returns>
        public bool MergeDependentCampaigns(List<AdfCampaign> items)
        {
            return MergeDependentEntitiesWithExisted(items);
        }

        /// <summary>
        /// Finds existed campaign in DB by its external ID.
        /// </summary>
        /// <param name="externalId">Campaign external ID for searching.</param>
        /// <returns>Campaign from DB.</returns>
        public AdfCampaign GetCampaignByExternalId(string externalId)
        {
            using (var db = new ClientPortalProgContext())
            {
                return db.AdfCampaigns.FirstOrDefault(campaign => campaign.ExternalId == externalId);
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Merges (inserts or updates) Campaign summaries with existed in DB.
        /// </summary>
        /// <param name="items">Campaign summary items for merge.</param>
        /// <returns>True if successfully merged.</returns>
        protected override bool MergeItemsWithExisted(List<AdfCampaignSummary> items)
        {
            var entities = items.Select(x => x.Campaign).ToList();
            var result = MergeDependentCampaigns(entities);
            if (result)
            {
                result = MergeSummariesWithExisted(items);
            }
            return result;
        }

        /// <inheritdoc/>
        protected override int Load(List<AdfCampaignSummary> items)
        {
            Logger.Info(accountId, "Loading {0} Adform Campaign Summaries..", items.Count);
            try
            {
                var result = MergeItemsWithExisted(items);
                return result ? items.Count : 0;
            }
            catch (Exception e)
            {
                ProcessFailedStatsExtraction(e, items);
                return items.Count;
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Sets DB identifier of parent Account for Campaign.
        /// </summary>
        /// <param name="entity">Campaign for which the parent Account ID will be set.</param>
        protected override void SetEntityParents(AdfCampaign entity)
        {
            entity.AccountId = accountId;
        }

        /// <inheritdoc />
        /// <summary>
        /// Sets DB identifiers of parent Campaign and Media type for Campaign summary.
        /// </summary>
        /// <param name="summary">Campaign summary.</param>
        protected override void SetSummaryParents(AdfCampaignSummary summary)
        {
            base.SetSummaryParents(summary);
            summary.EntityId = summary.Campaign.Id;
        }

        private void ProcessFailedStatsExtraction(Exception e, List<AdfCampaignSummary> items)
        {
            Logger.Error(accountId, e);
            var exception = GetFailedStatsLoadingException(e, items, StatsTypeAgg.StrategyArg);
            InvokeProcessFailedExtractionHandlers(exception);
        }
    }
}
