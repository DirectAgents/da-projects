using System.ComponentModel.Composition;
using System.Linq;
using CakeExtracter.Common;
using DirectAgents.Domain.Contexts;

namespace CakeExtracter.Commands.Test
{
    [Export(typeof(ConsoleCommand))]
    public class TestCakeCommand : ConsoleCommand
    {
        public override void ResetProperties()
        {
        }

        public TestCakeCommand()
        {
            IsCommand("testCake");
        }

        public override int Execute(string[] remainingArguments)
        {
            int x;
            using (var db = new DAContext())
            {
                x = db.Advertisers.Count();
            }
            Logger.Info("{0}", x);

            return 0;
        }
    }
}
