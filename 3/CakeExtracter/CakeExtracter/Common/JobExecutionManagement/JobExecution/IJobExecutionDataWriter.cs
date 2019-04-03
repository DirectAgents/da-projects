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
        /// Sets the current task failed status.
        /// </summary>
        void SetCurrentTaskFailedStatus();

        /// <summary>
        /// Logs the error in history.
        /// </summary>
        void LogErrorInHistory();

        /// <summary>
        /// Logs the warning in history.
        /// </summary>
        void LogWarningInHistory();
    }
}
