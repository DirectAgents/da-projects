using System;
using System.Linq;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Entities.CPProg;

namespace DirectAgents.Web.Areas.TD.Controllers
{
    public class BudgetInfosController : DirectAgents.Web.Controllers.ControllerBase
    {
        public BudgetInfosController(ICPProgRepository cpProgRepository)
        {
            this.cpProgRepo = cpProgRepository;
        }

        public ActionResult CreateNew(int campId, DateTime date)
        {
            var campaign = cpProgRepo.Campaign(campId);
            if (campaign == null)
                return HttpNotFound();
            var budgetInfo = new BudgetInfo
            {
                CampaignId = campId,
                Date = date,
                MediaSpend = campaign.DefaultBudgetInfo.MediaSpend,
                MgmtFeePct = campaign.DefaultBudgetInfo.MgmtFeePct,
                MarginPct = campaign.DefaultBudgetInfo.MarginPct
            };
            cpProgRepo.AddBudgetInfo(budgetInfo);
            return RedirectToAction("Edit", "Campaigns", new { id = campId });
        }

        [HttpGet]
        public ActionResult Edit(int campId, DateTime date)
        {
            var budgetInfo = cpProgRepo.BudgetInfo(campId, date);
            if (budgetInfo == null)
                return HttpNotFound();
            SetupForEdit(budgetInfo);
            return View(budgetInfo);
        }
        [HttpPost]
        public ActionResult Edit(BudgetInfo bi)
        {
            if (ModelState.IsValid)
            {
                if (cpProgRepo.SaveBudgetInfo(bi))
                    return RedirectToAction("Edit", new { campId = bi.CampaignId, date = bi.Date.ToShortDateString() });
                    //return RedirectToAction("Edit", "Campaigns", new { id = bi.CampaignId });
                ModelState.AddModelError("", "BudgetInfo could not be saved.");
            }
            cpProgRepo.FillExtended(bi);
            SetupForEdit(bi);
            return View(bi);
        }
        private void SetupForEdit(BudgetInfo bi)
        {
            ViewBag.Platforms = cpProgRepo.PlatformsWithoutBudgetInfo(bi.CampaignId, bi.Date).OrderBy(p => p.Name);
        }
    }
}