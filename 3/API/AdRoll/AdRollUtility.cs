﻿using System;
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
        private AdvertisableReportClient _AdvertisableReportClient;
        private AdvertisableReportClient AdvertisableReportClient
        {
            get
            {
                if (_AdvertisableReportClient == null)
                {
                    _AdvertisableReportClient = new AdvertisableReportClient();
                    SetupApiClient(_AdvertisableReportClient);
                }
                return _AdvertisableReportClient;
            }
        }

        private void SetupApiClient(ApiClient apiClient)
        {
            apiClient.SetCredentials(Username, Password);
            apiClient.SetLogging(m => LogInfo(m), m => LogError(m));
        }

        // --- Methods ---
        public List<AdSummary> AdSummaries(DateTime date, string advertisableEid)
        {
            //Note: We can only do one day at a time because the API allows either:
            // - breakdown by ad for a specified date or daterange (and advertisable)
            // - breakdown by day for a specified ad or group of ads (within the specified advertisable)
            // (but not both) ... so we'd need to know the Eid for each ad and make a call for each one.
            var request = new AdReportRequest
            {
                start_date = date.ToString("MM-dd-yyyy"),
                end_date = date.ToString("MM-dd-yyyy"),
                advertisables = advertisableEid
            };
            var response = this.AdReportClient.AdSummaries(request);
            if (response == null)
            {
                LogInfo("No AdSummaries found");
                return new List<AdSummary>();
            }
            var adSummaries = response.results;

            //Note: As is, the API returns an adsummary for each of the advertisable's ads, even though many of them have all zeros.
            // If we decide not to include zeros, think about the situation where an adsummary was non-zero then later because all-zeros. It wouldn't get updated.

            //bool includeZeros = true;
            //if (!includeZeros)
            //{
            //    adSummaries = new List<AdSummary>();
            //    foreach (var adSum in response.results)
            //    {
            //        if (!adSum.AllZeros())
            //            adSummaries.Add(adSum);
            //    }
            //}

            // Set the date for each summary, b/c the API doesn't include it for this query
            foreach (var adSum in adSummaries)
            {
                adSum.date = date;
            }
            return adSummaries;
        }

        public List<DailySummary> AdvertisableSummaries(DateTime startDate, DateTime endDate, string advertisableEid)
        {
            var request = new AdvertisableReportRequest
            {
                start_date = startDate.ToString("MM-dd-yyyy"),
                end_date = endDate.ToString("MM-dd-yyyy"),
                advertisables = advertisableEid
            };
            var response = this.AdvertisableReportClient.DailySummaries(request);
            if (response == null)
            {
                LogInfo("No DailySummaries found for the Advertisable");
                return new List<DailySummary>();
            }
            return response.results;
        }
    }
}
