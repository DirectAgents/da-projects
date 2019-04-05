using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CakeExtracter.Bootstrappers;
using CakeExtracter.Common;
using CakeExtracter.Logging.Loggers;
using CakeExtracter.Mef;
using CakeExtractor.SeleniumApplication.Jobs;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace CakeExtractor.SeleniumApplication
{
    internal class Program
    {
        private readonly Composer<Program> composer;

        [ImportMany]
        private IEnumerable<IBootstrapper> bootstrappers;

        [ImportMany]
        private IEnumerable<ConsoleCommand> Commands;

        private Program(string[] args)
        {
            composer = new Composer<Program>(this);
            composer.Compose();
            bootstrappers.ToList().ForEach(c => c.Run());
            InitializeLogging(args[0]);
        }

        static void Main(string[] args)
        {
            AutoMapperBootstrapper.CheckRunSetup();
            ScheduleJobsForCommandFromArgs(args).Wait();
            AlwaysSleep();
        }

        private static async Task ScheduleJobsForCommandFromArgs(string[] args)
        {
            await AmazonSeleniumCommandsJobScheduler.ConfigureJobSchedule(args, null);
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
            InitializeEnterpriseLibrary();
            CakeExtracter.Logger.Instance = new EnterpriseLibraryLogger(jobName);
        }
    }
}