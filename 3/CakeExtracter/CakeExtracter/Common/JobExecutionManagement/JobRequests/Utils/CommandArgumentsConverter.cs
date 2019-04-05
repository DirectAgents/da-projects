using System;
using System.Linq;
using DirectAgents.Domain.Entities.Administration.JobExecution;

namespace CakeExtracter.Common.JobExecutionManagement.JobRequests.Utils
{
    internal static class CommandArgumentsConverter
    {
        private const string ArgumentsSeparator = "   ;   ";
        private const string ArgumentsSeparatorForConsole = " ";
        private const string ArgumentsPrefix = "-";
        private const string ArgumentNameAndValueSeparator = " ";

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

        public static string GetJobArgumentsAsArgumentsForConsole(JobRequest request)
        {
            var requestArgument = GetArgument(ConsoleCommand.RequestIdArgumentName, request.Id);
            var arguments = JoinArguments(request.CommandName, request.CommandExecutionArguments, requestArgument);
            var argumentsForConsole = arguments.Replace(ArgumentsSeparator, ArgumentsSeparatorForConsole);
            return argumentsForConsole;
        }

        private static string JoinArguments(params string[] arguments)
        {
            return string.Join(ArgumentsSeparator, arguments);
        }

        private static string GetArgument(string argName, object argValue)
        {
            return argValue != null
                ? $"{ArgumentsPrefix}{argName}{ArgumentNameAndValueSeparator}{argValue}"
                : string.Empty;
        }
    }
}
