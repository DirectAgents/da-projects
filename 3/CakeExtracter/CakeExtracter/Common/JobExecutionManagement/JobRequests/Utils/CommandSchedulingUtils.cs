using System;

namespace CakeExtracter.Common.JobExecutionManagement.JobRequests.Utils
{
    /// <summary>
    /// Command Scheduling Utils.
    /// </summary>
    internal static class CommandSchedulingUtils
    {
        /// <summary>
        /// Gets the max time for valid command launch.
        /// </summary>
        public static DateTime MaxTimeForValidCommandLaunch => DateTime.UtcNow;

        /// <summary>
        /// Gets default scheduled time.
        /// </summary>
        public static DateTime DefaultScheduledTime => DateTime.UtcNow;

        /// <summary>
        /// Gets the command retry scheduled time.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Command retry scheduled time.</returns>
        public static DateTime GetCommandRetryScheduledTime(ConsoleCommand command)
        {
            return DefaultScheduledTime.AddMinutes(command.IntervalBetweenUnsuccessfulAndNewRequestInMinutes);
        }

        /// <summary>
        /// Converts the source time to correct for schedule time zone.
        /// </summary>
        /// <param name="time">The source time.</param>
        /// <returns>The time in the correct time zone.</returns>
        public static DateTime GetTimeInCorrectTimeZone(DateTime time)
        {
            return time.ToUniversalTime();
        }
    }
}
