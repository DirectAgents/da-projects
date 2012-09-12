using System.Linq;
using DirectAgents.Domain.Entities;
using System.Collections.Generic;

namespace DirectAgents.Domain.Abstract
{
    public interface ICampaignRepository
    {
        void SaveChanges();
        IQueryable<Campaign> Campaigns { get; }
        List<string> AllCountryCodes { get; }
        Campaign FindById(int pid);
        void SaveCampaign(Campaign campaign);
    }
}
