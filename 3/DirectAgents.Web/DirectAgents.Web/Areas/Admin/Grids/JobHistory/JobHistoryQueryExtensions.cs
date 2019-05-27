using System;
using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Entities.Administration.JobExecution;
using DirectAgents.Domain.Entities.Administration.JobExecution.Enums;
using MVCGrid.Models;

namespace DirectAgents.Web.Areas.Admin.Grids.JobHistory
{
    /// <summary>
    /// job history query extensions for applying paging and filtering.
    /// </summary>
    public static class JobHistoryQueryExtensions
    {
        /// <summary>
        /// Applies the command name filter.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="options">The options.</param>
        /// <param name="historyItemJobsBlackList">The history item jobs black list.</param>
        /// <returns>Query with applied command name filter</returns>
        public static IQueryable<JobRequestExecution> ApplyCommandNameFilter(
            this IQueryable<JobRequestExecution> source,
            QueryOptions options,
            List<string> historyItemJobsBlackList)
        {
            const string commandNameFilterKey = "CommandName";
            if (options.Filters.ContainsKey(commandNameFilterKey))
            {
                var filterValue = options.Filters[commandNameFilterKey];
                source = source.Where(jExecution => jExecution.JobRequest.CommandName.ToLower().Contains(filterValue.ToLower()));
            }
            else
            {
                source = source.Where(jExecution => !historyItemJobsBlackList.Contains(jExecution.JobRequest.CommandName));
            }
            return source;
        }

        /// <summary>
        /// Applies the parent job identifier filter.
        /// </summary>
        /// <param name="source">The source query object.</param>
        /// <param name="options">The grid options.</param>
        /// <returns>Query with applied parent job filter.</returns>
        public static IQueryable<JobRequestExecution> ApplyParentJobIdFilter(this IQueryable<JobRequestExecution> source, QueryOptions options)
        {
            const string parentJobFilterKey = "ParentJobId";
            if (options.Filters.ContainsKey(parentJobFilterKey))
            {
                int filterJobIdValue;
                if (int.TryParse(options.Filters[parentJobFilterKey], out filterJobIdValue))
                {
                    source = filterJobIdValue > 0 ?
                        source.Where(jExecution => jExecution.JobRequest.ParentJobRequestId == filterJobIdValue) :
                        source.Where(jExecution => jExecution.JobRequest.ParentJobRequestId == null);
                }
            }
            return source;
        }

        /// <summary>
        /// Applies the status filter.
        /// </summary>
        /// <param name="source">The source query object.</param>
        /// <param name="options">The grid options.</param>
        /// <returns>Query with applied status filter.</returns>
        public static IQueryable<JobRequestExecution> ApplyStatusFilter(this IQueryable<JobRequestExecution> source, QueryOptions options)
        {
            const string statusFilterKey = "Status";
            if (options.Filters.ContainsKey(statusFilterKey))
            {
                const string withErrorsStatus = "errors";
                if (options.Filters[statusFilterKey] != withErrorsStatus)
                {
                    int statusValue;
                    if (int.TryParse(options.Filters[statusFilterKey], out statusValue))
                    {
                        var status = (JobExecutionStatus)statusValue;
                        source = source.Where(jExecution => jExecution.Status == status);
                    }
                }
                else
                {
                    source = source.Where(jExecution => jExecution.Errors != null);
                }
            }
            return source;
        }

        /// <summary>
        /// Applies the start date filter.
        /// </summary>
        /// <param name="source">The source query object.</param>
        /// <param name="options">The grid options.</param>
        /// <returns>Query with applied start date filter.</returns>
        public static IQueryable<JobRequestExecution> ApplyStartDateFilter(this IQueryable<JobRequestExecution> source, QueryOptions options)
        {
            const string startDateFilterKey = "StartTime";
            if (options.Filters.ContainsKey(startDateFilterKey))
            {
                DateTime dateFilterValue = default(DateTime);
                if (DateTime.TryParse(options.Filters[startDateFilterKey], out dateFilterValue))
                {
                    // Filtering should be applied in local time. In db dates stored in UTC.
                    // Filter values are first tick and last tick of the day of local time converted to UTC.
                    DateTime startTimeOfDayUtc = dateFilterValue.Date.ToUniversalTime();
                    DateTime endTimeOfDayUtc = startTimeOfDayUtc.AddDays(1).AddTicks(-1).ToUniversalTime();
                    source = source.Where(jExecution => jExecution.StartTime.Value >= startTimeOfDayUtc && jExecution.StartTime.Value <= endTimeOfDayUtc);
                }
            }
            return source;
        }

        /// <summary>
        /// Applies the start time sorting.
        /// </summary>
        /// <param name="source">The source query object.</param>
        /// <param name="options">The grid options.</param>
        /// <returns>Query with applied  start time sorting.</returns>
        public static IQueryable<JobRequestExecution> ApplyStartTimeSorting(this IQueryable<JobRequestExecution> source, QueryOptions options)
        {
            source = source.OrderByDescending(p => p.StartTime);
            return source;
        }

        /// <summary>
        /// Applies the paging.
        /// </summary>
        /// <param name="source">The source query object.</param>
        /// <param name="options">The grid options.</param>
        /// <returns>Query will applied paging.</returns>
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