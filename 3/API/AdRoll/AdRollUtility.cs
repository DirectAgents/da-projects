using System;
using System.Collections.Generic;
using System.Configuration;
using AdRoll.Clients;
using AdRoll.Entities;

namespace AdRoll
{
    public class AdRollUtility
    {
        private readonly string Username = ConfigurationManager.AppSettings["AdRollUsername"];
        private readonly string Password = ConfigurationManager.AppSettings["AdRollPassword"];

        // --- Logging ---
        private Action<string> _LogInfo;
        private Action<string> _LogError;

        private void LogInfo(string message)
        {
            if (_LogInfo == null)
                Console.WriteLine(message);
            else
                _LogInfo("[AdRollUtility] " + message);
        }

        private void LogError(string message)
        {
            if (_LogError == null)
                Console.WriteLine(message);
            else
                _LogError("[AdRollUtility] " + message);
        }

        // --- Constructors ---
        public AdRollUtility()
        {
        }
        public AdRollUtility(Action<string> logInfo, Action<string> logError)
        {
            _LogInfo = logInfo;
            _LogError = logError;
        }

        // --- Clients ---
        private AdReportClient _AdReportClient;
        private AdReportClient AdReportClient
        {
            get
            {
                if (_AdReportClient == null)
                {
                    _AdReportClient = new AdReportClient();
                    SetupApiClient(_AdReportClient);
                }
                return _AdReportClient;
            }
        }

        private void SetupApiClient(ApiClient apiClient)
        {
            apiClient.SetCredentials(Username, Password);
            apiClient.SetLogging(m => LogInfo(m), m => LogError(m));
        }

        // --- Methods ---
        public List<AdSummary> AdSummaries(DateTime date, string advertisableId)
        {
            var request = new AdReportRequest
            {
                start_date = date.ToString("MM-dd-yyyy"),
                end_date = date.ToString("MM-dd-yyyy"),
                advertisables = advertisableId
            };
            var response = this.AdReportClient.AdSummaries(request);
            if (response == null)
            {
                LogInfo("No AdSummaries found");
                return new List<AdSummary>();
            }
            else
                return response.results;
        }
    }
}
