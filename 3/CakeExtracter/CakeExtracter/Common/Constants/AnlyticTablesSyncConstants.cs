namespace CakeExtracter.Common.Constants
{
    /// <summary>
    /// Analytic tables synchronization process constants.
    /// </summary>
    public class AnlyticTablesSyncConstants
    {
        /// <summary>
        /// The maximum retry attempts number for retry mechanism.
        /// </summary>
        public const int maxRetryAttempts = 5;

        /// <summary>
        /// The seconds to wait between attempts in retry mechanism. 
        /// </summary>
        public const int secondsToWait = 5;
    }
}
