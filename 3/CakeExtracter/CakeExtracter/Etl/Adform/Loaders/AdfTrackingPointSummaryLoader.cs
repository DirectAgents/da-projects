using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Common;
using CakeExtracter.SimpleRepositories.BaseRepositories.Interfaces;
using DirectAgents.Domain.Entities.CPProg.Adform;
using DirectAgents.Domain.Entities.CPProg.Adform.Summaries;

namespace CakeExtracter.Etl.Adform.Loaders
{
    /// <inheritdoc />
    /// <summary>
    /// Adform loader of Tracking Point level summaries.
    /// </summary>
    internal class AdfTrackingPointSummaryLoader : AdfBaseSummaryLoader<AdfTrackingPoint, AdfTrackingPointSummary>
    {
        private readonly AdfLineItemSummaryLoader lineItemLoader;

        /// <inheritdoc cref="AdfBaseSummaryLoader{AdfTrackingPoint, AdfTrackingPointSummary}"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="AdfTrackingPointSummaryLoader" /> class.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="entityRepository"></param>
        /// <param name="summaryRepository"></param>
        /// <param name="lineItemLoader">LineItem summary loader.</param>
        /// <param name="mediaTypeLoader"></param>
        public AdfTrackingPointSummaryLoader(
            int accountId,
            IBaseRepository<AdfTrackingPoint> entityRepository,
            IBaseRepository<AdfTrackingPointSummary> summaryRepository,
            AdfLineItemSummaryLoader lineItemLoader,
            AdfMediaTypeLoader mediaTypeLoader)
            : base(accountId, entityRepository, summaryRepository, mediaTypeLoader)
        {
            this.lineItemLoader = lineItemLoader;
        }

        /// <summary>
        /// Merges (inserts or updates) dependent tracking points with existed in DB.
        /// </summary>
        /// <param name="items">Tracking point items for merge.</param>
        /// <returns>True if successfully.</returns>
        public bool MergeDependentTrackingPoints(List<AdfTrackingPoint> items)
        {
            return MergeDependentEntitiesWithExisted(items);
        }

        /// <inheritdoc />
        /// <summary>
        /// Merges (inserts or updates) Tracking point summaries with existed in DB.
        /// </summary>
        /// <param name="items">Tracking point summary items for merge.</param>
        /// <returns>True if successfully merged.</returns>
        protected override bool MergeItemsWithExisted(List<AdfTrackingPointSummary> items)
        {
            var entities = items.Select(x => x.TrackingPoint).ToList();
            var result = MergeDependentTrackingPoints(entities);
            if (result)
            {
                items.ForEach(x =>
                {
                    var lineItem = lineItemLoader.GetLineItemByExternalId(x.LineItem.ExternalId);
                    x.LineItemId = lineItem?.Id ?? 0;
                    x.LineItem = null;
                });
                result = MergeSummariesWithExisted(items.Where(x => x.LineItemId > 0));
            }
            return result;
        }

        /// <inheritdoc/>
        protected override int Load(List<AdfTrackingPointSummary> items)
        {
            Logger.Info(accountId, "Loading {0} Adform TrackingPoint Summaries..", items.Count);
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
        /// Cannot be applied for Tracking Point since it doesn't have parent.
        /// </summary>
        /// <param name="entity">Tracking point.</param>
        protected override void SetEntityParents(AdfTrackingPoint entity)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Sets DB identifiers of parent Tracking point and Media type for Tracking point summary.
        /// </summary>
        /// <param name="summary">Tracking point summary.</param>
        protected override void SetSummaryParents(AdfTrackingPointSummary summary)
        {
            base.SetSummaryParents(summary);
            summary.EntityId = summary.TrackingPoint.Id;
        }

        private void ProcessFailedStatsExtraction(Exception e, List<AdfTrackingPointSummary> items)
        {
            Logger.Error(accountId, e);
            var exception = GetFailedStatsLoadingException(e, items, AdfStatsTypeAgg.TrackingPointArg);
            InvokeProcessFailedExtractionHandlers(exception);
        }
    }
}
