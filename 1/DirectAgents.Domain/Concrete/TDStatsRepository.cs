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
        public TDStatsGauge GetStatsGauge(int? acctId = null)
        {
            var gauge = new TDStatsGauge();
            var dSums = DailySummaries(null, null, acctId: acctId);
            if (dSums.Any())
            {
                gauge.Daily.Earliest = dSums.Min(s => s.Date);
                gauge.Daily.Latest = dSums.Max(s => s.Date);
            }
            var sSums = StrategySummaries(null, null, acctId: acctId);
            if (sSums.Any())
            {
                gauge.Strategy.Earliest = sSums.Min(s => s.Date);
                gauge.Strategy.Latest = sSums.Max(s => s.Date);
            }
            var tSums = TDadSummaries(null, null, acctId: acctId);
            if (tSums.Any())
            {
                gauge.Creative.Earliest = tSums.Min(s => s.Date);
                gauge.Creative.Latest = tSums.Max(s => s.Date);
            }
            var siteSums = SiteSummaries(null, null, acctId: acctId);
            if (siteSums.Any())
            {
                gauge.Site.Earliest = siteSums.Min(s => s.Date);
                gauge.Site.Latest = siteSums.Max(s => s.Date);
            }
            var convs = Convs(null, null, acctId: acctId);
            if (convs.Any())
            {
                gauge.Conv.Earliest = convs.Min(c => c.Time);
                gauge.Conv.Latest = convs.Max(c => c.Time);
            }
            return gauge;
        }

        //public DateTime? EarliestDailyStatDate(int? acctId = null)
        //{
        //    var dSums = DailySummaries(null, null, acctId: acctId);
        //    if (!dSums.Any()) return null;
        //    return dSums.Min(ds => ds.Date);
        //}
        //public DateTime? EarliestStrategyStatDate(int? acctId = null)
        //{
        //    var sSums = StrategySummaries(null, null, acctId: acctId);
        //    if (!sSums.Any()) return null;
        //    return sSums.Min(s => s.Date);
        //}
        //public DateTime? EarliestTDadStatDate(int? acctId = null)
        //{
        //    var tSums = TDadSummaries(null, null, acctId: acctId);
        //    if (!tSums.Any()) return null;
        //    return tSums.Min(s => s.Date);
        //}

        //public DateTime? LatestDailyStatDate(int? acctId = null)
        //{
        //    var dSums = DailySummaries(null, null, acctId: acctId);
        //    if (!dSums.Any()) return null;
        //    return dSums.Max(ds => ds.Date);
        //}
        //public DateTime? LatestStrategyStatDate(int? acctId = null)
        //{
        //    var sSums = StrategySummaries(null, null, acctId: acctId);
        //    if (!sSums.Any()) return null;
        //    return sSums.Max(s => s.Date);
        //}
        //public DateTime? LatestTDadStatDate(int? acctId = null)
        //{
        //    var tSums = TDadSummaries(null, null, acctId: acctId);
        //    if (!tSums.Any()) return null;
        //    return tSums.Max(s => s.Date);
        //}

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

        public IQueryable<StrategySummary> StrategySummaries(DateTime? startDate, DateTime? endDate, int? stratId = null, int? acctId = null, int? campId = null)
        {
            var sSums = context.StrategySummaries.AsQueryable();
            if (startDate.HasValue)
                sSums = sSums.Where(s => s.Date >= startDate.Value);
            if (endDate.HasValue)
                sSums = sSums.Where(s => s.Date <= endDate.Value);
            if (stratId.HasValue)
                sSums = sSums.Where(s => s.StrategyId == stratId.Value);
            if (acctId.HasValue)
                sSums = sSums.Where(s => s.Strategy.AccountId == acctId.Value);
            if (campId.HasValue)
                sSums = sSums.Where(s => s.Strategy.ExtAccount.CampaignId == campId);
            return sSums;
        }

        public IQueryable<TDadSummary> TDadSummaries(DateTime? startDate, DateTime? endDate, int? tdadId = null, int? acctId = null, int? campId = null)
        {
            var tSums = context.TDadSummaries.AsQueryable();
            if (startDate.HasValue)
                tSums = tSums.Where(s => s.Date >= startDate.Value);
            if (endDate.HasValue)
                tSums = tSums.Where(s => s.Date <= endDate.Value);
            if (tdadId.HasValue)
                tSums = tSums.Where(s => s.TDadId == tdadId.Value);
            if (acctId.HasValue)
                tSums = tSums.Where(s => s.TDad.AccountId == acctId.Value);
            if (campId.HasValue)
                tSums = tSums.Where(s => s.TDad.ExtAccount.CampaignId == campId);
            return tSums;
        }

        public IQueryable<SiteSummary> SiteSummaries(DateTime? startDate, DateTime? endDate, int? acctId = null, int? campId = null)
        {
            var sSums = context.SiteSummaries.AsQueryable();
            if (startDate.HasValue)
                sSums = sSums.Where(s => s.Date >= startDate.Value);
            if (endDate.HasValue)
                sSums = sSums.Where(s => s.Date <= endDate.Value);
            if (acctId.HasValue)
                sSums = sSums.Where(s => s.AccountId == acctId.Value);
            if (campId.HasValue)
                sSums = sSums.Where(s => s.ExtAccount.CampaignId == campId);
            return sSums;
        }

        public IQueryable<Conv> Convs(DateTime? startDate, DateTime? endDate, int? acctId = null, int? campId = null)
        {
            var convs = context.Convs.AsQueryable();
            if (startDate.HasValue)
                convs = convs.Where(s => s.Time >= startDate.Value);
            if (endDate.HasValue)
                convs = convs.Where(s => s.Time < endDate.Value.AddDays(1));
                // Include up to 11:59:59.999... on the endDate specified
            if (acctId.HasValue)
                convs = convs.Where(s => s.AccountId == acctId.Value);
            if (campId.HasValue)
                convs = convs.Where(s => s.ExtAccount.CampaignId == campId);
            return convs;
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

        //TODO: by campaignId, etc
        public IEnumerable<TDRawStat> GetStrategyStats(DateTime? startDate, DateTime? endDate, int? acctId = null)
        {
            var sSums = StrategySummaries(startDate, endDate, acctId: acctId);
            var sGroups = sSums.GroupBy(s => s.Strategy);
            var stats = new List<TDRawStat>();
            foreach (var sGroup in sGroups)
            {
                var stat = new TDRawStat(sGroup)
                {
                    Strategy = sGroup.Key
                };
                stats.Add(stat);
            }
            return stats;
        }

        //TODO: by campaignId, etc
        public IEnumerable<TDRawStat> GetTDadStats(DateTime? startDate, DateTime? endDate, int? acctId = null)
        {
            var sums = TDadSummaries(startDate, endDate, acctId: acctId);
            var groups = sums.GroupBy(s => s.TDad);
            var stats = new List<TDRawStat>();
            foreach (var group in groups)
            {
                var stat = new TDRawStat(group)
                {
                    TDad = group.Key
                };
                stats.Add(stat);
            }
            return stats;
        }

        //TODO: by campaignId, etc
        public IEnumerable<TDRawStat> GetSiteStats(DateTime? startDate, DateTime? endDate, int? acctId = null, int? minImpressions = null)
        {
            var sums = SiteSummaries(startDate, endDate, acctId: acctId);
            if (minImpressions.HasValue)
                sums = sums.Where(s => s.Impressions >= minImpressions.Value);
            var groups = sums.GroupBy(s => s.Site);
            var stats = new List<TDRawStat>();
            foreach (var group in groups)
            {
                var stat = new TDRawStat(group)
                {
                    Site = group.Key
                };
                stats.Add(stat);
            }
            return stats;
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
