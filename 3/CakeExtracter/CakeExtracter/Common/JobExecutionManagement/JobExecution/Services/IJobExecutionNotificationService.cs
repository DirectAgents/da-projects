using System.Collections.Generic;
using DirectAgents.Domain.Entities.Administration.JobExecution;

namespace CakeExtracter.Common.JobExecutionManagement.JobExecution.Services
{
    /// <summary>
    /// Job Execution Notification Service.
    /// </summary>
    public interface IJobExecutionNotificationService
    {
        /// <summary>
        /// Gets the execution items for error notifying.
        /// </summary>
        /// <returns>Collection of jobs with error that was not notified.</returns>
        List<JobRequestExecution> GetExecutionItemsForErrorNotifying();

        /// <summary>
        /// Notifies the about failed jobs.
        /// </summary>
        /// <param name="jobsToNotify">The jobs to notify.</param>
        void NotifyAboutFailedJobs(List<JobRequestExecution> jobsToNotify);
    }
}