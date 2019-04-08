using System;
using System.Linq;
using DirectAgents.Domain.Entities.Administration.JobExecution;

namespace CakeExtracter.Common.JobExecutionManagement.JobRequests.Utils
{
    /// <summary>
    /// The utility that helps convert command and request arguments to console arguments and vice versa.
    /// </summary>
    internal static class CommandArgumentsConverter
    {
        private const string ArgumentsSeparatorForConsole = " ";
        private const string ArgumentsPrefix = "-";
        private const string ArgumentNameAndValueSeparator = " ";
        private const string ArgumentStringValueSeparator = "\"";

        /// <summary>
        /// Converts arguments of a console command object to use to launch it from the console.
        /// </summary>
        /// <param name="command">The command to convert.</param>
        /// <returns>Command arguments to run from console.</returns>
        public static string GetCommandArgumentsAsLine(ConsoleCommand command)
        {
            var argOptions = command.GetActualOptions();
            argOptions.Remove(ConsoleCommand.RequestIdArgumentName);
            var argNames = argOptions.Select(x => x.GetNames().First()).ToArray();
            var argValues = command.GetArgumentProperties().Select(x => x.GetValue(command)).ToArray();
            Array.Sort(argNames, argValues);
            return argNames.Select((x, i) => GetArgument(x, argValues[i]))
                .Aggregate(string.Empty, (s, s1) => JoinArguments(s, s1));
        }

        /// <summary>
        /// Converts a job request object to use to run its command from the console.
        /// </summary>
        /// <param name="request">The job request to convert.</param>
        /// <returns>Job request arguments to run from console.</returns>
        public static string GetJobArgumentsAsArgumentsForConsole(JobRequest request)
        {
            var requestArgument = GetArgument(ConsoleCommand.RequestIdArgumentName, request.Id);
            var arguments = JoinArguments(request.CommandName, request.CommandExecutionArguments, requestArgument);
            return arguments;
        }

        private static string JoinArguments(params string[] arguments)
        {
            return string.Join(ArgumentsSeparatorForConsole, arguments);
        }

        private static string GetArgument(string argName, object argValue)
        {
            if (argValue is string || argValue is DateTime)
            {
                argValue = ArgumentStringValueSeparator + argValue + ArgumentStringValueSeparator;
            }

            return argValue != null
                ? $"{ArgumentsPrefix}{argName}{ArgumentNameAndValueSeparator}{argValue}"
                : string.Empty;
        }
    }
}
