using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DirectAgents.Domain.Entities;

namespace EomToolWeb.Models
{
    public class CampaignsByCountryViewModel
    {
        public Dictionary<string, List<Campaign>> Countries { get; set; }

        public CampaignsByCountryViewModel(IQueryable<Campaign> campaigns, List<string> countryCodes)
        {
            this.Countries = new Dictionary<string, List<Campaign>>();
            foreach (var countryCode in countryCodes)
            {
                var countryCampaigns = campaigns.Where(c => c.Countries.Contains(countryCode));
//                var countryCampaigns = campaigns.Where(c => c.CountryCodes.Contains(countryCode));
                this.Countries[countryCode] = countryCampaigns.ToList();
            }
        }
    }
}