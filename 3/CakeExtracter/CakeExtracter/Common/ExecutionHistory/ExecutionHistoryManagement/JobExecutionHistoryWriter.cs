using DirectAgents.Domain.Entities.Administration.JobExecutionHistory;
using System;

namespace CakeExtracter.Common.ExecutionHistory.ExecutionHistoryManagement
{
    /// <summary>
    /// Job execution history writer. Clent for commands to manage execution history.
    /// </summary>
    public class JobExecutionHistoryWriter : IJobExecutionHistoryWriter
    {
        private JobExecutionHistoryItem scopeJobExecutionHistoryitem;

        private IJobExecutionHistoryItemService jobExecutionHistoryItemService;

        /// <summary>
        /// Hidden singleton constructor.
        /// </summary>
        public JobExecutionHistoryWriter(IJobExecutionHistoryItemService jobExecutionHistoryItemService)
        {
            this.jobExecutionHistoryItemService = jobExecutionHistoryItemService;
        }

        /// <summary>
        /// Inits job execution history item.
        /// </summary>
        /// <param name="profileNumber">The profile number.</param>
        /// <exception cref="System.Exception">Error occured while extracting profile number configuration. Check Execution profiles configs.</exception>
        public void InitCurrentExecutionHistoryItem(string commandName, string[] arguments)
        {
            if (scopeJobExecutionHistoryitem != null)
            {
                throw new Exception("Job execution history item already initialised. Can't be created twice in scope of one command execution");
            }
            else
            {
                scopeJobExecutionHistoryitem = jobExecutionHistoryItemService.CreateJobExecutionHistoryItem(commandName, arguments);
            }
        }

        public void LogErrorInHistory()
        {
            throw new NotImplementedException();
        }

        public void LogWarningInHistory()
        {
            throw new NotImplementedException();
        }

        public void SetCurrentTaskFailedStatus()
        {
            if (scopeJobExecutionHistoryitem != null)
            {
                jobExecutionHistoryItemService.SetExecutionHistoryItemFailedState(scopeJobExecutionHistoryitem);
            }
            else
            {
                throw new Exception("Job execution history is not initialised yet.");
            }
        }

        public void SetCurrentTaskFinishTime()
        {
            if (scopeJobExecutionHistoryitem != null)
            {
                jobExecutionHistoryItemService.SetExecutionHistoryItemFinishedState(scopeJobExecutionHistoryitem);
            }
            else
            {
                throw new Exception("Job execution history is not initialised yet.");
            }
        }
    }
}
