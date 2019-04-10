using System;
using System.Collections.Generic;

namespace CakeExtracter.SimpleRepositories.BaseRepositories.Interfaces
{
    /// <summary>
    /// The interface describes the behavior for working with certain entities of a repository.
    /// </summary>
    /// <typeparam name="T">The type of repository entity.</typeparam>
    public interface IBaseRepository<T>
    {
        /// <summary>
        /// Receives an item by ID from the repository.
        /// </summary>
        /// <param name="id">The item ID.</param>
        /// <returns>The item.</returns>
        T GetItem(int id);

        /// <summary>
        /// Gets a first item by the predicate from the repository.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>The item.</returns>
        T GetFirstItem(Func<T, bool> predicate);

        /// <summary>
        /// Gets items by the predicate from the repository.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>The items.</returns>
        List<T> GetItems(Func<T, bool> predicate);

        /// <summary>
        /// Adds a new entity to the repository.
        /// </summary>
        /// <param name="item">The entity to add.</param>
        void AddItem(T item);

        /// <summary>
        /// Adds new entities to the repository.
        /// </summary>
        /// <param name="items">The items to add.</param>
        void AddItems(IEnumerable<T> items);

        /// <summary>
        /// Updates a repository entity.
        /// </summary>
        /// <param name="itemToUpdate">The repository entity that was updated.</param>
        void UpdateItem(T itemToUpdate);

        /// <summary>
        /// Updates repository entities.
        /// </summary>
        /// <param name="itemsToUpdate">The repository entities that was updated.</param>
        void UpdateItems(IEnumerable<T> itemsToUpdate);
    }
}
