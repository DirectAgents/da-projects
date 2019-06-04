﻿using DirectAgents.Domain.Entities.Administration.JobExecution;

namespace CakeExtracter.Common.JobExecutionManagement.JobExecution.Models
{
    /// <summary>
    /// Notification model for the failed job.
    /// </summary>
    public class FailedJobNotificationModel
    {
        /// <summary>
        /// Gets or sets the job request.
        /// </summary>
        /// <value>
        /// The job request.
        /// </value>
        public JobRequest JobRequest { get; set; }

        /// <summary>
        /// Gets or sets the job request execution.
        /// </summary>
        /// <value>
        /// The job request execution.
        /// </value>
        public JobRequestExecution JobRequestExecution { get; set; }

        /// <summary>
        /// Gets or sets the execution start time.
        /// </summary>
        /// <value>
        /// The execution start time.
        /// </value>
        public string ExecutionStartTime { get; set; }

        /// <summary>
        /// Gets or sets the errors.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        public JobExecutionLogData Errors { get; set; }
    }
}
