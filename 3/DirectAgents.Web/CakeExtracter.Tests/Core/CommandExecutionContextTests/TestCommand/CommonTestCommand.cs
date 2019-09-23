using CakeExtracter.Common;

namespace CakeExtracter.Tests.Core.CommandExecutionContextTests.TestCommand
{
    public class CommonTestCommand : ConsoleCommand
    {
        public override int Execute(string[] remainingArguments)
        {
            return 0;
        }
    }
}
