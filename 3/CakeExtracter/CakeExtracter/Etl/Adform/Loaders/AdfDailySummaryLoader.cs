using System.Collections.Generic;
using System.Linq;
using CakeExtracter.SimpleRepositories.BaseRepositories.Interfaces;
using DirectAgents.Domain.Entities.CPProg.Adform;
using DirectAgents.Domain.Entities.CPProg.Adform.Summaries;

namespace CakeExtracter.Etl.Adform.Loaders
{
    /// <inheritdoc />
    /// <summary>
    /// Adform loader of Daily level summaries.
    /// </summary>
    internal class AdfDailySummaryLoader : AdfBaseSummaryLoader<AdfBaseEntity, AdfDailySummary>
    {
        /// <inheritdoc cref="AdfBaseSummaryLoader{AdfBaseEntity, AdfDailySummary}"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="AdfDailySummaryLoader" /> class.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="summaryRepository"></param>
        /// <param name="mediaTypeLoader"></param>
        public AdfDailySummaryLoader(
            int accountId,
            IBaseRepository<AdfDailySummary> summaryRepository,
            AdfMediaTypeLoader mediaTypeLoader)
            : base(accountId, null, summaryRepository, mediaTypeLoader)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Merges (inserts or updates) Daily summaries with existed in DB.
        /// </summary>
        /// <param name="items">Daily summary items for merge.</param>
        /// <returns>True if successfully merged.</returns>
        protected override bool MergeItemsWithExisted(List<AdfDailySummary> items)
        {
            var mediaTypes = items.Select(i => i.MediaType).ToList();
            var result = MergeDependentMediaTypesWithExisted(mediaTypes);
            if (result)
            {
                result = MergeSummariesWithExisted(items);
            }
            return result;
        }

        /// <inheritdoc />
        protected override int Load(List<AdfDailySummary> items)
        {
            Logger.Info(accountId, "Loading {0} Adform Daily Summaries..", items.Count);
            var result = MergeItemsWithExisted(items);
            return result ? items.Count : 0;
        }

        /// <inheritdoc />
        protected override void SetEntityParents(AdfBaseEntity entity)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        /// <summary>
        /// Sets DB identifiers of parent account and Media type for Daily summary.
        /// </summary>
        /// <param name="summary">Daily summary.</param>
        protected override void SetSummaryParents(AdfDailySummary summary)
        {
            base.SetSummaryParents(summary);
            summary.EntityId = accountId;
        }
    }
}
