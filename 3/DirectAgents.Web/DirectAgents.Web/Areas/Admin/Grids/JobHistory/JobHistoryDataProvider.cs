using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.Administration.JobExecution;
using MVCGrid.Models;

namespace DirectAgents.Web.Areas.Admin.Grids.JobHistory
{
    /// <summary>
    /// Job History Data Provider
    /// </summary>
    /// <seealso cref="IJobHistoryDataProvider" />
    public class JobHistoryDataProvider : IJobHistoryDataProvider
    {
        private readonly List<string> historyItemJobsBlackList = new List<string>
        {
            "ScheduledRequestsLauncherCommand",
        };

        private List<string> commandNames = null;

        /// <summary>
        /// Gets the query result.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns>Job request execution query result.</returns>
        public QueryResult<JobRequestExecution> GetQueryResult(QueryOptions options)
        {
            using (var db = new ClientPortalProgContext())
            {
                var result = new QueryResult<JobRequestExecution>();
                IQueryable<JobRequestExecution> query = db.JobRequestExecutions.AsQueryable().Include(ex => ex.JobRequest);
                query = query.ApplyStatusFilter(options)
                    .ApplyParentJobIdFilter(options)
                    .ApplyStartTimeSorting(options)
                    .ApplyStartDateFilter(options)
                    .ApplyCommandNameFilter(options, historyItemJobsBlackList);
                result.TotalRecords = query.Count();
                query = query.ApplyPaging(options);
                result.Items = query.ToList();
                return result;
            }
        }
    }
}