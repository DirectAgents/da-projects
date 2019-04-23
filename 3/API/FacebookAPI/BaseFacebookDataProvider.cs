using Facebook;
using System;
using System.Configuration;

namespace FacebookAPI
{
    /// <summary>
    /// Base facebook data provider
    /// </summary>
    public class BaseFacebookDataProvider
    {
        public string AccessToken { get; set; }

        public string ApiVersion { get; set; }

        private Action<string> _LogInfo;

        private Action<string> _LogError;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseFacebookDataProvider"/> class.
        /// </summary>
        /// <param name="logInfo">The log information.</param>
        /// <param name="logError">The log error.</param>
        public BaseFacebookDataProvider(Action<string> logInfo, Action<string> logError)
        {
            _LogInfo = logInfo;
            _LogError = logError;
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
        /// Logs the error.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void LogError(string message)
        {
            if (_LogError == null)
                Console.WriteLine(message);
            else
                _LogError("[FacebookUtility] " + message);
        }

        private void SetupAccessTokens()
        {
            AccessToken = ConfigurationManager.AppSettings["FacebookToken"];
            ApiVersion = ConfigurationManager.AppSettings["FacebookApiVersion"];
        }
    }
}
