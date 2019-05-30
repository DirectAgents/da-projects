﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Services.JobRequestSchedulers.Interfaces;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Utils;
using CakeExtractor.SeleniumApplication.Commands;
using DirectAgents.Domain.Entities.Administration.JobExecution;

namespace DirectAgents.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// Jobs Request Controller.
    /// </summary>
    /// <seealso cref="Controller" />
    public class JobsRequestController : Controller
    {
        private readonly IJobExecutionRequestScheduler requestScheduler;

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:DirectAgents.Web.Areas.Admin.Controllers.JobsRequestController" /> class.
        /// </summary>
        /// <param name="requestScheduler">Job request scheduler service.</param>
        public JobsRequestController(IJobExecutionRequestScheduler requestScheduler)
        {
            this.requestScheduler = requestScheduler;
        }

        /// <summary>
        /// GET: Admin/JobsRequest
        /// Job requests page endpoint.
        /// </summary>
        /// <returns>Action result.</returns>
        public ActionResult Launch()
        {
            var existingCommands = GetAllExistingCommand();
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
            if (!jobRequest.ScheduledTime.HasValue)
            {
                jobRequest.ScheduledTime = DateTime.Now;
            }

            try
            {
                var existingCommands = GetAllExistingCommand();
                requestScheduler.VerifyJobRequest(existingCommands, jobRequest);
                requestScheduler.ScheduleJobRequest(jobRequest);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        private List<ManyConsole.ConsoleCommand> GetAllExistingCommand()
        {
            var consoleCommands =
                ManyConsole.ConsoleCommandDispatcher.FindCommandsInSameAssemblyAs(typeof(ConsoleCommand));
            var seleniumCommands =
                ManyConsole.ConsoleCommandDispatcher.FindCommandsInSameAssemblyAs(typeof(SyncAmazonPdaCommand));
            var existingCommands = consoleCommands.Concat(seleniumCommands);
            return existingCommands.ToList();
        }

        private object GetCommandInfo(string commandName)
        {
            var existingCommands = GetAllExistingCommand();
            var command = existingCommands.Find(x => x.Command == commandName);
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