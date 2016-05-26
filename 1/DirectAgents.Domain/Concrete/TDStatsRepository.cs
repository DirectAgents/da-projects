using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Data.SqlClient;
using System.Linq;
using DirectAgents.Domain.DTO;
using DirectAgents.Domain.Entities.TD;

namespace DirectAgents.Domain.Concrete
{
    public partial class TDRepository
    {
        public IEnumerable<BasicStat> DayOfWeekBasicStats(int advId, DateTime? startDate = null, DateTime? endDate = null, bool mondayFirst = false)
        {
            var sql = @"select DatePart(weekday, Date) as Day, sum(Impressions) as Impressions, sum(Clicks) as Clicks, sum(Conversions) as Conversions, sum(MediaSpend) as MediaSpend, sum(MgmtFee) as MgmtFee
from td.fDailySummaryBasicStats(@p1, @p2, @p3)
group by DatePart(weekday, Date) order by Day";
            var stats = DailySummaryBasicStatsWithCompute(advId, startDate, endDate, sql: sql);
            if (mondayFirst)
            {
                return stats.OrderBy(s => s.Day < 2).ThenBy(s => s.Day);
            }
            else
                return stats;
        }

        public IEnumerable<BasicStat> WeeklyBasicStats(int advId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var dailyStats = DailySummaryBasicStatsWithCompute(advId, startDate, endDate, computeWeekStartDate: true);
            var weekGroups = dailyStats.GroupBy(s => s.StartDate).OrderBy(g => g.Key);
            foreach (var group in weekGroups)
            {
                var weekStat = new BasicStat
                {
                    Date = group.Key,
                    Impressions = group.Sum(s => s.Impressions),
                    Clicks = group.Sum(s => s.Clicks),
                    Conversions = group.Sum(s => s.Conversions),
                    MediaSpend = group.Sum(s => s.MediaSpend),
                    MgmtFee = group.Sum(s => s.MgmtFee)
                };
                weekStat.ComputeCalculatedStats();
                yield return weekStat;
            }
        }

        public IEnumerable<BasicStat> DailySummaryBasicStats(int advId, DateTime? startDate = null, DateTime? endDate = null)
        {
            return DailySummaryBasicStatsWithCompute(advId, startDate, endDate, null);
        }

        private IEnumerable<BasicStat> DailySummaryBasicStatsWithCompute(int advId, DateTime? startDate, DateTime? endDate, string sql = null, bool computeWeekStartDate = false)
        {
            DateTime yesterday = DateTime.Today.AddDays(-1);
            if (!startDate.HasValue)
                startDate = EarliestStatDate(advId) ?? yesterday; // if no stats, just set to yesterday
            if (!endDate.HasValue)
                endDate = yesterday;

            if (sql == null) // Default query - one row per day
                sql = @"select Date, sum(Impressions) as Impressions, sum(Clicks) as Clicks, sum(Conversions) as Conversions, sum(MediaSpend) as MediaSpend, sum(MgmtFee) as MgmtFee
from td.fDailySummaryBasicStats(@p1, @p2, @p3)
group by Date order by Date";
            var stats = DailySummaryBasicStatsRaw(advId, startDate.Value, endDate.Value, sql);
            foreach (var stat in stats)
            {
                stat.ComputeCalculatedStats();
                if (computeWeekStartDate)
                    stat.ComputeWeekStartDate();
                yield return stat;
            }
        }

        public IEnumerable<BasicStat> MTDStrategyBasicStats(int advId, DateTime endDate)
        {
            var startDate = new DateTime(endDate.Year, endDate.Month, 1);
            return StrategyBasicStats(advId, startDate, endDate);
        }
        public IEnumerable<BasicStat> StrategyBasicStats(int advId, DateTime startDate, DateTime endDate)
        {
            string sql = @"select PlatformAlias,StrategyName,StrategyId,ShowClickAndViewConv, sum(Impressions) as Impressions, sum(Clicks) as Clicks, sum(PostClickConv) as PostClickConv,sum(PostViewConv) as PostViewConv, sum(Conversions) as Conversions, sum(MediaSpend) as MediaSpend, sum(MgmtFee) as MgmtFee
from td.fStrategySummaryBasicStats(@p1, @p2, @p3)
group by PlatformAlias,StrategyName,StrategyId,ShowClickAndViewConv order by PlatformAlias,MediaSpend desc,StrategyName";
            var stats = DailySummaryBasicStatsRaw(advId, startDate, endDate, sql);
            foreach (var stat in stats)
            {
                stat.ComputeCalculatedStats();
                yield return stat;
            }
        }
        //public IEnumerable<BasicStat> StrategyDailySummaryBasicStats(int advId, DateTime startDate, DateTime endDate)
        //{
        //    string sql = "select * from td.fStrategySummaryBasicStats(@p1, @p2, @p3)";
        //    return DailySummaryBasicStatsRaw(advId, startDate, endDate, sql);
        //}

        public IEnumerable<BasicStat> CreativePerfBasicStats2(int advId) // *** does not compute spend markup ***
        {
            DateTime yesterday = DateTime.Today.AddDays(-1);
            DateTime startDate = EarliestStatDate_TDad(advId) ?? yesterday;

            var sums = TDadSummaries(startDate, yesterday, advId: advId);
            var adGroups = sums.GroupBy(s => s.TDad);
            var stats = adGroups.Select(g => new BasicStat
            {
                AdId = g.Key.Id,
                AdName = g.Key.Name,
                Impressions = g.Sum(s => s.Impressions),
                Clicks = g.Sum(s => s.Clicks),
                PostClickConv = g.Sum(s => s.PostClickConv),
                PostViewConv = g.Sum(s => s.PostViewConv),
                MediaSpend = g.Sum(s => s.Cost)
            });
            foreach (var stat in stats)
            {
                stat.Conversions = stat.PostClickConv + stat.PostViewConv;
                stat.ComputeCalculatedStats();
                yield return stat;
            }
        }

        public IEnumerable<BasicStat> CreativePerfBasicStats(int advId, DateTime? startDate = null, DateTime? endDate = null, bool includeInfo = false)
        {
            DateTime yesterday = DateTime.Today.AddDays(-1);
            if (!startDate.HasValue)
                startDate = EarliestStatDate_TDad(advId) ?? yesterday; // if no stats, just set to yesterday
            if (!endDate.HasValue)
                endDate = yesterday;

            var sql = "select * from td.fCreativeProgressBasicStats(@p1, @p2, @p3, NULL)";
            var stats = DailySummaryBasicStatsRaw(advId, startDate.Value, endDate.Value, sql);
            if (includeInfo)
                return CreativeStatsWithInfo(stats);
            else
                return stats;
        }
        private IEnumerable<BasicStat> CreativeStatsWithInfo(IEnumerable<BasicStat> stats)
        {
            var statsList = stats.ToList();
            var adIds = statsList.Select(s => s.AdId).ToArray();
            var ads = context.TDads.Where(a => adIds.Contains(a.Id))
                .Select(a => new { a.Id, a.Url, a.Width, a.Height });

            foreach (var stat in statsList)
            {
                var ad = ads.Where(a => a.Id == stat.AdId).First();
                stat.Url = ad.Url;
                stat.AdWidth = ad.Width;
                stat.AdHeight = ad.Height;
                //yield return stat;
            }
            return statsList;
        }

        public IEnumerable<BasicStat> MTDSiteBasicStats(int advId, DateTime endDate)
        {
            var startDate = new DateTime(endDate.Year, endDate.Month, 1);
            return SiteSummaryBasicStatsRaw(advId, startDate, endDate); // since the site stats are only on the 1st, this works to get one stat per site
        }
        // TODO: a middle method that groups by SiteId/SiteName and sums the stats (and computes...)
        private IEnumerable<BasicStat> SiteSummaryBasicStatsRaw(int advId, DateTime startDate, DateTime endDate)
        {
            var sql = "select * from td.fSiteSummaryBasicStats(@p1, @p2, @p3)";
            return DailySummaryBasicStatsRaw(advId, startDate, endDate, sql);
        }

        // Return value is unexecuted query (?)
        private IEnumerable<BasicStat> DailySummaryBasicStatsRaw(int advId, DateTime startDate, DateTime endDate, string sql = null)
        {
            if (sql == null)
                sql = "select * from td.fDailySummaryBasicStats(@p1, @p2, @p3)";
            return context.Database.SqlQuery<BasicStat>(
                sql,
                new SqlParameter("@p1", advId),
                new SqlParameter("@p2", startDate),
                new SqlParameter("@p3", endDate)
                );
            //Note: With default sql, calculated stats are computed by SQL Server
            //      Can return multiple rows per day... (one per campaign/account)
        }

        public BasicStat MTDBasicStat(int advId, DateTime endDate)
        {
            var startDate = new DateTime(endDate.Year, endDate.Month, 1);
            return DateRangeBasicStat(advId, startDate, endDate);
        }

        //NOTE: as of now, basicstat.Budget will only be valid if startDate is the 1st and endDate is within the same month
        public BasicStat DateRangeBasicStat(int advId, DateTime startDate, DateTime endDate)
        {
            var stats = DailySummaryBasicStatsRaw(advId, startDate, endDate).ToList();
            if (stats.Count() == 0)
            {
                return new BasicStat
                {
                    StartDate = startDate,
                    Date = endDate
                };
            }
            var stat = new BasicStat
            {
                StartDate = startDate,
                Date = endDate,
                Impressions = stats.Sum(s => s.Impressions),
                Clicks = stats.Sum(s => s.Clicks),
                Conversions = stats.Sum(s => s.Conversions),
                MediaSpend = stats.Sum(s => s.MediaSpend),
                MgmtFee = stats.Sum(s => s.MgmtFee)
            };
            if (startDate.Day == 1 && startDate.Month == endDate.Month && startDate.Year == endDate.Year)
                stat.Budget = stats.First().Budget;
            stat.ComputeCalculatedStats();
            return stat;
        }

////Doing the summing in SQL Server... only issue is with Budget which shouldn't be summed (would have to group by Budget or something)
//        public BasicStat DateRangeBasicStatX(int advId, DateTime startDate, DateTime endDate)
//        {
//            var sql = @"select sum(Impressions) as Impressions, sum(Clicks) as Clicks, sum(Conversions) as Conversions, sum(MediaSpend) as MediaSpend, sum(MgmtFee) as MgmtFee
//from td.fDailySummaryBasicStats(@p1, @p2, @p3)";
//            var stats = context.Database.SqlQuery<BasicStat>(
//                sql,
//                new SqlParameter("@p1", advId),
//                new SqlParameter("@p2", startDate),
//                new SqlParameter("@p3", endDate)
//                ).ToList();
//            if (stats.Count() == 0)
//                return new BasicStat();
//
//            var stat = stats.First();
//            stat.ComputeCalculatedStats();
//            return stat;
//        }

        public IEnumerable<LeadInfo> MTDLeadInfos(int advId, DateTime endDate)
        {
            var startDate = new DateTime(endDate.Year, endDate.Month, 1);
            return LeadInfosRaw(advId, startDate, endDate);
        }
        public IEnumerable<LeadInfo> LeadInfos(int advId, DateTime? startDate, DateTime? endDate)
        {
            DateTime yesterday = DateTime.Today.AddDays(-1);
            if (!startDate.HasValue)
                startDate = EarliestStatDate_Conv(advId) ?? yesterday; // if no convs, just set to yesterday
            if (!endDate.HasValue)
                endDate = yesterday;
            //if (!startDate.HasValue)
            //    startDate = EarliestStatDate_Conv(advId) ?? (endDate.Value < yesterday ? endDate.Value : yesterday);
            return LeadInfosRaw(advId, startDate.Value, endDate.Value);
        }
        private IEnumerable<LeadInfo> LeadInfosRaw(int advId, DateTime startDate, DateTime endDate, bool includeFullEndDate = true, string sql = null)
        {
            if (sql == null)
                sql = "select * from td.fLeadIDs(@p1, @p2, @p3)";
            if (includeFullEndDate)
                endDate = endDate.Date.AddDays(1).AddSeconds(-1); // to include through 23:59:59 on the specified day
            return context.Database.SqlQuery<LeadInfo>(
                sql,
                new SqlParameter("@p1", advId),
                new SqlParameter("@p2", startDate),
                new SqlParameter("@p3", endDate)
                );
        }

        public DateTime? EarliestStatDate(int? advId, bool checkAll = false)
        {
            var earliest = EarliestStatDate_Daily(advId);
            if (checkAll)
            {
                var earliestStrat = EarliestStatDate_Strategy(advId);
                if (!earliest.HasValue || (earliestStrat.HasValue && earliestStrat.Value < earliest.Value))
                    earliest = earliestStrat;
                var earliestTDad = EarliestStatDate_TDad(advId);
                if (!earliest.HasValue || (earliestTDad.HasValue && earliestTDad.Value < earliest.Value))
                    earliest = earliestTDad;
                //NOTE: not including Site stats or Convs
            }
            return earliest;
        }
        private DateTime? EarliestStatDate_Daily(int? advId)
        {
            DateTime? earliest = null;
            var dSums = DailySummaries(null, null, advId: advId);
            if (dSums.Any())
                earliest = dSums.Min(s => s.Date);
            return earliest;
        }
        public DateTime? EarliestStatDate_Strategy(int? advId)
        {
            DateTime? earliest = null;
            var sums = StrategySummaries(null, null, advId: advId);
            if (sums.Any())
                earliest = sums.Min(s => s.Date);
            return earliest;
        }
        public DateTime? EarliestStatDate_TDad(int? advId)
        {
            DateTime? earliest = null;
            var sums = TDadSummaries(null, null, advId: advId);
            if (sums.Any())
                earliest = sums.Min(s => s.Date);
            return earliest;
        }
        public DateTime? EarliestStatDate_Conv(int? advId)
        {
            DateTime? earliest = null;
            var convs = Convs(null, null, advId: advId);
            if (convs.Any())
                earliest = convs.Min(s => s.Time);
            return earliest;
        }

        public TDStatsGauge GetStatsGauge(ExtAccount extAccount)
        {
            if (extAccount != null)
            {
                var gauge = GetStatsGauge(extAccount.Id);
                gauge.ExtAccount = extAccount;
                return gauge;
            }
            else
                return GetStatsGauge();
        }
        public TDStatsGauge GetStatsGauge(int? acctId = null) //Note: doesn't fill in ExtAccount
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

        public IQueryable<DailySummary> DailySummaries(DateTime? startDate, DateTime? endDate, int? acctId = null, int? campId = null, int? advId = null)
        {
            var dSums = context.DailySummaries.AsQueryable();
            if (startDate.HasValue)
                dSums = dSums.Where(ds => ds.Date >= startDate.Value);
            if (endDate.HasValue)
                dSums = dSums.Where(ds => ds.Date <= endDate.Value);
            if (acctId.HasValue)
                dSums = dSums.Where(ds => ds.AccountId == acctId.Value);
            if (campId.HasValue)
                dSums = dSums.Where(ds => ds.ExtAccount.CampaignId == campId.Value);
            if (advId.HasValue)
                dSums = dSums.Where(ds => ds.ExtAccount.Campaign.AdvertiserId == advId.Value);
            return dSums;
        }

        public IQueryable<StrategySummary> StrategySummaries(DateTime? startDate, DateTime? endDate, int? stratId = null, int? acctId = null, int? campId = null, int? advId = null)
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
                sSums = sSums.Where(s => s.Strategy.ExtAccount.CampaignId == campId.Value);
            if (advId.HasValue)
                sSums = sSums.Where(s => s.Strategy.ExtAccount.Campaign.AdvertiserId == advId.Value);
            return sSums;
        }

        public IQueryable<TDadSummary> TDadSummaries(DateTime? startDate, DateTime? endDate, int? tdadId = null, int? acctId = null, int? campId = null, int? advId = null)
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
                tSums = tSums.Where(s => s.TDad.ExtAccount.CampaignId == campId.Value);
            if (advId.HasValue)
                tSums = tSums.Where(s => s.TDad.ExtAccount.Campaign.AdvertiserId == advId.Value);
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
                sSums = sSums.Where(s => s.ExtAccount.CampaignId == campId.Value);
            return sSums;
        }

        public IQueryable<Conv> Convs(DateTime? startDate, DateTime? endDate, int? acctId = null, int? campId = null, int? advId = null)
        {
            var convs = context.Convs.AsQueryable();
            if (startDate.HasValue)
                convs = convs.Where(s => s.Time >= startDate.Value);
            if (endDate.HasValue)
            {
                var date = endDate.Value.AddDays(1);
                convs = convs.Where(s => s.Time < date);
                // Include up to 11:59:59.999... on the endDate specified
            }
            if (acctId.HasValue)
                convs = convs.Where(s => s.AccountId == acctId.Value);
            if (campId.HasValue)
                convs = convs.Where(s => s.ExtAccount.CampaignId == campId.Value);
            if (advId.HasValue)
                convs = convs.Where(s => s.ExtAccount.Campaign.AdvertiserId == advId.Value);
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
