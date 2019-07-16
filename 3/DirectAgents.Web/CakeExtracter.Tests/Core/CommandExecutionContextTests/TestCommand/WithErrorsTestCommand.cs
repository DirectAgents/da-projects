using System;
using CakeExtracter.Common;

namespace CakeExtracter.Tests.Core.CommandExecutionContextTests.TestCommand
{
    internal class WithErrorsTestCommand : ConsoleCommand
    {
        public override int Execute(string[] remainingArguments)
        {
            var exception = new Exception("Test error");
            Logger.Error(exception);
            return 0;
        }
    }
}
