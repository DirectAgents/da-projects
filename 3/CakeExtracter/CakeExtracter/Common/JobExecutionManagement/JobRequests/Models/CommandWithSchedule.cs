using System;

namespace CakeExtracter.Common.JobExecutionManagement.JobRequests.Models
{
    public class CommandWithSchedule
    {
        public ConsoleCommand Command { get; set; }

        public DateTime ScheduledTime { get; set; }
    }
}
