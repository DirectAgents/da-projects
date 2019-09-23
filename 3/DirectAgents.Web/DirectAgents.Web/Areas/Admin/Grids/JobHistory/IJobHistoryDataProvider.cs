using DirectAgents.Domain.Entities.Administration.JobExecution;
using MVCGrid.Models;

namespace DirectAgents.Web.Areas.Admin.Grids.JobHistory
{
    /// <summary>
    /// Job History Grid Data Provider.
    /// </summary>
    public interface IJobHistoryDataProvider
    {
        /// <summary>
        /// Gets the query result.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns>Job request execution query result.</returns>
        QueryResult<JobRequestExecution> GetQueryResult(QueryOptions options);
    }
}