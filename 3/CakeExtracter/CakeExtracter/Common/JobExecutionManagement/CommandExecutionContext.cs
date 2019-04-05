﻿using System;
using CakeExtracter.Common.JobExecutionManagement.JobExecution;
using CakeExtracter.Common.JobExecutionManagement.JobRequests;
using DirectAgents.Domain.Entities.Administration.JobExecution;

namespace CakeExtracter.Common.JobExecutionManagement
{
    internal class CommandExecutionContext
    {
        public static CommandExecutionContext Current;

        private JobRequest currentJobRequest;
        private ConsoleCommand currentCommand;

        public JobExecutionDataWriter JobDataWriter { get; }

        public JobExecutionRequestManager JobRequestManager { get; }

        private CommandExecutionContext(ConsoleCommand command)
        {
            var executionItemRepository = new JobExecutionItemRepository();
            var executionItemService = new JobExecutionItemService(executionItemRepository);
            JobDataWriter = new JobExecutionDataWriter(executionItemService);
            JobRequestManager = new JobExecutionRequestManager();
            InitCurrentJobRequest(command);
        }

        public static void InitContext(ConsoleCommand command)
        {
            if (Current != null)
            {
                throw new Exception("Execution context already initialized.");
            }

            Current = new CommandExecutionContext(command);
        }

        public void StartRequestExecution()
        {
            JobRequestManager.MarkJobRequestAsProcessing(currentJobRequest);
            JobDataWriter.InitCurrentExecutionHistoryItem(currentJobRequest);
        }

        public void CompleteRequestExecution()
        {
            JobDataWriter.SetCurrentTaskFinishedStatus();
            JobRequestManager.CreateRequestsForScheduledCommands(currentCommand, currentJobRequest);
        }

        public void FailRequestExecution()
        {
            JobDataWriter.SetCurrentTaskFailedStatus();
            var scheduledTime = DateTime.Now.AddMinutes(currentCommand.IntervalBetweenUnsuccessfulAndNewRequestInMinutes);
            JobRequestManager.RescheduleRequest(currentJobRequest, scheduledTime);
        }

        private void InitCurrentJobRequest(ConsoleCommand command)
        {
            currentCommand = command;
            currentJobRequest = JobRequestManager.GetJobRequest(currentCommand);
        }
    }
}
