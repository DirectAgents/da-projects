﻿using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.DTO;
using DirectAgents.Domain.Entities;

namespace DirectAgents.Domain.Abstract
{
    public interface ICampaignRepository
    {
        void SaveChanges();
        IQueryable<Campaign> Campaigns { get; }
        IQueryable<Country> Countries { get; }
        IQueryable<string> AllCountryCodes { get; }
        IQueryable<Vertical> Verticals { get; }
        IQueryable<TrafficType> TrafficTypes { get; }
        Campaign FindById(int pid);
        void SaveCampaign(Campaign campaign);
        IEnumerable<CampaignSummary> TopCampaignsByRevenue(int num);
    }
}
