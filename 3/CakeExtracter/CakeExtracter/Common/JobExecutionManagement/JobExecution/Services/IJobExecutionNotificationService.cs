namespace CakeExtracter.Common.JobExecutionManagement.JobExecution.Services
{
    /// <summary>
    /// Job Execution Notification Service.
    /// </summary>
    public interface IJobExecutionNotificationService
    {
        /// <summary>
        /// Notifies the about failed jobs.
        /// </summary>
        /// <param name="jobsToNotify">The jobs to notify.</param>
        void NotifyAboutFailedJobs();
    }
}