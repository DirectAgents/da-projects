using DirectAgents.Domain.Entities.Administration.JobExecution;

namespace CakeExtracter.Common.JobExecutionManagement.JobExecution
{
    /// <summary>
    /// Job execution history writer. Clent for commands to manage execution history.
    /// </summary>
    public interface IJobExecutionDataWriter
    {
        /// <summary>
        /// Inits job execution history item.
        /// </summary>
        /// <param name="profileNumber">The profile number.</param>
        /// <exception cref="System.Exception">Error occured while extracting profile number configuration. Check Execution profiles configs.</exception>
        void InitCurrentExecutionHistoryItem(JobRequest jobRequest);

        /// <summary>
        /// Sets the current task finish time. Set status to completed.
        /// </summary>
        void SetCurrentTaskFinishedStatus();

        /// <summary>
        /// Sets the current task failed status in job execution history.
        /// </summary>
        void SetCurrentTaskFailedStatus();

        /// <summary>
        /// Logs the error in job execution history.
        /// </summary>
        void LogErrorInHistory(string message, int? accountId = null);

        /// <summary>
        /// Logs the warning in job execution history.
        /// </summary>
        void LogWarningInHistory(string message, int? accountId = null);

        /// <summary>
        /// Sets the state in job execution history.
        /// </summary>
        /// <param name="stateMesasge">The state mesasge.</param>
        /// <param name="accountId">The account identifier.</param>
        void SetStateInHistory(string stateMesasge, int? accountId = null);
    }
}
