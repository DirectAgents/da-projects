using System;
using System.Configuration;
using Facebook;

namespace FacebookAPI.Providers
{
    /// <summary>
    /// Base facebook data provider.
    /// </summary>
    public class BaseFacebookDataProvider
    {
        public string AccessToken { get; set; }

        public string ApiVersion { get; set; }

        private Action<string> _LogInfo;

        private Action<string> _LogWarn;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseFacebookDataProvider"/> class.
        /// </summary>
        /// <param name="logInfo">The log information.</param>
        /// <param name="logError">The log error.</param>
        public BaseFacebookDataProvider(Action<string> logInfo, Action<string> logWarn)
        {
            _LogInfo = logInfo;
            _LogWarn = logWarn;
            SetupAccessTokens();
        }

        /// <summary>
        /// Creates the fb client.
        /// </summary>
        /// <returns></returns>
        protected FacebookClient CreateFBClient()
        {
            var fbClient = new FacebookClient(AccessToken) { Version = "v" + ApiVersion };
            return fbClient;
        }

        /// <summary>
        /// Logs the information.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void LogInfo(string message)
        {
            if (_LogInfo == null)
                Console.WriteLine(message);
            else
                _LogInfo("[FacebookUtility] " + message);
        }

        /// <summary>
        /// Logs the warning.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void LogWarn(string message)
        {
            if (_LogWarn == null)
                Console.WriteLine(message);
            else
                _LogWarn("[FacebookUtility] " + message);
        }

        private void SetupAccessTokens()
        {
            AccessToken = ConfigurationManager.AppSettings["FacebookToken"];
            ApiVersion = ConfigurationManager.AppSettings["FacebookApiVersion"];
        }
    }
}
