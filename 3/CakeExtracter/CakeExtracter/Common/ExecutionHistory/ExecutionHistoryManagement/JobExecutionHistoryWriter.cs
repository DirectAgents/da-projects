using CakeExtracter.Common.ExecutionHistory.Constants;
using CakeExtracter.Common.ExecutionHistory.Models;
using System;

namespace CakeExtracter.Common.ExecutionHistory.ExecutionHistoryManagement
{
    public class JobExecutionHistoryWriter
    {
        private JobExecutionHistoryItem jobExecutionHistoryitem;

        /// <summary>
        /// Hidden singleton constructor.
        /// </summary>
        private JobExecutionHistoryWriter()
        {
        }

        /// <summary>
        /// Access point for singleton object.
        /// </summary>
        public static JobExecutionHistoryWriter Current = new JobExecutionHistoryWriter();

        /// <summary>
        /// Inits job execution history item.
        /// </summary>
        /// <param name="profileNumber">The profile number.</param>
        /// <exception cref="System.Exception">Error occured while extracting profile number configuration. Check Execution profiles configs.</exception>
        public void InitCurrentExecutionHistoryItem(string comamndName)
        {
            if (jobExecutionHistoryitem != null)
            {
                throw new Exception("Job execution history item already initialised. Can't be created twice in scope of one command execution");
            }
            else
            {
                jobExecutionHistoryitem = new JobExecutionHistoryItem
                {
                    Status = JobStatuses.Processing,
                    CommandName = comamndName,
                    StartDate = DateTime.UtcNow
                };
            }
        }
    }
}
