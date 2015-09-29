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
                //.OrderBy(a => a.Platform.Name).ThenBy(a => a.Name)
                .Select(a => new
                {
                    a.Id,
                    a.ExternalId,
                    a.Name,
                    Platform = a.Platform.Name
                });
            var json = Json(new { data = extAccounts });
            return json;
        }

        public ActionResult MaintenanceDetail(int id)
        {
            var extAcct = tdRepo.ExtAccount(id);
            if (extAcct == null)
                return HttpNotFound();

            var model = new AccountMaintenanceVM
            {
                ExtAccount = extAcct,
                LatestStatDate = tdRepo.LatestStatDate(extAcct.Id)
            };
            return PartialView(model);
        }

        public JsonResult Synch(int id, DateTime? start)
        {
            var extAcct = tdRepo.ExtAccount(id);
            if (extAcct == null)
                return null;

            if (extAcct.Platform.Code == Platform.Code_AdRoll)
            {
                DASynchAdrollStats.RunStatic(extAcct.ExternalId, startDate: start);
            }
            else if (extAcct.Platform.Code == Platform.Code_DBM)
            {
                int ioID;
                if (int.TryParse(extAcct.ExternalId, out ioID))
                    DASynchDBMStats.RunStatic(ioID); // gets report with stats up to yesterday (and back 30 days)
            }
            //else
            return null;
        }
	}
}