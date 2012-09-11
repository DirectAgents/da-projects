using System.Linq;
using DirectAgents.Domain.Entities;

namespace DirectAgents.Domain.Abstract
{
    public interface ICampaignRepository
    {
        IQueryable<Campaign> Campaigns { get; }
    }
}
