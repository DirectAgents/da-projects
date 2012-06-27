using System;

namespace DAgents.Common
{
    /// <summary>
    /// Logger that logs to standard output.
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        /// <summary>
        /// Logs a message to standard output.
        /// </summary>
        /// <param name="message">The message to write to the log.</param>
        public void Log(string message)
        {
            Console.WriteLine(message);
        }

        /// <summary>
        /// Logs an error to standard output.
        /// </summary>
        /// <param name="message">The error message to log.</param>
        public void LogError(string message)
        {
            Console.WriteLine("Error! " + message);
        }
    }
}
