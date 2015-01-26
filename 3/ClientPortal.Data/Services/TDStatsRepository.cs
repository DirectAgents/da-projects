using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ClientPortal.Data.DTOs.TD;
using ClientPortal.Data.Entities.TD;
using ClientPortal.Data.Entities.TD.AdRoll;
using ClientPortal.Data.Entities.TD.DBM;

namespace ClientPortal.Data.Services
{
    public partial class TDRepository
    {
        //TODO: eliminate this, remove DBMDailySummaries (in ETL code + db context)
        //      (replaced with GetDailyStatsSummaries...)
        private IQueryable<DBMDailySummary> GetDailySummaries(DateTime? start, DateTime? end, int? insertionOrderID)
        {
            var dailySummaries = context.DBMDailySummaries.AsQueryable();

            if (start.HasValue) dailySummaries = dailySummaries.Where(ds => ds.Date >= start.Value);
            if (end.HasValue) dailySummaries = dailySummaries.Where(ds => ds.Date <= end.Value);

            if (insertionOrderID.HasValue) dailySummaries = dailySummaries.Where(ds => ds.InsertionOrderID == insertionOrderID.Value);

            return dailySummaries;
        }

        // --- Daily StatsSummaries (combined)

        public IEnumerable<StatsSummary> GetDailyStatsSummaries(DateTime? start, DateTime? end, TradingDeskAccount tda)
        {
            int? insertionOrderID = tda.InsertionOrderID();
            int? adrollProfileId = tda.AdRollProfileId();
            if (!insertionOrderID.HasValue && !adrollProfileId.HasValue)
                return new List<StatsSummary>();

            IEnumerable<StatsSummary> summaries = null;
            if (insertionOrderID.HasValue)
                summaries = GetDailyStatsSummariesDBM(start, end, insertionOrderID.Value);
            if (adrollProfileId.HasValue)
            {
                var summariesAdRoll = GetDailyStatsSummariesAdRoll(start, end, adrollProfileId.Value);
                if (!insertionOrderID.HasValue)
                    summaries = summariesAdRoll;
                else
                { // combine two sources
                    summaries = summaries.Concat(summariesAdRoll);
                    summaries = summaries.GroupBy(s => s.Date).Select(g =>
                        new StatsSummary
                        {
                            Date = g.Key,
                            Impressions = g.Sum(s => s.Impressions),
                            Clicks = g.Sum(s => s.Clicks),
                            Conversions = g.Sum(s => s.Conversions),
                            Spend = g.Sum(s => s.Spend)
                        }).ToList();
                }
            }

            if (tda.FixedCPM.HasValue || tda.FixedCPC.HasValue)
            {
                summaries = summaries.Select(s =>
                    new StatsSummary
                    {
                        Date = s.Date,
                        Impressions = s.Impressions,
                        Clicks = s.Clicks,
                        Conversions = s.Conversions,
                        Spend = tda.FixedCPM.HasValue ? tda.FixedCPM.Value * s.Impressions / 1000 : tda.FixedCPC.Value * s.Clicks
                    });
            }
            else if (tda.SpendMultiplier.HasValue)
            {
                summaries = summaries.Select(s =>
                    new StatsSummary
                    {
                        Date = s.Date,
                        Impressions = s.Impressions,
                        Clicks = s.Clicks,
                        Conversions = s.Conversions,
                        Spend = s.Spend * tda.SpendMultiplier.Value
                    });
            }
            return summaries;
        }

        // --- Daily Stats - for each subsystem

        private IEnumerable<StatsSummary> GetDailyStatsSummariesDBM(DateTime? start, DateTime? end, int insertionOrderID)
        {
            var dailySummaries = GetCreativeDailySummaries(start, end, insertionOrderID);
            var statsSummaries = dailySummaries.GroupBy(cds => cds.Date).Select(g =>
                new StatsSummary
                {
                    Date = g.Key,
                    Impressions = g.Sum(s => s.Impressions),
                    Clicks = g.Sum(s => s.Clicks),
                    Conversions = g.Sum(s => s.Conversions),
                    Spend = g.Sum(s => s.Revenue)
                }).ToList();
            return statsSummaries;
        }

        private IEnumerable<StatsSummary> GetDailyStatsSummariesAdRoll(DateTime? start, DateTime? end, int adrollProfileId)
        {
            var dailySummaries = GetAdDailySummaries(start, end, adrollProfileId);
            var summaries = dailySummaries.GroupBy(ads => ads.Date).Select(g =>
                new StatsSummary
                {
                    Date = g.Key,
                    Impressions = g.Sum(s => s.Impressions),
                    Clicks = g.Sum(s => s.Clicks),
                    Conversions = g.Sum(s => s.Conversions),
                    Spend = g.Sum(s => s.Spend)
                }).ToList();
            return summaries;
        }

        // --- Creative Stats - DBM

        private IEnumerable<CreativeStatsSummary> GetCreativeStatsSummariesDBM(DateTime? start, DateTime? end, int insertionOrderID)
        {
            var creativeDailySummaries = GetCreativeDailySummaries(start, end, insertionOrderID);
            var creativeStatsSummaries = creativeDailySummaries.GroupBy(cds => cds.Creative).Select(g =>
                new CreativeStatsSummary
                {
                    CreativeID = g.Key.CreativeID,
                    CreativeName = g.Key.CreativeName,
                    Impressions = g.Sum(c => c.Impressions),
                    Clicks = g.Sum(c => c.Clicks),
                    Conversions = g.Sum(c => c.Conversions),
                    Spend = g.Sum(c => c.Revenue)
                }).ToList();
            return creativeStatsSummaries;
        }

        private IQueryable<CreativeDailySummary> GetCreativeDailySummaries(DateTime? start, DateTime? end, int? insertionOrderID)
        {
            var cds = context.CreativeDailySummaries.AsQueryable();
            if (start.HasValue)
                cds = cds.Where(s => s.Date >= start.Value);
            if (end.HasValue)
                cds = cds.Where(s => s.Date <= end.Value);
            if (insertionOrderID.HasValue)
                cds = cds.Where(s => s.Creative.InsertionOrderID == insertionOrderID.Value);
            return cds;
        }

        // --- Creative Stats - AdRoll

        private IEnumerable<CreativeStatsSummary> GetCreativeStatsSummariesAdRoll(DateTime? start, DateTime? end, int adrollProfileId)
        {
            var dailySummaries = GetAdDailySummaries(start, end, adrollProfileId);
            var summaries = dailySummaries.GroupBy(ads => ads.AdRollAd).Select(g =>
                new CreativeStatsSummary
                {
                    CreativeID = g.Key.Id,
                    CreativeName = g.Key.Name,
                    Impressions = g.Sum(s => s.Impressions),
                    Clicks = g.Sum(s => s.Clicks),
                    Conversions = g.Sum(s => s.Conversions),
                    Spend = g.Sum(s => s.Spend)
                }).ToList();
            return summaries;
        }

        private IQueryable<AdDailySummary> GetAdDailySummaries(DateTime? start, DateTime? end, int? adrollProfileId)
        {
            var ads = context.AdDailySummaries.AsQueryable();
            if (start.HasValue)
                ads = ads.Where(s => s.Date >= start.Value);
            if (end.HasValue)
                ads = ads.Where(s => s.Date <= end.Value);
            if (adrollProfileId.HasValue)
                ads = ads.Where(s => s.AdRollAd.AdRollProfileId == adrollProfileId.Value);
            return ads;
        }

        // --- Creative Stats - combined

        public IEnumerable<CreativeStatsSummary> GetCreativeStatsSummaries(DateTime? start, DateTime? end, TradingDeskAccount tda)
        {
            //TODO: find matching creatives from the two sources and combine their stats
            int? insertionOrderID = tda.InsertionOrderID();
            int? adrollProfileId = tda.AdRollProfileId();
            IEnumerable<CreativeStatsSummary> summaries;

            if (insertionOrderID.HasValue)
                summaries = GetCreativeStatsSummariesDBM(start, end, insertionOrderID.Value);
            else
                summaries = new List<CreativeStatsSummary>();
            if (adrollProfileId.HasValue)
                summaries = summaries.Concat(GetCreativeStatsSummariesAdRoll(start, end, adrollProfileId.Value));

            if (tda.FixedCPM.HasValue || tda.FixedCPC.HasValue)
            {
                summaries = summaries.Select(c =>
                    new CreativeStatsSummary
                    {
                        CreativeID = c.CreativeID,
                        CreativeName = c.CreativeName,
                        Impressions = c.Impressions,
                        Clicks = c.Clicks,
                        Conversions = c.Conversions,
                        Spend = tda.FixedCPM.HasValue ? tda.FixedCPM.Value * c.Impressions / 1000 : tda.FixedCPC.Value * c.Clicks
                    });
            }
            else if (tda.SpendMultiplier.HasValue)
            {
                summaries = summaries.Select(c =>
                    new CreativeStatsSummary
                    {
                        CreativeID = c.CreativeID,
                        CreativeName = c.CreativeName,
                        Impressions = c.Impressions,
                        Clicks = c.Clicks,
                        Conversions = c.Conversions,
                        Spend = c.Spend * tda.SpendMultiplier.Value
                    });
            }
            return summaries;
        }

        // --- Weekly/Monthly Stats

        public IEnumerable<RangeStat> GetWeekStats(TradingDeskAccount tda, int numWeeks, DateTime? endDate)
        {
            DayOfWeek startDayOfWeek = DayOfWeek.Monday; //TODO: get from TDA

            if (!endDate.HasValue)
            {
                // Start with yesterday and go back until it's the end of the latest full week
                endDate = DateTime.Today.AddDays(-1);
                while (endDate.Value.AddDays(1).DayOfWeek != startDayOfWeek)
                    endDate = endDate.Value.AddDays(-1);
            }

            // Go back X weeks from endDate, then forward 1 day, so it's the right number of weeks, inclusive of start and endDate
            DateTime startDate = endDate.Value.AddDays(-7 * numWeeks + 1);

            // Now move start date back to the closest startDayOfWeek (there may be a partial week now, at the end)
            // (Will only apply if endDate was set to a day other than at the end of the week)
            while (startDate.DayOfWeek != startDayOfWeek)
                startDate = startDate.AddDays(-1);

            var daySums = GetDailyStatsSummaries(startDate, endDate, tda);

            // title ?

            var adjuster = new YearWeekAdjuster
            {
                StartDayOfWeek = startDayOfWeek,
                CalendarWeekRule = CalendarWeekRule.FirstFullWeek
            };

            var stats = daySums
                    .Select(s => new
                    {
                        Date = s.Date,
                        Year = adjuster.GetYearAdjustedByWeek(s.Date),
                        Week = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(s.Date, adjuster.CalendarWeekRule, startDayOfWeek),
                        Impressions = s.Impressions,
                        Clicks = s.Clicks,
                        Conversions = s.Conversions,
                        Spend = s.Spend
                    })
                    .GroupBy(x => new { x.Year, x.Week })
                    .OrderBy(g => g.Key.Year)
                    .ThenBy(g => g.Key.Week)
                    .Select((g, i) => new RangeStat
                    {
                        WeekStartDay = startDayOfWeek,
                        FillToLatest = endDate, // Acts as a boolean except for the most recent week
                        WeekByMaxDate = g.Max(s => s.Date), // Supply the latest date in the group of dates (which are all part of a week)
                        //TitleIfNotNull = title, // (if null, Title is "Range")
                        Impressions = g.Sum(s => s.Impressions),
                        Clicks = g.Sum(s => s.Clicks),
                        Conversions = g.Sum(s => s.Conversions),
                        Spend = g.Sum(s => s.Spend)
                    });
            return stats;
        }

        // ---

        public StatsRollup AdRollStatsRollup(int profileId)
        {
            var adDailySummaries = context.AdDailySummaries.Where(s => s.AdRollAd.AdRollProfileId == profileId);
            var statsRollup = new StatsRollup();
            if (adDailySummaries.Any())
            {
                statsRollup.MinDate = adDailySummaries.Min(s => s.Date);
                statsRollup.MaxDate = adDailySummaries.Max(s => s.Date);
                statsRollup.NumCreatives = adDailySummaries.Select(s => s.AdRollAdId).Distinct().Count();
            }
            return statsRollup;
        }
    }
}
