using DirectAgents.Domain.Entities.Administration.JobExecution;

namespace CakeExtracter.Common.JobExecutionManagement.JobExecution.Models
{
    /// <summary>
    /// Notification model for processing job.
    /// </summary>
    public class ProcessingJobNotificationModel
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
