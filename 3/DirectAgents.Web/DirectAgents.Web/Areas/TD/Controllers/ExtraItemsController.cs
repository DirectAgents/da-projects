using System;
using System.Linq;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Entities.TD;
using DirectAgents.Web.Areas.TD.Models;

namespace DirectAgents.Web.Areas.TD.Controllers
{
    public class ExtraItemsController : DirectAgents.Web.Controllers.ControllerBase
    {
        public ExtraItemsController(ITDRepository tdRepository)
        {
            this.tdRepo = tdRepository;
        }

        public ActionResult Index(int? campId, DateTime? month)
        {
            Campaign campaign = null;
            if (campId.HasValue)
            {
                campaign = tdRepo.Campaign(campId.Value);
                if (campaign == null)
                    return HttpNotFound();
            }
            DateTime? startDate = null, endDate = null;
            if (month.HasValue)
            {
                startDate = SetChooseMonthViewData_NonCookie(month);
                endDate = startDate.Value.AddMonths(1).AddDays(-1);
            }
            var items = tdRepo.ExtraItems(startDate, endDate, campId: campId);

            var model = new ExtraItemsVM
            {
                Campaign = campaign,
                Month = month.HasValue ? startDate : null,
                Items = items.OrderBy(i => i.Date).ThenBy(i => i.Id)
            };
            Session["campId"] = campId.ToString();
            Session["month"] = (month.HasValue ? month.Value.ToShortDateString() : ""); //TODO: set to startDate?
            return View(model);
        }

        public ActionResult CreateNew(int campId, DateTime? date)
        {
            var platformTD = tdRepo.Platform(Platform.Code_DATradingDesk);
            if (platformTD == null)
                return HttpNotFound();
            if (!date.HasValue)
                date = DateTime.Today;

            var extraItem = new ExtraItem
            {
                Date = date.Value,
                CampaignId = campId,
                PlatformId = platformTD.Id,
                Description = "New"
            };
            if (tdRepo.AddExtraItem(extraItem))
                return RedirectToAction("Index", new { campId = Session["campId"], month = Session["month"] });
            else
                return Content("Error creating Extra Item");
        }
        public ActionResult Delete(int id)
        {
            tdRepo.DeleteExtraItem(id);
            return RedirectToAction("Index", new { campId = Session["campId"], month = Session["month"] });
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var item = tdRepo.ExtraItem(id);
            if (item == null)
                return HttpNotFound();
            SetupForEdit();
            return View(item);
        }
        [HttpPost]
        public ActionResult Edit(ExtraItem item)
        {
            if (ModelState.IsValid)
            {
                if (tdRepo.SaveExtraItem(item))
                    return RedirectToAction("Index", new { campId = Session["campId"], month = Session["month"] });
                ModelState.AddModelError("", "ExtraItem could not be saved.");
            }
            tdRepo.FillExtended(item);
            SetupForEdit();
            return View(item);
        }
        private void SetupForEdit()
        {
            ViewBag.Platforms = tdRepo.Platforms().OrderBy(p => p.Name).ToList();
            //TODO: fill campaign name? FillExtended?
        }
	}
}