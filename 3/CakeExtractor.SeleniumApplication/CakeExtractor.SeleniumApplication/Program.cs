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

        private static BaseSeleniumCommand GetCommandFromCmdLineParams(string[] args)
        {
            var comamndsConfig = args.Length > 0 ? args[0] : null;
            var command = CommandsProvider.GetExecutionCommand(comamndsConfig);
            return command;
        }

        private static int GetExecutionProfileNumberFromCmdLineParams(string[] args)
        {
            const int defaultExecutionProfileNumber = 1;
            var executionProfileNumberConfigValue = args.Length >= 2 ? args[1] : null;
            try
            {
                var configProfileNumber = int.Parse(executionProfileNumberConfigValue);
                return configProfileNumber;
            }
            catch
            {
                CakeExtracter.Logger.Warn($"Execution profile number not specified of specified incorrectly. {defaultExecutionProfileNumber} will be used as profile number");
                return defaultExecutionProfileNumber;
            }
        }

        private static async Task ScheduleJobsForCommand(BaseSeleniumCommand command)
        {
            await AmazonSeleniumCommandsJobScheduler.ConfigureJobSchedule(new List<BaseSeleniumCommand> { command });
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