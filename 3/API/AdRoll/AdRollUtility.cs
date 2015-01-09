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

        // logging

        private AdReportClient _AdReportClient;
        private AdReportClient AdReportClient
        {
            get
            {
                if (_AdReportClient == null)
                {
                    _AdReportClient = new AdReportClient();
                    _AdReportClient.SetCredentials(Username, Password);
                    // set logger ?
                }
                return _AdReportClient;
            }
        }

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
                // log?
                return new List<AdSummary>();
            }
            else
                return response.results;
        }
    }
}
