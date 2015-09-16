using System.Collections.Generic;
using DirectAgents.Domain.DTO;

namespace DirectAgents.Web.Areas.TD.Models
{
    public class CampaignPacingVM
    {
        public IEnumerable<TDStatWithBudget> CampaignBudgetStats { get; set; }
        public bool ShowPerfStats { get; set; }
    }

    public class CampaignPacingDTO
    {
        public int NumExtAccts { get; set; }
        public string Advertiser { get; set; }
        public string Campaign { get; set; }
        public decimal Budget { get; set; }
        public decimal Cost { get; set; }
        public decimal MediaSpend { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal Margin { get; set; }
        public decimal MarginPct { get; set; }
        public string PlatformNames { get; set; }
        public decimal PctOfGoal { get; set; }
        public string SalesRep { get; set; }
        public string AM { get; set; }
    }
}