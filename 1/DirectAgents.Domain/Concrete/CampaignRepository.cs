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
            var query = from c in context.Conversions
                        group c by new { Pid = c.offer.offer_id, Name = c.offer.offer_name, RevenueCurrencyId = c.received.currency_id, CostCurrencyId = c.paid.currency_id } into g
                        select new CampaignSummary
                        {
                            Pid = g.Key.Pid,
                            CampaignName = g.Key.Name,
                            RevenueCurrencyId = g.Key.RevenueCurrencyId,
                            Revenue = g.Sum(r => r.received.amount),
                            CostCurrencyId = g.Key.CostCurrencyId,
                            Cost = g.Sum(x => x.paid.amount)
                        };

            if (trafficType != null)
            {
                query = from c in query
                        join cp in context.Campaigns on c.Pid equals cp.Pid
                        where cp.TrafficTypes.Select(t => t.Name).Contains(trafficType)
                        select c;
            }

            switch (by)
            {
                case TopCampaignsBy.Revenue:
                    return query.OrderByDescending(c => c.Revenue).Take(num).ToList();
                case TopCampaignsBy.Cost:
                    return query.OrderByDescending(c => c.Cost).Take(num).ToList();
                default:
                    throw new Exception("Invalid TopCampaignsBy: " + by.ToString());
            }
        }
    }
}
