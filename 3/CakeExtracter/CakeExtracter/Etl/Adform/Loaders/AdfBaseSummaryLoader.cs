using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Etl.Adform.Exceptions;
using CakeExtracter.SimpleRepositories.BaseRepositories.Interfaces;
using DirectAgents.Domain.Entities.CPProg.Adform;
using DirectAgents.Domain.Entities.CPProg.Adform.Summaries;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Z.EntityFramework.Extensions;

namespace CakeExtracter.Etl.Adform.Loaders
{
    /// <summary>
    /// Adform base summary loader.
    /// </summary>
    /// <typeparam name="TEntity">Common entity type.</typeparam>
    /// <typeparam name="TSummary">Common summary type.</typeparam>
    public abstract class AdfBaseSummaryLoader<TEntity, TSummary> : Loader<TSummary>
        where TEntity : AdfBaseEntity
        where TSummary : AdfBaseSummary
    {
        private const int DefaultBatchSize = 1000;

        /// <summary>
        /// Action for exception of process failed extraction.
        /// </summary>
        public event Action<AdformFailedStatsLoadingException> ProcessFailedExtraction;

        private readonly AdfMediaTypeLoader mediaTypeLoader;
        private readonly IBaseRepository<TEntity> entityRepository;
        private readonly IBaseRepository<TSummary> summaryRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdfBaseSummaryLoader{TEntity, TSummary}"/> class.
        /// </summary>
        /// <param name="accountId">Identifier of DB account.</param>
        /// <param name="entityRepository">DB repository for entities.</param>
        /// <param name="summaryRepository">DB repository for summaries.</param>
        /// <param name="mediaTypeLoader">Loader of media types.</param>
        protected AdfBaseSummaryLoader(
            int accountId,
            IBaseRepository<TEntity> entityRepository,
            IBaseRepository<TSummary> summaryRepository,
            AdfMediaTypeLoader mediaTypeLoader)
            : base(accountId, DefaultBatchSize)
        {
            this.entityRepository = entityRepository;
            this.summaryRepository = summaryRepository;
            this.mediaTypeLoader = mediaTypeLoader;
        }

        /// <summary>
        /// Merges (inserts or updates) dependent entities with existed in DB.
        /// </summary>
        /// <param name="items">Items for merge.</param>
        /// <returns>True if successfully merged.</returns>
        public bool MergeDependentEntitiesWithExisted(IEnumerable<TEntity> items)
        {
            return MergeDependentEntitiesWithExisted(items, options =>
            {
                options.ColumnPrimaryKeyExpression = x => x.ExternalId;
            });
        }

        /// <summary>
        /// Merges (inserts or updates) dependent entities with existed in DB.
        /// </summary>
        /// <param name="items">Items for merge.</param>
        /// <param name="entityBulkOptionsAction">Action for bulk merge extension.</param>
        /// <returns>True if successfully merged.</returns>
        public bool MergeDependentEntitiesWithExisted(
            IEnumerable<TEntity> items,
            Action<EntityBulkOperation<TEntity>> entityBulkOptionsAction)
        {
            var entities = GetUniqueEntities(items, SetEntityParents);
            var result = entityRepository.MergeItems(entities, entityBulkOptionsAction);
            SetEntityDatabaseIds(items, entities);
            LogMergedEntities(entities, entityRepository.EntityName);
            return result;
        }

        /// <summary>
        /// Merges (inserts or updates) summaries with existed in DB.
        /// </summary>
        /// <param name="items">Summary items for merge.</param>
        /// <returns>True if successfully merged.</returns>
        public bool MergeSummariesWithExisted(IEnumerable<TSummary> items)
        {
            items.ForEach(SetSummaryParents);
            var result = summaryRepository.MergeItems(items);
            LogMergedEntities(items, summaryRepository.EntityName);
            return result;
        }

        /// <summary>
        /// Merges (inserts or updates) summaries with existed in DB.
        /// </summary>
        /// <param name="items">Summary items for merge.</param>
        /// <returns>True if successfully merged.</returns>
        protected abstract bool MergeItemsWithExisted(List<TSummary> items);

        /// <summary>
        /// Sets DB identifier of parent entity for entity.
        /// </summary>
        /// <param name="entity">Entity for which the parent ID will be set.</param>
        protected abstract void SetEntityParents(TEntity entity);

        /// <summary>
        /// Sets DB identifier of parent entity for summary.
        /// </summary>
        /// <param name="summary">Summary for which the parent entity ID will be set.</param>
        protected virtual void SetSummaryParents(TSummary summary)
        {
            var dbMediaType = mediaTypeLoader.GetMediaTypeByExternalId(summary.MediaType.ExternalId);
            summary.MediaTypeId = dbMediaType.Id;
        }

        /// <summary>
        /// Gets exception with information for relaunch logic.
        /// </summary>
        /// <param name="e">Inner exception.</param>
        /// <param name="items">Loaded summary items.</param>
        /// <returns>Exception for relaunch logic.</returns>
        protected virtual AdformFailedStatsLoadingException GetFailedStatsLoadingException(Exception e, List<TSummary> items)
        {
            var fromDate = items.Min(x => x.Date);
            var toDate = items.Max(x => x.Date);
            var fromDateArg = fromDate == default(DateTime) ? null : (DateTime?)fromDate;
            var toDateArg = toDate == default(DateTime) ? null : (DateTime?)toDate;
            var exception = new AdformFailedStatsLoadingException(fromDateArg, toDateArg, accountId, e);
            return exception;
        }

        /// <summary>
        /// Merges (inserts or updates) dependent media types with existed in DB.
        /// </summary>
        /// <param name="mediaTypes">Media types for merge.</param>
        /// <returns>True if successfully merged.</returns>
        protected bool MergeDependentMediaTypesWithExisted(IEnumerable<AdfMediaType> mediaTypes)
        {
            return mediaTypeLoader.MergeDependentEntitiesWithExisted(mediaTypes);
        }

        /// <summary>
        /// Invokes exception of process failed extraction.
        /// </summary>
        /// <param name="exception">Exception.</param>
        protected void InvokeProcessFailedExtractionHandlers(AdformFailedStatsLoadingException exception)
        {
            ProcessFailedExtraction?.Invoke(exception);
        }

        private void LogMergedEntities(IEnumerable<object> items, string entitiesName)
        {
            Logger.Info(accountId, $"{entitiesName} were merged: {items.Count()}.");
        }

        private static IEnumerable<TEntity> GetUniqueEntities(IEnumerable<TEntity> items, Action<TEntity> setParentsAction)
        {
            var notNullableItems = items.Where(x => x != null).ToList();
            notNullableItems.ForEach(setParentsAction);
            var entities = notNullableItems
                .GroupBy(x => new { x.Name, x.ExternalId })
                .Select(x => x.First())
                .ToList();
            return entities;
        }

        private static void SetEntityDatabaseIds(IEnumerable<TEntity> items, IEnumerable<TEntity> dbEntities)
        {
            foreach (var item in items)
            {
                var dbEntity = dbEntities.FirstOrDefault(x => item.ExternalId == x.ExternalId);
                if (dbEntity != null)
                {
                    item.Id = dbEntity.Id;
                }
            }
        }
    }
}
