namespace CakeExtracter.SimpleRepositories.BasicRepositories.Interfaces
{
    /// <summary>
    /// The interface describes the behavior for working with certain entities of a repository.
    /// </summary>
    /// <typeparam name="T">The type of repository entity.</typeparam>
    public interface IBasicRepository<T>
    {
        /// <summary>
        /// Receives an item by ID from the repository.
        /// </summary>
        /// <param name="id">The item ID.</param>
        /// <returns>The item.</returns>
        T GetItem(int id);

        /// <summary>
        /// Adds a new entity to the repository.
        /// </summary>
        /// <param name="item">The entity to add.</param>
        /// <returns>Added repository entity.</returns>
        T AddItem(T item);

        /// <summary>
        /// Updates a repository entity.
        /// </summary>
        /// <param name="itemToUpdate">The repository entity that was updated.</param>
        void UpdateItem(T itemToUpdate);
    }
}
