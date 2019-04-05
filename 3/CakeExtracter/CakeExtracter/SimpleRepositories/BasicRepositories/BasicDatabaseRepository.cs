using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CakeExtracter.Helpers;
using CakeExtracter.SimpleRepositories.BasicRepositories.Interfaces;

namespace CakeExtracter.SimpleRepositories.BasicRepositories
{
    /// <inheritdoc />
    /// <summary>
    /// The basic abstract generic class for implementing the common methods of the IBasicRepository interface for a database.
    /// </summary>
    /// <typeparam name="T">The type of repository entity.</typeparam>
    /// <typeparam name="TContext">The context of repository.</typeparam>
    internal abstract class BasicDatabaseRepository<T, TContext> : IBasicRepository<T> 
        where T: class
        where TContext : DbContext, new()
    {
        private readonly object locker;

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="locker">The object to lock work with entities in a database.</param>
        protected BasicDatabaseRepository(object locker)
        {
            this.locker = locker;
        }

        /// <summary>
        /// The string name of the entity for which the repository is used.
        /// </summary>
        public abstract string EntityName { get; }

        /// <summary>
        /// Returns database keys of the entity.
        /// </summary>
        /// <param name="item">The entity of type for which the repository is used.</param>
        /// <returns>The set of keys.</returns>
        public abstract object[] GetKeys(T item);

        /// <inheritdoc />
        public T GetItem(int id)
        {
            T item = null;
            SafeContextWrapper.TryMakeTransactionWithLock<TContext>(dbContext => item = GetItem(dbContext, id), locker,
                $"Getting {typeof(T).Name} database item");
            return item;
        }

        /// <inheritdoc />
        public T GetFirstItem(Func<T, bool> predicate)
        {
            T item = null;
            SafeContextWrapper.TryMakeTransactionWithLock<TContext>(dbContext => item = GetFirstItem(dbContext, predicate), locker,
                $"Getting first {typeof(T).Name} database item");
            return item;
        }

        /// <inheritdoc />
        public void AddItem(T item)
        {
            SafeContextWrapper.TryMakeTransactionWithLock<TContext>(dbContext => AddItem(dbContext, item), locker,
                $"Inserting {typeof(T).Name} database item.");
        }

        /// <inheritdoc />
        public void AddItems(IEnumerable<T> items)
        {
            SafeContextWrapper.TryMakeTransactionWithLock<TContext>(dbContext => AddItems(dbContext, items), locker,
                $"Inserting {typeof(T).Name} database items.");
        }

        /// <inheritdoc />
        public void UpdateItem(T itemToUpdate)
        {
            SafeContextWrapper.TryMakeTransactionWithLock<TContext>(dbContext => UpdateItem(dbContext, itemToUpdate),
                locker, $"Updating {typeof(T).Name} database item");
        }

        private T GetItem(TContext dbContext, params object[] keys)
        {
            return dbContext.Set<T>().Find(keys);
        }

        private T GetFirstItem(TContext dbContext, Func<T, bool> predicate)
        {
            return dbContext.Set<T>().FirstOrDefault(predicate);
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
    }
}
