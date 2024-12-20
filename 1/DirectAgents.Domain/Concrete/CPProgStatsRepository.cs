﻿using DirectAgents.Domain.DTO;
using DirectAgents.Domain.Entities.CPProg;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using Z.EntityFramework.Plus;

namespace DirectAgents.Domain.Concrete
{
    public partial class CPProgRepository
    {
        public IEnumerable<BasicStat> DayOfWeekBasicStats(int advId, DateTime? startDate = null, DateTime? endDate = null, bool mondayFirst = false)
        {
            var sql = @"select DatePart(weekday, Date) as Day, sum(Impressions) as Impressions, sum(Clicks) as Clicks, sum(Conversions) as Conversions, sum(MediaSpend) as MediaSpend, sum(MgmtFee) as MgmtFee
from td.fDailySummaryBasicStats(@p1, @p2, @p3)
group by DatePart(weekday, Date) order by Day";
            var stats = DailyBasicStatsWithCompute(advId, startDate, endDate, sql: sql);
            if (mondayFirst)
            {
                return stats.OrderBy(s => s.Day < 2).ThenBy(s => s.Day);
            }
            else
            {
                return stats;
            }
        }

        public IEnumerable<BasicStat> WeeklyBasicStats(int advId, DateTime? startDate = null, DateTime? endDate = null, bool computeCalculatedStats = true)
        {
            return WeeklyMonthlyBasicStats(advId, startDate, endDate, computeCalculatedStats, weeklyNotMonthly: true);
        }
        public IEnumerable<BasicStat> MonthlyBasicStats(int advId, DateTime? startDate = null, DateTime? endDate = null, bool computeCalculatedStats = true)
        {
            return WeeklyMonthlyBasicStats(advId, startDate, endDate, computeCalculatedStats, weeklyNotMonthly: false);
        }
        private IEnumerable<BasicStat> WeeklyMonthlyBasicStats(int advId, DateTime? startDate = null, DateTime? endDate = null, bool computeCalculatedStats = true, bool weeklyNotMonthly = true)
        {
            var dailyStats = DailyBasicStatsWithCompute(advId, startDate, endDate, computeCalculatedStats: false, computeWeekStartDate: weeklyNotMonthly, computeMonthStartDate: !weeklyNotMonthly);
            var groupedStats = dailyStats.GroupBy(s => s.StartDate).OrderBy(g => g.Key);
            foreach (var group in groupedStats)
            {
                var groupStat = new BasicStat
                {
                    Date = group.Key,
                    Impressions = group.Sum(s => s.Impressions),
                    Clicks = group.Sum(s => s.Clicks),
                    Conversions = group.Sum(s => s.Conversions),
                    MediaSpend = group.Sum(s => s.MediaSpend),
                    MgmtFee = group.Sum(s => s.MgmtFee)
                };
                if (computeCalculatedStats)
                {
                    groupStat.ComputeCalculatedStats();
                }

                yield return groupStat;
            }
        }

        public IEnumerable<BasicStat> DailyBasicStats(int advId, DateTime? startDate = null, DateTime? endDate = null, bool computeCalculatedStats = true)
        {
            return DailyBasicStatsWithCompute(advId, startDate, endDate, computeCalculatedStats: computeCalculatedStats);
        }

        private IEnumerable<BasicStat> DailyBasicStatsWithCompute(int advId, DateTime? startDate, DateTime? endDate, string sql = null, bool computeCalculatedStats = true, bool computeWeekStartDate = false, bool computeMonthStartDate = false)
        {
            DateTime yesterday = DateTime.Today.AddDays(-1);
            if (!startDate.HasValue)
            {
                startDate = EarliestStatDate(advId) ?? yesterday; // if no stats, just set to yesterday
            }

            if (!endDate.HasValue)
            {
                endDate = yesterday;
            }

            if (sql == null) // Default query - one row per day
            {
                sql = @"select Date, sum(Impressions) as Impressions, sum(Clicks) as Clicks, sum(Conversions) as Conversions, sum(MediaSpend) as MediaSpend, sum(MgmtFee) as MgmtFee
from td.fDailySummaryBasicStats(@p1, @p2, @p3)
group by Date order by Date";
            }

            var stats = DailyBasicStatsRaw(advId, startDate.Value, endDate.Value, sql);
            foreach (var stat in stats)
            {
                if (computeCalculatedStats)
                {
                    stat.ComputeCalculatedStats();
                }

                if (computeWeekStartDate)
                {
                    stat.ComputeWeekStartDate();
                }

                if (computeMonthStartDate)
                {
                    stat.ComputeMonthStartDate();
                }

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
            var stats = DailyBasicStatsRaw(advId, startDate, endDate, sql);
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
            DateTime startDate = StatsRange_TDad(advId).Earliest ?? yesterday;

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
            {
                startDate = StatsRange_TDad(advId).Earliest ?? yesterday; // if no stats, just set to yesterday
            }

            if (!endDate.HasValue)
            {
                endDate = yesterday;
            }

            var sql = "select * from td.fCreativeProgressBasicStatsGroupByName(@p1, @p2, @p3, NULL)";
            var stats = DailyBasicStatsRaw(advId, startDate.Value, endDate.Value, sql);
            if (includeInfo)
            {
                return CreativeStatsWithInfo(stats);
            }
            else
            {
                return stats;
            }
        }
        private IEnumerable<BasicStat> CreativeStatsWithInfo(IEnumerable<BasicStat> stats)
        {
            var statsList = stats.ToList();
            var adIds = statsList.Select(s => s.AdId).ToArray();
            var ads = context.TDads.Where(a => adIds.Contains(a.Id))
                .Select(a => new { a.Id, a.Url, a.Width, a.Height, a.Body, a.Headline, a.Message, a.DestinationUrl });

            foreach (var stat in statsList)
            {
                var ad = ads.Where(a => a.Id == stat.AdId).First();
                stat.Url = ad.Url;
                stat.AdWidth = ad.Width;
                stat.AdHeight = ad.Height;
                stat.AdBody = ad.Body;
                stat.AdHeadline = ad.Headline;
                stat.AdMessage = ad.Message;
                if (ad.DestinationUrl != null)
                {
                    stat.AdDestinationUrl = new Uri(ad.DestinationUrl).Host;
                }
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
            return DailyBasicStatsRaw(advId, startDate, endDate, sql);
        }

        // Return value is unexecuted query (?)
        private IEnumerable<BasicStat> DailyBasicStatsRaw(int advId, DateTime startDate, DateTime endDate, string sql = null, IEnumerable<int> includeAccountIds = null, IEnumerable<int> excludeAccountIds = null)
        {
            if (sql == null)
            {
                sql = "select * from td.fDailySummaryBasicStats(@p1, @p2, @p3)" + SqlForAccountIds(includeAccountIds, excludeAccountIds);
            }
            return context.Database.SqlQuery<BasicStat>(
                sql,
                new SqlParameter("@p1", advId),
                new SqlParameter("@p2", startDate),
                new SqlParameter("@p3", endDate)
                );
            //Note: With default sql, calculated stats are computed by SQL Server
            //      Can return multiple rows per day... (one per campaign/account)
        }

        private string SqlForAccountIds(IEnumerable<int> includeAccountIds, IEnumerable<int> excludeAccountIds)
        {
            bool anyIncludes = (includeAccountIds != null && includeAccountIds.Any());
            bool anyExcludes = (excludeAccountIds != null && excludeAccountIds.Any());
            var sql = "";
            if (anyIncludes || anyExcludes)
            {
                sql = " WHERE ";
                if (anyIncludes)
                {
                    sql += "AccountId IN (" + String.Join(",", includeAccountIds) + ")";
                    if (anyExcludes)
                    {
                        sql += " AND";
                    }
                }
                if (anyExcludes)
                {
                    sql += "AccountId NOT IN (" + String.Join(",", excludeAccountIds) + ")";
                }
            }
            return sql;
        }

        public BasicStat MTDBasicStat(int advId, DateTime endDate, IEnumerable<int> includeAccountIds = null, IEnumerable<int> excludeAccountIds = null)
        {
            var startDate = new DateTime(endDate.Year, endDate.Month, 1);
            return DateRangeBasicStat(advId, startDate, endDate, includeAccountIds: includeAccountIds, excludeAccountIds: excludeAccountIds);
        }

        //NOTE: as of now, basicstat.Budget will only be valid if startDate is the 1st and endDate is within the same month
        public BasicStat DateRangeBasicStat(int advId, DateTime startDate, DateTime endDate, IEnumerable<int> includeAccountIds = null, IEnumerable<int> excludeAccountIds = null)
        {
            var stats = DailyBasicStatsRaw(advId, startDate, endDate, includeAccountIds: includeAccountIds, excludeAccountIds: excludeAccountIds).ToList();
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
            {
                stat.Budget = stats.First().Budget;
            }

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
            {
                startDate = StatsRange_Conv(advId).Earliest ?? yesterday; // (if no convs, just set to yesterday)
            }

            if (!endDate.HasValue)
            {
                endDate = yesterday;
            }
            //if (!startDate.HasValue)
            //    startDate = EarliestStatDate_Conv(advId) ?? (endDate.Value < yesterday ? endDate.Value : yesterday);
            return LeadInfosRaw(advId, startDate.Value, endDate.Value);
        }
        private IEnumerable<LeadInfo> LeadInfosRaw(int advId, DateTime startDate, DateTime endDate, bool includeFullEndDate = true, string sql = null)
        {
            if (sql == null)
            {
                sql = "select * from td.fLeadIDs(@p1, @p2, @p3)";
            }

            if (includeFullEndDate)
            {
                endDate = endDate.Date.AddDays(1).AddSeconds(-1); // to include through 23:59:59 on the specified day
            }

            return context.Database.SqlQuery<LeadInfo>(
                sql,
                new SqlParameter("@p1", advId),
                new SqlParameter("@p2", startDate),
                new SqlParameter("@p3", endDate)
                );
        }

        public DateTime? EarliestStatDate(int? advId, bool checkAll = false)
        {
            var earliest = StatsRange_Daily(advId: advId).Earliest;
            if (checkAll)
            {
                // Check earliest Strat/TDad stats and use that if it's earlier or if "earliest" is still null...
                var earliestStrat = StatsRange_Strategy(advId).Earliest;
                if (!earliest.HasValue || (earliestStrat.HasValue && earliestStrat.Value < earliest.Value))
                {
                    earliest = earliestStrat;
                }

                var earliestTDad = StatsRange_TDad(advId).Earliest;
                if (!earliest.HasValue || (earliestTDad.HasValue && earliestTDad.Value < earliest.Value))
                {
                    earliest = earliestTDad;
                }
                //NOTE: not including Site stats or Convs
            }
            return earliest;
        }

        public IStatsRange StatsRange_All(int? advId, bool includeConvs = false, bool includeSiteSummaries = false)
        {
            var ssRange = new SimpleStatsRange();
            ssRange.UpdateWith(StatsRange_Daily(advId: advId));
            ssRange.UpdateWith(StatsRange_Strategy(advId));
            ssRange.UpdateWith(StatsRange_TDad(advId));

            if (includeConvs)
            {
                ssRange.UpdateWith(StatsRange_Conv(advId));
            }

            if (includeSiteSummaries) // usually don't include b/c they're dated on the 1st of the month by convention
            {
                ssRange.UpdateWith(StatsRange_Site(advId));
            }

            return ssRange;
        }

        //TODO: allow passing in (acctId, platformId?), campId

        public IStatsRange StatsRange_Daily(int? campId = null, int? advId = null)
        {
            var sums = DailySummaries(null, null, campId: campId, advId: advId);
            return new StatsSummaryRange(sums);
        }
        public IStatsRange StatsRange_Strategy(int? advId)
        {
            var sums = StrategySummaries(null, null, advId: advId);
            return new StatsSummaryRange(sums);
        }
        public IStatsRange StatsRange_TDad(int? advId)
        {
            var sums = TDadSummaries(null, null, advId: advId);
            return new StatsSummaryRange(sums);
        }
        public IStatsRange StatsRange_Site(int? advId)
        {
            var sums = SiteSummaries(null, null, advId: advId);
            return new StatsSummaryRange(sums);
        }
        public IStatsRange StatsRange_Conv(int? advId)
        {
            var convs = Convs(null, null, advId: advId);
            return new ConvRange(convs);
        }

        public TDStatsGauge GetStatsGauge(ExtAccount extAccount = null, Platform platform = null, bool extended = false)
        {
            if (extAccount != null || platform != null)
            {
                var gauge = GetStatsGaugeViaIds(
                    acctId: (extAccount != null) ? (int?)extAccount.Id : null,
                    platformId: (platform != null) ? (int?)platform.Id : null,
                    extended: extended
                );
                gauge.ExtAccount = extAccount;
                gauge.Platform = platform;
                return gauge;
            }
            else
            {
                return GetStatsGaugeViaIds();
            }
        }
        //Note: doesn't fill in ExtAccount or Platform
        public TDStatsGauge GetStatsGaugeViaIds(int? acctId = null, int? platformId = null, bool extended = false)
        {
            var gauge = new TDStatsGauge();

            var dSums = DailySummaries(null, null, acctId: acctId, platformId: platformId);
            var ssRange = new StatsSummaryRange(dSums);
            gauge.Daily.Earliest = ssRange.Earliest;
            gauge.Daily.Latest = ssRange.Latest;

            var sSums = StrategySummaries(null, null, acctId: acctId, platformId: platformId);
            ssRange = new StatsSummaryRange(sSums);
            gauge.Strategy.Earliest = ssRange.Earliest;
            gauge.Strategy.Latest = ssRange.Latest;

            var tSums = TDadSummaries(null, null, acctId: acctId, platformId: platformId);
            ssRange = new StatsSummaryRange(tSums);
            gauge.Creative.Earliest = ssRange.Earliest;
            gauge.Creative.Latest = ssRange.Latest;

            var aSums = AdSetSummaries(null, null, acctId: acctId, platformId: platformId);
            ssRange = new StatsSummaryRange(aSums);
            gauge.AdSet.Earliest = ssRange.Earliest;
            gauge.AdSet.Latest = ssRange.Latest;

            if (extended) // the following are older / not used much
            {
                var siteSums = SiteSummaries(null, null, acctId: acctId, platformId: platformId);
                ssRange = new StatsSummaryRange(siteSums);
                gauge.Site.Earliest = ssRange.Earliest;
                gauge.Site.Latest = ssRange.Latest;

                var convs = Convs(null, null, acctId: acctId, platformId: platformId);
                var cRange = new ConvRange(convs);
                gauge.Conv.Earliest = cRange.Earliest;
                gauge.Conv.Latest = cRange.Latest;
            }
            var actions = AdSetActions(null, null, acctId: acctId, platformId: platformId);
            ssRange = new StatsSummaryRange(actions);
            gauge.Action.Earliest = ssRange.Earliest;
            gauge.Action.Latest = ssRange.Latest;

            return gauge;
        }

        private IEnumerable<SimpleStatsRange> DailySummaryRangesByPlatform()
        {
            var platGroups =
                from sum in context.DailySummaries
                group sum by sum.ExtAccount.PlatformId
                    into g
                select new SimpleStatsRange
                {
                    Id = g.Key,
                    Earliest = g.Min(x => x.Date),
                    Latest = g.Max(x => x.Date)
                };
            return platGroups.ToList();
        }
        private IEnumerable<SimpleStatsRange> StrategySummaryRangesByPlatform()
        {
            var platGroups =
                from sum in context.StrategySummaries
                group sum by sum.Strategy.ExtAccount.PlatformId
                    into g
                select new SimpleStatsRange
                {
                    Id = g.Key,
                    Earliest = g.Min(x => x.Date),
                    Latest = g.Max(x => x.Date)
                };
            return platGroups.ToList();
        }
        private IEnumerable<SimpleStatsRange> AdSetSummaryRangesByPlatform()
        {
            var platGroups =
                from sum in context.AdSetSummaries
                group sum by sum.AdSet.ExtAccount.PlatformId
                    into g
                select new SimpleStatsRange
                {
                    Id = g.Key,
                    Earliest = g.Min(x => x.Date),
                    Latest = g.Max(x => x.Date)
                };
            return platGroups.ToList();
        }
        private IEnumerable<SimpleStatsRange> AdSetActionRangesByPlatform()
        {
            var platGroups =
                from stat in context.AdSetActions
                group stat by stat.AdSet.ExtAccount.PlatformId
                    into g
                select new SimpleStatsRange
                {
                    Id = g.Key,
                    Earliest = g.Min(x => x.Date),
                    Latest = g.Max(x => x.Date)
                };
            return platGroups.ToList();
        }
        private IEnumerable<SimpleStatsRange> TDadSummaryRangesByPlatform()
        {
            var platGroups =
                from sum in context.TDadSummaries
                group sum by sum.TDad.ExtAccount.PlatformId
                    into g
                select new SimpleStatsRange
                {
                    Id = g.Key,
                    Earliest = g.Min(x => x.Date),
                    Latest = g.Max(x => x.Date)
                };
            return platGroups.ToList();
        }
        private IEnumerable<SimpleStatsRange> KeywordSummaryRangesByPlatform()
        {
            var platGroups =
                from sum in context.KeywordSummaries
                group sum by sum.Keyword.ExtAccount.PlatformId
                into g
                select new SimpleStatsRange
                {
                    Id = g.Key,
                    Earliest = g.Min(x => x.Date),
                    Latest = g.Max(x => x.Date)
                };
            return platGroups.ToList();
        }
        private IEnumerable<SimpleStatsRange> SearchTermSummaryRangesByPlatform()
        {
            var platGroups =
                from sum in context.SearchTermSummaries
                group sum by sum.SearchTerm.ExtAccount.PlatformId
                into g
                select new SimpleStatsRange
                {
                    Id = g.Key,
                    Earliest = g.Min(x => x.Date),
                    Latest = g.Max(x => x.Date)
                };
            return platGroups.ToList();
        }
        private IEnumerable<SimpleStatsRange> SiteSummaryRangesByPlatform()
        {
            var platGroups =
                from sum in context.SiteSummaries
                group sum by sum.ExtAccount.PlatformId
                    into g
                select new SimpleStatsRange
                {
                    Id = g.Key,
                    Earliest = g.Min(x => x.Date),
                    Latest = g.Max(x => x.Date)
                };
            return platGroups.ToList();
        }
        private IEnumerable<SimpleStatsRange> ConvRangesByPlatform()
        {
            var platGroups =
                from conv in context.Convs
                group conv by conv.ExtAccount.PlatformId
                    into g
                select new SimpleStatsRange
                {
                    Id = g.Key,
                    Earliest = g.Min(x => x.Time),
                    Latest = g.Max(x => x.Time)
                };
            return platGroups.ToList();
        }

        private IEnumerable<SimpleStatsRange> DailySummaryRangesByAccount(int? platformId = null, int? campId = null, DateTime? minDate = null)
        {
            var sums = DailySummaries(minDate, null, platformId: platformId, campId: campId);
            var acctRanges = sums.GroupBy(x => x.AccountId).Select(g =>
                new SimpleStatsRange { Id = g.Key, Earliest = g.Min(x => x.Date), Latest = g.Max(x => x.Date) });
            return acctRanges.ToList();
        }
        private IEnumerable<SimpleStatsRange> StrategySummaryRangesByAccount(int? platformId = null, int? campId = null, DateTime? minDate = null)
        {
            var sums = StrategySummaries(minDate, null, platformId: platformId, campId: campId);
            var acctRanges = sums.GroupBy(x => x.Strategy.AccountId).Select(g =>
                new SimpleStatsRange { Id = g.Key, Earliest = g.Min(x => x.Date), Latest = g.Max(x => x.Date) });
            return acctRanges.ToList();
        }
        private IEnumerable<SimpleStatsRange> AdSetSummaryRangesByAccount(int? platformId = null, int? campId = null, DateTime? minDate = null)
        {
            var sums = AdSetSummaries(minDate, null, platformId: platformId, campId: campId);
            var acctRanges = sums.GroupBy(x => x.AdSet.AccountId).Select(g =>
                new SimpleStatsRange { Id = g.Key, Earliest = g.Min(x => x.Date), Latest = g.Max(x => x.Date) });
            return acctRanges.ToList();
        }
        private IEnumerable<SimpleStatsRange> AdSetActionRangesByAccount(int? platformId = null, int? campId = null, DateTime? minDate = null)
        {
            var actionStats = AdSetActions(minDate, null, platformId: platformId, campId: campId);
            var acctRanges = actionStats.GroupBy(x => x.AdSet.AccountId).Select(g =>
                new SimpleStatsRange { Id = g.Key, Earliest = g.Min(x => x.Date), Latest = g.Max(x => x.Date) });
            return acctRanges.ToList();
        }
        private IEnumerable<SimpleStatsRange> TDadSummaryRangesByAccount(int? platformId = null, int? campId = null, DateTime? minDate = null)
        {
            var sums = TDadSummaries(minDate, null, platformId: platformId, campId: campId);
            var acctRanges = sums.GroupBy(x => x.TDad.AccountId).Select(g =>
                new SimpleStatsRange { Id = g.Key, Earliest = g.Min(x => x.Date), Latest = g.Max(x => x.Date) });
            return acctRanges.ToList();
        }
        private IEnumerable<SimpleStatsRange> KeywordSummaryRangesByAccount(int? platformId = null, int? campId = null, DateTime? minDate = null)
        {
            var sums = KeywordSummaries(minDate, null, platformId: platformId, campId: campId);
            var acctRanges = sums.GroupBy(x => x.Keyword.AccountId).Select(g =>
                new SimpleStatsRange { Id = g.Key, Earliest = g.Min(x => x.Date), Latest = g.Max(x => x.Date) });
            return acctRanges.ToList();
        }
        private IEnumerable<SimpleStatsRange> SearchTermSummaryRangesByAccount(int? platformId = null, int? campId = null, DateTime? minDate = null)
        {
            var sums = SearchTermSummaries(minDate, null, platformId: platformId, campId: campId);
            var acctRanges = sums.GroupBy(x => x.SearchTerm.AccountId).Select(g =>
                new SimpleStatsRange { Id = g.Key, Earliest = g.Min(x => x.Date), Latest = g.Max(x => x.Date) });
            return acctRanges.ToList();
        }
        private IEnumerable<SimpleStatsRange> SiteSummaryRangeByAccount(int? platformId = null, int? campId = null, DateTime? minDate = null)
        {
            var sums = SiteSummaries(minDate, null, platformId: platformId, campId: campId);
            var acctRanges = sums.GroupBy(x => x.AccountId).Select(g =>
                new SimpleStatsRange { Id = g.Key, Earliest = g.Min(x => x.Date), Latest = g.Max(x => x.Date) });
            return acctRanges.ToList();
        }
        private IEnumerable<SimpleStatsRange> ConvRangeByAccount(int? platformId = null, int? campId = null, DateTime? minDate = null)
        {
            var convs = Convs(minDate, null, platformId: platformId, campId: campId);
            var acctRanges = convs.GroupBy(x => x.AccountId).Select(g =>
                new SimpleStatsRange { Id = g.Key, Earliest = g.Min(x => x.Time), Latest = g.Max(x => x.Time) });
            return acctRanges.ToList();
        }

        public IEnumerable<TDStatsGauge> StatsGaugesByPlatform(bool extended = false)
        {
            var dsRanges = DailySummaryRangesByPlatform();
            var ssRanges = StrategySummaryRangesByPlatform();
            var adsRanges = AdSetSummaryRangesByPlatform();
            var actRanges = AdSetActionRangesByPlatform();
            var adRanges = TDadSummaryRangesByPlatform();
            var keywRanges = KeywordSummaryRangesByPlatform();
            var stermRanges = SearchTermSummaryRangesByPlatform();
            var siteRanges = extended ? SiteSummaryRangesByPlatform() : SimpleStatsRange.EmptyIEnumerable();
            var convRanges = extended ? ConvRangesByPlatform() : SimpleStatsRange.EmptyIEnumerable();
            var platforms = Platforms().ToList();
            var x = from plat in platforms
                    join dsRange in dsRanges
                    on plat.Id equals dsRange.Id into dsJoined
                    from dsr in dsJoined.DefaultIfEmpty() // left join
                    join ssRange in ssRanges
                    on plat.Id equals ssRange.Id into ssJoined
                    from ssr in ssJoined.DefaultIfEmpty() // left join
                    join adsRange in adsRanges
                    on plat.Id equals adsRange.Id into adsJoined
                    from adsr in adsJoined.DefaultIfEmpty() // left join
                    join actRange in actRanges
                    on plat.Id equals actRange.Id into actJoined
                    from act in actJoined.DefaultIfEmpty() // left join
                    join adRange in adRanges
                    on plat.Id equals adRange.Id into adJoined
                    from ad in adJoined.DefaultIfEmpty() // left join
                    join keywRange in keywRanges
                    on plat.Id equals keywRange.Id into keywJoined
                    from keyw in keywJoined.DefaultIfEmpty() // left join
                    join stermRange in stermRanges
                    on plat.Id equals stermRange.Id into stermJoined
                    from sterm in stermJoined.DefaultIfEmpty() // left join
                    join siteRange in siteRanges
                    on plat.Id equals siteRange.Id into siteJoined
                    from site in siteJoined.DefaultIfEmpty() // left join
                    join convRange in convRanges
                    on plat.Id equals convRange.Id into convJoined
                    from conv in convJoined.DefaultIfEmpty() // left join
                    select new TDStatsGauge(dsr, ssr, adsr, act, ad, keyw, sterm, site, conv)
                    {
                        Platform = plat
                    };
            return x; // ToList() needed?
        }

        public IEnumerable<TDStatsGauge> StatsGaugesByAccount(int? platformId = null, int? campId = null, DateTime? minDate = null, bool extended = false)
        {
            var dsRanges = DailySummaryRangesByAccount(platformId: platformId, campId: campId, minDate: minDate);
            var ssRanges = StrategySummaryRangesByAccount(platformId: platformId, campId: campId, minDate: minDate);
            var adsRanges = AdSetSummaryRangesByAccount(platformId: platformId, campId: campId, minDate: minDate);
            var actRanges = AdSetActionRangesByAccount(platformId: platformId, campId: campId, minDate: minDate);
            var adRanges = TDadSummaryRangesByAccount(platformId: platformId, campId: campId, minDate: minDate);
            var keywRanges = KeywordSummaryRangesByAccount(platformId: platformId, campId: campId, minDate: minDate);
            var stermRanges = SearchTermSummaryRangesByAccount(platformId: platformId, campId: campId, minDate: minDate);
            var siteRanges = extended ? SiteSummaryRangeByAccount(platformId: platformId, campId: campId, minDate: minDate) : SimpleStatsRange.EmptyIEnumerable();
            var convRanges = extended ? ConvRangeByAccount(platformId: platformId, campId: campId, minDate: minDate) : SimpleStatsRange.EmptyIEnumerable();
            var extAccts = ExtAccounts(platformId: platformId, campId: campId, includePlatform: true).ToList();
            var x = from acct in extAccts
                    join dsRange in dsRanges
                    on acct.Id equals dsRange.Id into dsJoined
                    from dsr in dsJoined.DefaultIfEmpty() // left join
                    join ssRange in ssRanges
                    on acct.Id equals ssRange.Id into ssJoined
                    from ssr in ssJoined.DefaultIfEmpty() // left join
                    join adsRange in adsRanges
                    on acct.Id equals adsRange.Id into adsJoined
                    from adsr in adsJoined.DefaultIfEmpty() // left join
                    join actRange in actRanges
                    on acct.Id equals actRange.Id into actJoined
                    from act in actJoined.DefaultIfEmpty() // left join
                    join adRange in adRanges
                    on acct.Id equals adRange.Id into adJoined
                    from ad in adJoined.DefaultIfEmpty() // left join
                    join keywRange in keywRanges
                    on acct.Id equals keywRange.Id into keywJoined
                    from keyw in keywJoined.DefaultIfEmpty() // left join
                    join stermRange in stermRanges
                    on acct.Id equals stermRange.Id into stermJoined
                    from sterm in stermJoined.DefaultIfEmpty() // left join
                    join siteRange in siteRanges
                    on acct.Id equals siteRange.Id into siteJoined
                    from site in siteJoined.DefaultIfEmpty() // left join
                    join convRange in convRanges
                    on acct.Id equals convRange.Id into convJoined
                    from conv in convJoined.DefaultIfEmpty() // left join
                    select new TDStatsGauge(dsr, ssr, adsr, act, ad, keyw, sterm, site, conv)
                    {
                        ExtAccount = acct
                    };
            return x; // ToList() needed?
        }

        public DailySummary DailySummary(DateTime date, int acctId)
        {
            return context.DailySummaries.Find(date, acctId);
        }

        public bool AddDailySummary(DailySummary daySum)
        {
            if (context.DailySummaries.Any(ds => ds.Date == daySum.Date && ds.AccountId == daySum.AccountId))
            {
                return false;
            }

            if (!context.ExtAccounts.Any(ea => ea.Id == daySum.AccountId))
            {
                return false;
            }

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
            {
                daySum.ExtAccount = ExtAccount(daySum.AccountId);
            }
        }

        public IQueryable<DailySummary> DailySummaries(DateTime? startDate, DateTime? endDate, int? acctId = null, int? platformId = null, int? campId = null, int? advId = null)
        {
            var dSums = context.DailySummaries.AsQueryable();
            dSums = FilterSummariesByDate(dSums, startDate, endDate);

            if (acctId.HasValue)
            {
                dSums = dSums.Where(ds => ds.AccountId == acctId.Value);
            }
            if (platformId.HasValue)
            {
                dSums = dSums.Where(ds => ds.ExtAccount.PlatformId == platformId.Value);
            }
            if (campId.HasValue)
            {
                dSums = dSums.Where(ds => ds.ExtAccount.CampaignId == campId.Value);
            }
            if (advId.HasValue)
            {
                dSums = dSums.Where(ds => ds.ExtAccount.Campaign.AdvertiserId == advId.Value);
            }

            return dSums;
        }

        public IQueryable<StrategySummary> StrategySummaries(DateTime? startDate, DateTime? endDate, int? stratId = null, int? acctId = null, int? platformId = null, int? campId = null, int? advId = null)
        {
            var sSums = context.StrategySummaries.AsQueryable();
            sSums = FilterSummariesByDate(sSums, startDate, endDate);

            if (stratId.HasValue)
            {
                sSums = sSums.Where(s => s.StrategyId == stratId.Value);
            }
            if (acctId.HasValue)
            {
                sSums = sSums.Where(s => s.Strategy.AccountId == acctId.Value);
            }
            if (platformId.HasValue)
            {
                sSums = sSums.Where(ds => ds.Strategy.ExtAccount.PlatformId == platformId.Value);
            }
            if (campId.HasValue)
            {
                sSums = sSums.Where(ds => ds.Strategy.ExtAccount.CampaignId == campId.Value);
            }
            if (advId.HasValue)
            {
                sSums = sSums.Where(ds => ds.Strategy.ExtAccount.Campaign.AdvertiserId == advId.Value);
            }

            return sSums;
        }

        public IQueryable<AdSetSummary> AdSetSummaries(DateTime? startDate, DateTime? endDate, int? adsetId = null, int? stratId = null, int? acctId = null, int? platformId = null, int? campId = null, int? advId = null)
        {
            var aSums = context.AdSetSummaries.AsQueryable();
            aSums = FilterSummariesByDate(aSums, startDate, endDate);

            if (adsetId.HasValue)
            {
                aSums = aSums.Where(s => s.AdSetId == adsetId.Value);
            }
            if (stratId.HasValue)
            {
                aSums = aSums.Where(s => s.AdSet.StrategyId == stratId.Value);
            }
            if (acctId.HasValue)
            {
                aSums = aSums.Where(s => s.AdSet.AccountId == acctId.Value);
            }
            if (platformId.HasValue)
            {
                aSums = aSums.Where(ds => ds.AdSet.ExtAccount.PlatformId == platformId.Value);
            }
            if (campId.HasValue)
            {
                aSums = aSums.Where(ds => ds.AdSet.ExtAccount.CampaignId == campId.Value);
            }
            if (advId.HasValue)
            {
                aSums = aSums.Where(ds => ds.AdSet.ExtAccount.Campaign.AdvertiserId == advId.Value);
            }

            return aSums;
        }

        public IQueryable<TDadSummary> TDadSummaries(DateTime? startDate, DateTime? endDate, int? tdadId = null, int? acctId = null, int? platformId = null, int? campId = null, int? advId = null, int? adSetId = null)
        {
            var tSums = context.TDadSummaries.AsQueryable();
            tSums = FilterSummariesByDate(tSums, startDate, endDate);

            if (tdadId.HasValue)
            {
                tSums = tSums.Where(s => s.TDadId == tdadId.Value);
            }
            if (adSetId.HasValue)
            {
                tSums = tSums.Where(s => s.TDad.AdSetId == adSetId.Value);
            }
            if (acctId.HasValue)
            {
                tSums = tSums.Where(s => s.TDad.AccountId == acctId.Value);
            }
            if (platformId.HasValue)
            {
                tSums = tSums.Where(ds => ds.TDad.ExtAccount.PlatformId == platformId.Value);
            }
            if (campId.HasValue)
            {
                tSums = tSums.Where(ds => ds.TDad.ExtAccount.CampaignId == campId.Value);
            }
            if (advId.HasValue)
            {
                tSums = tSums.Where(ds => ds.TDad.ExtAccount.Campaign.AdvertiserId == advId.Value);
            }

            return tSums;
        }

        public IQueryable<SiteSummary> SiteSummaries(DateTime? startDate, DateTime? endDate, int? acctId = null, int? platformId = null, int? campId = null, int? advId = null)
        {
            var sSums = context.SiteSummaries.AsQueryable();
            sSums = FilterSummariesByDate(sSums, startDate, endDate);

            if (acctId.HasValue)
            {
                sSums = sSums.Where(s => s.AccountId == acctId.Value);
            }
            if (platformId.HasValue)
            {
                sSums = sSums.Where(ds => ds.ExtAccount.PlatformId == platformId.Value);
            }
            if (campId.HasValue)
            {
                sSums = sSums.Where(ds => ds.ExtAccount.CampaignId == campId.Value);
            }
            if (advId.HasValue)
            {
                sSums = sSums.Where(ds => ds.ExtAccount.Campaign.AdvertiserId == advId.Value);
            }

            return sSums;
        }

        public IQueryable<KeywordSummary> KeywordSummaries(DateTime? startDate, DateTime? endDate, int? keywId = null, int? adsetId = null, int? stratId = null, int? acctId = null, int? platformId = null, int? campId = null, int? advId = null)
        {
            var sums = context.KeywordSummaries.AsQueryable();
            sums = FilterSummariesByDate(sums, startDate, endDate);

            if (keywId.HasValue)
            {
                sums = sums.Where(s => s.Keyword.Id == keywId.Value);
            }
            if (acctId.HasValue)
            {
                sums = sums.Where(s => s.Keyword.AccountId == acctId.Value);
            }
            if (adsetId.HasValue)
            {
                sums = sums.Where(s => s.Keyword.AdSetId == adsetId.Value);
            }
            if (stratId.HasValue)
            {
                sums = sums.Where(s => s.Keyword.StrategyId == stratId.Value);
            }
            if (platformId.HasValue)
            {
                sums = sums.Where(ds => ds.Keyword.ExtAccount.PlatformId == platformId.Value);
            }
            if (campId.HasValue)
            {
                sums = sums.Where(ds => ds.Keyword.ExtAccount.CampaignId == campId.Value);
            }
            if (advId.HasValue)
            {
                sums = sums.Where(ds => ds.Keyword.ExtAccount.Campaign.AdvertiserId == advId.Value);
            }

            return sums;
        }

        public IQueryable<SearchTermSummary> SearchTermSummaries(DateTime? startDate, DateTime? endDate, int? stermId = null, int? keywId = null, int? acctId = null, int? platformId = null, int? campId = null, int? advId = null)
        {
            var sSums = context.SearchTermSummaries.AsQueryable();
            sSums = FilterSummariesByDate(sSums, startDate, endDate);

            if (stermId.HasValue)
            {
                sSums = sSums.Where(s => s.SearchTerm.Id == stermId.Value);
            }
            if (acctId.HasValue)
            {
                sSums = sSums.Where(s => s.SearchTerm.AccountId == acctId.Value);
            }
            if (keywId.HasValue)
            {
                sSums = sSums.Where(s => s.SearchTerm.KeywordId == keywId.Value);
            }
            if (platformId.HasValue)
            {
                sSums = sSums.Where(ds => ds.SearchTerm.ExtAccount.PlatformId == platformId.Value);
            }
            if (campId.HasValue)
            {
                sSums = sSums.Where(ds => ds.SearchTerm.ExtAccount.CampaignId == campId.Value);
            }
            if (advId.HasValue)
            {
                sSums = sSums.Where(ds => ds.SearchTerm.ExtAccount.Campaign.AdvertiserId == advId.Value);
            }

            return sSums;
        }

        private IQueryable<T> FilterSummariesByDate<T>(IQueryable<T> dSums, DateTime? startDate, DateTime? endDate)
            where T : DatedStatsSummary
        {
            if (startDate.HasValue)
            {
                dSums = dSums.Where(ds => ds.Date >= startDate.Value);
            }
            if (endDate.HasValue)
            {
                dSums = dSums.Where(ds => ds.Date <= endDate.Value);
            }
            return dSums;
        }

        public IQueryable<Conv> Convs(DateTime? startDate, DateTime? endDate, int? acctId = null, int? platformId = null, int? campId = null, int? advId = null)
        {
            var convs = context.Convs.AsQueryable();
            if (startDate.HasValue)
            {
                convs = convs.Where(s => s.Time >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                var date = endDate.Value.AddDays(1);
                convs = convs.Where(s => s.Time < date);
                // Include up to 11:59:59.999... on the endDate specified
            }
            if (acctId.HasValue)
            {
                convs = convs.Where(s => s.AccountId == acctId.Value);
            }

            if (platformId.HasValue)
            {
                convs = convs.Where(s => s.ExtAccount.PlatformId == platformId.Value);
            }

            if (campId.HasValue)
            {
                convs = convs.Where(s => s.ExtAccount.CampaignId == campId.Value);
            }

            if (advId.HasValue)
            {
                convs = convs.Where(s => s.ExtAccount.Campaign.AdvertiserId == advId.Value);
            }

            return convs;
        }

        public IQueryable<StrategyAction> StrategyActions(DateTime? startDate, DateTime? endDate, int? stratId = null, int? acctId = null, int? platformId = null, int? campId = null, int? advId = null)
        {
            var sActions = context.StrategyActions.AsQueryable();
            if (startDate.HasValue)
            {
                sActions = sActions.Where(x => x.Date >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                sActions = sActions.Where(x => x.Date <= endDate.Value);
            }

            if (stratId.HasValue)
            {
                sActions = sActions.Where(x => x.StrategyId == stratId.Value);
            }

            if (acctId.HasValue)
            {
                sActions = sActions.Where(x => x.Strategy.AccountId == acctId.Value);
            }

            if (platformId.HasValue)
            {
                sActions = sActions.Where(x => x.Strategy.ExtAccount.PlatformId == platformId.Value);
            }

            if (campId.HasValue)
            {
                sActions = sActions.Where(x => x.Strategy.ExtAccount.CampaignId == campId.Value);
            }

            if (advId.HasValue)
            {
                sActions = sActions.Where(x => x.Strategy.ExtAccount.Campaign.AdvertiserId == advId.Value);
            }

            return sActions;
        }

        public IQueryable<AdSetAction> AdSetActions(DateTime? startDate, DateTime? endDate, int? adsetId = null, int? stratId = null, int? acctId = null, int? platformId = null, int? campId = null, int? advId = null)
        {
            var actions = context.AdSetActions.AsQueryable();
            if (startDate.HasValue)
            {
                actions = actions.Where(x => x.Date >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                actions = actions.Where(x => x.Date <= endDate.Value);
            }

            if (adsetId.HasValue)
            {
                actions = actions.Where(x => x.AdSetId == adsetId.Value);
            }

            if (stratId.HasValue)
            {
                actions = actions.Where(x => x.AdSet.StrategyId == stratId.Value);
            }

            if (acctId.HasValue)
            {
                actions = actions.Where(x => x.AdSet.AccountId == acctId.Value);
            }

            if (platformId.HasValue)
            {
                actions = actions.Where(x => x.AdSet.ExtAccount.PlatformId == platformId.Value);
            }

            if (campId.HasValue)
            {
                actions = actions.Where(x => x.AdSet.ExtAccount.CampaignId == campId.Value);
            }

            if (advId.HasValue)
            {
                actions = actions.Where(x => x.AdSet.ExtAccount.Campaign.AdvertiserId == advId.Value);
            }

            return actions;
        }

        public void DeleteDailySummaries(IQueryable<DailySummary> sums)
        {
            sums.Delete();
            context.SaveChanges();
        }
        public void DeleteStrategySummaries(IQueryable<StrategySummary> sums)
        {
            sums.Delete();
            context.SaveChanges();
        }
        public void DeleteAdSetSummaries(IQueryable<AdSetSummary> sums)
        {
            sums.Delete();
            context.SaveChanges();
        }
        public void DeleteTDadSummaries(IQueryable<TDadSummary> sums)
        {
            sums.Delete();
            context.SaveChanges();
        }
        public void DeleteAdSetActionStats(IQueryable<AdSetAction> actionStats)
        {
            actionStats.Delete();
            context.SaveChanges();
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
            var accountId = extAccount?.Id;
            var dSums = DailySummaries(startDate, endDate, accountId);
            var sumList = dSums.ToList();
            sumList.ForEach(x => x.InitialMetrics = x.Metrics);
            var stat = new TDRawStat(sumList)
            {
                ExtAccount = extAccount
            };
            return stat;
        }

        //TODO: by campaignId, etc
        public IEnumerable<TDRawStat> GetStrategyStats(DateTime? startDate, DateTime? endDate, int? acctId = null)
        {
            var sSums = StrategySummaries(startDate, endDate, acctId: acctId);
            var sumList = sSums.ToList();
            sumList.ForEach(x => x.InitialMetrics = x.Metrics);
            var sGroups = sumList.GroupBy(s => s.Strategy);
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
        public IEnumerable<TDRawStat> GetAdSetStats(DateTime? startDate, DateTime? endDate, int? acctId = null, int? stratId = null)
        {
            var sums = AdSetSummaries(startDate, endDate, acctId: acctId, stratId: stratId);
            var sumList = sums.ToList();
            sumList.ForEach(x => x.InitialMetrics = x.Metrics);
            var groups = sumList.GroupBy(s => s.AdSet);
            var stats = new List<TDRawStat>();
            foreach (var group in groups)
            {
                var stat = new TDRawStat(group)
                {
                    AdSet = group.Key
                };
                stats.Add(stat);
            }
            return stats;
        }

        //TODO: by campaignId, etc
        public IEnumerable<TDRawStat> GetTDadStats(DateTime? startDate, DateTime? endDate, int? acctId = null, int? adSetId = null)
        {
            var sums = TDadSummaries(startDate, endDate, acctId: acctId, adSetId: adSetId);
            var sumList = sums.ToList();
            sumList.ForEach(x => x.InitialMetrics = x.Metrics);
            var groups = sumList.GroupBy(s => s.TDad);
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

        public IEnumerable<TDRawStat> GetKeywordStats(DateTime? startDate, DateTime? endDate, int? acctId = null, int? stratId = null, int? adSetId = null)
        {
            var sums = KeywordSummaries(startDate, endDate, acctId: acctId, stratId: stratId, adsetId: adSetId);
            var sumList = sums.ToList();
            sumList.ForEach(x => x.InitialMetrics = x.Metrics);
            var groups = sumList.GroupBy(s => s.Keyword);
            var stats = new List<TDRawStat>();
            foreach (var group in groups)
            {
                var stat = new TDRawStat(group)
                {
                    Keyword = group.Key
                };
                stats.Add(stat);
            }
            return stats;
        }

        public IEnumerable<TDRawStat> GetSearchTermStats(DateTime? startDate, DateTime? endDate, int? acctId = null, int? keywId = null)
        {
            var sums = SearchTermSummaries(startDate, endDate, acctId: acctId, keywId: keywId);
            var sumList = sums.ToList();
            sumList.ForEach(x => x.InitialMetrics = x.Metrics);
            var groups = sumList.GroupBy(s => s.SearchTerm);
            var stats = new List<TDRawStat>();
            foreach (var group in groups)
            {
                var stat = new TDRawStat(group)
                {
                    SearchTerm = group.Key
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
            {
                sums = sums.Where(s => s.Impressions >= minImpressions.Value);
            }

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

        //public IEnumerable<TDRawStat> GetStrategyActionStats(DateTime? startDate, DateTime? endDate, int? acctId = null, int? stratId = null)
        //{
        //    var actions = StrategyActions(startDate, endDate, acctId: acctId, stratId: stratId);
        //    return GetActionStats(actions);
        //}
        public IEnumerable<TDRawStat> GetAdSetActionStats(DateTime? startDate, DateTime? endDate, int? acctId = null, int? stratId = null, int? adsetId = null)
        {
            var actions = AdSetActions(startDate, endDate, acctId: acctId, stratId: stratId, adsetId: adsetId);
            return GetActionStats(actions);
        }
        private IEnumerable<TDRawStat> GetActionStats(IQueryable<ActionStatsWithVals> actionStats)
        {
            var groups = actionStats.GroupBy(x => x.ActionType);
            var stats = new List<TDRawStat>();
            foreach (var group in groups)
            {
                var stat = new TDRawStat(group)
                {
                    ActionType = group.Key
                };
                stats.Add(stat);
            }
            return stats;
        }

        public TDCampStats GetCampStats(DateTime monthStart, int campId)
        {
            var campaign = Campaign(campId);
            if (campaign == null)
            {
                return null; // ?new TDCampStats - blank?
            }

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
            {
                return statList;
            }

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
            {
                items = items.Where(i => i.Date >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                items = items.Where(i => i.Date <= endDate.Value);
            }

            if (campId.HasValue)
            {
                items = items.Where(i => i.CampaignId == campId.Value);
            }

            return items;
        }

        public bool AddExtraItem(ExtraItem item)
        {
            if (context.ExtraItems.Any(i => i.Id == item.Id))
            {
                return false;
            }

            context.ExtraItems.Add(item);
            context.SaveChanges();
            return true;
        }
        public bool DeleteExtraItem(int id)
        {
            var item = context.ExtraItems.Find(id);
            if (item == null)
            {
                return false;
            }

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
            {
                item.Campaign = context.Campaigns.Find(item.CampaignId);
            }
        }
    }
}
