using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EomTool.Domain.Abstract;
using EomTool.Domain.DTOs;
using EomTool.Domain.Entities;

namespace EomToolWeb.Controllers
{
    public class StatsController : EOMController
    {
        public StatsController(IMainRepository mainRepository, ISecurityRepository securityRepository, IDAMain1Repository daMain1Repository, IEomEntitiesConfig eomEntitiesConfig)
        {
            this.mainRepo = mainRepository;
            this.securityRepo = securityRepository;
            this.daMain1Repo = daMain1Repository;
            this.eomEntitiesConfig = eomEntitiesConfig;
        }

        public ActionResult Links()
        {
            SetAccountingPeriodViewData();
            return View();
        }

        // Set take=0 for all
        public ActionResult TopAdvertisers(int? unitType, int take = 10, string jsoncallback = null) // period?
        {
            var advStats = mainRepo.EOMStatsByAdvertiser(unitType);
            advStats = advStats.OrderByDescending(a => a.MarginUSD);
            if (take > 0)
                advStats = advStats.Take(take);

            if (!string.IsNullOrWhiteSpace(jsoncallback))
            {
                var json = Json(advStats, JsonRequestBehavior.AllowGet);
                return json.ToJsonp();
            }
            else
            {
                ViewBag.CurrentEomDateString = eomEntitiesConfig.CurrentEomDateString;
                return View(advStats);
            }
        }

        // Overall stats, by unit type...
        public ActionResult Month(string source)
        {
            var statsDict = GetStatsByUnitType(mainRepo, source);
            var json = Json(statsDict, JsonRequestBehavior.AllowGet);
            return json;
        }
        public ActionResult MTD(string source)
        {
            var currMonthRepo = CreateMainRepository();
            Dictionary<string, EOMStat> statsDict;

            if (currMonthRepo == null)
                statsDict = GetBlankStats();
            else
                statsDict = GetStatsByUnitType(currMonthRepo, source);

            var json = Json(statsDict, JsonRequestBehavior.AllowGet);
            return json.ToJsonp();
        }

        // TODO: make a simpler repo method - e.g. that's only USD and doesn't group by... aff, accounting status, item ids, etc ?
        //       or, just retrieve items and group here ?
        protected static Dictionary<string, EOMStat> GetStatsByUnitType(IMainRepository mainRepo, string source)
        {
            var campAffItems = mainRepo.CampAffItems(false, null, null, Source.ToSourceId(source));
            var unitTypeGroups = campAffItems.GroupBy(ca => new { ca.UnitTypeId, ca.UnitTypeName });
            var unitTypeStats = unitTypeGroups.Select(g => new EOMStat
            {
                Name = g.Key.UnitTypeName,
                RevUSD = g.Sum(ca => ca.RevUSD),
                CostUSD = g.Sum(ca => ca.CostUSD)
            });

            var statsDict = unitTypeStats.OrderBy(u => u.Name).ToDictionary<EOMStat, string>(i => i.Name.Replace(" ", ""));
            statsDict["Total"] = new EOMStat
            {
                Name = "Total",
                RevUSD = campAffItems.Sum(ca => ca.RevUSD),
                CostUSD = campAffItems.Sum(ca => ca.CostUSD)
            };
            return statsDict;
        }

        protected static Dictionary<string, EOMStat> GetBlankStats()
        {
            var statsDict = new Dictionary<string, EOMStat>();
            statsDict["Total"] = new EOMStat { Name = "Total" };
            return statsDict;
        }
    }
}