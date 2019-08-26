using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Helpers;
using CakeExtracter.Logging.TimeWatchers.Amazon;
using DirectAgents.Domain.Contexts;
using Z.EntityFramework.Extensions;

namespace CakeExtracter.Etl.Amazon.Loaders.EntitiesLoaders
{
    public abstract class AmazonBaseEntityLoader<T>
        where T : class
    {
        protected readonly int accountId;

        /// <summary>
        /// Gets the locker object for multithreading operations.
        /// </summary>
        /// <value>
        /// The locker object.
        /// </value>
        protected abstract object LockerObject { get; }

        /// <summary>
        /// Gets the name of amazon job information level.
        /// </summary>
        /// <value>
        /// The name of the level.
        /// </value>
        protected abstract string LevelName { get; }

        protected AmazonBaseEntityLoader(int accountId)
        {
            this.accountId = accountId;
        }

        protected void MergeItems(IEnumerable<T> items, Action<EntityBulkOperation<T>> entityBulkOptionsAction)
        {
            AmazonTimeTracker.Instance.ExecuteWithTimeTracking(
                () =>
                {
                    SafeContextWrapper.TryMakeTransactionWithLock(
                        (ClientPortalProgContext db) => db.BulkMerge(items, entityBulkOptionsAction),
                        LockerObject,
                        "BulkMerge");
                },
                accountId,
                LevelName,
                AmazonJobOperations.LoadSummaryItemsData);
        }

        protected void LogMergedEntities(IEnumerable<object> items)
        {
            Logger.Info(accountId, $"{LevelName} were merged: {items.Count()}.");
        }
    }
}