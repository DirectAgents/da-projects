using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.Administration.JobExecution;
using MVCGrid.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DirectAgents.Web.Areas.Admin.Grids.JobHistory
{
    /// <summary>
    /// Job History Data Provider
    /// </summary>
    /// <seealso cref="DirectAgents.Web.Areas.Admin.Grids.JobHistory.IJobHistoryDataProvider" />
    public class JobHistoryDataProvider : IJobHistoryDataProvider
    {
        private readonly List<string> HistoryItemJobsBlackList = new List<string>
        {
            "ScheduledRequestsLauncherCommand"
        };

        /// <summary>
        /// Gets the query result.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public QueryResult<JobRequestExecution> GetQueryResult(QueryOptions options)
        {
            using (var db = new ClientPortalProgContext())
            {
                var result = new QueryResult<JobRequestExecution>();
                IQueryable<JobRequestExecution> query = db.JobRequestExecutions.AsQueryable().Include(ex=>ex.JobRequest);
                query = query.ApplyStatusFilter(options)
                    .ApplyParentJobIdFilter(options)
                    .ApplyStartTimeSorting(options)
                    .ApplyStartDateFilter(options)
                    .ApplyCommandNameFilter(options, HistoryItemJobsBlackList);
                result.TotalRecords = query.Count();
                query = query.ApplyPaging(options);
                result.Items = query.ToList();
                return result;
            }
        }
    }
}