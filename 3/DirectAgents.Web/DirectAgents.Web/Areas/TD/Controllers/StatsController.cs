using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.DTO;
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
            // Month/MTD view
            var campaign = tdRepo.Campaign(campId);
            if (campaign == null)
                return Content("Campaign not found");

            if (!date.HasValue)
                date = DateTime.Today;
            var startOfMonth = new DateTime(date.Value.Year, date.Value.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            var budgetInfo = campaign.BudgetInfoFor(startOfMonth);
            var tdStat = tdRepo.GetTDStat(startOfMonth, endOfMonth, campaign: campaign, marginFees: budgetInfo);
            var statWithBudget = new TDStatWithBudget(tdStat, budgetInfo);
            if (budgetInfo == null)
                statWithBudget.Date = startOfMonth;

            return View(statWithBudget);
        }

        // Stats by "external account"
        public ActionResult ExtAccount(string platform, int? campId, DateTime? month)
        {
            var startOfMonth = SetChooseMonthViewData_NonCookie(month);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);
            var stats = new List<TDStat>();

            var extAccounts = tdRepo.ExtAccounts(platformCode: platform, campId: campId)
                                .OrderBy(a => a.Platform.Code).ThenBy(a => a.Name);
            foreach (var extAccount in extAccounts)
            {   //Note: Multiple Active Record Sets used here
                var stat = tdRepo.GetTDStatWithAccount(startOfMonth, endOfMonth, extAccount: extAccount);
                if (!stat.AllZeros())
                    stats.Add(stat);
            }
            var model = new TDStatsVM
            {
                //Name =
                Month = startOfMonth,
                PlatformCode = platform,
                CampaignId = campId,
                Stats = stats
            };
            return View(model);
        }

        // Stats by Ad
        public ActionResult AdRoll(string advEid, DateTime? month)
        {
            var startOfMonth = SetChooseMonthViewData_NonCookie(month);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);
            var stats = new List<TDStat>();

            var advertisable = tdRepo.Advertisable(advEid);
            if (advertisable == null)
                return Content("not found");

            var ads = tdRepo.AdRoll_Ads(advEid: advEid);
            foreach (var ad in ads)
            {   //Note: Multiple Active Record Sets used here
                var stat = tdRepo.GetAdRollStat(ad, startOfMonth, endOfMonth);
                if (!stat.AllZeros())
                    stats.Add(stat);
            }
            var model = new TDStatsVM
            {
                Name = "ExtAccount: " + advertisable.Name + "(AdRoll)",
                ExternalId = advEid,
                Month = startOfMonth,
                Stats = stats.OrderBy(a => a.Name).ToList()
            };
            return View(model);
        }

        // Stats by Creative
        public ActionResult DBM(int ioID, DateTime? month)
        {
            var startOfMonth = SetChooseMonthViewData_NonCookie(month);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            var insertionOrder = tdRepo.InsertionOrder(ioID);
            if (insertionOrder == null)
                return Content("not found");

            var stats = tdRepo.GetDBMStatsByCreative(ioID, startOfMonth, endOfMonth);
            var model = new TDStatsVM
            {
                Name = "ExtAccount: " + insertionOrder.Name + "(DBM)",
                ExternalId = ioID.ToString(),
                Month = startOfMonth,
                Stats = stats.OrderBy(a => a.Name).ToList()
            };
            return View("Generic", model);
        }

        // Edit a DailySummary
        [HttpGet]
        public ActionResult Edit(DateTime date, int acctId, bool create = false)
        {
            var daySum = tdRepo.DailySummary(date, acctId);
            if (daySum == null)
            {
                if (create)
                {
                    daySum = new DailySummary
                    {
                        Date = date,
                        AccountId = acctId
                    };
                    if (tdRepo.AddDailySummary(daySum))
                        tdRepo.FillExtended(daySum);
                    else
                        return Content("DailySummary could not be created");
                }
                else
                    return HttpNotFound();
            }
            return View(daySum);
        }
        [HttpPost]
        public ActionResult Edit(DailySummary daySum)
        {
            if (ModelState.IsValid)
            {
                if (tdRepo.SaveDailySummary(daySum))
                    return Content("saved");
                ModelState.AddModelError("", "DailySummary could not be saved");
            }
            tdRepo.FillExtended(daySum);
            return View(daySum);
        }

        // Demo - DBM stats by creative
        public ActionResult Spreadsheet(int ioID, DateTime? month)
        {
            return View(ioID);
        }
        public JsonResult SpreadsheetData(int ioID, DateTime? month)
        {
            var startOfMonth = SetChooseMonthViewData_NonCookie(month);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            var insertionOrder = tdRepo.InsertionOrder(ioID);
            if (insertionOrder == null)
                return null;

            var stats = tdRepo.GetDBMStatsByCreative(ioID, startOfMonth, endOfMonth);
            var json = Json(stats, JsonRequestBehavior.AllowGet);
            //var json = Json(stats);
            return json;
        }
    }

    class KG<T>
    {
        public IEnumerable<T> data { get; set; }
        public int total { get; set; }
        public object aggregates { get; set; }
    }
}