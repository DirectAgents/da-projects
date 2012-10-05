using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DirectAgents.Domain.Entities;
using System.Web.Mvc;

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
                return string.Join(" ", countryCodes);
            }
        }

        public string TrafficTypes
        {
            get
            {
                var trafficTypes = campaign.TrafficTypes.Select(t => t.Name).ToArray();
                return string.Join(", ", trafficTypes);
            }
        }

        public string Name { get { return campaign.Name; } }

        public string Description { get { return campaign.Description; } }

        public int Pid { get { return campaign.Pid; } }

        public string ImageUrl
        {
            get
            {
                return string.IsNullOrWhiteSpace(campaign.ImageUrl)
                    ? VirtualPathUtility.ToAbsolute("~/Images/noimage_sm.gif")
                    : campaign.ImageUrl; ;
            }
        }

        public string PayableAction { get { return campaign.PayableAction ?? string.Empty; } }

        public string Link
        {
            get
            {
                string link = campaign.Link;
                if(link.Contains('?'))
                    return link.Substring(0, link.IndexOf('?'));
                else
                    return link;
            }
        }


        public string CostCurrency { get { return campaign.CostCurrency; } }
        public decimal Cost { get { return campaign.Cost; } }

        public string RevenueCurrency { get { return campaign.RevenueCurrency; } }
        public decimal Revenue { get { return campaign.Revenue; } }

        public MvcHtmlString ImportantDetails
        {
            get
            {
                if (string.IsNullOrWhiteSpace(campaign.ImportantDetails))
                    return null;
                else
                    return MvcHtmlString.Create(campaign.ImportantDetails.Replace(System.Environment.NewLine, "<br />"));
            }
        }

        public string ImportantDetailsAsString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(campaign.ImportantDetails))
                    return string.Empty;
                else
                    return campaign.ImportantDetails;
            }
        }

        public string BannedNetworks { get { return campaign.BannedNetworks ?? string.Empty; } }

        public decimal CampaignCap { get { return campaign.CampaignCap; } }

        public string ScrubPolicy { get { return campaign.ScrubPolicy ?? string.Empty; } }

        public string EomNotes { get { return campaign.EomNotes ?? string.Empty; } }

        public string Vertical { get { return campaign.Vertical.Name; } }
    }
}