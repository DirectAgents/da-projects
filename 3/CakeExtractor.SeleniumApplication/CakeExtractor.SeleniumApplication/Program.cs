using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CakeExtracter.Bootstrappers;
using CakeExtracter.Common;
using CakeExtracter.Logging.Loggers;
using CakeExtracter.Mef;
using CakeExtractor.SeleniumApplication.Commands;
using CakeExtractor.SeleniumApplication.Jobs;

namespace CakeExtractor.SeleniumApplication
{
    internal class Program
    {
        private readonly Composer<Program> composer;

        [ImportMany]
        private IEnumerable<IBootstrapper> bootstrappers;

        private Program(string[] args)
        {
            composer = new Composer<Program>(this);
            composer.Compose();
            bootstrappers.ToList().ForEach(c => c.Run());
            CakeExtracter.Logger.Instance = new EnterpriseLibraryLogger(args[0]);
        }

        private async Task Run(string[] args)
        {
            await AmazonSeleniumCommandsJobScheduler.ConfigureJobSchedule(args, new List<ConsoleCommand> {
                new SyncAmazonPdaCommand(),
                new SyncAmazonVcdCommand()
            });
        }

        [MTAThread]
        static void Main(string[] args)
        {
            var program = new Program(args);
            program.Run(args).Wait();
            AlwaysSleep();
        }

        private static void AlwaysSleep()
        {
            while (true)
            {
                Thread.Sleep(int.MaxValue);
            }
        }
    }
}