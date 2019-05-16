using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.SimpleRepositories.BaseRepositories.Interfaces;
using DirectAgents.Domain.Entities.CPProg.YAM;
using DirectAgents.Domain.Entities.CPProg.YAM.Summaries;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Z.EntityFramework.Extensions;

namespace CakeExtracter.Etl.YAM.Loaders
{
    public abstract class BaseYamSummaryLoader<TEntity, TSummary> : Loader<TSummary>
        where TEntity : BaseYamEntity
        where TSummary : BaseYamSummary
    {
        private readonly IBaseRepository<TEntity> entityRepository;
        private readonly IBaseRepository<TSummary> summaryRepository;

        protected BaseYamSummaryLoader(int accountId, IBaseRepository<TEntity> entityRepository,
            IBaseRepository<TSummary> summaryRepository) : base(accountId)
        {
            this.entityRepository = entityRepository;
            this.summaryRepository = summaryRepository;
        }

        protected abstract void SetEntityParents(TEntity entity);

        protected abstract void SetSummaryParents(TSummary summary);

        public bool MergeDependentEntitiesWithExisted(IEnumerable<TEntity> items)
        {
            return MergeDependentEntitiesWithExisted(items, options =>
            {
                options.ColumnPrimaryKeyExpression = x => x.ExternalId;
            });
        }

        public bool MergeDependentEntitiesWithExisted(IEnumerable<TEntity> items,
            Action<EntityBulkOperation<TEntity>> entityBulkOptionsAction)
        {
            var entities = GetUniqueEntities(items, SetEntityParents);
            var result = entityRepository.MergeItems(entities, entityBulkOptionsAction);
            SetEntityDatabaseIds(items, entities);
            LogMergedEntities(items, entityRepository.EntityName);
            return result;
        }

        public bool MergeSummariesWithExisted(IEnumerable<TSummary> items)
        {
            items.ForEach(SetSummaryParents);
            var result = summaryRepository.MergeItems(items);
            LogMergedEntities(items, summaryRepository.EntityName);
            return result;
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
            where TEntity : BaseYamEntity
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
