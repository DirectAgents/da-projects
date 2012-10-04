using System.Collections.Generic;
using System.Data;
using System.Linq;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.DTO;
using DirectAgents.Domain.Entities;
using System;

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

        public IEnumerable<CampaignSummary> TopCampaigns(int num, TopCampaignsBy by, string trafficType)
        {
            var campaigns = this.Campaigns.Where(c => !c.Name.ToLower().Contains("paused"));
            var monthlySummaries = context.MonthlySummaries.Where(s => s.clicks >= 50);
            if (trafficType != null)
            {
                campaigns = campaigns.Where(c => c.TrafficTypes.Select(t => t.Name).Contains(trafficType));
            }
            var query = from s in monthlySummaries
                        join c in campaigns on s.pid equals c.Pid
                        select new CampaignSummary
                        {
                            Pid = c.Pid,
                            CampaignName = c.Name,
                            Revenue = s.revenue,
                            Cost = s.cost,
                            EPC = (s.clicks == 0) ? 0 : (s.cost / s.clicks),
                            Clicks = s.clicks
                        };

            switch (by)
            {
                case TopCampaignsBy.Revenue:
                    return query.OrderByDescending(c => c.Revenue).Take(num).ToList();
                case TopCampaignsBy.Cost:
                    return query.OrderByDescending(c => c.Cost).Take(num).ToList();
                case TopCampaignsBy.EPC:
                    return query.OrderByDescending(c => c.EPC).Take(num).ToList();
                default:
                    throw new Exception("Invalid TopCampaignsBy: " + by.ToString());
            }
        }
    }
}
