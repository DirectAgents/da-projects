using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CakeExtracter.Bootstrappers;
using CakeExtracter.Logging.Loggers;
using CakeExtractor.SeleniumApplication.Commands;
using CakeExtractor.SeleniumApplication.Jobs;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace CakeExtractor.SeleniumApplication
{
    internal class Program
    {
        private static readonly List<BaseAmazonSeleniumCommand> Commands = GetCommandList();

        static void Main(string[] args)
        {
            InitializeEnterpriseLibrary();
            InitializeLogging();
            AutoMapperBootstrapper.CheckRunSetup();
            PrepareCommandsEnvironment();
            ScheduleJobs(args).Wait();
            AlwaysSleep();
        }

        private static List<BaseAmazonSeleniumCommand> GetCommandList()
        {
            var commandList = new List<BaseAmazonSeleniumCommand>();

            var arrayOfCommands = Properties.Settings.Default.CommandsToScheduler.Split('|');
            foreach (var command in arrayOfCommands)
            {
                switch (command)
                {
                    case "SyncAmazonPdaCommand": commandList.Add(new SyncAmazonPdaCommand());
                        break;
                    case "SyncAmazonVcdCommand": commandList.Add(new SyncAmazonVcdCommand());
                        break;
                    default: throw new NotImplementedException();
                }                    
            }
            return commandList;
        }

        private static void PrepareCommandsEnvironment()
        {
            Commands.ForEach(command =>
            {
                command.PrepareCommandEnvironment();
            });
        }

        private static async Task ScheduleJobs(string[] args)
        {
            await AmazonSeleniumCommandsJobScheduler.ConfigureJobSchedule(Commands);
        }

        private static void AlwaysSleep()
        {
            while (true)
            {
                Thread.Sleep(int.MaxValue);
            }
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