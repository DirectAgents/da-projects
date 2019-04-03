using System;
using System.ComponentModel.DataAnnotations;
using DirectAgents.Domain.Entities.Administration.JobExecution.Enums;

namespace DirectAgents.Domain.Entities.Administration.JobExecution
{
    /// <summary>
    /// The entity for a job execution request item.
    /// </summary>
    public class JobRequest
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier of a parent job request that caused this request.
        /// </summary>
        /// <value>
        /// The parent job request identifier.
        /// </value>
        public int? ParentJobRequestId { get; set; }

        /// <summary>
        /// Gets or sets the name of the command.
        /// </summary>
        /// <value>
        /// The name of the command.
        /// </value>
        [Required]
        public string CommandName { get; set; }

        /// <summary>
        /// Gets or sets the execution arguments in a view for the console.
        /// </summary>
        /// <value>
        /// The execution arguments.
        /// </value>
        public string CommandExecutionArguments { get; set; }

        /// <summary>
        /// Gets or sets the job execution time for this request, if the request is to be scheduled.
        /// </summary>
        /// <value>
        /// The scheduled time.
        /// </value>
        public DateTime? ScheduledTime { get; set; }

        /// <summary>
        /// Gets or sets the attempt number of this request execution.
        /// </summary>
        /// <value>
        /// The attempt number.
        /// </value>
        public int AttemptNumber { get; set; }

        /// <summary>
        /// Job request status. <see cref="JobRequestStatus"/>
        /// </summary>
        /// <value>
        /// The request status.
        /// </value>
        public JobRequestStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the parent job request that caused this request. <see cref="JobRequest"/>
        /// </summary>
        /// <value>
        /// The parent job request.
        /// </value>
        public virtual JobRequest ParentJobRequest { get; set; }
    }
}
