using System;
using DirectAgents.Domain.Entities.Administration.JobExecution;

namespace CakeExtracter.Common.ExecutionHistory.ExecutionHistoryManagement
{
    /// <summary>
    /// Job execution history writer. Clent for commands to manage execution history.
    /// </summary>
    public class JobExecutionDataWriter : IJobExecutionDataWriter
    {
        private JobRequestExecution jobRequestExecutionData;

        private IJobExecutionItemService jobExecutionHistoryItemService;

        /// <summary>
        /// Hidden singleton constructor.
        /// </summary>
        public JobExecutionDataWriter(IJobExecutionItemService jobExecutionHistoryItemService)
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
            if (jobRequestExecutionData != null)
            {
                throw new Exception("Job execution history item already initialised. Can't be created twice in scope of one command execution");
            }
            else
            {
                jobRequestExecutionData = jobExecutionHistoryItemService.CreateJobRequestExecutionItem(commandName, arguments);
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
        }

        public void SetCurrentTaskFinishTime()
        {

        }
    }
}
