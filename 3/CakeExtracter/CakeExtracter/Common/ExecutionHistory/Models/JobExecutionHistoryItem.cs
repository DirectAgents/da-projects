using System;

namespace CakeExtracter.Common.ExecutionHistory.Models
{
    /// <summary>
    /// Entity for job information history item. One item per one job execution.
    /// </summary>
    public class JobExecutionHistoryItem
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Job execution start time.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Job execution end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the name of the command.
        /// </summary>
        /// <value>
        /// The name of the command.
        /// </value>
        public string CommandName { get; set; }

        /// <summary>
        /// Gets or sets the execution argument.
        /// </summary>
        /// <value>
        /// The execution argument.
        /// </value>
        public string ExecutionArguments { get; set; }

        /// <summary>
        /// Job execution status. Short status value (scheduled, finished, processing, failed etc)
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string Status { get; set; }

        /// <summary>
        /// Job free status description. Contain information about execution state of job. 
        /// </summary>
        /// <value>
        /// The state of the current.
        /// </value>
        public string CurrentState { get; set; }

        /// <summary>
        /// Job warning messages.
        /// </summary>
        /// <value>
        /// The warnings.
        /// </value>
        public string Warnings { get; set; }

        /// <summary>
        /// Job error messages.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        public string Errors { get; set; }
    }
}
