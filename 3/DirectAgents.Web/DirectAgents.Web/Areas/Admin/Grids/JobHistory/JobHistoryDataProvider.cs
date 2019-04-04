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
                var query = db.JobRequestExecutions.AsQueryable().Include(ex=>ex.JobRequest);
                result.TotalRecords = query.Count();
                query = query.OrderByDescending(p => p.StartTime);
                if (options.GetLimitOffset().HasValue)
                {
                    query = query.Skip(options.GetLimitOffset().Value).Take(options.GetLimitRowcount().Value);
                }
                result.Items = query.ToList();
                return result;
            }
        }
    }
}