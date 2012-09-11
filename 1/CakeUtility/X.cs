using System;
using System.Linq;
using Cake.Data.Wsdl;
using DirectAgents.Common;

namespace CakeUtility
{
    class X
    {
        public static void DoIt()
        {
            var service = Locator.Get<ICakeService>();
            var campaignsCollection = service.ExportCampaigns();
            using (var db = new Cake.Model.CakeEntities())
            {
                int offerID = 0;
                foreach (var campaigns in campaignsCollection)
                {
                    foreach (var campaign in campaigns)
                    {
                        offerID = campaign.offer.offer_id;
                        int campaignID = campaign.campaign_id;
                        var existing = db.CakeCampaigns.Where(c => c.Campaign_Id == campaignID).FirstOrDefault();
                        if (existing == null)
                        {
                            existing = new Cake.Model.CakeCampaign {
                                Campaign_Id = campaignID,
                                CakeOffer_Offer_Id = offerID
                            };
                            db.CakeCampaigns.AddObject(existing);
                            Console.WriteLine("Added campaign Id={0}, Offer Id={1}", campaignID, offerID);
                        }
                    }
                    Console.WriteLine("Saving changes for offer ID {0}", offerID);
                    db.SaveChanges();
                }
            }
        }
    }
}
