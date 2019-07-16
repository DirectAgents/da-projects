﻿using System;
using System.Threading;
using CakeExtracter.Common;

namespace CakeExtracter.Tests.Core.CommandExecutionContextTests.TestCommand
{
    internal class CommonTestCommand : ConsoleCommand
    {
        public CommonTestCommand()
        {
            IsAutoShutDownMechanismEnabled = false;
        }

        public override int Execute(string[] remainingArguments)
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));
            return 0;
        }
    }
}
