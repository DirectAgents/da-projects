using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.Administration.JobExecution;
using MVCGrid.Models;
using System.Data.Entity;
using System.Linq;

namespace DirectAgents.Web.Areas.Admin.Grids.JobHistory
{
    public class JobHistoryDataProvider : IJobHistoryDataProvider
    {
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
                    .ApplyCommandNameFilter(options)
                    .ApplyPaging(options);
                result.TotalRecords = query.Count();
                result.Items = query.ToList();
                return result;
            }
        }
    }
}