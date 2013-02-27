using System.Web.Configuration;
using System.Web.Mvc;
using Huggies.Web.Models;
using KendoGridBinder;

namespace Huggies.Web.Controllers
{
    public class AdminController : ControllerBase<Context>
    {
        private bool IsAllowedAdminAccess
        {
            get
            {
                if (Request != null)
                    if (Request.UserHostAddress != null)
                        return WebConfigurationManager.AppSettings["AdminIps"].Contains(Request.UserHostAddress);
                return false;
            }
        }

        //
        // GET: /Admin/
        public ActionResult Index()
        {
            if (!IsAllowedAdminAccess)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public ActionResult LeadsGrid(KendoGridRequest request)
        {
            if (!IsAllowedAdminAccess)
                return null;

            var leads = Context.Leads;
            var grid = new KendoGrid<Lead>(request, leads);
            return Json(grid);
        }
    }
}