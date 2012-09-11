using System.Linq;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Entities;

namespace DirectAgents.Domain.Concrete
{
    public class CampaignRepository : ICampaignRepository
    {
        EFDbContext context;

        public CampaignRepository(EFDbContext context)
        {
            this.context = context;
        }

        public IQueryable<Campaign> Campaigns
        {
            get { return context.Campaigns; }
        }
    }
}
