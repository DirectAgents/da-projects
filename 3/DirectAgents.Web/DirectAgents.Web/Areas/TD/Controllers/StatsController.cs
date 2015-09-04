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

        public ActionResult MTD(int campId, DateTime? date)
        {
            var campaign = tdRepo.Campaign(campId);
            if (campaign == null)
                return Content("Campaign not found");

            if (!date.HasValue)
                date = DateTime.Today;
            var startOfMonth = new DateTime(date.Value.Year, date.Value.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            var tdStat = tdRepo.GetTDStat(startOfMonth, endOfMonth, campaign: campaign);
            var budgetWithStats = new BudgetWithStats(campaign.BudgetFor(startOfMonth), tdStat);

            return View(budgetWithStats);
        }

        // Stats by "external account".  Either MTD or for the specified month (indicated by "date")
        public ActionResult Account(string platformCode, int? campId, DateTime? date)
        {
            if (!date.HasValue)
                date = DateTime.Today;

            var startOfMonth = new DateTime(date.Value.Year, date.Value.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);
            var stats = new List<TDStatWithAccount>();

            var extAccounts = tdRepo.ExtAccounts(platformCode: platformCode, campId: campId)
                                .OrderBy(a => a.Platform.Code).ThenBy(a => a.Name);
            foreach (var extAccount in extAccounts)
            {   //Note: Multiple Active Record Sets used here
                var stat = tdRepo.GetTDStatWithAccount(startOfMonth, endOfMonth, extAccount: extAccount); // MTD
                if (!stat.AllZeros())
                    stats.Add(stat);
            }
            return View(stats);
        }

        public ActionResult AdRoll(string advEid)
        {
            var today = DateTime.Today;
            var startOfMonth = new DateTime(today.Year, today.Month, 1);
            var stats = new List<TDStat>();

            var advertisable = tdRepo.Advertisable(advEid);
            if (advertisable == null)
                return Content("not found");

            var ads = tdRepo.AdRoll_Ads(advEid: advEid);
            foreach (var ad in ads)
            {   //Note: Multiple Active Record Sets used here
                var stat = tdRepo.GetAdRollStat(ad, startOfMonth, null); // MTD
                if (!stat.AllZeros())
                    stats.Add(stat);
            }
            var model = new TDStatsVM
            {
                Name = "MTD Stats - " + advertisable.Name + "(AdRoll)",
                Stats = stats.OrderBy(a => a.Name).ToList()
            };
            return View(model);
        }

        public ActionResult DBM(int ioID)
        {
            var today = DateTime.Today;
            var startOfMonth = new DateTime(today.Year, today.Month, 1);
            var stats = new List<TDStat>();

            var insertionOrder = tdRepo.InsertionOrder(ioID);
            if (insertionOrder == null)
                return Content("not found");

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