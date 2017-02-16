using System.ComponentModel.Composition;
using CakeExtracter.Common;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Seed;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class TestABCommand : ConsoleCommand
    {
        public override void ResetProperties()
        {
        }

        public TestABCommand()
        {
            IsCommand("testAB");
        }

        public override int Execute(string[] remainingArguments)
        {
            SeedAB();
            return 0;
        }

        public static void SeedAB()
        {
            using (var abContext = new ABContext())
            {
                var seeder = new ABSeeder(abContext);
                seeder.SeedUnitTypes();
            }
        }
    }
}
