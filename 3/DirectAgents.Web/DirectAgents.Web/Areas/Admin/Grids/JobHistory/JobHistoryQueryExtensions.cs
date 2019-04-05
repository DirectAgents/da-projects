using DirectAgents.Domain.Entities.Administration.JobExecution;
using DirectAgents.Domain.Entities.Administration.JobExecution.Enums;
using MVCGrid.Models;
using System;
using System.Linq;

namespace DirectAgents.Web.Areas.Admin.Grids.JobHistory
{
    public static class JobHistoryQueryExtensions
    {
        public static IQueryable<JobRequestExecution> ApplyCommandNameFilter(this IQueryable<JobRequestExecution> source, QueryOptions options)
        {
            const string commandNameFilterKey = "CommandName";
            if (options.Filters.ContainsKey(commandNameFilterKey))
            {
                var filterValue = options.Filters[commandNameFilterKey];
                source = source.Where(jeXecution => jeXecution.JobRequest.CommandName.StartsWith(filterValue));
            }
            return source;
        }

        public static IQueryable<JobRequestExecution> ApplyParentJobIdFilter(this IQueryable<JobRequestExecution> source, QueryOptions options)
        {
            const string parentJobFilterKey = "ParentJobId";
            if (options.Filters.ContainsKey(parentJobFilterKey))
            {
                int filterJobIdValue;
                if (int.TryParse(options.Filters[parentJobFilterKey], out filterJobIdValue))
                {
                    source = filterJobIdValue > 0 ?
                        source.Where(jeXecution => jeXecution.JobRequest.ParentJobRequestId == filterJobIdValue):
                        source.Where(jeXecution => jeXecution.JobRequest.ParentJobRequestId == null);
                }
            }
            return source;
        }

        public static IQueryable<JobRequestExecution> ApplyStatusFilter(this IQueryable<JobRequestExecution> source, QueryOptions options)
        {
            const string statusFilterKey = "Status";
            if (options.Filters.ContainsKey(statusFilterKey))
            {
                int statusValue;
                if (int.TryParse(options.Filters[statusFilterKey], out statusValue))
                {
                    var status = (JobExecutionStatus)statusValue;
                    source = source.Where(jeXecution => jeXecution.Status == status);
                }
            }
            return source;
        }

        public static IQueryable<JobRequestExecution> ApplyStartDateFilter(this IQueryable<JobRequestExecution> source, QueryOptions options)
        {
            const string startDateFilterKey = "StartTime";
            if (options.Filters.ContainsKey(startDateFilterKey))
            {
                DateTime dateFilterValue = new DateTime();
                if (DateTime.TryParse(options.Filters[startDateFilterKey], out dateFilterValue))
                {
                    source = source.Where(jeXecution => jeXecution.StartTime.Value.Day == dateFilterValue.Day);
                }
            }
            return source;
        }

        public static IQueryable<JobRequestExecution> ApplyStartTimeSorting(this IQueryable<JobRequestExecution> source, QueryOptions options)
        {
            source = source.OrderByDescending(p => p.StartTime);
            return source;
        }

        public static IQueryable<JobRequestExecution> ApplyPaging(this IQueryable<JobRequestExecution> source, QueryOptions options)
        {
            if (options.GetLimitOffset().HasValue)
            {
                source = source.Skip(options.GetLimitOffset().Value).Take(options.GetLimitRowcount().Value);
            }
            return source;
        }

    }
}