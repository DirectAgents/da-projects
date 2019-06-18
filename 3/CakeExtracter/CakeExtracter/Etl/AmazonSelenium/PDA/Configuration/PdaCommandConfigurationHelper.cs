using System;
using System.Configuration;
using CakeExtracter.Etl.AmazonSelenium.Configuration;
using CakeExtracter.Helpers;

namespace CakeExtracter.Etl.AmazonSelenium.PDA.Configuration
{
    /// <inheritdoc />
    /// <summary>
    /// PDA configuration helper. Prepare config values for using in application.
    /// </summary>
    internal class PdaCommandConfigurationHelper : SeleniumCommandConfigurationHelper
    {
        private const string CookiesDirectoryNameConfigurationKey = "PDA_CookiesDirectoryName";
        private const string EMailConfigurationKey = "PDA_EMail";
        private const string EMailPasswordConfigurationKey = "PDA_EMailPassword";
        private const string MaxRetryAttemptsConfigurationKey = "PDA_MaxRetryAttempts";
        private const string PauseBetweenAttemptsConfigurationKey = "PDA_PauseBetweenAttemptsInSeconds";

        /// <summary>
        /// Gets the name of cookie directory from the config application setting.
        /// </summary>
        /// <returns>Name of cookie directory.</returns>
        public static string GetCookiesDirectoryName()
        {
            return ConfigurationManager.AppSettings[CookiesDirectoryNameConfigurationKey];
        }

        /// <summary>
        /// Gets the login e-mail from the config application setting.
        /// </summary>
        /// <returns>E-mail for login.</returns>
        public static string GetEMail()
        {
            return ConfigurationManager.AppSettings[EMailConfigurationKey];
        }

        /// <summary>
        /// Gets the password for login e-mail from the config application setting.
        /// </summary>
        /// <returns>Password of e-mail for login.</returns>
        public static string GetEMailPassword()
        {
            return ConfigurationManager.AppSettings[EMailPasswordConfigurationKey];
        }

        /// <summary>
        /// Gets the number of maximum retry attempts from the config application setting.
        /// </summary>
        /// <returns>Number of maximum retry attempts.</returns>
        public static int GetMaxRetryAttempts()
        {
            return ConfigurationHelper.GetIntConfigurationValue(MaxRetryAttemptsConfigurationKey);
        }

        /// <summary>
        /// Gets the number seconds of pause between attempts from the config application setting.
        /// </summary>
        /// <returns>Number seconds of pause between attempts.</returns>
        public static TimeSpan GetPauseBetweenAttempts()
        {
            return TimeSpan.FromSeconds(ConfigurationHelper.GetIntConfigurationValue(PauseBetweenAttemptsConfigurationKey));
        }
    }
}
