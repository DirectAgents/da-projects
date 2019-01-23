using System.Collections.Generic;
using System.Linq;

namespace CakeExtractor.SeleniumApplication.Commands
{
    internal class CommandsProvider
    {
        public static List<BaseAmazonSeleniumCommand> GetExecutionCommands()
        {
            var allCommands = GetAllCommands();
            var commandNamesToExecute = Properties.Settings.Default.CommandsToScheduler.Split('|');
            var commandsToExecute = allCommands.Where(command => commandNamesToExecute.Any(name => command.CommandName == name)).ToList();
            return commandsToExecute;
        }

        private static List<BaseAmazonSeleniumCommand> GetAllCommands()
        {
            return new List<BaseAmazonSeleniumCommand>
            {
                new SyncAmazonPdaCommand(),
                new SyncAmazonVcdCommand()
            };
        }
    }
}
