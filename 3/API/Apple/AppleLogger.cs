using System;

namespace Apple
{
    /// <summary>
    /// The logger for Apple API.
    /// </summary>
    public class AppleLogger
    {
        private const string LoggerPrefix = "[Apple Utility]";

        private readonly Action<Exception> logError;
        private readonly Action<string> logWarning;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppleLogger"/> class.
        /// </summary>
        /// <param name="logError">Action for logging (error level).</param>
        /// <param name="logWarning">Action for logging (warning level).</param>
        public AppleLogger(Action<string> logWarning, Action<Exception> logError)
        {
            this.logError = logError;
            this.logWarning = logWarning;
        }

        /// <summary>
        /// Log an exception.
        /// </summary>
        /// <param name="message">Message for logging.</param>
        public void LogError(string message)
        {
            if (logError == null)
            {
                Console.WriteLine(message);
            }
            else
            {
                var exception = new Exception(LoggerPrefix + message);
                logError(exception);
            }
        }

        /// <summary>
        /// Log a message as warning.
        /// </summary>
        /// <param name="message">Message for logging.</param>
        public void LogWarn(string message)
        {
            if (logWarning == null)
            {
                Console.WriteLine(message);
            }
            else
            {
                logWarning(LoggerPrefix + message);
            }
        }
    }
}
