﻿using System;
using System.Collections.Generic;
using Z.EntityFramework.Extensions;

namespace CakeExtracter.SimpleRepositories.BaseRepositories.Interfaces
{
    /// <summary>
    /// The interface describes the behavior for working with certain entities of a repository.
    /// </summary>
    /// <typeparam name="T">The type of repository entity.</typeparam>
    public interface IBaseRepository<T>
        where T : class
    {
        /// <summary>
        /// Gets the string name of the entity for which the repository is used.
        /// </summary>
        string EntityName { get; }

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
        /// Gets the items  by the predicate from the repository with includes.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="includeProperty">The include property.</param>
        /// <returns>The items</returns>
        List<T> GetItemsWithIncludes(Func<T, bool> predicate, string includeProperty);

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
        /// Updates a repository entity.
        /// </summary>
        /// <param name="itemToUpdate">The repository entity that was updated.</param>
        /// <param name="ignoredPropertiesNames">The names of ignored properties.</param>
        void UpdateItem(T itemToUpdate, params string[] ignoredPropertiesNames);

        /// <summary>
        /// Updates repository entities.
        /// </summary>
        /// <param name="itemsToUpdate">The repository entities that was updated.</param>
        void UpdateItems(IEnumerable<T> itemsToUpdate);

        /// <summary>
        /// Updates repository entities.
        /// </summary>
        /// <param name="itemsToUpdate">The repository entities that was updated.</param>
        /// <param name="entityBulkOptionsAction">The entity bulk options action.</param>
        void UpdateItems(IEnumerable<T> itemsToUpdate, Action<EntityBulkOperation<T>> entityBulkOptionsAction);

        /// <summary>
        /// Merges the items. Uses Third party EF extensions library.
        /// </summary>
        /// <param name="itemsToMerge">The items to merge.</param>
        /// <returns>Merged items.</returns>
        bool MergeItems(IEnumerable<T> itemsToMerge);

        /// <summary>
        /// Merges the items with options. Uses Third party EF extensions library.
        /// </summary>
        /// <param name="itemsToMerge">The items to merge.</param>
        /// <param name="entityBulkOptionsAction">The entity bulk options action.</param>
        /// <returns>Merged items.</returns>
        bool MergeItems(IEnumerable<T> itemsToMerge, Action<EntityBulkOperation<T>> entityBulkOptionsAction);
    }
}
