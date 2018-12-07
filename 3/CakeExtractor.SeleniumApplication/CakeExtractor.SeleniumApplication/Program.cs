using System;
using System.Collections.Generic;
using CakeExtractor.SeleniumApplication.Commands;
using ManyConsole;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;

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
            var configurationSource = ConfigurationSourceFactory.Create();
            var logWriterFactory = new LogWriterFactory(configurationSource);
            Logger.SetLogWriter(logWriterFactory.Create());

            ConsoleCommandDispatcher.DispatchCommand(Commands, args, Console.Out);
        }
    }
}
