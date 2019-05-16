using System.Web.Mvc;

namespace DirectAgents.Web.Areas.ProgAdmin.Controllers
{
    public class MaintenanceController : DirectAgents.Web.Controllers.ControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}