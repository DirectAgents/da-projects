using System;
using System.Collections.Generic;
using CakeExtractor.SeleniumApplication.Commands;
using ManyConsole;

namespace CakeExtractor.SeleniumApplication
{
    internal class Program
    {
        private static readonly IEnumerable<ConsoleCommand> Commands = new []
        {
            new SyncAmazonPdaCommand()
        };

        static void Main(string[] args)
        {
            ConsoleCommandDispatcher.DispatchCommand(Commands, args, Console.Out);
        }
    }
}
