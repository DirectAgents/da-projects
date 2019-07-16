using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Contexts;
using DirectAgents.Web.Areas.Admin.Grids.DataProviders;
using MVCGrid.Models;

namespace DirectAgents.Web.Areas.Admin.Grids.JobRequest
{
    /// <inheritdoc />
    /// <summary>
    /// Job Requests Data Provider.
    /// </summary>
    public class JobRequestsDataProvider : IGridDataProvider<Domain.Entities.Administration.JobExecution.JobRequest>
    {
        /// <inheritdoc />
        /// <summary>
        /// Gets the query result.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns>Job request execution query result.</returns>
        public QueryResult<Domain.Entities.Administration.JobExecution.JobRequest> GetQueryResult(QueryOptions options)
        {
            using (var db = new ClientPortalProgContext())
            {
                var result = new QueryResult<Domain.Entities.Administration.JobExecution.JobRequest>();
                var query = db.JobRequests.AsQueryable();
                query = query.ApplyStatusFilter(options)
                    .ApplyScheduleTimeSorting(options);
                result.TotalRecords = query.Count();
                query = query.ApplyPaging(options);
                var queryItems = query.ToList();
                result.Items = ConvertDatesToLocalFormat(queryItems); // Dates should be displayed in server local time.
                return result;
            }
        }

        private IEnumerable<Domain.Entities.Administration.JobExecution.JobRequest> ConvertDatesToLocalFormat(
            List<Domain.Entities.Administration.JobExecution.JobRequest> sourceItems)
        {
            sourceItems.ForEach(item => { item.ScheduledTime = item.ScheduledTime?.ToLocalTime(); });
            return sourceItems;
        }
    }
}