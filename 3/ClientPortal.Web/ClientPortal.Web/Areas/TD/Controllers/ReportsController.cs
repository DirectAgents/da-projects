using ClientPortal.Data.Contracts;
using ClientPortal.Data.Entities.TD;
using DirectAgents.Mvc.KendoGridBinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClientPortal.Web.Areas.TD.Controllers
{
    public class ReportsController : Controller
    {
        ITDRepository tdRepo;

        public ReportsController(ITDRepository tdRepository)
        {
            tdRepo = tdRepository;
        }

        public ActionResult Sample()
        {
            return View();
        }

        public JsonResult SampleData(KendoGridRequest request)
        {
            var summaries = tdRepo.GetDailySummaries(null, null, null);
            var kgrid = new KendoGrid<DailySummary>(request, summaries);

            var json = Json(kgrid, JsonRequestBehavior.AllowGet);
            return json;
        }

    }
}
