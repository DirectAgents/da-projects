using System;
using DirectAgents.Domain.Entities.Administration.JobExecution;

namespace CakeExtracter.Common.JobExecutionManagement.JobExecution
{
    /// <summary>
    /// Job execution history writer. Clent for commands to manage execution history.
    /// </summary>
    public class JobExecutionDataWriter : IJobExecutionDataWriter
    {
        private JobRequestExecution jobRequestExecutionItem;

        private IJobExecutionItemService jobExecutionItemService;

        /// <summary>
        /// Hidden singleton constructor.
        /// </summary>
        public JobExecutionDataWriter(IJobExecutionItemService jobExecutionItemService)
        {
            this.jobExecutionItemService = jobExecutionItemService;
        }

        /// <summary>
        /// Inits job execution history item.
        /// </summary>
        /// <param name="profileNumber">The profile number.</param>
        /// <exception cref="System.Exception">Error occured while extracting profile number configuration. Check Execution profiles configs.</exception>
        public void InitCurrentExecutionHistoryItem(JobRequest jobRequest)
        {
            if (jobRequestExecutionItem != null)
            {
                throw new Exception("Job execution history item already initialised. Can't be created twice in scope of one command execution");
            }
            else
            {
                jobRequestExecutionItem = jobExecutionItemService.CreateJobExecutionItem(jobRequest);
            }
        }

        /// <summary>
        /// Logs the error in job execution history.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="accountId"></param>
        public void LogErrorInHistory(string message, int? accountId = null)
        {
            if (jobRequestExecutionItem != null)
            {
                jobExecutionItemService.AddErrorToJobExecutionItem(jobRequestExecutionItem, message, accountId);
            }
        }

        /// <summary>
        /// Logs the warning in job execution history history.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="accountId"></param>
        public void LogWarningInHistory(string message, int? accountId = null)
        {
            if (jobRequestExecutionItem != null)
            {
                jobExecutionItemService.AddWarningToJobExecutionItem(jobRequestExecutionItem, message, accountId);
            }
        }

        /// <summary>
        /// Sets the state in job execution history history.
        /// </summary>
        /// <param name="stateMesasge">The state mesasge.</param>
        /// <param name="accountId">The account identifier.</param>
        public void SetStateInHistory(string stateMesasge, int? accountId = null)
        {
            if (jobRequestExecutionItem != null)
            {
                jobExecutionItemService.AddStateMessage(jobRequestExecutionItem, stateMesasge, accountId);
            }
        }

        /// <summary>
        /// Sets the current task failed status in job execution history.
        /// </summary>
        public void SetCurrentTaskFailedStatus()
        {
            if (jobRequestExecutionItem != null)
            {
                jobExecutionItemService.SetJobExecutionItemFailedState(jobRequestExecutionItem);
            }
        }

        /// <summary>
        /// Sets the current task finish time. Set status to completed.
        /// </summary>
        public void SetCurrentTaskFinishedStatus()
        {
            if (jobRequestExecutionItem != null)
            {
                jobExecutionItemService.SetJobExecutionItemFinishedState(jobRequestExecutionItem);
            }
        }
    }
}
