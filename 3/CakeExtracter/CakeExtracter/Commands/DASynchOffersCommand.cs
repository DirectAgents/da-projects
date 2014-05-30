using System.ComponentModel.Composition;
using System.Threading;
using CakeExtracter.Common;
using CakeExtracter.Etl.CakeMarketing.Extracters;
using CakeExtracter.Etl.CakeMarketing.DALoaders;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class DASynchOffersCommand : ConsoleCommand
    {
        public static int RunStatic(int advertiserId)
        {
            var cmd = new DASynchOffersCommand
            {
                AdvertiserId = advertiserId
            };
            return cmd.Run();
        }

        public int AdvertiserId { get; set; }

        public override void ResetProperties()
        {
            AdvertiserId = 0;
        }

        public DASynchOffersCommand()
        {
            IsCommand("daSynchOffers", "synch Offers");
            HasOption<int>("a|advertiserId=", "Advertiser Id (0 = all (default))", c => AdvertiserId = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            var extracter = new OffersExtracter(AdvertiserId);
            var loader = new DAOffersLoader();
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
            return 0;
        }
    }
}
