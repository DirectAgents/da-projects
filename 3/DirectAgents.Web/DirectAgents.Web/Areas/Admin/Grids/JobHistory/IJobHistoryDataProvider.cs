using DirectAgents.Domain.Entities.Administration.JobExecution;
using MVCGrid.Models;

namespace DirectAgents.Web.Areas.Admin.Grids.JobHistory
{
    public interface IJobHistoryDataProvider
    {
        QueryResult<JobRequestExecution> GetQueryResult(QueryOptions options);
    }
}