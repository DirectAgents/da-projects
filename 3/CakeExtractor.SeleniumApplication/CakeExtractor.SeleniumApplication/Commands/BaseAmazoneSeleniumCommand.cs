using CakeExtracter.Common;
using ConsoleCommand = ManyConsole.ConsoleCommand;

namespace CakeExtractor.SeleniumApplication.Commands
{
    public abstract class BaseAmazoneSeleniumCommand : ConsoleCommand
    {
        public abstract void PrepareCommandEnvironment();
    }
}
