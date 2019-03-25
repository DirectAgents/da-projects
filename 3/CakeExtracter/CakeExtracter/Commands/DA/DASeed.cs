using System.ComponentModel.Composition;
using CakeExtracter.Common;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Seed;

namespace CakeExtracter.Commands.DA
{
    [Export(typeof(ConsoleCommand))]
    public class DASeed : ConsoleCommand
    {
        public string Type { get; set; }

        public override void ResetProperties()
        {
            Type = null;
        }

        public DASeed()
        {
            IsCommand("daSeed", "Seed the database");
            HasOption("t|type=", "What type of seeding to do", c => Type = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            switch (Type.ToUpper())
            {
                case "DA":
                    DoDASeed();
                    break;
                case "TD":
                    DoTDSeed();
                    break;
            }

            return 0;
        }

        public static void DoDASeed()
        {
            using (var daContext = new DAContext())
            {
                var seeder = new DASeeder(daContext);
                seeder.SeedCurrencies();
            }
        }

        public void DoTDSeed()
        {
            using (var db = new ClientPortalProgContext())
            {
                var tdSeeder = new TDSeeder(db);
                tdSeeder.SeedPlatforms();
                tdSeeder.SeedNetworks();
            }
        }
    }
}
