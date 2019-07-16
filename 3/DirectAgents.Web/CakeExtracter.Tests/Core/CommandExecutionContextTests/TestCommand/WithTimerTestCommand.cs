using System;
using System.Threading;
using CakeExtracter.Common;

namespace CakeExtracter.Tests.Core.CommandExecutionContextTests.TestCommand
{
    internal class WithTimerTestCommand : ConsoleCommand
    {
        public WithTimerTestCommand(bool timerEnabled)
        {
            IsCommand("WithTimerTestCommand", "WithTimerTestCommand");
            IsAutoShutDownMechanismEnabled = timerEnabled;
        }

        public override int Execute(string[] remainingArguments)
        {
            Thread.Sleep(TimeSpan.FromMinutes(10));
            return 0;
        }
    }
}
