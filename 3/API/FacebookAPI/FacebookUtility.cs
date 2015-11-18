using System;
using System.Net;
using Facebook;

namespace FacebookAPI
{
    public class FacebookUtility
    {
        public string AccessToken { get; set; }

        // --- Logging ---
        private Action<string> _LogInfo;
        private Action<string> _LogError;

        private void LogInfo(string message)
        {
            if (_LogInfo == null)
                Console.WriteLine(message);
            else
                _LogInfo("[FacebookUtility] " + message);
        }

        private void LogError(string message)
        {
            if (_LogError == null)
                Console.WriteLine(message);
            else
                _LogError("[FacebookUtility] " + message);
        }

        // --- Constructors ---
        public FacebookUtility()
        {
        }
        public FacebookUtility(Action<string> logInfo, Action<string> logError)
        {
            _LogInfo = logInfo;
            _LogError = logError;
            GetAccessToken();
        }

        private void GetAccessToken()
        {
            var client = new WebClient();
            var appid = "461180010752749";
            var appsecret = "ace0fafffa9e19c92b889861248cf48e";
            var oauthUrl = string.Format("https://graph.facebook.com/oauth/access_token?type=client_cred&client_id={0}&client_secret={1}", appid, appsecret);
            AccessToken = client.DownloadString(oauthUrl).Split('=')[1];
        }

        public void Test()
        {
            var accessToken = "CAAGjcNa36u0BAD19IrDX7aJn0t0vQ0OLKQfpUtpPJLirBK6AF1nVBxZBINvZAwoBt9FUZAuZBmfE3WHtTRZC2isoz9Fqn652zlvCpz9pIb6SZBwzrNmXQqBTNDh1JEGxVm07JluteDPbJ6htvzHL3ZBY6VZBcfjbVEZBGTk7rgBndiDUGfa0SPguZBQZCavcIBNZBhwWHFOzTyPTHEku1xNHIxP2";
            //var accessToken = AccessToken;
            var client = new FacebookClient(accessToken);
            string path = "act_101672655/insights?fields=impressions";
            dynamic obj = client.Get(path);
        }
    }
}
