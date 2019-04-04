using CakeExtracter.Common.JobExecutionManagement.JobExecution;
using System.Web.Mvc;

namespace DirectAgents.Web.Controllers
{
    public class JobsController : Controller
    {
        private IJobExecutionItemService jobExecutionItemService;

        public JobsController(IJobExecutionItemService jobExecutionItemService)
        {
            this.jobExecutionItemService = jobExecutionItemService;
        }

        // GET: Jobs
        public ActionResult History()
        {
            var historyItems = jobExecutionItemService.GetJobExecutionHistoryItems();
            return View();
        }
    }
}