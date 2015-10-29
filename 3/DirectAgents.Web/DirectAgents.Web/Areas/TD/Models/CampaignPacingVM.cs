using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.DTO;

namespace DirectAgents.Web.Areas.TD.Models
{
    public class CampaignPacingVM
    {
        public IEnumerable<TDCampStats> CampStats { get; set; }
        public bool ShowPerfStats { get; set; }
    }

    public class CampaignPacingDTO
    {
        public int NumPlatforms { get; set; }
        public string Platform { get; set; } // one or multiple
        public string Advertiser { get; set; }
        public int CampaignId { get; set; }
        public string Campaign { get; set; }
        public decimal Budget { get; set; }
        public decimal DACost { get; set; }
        public decimal ClientCost { get; set; }
        public decimal TotalRev { get; set; }
        public decimal Margin { get; set; }
        public decimal MarginPct { get; set; }
        public decimal PctOfGoal { get; set; }
        public string SalesRep { get; set; }
        public string AM { get; set; }

        public CampaignPacingDTO() { }
        public CampaignPacingDTO(TDMediaStatWithBudget bs)
        {
            if (bs.Campaign != null)
            {
                //NumPlatforms = (what only platforms that have stats?)
                Advertiser = bs.Campaign.Advertiser.Name;
                CampaignId = bs.Campaign.Id;
                Campaign = bs.Campaign.Name;
                Platform = string.Join(",", bs.Campaign.ExtAccounts.Select(a => a.Platform).Distinct().Select(p => p.Name));
                SalesRep = bs.Campaign.Advertiser.SalesRepName();
                AM = bs.Campaign.Advertiser.AMName();
            }
            if (bs.Platform != null)
            {
                Platform = bs.Platform.Name;
            }
            Budget = bs.Budget.MediaSpend;
            DACost = bs.DACost();
            ClientCost = bs.MediaSpend();
            TotalRev = bs.TotalRevenue();
            Margin = bs.Margin();
            MarginPct = bs.MarginPct / 100;
            PctOfGoal = bs.FractionReached();
        }
        public CampaignPacingDTO(TDCampStats cstat)
        {
            NumPlatforms = cstat.PlatformStats.Count();
            Advertiser = cstat.Campaign.Advertiser.Name;
            CampaignId = cstat.Campaign.Id;
            Campaign = cstat.Campaign.Name;
            Budget = cstat.Budget.MediaSpend;
            DACost = cstat.DACost;
            ClientCost = cstat.ClientCost;
            TotalRev = cstat.TotalRevenue;
            Margin = cstat.Margin;
            MarginPct = cstat.MarginPct / 100;
            //Platform =
            PctOfGoal = cstat.FractionReached();
            SalesRep = cstat.Campaign.Advertiser.SalesRepName();
            AM = cstat.Campaign.Advertiser.AMName();
        }
    }

    public class PerformanceDTO
    {
        public int CampaignId { get; set; }
        public string Campaign { get; set; }
        public decimal Budget { get; set; }
        public decimal DACost { get; set; }
        public decimal ClientCost { get; set; }
        public decimal TotalRev { get; set; }
        public decimal Margin { get; set; }
        public decimal MarginPct { get; set; }
        public string Platform { get; set; }
        public decimal PctOfGoal { get; set; }
        public int Impressions { get; set; }
        public int Clicks { get; set; }
        public int TotalConv { get; set; }
        public int PostClickConv { get; set; }
        public int PostViewConv { get; set; }
        public double CTR { get; set; }
        public decimal CPA { get; set; }

        public PerformanceDTO() { }
        public PerformanceDTO(TDMediaStatWithBudget bs)
        {
            if (bs.Campaign != null)
            {
                CampaignId = bs.Campaign.Id;
                Campaign = bs.Campaign.Name;
                Platform = string.Join(",", bs.Campaign.ExtAccounts.Select(a => a.Platform).Distinct().Select(p => p.Name));
            }
            Budget = bs.Budget.MediaSpend;
            DACost = bs.DACost();
            ClientCost = bs.MediaSpend();
            TotalRev = bs.TotalRevenue();
            Margin = bs.Margin();
            MarginPct = bs.MarginPct / 100;
            PctOfGoal = bs.FractionReached();
            Impressions = bs.Impressions;
            Clicks = bs.Clicks;
            TotalConv = bs.TotalConv;
            PostClickConv = bs.PostClickConv;
            PostViewConv = bs.PostViewConv;
            CTR = bs.CTR;
            CPA = bs.CPA;
        }
        public PerformanceDTO(TDCampStats cstat)
        {
            CampaignId = cstat.Campaign.Id;
            Campaign = cstat.Campaign.Name;
            Budget = cstat.Budget.MediaSpend;
            DACost = cstat.DACost;
            ClientCost = cstat.ClientCost;
            TotalRev = cstat.TotalRevenue;
            Margin = cstat.Margin;
            MarginPct = cstat.MarginPct / 100;
            PctOfGoal = cstat.FractionReached();
            Impressions = cstat.Impressions;
            Clicks = cstat.Clicks;
            TotalConv = cstat.TotalConv;
            PostClickConv = cstat.PostClickConv;
            PostViewConv = cstat.PostViewConv;
            CTR = cstat.CTR;
            CPA = cstat.CPA;
        }
    }
}