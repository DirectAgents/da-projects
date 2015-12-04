using System;
using System.Linq;
using System.Web.Mvc;
using CakeExtracter.Commands;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Entities.TD;
using DirectAgents.Web.Areas.TD.Models;

namespace DirectAgents.Web.Areas.TD.Controllers
{
    public class ExtAccountsController : DirectAgents.Web.Controllers.ControllerBase
    {
        public ExtAccountsController(ITDRepository tdRepository)
        {
            this.tdRepo = tdRepository;
        }

        public ActionResult Index(string platform, int? campId)
        {
            var extAccounts = tdRepo.ExtAccounts(platformCode: platform, campId: campId)
                .OrderBy(a => a.Platform.Name).ThenBy(a => a.Name);

            Session["platformCode"] = platform;
            Session["campId"] = campId.ToString();
            return View(extAccounts);
        }

        public ActionResult CreateNew(string platform)
        {
            var plat = tdRepo.Platform(platform);
            if (plat != null)
            {
                var extAcct = new ExtAccount
                {
                    PlatformId = plat.Id,
                    Name = "zNew"
                };
                if (tdRepo.AddExtAccount(extAcct))
                    return RedirectToAction("Index", new { platform = Session["platformCode"], campId = Session["campId"] });
            }
            return Content("Error creating ExtAccount");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var extAcct = tdRepo.ExtAccount(id);
            if (extAcct == null)
                return HttpNotFound();
            return View(extAcct);
        }
        [HttpPost]
        public ActionResult Edit(ExtAccount extAcct)
        {
            if (ModelState.IsValid)
            {
                if (tdRepo.SaveExtAccount(extAcct))
                    return RedirectToAction("Index", new { platform = Session["platformCode"], campId = Session["campId"] });
                ModelState.AddModelError("", "ExtAccount could not be saved.");
            }
            tdRepo.FillExtended(extAcct);
            return View(extAcct);
        }

        // json data for Maintenance Grid
        public JsonResult IndexData(string platform)
        {
            var extAccounts = tdRepo.ExtAccounts(platformCode: platform)
                .Select(a => new
                {
                    a.Id,
                    a.ExternalId,
                    a.Name,
                    Platform = a.Platform.Name
                });
            var json = Json(new { data = extAccounts, total = extAccounts.Count() });
            return json;
        }

        public ActionResult MaintenanceDetail(int id)
        {
            var extAcct = tdRepo.ExtAccount(id);
            if (extAcct == null)
                return HttpNotFound();

            bool syncable = extAcct.Platform.Code == Platform.Code_AdRoll ||
                            extAcct.Platform.Code == Platform.Code_FB;
            if (extAcct.Platform.Code == Platform.Code_DBM)
            {
                int ioID;
                if (int.TryParse(extAcct.ExternalId, out ioID))
                {
                    var io = tdRepo.InsertionOrder(ioID);
                    if (io != null)
                        syncable = !string.IsNullOrWhiteSpace(io.Bucket);
                }
            }
            var model = new AccountMaintenanceVM
            {
                ExtAccount = extAcct,
                LatestStatDate = tdRepo.LatestStatDate(extAcct.Id),
                Syncable = syncable
            };
            return PartialView(model);
        }

        public JsonResult Sync(int id, DateTime? start)
        {
            var extAcct = tdRepo.ExtAccount(id);
            if (extAcct == null)
                return null;

            // Go back to the 1st - if requesting this month or last month
            var firstOfMonth = Common.FirstOfMonth();
            var firstOfLastMonth = firstOfMonth.AddMonths(-1);
            if (!start.HasValue)
                start = firstOfLastMonth;
            else if (start >= firstOfMonth)
                start = firstOfMonth;
            else if (start > firstOfLastMonth)
                start = firstOfLastMonth;

            switch (extAcct.Platform.Code)
            {
                case Platform.Code_AdRoll:
                    DASynchAdrollStats.RunStatic(extAcct.ExternalId, startDate: start);
                    break;
                case Platform.Code_DBM:
                    int ioID;
                    if (int.TryParse(extAcct.ExternalId, out ioID))
                        DASynchDBMStats.RunStatic(insertionOrderID: ioID); // gets report with stats up to yesterday (and back ?30? days)
                    break;
                case Platform.Code_FB:
                    DASynchFacebookStats.RunStatic(extAcctId: extAcct.Id, startDate: start);
                    break;
            }
            return null;
        }
	}
}