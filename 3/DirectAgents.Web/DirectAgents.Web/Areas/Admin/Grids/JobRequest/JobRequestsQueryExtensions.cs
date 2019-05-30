using System.Linq;
using DirectAgents.Domain.Entities.Administration.JobExecution.Enums;
using MVCGrid.Models;

namespace DirectAgents.Web.Areas.Admin.Grids.JobRequest
{
    /// <summary>
    /// Job requests query extensions for applying paging and filtering.
    /// </summary>
    public static class JobRequestsQueryExtensions
    {
        /// <summary>
        /// Applies the status filter.
        /// </summary>
        /// <param name="source">The source query object.</param>
        /// <param name="options">The grid options.</param>
        /// <returns>Query with applied status filter.</returns>
        public static IQueryable<Domain.Entities.Administration.JobExecution.JobRequest> ApplyStatusFilter(
            this IQueryable<Domain.Entities.Administration.JobExecution.JobRequest> source, QueryOptions options)
        {
            var status = JobRequestStatus.Scheduled;
            source = source.Where(x => x.Status == status);
            return source;
        }

        /// <summary>
        /// Applies the schedule time sorting.
        /// </summary>
        /// <param name="source">The source query object.</param>
        /// <param name="options">The grid options.</param>
        /// <returns>Query with applied schedule time sorting.</returns>
        public static IQueryable<Domain.Entities.Administration.JobExecution.JobRequest> ApplyScheduleTimeSorting(
            this IQueryable<Domain.Entities.Administration.JobExecution.JobRequest> source, QueryOptions options)
        {
            source = source.OrderBy(x => x.ScheduledTime);
            return source;
        }

        /// <summary>
        /// Applies the paging.
        /// </summary>
        /// <param name="source">The source query object.</param>
        /// <param name="options">The grid options.</param>
        /// <returns>Query will applied paging.</returns>
        public static IQueryable<Domain.Entities.Administration.JobExecution.JobRequest> ApplyPaging(
            this IQueryable<Domain.Entities.Administration.JobExecution.JobRequest> source, QueryOptions options)
        {
            if (options.GetLimitOffset().HasValue)
            {
                source = source.Skip(options.GetLimitOffset().Value).Take(options.GetLimitRowcount().Value);
            }

            return source;
        }
    }
}