using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DirectAgents.Domain.Concrete;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities;
using DirectAgents.Domain.Entities.Screen;
using DirectAgents.Web.Models;

namespace DirectAgents.Web.Controllers
{
    public class ScreensController : ControllerBase
    {
        public ScreensController()
        {
            this.mainRepo = new MainRepository(new DAContext());
            //this.securityRepo = new SecurityRepository();
        }

        public ActionResult SalesStatsJson(int numweeks = 4)
        {
            var minDate = MinDateFromNumWeeks(numweeks);
            var stats = mainRepo.SalespersonStats(minDate).ToList();
            stats = stats.Select(s => new SalespersonStat(s)).ToList();

            var currWeekStart = MostRecentSunday().AddDays(-7);
            var currWeekEnd = currWeekStart.AddDays(6);

            var salespeople = mainRepo.Salespeople().ToList();
            foreach (var salesperson in salespeople)
            {
                salesperson.CurrentStat = StatOrBlank(stats, salesperson.Id, currWeekStart);
                var salespersonStats = new List<SalespersonStat>();
                if (numweeks >= 1)
                {
                    for (var iDate = currWeekStart.AddDays(-7 * (numweeks - 1)); iDate <= currWeekStart; iDate = iDate.AddDays(7))
                    {
                        salespersonStats.Add(StatOrBlank(stats, salesperson.Id, iDate));
                    }
                    salesperson.Stats = salespersonStats;
                }
            }
            var statsObj = new SalesStatsObj
            {
                WeekText = String.Format("Week of {0:M/dd} - {1:M/dd}", currWeekStart, currWeekEnd),
                Salespeople = salespeople.OrderByDescending(s => s.CurrentStat.EmailReplied).ToList()
            };
            var json = Json(statsObj, JsonRequestBehavior.AllowGet);
            return json.ToJsonp();
        }
        private SalespersonStat StatOrBlank(List<SalespersonStat> stats, int salespersonId, DateTime weekStart)
        {
            var stat = stats.Where(s => s.SalespersonId == salespersonId && s.Date == weekStart).FirstOrDefault()
                        ?? SalespersonStat.BlankStat(salespersonId, weekStart);
            return stat;
        }

        public ActionResult SalesStats(int numweeks = 4)
        {
            var minDate = MinDateFromNumWeeks(numweeks);
            var stats = mainRepo.SalespersonStats(minDate);
            return View(stats);
        }
        private DateTime? MinDateFromNumWeeks(int numweeks)
        {
            if (numweeks < 0)
                return null;

            var minDate = MostRecentSunday();
            minDate = minDate.AddDays(-7 * numweeks);
            return minDate;
        }

        [HttpGet]
        public ActionResult EditSalesStats(DateTime? date)
        {
            SalespeopleStatsVM model;
            if (date.HasValue)
            {
                var stats = mainRepo.SalespersonStats(null, date).OrderBy(s => s.Salesperson.FirstName).ThenBy(s => s.Salesperson.LastName);
                model = new SalespeopleStatsVM
                {
                    Stats = stats.ToList(),
                    Date = date.Value
                };
            }
            else
            {
                var salespeople = mainRepo.Salespeople();
                var stats = salespeople.OrderBy(s => s.FirstName).ThenBy(s => s.LastName).ToList().Select(s => new SalespersonStat
                {
                    Salesperson = s,
                    SalespersonId = s.Id
                }).ToList();
                model = new SalespeopleStatsVM
                {
                    Stats = stats,
                    Date = DateForNextStats()
                };
            }
            return View(model);
        }
        private DateTime DateForNextStats()
        {
            DateTime nextStatsDate;
            var stats = mainRepo.SalespersonStats();
            if (stats.Any())
            {
                nextStatsDate = stats.Max(s => s.Date).AddDays(7);
            }
            else
            {
                nextStatsDate = MostRecentSunday();
            }
            return nextStatsDate;
        }
        private DateTime MostRecentSunday()
        {
            var date = DateTime.Today;
            while (date.DayOfWeek != DayOfWeek.Sunday)
                date = date.AddDays(-1);
            return date;
        }

        [HttpPost]
        public ActionResult EditSalesStats(List<SalespersonStat> stats, DateTime date)
        {
            foreach (var stat in stats)
            {
                stat.Date = date;
                mainRepo.SaveSalespersonStat(stat);
            }
            mainRepo.SaveChanges();

            return RedirectToAction("SalesStats");
        }

        public ActionResult DeleteSalesStats(DateTime date)
        {
            mainRepo.DeleteSalespersonStats(date);
            mainRepo.SaveChanges();
            return RedirectToAction("SalesStats");
        }

        // --- Variables ---

        public ActionResult Variables()
        {
            var variables = mainRepo.GetVariables();
            return View(variables);
        }

        [HttpGet]
        public ActionResult Variable(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
                return Content("No variable name supplied.");

            var variable = mainRepo.GetVariable(name);
            if (variable == null)
            {
                variable = new Variable() { Name = name };
                mainRepo.SaveVariable(variable);
            }
            return View(variable);
        }
        [HttpPost]
        public ActionResult Variable(Variable variable)
        {
            mainRepo.SaveVariable(variable);
            return Content("Saved");
        }

        public ActionResult NewClients(string area)
        {
            var newClientsVar = mainRepo.GetVariable("newClients_" + area);
            string[] newClients = new string[] { };
            if (newClientsVar != null && newClientsVar.StringVal != null)
            {
                newClients = newClientsVar.StringVal.Split(new char[] { '|' });
            }
            var json = Json(newClients, JsonRequestBehavior.AllowGet);
            return json.ToJsonp();
        }
    }

    public class SalesStatsObj
    {
        public string WeekText { get; set; }
        public List<Salesperson> Salespeople { get; set; }
    }
}
