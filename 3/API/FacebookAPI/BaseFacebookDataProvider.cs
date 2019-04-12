using System;
using System.Configuration;

namespace FacebookAPI
{
    public class BaseFacebookDataProvider
    {
        public string AccessToken { get; set; }

        public string ApiVersion { get; set; }

        private Action<string> _LogInfo;

        private Action<string> _LogError;

        protected void LogInfo(string message)
        {
            if (_LogInfo == null)
                Console.WriteLine(message);
            else
                _LogInfo("[FacebookUtility] " + message);
        }

        protected void LogError(string message)
        {
            if (_LogError == null)
                Console.WriteLine(message);
            else
                _LogError("[FacebookUtility] " + message);
        }

        public BaseFacebookDataProvider(Action<string> logInfo, Action<string> logError)
        {
            _LogInfo = logInfo;
            _LogError = logError;
            SetupAccessTokens();
        }

        private void SetupAccessTokens()
        {
            AccessToken = ConfigurationManager.AppSettings["FacebookToken"];
            ApiVersion = ConfigurationManager.AppSettings["FacebookApiVersion"];
        }
    }
}
