using System;

namespace CakeExtracter.Common.JobExecutionManagement.JobRequests.Models
{
    /// <summary>
    /// The model encapsulates the command and the time assigned to it.
    /// </summary>
    public class CommandWithSchedule
    {
        /// <summary>
        /// The command.
        /// </summary>
        public ConsoleCommand Command { get; set; }

        /// <summary>
        /// The scheduled time.
        /// </summary>
        public DateTime ScheduledTime { get; set; }
    }
}
