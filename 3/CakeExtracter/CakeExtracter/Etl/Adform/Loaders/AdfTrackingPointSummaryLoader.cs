using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Commands;
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
            var lineItems = items.Select(x => x.LineItem).ToList();
            var result = lineItemLoader.MergeDependentLineItems(lineItems);
            if (result)
            {
                result = MergeDependentEntitiesWithExisted(items);
            }
            return result;
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
                result = MergeSummariesWithExisted(items);
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
        /// Sets DB identifier of parent Line item for Tracking Point.
        /// </summary>
        /// <param name="entity">Tracking point for which the parent Line item ID will be set.</param>
        protected override void SetEntityParents(AdfTrackingPoint entity)
        {
            entity.LineItemId = entity.LineItem.Id;
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
