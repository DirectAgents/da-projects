using System;
using System.Linq;
using System.Web.Mvc;
using CakeExtracter.Commands;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Entities.TD;
using DirectAgents.Web.Areas.TD.Models;

namespace DirectAgents.Web.Areas.TD.Controllers
{
    public class AccountsController : DirectAgents.Web.Controllers.ControllerBase
    {
        public AccountsController(ITDRepository tdRepository)
        {
            this.tdRepo = tdRepository;
        }

        public ActionResult Index(string platform, int? campId)
        {
            var extAccounts = tdRepo.ExtAccounts(platformCode: platform, campId: campId)
                .OrderBy(a => a.Platform.Name).ThenBy(a => a.Name);

            return View(extAccounts);
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

            bool syncable = extAcct.Platform.Code == Platform.Code_AdRoll;
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

            var firstOfMonth = Common.FirstOfMonth();
            var firstOfLastMonth = firstOfMonth.AddMonths(-1);
            if (!start.HasValue)
                start = firstOfLastMonth;
            else if (start >= firstOfMonth)
                start = firstOfMonth;
            else if (start > firstOfLastMonth)
                start = firstOfLastMonth;

            if (extAcct.Platform.Code == Platform.Code_AdRoll)
            {
                DASynchAdrollStats.RunStatic(extAcct.ExternalId, startDate: start);
            }
            else if (extAcct.Platform.Code == Platform.Code_DBM)
            {
                int ioID;
                if (int.TryParse(extAcct.ExternalId, out ioID))
                    DASynchDBMStats.RunStatic(insertionOrderID: ioID); // gets report with stats up to yesterday (and back ?30? days)
            }
            //else
            return null;
        }
	}
}