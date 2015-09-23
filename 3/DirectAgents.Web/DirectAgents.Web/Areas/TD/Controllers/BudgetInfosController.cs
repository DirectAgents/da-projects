using System;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Entities.TD;

namespace DirectAgents.Web.Areas.TD.Controllers
{
    public class BudgetInfosController : DirectAgents.Web.Controllers.ControllerBase
    {
        public BudgetInfosController(ITDRepository tdRepository)
        {
            this.tdRepo = tdRepository;
        }

        public ActionResult Create(int campId, DateTime date)
        {
            var campaign = tdRepo.Campaign(campId);
            if (campaign == null)
                return HttpNotFound();
            var budgetInfo = new BudgetInfo
            {
                CampaignId = campId,
                Date = date,
                MediaSpend = campaign.DefaultBudget.MediaSpend,
                MgmtFeePct = campaign.DefaultBudget.MgmtFeePct,
                MarginPct = campaign.DefaultBudget.MarginPct
            };
            tdRepo.AddBudgetInfo(budgetInfo);
            return RedirectToAction("Edit", "Campaigns", new { id = campId });
        }

        [HttpGet]
        public ActionResult Edit(int campId, DateTime date)
        {
            var budgetInfo = tdRepo.BudgetInfo(campId, date);
            if (budgetInfo == null)
                return HttpNotFound();
            return View(budgetInfo);
        }
        [HttpPost]
        public ActionResult Edit(BudgetInfo bi)
        {
            if (ModelState.IsValid)
            {
                if (tdRepo.SaveBudgetInfo(bi))
                    return RedirectToAction("Edit", "Campaigns", new { id = bi.CampaignId });
                ModelState.AddModelError("", "BudgetInfo could not be saved.");
            }
            tdRepo.FillExtended(bi);
            return View(bi);
        }
    }
}