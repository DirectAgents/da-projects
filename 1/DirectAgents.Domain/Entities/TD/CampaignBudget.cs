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

        public virtual ICollection<ExtAccount> ExtAccounts { get; set; }
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
        //The key values that define a budget...
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

    public class BudgetWithStats //TODO: make this a subclass of BudgetInfo? or call it StatsWithBudget?
    {
        public BudgetInfo Budget { get; set; }
        public TDStat Stats { get; set; }

        public IEnumerable<BudgetWithStats> BudgetStatsByExtAccount { get; set; }

        public BudgetWithStats(BudgetInfo budgetInfo, TDStat tdStat, Campaign campaign = null)
        {
            if (budgetInfo == null)
                this.Budget = new BudgetInfo(); //TODO: set Campaign here?
            else
                this.Budget = budgetInfo;
            this.Stats = tdStat;
            this.Campaign = campaign;
        }

        private Campaign _campaign; // only needed if Budget==null
        public Campaign Campaign
        {
            get
            {
                if (_campaign != null)
                    return _campaign;
                return (Budget != null) ? Budget.Campaign : null;
            }
            set { _campaign = value; }
        }

        public decimal FractionReached()
        {
            if (Budget == null)
                return 0;
            var budgetedCost = Budget.Cost();
            if (budgetedCost == 0)
                return 0;
            else
                return Stats.Cost / budgetedCost;
        }

        public decimal RevToCostMultiplier() // generally used in reverse (cost -> rev)
        {
            if (Budget == null)
                return 1;
            else
                return (1 - Budget.MarginPct / 100);
        }
        public decimal MediaSpendToRevMultiplier() // generally used in reverse (rev -> mediaspend)
        {
            if (Budget == null)
                return 1;
            else return (1 + Budget.MgmtFeePct / 100);
        }

        public decimal TotalRevenue()
        {
            return Stats.Cost / RevToCostMultiplier();
        }

        public decimal MediaSpend()
        {
            return TotalRevenue() / MediaSpendToRevMultiplier();
        }

        //?compute w/o MediaSpend?
        public decimal MgmtFee()
        {
            return TotalRevenue() - MediaSpend();
        }

        public decimal Margin()
        {
            //return Stats.Cost * Budget.MarginPct / (100 - Budget.MarginPct);
            return TotalRevenue() - Stats.Cost;
        }

    }
}
