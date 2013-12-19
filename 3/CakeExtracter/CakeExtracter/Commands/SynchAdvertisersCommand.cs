using System.ComponentModel.Composition;
using CakeExtracter.Common;
using CakeExtracter.Etl.CakeMarketing.Extracters;
using CakeExtracter.Etl.CakeMarketing.Loaders;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class SynchAdvertisersCommand : ConsoleCommand
    {
        public int AdvertiserId { get; set; }
        public bool IncludeContacts { get; set; }

        public override void ResetProperties()
        {
            AdvertiserId = 0;
            IncludeContacts = false;
        }

        public SynchAdvertisersCommand()
        {
            IsCommand("synchAdvertisers", "synch Advertisers");
            HasOption<int>("a|advertiserId=", "Advertiser Id (0 = all (default))", c => AdvertiserId = c);
            HasOption("c|contacts=", "synch Contacts also (default is false)", c => IncludeContacts = bool.Parse(c));
        }

        public override int Execute(string[] remainingArguments)
        {
            var extracter = new AdvertisersExtracter(AdvertiserId);
            var loader = new AdvertisersLoader(IncludeContacts);
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
            return 0;
        }
    }
}
