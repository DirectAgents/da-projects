﻿using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.SimpleRepositories.BaseRepositories.Interfaces;
using DirectAgents.Domain.Entities.CPProg.Adform;
using DirectAgents.Domain.Entities.CPProg.Adform.Summaries;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Z.EntityFramework.Extensions;

namespace CakeExtracter.Etl.Adform.Loaders
{
    public abstract class AdfBaseSummaryLoader<TEntity, TSummary> : Loader<TSummary>
        where TEntity : AdfBaseEntity
        where TSummary : AdfBaseSummary
    {
        private const int DefaultBatchSize = 1000;
        private readonly AdfMediaTypeLoader mediaTypeLoader;
        private readonly IBaseRepository<TEntity> entityRepository;
        private readonly IBaseRepository<TSummary> summaryRepository;

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

        protected abstract void SetEntityParents(TEntity entity);

        public bool MergeDependentEntitiesWithExisted(IEnumerable<TEntity> items)
        {
            return MergeDependentEntitiesWithExisted(items, options =>
            {
                options.ColumnPrimaryKeyExpression = x => x.ExternalId;
            });
        }

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

        public bool MergeSummariesWithExisted(IEnumerable<TSummary> items)
        {
            items.ForEach(SetSummaryParents);
            var result = summaryRepository.MergeItems(items);
            LogMergedEntities(items, summaryRepository.EntityName);
            return result;
        }

        protected virtual void SetSummaryParents(TSummary summary)
        {
            var dbMediaType = mediaTypeLoader.GetMediaTypeByExternalId(summary.MediaType.ExternalId);
            summary.MediaTypeId = dbMediaType.Id;
        }

        protected bool MergeDependentMediaTypesWithExisted(IEnumerable<AdfMediaType> mediaTypes)
        {
            return mediaTypeLoader.MergeDependentEntitiesWithExisted(mediaTypes);
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

        private static void SetEntityDatabaseIds<TEntity>(IEnumerable<TEntity> items, IEnumerable<TEntity> dbEntities)
            where TEntity : AdfBaseEntity
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
