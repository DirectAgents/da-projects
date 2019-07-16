using System;

namespace CakeExtracter.Common.JobExecutionManagement.JobRequests.Utils
{
    /// <summary>
    /// Command Scheduling Utils.
    /// </summary>
    internal static class CommandSchedulingUtils
    {
        /// <summary>
        /// Gets the command retry scheduled time.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Command retry scheduled time.</returns>
        public static DateTime GetCommandRetryScheduledTime(ConsoleCommand command)
        {
            return DateTime.Now.AddMinutes(command.IntervalBetweenUnsuccessfulAndNewRequestInMinutes);
        }
    }
}
