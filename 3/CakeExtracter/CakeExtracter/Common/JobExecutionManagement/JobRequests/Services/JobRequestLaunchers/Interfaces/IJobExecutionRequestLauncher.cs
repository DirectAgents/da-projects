namespace CakeExtracter.Common.JobExecutionManagement.JobRequests.Services.JobRequestLaunchers.Interfaces
{
    /// <summary>
    /// The service for work with launch of job request items.
    /// </summary>
    public interface IJobExecutionRequestLauncher
    {
        /// <summary>
        /// Runs all past job requests, if available.
        /// </summary>
        /// <param name="maxNumberOfJobRequests">The maximum number of attempts for each request.</param>
        /// <param name="maxNumberOfRunningRequests">The maximum number of processes that execute job requests.</param>
        void ExecuteScheduledInPastJobRequests(int maxNumberOfJobRequests, int maxNumberOfRunningRequests);
    }
}
