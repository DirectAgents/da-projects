using System;
using CakeExtracter.Common;

namespace CakeExtracter.Tests.Core.CommandExecutionContextTests.TestCommand
{
    class FailingTestCommand : ConsoleCommand
    {
        public override int Execute(string[] remainingArguments)
        {
            throw new NotImplementedException();
        }
    }
}
