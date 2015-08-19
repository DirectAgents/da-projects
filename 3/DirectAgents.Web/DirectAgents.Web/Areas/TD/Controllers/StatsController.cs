using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Entities.TD;

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
            var stats = new List<TDStat>();

            var accounts = tdRepo.Accounts(platformCode: platformCode)
                            .OrderBy(a => a.Platform.Code).ThenBy(a => a.Name);
            foreach (var account in accounts)
            {   //Note: Multiple Active Record Sets used here
                var stat = tdRepo.GetTDStat(startOfMonth, null, account: account); // MTD
                stats.Add(stat);
            }
            return View(stats);
        }
	}
}