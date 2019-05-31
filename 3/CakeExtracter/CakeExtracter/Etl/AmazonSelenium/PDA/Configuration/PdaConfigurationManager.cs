using System;
using System.Configuration;
using CakeExtracter.Helpers;

namespace CakeExtracter.Etl.AmazonSelenium.PDA.Configuration
{
    /// <summary>
    /// Configuration manager for PDA settings.
    /// </summary>
    internal class PdaConfigurationManager
    {
        private const string SignInPageUrlConfigurationKey = "PDA_SignInPageUrl";
        private const string CampaignsPageUrlConfigurationKey = "PDA_CampaignsPageUrl";
        private const string FilesNameTemplateConfigurationKey = "PDA_FilesNameTemplate";
        private const string DownloadsDirectoryNameConfigurationKey = "PDA_DownloadsDirectoryName";
        private const string CookiesDirectoryNameConfigurationKey = "PDA_CookiesDirectoryName";
        private const string EMailConfigurationKey = "PDA_EMail";
        private const string EMailPasswordConfigurationKey = "PDA_EMailPassword";
        private const string WaitPageTimeoutConfigurationKey = "PDA_WaitPageTimeoutInMinutes";
        private const string MaxRetryAttemptsConfigurationKey = "PDA_MaxRetryAttempts";
        private const string PauseBetweenAttemptsConfigurationKey = "PDA_PauseBetweenAttemptsInSeconds";

        /// <summary>
        /// Gets the URL to the sign-in page from the config application setting.
        /// </summary>
        /// <returns>URL to the sign-in page.</returns>
        public string GetSignInPageUrl()
        {
            return ConfigurationManager.AppSettings[SignInPageUrlConfigurationKey];
        }

        /// <summary>
        /// Gets the URL to the Campaign page from the config application setting.
        /// </summary>
        /// <returns>URL to the Campaign page.</returns>
        public string GetCampaignsPageUrl()
        {
            return ConfigurationManager.AppSettings[CampaignsPageUrlConfigurationKey];
        }

        /// <summary>
        /// Gets the template of file names from the config application setting.
        /// </summary>
        /// <returns>Template of file names.</returns>
        public string GetFilesNameTemplate()
        {
            return ConfigurationManager.AppSettings[FilesNameTemplateConfigurationKey];
        }

        /// <summary>
        /// Gets the name of download directory from the config application setting.
        /// </summary>
        /// <returns>Name of download directory.</returns>
        public string GetDownloadsDirectoryName()
        {
            return ConfigurationManager.AppSettings[DownloadsDirectoryNameConfigurationKey];
        }

        /// <summary>
        /// Gets the name of cookie directory from the config application setting.
        /// </summary>
        /// <returns>Name of cookie directory.</returns>
        public string GetCookiesDirectoryName()
        {
            return ConfigurationManager.AppSettings[CookiesDirectoryNameConfigurationKey];
        }

        /// <summary>
        /// Gets the login e-mail from the config application setting.
        /// </summary>
        /// <returns>E-mail for login.</returns>
        public string GetEMail()
        {
            return ConfigurationManager.AppSettings[EMailConfigurationKey];
        }

        /// <summary>
        /// Gets the password for login e-mail from the config application setting.
        /// </summary>
        /// <returns>Password of e-mail for login.</returns>
        public string GetEMailPassword()
        {
            return ConfigurationManager.AppSettings[EMailPasswordConfigurationKey];
        }

        /// <summary>
        /// Gets the number minutes of timeout for waiting page elements from the config application setting.
        /// </summary>
        /// <returns>Number minutes of timeout for waiting page elements.</returns>
        public int GetWaitPageTimeout()
        {
            return ConfigurationHelper.GetIntConfigurationValue(WaitPageTimeoutConfigurationKey);
        }

        /// <summary>
        /// Gets the number of maximum retry attempts from the config application setting.
        /// </summary>
        /// <returns>Number of maximum retry attempts.</returns>
        public int GetMaxRetryAttempts()
        {
            return ConfigurationHelper.GetIntConfigurationValue(MaxRetryAttemptsConfigurationKey);
        }

        /// <summary>
        /// Gets the number seconds of pause between attempts from the config application setting.
        /// </summary>
        /// <returns>Number seconds of pause between attempts.</returns>
        public TimeSpan GetPauseBetweenAttempts()
        {
            return TimeSpan.FromSeconds(ConfigurationHelper.GetIntConfigurationValue(PauseBetweenAttemptsConfigurationKey));
        }
    }
}
