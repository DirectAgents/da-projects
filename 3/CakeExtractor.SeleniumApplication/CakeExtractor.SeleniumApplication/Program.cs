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
        static void Main(string[] args)
        {
            InitializeEnterpriseLibrary();
            InitializeLogging(args[0]);
            AutoMapperBootstrapper.CheckRunSetup();
            var command = GetCommandFromCmdLineParams(args);
            var executionProfileNumber = GetExecutionProfileNumberFromCmdLineParams(args);
            command.PrepareCommandEnvironment(executionProfileNumber);
            ScheduleJobsForCommand(command).Wait();
            AlwaysSleep();
        }

        private static BaseAmazonSeleniumCommand GetCommandFromCmdLineParams(string[] args)
        {
            var commandsConfig = args.Length > 0 ? args[0] : null;
            var command = CommandsProvider.GetExecutionCommand(commandsConfig);
            return command;
        }

        private static int? GetExecutionProfileNumberFromCmdLineParams(string[] args)
        {
            var executionProfileNumberConfigValue = args.Length >= 2 ? args[1] : null;
            if (int.TryParse(executionProfileNumberConfigValue, out var configProfileNumber))
            {
                return configProfileNumber;
            }
            else
            {
                return null;
            }
        }

        private static async Task ScheduleJobsForCommand(BaseAmazonSeleniumCommand command)
        {
            await AmazonSeleniumCommandsJobScheduler.ConfigureJobSchedule(new List<BaseAmazonSeleniumCommand> { command });
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

        private static void InitializeLogging(string jobName)
        {
            CakeExtracter.Logger.Instance = new EnterpriseLibraryLogger(jobName);
        }
    }
}