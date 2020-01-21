using System;
using System.Configuration;
using CakeExtracter.Etl.AmazonSelenium.Configuration;
using CakeExtracter.Helpers;

namespace CakeExtracter.Etl.AmazonSelenium.PDA.Configuration
{
    /// <inheritdoc />
    /// <summary>
    /// PDA configuration manager. Prepare config values for using in application.
    /// </summary>
    internal class PdaCommandConfigurationManager : SeleniumCommandConfigurationManager
    {
        private const int DefaultIntervalBetweenUnsuccessfulAndNewRequestsInMinutes = 60;

        private readonly string cookiesDirectoryNameConfigurationKey;
        private readonly string emailConfigurationKey;
        private readonly string emailPasswordConfigurationKey;
        private readonly string maxRetryAttemptsConfigurationKey;
        private readonly string pauseBetweenAttemptsConfigurationKey;
        private readonly string intervalBetweenUnsuccessfulAndNewRequestsConfigurationKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="PdaCommandConfigurationManager"/> class.
        /// </summary>
        /// <param name="cookiesDirectoryNameConfigurationKey"></param>
        /// <param name="emailConfigurationKey"></param>
        /// <param name="emailPasswordConfigurationKey"></param>
        /// <param name="maxRetryAttemptsConfigurationKey"></param>
        /// <param name="pauseBetweenAttemptsConfigurationKey"></param>
        /// <param name="intervalBetweenUnsuccessfulAndNewRequestsConfigurationKey"></param>
        public PdaCommandConfigurationManager(
            string cookiesDirectoryNameConfigurationKey,
            string emailConfigurationKey,
            string emailPasswordConfigurationKey,
            string maxRetryAttemptsConfigurationKey,
            string pauseBetweenAttemptsConfigurationKey,
            string intervalBetweenUnsuccessfulAndNewRequestsConfigurationKey)
        {
            this.cookiesDirectoryNameConfigurationKey = cookiesDirectoryNameConfigurationKey;
            this.emailConfigurationKey = emailConfigurationKey;
            this.emailPasswordConfigurationKey = emailPasswordConfigurationKey;
            this.maxRetryAttemptsConfigurationKey = maxRetryAttemptsConfigurationKey;
            this.pauseBetweenAttemptsConfigurationKey = pauseBetweenAttemptsConfigurationKey;
            this.intervalBetweenUnsuccessfulAndNewRequestsConfigurationKey =
                intervalBetweenUnsuccessfulAndNewRequestsConfigurationKey;
        }

        /// <summary>
        /// Gets the name of cookie directory from the config application setting.
        /// </summary>
        /// <returns>Name of cookie directory.</returns>
        public string GetCookiesDirectoryName()
        {
            return ConfigurationManager.AppSettings[cookiesDirectoryNameConfigurationKey];
        }

        /// <summary>
        /// Gets the login e-mail from the config application setting.
        /// </summary>
        /// <returns>E-mail for login.</returns>
        public string GetEMail()
        {
            return ConfigurationManager.AppSettings[emailConfigurationKey];
        }

        /// <summary>
        /// Gets the password for login e-mail from the config application setting.
        /// </summary>
        /// <returns>Password of e-mail for login.</returns>
        public string GetEMailPassword()
        {
            return ConfigurationManager.AppSettings[emailPasswordConfigurationKey];
        }

        /// <summary>
        /// Gets the number of maximum retry attempts from the config application setting.
        /// </summary>
        /// <returns>Number of maximum retry attempts.</returns>
        public int GetMaxRetryAttempts()
        {
            return ConfigurationHelper.GetIntConfigurationValue(maxRetryAttemptsConfigurationKey);
        }

        /// <summary>
        /// Gets the number seconds of pause between attempts from the config application setting.
        /// </summary>
        /// <returns>Number seconds of pause between attempts.</returns>
        public TimeSpan GetPauseBetweenAttempts()
        {
            var numberOfSecondsForPauseBetweenAttempts =
                ConfigurationHelper.GetIntConfigurationValue(pauseBetweenAttemptsConfigurationKey);
            return TimeSpan.FromSeconds(numberOfSecondsForPauseBetweenAttempts);
        }

        /// <summary>
        /// Gets the number minutes of interval between unsuccessful and new job execution requests
        /// from the config application setting.
        /// </summary>
        /// <returns>Number minutes of interval between unsuccessful and new job execution requests.</returns>
        public int GetIntervalBetweenUnsuccessfulAndNewRequest()
        {
            return ConfigurationHelper.GetIntConfigurationValue(
                intervalBetweenUnsuccessfulAndNewRequestsConfigurationKey,
                DefaultIntervalBetweenUnsuccessfulAndNewRequestsInMinutes);
        }
    }
}
