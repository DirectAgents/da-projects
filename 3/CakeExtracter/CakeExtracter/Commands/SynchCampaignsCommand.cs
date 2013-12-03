using CakeExtracter.Common;
using CakeExtracter.Etl.CakeMarketing.Extracters;
using CakeExtracter.Etl.CakeMarketing.Loaders;
using ClientPortal.Data.Contexts;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class SynchCampaignsCommand : ConsoleCommand
    {
        public int AdvertiserId { get; set; }

        public override void ResetProperties()
        {
            AdvertiserId = 0;
        }

        public SynchCampaignsCommand()
        {
            IsCommand("synchCampaigns", "synch Campaigns");
            HasOption<int>("a|advertiserId=", "Advertiser Id (0 = all (default))", c => AdvertiserId = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            var offers = GetOffers();
            foreach (var offer in offers)
            {
                var extracter = new CampaignsExtracter(offer.OfferId);
                var loader = new CampaignsLoader();
                var extracterThread = extracter.Start();
                var loaderThread = loader.Start(extracter);
                extracterThread.Join();
                loaderThread.Join();
            }
            return 0;
        }

        private IEnumerable<Offer> GetOffers()
        {
            using (var db = new ClientPortalContext())
            {
                var offers = db.Offers.AsQueryable();
                if (this.AdvertiserId != 0)
                    offers = offers.Where(o => o.AdvertiserId == AdvertiserId);

                return offers.ToList();
            }

        }
    }
}
