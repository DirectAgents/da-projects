using CakeExtracter.Helpers;

namespace CakeExtracter.Etl.AmazonSelenium.Configuration
{
    /// <summary>
    /// Selenium commands configuration helper. Prepare config values for using in application.
    /// </summary>
    internal class SeleniumCommandConfigurationManager
    {
        private const string WaitPageTimeoutConfigurationKey = "SeleniumWaitPageTimeoutInMinutes";

        /// <summary>
        /// Gets the number minutes of timeout for waiting page elements from the config application setting.
        /// </summary>
        /// <returns>Number minutes of timeout for waiting page elements.</returns>
        public static int GetWaitPageTimeout()
        {
            return ConfigurationHelper.GetIntConfigurationValue(WaitPageTimeoutConfigurationKey);
        }
    }
}
