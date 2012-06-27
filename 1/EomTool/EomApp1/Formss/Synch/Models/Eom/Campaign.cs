using System.Linq;
using DAgents.Common;
using EomApp1.Formss.Synch.Services.Cake;

namespace EomApp1.Formss.Synch.Models.Eom
{
    public partial class Campaign
    {
        public Campaign()
        {
        }

        public Campaign(EomDatabaseEntities eomEntities, int cakeOfferID, CakeWebService cakeService, ILogger logger)
        {
            logger.Log("Creating new Campaign in database from cakeOfferID " + cakeOfferID + "...");

            var offer = cakeService.OfferById(cakeOfferID);

            this.pid = cakeOfferID;

            this.account_manager_id = eomEntities.AccountManagers.GetOrCreate(cakeOfferID, cakeService);

            this.campaign_status_id = eomEntities.CampaignStatus.First(c => c.name == "Active").id;

            this.ad_manager_id = eomEntities.AdManagers.GetOrCreate(cakeOfferID, cakeService);

            this.advertiser_id = eomEntities.Advertisers.GetOrCreate(cakeOfferID, cakeService);

            this.campaign_name = offer.offer_name;

            using (var context = EomDatabaseEntities.Create())
            {
                context.Campaigns.AddObject(this);
                context.SaveChanges();
                context.Detach(this);
            }
            eomEntities.Attach(this);
        }
    }
}
