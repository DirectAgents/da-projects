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

        public List<string> AllCountryCodes
        {
            get
            {
                //var countryCodes = this.Campaigns.SelectMany(c => c.Countries.Split(new char[] { ',' })).Distinct().OrderBy(c => c);
                var countryCodeCommaStrings = this.Campaigns.Select(c => c.Countries).ToList();
                var countryCodes = countryCodeCommaStrings.Select(c => c.Split(new char[] { ',' })).SelectMany(c => c).Distinct().Where(c => !string.IsNullOrWhiteSpace(c)).OrderBy(c => c);
                return countryCodes.ToList();
            }
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
