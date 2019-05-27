using System;
using DirectAgents.Domain.Entities.Administration.JobExecution.Enums;

namespace DirectAgents.Domain.Entities.Administration.JobExecution
{
    /// <summary>
    /// The entity for a job request execution item. One item per one job execution.
    /// </summary>
    public class JobRequestExecution
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier of a job request.
        /// </summary>
        /// <value>
        /// The request identifier.
        /// </value>
        public int JobRequestId { get; set; }

        /// <summary>
        /// Gets or sets job execution start time.
        /// </summary>
        /// <value>
        /// The start time.
        /// </value>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// Gets or sets job execution end time.
        /// </summary>
        /// <value>
        /// The end time.
        /// </value>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets job execution status. <see cref="JobExecutionStatus"/>.
        /// </summary>
        /// <value>
        /// The execution status.
        /// </value>
        public JobExecutionStatus Status { get; set; }

        /// <summary>
        /// Gets or sets job error messages.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        public string Errors { get; set; }

        /// <summary>
        /// Gets or sets job warning messages.
        /// </summary>
        /// <value>
        /// The warnings.
        /// </value>
        public string Warnings { get; set; }

        /// <summary>
        /// Gets or sets job free status description. Contain information about execution state of job.
        /// </summary>
        /// <value>
        /// The state of the current.
        /// </value>
        public string CurrentState { get; set; }

        /// <summary>
        /// Gets or sets the job request. <see cref="JobRequest"/>.
        /// </summary>
        /// <value>
        /// The job request.
        /// </value>
        public virtual JobRequest JobRequest { get; set; }
    }
}
