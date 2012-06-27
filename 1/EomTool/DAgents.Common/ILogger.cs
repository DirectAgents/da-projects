namespace DAgents.Common
{
    public interface ILogger
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        void Log(string message);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        void LogError(string message);
    }
}
