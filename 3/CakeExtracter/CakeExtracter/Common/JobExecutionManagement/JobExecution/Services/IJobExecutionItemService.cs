using DirectAgents.Domain.Entities.Administration.JobExecution;

namespace CakeExtracter.Common.JobExecutionManagement.JobExecution.Services
{
    /// <summary>
    /// Service for work with job execution items.
    /// </summary>
    public interface IJobExecutionItemService
    {
        /// <summary>
        /// Creates the job execution item.
        /// </summary>
        /// <param name="jobRequest">The job request to create.</param>
        /// <returns>Created job execution item.</returns>
        JobRequestExecution CreateJobExecutionItem(JobRequest jobRequest);

        /// <summary>
        /// Sets the state of the job execution item finished.
        /// </summary>
        /// <param name="executionHistoryItem">The execution history item.</param>
        void SetJobExecutionItemFinishedState(JobRequestExecution executionHistoryItem);

        /// <summary>
        /// Sets the state of the job execution item failed.
        /// </summary>
        /// <param name="executionHistoryItem">The execution history item.</param>
        void SetJobExecutionItemFailedState(JobRequestExecution executionHistoryItem);

        /// <summary>
        /// Sets the state of the job execution items to aborted.
        /// </summary>
        /// <param name="itemIds">The item ids.</param>
        void SetJobExecutionItemsAbortedState(int[] itemIds);

        /// <summary>
        /// Sets the state of the job execution item to aborted by timeout.
        /// </summary>
        /// <param name="currentJobRequestExecution">The current job request execution.</param>
        void SetJobExecutionItemAbortedByTimeoutState(JobRequestExecution currentJobRequestExecution);

        /// <summary>
        /// Adds the warning to job execution item.
        /// </summary>
        /// <param name="executionHistoryItem">The execution history item.</param>
        /// <param name="message">The message.</param>
        /// <param name="accountId">The account identifier.</param>
        void AddWarningToJobExecutionItem(JobRequestExecution executionHistoryItem, string message, int? accountId = null);

        /// <summary>
        /// Adds the error to job execution item.
        /// </summary>
        /// <param name="executionHistoryItem">The execution history item.</param>
        /// <param name="message">The message.</param>
        /// <param name="accountId">The account identifier.</param>
        void AddErrorToJobExecutionItem(JobRequestExecution executionHistoryItem, string message, int? accountId = null);

        /// <summary>
        /// Sets the state message.
        /// </summary>
        /// <param name="executionHistoryItem">The execution history item.</param>
        /// <param name="message">The message.</param>
        /// <param name="accountId">The account identifier.</param>
        void SetStateMessage(JobRequestExecution executionHistoryItem, string message, int? accountId = null);

        /// <summary>
        /// Adds the state message to job execution item.
        /// </summary>
        /// <param name="executionHistoryItem">The execution history item.</param>
        /// <param name="message">The message.</param>
        /// <param name="accountId">The account identifier.</param>
        void AddStateMessage(JobRequestExecution executionHistoryItem, string message, int? accountId = null);
    }
}
