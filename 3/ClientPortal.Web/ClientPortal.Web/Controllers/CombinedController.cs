using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using CakeExtracter.Common;
using ClientPortal.Data.Contracts;
using ClientPortal.Data.DTOs;
using ClientPortal.Web.Models;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Entities.CPProg;

namespace ClientPortal.Web.Controllers
{
    public class CombinedController : CPController
    {
        public CombinedController(IClientPortalRepository cpRepository, ICPProgRepository progRepository)
        {
            this.cpRepo = cpRepository;
            this.progRepo = progRepository;
        }

        public FileResult Logo()
        {
            var userInfo = GetUserInfo();
            if (userInfo.ClientLogo == null)
                return null;
            WebImage logo = new WebImage(userInfo.ClientLogo);
            return File(logo.GetBytes(), "image/" + logo.ImageFormat, logo.FileName);
        }

        public ActionResult Index() // Combined Dashboard
        {
            var userInfo = GetUserInfo();
            if (!userInfo.HasSearch || !userInfo.HasProgrammatic())
                return RedirectToAction("Go", "Home");

            var today = DateTime.Today;
            var yesterday = today.AddDays(-1);
            var monthStart = new DateTime(yesterday.Year, yesterday.Month, 1);
            var weekStart = yesterday;
            while (weekStart.DayOfWeek != DayOfWeek.Monday) //TODO: use client's start-day-of-week
            {
                weekStart = weekStart.AddDays(-1);
            }
            // Get search stats
            var mtdSearch = cpRepo.GetSearchStats(userInfo.SearchProfile, monthStart, yesterday, false);
            var wtdSearch = cpRepo.GetSearchStats(userInfo.SearchProfile, weekStart, yesterday, false);

            // Get programmatic stats
            int progAdvId = userInfo.ProgAdvertiser.Id;
            var mtdProg = progRepo.MTDBasicStat(progAdvId, yesterday);
            var wtdProg = progRepo.DateRangeBasicStat(progAdvId, weekStart, yesterday);

            // Combine them
            var mtdStat = new StatVM(monthStart, yesterday);
            mtdStat.Add(mtdSearch);
            mtdStat.Add(mtdProg);

            var wtdStat = new StatVM(weekStart, yesterday);
            wtdStat.Add(wtdSearch);
            wtdStat.Add(wtdProg);

            var model = new CombinedVM
            {
                UserInfo = userInfo,
                MTDStat = mtdStat,
                WTDStat = wtdStat
            };
            ViewBag.EndDate = yesterday.ToString("yyyy-MM-dd");
            ViewBag.StartDate = today.AddYears(-1).ToString("yyyy-MM-dd");
            return View(model);
        }

        //NEXT: weekly & monthly
        public JsonResult DailyStats()
        {
            var today = DateTime.Today;
            var oneYearAgo = today.AddYears(-1);
            var yesterday = today.AddDays(-1);
            return IntervalStats(oneYearAgo, yesterday);
        }

        public JsonResult StatsTemp(string interval, string daterange, DateTime? start, DateTime? end)
        {
            var dates = DateRangeFromString(daterange, start, end);
            return IntervalStats(dates.FromDate, dates.ToDate, interval);
        }

        public JsonResult PieStats(string daterange, DateTime? start, DateTime? end)
        {
            var dates = DateRangeFromString(daterange, start, end);
            var statList = new List<StatVM>();

            var userInfo = GetUserInfo();
            if (userInfo.HasSearch)
            {
                var searchStat = cpRepo.GetSearchStats(userInfo.SearchProfile, dates.FromDate, dates.ToDate, null);
                var statVM = new StatVM(searchStat) { Name = "Paid Search" };
                statList.Add(statVM);
            }
            if (userInfo.HasProgrammatic())
            {
                var socialAccountIds = progRepo.ExtAccountIds_Social(advId: userInfo.ProgAdvertiser.Id);

                var progStat = progRepo.DateRangeBasicStat(userInfo.ProgAdvertiser.Id, dates.FromDate, dates.ToDate, excludeAccountIds: socialAccountIds);
                var progStatVM = new StatVM(progStat) { Name = "Programmatic" };
                statList.Add(progStatVM);

                var socialStat = progRepo.DateRangeBasicStat(userInfo.ProgAdvertiser.Id, dates.FromDate, dates.ToDate, includeAccountIds: socialAccountIds);
                var socialStatVM = new StatVM(socialStat) { Name = "Social" };
                statList.Add(socialStatVM);
            }
            return CreateJsonResult(statList, computeAggregates: false);
        }

        private DateRange DateRangeFromString(string range, DateTime? startdate, DateTime? enddate)
        {
            var today = DateTime.Today;
            var yesterday = today.AddDays(-1);
            var end = enddate ?? yesterday;
            DateTime start;
            if (startdate == null)
            {
                switch (range)
                {
                    case "mtd":
                        start = new DateTime(yesterday.Year, yesterday.Month, 1);
                        break;
                    case "ytd":
                        start = new DateTime(yesterday.Year, 1, 1);
                        break;
                    default: // since one year ago
                        start = today.AddYears(-1);
                        break;
                }
            }
            else
            {
                start = startdate.Value;
            }
            var dateRange = new DateRange(start, end);
            return dateRange;
        }

        public JsonResult IntervalStats(DateTime? start, DateTime? end, string interval = "daily")
        {
            var userInfo = GetUserInfo();
            IEnumerable<SearchStat> searchStats = new List<SearchStat>();
            if (userInfo.HasSearch)
            {
                if (interval == "monthly")
                    searchStats = cpRepo.GetMonthStats(userInfo.SearchProfile, null, start, end);
                else if (interval == "weekly")
                    searchStats = cpRepo.GetWeekStats(userInfo.SearchProfile, null, start, end);
                else
                    searchStats = cpRepo.GetDailyStats(userInfo.SearchProfile, start, end);
            }
            IEnumerable<BasicStat> progStats = new List<BasicStat>();
            if (userInfo.HasProgrammatic())
            {
                if (interval == "monthly")
                    progStats = progRepo.MonthlyBasicStats(userInfo.ProgAdvertiser.Id, start, end, computeCalculatedStats: false);
                else if (interval == "weekly")
                    progStats = progRepo.WeeklyBasicStats(userInfo.ProgAdvertiser.Id, start, end, computeCalculatedStats: false);
                else
                    progStats = progRepo.DailyBasicStats(userInfo.ProgAdvertiser.Id, start, end, computeCalculatedStats: false);
            }
            var stats = CombinedStats(searchStats, progStats);
            return CreateJsonResult(stats);
        }

        private IEnumerable<StatVM> CombinedStats(IEnumerable<SearchStat> searchStats, IEnumerable<BasicStat> progStats)
        {
            var searchEnumerator = searchStats.GetEnumerator();
            var progEnumerator = progStats.GetEnumerator();

            SearchStat searchStat = null;
            if (searchEnumerator.MoveNext())
                searchStat = searchEnumerator.Current;
            BasicStat progStat = null;
            if (progEnumerator.MoveNext())
                progStat = progEnumerator.Current;

            while (searchStat != null || progStat != null)
            {
                DateTime? searchDate = (searchStat != null) ? searchStat.StartDate : (DateTime?)null;
                DateTime? progDate = (progStat != null) ? progStat.Date : (DateTime?)null;
                var statVM = new StatVM(DateTime.Today);
                if (progDate == null || (searchDate.HasValue && searchDate.Value <= progDate.Value))
                {
                    // use the searchStat
                    statVM.Date = searchStat.StartDate;
                    statVM.Add(searchStat);
                    if (searchEnumerator.MoveNext())
                        searchStat = searchEnumerator.Current;
                    else
                        searchStat = null;
                }
                if (searchDate == null || (progDate.HasValue && progDate.Value <= searchDate.Value))
                {
                    // use the progStat
                    statVM.Date = progStat.Date;
                    statVM.Add(progStat);
                    if (progEnumerator.MoveNext())
                        progStat = progEnumerator.Current;
                    else
                        progStat = null;
                }
                yield return statVM;
            }
        }

        public JsonResult DailyStatsTest()
        {
            var userInfo = GetUserInfo();
            var yesterday = DateTime.Today.AddDays(-1);
            var start = new DateTime(yesterday.Year, yesterday.Month, 1);

            IEnumerable<SearchStat> searchDailyStats = new List<SearchStat>();
            if (userInfo.HasSearch)
                searchDailyStats = cpRepo.GetDailyStats(userInfo.SearchProfile, start, yesterday);
            IEnumerable<BasicStat> progDailyStats = new List<BasicStat>();
            if (userInfo.HasProgrammatic())
                progDailyStats = progRepo.DailyBasicStats(userInfo.ProgAdvertiser.Id, start, yesterday, computeCalculatedStats: false);

            var stats = CombinedStats(searchDailyStats, progDailyStats);
            var totals = new StatVM(start)
            {
                Spend = stats.Sum(s => s.Spend),
                Impressions = stats.Sum(s => s.Impressions),
                Clicks = stats.Sum(s => s.Clicks),
                Convs = stats.Sum(s => s.Convs)
            };
            var json = Json(totals, JsonRequestBehavior.AllowGet); //TODO: don't allow get
            return json;
        }

        private JsonResult CreateJsonResult(IEnumerable<StatVM> stats, bool computeAggregates = true)
        {
            var kg = new KG<StatVM>();
            kg.data = stats;
            kg.total = stats.Count();
            if (computeAggregates)
                kg.aggregates = Aggregates(stats);

            var json = Json(kg);
            return json;
        }
        private object Aggregates(IEnumerable<StatVM> stats)
        {
            if (stats.Count() == 0)
                return null;
            var aggregates = new
            {
                Spend = new { sum = stats.Sum(s => s.Spend) },
                Impressions = new { sum = stats.Sum(s => s.Impressions) },
                Clicks = new { sum = stats.Sum(s => s.Clicks) },
                Convs = new { sum = stats.Sum(s => s.Convs) }
            };
            return aggregates;
        }

    }
}