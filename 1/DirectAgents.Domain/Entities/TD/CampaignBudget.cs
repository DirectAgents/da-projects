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
        public virtual ICollection<BudgetInfo> BudgetInfos { get; set; }
        public BudgetVals DefaultBudget { get; set; }
        public virtual ICollection<PlatformBudgetInfo> PlatformBudgetInfos { get; set; }

        public IEnumerable<PlatformBudgetInfo> PlatformBudgetInfosFor(DateTime month)
        {
            return PlatformBudgetInfos.Where(pbi => pbi.Date == month);
        }

        public BudgetInfo BudgetInfoFor(DateTime desiredMonth)
        {
            if (BudgetInfos == null)
                return null;
            var firstOfMonth = new DateTime(desiredMonth.Year, desiredMonth.Month, 1);
            var budgets = BudgetInfos.Where(b => b.Date == firstOfMonth);
            if (!budgets.Any())
                return null;
            return budgets.First();
        }

        // returns months in reverse chronological order
        public DateTime[] MonthsWithoutBudgetInfos(int monthsToCheck = 12)
        {
            var months = new List<DateTime>();
            var iMonth = DateTime.Today.AddMonths(1);
            iMonth = new DateTime(iMonth.Year, iMonth.Month, 1); // start with next month (the 1st of)
            for (int i = 0; i < monthsToCheck; i++)
            {
                if (BudgetInfos == null || !BudgetInfos.Any(b => b.Date == iMonth))
                    months.Add(iMonth);
                iMonth = iMonth.AddMonths(-1);
            }
            return months.ToArray();
        }

        // flight dates, goal...
    }

    public class BudgetInfo : BudgetVals
    {
        public int CampaignId { get; set; }
        public DateTime Date { get; set; }
        public virtual Campaign Campaign { get; set; }
    }

    public class PlatformBudgetInfo : BudgetVals
    {
        public int CampaignId { get; set; }
        public int PlatformId { get; set; }
        public DateTime Date { get; set; }

        public virtual Campaign Campaign { get; set; }
        public virtual Platform Platform { get; set; }
    }

    public class BudgetVals : MarginFeeVals
    {
        //The key values that define a budget...
        public decimal MediaSpend { get; set; }

        //Computed budget vals
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
