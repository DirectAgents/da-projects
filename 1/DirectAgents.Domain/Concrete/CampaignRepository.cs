using System.Linq;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Entities;
using System.Collections.Generic;
using System.Data;

namespace DirectAgents.Domain.Concrete
{
    public class CampaignRepository : ICampaignRepository
    {
        EFDbContext context;

        public CampaignRepository(EFDbContext context)
        {
            this.context = context;
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public IQueryable<Campaign> Campaigns
        {
            get { return context.Campaigns; }
        }

        public IQueryable<Country> Countries
        {
            get { return context.Countries; }
        }

        public IQueryable<string> AllCountryCodes
        {
            get { return context.Countries.Select(c => c.CountryCode); }
        }

        public IQueryable<Vertical> Verticals
        {
            get { return context.Verticals; }
        }

        public IQueryable<TrafficType> TrafficTypes
        {
            get { return context.TrafficTypes; }
        }

        public Campaign FindById(int pid)
        {
            var campaign = context.Campaigns.Where(c => c.Pid == pid).FirstOrDefault();
            return campaign;
        }

        public void SaveCampaign(Campaign campaign)
        {
            var existingCampaign = this.FindById(campaign.Pid);
            if (existingCampaign != null)
            {
                context.Entry(campaign).State = EntityState.Modified;
            }
            else
            {
                context.Campaigns.Add(campaign);
            }
            context.SaveChanges();
        }
    }
}
