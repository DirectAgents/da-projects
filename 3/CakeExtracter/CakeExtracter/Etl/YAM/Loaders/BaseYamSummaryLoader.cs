using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg.YAM;
using DirectAgents.Domain.Entities.CPProg.YAM.Summaries;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;

namespace CakeExtracter.Etl.YAM.Loaders
{
    internal class BaseYamSummaryLoader
    {
        private readonly int accountId;

        public BaseYamSummaryLoader(int accountId)
        {
            this.accountId = accountId;
        }

        public void MergeDependentEntitiesWithExisted<TEntity>(IEnumerable<TEntity> items, MergeHelper mergeHelper,
            Action<TEntity> setParentsAction)
            where TEntity : BaseYamEntity
        {
            var entities = GetUniqueEntities(items, setParentsAction);
            MergeEntities(entities, mergeHelper.Locker);
            SetEntityDatabaseIds(items, entities);
            LogMergedEntities(entities, mergeHelper);
        }

        public void MergeSummariesWithExisted<TSummary>(IEnumerable<TSummary> items, MergeHelper mergeHelper,
            Action<TSummary> setParentsAction)
            where TSummary : BaseYamSummary
        {
            items.ForEach(setParentsAction);
            MergeSummaries(items, mergeHelper.Locker);
            LogMergedEntities(items, mergeHelper);
        }

        private void LogMergedEntities(IEnumerable<object> items, MergeHelper mergeHelper)
        {
            Logger.Info(accountId, $"YAM {mergeHelper.EntitiesName} were merged: {items.Count()}.");
        }

        private static IEnumerable<TEntity> GetUniqueEntities<TEntity>(IEnumerable<TEntity> items, Action<TEntity> setParentsAction)
            where TEntity : BaseYamEntity
        {
            var notNullableItems = items.Where(x => x != null).ToList();
            notNullableItems.ForEach(setParentsAction);
            var entities = notNullableItems
                .Where(x => !string.IsNullOrWhiteSpace(x.Name) || x.ExternalId != 0)
                .GroupBy(x => new { x.Name, x.ExternalId })
                .Select(x => x.First())
                .ToList();
            return entities;
        }

        private static void MergeEntities<TEntity>(IEnumerable<TEntity> items, object locker)
            where TEntity : BaseYamEntity
        {
            SafeContextWrapper.TryMakeTransactionWithLock(
                (ClientPortalProgContext db) =>
                {
                    db.BulkMerge(items, options => options.ColumnPrimaryKeyExpression = x => x.ExternalId);
                },
                locker, "BulkMerge");
        }

        private static void MergeSummaries<TSummary>(IEnumerable<TSummary> items, object locker)
            where TSummary : BaseYamSummary
        {
            SafeContextWrapper.TryMakeTransactionWithLock((ClientPortalProgContext db) =>
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
