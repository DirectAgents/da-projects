using System;
using System.Collections.Generic;

namespace DirectAgents.Domain.Entities.TD
{
    public class Advertiser
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Campaign> Campaigns { get; set; }
    }

    public class Campaign
    {
        public int Id { get; set; }
        public int AdvertiserId { get; set; }
        public virtual Advertiser Advertiser { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<BudgetInfo> Budgets { get; set; }
        public BudgetVals DefaultBudget { get; set; }

        // flight dates, goal, AM, salesperson
    }

    public class BudgetInfo : BudgetVals
    {
        public int CampaignId { get; set; }
        public DateTime Date { get; set; }
        public virtual Campaign Campaign { get; set; }
    }

    public class BudgetVals
    {
        public decimal MediaSpend { get; set; }
        public decimal MgmtFeePct { get; set; }
        public decimal MarginPct { get; set; }

        //MakeCopy()
    }
}
