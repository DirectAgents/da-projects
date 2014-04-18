using ClientPortal.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientPortal.Web.Areas.Admin.Models
{
    public class CampaignDropVM
    {
        private CampaignDrop Model { get; set; }

        public CampaignDropVM(CampaignDrop campaignDrop)
        {
            this.Model = campaignDrop;
        }

        public IEnumerable<CreativeStatVM> CreativeStats
        {
            get { return Model.CreativeStats.Select(cs => new CreativeStatVM(cs)); }
        }
        public bool CombineCreatives
        {
            get { return Model.CombineCreatives; }
        }

        public int AffiliateId
        {
            get { return Model.Campaign.AffiliateId; }
        }
        public string FromEmail
        {
            get { return Model.FromEmail; }
        }
        public string Subject
        {
            get { return Model.Subject; }
        }

        public string Date
        {
            get { return Model.Date.ToShortDateString(); }
        }
        public string Volume
        {
            get { return (Model.Volume.HasValue ? Model.Volume.Value.ToString("N0") : ""); }
        }
        public string Opens
        {
            get { return (Model.Opens.HasValue ? Model.Opens.Value.ToString("N0") : ""); }
        }
        public string OpenRate
        {
            get { return (Model.OpenRate.HasValue ? String.Format("{0:0.00%}", Model.OpenRate.Value) : ""); }
        }
        public string TotalClicks
        {
            get { return Model.CreativeStatTotals.Clicks.Value.ToString("N0"); }
        }
        public string ClickThroughRate
        {
            get { return (Model.ClickThroughRate.HasValue ? String.Format("{0:0.00%}", Model.ClickThroughRate.Value) : ""); }
        }
        public string TotalLeads
        {
            get { return Model.CreativeStatTotals.Leads.Value.ToString("N0"); }
        }
        public string ConversionRate
        {
            get { return (Model.ConversionRate.HasValue ? String.Format("{0:0.00%}", Model.ConversionRate.Value) : ""); }
        }
        public string CostPerLead
        {
            get { return (Model.CostPerLead.HasValue ? Model.CostPerLead.Value.ToString("C2") : ""); }
        }
        public string Cost
        {
            get { return (Model.Cost.HasValue ? Model.Cost.Value.ToString("C2") : ""); }
        }

        public string Extra
        {
            get { return Model.Extra; }
        }
    }
}