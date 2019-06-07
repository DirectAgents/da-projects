using DirectAgents.Domain.Entities.Administration.JobExecution;

namespace CakeExtracter.Common.JobExecutionManagement.JobExecution.Models
{
    /// <summary>
    /// Failed Job Notification model.
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
    }
}
