using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Commands;
using CakeExtracter.SimpleRepositories.BaseRepositories.Interfaces;
using DirectAgents.Domain.Entities.CPProg.Adform;
using DirectAgents.Domain.Entities.CPProg.Adform.Summaries;

namespace CakeExtracter.Etl.Adform.Loaders
{
    /// <inheritdoc />
    /// <summary>
    /// Adform loader of Banner level summaries.
    /// </summary>
    internal class AdfBannerSummaryLoader : AdfBaseSummaryLoader<AdfBanner, AdfBannerSummary>
    {
        /// <inheritdoc cref="AdfBaseSummaryLoader{AdfBanner, AdfBannerSummary}"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="AdfBannerSummaryLoader" /> class.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="entityRepository"></param>
        /// <param name="summaryRepository"></param>
        /// <param name="mediaTypeLoader"></param>
        public AdfBannerSummaryLoader(
            int accountId,
            IBaseRepository<AdfBanner> entityRepository,
            IBaseRepository<AdfBannerSummary> summaryRepository,
            AdfMediaTypeLoader mediaTypeLoader)
            : base(accountId, entityRepository, summaryRepository, mediaTypeLoader)
        {
        }

        /// <summary>
        /// Merges (inserts or updates) dependent banners with existed in DB.
        /// </summary>
        /// <param name="items">Banners for merge.</param>
        /// <returns>True if successfully.</returns>
        public bool MergeDependentBanners(List<AdfBanner> items)
        {
            return MergeDependentEntitiesWithExisted(
                items,
                options =>
                {
                    options.ColumnPrimaryKeyExpression = x => new { x.AccountId, x.ExternalId };
                });
        }

        /// <inheritdoc />
        /// <summary>
        /// Merges (inserts or updates) Banner summaries with existed in DB.
        /// </summary>
        /// <param name="items">Banner summary items for merge.</param>
        /// <returns>True if successfully merged.</returns>
        protected override bool MergeItemsWithExisted(List<AdfBannerSummary> items)
        {
            var entities = items.Select(x => x.Banner).ToList();
            var result = MergeDependentBanners(entities);
            if (result)
            {
                result = MergeSummariesWithExisted(items);
            }
            return result;
        }

        /// <inheritdoc/>
        protected override int Load(List<AdfBannerSummary> items)
        {
            Logger.Info(accountId, "Loading {0} Adform Banner Summaries..", items.Count);
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
        /// Sets DB identifier of parent account for Banner.
        /// </summary>
        /// <param name="entity">Banner for which the parent account ID will be set.</param>
        protected override void SetEntityParents(AdfBanner entity)
        {
            entity.AccountId = accountId;
        }

        /// <inheritdoc />
        /// <summary>
        /// Sets DB identifiers of parent Banner and Media type for Banner summary.
        /// </summary>
        /// <param name="summary">Line item summary.</param>
        protected override void SetSummaryParents(AdfBannerSummary summary)
        {
            base.SetSummaryParents(summary);
            summary.EntityId = summary.Banner.Id;
        }

        private void ProcessFailedStatsExtraction(Exception e, List<AdfBannerSummary> items)
        {
            Logger.Error(accountId, e);
            var exception = GetFailedStatsLoadingException(e, items, StatsTypeAgg.CreativeArg);
            InvokeProcessFailedExtractionHandlers(exception);
        }
    }
}
