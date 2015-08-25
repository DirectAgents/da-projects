using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Entities.AdRoll;
using DirectAgents.Domain.Entities.TD;
using DirectAgents.Web.Areas.TD.Models;

namespace DirectAgents.Web.Areas.TD.Controllers
{
    public class StatsController : DirectAgents.Web.Controllers.ControllerBase
    {
        public StatsController(ITDRepository tdRepository)
        {
            this.tdRepo = tdRepository;
        }

        public ActionResult Account(string platformCode)
        {
            var today = DateTime.Today;
            var startOfMonth = new DateTime(today.Year, today.Month, 1);
            var stats = new List<TDStatWithAccount>();

            var accounts = tdRepo.Accounts(platformCode: platformCode)
                            .OrderBy(a => a.Platform.Code).ThenBy(a => a.Name);
            foreach (var account in accounts)
            {   //Note: Multiple Active Record Sets used here
                var stat = tdRepo.GetTDStatWithAccount(startOfMonth, null, account: account); // MTD
                if (!stat.AllZeros())
                    stats.Add(stat);
            }
            return View(stats);
        }

        public ActionResult AdRoll(string advEid)
        {
            var today = DateTime.Today;
            var startOfMonth = new DateTime(today.Year, today.Month, 1);
            var stats = new List<AdRollStat>();

            if (!string.IsNullOrWhiteSpace(advEid))
            {
                var ads = tdRepo.AdRoll_Ads(advEid: advEid);
                foreach (var ad in ads)
                {   //Note: Multiple Active Record Sets used here
                    var stat = tdRepo.GetAdRollStat(ad, startOfMonth, null); // MTD
                    if (!stat.AllZeros())
                        stats.Add(stat);
                }
            }
            var model = stats.OrderBy(a => a.Name).ToList();
            return View(model);
        }

        public ActionResult DBM(int ioID)
        {
            var today = DateTime.Today;
            var startOfMonth = new DateTime(today.Year, today.Month, 1);
            var stats = new List<TDStat>();

            var insertionOrder = tdRepo.InsertionOrder(ioID);
            var creatives = tdRepo.DBM_Creatives(ioID);
            foreach (var creative in creatives)
            {   //Note: Multiple Active Record Sets used here
                var stat = tdRepo.GetDBMStat(creative, startOfMonth, null);
                if (!stat.AllZeros())
                    stats.Add(stat);
            }
            var model = new TDStatsVM
            {
                Name = "MTD Stats - " + insertionOrder.Name + "(DBM)",
                Stats = stats.OrderBy(a => a.Name).ToList()
            };
            return View("Generic", model);
        }
	}
}