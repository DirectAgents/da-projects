using System;
using DirectAgents.Domain.Entities.Administration.JobExecution;

namespace CakeExtracter.Common.JobExecutionManagement.JobExecution
{
    /// <summary>
    /// Job execution history writer. Clent for commands to manage execution history.
    /// </summary>
    public class JobExecutionDataWriter : IJobExecutionDataWriter
    {
        private JobRequestExecution jobRequestExecutionData;

        private IJobExecutionItemService jobExecutionItemService;

        /// <summary>
        /// Hidden singleton constructor.
        /// </summary>
        public JobExecutionDataWriter(IJobExecutionItemService jobExecutionHistoryItemService)
        {
            this.jobExecutionItemService = jobExecutionHistoryItemService;
        }

        /// <summary>
        /// Inits job execution history item.
        /// </summary>
        /// <param name="profileNumber">The profile number.</param>
        /// <exception cref="System.Exception">Error occured while extracting profile number configuration. Check Execution profiles configs.</exception>
        public void InitCurrentExecutionHistoryItem(JobRequest jobRequest)
        {
            if (jobRequestExecutionData != null)
            {
                throw new Exception("Job execution history item already initialised. Can't be created twice in scope of one command execution");
            }
            else
            {
                jobRequestExecutionData = jobExecutionItemService.CreateJobExecutionItem(jobRequest);
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
            jobExecutionItemService.SetJobExecutionItemFailedState(jobRequestExecutionData);
        }

        public void SetCurrentTaskFinishedStatus()
        {
            jobExecutionItemService.SetJobExecutionItemFinishedState(jobRequestExecutionData);
        }
    }
}
