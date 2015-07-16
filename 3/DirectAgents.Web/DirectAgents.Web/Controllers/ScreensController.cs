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

        //public ActionResult SalespersonStat(int id, DateTime date)
        //{
        //    var stat = mainRepo.GetSalespersonStat(id, date);
        //    return null;
        //}

        public ActionResult SalesStats(int numweeks = 2)
        {
            var stats = mainRepo.SalespersonStats();
            if (numweeks > 0)
            {
                var minDate = DateTime.Now.AddDays(-7 * numweeks);
                stats = stats.Where(s => s.Date >= minDate);
            }
            return View(stats);
        }

        [HttpGet]
        public ActionResult EditSalesStats(DateTime? date)
        {
            SalespeopleStatsVM model;
            if (date.HasValue)
            {
                var stats = mainRepo.SalespersonStats(null, date);
                model = new SalespeopleStatsVM
                {
                    Stats = stats.ToList(),
                    Date = date.Value
                };
            }
            else
            {
                var salespeople = mainRepo.Salespeople();
                var stats = salespeople.ToList().Select(s => new SalespersonStat
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
                nextStatsDate = DateTime.Today;
                while (nextStatsDate.DayOfWeek != DayOfWeek.Sunday)
                    nextStatsDate = nextStatsDate.AddDays(-1);
            }
            return nextStatsDate;
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
}
