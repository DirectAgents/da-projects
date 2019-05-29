using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CakeExtracter.Helpers;
using CakeExtracter.SimpleRepositories.BaseRepositories.Interfaces;
using Z.EntityFramework.Extensions;
using System.Data.Entity;

namespace CakeExtracter.SimpleRepositories.BaseRepositories
{
    /// <inheritdoc />
    /// <summary>
    /// The basic abstract generic class for implementing the common methods of the IBaseRepository interface for a database.
    /// </summary>
    /// <typeparam name="T">The type of repository entity.</typeparam>
    /// <typeparam name="TContext">The context of repository.</typeparam>
    public abstract class BaseDatabaseRepository<T, TContext> : IBaseRepository<T>
        where T : class
        where TContext : DbContext, new()
    {
        /// <inheritdoc />
        public abstract string EntityName { get; }

        /// <summary>
        /// The object to lock work with entities in a database.
        /// </summary>
        protected virtual object Locker { get; set; } = new object();

        /// <summary>
        /// Returns database keys of the entity.
        /// </summary>
        /// <param name="item">The entity of type for which the repository is used.</param>
        /// <returns>The set of keys.</returns>
        public abstract object[] GetKeys(T item);

        /// <inheritdoc />
        public virtual bool MergeItems(IEnumerable<T> itemsToMerge)
        {
            return MergeItems(itemsToMerge, null);
        }

        /// <inheritdoc />
        public virtual bool MergeItems(IEnumerable<T> itemsToMerge, Action<EntityBulkOperation<T>> entityBulkOptionsAction)
        {
            return SafeContextWrapper.TryMakeTransactionWithLock<TContext>(
                dbContext => MergeItems(dbContext, itemsToMerge, entityBulkOptionsAction), Locker,
                $"Merging {typeof(T).Name} database items");
        }

        /// <inheritdoc />
        public T GetItem(int id)
        {
            T item = null;
            SafeContextWrapper.TryMakeTransactionWithLock<TContext>(dbContext => item = GetItem(dbContext, id), Locker,
                $"Getting {typeof(T).Name} database item");
            return item;
        }

        /// <inheritdoc />
        public T GetFirstItem(Func<T, bool> predicate)
        {
            T item = null;
            SafeContextWrapper.TryMakeTransactionWithLock<TContext>(dbContext => item = GetFirstItem(dbContext, predicate), Locker,
                $"Getting first {typeof(T).Name} database item");
            return item;
        }

        /// <inheritdoc />
        public List<T> GetItems(Func<T, bool> predicate)
        {
            List<T> items = null;
            SafeContextWrapper.TryMakeTransactionWithLock<TContext>(dbContext => items = GetItems(dbContext, predicate), Locker,
                $"Getting {typeof(T).Name} database items");
            return items;
        }

        /// <inheritdoc />
        public List<T> GetItemsWithIncludes(Func<T, bool> predicate, string includeProperty)
        {
            List<T> items = null;
            SafeContextWrapper.TryMakeTransactionWithLock<TContext>(dbContext => items = GetItemsWithInclude(dbContext, predicate, includeProperty), Locker,
                $"Getting {typeof(T).Name} database items with includes");
            return items;
        }

        /// <inheritdoc />
        public void AddItem(T item)
        {
            SafeContextWrapper.TryMakeTransactionWithLock<TContext>(dbContext => AddItem(dbContext, item), Locker,
                $"Inserting {typeof(T).Name} database item.");
        }

        /// <inheritdoc />
        public void AddItems(IEnumerable<T> items)
        {
            SafeContextWrapper.TryMakeTransactionWithLock<TContext>(dbContext => AddItems(dbContext, items), Locker,
                $"Inserting {typeof(T).Name} database items.");
        }

        /// <inheritdoc />
        public void UpdateItem(T itemToUpdate)
        {
            SafeContextWrapper.TryMakeTransactionWithLock<TContext>(dbContext => UpdateItem(dbContext, itemToUpdate), Locker,
                $"Updating {typeof(T).Name} database item");
        }

        /// <inheritdoc />
        public void UpdateItems(IEnumerable<T> itemsToUpdate)
        {
            SafeContextWrapper.TryMakeTransactionWithLock<TContext>(dbContext => UpdateItems(dbContext, itemsToUpdate), Locker,
                $"Updating {typeof(T).Name} database items");
        }

        private T GetItem(TContext dbContext, params object[] keys)
        {
            return dbContext.Set<T>().Find(keys);
        }

        private T GetFirstItem(TContext dbContext, Func<T, bool> predicate)
        {
            return dbContext.Set<T>().FirstOrDefault(predicate);
        }

        private List<T> GetItems(TContext dbContext, Func<T, bool> predicate)
        {
            return dbContext.Set<T>().Where(predicate).ToList();
        }

        private List<T> GetItemsWithInclude(TContext dbContext, Func<T, bool> predicate, string includeProperty)
        {
            return dbContext.Set<T>().Include(includeProperty).Where(predicate).ToList();
        }

        private void AddItem(TContext dbContext, T item)
        {
            dbContext.Set<T>().Add(item);
            dbContext.SaveChanges();
        }

        private void AddItems(TContext dbContext, IEnumerable<T> items)
        {
            dbContext.BulkInsert(items);
        }

        private void UpdateItem(TContext dbContext, T itemToUpdate)
        {
            var itemKeys = GetKeys(itemToUpdate);
            var dbItem = GetItem(dbContext, itemKeys);
            dbContext.Entry(dbItem).CurrentValues.SetValues(itemToUpdate);
            dbContext.SaveChanges();
        }

        private void UpdateItems(TContext dbContext, IEnumerable<T> itemsToUpdate)
        {
            dbContext.BulkUpdate(itemsToUpdate);
        }

        private void MergeItems(TContext dbContext, IEnumerable<T> itemsToMerge,
            Action<EntityBulkOperation<T>> entityBulkOptionsAction)
        {
            if (entityBulkOptionsAction == null)
            {
                dbContext.BulkMerge(itemsToMerge);
            }
            else
            {
                dbContext.BulkMerge(itemsToMerge, entityBulkOptionsAction);
            }
        }
    }
}
