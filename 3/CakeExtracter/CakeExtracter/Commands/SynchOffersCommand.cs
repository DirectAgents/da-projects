using System.ComponentModel.Composition;
using CakeExtracter.Common;
using CakeExtracter.Etl.CakeMarketing.Extracters;
using CakeExtracter.Etl.CakeMarketing.Loaders;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class SynchOffersCommand : ConsoleCommand
    {
        public SynchOffersCommand()
        {
            IsCommand("synchOffers", "synch Offers");
        }

        public override int Execute(string[] remainingArguments)
        {
            Logger.Info("Synching Daily Summaries");
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
