using System.Collections.Generic;
using System.Linq;

namespace CakeExtractor.SeleniumApplication.Commands
{
    internal class CommandsProvider
    {
        public static List<BaseAmazonSeleniumCommand> GetExecutionCommands(string comamndsConfig)
        {
            var allCommands = GetAllCommands();
            if (string.IsNullOrEmpty(comamndsConfig))
            {
                return allCommands;
            }
            var commandNamesToExecute = comamndsConfig.Split('|');
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
