using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace CakeExtracter
{
    using Common;
    using Bootstrappers;
    using Mef;

    class Program
    {
        private readonly Composer<Program> composer;

        [ImportMany]
        private IEnumerable<IBootstrapper> bootstrappers;

        [ImportMany]
        private IEnumerable<ConsoleCommand> commands; 

        private Program()
        {
            composer = new Composer<Program>(this);
            composer.Compose();

            bootstrappers.ToList().ForEach(c => c.Run()); 
        }

        public static int Main(string[] args)
        {
            var program = new Program();
            var result = program.Run(args);
            return result;
        }

        private int Run(string[] args)
        {
            return ManyConsole.ConsoleCommandDispatcher.DispatchCommand(commands, args, Console.Out);
        }
    }
}
