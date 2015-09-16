using System;
using System.Collections.Generic;
using System.Linq;

namespace DirectAgents.Domain.Entities.TD
{
    public class Campaign
    {
        public int Id { get; set; }
        public int AdvertiserId { get; set; }
        public virtual Advertiser Advertiser { get; set; }

        public string Name { get; set; }

        public virtual ICollection<ExtAccount> ExtAccounts { get; set; }
        public virtual ICollection<BudgetInfo> Budgets { get; set; }
        public BudgetVals DefaultBudget { get; set; }

        public BudgetInfo BudgetInfoFor(DateTime desiredMonth)
        {
            if (Budgets == null)
                return null;
            var firstOfMonth = new DateTime(desiredMonth.Year, desiredMonth.Month, 1);
            var budgets = Budgets.Where(b => b.Date == firstOfMonth);
            if (!budgets.Any())
                return null;
            return budgets.First();
        }
        // flight dates, goal...
    }

    public class BudgetInfo : BudgetVals
    {
        public int CampaignId { get; set; }
        public DateTime Date { get; set; }
        public virtual Campaign Campaign { get; set; }
    }

    public class BudgetVals : MarginFeeVals
    {
        //The key values that define a budget...
        public decimal MediaSpend { get; set; }

        //Computed vals
        public decimal MgmtFee()
        {
            return MediaSpend * MgmtFeePct / 100;
        }
        public decimal TotalRevenue()
        {
            return MediaSpend * (1 + MgmtFeePct / 100);
        }
        public decimal Cost()
        {
            return TotalRevenue() * (100 - MarginPct) / 100;
        }
        public decimal Margin()
        {
            return TotalRevenue() * MarginPct / 100;
        }

        public void SetBudgetValsFrom(BudgetVals source)
        {
            MediaSpend = source.MediaSpend;
            MgmtFeePct = source.MgmtFeePct;
            MarginPct = source.MarginPct;
        }
    }
    public class MarginFeeVals
    {
        public decimal MgmtFeePct { get; set; }
        public decimal MarginPct { get; set; }
    }

}
