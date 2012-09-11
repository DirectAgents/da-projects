using System.Linq;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Entities;

namespace DirectAgents.Domain.Concrete
{
    public class AdminImpl : IAdmin
    {
        public void CreateDatabaseIfNotExists()
        {
            using (var context = new EFDbContext())
            {
                if (!context.Database.Exists())
                {
                    context.Database.Create();
                }
            }
        }

        public void ReCreateDatabase()
        {
            using (var context = new EFDbContext())
            {
                if (context.Database.Exists())
                {
                    context.Database.Delete();
                }
                context.Database.Create();
            }
        }

        public void LoadCampaigns()
        {
            using (var cake = new Cake.Model.Staging.CakeStagingEntities())
            using (var daDomain = new EFDbContext())
            {
                foreach (var offer in cake.CakeOffers)
                {
                    var campaign = daDomain.Campaigns.FirstOrDefault(c => c.Pid == offer.Offer_Id);
                    if (campaign == null)
                    {
                        campaign = new Campaign
                        {
                            Pid = offer.Offer_Id,
                        };
                        daDomain.Campaigns.Add(campaign);
                    }
                    campaign.Name = offer.OfferName;
                }
                daDomain.SaveChanges();
            }
        }
    }
}
