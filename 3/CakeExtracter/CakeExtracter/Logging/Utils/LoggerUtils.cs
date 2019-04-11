namespace CakeExtracter.Logging.Utils
{
    /// <summary>
    /// Logging utils container.
    /// </summary>
    public static class LoggerUtils
    {
        /// <summary>
        /// Gets the log message. Join format and params.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public static string GetLogMessage(string format, params object[] args)
        {
            try
            {
                return string.Format(format, args);
            }
            catch
            {
                return format; // Logging operations should not throw exceptions.
            }
        }
    }
}