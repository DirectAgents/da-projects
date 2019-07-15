using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Entities.Administration.JobExecution;
using ManyConsole.Internal;

namespace CakeExtracter.Common.JobExecutionManagement.JobRequests.Utils
{
    public static class CommandVerificationUtil
    {
        public static List<ManyConsole.ConsoleCommand> AllExistingCommands { get; }

        static CommandVerificationUtil()
        {
            AllExistingCommands =
                ManyConsole.ConsoleCommandDispatcher.FindCommandsInSameAssemblyAs(typeof(ConsoleCommand)).ToList();
        }

        public static ManyConsole.ConsoleCommand GetCommandByName(string commandName)
        {
            return AllExistingCommands.Find(x => x.Command == commandName);
        }

        public static void VerifyJobRequest(JobRequest jobRequest)
        {
            var command = GetCommandByName(jobRequest.CommandName);
            var commandLine = CommandArgumentsConverter.GetJobArgumentsAsArgumentsForConsole(jobRequest);
            var arguments = CommandLineParser.Parse(commandLine);
            var remainingArguments = command.GetActualOptions().Parse(arguments.Skip(1));
            ConsoleUtil.VerifyNumberOfArguments(
                remainingArguments.ToArray(),
                command.RemainingArgumentsCountMin,
                command.RemainingArgumentsCountMax);
            command.OverrideAfterHandlingArgumentsBeforeRun(remainingArguments.ToArray());
        }
    }
}
