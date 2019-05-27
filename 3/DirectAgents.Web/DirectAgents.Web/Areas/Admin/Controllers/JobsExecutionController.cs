using System.Web.Mvc;

namespace DirectAgents.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// Jobs Execution Controller.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class JobsExecutionController : Controller
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JobsExecutionController"/> class.
        /// </summary>
        public JobsExecutionController()
        {
        }

        /// <summary>
        /// History Endpoint.
        /// </summary>
        /// <returns>Action result for job execution history page.</returns>
        public ActionResult History()
        {

            return View("History");
        }
    }
}