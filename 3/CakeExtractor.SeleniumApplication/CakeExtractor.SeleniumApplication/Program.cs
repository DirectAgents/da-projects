using System;
using System.Collections.Generic;
using System.IO;
using CakeExtracter.Bootstrappers;
using CakeExtracter.Logging.Loggers;
using CakeExtractor.SeleniumApplication.Commands;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportParsing;
using ManyConsole;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace CakeExtractor.SeleniumApplication
{
    internal class Program
    {
        private static readonly IEnumerable<ConsoleCommand> Commands = new []
        {
            //new SyncAmazonPdaCommand()
            new SyncAmazonVcdCommand()
        };
        
        static void Main(string[] args)
        {
            InitializeEnterpriseLibrary();
            InitializeLogging();
            AutoMapperBootstrapper.CheckRunSetup();
            ConsoleCommandDispatcher.DispatchCommand(Commands, args, Console.Out);
        }

        private static void InitializeEnterpriseLibrary()
        {
            var configurationSource = ConfigurationSourceFactory.Create();
            var logWriterFactory = new LogWriterFactory(configurationSource);
            Logger.SetLogWriter(logWriterFactory.Create());
        }

        private static void InitializeLogging()
        {
            CakeExtracter.Logger.Instance = new EnterpriseLibraryLogger();   
        }
    }
}
