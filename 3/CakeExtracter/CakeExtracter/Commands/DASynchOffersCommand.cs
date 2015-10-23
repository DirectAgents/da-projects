using System.ComponentModel.Composition;
using CakeExtracter.Bootstrappers;
using CakeExtracter.Common;
using CakeExtracter.Etl.CakeMarketing.DALoaders;
using CakeExtracter.Etl.CakeMarketing.Extracters;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class DASynchOffersCommand : ConsoleCommand
    {
        public static int RunStatic(int advertiserId, bool loadInactive)
        {
            AutoMapperBootstrapper.CheckRunSetup();
            var cmd = new DASynchOffersCommand
            {
                AdvertiserId = advertiserId,
                LoadInactive = loadInactive
            };
            return cmd.Run();
        }

        public int AdvertiserId { get; set; }
        public bool LoadInactive { get; set; }

        public override void ResetProperties()
        {
            AdvertiserId = 0;
            LoadInactive = false;
        }

        public DASynchOffersCommand()
        {
            IsCommand("daSynchOffers", "synch Offers");
            HasOption<int>("a|advertiserId=", "Advertiser Id (0 = all (default))", c => AdvertiserId = c);
            HasOption("l|loadInactive=", "load inactive offers (default is false)", c => LoadInactive = bool.Parse(c));
        }

        public override int Execute(string[] remainingArguments)
        {
            var extracter = new OffersExtracter(AdvertiserId);
            var loader = new DAOffersLoader(LoadInactive);
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
            return 0;
        }
    }
}
