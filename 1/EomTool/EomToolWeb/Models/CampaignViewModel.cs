using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DirectAgents.Domain.Entities;

namespace EomToolWeb.Models
{
    public class CampaignViewModel
    {
        private Campaign campaign;

        public CampaignViewModel(Campaign campaign)
        {
            this.campaign = campaign;
        }

        public string AdManagers
        {
            get
            {
                string result = string.Join(", ", campaign.AdManagers.Select(am => am.Name));
                return result;
            }
        }

        public string AccountManagers
        {
            get
            {
                string result = string.Join(", ", campaign.AccountManagers.Select(am => am.Name));
                return result;
            }
        }

        public string CountryCodes
        {
            get
            {
                var countryCodes = campaign.Countries.Select(c => c.CountryCode).ToArray();
                return string.Join(",", countryCodes);
            }
        }

        public string Name { get { return campaign.Name; } }

        public string Description { get { return campaign.Description; } }

        public int Pid { get { return campaign.Pid; } }

        public string ImageUrl { get { return campaign.ImageUrl; } }

        public string PayableAction { get { return campaign.PayableAction; } }

        public string TrafficTypes { get { return campaign.TrafficType; } }

        public string OfferLink { get { return campaign.Link; } }
    }
}