using System.Web.Mvc;

namespace DirectAgents.Web.Controllers
{
    /// <summary>
    /// Home Controller
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class HomeController : Controller
    {
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return RedirectToAction("History", "JobsExecution", new { Area = "Admin" });
        }
    }
}