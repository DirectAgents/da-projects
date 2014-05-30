﻿using System.ComponentModel.Composition;
using System.Threading;
using CakeExtracter.Common;
using CakeExtracter.Etl.CakeMarketing.Extracters;
using CakeExtracter.Etl.CakeMarketing.DALoaders;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class DASynchAdvertisersCommand : ConsoleCommand
    {
        public static int RunStatic(int advertiserId, bool synchOffersAlso = false)
        {
            var cmd = new DASynchAdvertisersCommand
            {
                AdvertiserId = advertiserId,
                SynchOffersAlso = synchOffersAlso
            };
            return cmd.Run();
        }

        public int AdvertiserId { get; set; }
//        public bool IncludeContacts { get; set; }
        public bool SynchOffersAlso { get; set; }

        public override void ResetProperties()
        {
            AdvertiserId = 0;
//            IncludeContacts = false;
            SynchOffersAlso = false;
        }

        public DASynchAdvertisersCommand()
        {
            IsCommand("daSynchAdvertisers", "synch Advertisers");
            HasOption<int>("a|advertiserId=", "Advertiser Id (0 = all (default))", c => AdvertiserId = c);
//            HasOption("c|contacts=", "synch Contacts also (default is false)", c => IncludeContacts = bool.Parse(c));
            HasOption("o|offers=", "synch Offers also (default is false)", c => SynchOffersAlso = bool.Parse(c));
        }

        public override int Execute(string[] remainingArguments)
        {
            var extracter = new AdvertisersExtracter(AdvertiserId);
            var loader = new DAAdvertisersLoader();
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();

            if (SynchOffersAlso)
                DASynchOffersCommand.RunStatic(AdvertiserId);

            return 0;
        }
    }
}
