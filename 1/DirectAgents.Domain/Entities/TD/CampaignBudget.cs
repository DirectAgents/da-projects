using System;
using System.Collections.Generic;
using System.Linq;

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

        public BudgetInfo BudgetFor(DateTime desiredMonth)
        {
            if (Budgets == null)
                return null;
            var firstOfMonth = new DateTime(desiredMonth.Year, desiredMonth.Month, 1);
            var budgets = Budgets.Where(b => b.Date == firstOfMonth);
            if (!budgets.Any())
                return null;
            return budgets.First();
        }
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

        public void SetBudgetValsFrom(BudgetVals source)
        {
            MediaSpend = source.MediaSpend;
            MgmtFeePct = source.MgmtFeePct;
            MarginPct = source.MarginPct;
        }
    }

    public class BudgetWithStats
    {
        public BudgetInfo Budget { get; set; }
        public TDStat Stats { get; set; }

        public BudgetWithStats(BudgetInfo budgetInfo, TDStat tdStat)
        {
            this.Budget = budgetInfo;
            this.Stats = tdStat;
        }

        public decimal FractionReached()
        {
            return Stats.Cost / Budget.Cost();
        }

        //public decimal SpendMultiplier
        //{
        //    get { return 1 / ((1 - Budget.MarginPct / 100) * (1 + Budget.MgmtFeePct / 100)); }
        //}

        public decimal TotalRevenue
        {
            get { return Stats.Cost / (1 - Budget.MarginPct / 100); }
        }

        public decimal MediaSpend
        {
            //get { return SpendMultiplier * Stats.Cost; }
            get { return TotalRevenue / (1 + Budget.MgmtFeePct / 100); }
        }

        //?compute w/o MediaSpend?
        public decimal MgmtFee
        {
            get { return TotalRevenue - MediaSpend; }
        }

    }
}
