using CakeExtracter.Common.JobExecutionManagement.JobExecution;
using System.Web.Mvc;

namespace DirectAgents.Web.Areas.Admin.Controllers
{
    public class JobsExecutionController : Controller
    {
        public JobsExecutionController()
        {
        }

        public ActionResult History()
        {
            return View("History");
        }
    }
}