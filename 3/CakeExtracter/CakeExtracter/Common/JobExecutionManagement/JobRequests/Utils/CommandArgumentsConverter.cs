using System.Linq;
using System.Reflection;
using DirectAgents.Domain.Entities.Administration.JobExecution;
using Mono.Options;

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
            var argValueProperties = command.GetArgumentProperties().ToArray();
            return argOptions.Select((t, i) => GetArgument(command, t, argValueProperties[i]))
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

        private static string GetArgument(ConsoleCommand command, Option argOption, PropertyInfo argValueProperty)
        {
            var argValue = argValueProperty.GetValue(command);
            if (argValue == null)
            {
                return null;
            }

            var argName = argOption.GetNames().First();
            return GetArgument(argName, argValue);
        }

        private static string GetArgument(string argName, object argValue)
        {
            return $"{ArgumentsPrefix}{argName}{ArgumentNameAndValueSeparator}{argValue}";
        }
    }
}
