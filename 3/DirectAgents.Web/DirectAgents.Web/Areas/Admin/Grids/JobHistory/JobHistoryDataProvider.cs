using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.Administration.JobExecution;
using DirectAgents.Web.Areas.Admin.Grids.DataProviders;
using MVCGrid.Models;

namespace DirectAgents.Web.Areas.Admin.Grids.JobHistory
{
    /// <summary>
    /// Job History Data Provider.
    /// </summary>
    /// <seealso cref="IGridDataProvider&lt;JobRequestExecution&gt;" />
    public class JobHistoryDataProvider : IGridDataProvider<JobRequestExecution>
    {
        private readonly List<string> historyItemJobsBlackList = new List<string>
        {
            "ScheduledRequestsLauncherCommand",
        };

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
                var queryItems = query.ToList();
                result.Items = ConvertDatesToLocalFormat(queryItems); // Dates should be displayed in server local time.
                return result;
            }
        }

        private List<JobRequestExecution> ConvertDatesToLocalFormat(List<JobRequestExecution> sourceItems)
        {
            sourceItems.ForEach(item =>
            {
                item.StartTime = item.StartTime?.ToLocalTime();
                item.EndTime = item.EndTime?.ToLocalTime();
            });
            return sourceItems;
        }
    }
}