using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Entities.Administration.JobExecution;
using ManyConsole.Internal;

namespace CakeExtracter.Common.JobExecutionManagement.JobRequests.Utils
{
    /// <summary>
    /// The utility that helps to get info about existing commands and the correctness of commands.
    /// </summary>
    public static class CommandVerificationUtil
    {
        /// <summary>
        /// Gets all commands that exist in the solution.
        /// </summary>
        public static List<ManyConsole.ConsoleCommand> AllExistingCommands { get; }

        static CommandVerificationUtil()
        {
            AllExistingCommands =
                ManyConsole.ConsoleCommandDispatcher.FindCommandsInSameAssemblyAs(typeof(ConsoleCommand)).ToList();
        }

        /// <summary>
        /// Returns a standard command class object based on its command name.
        /// </summary>
        /// <param name="commandName">The command name.</param>
        /// <returns>The command.</returns>
        public static ManyConsole.ConsoleCommand GetCommandByName(string commandName)
        {
            return AllExistingCommands.Find(x => x.Command == commandName);
        }

        /// <summary>
        /// Checks the job request, throws an exception if the command name or job request arguments are incorrect.
        /// </summary>
        /// <param name="jobRequest">The job request for checking.</param>
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
