using System;
using System.Web.Mvc;
using DirectAgents.Domain.Concrete;
using DirectAgents.Domain.Contexts;

namespace DirectAgents.Web.Controllers
{
    public class ScreensController : ControllerBase
    {
        public ScreensController()
        {
            this.mainRepo = new MainRepository(new DAContext());
            //this.securityRepo = new SecurityRepository();
        }

        // TODO: retire this whole thing

        public JsonResult CakeStats() //MTD...
        {
            var today = DateTime.Today;
            var monthStart = new DateTime(today.Year, today.Month, 1);
            var stats = mainRepo.GetStatsSummary(null, monthStart, null);

            var json = Json(stats, JsonRequestBehavior.AllowGet);
            return json.ToJsonp();
        }

    }
}
