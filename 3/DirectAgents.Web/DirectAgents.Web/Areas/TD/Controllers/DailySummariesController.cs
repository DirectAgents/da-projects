using System;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Entities.TD;
using DirectAgents.Web.Areas.TD.Models;

namespace DirectAgents.Web.Areas.TD.Controllers
{
    public class DailySummariesController : DirectAgents.Web.Controllers.ControllerBase
    {
        public DailySummariesController(ITDRepository tdRepository)
        {
            this.tdRepo = tdRepository;
        }

        public ActionResult Index(int acctId, DateTime? month)
        {
            var extAcct = tdRepo.ExtAccount(acctId);
            if (extAcct == null)
                return HttpNotFound();
            DateTime? startDate = null, endDate = null;
            if (month.HasValue)
            {
                startDate = SetChooseMonthViewData_NonCookie(month);
                endDate = startDate.Value.AddMonths(1).AddDays(-1);
            }
            var model = new DailySummariesVM
            {
                ExtAccount = extAcct,
                Month = month.HasValue ? startDate : null,
                DailySummaries = tdRepo.DailySummaries(startDate, endDate, acctId: acctId)
            };
            Session["month"] = (month.HasValue ? month.Value.ToShortDateString() : ""); //TODO: set to startDate?
            return View(model);
        }

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
                    return RedirectToAction("Index", new { acctId = daySum.AccountId, month = Session["month"] });
                ModelState.AddModelError("", "DailySummary could not be saved");
            }
            tdRepo.FillExtended(daySum);
            return View(daySum);
        }

    }
}