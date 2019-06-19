using DirectAgents.Domain.Entities.Administration.JobExecution;

namespace CakeExtracter.Common.JobExecutionManagement.JobRequests.Services.JobRequestSchedulers.Interfaces
{
    /// <summary>
    /// Job Request Life Cycle Manager.
    /// </summary>
    public interface IJobRequestLifeCycleManager
    {
        /// <summary>
        /// Returns a saved job request object for a command.
        /// </summary>
        /// <param name="command">The command for which job request is extracting.</param>
        /// <returns>The saved job request object.</returns>
        JobRequest GetJobRequest(ConsoleCommand command);

        /// <summary>
        /// Sets a status of the job request to Processing.
        /// </summary>
        /// <param name="request">The job request.</param>
        void SetJobRequestAsProcessing(JobRequest request);

        /// <summary>
        /// Reschedules the job request.
        /// </summary>
        /// <param name="request">The job request.</param>
        /// <param name="sourceCommand">The source command of the request.</param>
        void ProcessFailedRequest(JobRequest request, ConsoleCommand sourceCommand);

        /// <summary>
        /// Creates job requests for scheduled commands based on the current command and job request.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="request">The request.</param>
        /// <param name="retryRequestsHolder">The retry requests holder.</param>
        void ProcessCompletedRequest(ConsoleCommand command, JobRequest request, RetryRequestsHolder retryRequestsHolder);

        /// <summary>
        /// Ends the job request.
        /// </summary>
        /// <param name="request">The job request.</param>
        void ProcessAbortedRequest(JobRequest request);

        /// <summary>
        /// Updates the status of retry pending jobs. Checks all retry pending jobs and it's retries jobs and actualize it's status.
        /// </summary>
        void ActualizeStatusOfRetryPendingJobs();
    }
}
