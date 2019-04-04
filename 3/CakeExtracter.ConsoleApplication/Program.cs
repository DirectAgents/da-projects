using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using CakeExtracter.Bootstrappers;
using CakeExtracter.Common;
using CakeExtracter.Mef;

namespace CakeExtracter
{
    using CakeExtracter.Common.JobExecutionManagement;
    using CakeExtracter.Logging.Loggers;

    class Program
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
            Logger.Instance = new EnterpriseLibraryLogger(args[0]);
        }

        private int Run(string[] args)
        {
            CommandExecutionContext.InitContext();
            return ManyConsole.ConsoleCommandDispatcher.DispatchCommand(Commands, args, Console.Out);
        }

        // --- static methods ---
        [STAThread]
        public static int Main(string[] args)
        {
            var program = new Program(args);
            var result = program.Run(args);
            return result;
        }
    }
}
