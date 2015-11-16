﻿using System;
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
        //public TDStat GetTDStat(DateTime? startDate, DateTime? endDate, Campaign campaign = null, MarginFeeVals marginFees = null)
        //{
        //    var dSums = DailySummaries(startDate, endDate);
        //    if (campaign != null)
        //    {
        //        var accountIds = campaign.ExtAccounts.Select(a => a.Id).ToArray();
        //        dSums = dSums.Where(ds => accountIds.Contains(ds.AccountId));
        //    }
        //    var stat = new TDStat(dSums, marginFees)
        //    {
        //        Campaign = campaign
        //    };
        //    return stat;
        //}

        //NOTE: This will sum stats for ALL accounts if none specified.
        public TDRawStat GetTDStatWithAccount(DateTime? startDate, DateTime? endDate, ExtAccount extAccount = null)
        {
            int? accountId = (extAccount != null) ? extAccount.Id : (int?)null;
            var dSums = DailySummaries(startDate, endDate, acctId: accountId);

            var stat = new TDRawStat(dSums)
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

            var platStats = new List<ITDLineItem>();
            var monthEnd = monthStart.AddMonths(1).AddDays(-1);

            // Get MediaStats
            var daySums = DailySummaries(monthStart, monthEnd, campId: campId);
            var platforms = daySums.Select(ds => ds.ExtAccount.Platform).Distinct().OrderBy(p => p.Name).ToList();
            foreach (var plat in platforms)
            {
                var platDaySums = daySums.Where(ds => ds.ExtAccount.PlatformId == plat.Id);
                var budgetInfoVals = campaign.PlatformBudgetInfoFor(monthStart, plat.Id, useParentValsIfNone: true);
                var platStat = new TDMediaStatWithBudget(platDaySums, budgetInfoVals)
                {
                    Platform = plat
                };
                platStats.Add(platStat);
            }

            // Get ExtraItems
            var extraItems = ExtraItems(monthStart, monthEnd, campId);
            platforms = extraItems.Select(i => i.Platform).Distinct().OrderBy(p => p.Name).ToList();
            foreach (var plat in platforms)
            {
                var platItems = extraItems.Where(i => i.PlatformId == plat.Id);
                var budgetInfoVals = campaign.PlatformBudgetInfoFor(monthStart, plat.Id, useParentValsIfNone: false);
                var lineItem = new TDLineItem(platItems, (budgetInfoVals != null ? budgetInfoVals.MediaSpend : (decimal?)null))
                {
                    Platform = plat,
                    MoneyValsOnly = true // no click stats
                };
                platStats.Add(lineItem);
            }

            //NOTE: if there is both a MediaStat and an ExtraItem for a particular platform, the budget(mediaspend) is listed for both.

            var budgetInfo = campaign.BudgetInfoFor(monthStart, useDefaultIfNone: true);
            var campStats = new TDCampStats(campaign, platStats, monthStart, (budgetInfo != null ? budgetInfo.MediaSpend : (decimal?)null));
            return campStats;
        }

        public IEnumerable<TDLineItem> GetDailyStatsLI(int campId, DateTime? startDate, DateTime? endDate)
        {
            var statList = new List<TDLineItem>();
            var campaign = Campaign(campId);
            if (campaign == null)
                return statList;

            var dsGroups = DailySummaries(startDate, endDate, campId: campId).GroupBy(ds => ds.Date);
            foreach (var dayGroup in dsGroups.OrderBy(g => g.Key)) // group by day
            {
                var li = new TDLineItem
                {
                    Date = dayGroup.Key,
                    Impressions = dayGroup.Sum(d => d.Impressions),
                    Clicks = dayGroup.Sum(d => d.Clicks),
                    PostClickConv = dayGroup.Sum(d => d.PostClickConv),
                    PostViewConv = dayGroup.Sum(d => d.PostViewConv),
                };
                // Compute marked-up ClientCost - could be a different markup for each platform
                foreach (var platGroup in dayGroup.GroupBy(g => g.ExtAccount.PlatformId))
                {
                    MarginFeeVals mfVals = campaign.PlatformBudgetInfoFor(dayGroup.Key, platGroup.Key, useParentValsIfNone: true);
                    li.ClientCost += mfVals.CostToClientCost(platGroup.Sum(g => g.Cost));
                }
                statList.Add(li);
            }
            return statList;
        }

        // --- ExtraItems ---

        public ExtraItem ExtraItem(int id)
        {
            return context.ExtraItems.Find(id);
        }
        public IQueryable<ExtraItem> ExtraItems(DateTime? startDate, DateTime? endDate, int? campId = null)
        {
            var items = context.ExtraItems.AsQueryable();
            if (startDate.HasValue)
                items = items.Where(i => i.Date >= startDate.Value);
            if (endDate.HasValue)
                items = items.Where(i => i.Date <= endDate.Value);
            if (campId.HasValue)
                items = items.Where(i => i.CampaignId == campId.Value);
            return items;
        }

        public bool AddExtraItem(ExtraItem item)
        {
            if (context.ExtraItems.Any(i => i.Id == item.Id))
                return false;
            context.ExtraItems.Add(item);
            context.SaveChanges();
            return true;
        }
        public bool DeleteExtraItem(int id)
        {
            var item = context.ExtraItems.Find(id);
            if (item == null)
                return false;
            context.ExtraItems.Remove(item);
            context.SaveChanges();
            return true;
        }
        public bool SaveExtraItem(ExtraItem item)
        {
            if (context.ExtraItems.Any(i => i.Id == item.Id))
            {
                var entry = context.Entry(item);
                entry.State = EntityState.Modified;
                context.SaveChanges();
                return true;
            }
            return false;
        }
        public void FillExtended(ExtraItem item)
        {
            if (item.Campaign == null)
                item.Campaign = context.Campaigns.Find(item.CampaignId);
        }

    }
}
