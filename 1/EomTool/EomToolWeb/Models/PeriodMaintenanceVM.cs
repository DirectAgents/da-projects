using System;
using System.Collections.Generic;
using EomTool.Domain.Entities;

namespace EomToolWeb.Models
{
    public class PeriodMaintenanceVM
    {
        public String CurrentEomDateString { get; set; }

        public IEnumerable<Advertiser> NewAdvertisers { get; set; }
        public IEnumerable<Advertiser> ExpiredAdvertisers { get; set; }
        public IEnumerable<Advertiser> ChangedAdvertisers { get; set; }

        public IEnumerable<Campaign> NewCampaigns { get; set; }
        public IEnumerable<Campaign> ExpiredCampaigns { get; set; }
        public IEnumerable<Campaign> ChangedCampaigns { get; set; }

        public IEnumerable<Affiliate> NewAffiliates { get; set; }
        public IEnumerable<Affiliate> ExpiredAffiliates { get; set; }
        public IEnumerable<Affiliate> ChangedAffiliates { get; set; }
    }
}