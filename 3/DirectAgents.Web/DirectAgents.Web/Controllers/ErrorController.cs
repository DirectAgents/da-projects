using System.Web.Mvc;

namespace DirectAgents.Web.Controllers
{
    /// <summary>
    /// Controller to display an error page.
    /// </summary>
    public class ErrorController : Controller
    {
        /// <summary>
        /// Get a common error page.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}