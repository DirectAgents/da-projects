using System.ComponentModel.Composition;
using CakeExtracter.Common;
using CakeExtracter.Etl.CakeMarketing.Extracters;
using CakeExtracter.Etl.CakeMarketing.Loaders;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class SynchAdvertisersCommand : ConsoleCommand
    {
        public SynchAdvertisersCommand()
        {
            IsCommand("synchAdvertisers", "synch Advertisers");
        }

        public override int Execute(string[] remainingArguments)
        {
            var extracter = new AdvertisersExtracter();
            var loader = new AdvertisersLoader();
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
            return 0;
        }
    }
}
