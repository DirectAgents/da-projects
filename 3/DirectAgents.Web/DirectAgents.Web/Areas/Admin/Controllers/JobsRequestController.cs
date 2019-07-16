using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Services.JobRequestsLifeCycleManagers.Interfaces;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Utils;
using DirectAgents.Domain.Entities.Administration.JobExecution;

namespace DirectAgents.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// Jobs Request Controller.
    /// </summary>
    /// <seealso cref="Controller" />
    public class JobsRequestController : Controller
    {
        private readonly IJobRequestLifeCycleManager requestManager;

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="JobsRequestController"/> class.
        /// </summary>
        /// <param name="requestManager">Job request scheduler service.</param>
        public JobsRequestController(IJobRequestLifeCycleManager requestManager)
        {
            this.requestManager = requestManager;
        }

        /// <summary>
        /// GET: Admin/JobsRequest
        /// Job requests page endpoint.
        /// </summary>
        /// <returns>Action result.</returns>
        public ActionResult Launch()
        {
            var existingCommands = CommandVerificationUtil.AllExistingCommands;
            var orderedCommands = existingCommands.OrderBy(x => x.Command).ToList();
            return View(orderedCommands);
        }

        /// <summary>
        /// Returns an action result with information about a command by its name.
        /// </summary>
        /// <param name="commandName">Command name.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public ActionResult GetCommandDescription(string commandName)
        {
            try
            {
                var commandInfo = GetCommandInfo(commandName);
                return Json(commandInfo);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        ///  Schedules a new command launch based on arguments.
        /// </summary>
        /// <param name="jobRequest">Job request object with arguments.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public ActionResult ScheduleJobRequest(JobRequest jobRequest)
        {
            try
            {
                if (!jobRequest.ScheduledTime.HasValue)
                {
                    jobRequest.ScheduledTime = DateTime.Now;
                }
                requestManager.ScheduleJobRequest(jobRequest);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);
            }
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
                requestManager.SetJobRequestsAsAborted(ids);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        private object GetCommandInfo(string commandName)
        {
            var command = CommandVerificationUtil.GetCommandByName(commandName);
            var arguments = CommandArgumentsConverter.GetCommandArguments(command);
            var argumentsLine = command is ConsoleCommand customCommand
                ? CommandArgumentsConverter.GetExampleCommandArgumentsAsLine(customCommand)
                : null;
            return new
            {
                Name = command.Command,
                Description = command.OneLineDescription,
                ArgumentsExample = argumentsLine,
                Arguments = arguments.Select(x => new
                {
                    x.Prototype,
                    x.Description,
                }).OrderBy(x => x.Prototype).ToList(),
            };
        }
    }
}