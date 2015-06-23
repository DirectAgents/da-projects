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

        public ActionResult Month(string source)
        {
            var statsDict = GetStats(mainRepo, source);
            var json = Json(statsDict, JsonRequestBehavior.AllowGet);
            return json;
        }

        public ActionResult MTD(string source)
        {
            var currMonthRepo = CreateMainRepository();
            if (currMonthRepo == null) return null;

            var statsDict = GetStats(currMonthRepo, source);
            var json = Json(statsDict, JsonRequestBehavior.AllowGet);
            return json.ToJsonp();
        }

        // TODO: make a simpler repo method - e.g. that's only USD and doesn't group by... aff, accounting status, item ids, etc ?
        //       or, just retrieve items and group here ?
        protected static Dictionary<string, EOMStat> GetStats(IMainRepository mainRepo, string source)
        {
            var campAffItems = mainRepo.CampAffItems(false, null, null, Source.ToSourceId(source));
            var unitTypeGroups = campAffItems.GroupBy(ca => new { ca.UnitTypeId, ca.UnitTypeName });
            var unitTypeStats = unitTypeGroups.Select(g => new EOMStat
            {
                UnitType = g.Key.UnitTypeName,
                RevUSD = g.Sum(ca => ca.RevUSD),
                CostUSD = g.Sum(ca => ca.CostUSD)
            });

            var statsDict = unitTypeStats.OrderBy(u => u.UnitType).ToDictionary<EOMStat, string>(i => i.UnitType.Replace(" ", ""));
            statsDict["Total"] = new EOMStat
            {
                UnitType = "Total",
                RevUSD = campAffItems.Sum(ca => ca.RevUSD),
                CostUSD = campAffItems.Sum(ca => ca.CostUSD)
            };
            return statsDict;
        }
    }
}