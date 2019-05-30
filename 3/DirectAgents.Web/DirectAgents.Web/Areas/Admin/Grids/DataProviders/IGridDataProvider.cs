using MVCGrid.Models;

namespace DirectAgents.Web.Areas.Admin.Grids.DataProviders
{
    /// <summary>
    /// Grid Data Provider interface.
    /// </summary>
    public interface IGridDataProvider<T>
    {
        /// <summary>
        /// Gets the query result.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns>Items query result.</returns>
        QueryResult<T> GetQueryResult(QueryOptions options);
    }
}