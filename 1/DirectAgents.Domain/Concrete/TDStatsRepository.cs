using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DirectAgents.Domain.DTO;
using DirectAgents.Domain.Entities.TD;

namespace DirectAgents.Domain.Concrete
{
    public partial class TDRepository
    {
        public DateTime? LatestStatDate(int? acctId = null)
        {
            var dSums = DailySummaries(null, null, acctId: acctId);
            if (!dSums.Any())
                return null;
            return dSums.Max(ds => ds.Date);
        }

        public DailySummary DailySummary(DateTime date, int acctId)
        {
            return context.DailySummaries.Find(date, acctId);
        }

        public bool AddDailySummary(DailySummary daySum)
        {
            if (context.DailySummaries.Any(ds => ds.Date == daySum.Date && ds.AccountId == daySum.AccountId))
                return false;
            if (!context.ExtAccounts.Any(ea => ea.Id == daySum.AccountId))
                return false;
            context.DailySummaries.Add(daySum);
            context.SaveChanges();
            return true;
        }
        public bool SaveDailySummary(DailySummary daySum)
        {
            if (context.DailySummaries.Any(ds => ds.Date == daySum.Date && ds.AccountId == daySum.AccountId))
            {
                var entry = context.Entry(daySum);
                entry.State = EntityState.Modified;
                context.SaveChanges();
                return true;
            }
            return false;
        }
        public void FillExtended(DailySummary daySum)
        {
            if (daySum.ExtAccount == null)
                daySum.ExtAccount = ExtAccount(daySum.AccountId);
        }

        public IQueryable<DailySummary> DailySummaries(DateTime? startDate, DateTime? endDate, int? acctId = null, int? campId = null)
        {
            var dSums = context.DailySummaries.AsQueryable();
            if (startDate.HasValue)
                dSums = dSums.Where(ds => ds.Date >= startDate.Value);
            if (endDate.HasValue)
                dSums = dSums.Where(ds => ds.Date <= endDate.Value);
            if (acctId.HasValue)
                dSums = dSums.Where(ds => ds.AccountId == acctId.Value);
            if (campId.HasValue)
                dSums = dSums.Where(ds => ds.ExtAccount.CampaignId == campId);
            return dSums;
        }

        //NOTE: This will sum stats for ALL campaigns if none specified.
        public TDStat GetTDStat(DateTime? startDate, DateTime? endDate, Campaign campaign = null, MarginFeeVals marginFees = null)
        {
            var dSums = DailySummaries(startDate, endDate);
            if (campaign != null)
            {
                var accountIds = campaign.ExtAccounts.Select(a => a.Id).ToArray();
                dSums = dSums.Where(ds => accountIds.Contains(ds.AccountId));
            }
            var stat = new TDStat(dSums, marginFees)
            {
                Campaign = campaign
            };
            return stat;
        }

        //NOTE: This will sum stats for ALL accounts if none specified.
        public TDStat GetTDStatWithAccount(DateTime? startDate, DateTime? endDate, ExtAccount extAccount = null, MarginFeeVals marginFees = null)
        {
            int? accountId = (extAccount != null) ? extAccount.Id : (int?)null;
            var dSums = DailySummaries(startDate, endDate, acctId: accountId);

            var stat = new TDStat(dSums, marginFees)
            {
                ExtAccount = extAccount
            };
            return stat;
        }

        public TDCampStats GetCampStats(DateTime monthStart, int campId)
        {
            var campaign = Campaign(campId);
            if (campaign == null)
                return null; // ?new TDCampStats - blank?
            var budgetInfo = campaign.BudgetInfoFor(monthStart);

            var platStats = new List<TDStatWithBudget>(); // one for each platform
            var monthEnd = monthStart.AddMonths(1).AddDays(-1);
            var daySums = DailySummaries(monthStart, monthEnd, campId: campId);

            //TODO: include platforms that have a PlatformBudgetInfo, even if stats are all zero ?
            var platforms = daySums.Select(ds => ds.ExtAccount.Platform).Distinct().OrderBy(p => p.Name).ToList();
            foreach (var plat in platforms)
            {
                // See if there is a PlatformBudgetInfo for this campaign/platform/month
                BudgetInfoVals pbInfo = PlatformBudgetInfo(campId, plat.Id, monthStart);
                if (pbInfo == null)
                {   // ...if not, use defaults - for the campaign/month or the campaign
                    pbInfo = new BudgetInfoVals
                    {
                        MediaSpend = 0,
                        MgmtFeePct = (budgetInfo != null ? budgetInfo.MgmtFeePct : campaign.DefaultBudgetInfo.MgmtFeePct),
                        MarginPct = (budgetInfo != null ? budgetInfo.MarginPct : campaign.DefaultBudgetInfo.MarginPct)
                    };
                }
                var platDaySums = daySums.Where(ds => ds.ExtAccount.PlatformId == plat.Id);
                var platStat = new TDStatWithBudget(platDaySums, pbInfo)
                {
                    Platform = plat
                };
                platStats.Add(platStat);
            }

            var campStats = new TDCampStats(campaign, platStats, monthStart);
            if (budgetInfo != null)
                campStats.Budget.MediaSpend = budgetInfo.MediaSpend;

            return campStats;
        }

    }
}
