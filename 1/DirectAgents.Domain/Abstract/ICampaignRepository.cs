using System.Linq;
using DirectAgents.Domain.Entities;
using System.Collections.Generic;

namespace DirectAgents.Domain.Abstract
{
    public interface ICampaignRepository
    {
        void SaveChanges();
        IQueryable<Campaign> Campaigns { get; }
        IQueryable<Country> Countries { get; }
        IQueryable<string> AllCountryCodes { get; }
        IQueryable<Vertical> Verticals { get; }
        Campaign FindById(int pid);
        void SaveCampaign(Campaign campaign);
    }
}
