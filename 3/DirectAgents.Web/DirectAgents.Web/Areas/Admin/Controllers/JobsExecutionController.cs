using System;
using System.Net;
using System.Web.Mvc;
using CakeExtracter.Common.JobExecutionManagement.JobExecution.Services;

namespace DirectAgents.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// Jobs Execution Controller.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class JobsExecutionController : Controller
    {
        private IJobExecutionItemService jobExecutionItemService;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobsExecutionController" /> class.
        /// </summary>
        /// <param name="jobExecutionItemService">The job execution item service.</param>
        public JobsExecutionController(IJobExecutionItemService jobExecutionItemService)
        {
            this.jobExecutionItemService = jobExecutionItemService;
        }

        /// <summary>
        /// History Endpoint.
        /// </summary>
        /// <returns>Action result for job execution history page.</returns>
        public ActionResult History()
        {
            return View("History");
        }

        /// <summary>
        /// Sets the aborted status to items.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>Action Result Status Code.</returns>
        [HttpPost]
        public ActionResult SetAbortedStatusToItems(int[] ids)
        {
            try
            {
                jobExecutionItemService.SetJobExecutionItemsAbortedState(ids);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}