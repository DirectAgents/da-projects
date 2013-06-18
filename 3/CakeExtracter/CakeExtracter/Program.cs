using System;
using System.Data.Entity;

namespace CakeExtracter
{
    class Program
    {
        public static void Main(string[] args)
        {
            Database.SetInitializer<UsersContext>(null);

            var programArgs = new ProgramArgs(args);

            if (programArgs.IsScheduler)
            {
                var scheduler = new Scheduler();
                scheduler.Run();
            }
            else if (programArgs.ValidForSyncher)
            {
                var syncher = new Syncher();
                syncher.Run(args);
            }
            else
            {
                Console.WriteLine(Globals.Usage);
            }
        }
    }
}
