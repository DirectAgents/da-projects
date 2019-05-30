using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DirectAgents.Domain.Entities.Administration.JobExecution;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Mono.Options;

namespace CakeExtracter.Common.JobExecutionManagement.JobRequests.Utils
{
    /// <summary>
    /// The utility that helps convert command and request arguments to console arguments and vice versa.
    /// </summary>
    public static class CommandArgumentsConverter
    {
        private const string ArgumentsSeparatorForConsole = " ";
        private const string ArgumentsPrefix = "-";
        private const string ArgumentNameAndValueSeparator = " ";
        private const string ArgumentStringValueSeparator = "\"";

        private static readonly Random Random = new Random();

        private static readonly string[] MissingCommandArguments =
        {
            ConsoleCommand.RequestIdArgumentName,
            ConsoleCommand.NoNeedToCreateRepeatRequestsArgumentName,
        };

        /// <summary>
        /// Converts arguments of a console command object to use to launch it from the console.
        /// </summary>
        /// <param name="command">The command to convert.</param>
        /// <returns>Command arguments to run from console.</returns>
        public static string GetCommandArgumentsAsLine(ConsoleCommand command)
        {
            return GetCommandArgumentsAsLine(command, GetCommandArgumentValues);
        }

        /// <summary>
        /// Converts arguments of a console command object to use it as example of launch the command from the console.
        /// </summary>
        /// <param name="command">The command to convert.</param>
        /// <returns>Example command arguments.</returns>
        public static string GetExampleCommandArgumentsAsLine(ConsoleCommand command)
        {
            return GetCommandArgumentsAsLine(command, GetExampleCommandArgumentValues);
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

        /// <summary>
        /// Returns objects for command arguments.
        /// </summary>
        /// <param name="command">Command.</param>
        /// <returns>Objects for command arguments.</returns>
        public static List<Option> GetCommandArguments(ManyConsole.ConsoleCommand command)
        {
            var argOptions = command.GetActualOptions();
            return argOptions.ToList();
        }

        private static string GetCommandArgumentsAsLine(ConsoleCommand command, Func<ConsoleCommand, object[]> getCommandArgumentValuesFunc)
        {
            var argNames = GetCommandArgumentNames(command);
            var argValues = getCommandArgumentValuesFunc(command);
            Array.Sort(argNames, argValues);
            return argNames.Select((x, i) => GetArgument(x, argValues[i]))
                .Aggregate(string.Empty, (s, s1) => JoinArguments(s, s1));
        }

        private static string[] GetCommandArgumentNames(ConsoleCommand command)
        {
            var argOptions = command.GetActualOptions();
            // You must remove all command arguments that are needed only for job requests and are common to all commands.
            MissingCommandArguments.ForEach(x => argOptions.Remove(x));
            var argNames = argOptions.Select(x => x.GetNames().First());
            return argNames.ToArray();
        }

        private static object[] GetCommandArgumentValues(ConsoleCommand command)
        {
            var argValues = command.GetArgumentProperties().Select(x => x.GetValue(command));
            return argValues.ToArray();
        }

        private static object[] GetExampleCommandArgumentValues(ConsoleCommand command)
        {
            var argValues = command.GetArgumentProperties().Select(x => GetExamplePropertyValue(x, command));
            return argValues.ToArray();
        }

        private static object GetExamplePropertyValue(PropertyInfo property, ConsoleCommand command)
        {
            var propertyNameParts =
                property.PropertyType.FullName.Split(new[] { '[', ']', ',', '`' }, StringSplitOptions.RemoveEmptyEntries);
            var propertyRealName = string.Equals(propertyNameParts[0], typeof(Nullable).FullName)
                ? propertyNameParts[2]
                : propertyNameParts[0];
            if (propertyRealName == typeof(short).FullName || propertyRealName == typeof(int).FullName ||
                propertyRealName == typeof(long).FullName)
            {
                return Random.Next(1, 100);
            }
            if (propertyRealName == typeof(DateTime).FullName)
            {
                return new DateTime(DateTime.Now.Year, Random.Next(1, 12), Random.Next(1, 28));
            }
            return propertyRealName == typeof(string).FullName ? "example" : property.GetValue(command);
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
