﻿using System;
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
                var result = campaign.ImageUrl;
                if (string.IsNullOrWhiteSpace(result))
                {
                    result = UrlHelper.GenerateContentUrl("~/Images/noimage.gif", this.Context);
                }
                return result;
            }
        }

        public string PayableAction { get { return campaign.PayableAction; } }

        public string OfferLink { get { return campaign.Link; } }

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

        public string BannedNetworks { get { return campaign.BannedNetworks; } }

        public decimal CampaignCap { get { return campaign.CampaignCap; } }

        public string ScrubPolicy { get { return campaign.ScrubPolicy; } }

        public string EomNotes { get { return campaign.EomNotes; } }

        public string Vertical { get { return campaign.Vertical.Name; } }

        public HttpContextBase Context { get; set; }
    }
}