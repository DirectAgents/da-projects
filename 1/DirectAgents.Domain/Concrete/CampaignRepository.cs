﻿using System.Collections.Generic;
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

        public IQueryable<Country> CountriesWithActiveCampaigns
        {
            get { return context.Countries.Where(c => c.Campaigns.Any(camp => camp.StatusId != Status.Inactive)); }
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
            int minClicks = 50;
            var minDate = DateTime.Now.AddDays(-16);
            var dailySummaries = context.DailySummaries.Where(s => s.Date > minDate);
            var campaigns = this.Campaigns.Where(c => !c.Name.ToLower().Contains("paused"));

            if (trafficType != null)
            {
                campaigns = campaigns.Where(c => c.TrafficTypes.Select(t => t.Name).Contains(trafficType));
            }
            var query = from s in dailySummaries
                        join c in campaigns on s.Pid equals c.Pid
                        group s by new { Pid = c.Pid, CampaignName = c.Name } into g
                        select new CampaignSummary
                        {
                            Pid = g.Key.Pid,
                            CampaignName = g.Key.CampaignName,
                            Revenue = g.Sum(ds => ds.Revenue),
                            Cost = g.Sum(ds => ds.Cost),
                            Clicks = g.Sum(ds => ds.Clicks)
                        };

            switch (by)
            {
                case TopCampaignsBy.Revenue:
                    return query.OrderByDescending(c => c.Revenue).Take(num).ToList();
                case TopCampaignsBy.Cost:
                    return query.OrderByDescending(c => c.Cost).Take(num).ToList();
                case TopCampaignsBy.EPC:
                    return query.Where(ds => ds.Clicks >= minClicks).ToList().OrderByDescending(c => c.EPC).Take(num);
                default:
                    throw new Exception("Invalid TopCampaignsBy: " + by.ToString());
            }
        }
    }
}
