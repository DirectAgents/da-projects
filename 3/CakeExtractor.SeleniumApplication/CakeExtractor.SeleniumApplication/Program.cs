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
            var comamndsConfig = args.Length > 0 ? args[0] : null;
            var commands = CommandsProvider.GetExecutionCommands(comamndsConfig);
            PrepareCommandsEnvironment(commands);
            ScheduleJobs(commands).Wait();
            AlwaysSleep();
        }

        private static void PrepareCommandsEnvironment(List<BaseAmazonSeleniumCommand> commands)
        {
            commands.ForEach(command =>
            {
                command.PrepareCommandEnvironment();
            });
        }

        private static async Task ScheduleJobs(List<BaseAmazonSeleniumCommand> commands)
        {
            await AmazonSeleniumCommandsJobScheduler.ConfigureJobSchedule(commands);
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