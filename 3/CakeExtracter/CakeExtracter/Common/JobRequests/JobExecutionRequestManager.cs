using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using CakeExtracter.SimpleRepositories;
using CakeExtracter.SimpleRepositories.BasicRepositories.Interfaces;
using DirectAgents.Domain.Entities.Administration.JobExecution;
using DirectAgents.Domain.Entities.Administration.JobExecution.Enums;
using Mono.Options;

namespace CakeExtracter.Common.JobRequests
{
    public class JobExecutionRequestManager<T>
        where T : ConsoleCommand
    {
        private const string ArgumentsSeparator = "   ;   ";
        private const string ArgumentsSeparatorForConsole = " ";
        private const string ArgumentsPrefix = "-";
        private const string ArgumentNameAndValueSeparator = " ";

        private readonly T sourceCommand;
        private readonly IBasicRepository<JobRequest> requestRepository;
        private readonly IEnumerable<ManyConsole.ConsoleCommand> commands;

        public T SourceCommand => (T) sourceCommand.Clone();

        public JobExecutionRequestManager(T command)
        {
            sourceCommand = command;
            requestRepository = RepositoriesContainer.GetJobRequestRepository();
            commands = ManyConsole.ConsoleCommandDispatcher.FindCommandsInAllLoadedAssemblies();
        }

        public void AddJobRequest(T command)
        {
            AddJobRequest(command, DateTime.Now);
        }

        public void AddJobRequest(T command, DateTime scheduledTime)
        {
            var request = CreateJobRequest(command, scheduledTime);
            requestRepository.AddItem(request);

            ExecuteJobRequest(request);
        }

        public void ExecuteJobRequest(JobRequest request)
        {
            UpdateRequestStatus(request, JobRequestStatus.Processing);
            RunRequestInNewProcess(request);
            UpdateRequestStatus(request, JobRequestStatus.Completed);
        }

        private JobRequest CreateJobRequest(T command, DateTime scheduledTime)
        {
           return new JobRequest
            {
                AttemptNumber = 0,
                CommandName = command.Command,
                CommandExecutionArguments = GetCommandArgumentsAsLine(command),
                ScheduledTime = scheduledTime,
                Status = JobRequestStatus.Scheduled
            };
        }

        private void RunRequestInNewProcess(JobRequest request)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = Assembly.GetEntryAssembly().Location,
                Arguments = GetJobArgumentsAsArgumentsForConsole(request)
            };
            Process.Start(startInfo);
        }

        private void UpdateRequestStatus(JobRequest request, JobRequestStatus status)
        {
            request.Status = status;
            requestRepository.UpdateItem(request);
        }

        private string GetCommandArgumentsAsLine(ConsoleCommand command)
        {
            var argOptions = command.GetActualOptions();
            argOptions.Remove(ConsoleCommand.RequestIdArgumentName);
            var argValueProperties = command.GetArgumentProperties().ToArray();
            return argOptions.Select((t, i) => GetArgument(command, t, argValueProperties[i]))
                .Aggregate(string.Empty, (s, s1) => JoinArguments(s, s1));
        }

        private string GetJobArgumentsAsArgumentsForConsole(JobRequest request)
        {
            var requestArgument = GetArgument(ConsoleCommand.RequestIdArgumentName, request.Id);
            var arguments = JoinArguments(request.CommandName, request.CommandExecutionArguments, requestArgument);
            var argumentsForConsole = arguments.Replace(ArgumentsSeparator, ArgumentsSeparatorForConsole);
            return argumentsForConsole;
        }

        private string JoinArguments(params string[] arguments)
        {
            return string.Join(ArgumentsSeparator, arguments);
        }

        private string GetArgument(ConsoleCommand command, Option argOption, PropertyInfo argValueProperty)
        {
            var argValue = argValueProperty.GetValue(command);
            if (argValue == null)
            {
                return null;
            }

            var argName = argOption.GetNames().First();
            return GetArgument(argName, argValue);
        }

        private string GetArgument(string argName, object argValue)
        {
            return $"{ArgumentsPrefix}{argName}{ArgumentNameAndValueSeparator}{argValue}";
        }
    }
}
