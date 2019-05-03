using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg.YAM;
using DirectAgents.Domain.Entities.CPProg.YAM.Summaries;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Z.EntityFramework.Extensions;

namespace CakeExtracter.Etl.YAM.Loaders
{
    internal class BaseYamSummaryLoader
    {
        private readonly int accountId;

        public BaseYamSummaryLoader(int accountId)
        {
            this.accountId = accountId;
        }

        public bool MergeDependentEntitiesWithExisted<TEntity>(IEnumerable<TEntity> items, MergeHelper mergeHelper,
            Action<TEntity> setParentsAction)
            where TEntity : BaseYamEntity
        {
            return MergeDependentEntitiesWithExisted(items, mergeHelper, setParentsAction, options =>
            {
                options.ColumnPrimaryKeyExpression = x => x.ExternalId;
            });
        }

        public bool MergeDependentEntitiesWithExisted<TEntity>(IEnumerable<TEntity> items, MergeHelper mergeHelper,
            Action<TEntity> setParentsAction, Action<EntityBulkOperation<TEntity>> entityBulkOptionsAction)
            where TEntity : BaseYamEntity
        {
            var entities = GetUniqueEntities(items, setParentsAction);
            var result = MergeEntities(entities, mergeHelper.Locker, entityBulkOptionsAction);
            SetEntityDatabaseIds(items, entities);
            LogMergedEntities(entities, mergeHelper, result);
            return result;
        }

        public bool MergeSummariesWithExisted<TSummary>(IEnumerable<TSummary> items, MergeHelper mergeHelper,
            Action<TSummary> setParentsAction)
            where TSummary : BaseYamSummary
        {
            items.ForEach(setParentsAction);
            var result = MergeSummaries(items, mergeHelper.Locker);
            LogMergedEntities(items, mergeHelper, result);
            return result;
        }

        private void LogMergedEntities(IEnumerable<object> items, MergeHelper mergeHelper, bool result)
        {
            Logger.Info(accountId, $"YAM {mergeHelper.EntitiesName} were merged: {items.Count()}.");
        }

        private static IEnumerable<TEntity> GetUniqueEntities<TEntity>(IEnumerable<TEntity> items, Action<TEntity> setParentsAction)
            where TEntity : BaseYamEntity
        {
            var notNullableItems = items.Where(x => x != null).ToList();
            notNullableItems.ForEach(setParentsAction);
            var entities = notNullableItems
                .GroupBy(x => new { x.Name, x.ExternalId })
                .Select(x => x.First())
                .ToList();
            return entities;
        }

        private static bool MergeEntities<TEntity>(IEnumerable<TEntity> items, object locker,
            Action<EntityBulkOperation<TEntity>> entityBulkOptionsAction)
            where TEntity : BaseYamEntity
        {
            return SafeContextWrapper.TryMakeTransactionWithLock((ClientPortalProgContext db) =>
                {
                    db.BulkMerge(items, entityBulkOptionsAction);
                },
                locker, "BulkMerge");
        }

        private static bool MergeSummaries<TSummary>(IEnumerable<TSummary> items, object locker)
            where TSummary : BaseYamSummary
        {
            return SafeContextWrapper.TryMakeTransactionWithLock((ClientPortalProgContext db) =>
                {
                    db.BulkMerge(items);
                },
                locker, "BulkMerge");
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
