using System.ComponentModel.Composition;
using System.Threading;
using CakeExtracter.Common;
using CakeExtracter.Etl.CakeMarketing.Extracters;
using CakeExtracter.Etl.CakeMarketing.Loaders;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class SynchOffersCommand : ConsoleCommand
    {
        public override void ResetProperties()
        {
        }

        public SynchOffersCommand()
        {
            IsCommand("synchOffers", "synch Offers");
        }

        public override int Execute(string[] remainingArguments)
        {
            var extracter = new OffersExtracter();
            var loader = new OffersLoader();
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
            return 0;
        }
    }
}
