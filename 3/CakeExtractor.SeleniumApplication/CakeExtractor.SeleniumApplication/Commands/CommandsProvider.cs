using System;
using System.Collections.Generic;
using System.Linq;

namespace CakeExtractor.SeleniumApplication.Commands
{
    /// <summary>
    /// provides command to execute based on cmd line config value.
    /// </summary>
    internal class CommandsProvider
    {
        /// <summary>
        /// Gets the execution command based on config value.
        /// </summary>
        /// <param name="commandsConfig">The commands configuration.</param>
        /// <returns>Commands objects to execute.</returns>
        /// <exception cref="Exception">At least one command should be defined</exception>
        public static BaseSeleniumCommand GetExecutionCommand(string commandName)
        {
            if (!string.IsNullOrEmpty(commandName))
            {
                var commandToExecute = GetAllCommands().FirstOrDefault(command =>  command.CommandName == commandName);
                if (commandToExecute != null)
                {
                    return commandToExecute;
                }
            }
            throw new Exception("No command was found with name defined in cmd args.");
        }

        private static List<BaseSeleniumCommand> GetAllCommands()
        {
            return new List<BaseSeleniumCommand>
            {
                new SyncAmazonPdaCommand(),
                new SyncAmazonVcdCommand(),
                new SyncKochavaDataCommand()
            };
        }
    }
}
