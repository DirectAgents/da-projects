using CakeExtracter.Common.JobExecutionManagement.JobExecution;
using System;
using CakeExtracter.Common.JobExecutionManagement.JobRequests;
using DirectAgents.Domain.Entities.Administration.JobExecution;

namespace CakeExtracter.Common.JobExecutionManagement
{
    internal class CommandExecutionContext
    {
        public static CommandExecutionContext Current;

        private JobRequest currentJobRequest;

        public JobExecutionDataWriter JobDataWriter { get; }

        public JobExecutionRequestManager JobRequestManager { get; }

        private CommandExecutionContext(ConsoleCommand command, int? currentRequestId)
        {
            var executionItemRepository = new JobExecutionItemRepository();
            var executionItemService = new JobExecutionItemService(executionItemRepository);
            JobDataWriter = new JobExecutionDataWriter(executionItemService);
            JobRequestManager = new JobExecutionRequestManager(command);
            InitCurrentJobRequest(command, currentRequestId);
        }

        public static void InitContext(ConsoleCommand command, int? currentRequestId)
        {
            if (Current != null)
            {
                throw new Exception("Execution context already initialized.");
            }
            Current = new CommandExecutionContext(command, currentRequestId);
        }

        public void StartRequestExecution()
        {
            JobRequestManager.MarkJobRequestAsProcessing(currentJobRequest);
            JobDataWriter.InitCurrentExecutionHistoryItem(currentJobRequest);
        }

        public void CompleteRequestExecution()
        {
            JobDataWriter.SetCurrentTaskFinishedStatus();
        }

        public void FailRequestExecution()
        {
            JobDataWriter.SetCurrentTaskFailedStatus();
        }

        private void InitCurrentJobRequest(ConsoleCommand command, int? currentRequestId)
        {
            currentJobRequest = currentRequestId.HasValue
                ? JobRequestManager.GetJobRequest(currentRequestId.Value)
                : JobRequestManager.AddJobRequest(command);
        }
    }
}
