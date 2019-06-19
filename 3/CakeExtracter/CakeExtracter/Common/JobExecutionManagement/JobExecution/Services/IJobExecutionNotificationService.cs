namespace CakeExtracter.Common.JobExecutionManagement.JobExecution.Services
{
    /// <summary>
    /// Job Execution Notification Service.
    /// </summary>
    public interface IJobExecutionNotificationService
    {
        /// <summary>
        /// Notifies the about failed jobs. (Critical Level)
        /// Sends emails if job failed (job execution failed and at least one retry jobs failed). Retries didn't help.
        /// </summary>
        /// <param name="jobsToNotify">The jobs to notify.</param>
        void NotifyAboutFailedJobs();

        /// <summary>
        /// Notifies the about errors in jobs. (Warning level)
        /// Sends email if error occurred while processing job. Warning for business. Retries can fix.
        /// </summary>
        void NotifyAboutErrorsInJobExecution();
    }
}