using System;
using System.Collections.Generic;
using AdRoll.Clients;
using AdRoll.Entities;

namespace AdRoll
{
    public class AdRollUtility
    {
        // credentials
        // logging

        public List<AdSummary> AdSummaries(DateTime date, string advertisableId)
        {
            var client = new AdReportClient();
            var request = new AdReportRequest
            {
                start_date = date.ToString("MM-dd-yyyy"),
                end_date = date.ToString("MM-dd-yyyy"),
                advertisables = advertisableId
            };
            var response = client.AdSummaries(request);
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
