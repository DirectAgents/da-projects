using System.Collections.Generic;
using System.Data.Entity;
using CakeExtracter.Helpers;

namespace CakeExtracter.SimpleRepositories.RepositoriesWithStorage.Interfaces
{
    /// <summary>
    /// The interface describes the behavior for working with a database and storing certain identifiers of entities in memory.
    /// </summary>
    /// <typeparam name="T">The type of repository entity.</typeparam>
    /// <typeparam name="TContext">The context of repository.</typeparam>
    internal interface IRepositoryWithStorage<T, in TContext>
        where TContext : DbContext
    {
        /// <summary>
        /// The storage for entity IDs.
        /// <see cref="EntityIdStorage"/>
        /// </summary>
        EntityIdStorage<T> IdStorage { get; }

        /// <summary>
        /// Gets all database entities that correspond the entity to compare.
        /// </summary>
        /// <param name="db">The repository context.</param>
        /// <param name="itemToCompare">The entity to compare.</param>
        /// <returns>List of appropriate database entities.</returns>
        List<T> GetItems(TContext db, T itemToCompare);

        /// <summary>
        /// Adds a new entity to a database.
        /// </summary>
        /// <param name="db">The repository context.</param>
        /// <param name="sourceItem">The entity to add.</param>
        /// <returns>Added database entity.</returns>
        T AddItem(TContext db, T sourceItem);

        /// <summary>
        /// Adds entities to a database.
        /// </summary>
        /// <param name="items">The entities to add.</param>
        void AddItems(IEnumerable<T> items);

        /// <summary>
        /// Adds entities to a database.
        /// </summary>
        /// <param name="db">The repository context.</param>
        /// <param name="items">The entities to add.</param>
        void AddItems(TContext db, IEnumerable<T> items);

        /// <summary>
        /// Updates a database entity.
        /// </summary>
        /// <param name="db">The repository context.</param>
        /// <param name="sourceItem">The entity by which the database entity should be updated.</param>
        /// <param name="targetItemInDb">The database object to be updated.</param>
        /// <returns>Number of changed database objects.</returns>
        int UpdateItem(TContext db, T sourceItem, T targetItemInDb);
    }
}
