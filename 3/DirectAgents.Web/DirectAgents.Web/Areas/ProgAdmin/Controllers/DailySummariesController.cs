using System;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Web.Areas.ProgAdmin.Models;

namespace DirectAgents.Web.Areas.ProgAdmin.Controllers
{
    public class DailySummariesController : DirectAgents.Web.Controllers.ControllerBase
    {
        public DailySummariesController(ICPProgRepository cpProgRepository)
        {
            this.cpProgRepo = cpProgRepository;
        }

        public ActionResult Index(int acctId, DateTime? start, DateTime? end)
        {
            var extAcct = cpProgRepo.ExtAccount(acctId);
            if (extAcct == null)
                return HttpNotFound();

            if (start.HasValue && start.Value.Day == 1 && !end.HasValue)
                end = start.Value.AddMonths(1).AddDays(-1);

            bool customDates = true;
            if (start.HasValue && end.HasValue)
                customDates = AreDatesCustom(start.Value, end.Value);

            if (!customDates)
                SetChooseMonthViewData_NonCookie(start);

            var model = new DailySummariesVM
            {
                ExtAccount = extAcct,
                Start = start,
                End = end,
                CustomDates = AreDatesCustom(start, end),
                DailySummaries = cpProgRepo.DailySummaries(start, end, acctId: acctId)
            };
            Session["start"] = (start.HasValue ? start.Value.ToShortDateString() : "");
            Session["end"] = (end.HasValue ? end.Value.ToShortDateString() : "");
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(DateTime date, int acctId, bool create = false)
        {
            var daySum = cpProgRepo.DailySummary(date, acctId);
            if (daySum == null)
            {
                if (create)
                {
                    daySum = new DailySummary
                    {
                        Date = date,
                        AccountId = acctId
                    };
                    if (cpProgRepo.AddDailySummary(daySum))
                        cpProgRepo.FillExtended(daySum);
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
                if (cpProgRepo.SaveDailySummary(daySum))
                    return RedirectToAction("Index", new { acctId = daySum.AccountId, start = Session["start"], end = Session["end"] });
                ModelState.AddModelError("", "DailySummary could not be saved");
            }
            cpProgRepo.FillExtended(daySum);
            return View(daySum);
        }

    }
}