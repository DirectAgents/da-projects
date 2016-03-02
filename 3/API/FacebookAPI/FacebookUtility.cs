﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using Facebook;
using FacebookAPI.Entities;

namespace FacebookAPI
{
    public class FacebookUtility
    {
        public const int RowsReturnedAtATime = 25;

        public int DaysPerCall_Campaign = 20;
        public int DaysPerCall_Ad = 8;

        //public string AppId { get; set; }
        //public string AppSecret { get; set; }
        public string AccessToken { get; set; }
        public string ApiVersion { get; set; }

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
            Setup();
        }
        public FacebookUtility(Action<string> logInfo, Action<string> logError)
            : this()
        {
            _LogInfo = logInfo;
            _LogError = logError;
        }

        private void Setup()
        {
            AccessToken = ConfigurationManager.AppSettings["FacebookToken"];
            ApiVersion = ConfigurationManager.AppSettings["FacebookApiVersion"];
            //AccessToken = AppId + "|" + AppSecret;
            //var client = new WebClient();
            //var oauthUrl = string.Format("https://graph.facebook.com/oauth/access_token?type=client_cred&client_id={0}&client_secret={1}", AppId, AppSecret);
            //AccessToken = client.DownloadString(oauthUrl).Split('=')[1];
        }
        private FacebookClient CreateFBClient()
        {
            var fbClient = new FacebookClient(AccessToken);
            fbClient.Version = "v" + ApiVersion;
            return fbClient;
        }

        //private void GetAccessToken2() // No can do; returns "unsupported browser"
        //{
        //    var client = new WebClient();
        //    var redirectUrl = "http://localhost/daweb/td/campaigns";
        //    var url1 = string.Format("https://www.facebook.com/dialog/oauth?client_id={0}&redirect_uri={1}&scope=ads_read", AppId, redirectUrl);
        //    var x = client.DownloadString(url1);
        //}

        // TODO: make asynchronous / do calls in parallel ?
        public IEnumerable<FBSummary> GetDailyStats(string accountId, DateTime start, DateTime end)
        {
            return GetFBSummariesLoop(accountId, start, end);
        }
        public IEnumerable<FBSummary> GetDailyCampaignStats(string accountId, DateTime start, DateTime end)
        {
            return GetFBSummariesLoop(accountId, start, end, byCampaign: true);
        }
        public IEnumerable<FBSummary> GetDailyAdStats(string accountId, DateTime start, DateTime end)
        {
            int daysPerCall = DaysPerCall_Ad;
            while (start <= end)
            {
                var tempEnd = start.AddDays(daysPerCall - 1);
                if (tempEnd > end)
                    tempEnd = end;
                var fbSummaries = GetFBSummaries(accountId, start, tempEnd, byAd: true);
                var groups = fbSummaries.GroupBy(s => new { s.Date, s.AdName });
                foreach (var group in groups)
                {
                    var fbSum = new FBSummary
                    {
                        Date = group.Key.Date,
                        AdName = group.Key.AdName,
                        Spend = group.Sum(g => g.Spend),
                        Impressions = group.Sum(g => g.Impressions),
                        UniqueClicks = group.Sum(g => g.UniqueClicks),
                        LinkClicks = group.Sum(g => g.LinkClicks),
                        TotalActions = group.Sum(g => g.TotalActions)
                    };
                    yield return fbSum;
                }
                start = start.AddDays(daysPerCall);
            }
        }

        public IEnumerable<FBSummary> GetFBSummariesLoop(string accountId, DateTime start, DateTime end, bool byCampaign = false, bool byAd = false)
        {
            int daysPerCall = 365; // default
            if (byCampaign)
                daysPerCall = DaysPerCall_Campaign;
            else if (byAd)
                daysPerCall = DaysPerCall_Ad;

            while (start <= end)
            {
                var tempEnd = start.AddDays(daysPerCall - 1);
                if (tempEnd > end)
                    tempEnd = end;
                var fbSummaries = GetFBSummaries(accountId, start, tempEnd, byCampaign, byAd);
                foreach (var fbSum in fbSummaries)
                {
                    yield return fbSum;
                }
                start = start.AddDays(daysPerCall);
            }
        }
        public IEnumerable<FBSummary> GetFBSummaries(string accountId, DateTime start, DateTime end, bool byCampaign = false, bool byAd = false)
        {
            string by = byCampaign ? " by Campaign" : "";
            by += byAd ? " by Ad" : "";
            LogInfo(string.Format("GetFBSummaries {0:d} - {1:d} ({2}{3})", start, end, accountId, by));

            var fbClient = CreateFBClient();
            var path = accountId + "/insights";

            var levelVal = "";
            var fieldsVal = "impressions,unique_clicks,inline_link_clicks,total_actions,spend";
            if (byCampaign)
            {
                levelVal = "campaign";
                fieldsVal += ",campaign_id,campaign_name";
            }
            else if (byAd)
            {
                levelVal = "ad";
                fieldsVal += ",ad_id,ad_name";
            }
            var afterVal = "";

            bool moreData;
            do
            {
                moreData = false;
                var parms = new
                {
                    //metadata = 1,
                    level = levelVal,
                    fields = fieldsVal,
                    time_range = new { since = DateString(start), until = DateString(end) },
                    time_increment = 1,
                    after = afterVal
                };
                dynamic retObj = null;
                int tryNumber = 0;
                do
                {
                    try
                    {
                        retObj = fbClient.Get(path, parms);
                        tryNumber = 0;
                    }
                    catch (Exception ex)
                    {
                        LogError(ex.Message);
                        tryNumber++;
                        if (tryNumber < 10)
                        {
                            LogInfo("Waiting 60 seconds before trying again.");
                            Thread.Sleep(60000);
                        }
                    }
                } while (tryNumber > 0 && tryNumber < 10);
                if (tryNumber >= 10)
                    throw new Exception("Tried 10 times. Throwing exception.");

                if (retObj == null)
                    continue;

                int impressions;
                if (retObj.data != null)
                {
                    foreach (var row in retObj.data)
                    {
                        var fbSum = new FBSummary
                        {
                            Date = DateTime.Parse(row.date_start),
                            Spend = (decimal)row.spend,
                            //Impressions = row.impressions,
                            UniqueClicks = (int)row.unique_clicks,
                            LinkClicks = (int)row.inline_link_clicks,
                            TotalActions = (int)row.total_actions,
                            CampaignId = row.campaign_id,
                            CampaignName = row.campaign_name,
                            AdId = row.ad_id,
                            AdName = row.ad_name
                        };
                        if (Int32.TryParse(row.impressions, out impressions))
                            fbSum.Impressions = impressions;
                        yield return fbSum;
                    }
                }
                moreData = (retObj.paging != null && retObj.paging.next != null);
                if (moreData)
                    afterVal = retObj.paging.cursors.after;
            } while (moreData);
        }

        public void TestToken()
        {
            var accessToken = "";
            var client = new FacebookClient(AccessToken);
            dynamic obj = client.Get("debug_token", new {
                input_token = accessToken
            });
        }

        public void Test()
        {
            //string path = "v2.5/me/adaccounts";
            //string path = "v2.5/act_101672655?fields=insights{impressions}";

            //string path = "v2.5/act_101672655";
            //dynamic obj = client.Get(path, new { fields = "insights{impressions}" });
            //string path = "v2.5/act_101672655/insights";
            //dynamic obj = client.Get(path, new { fields = "impressions", date_preset = "last_7_days" });
            //dynamic obj = client.Get(path, new { fields = "impressions", time_range = new { since = "2015-11-10", until = "2015-11-12" } });
            //var acctId = "act_10153287675738628"; // Crackle
            var acctId = "act_101672655"; // Zeel consumer
            var path = acctId + "/insights";
            //var fullpath = "v" + ApiVersion + "/" + path;
            var fbClient = CreateFBClient();
            dynamic obj = fbClient.Get(path, new {
                metadata = 1,
                fields = "impressions,unique_clicks,total_actions,spend",
                time_range = new { since = "2015-10-1", until = "2015-10-3" },
                time_increment = 1
            });
        }

        public static string DateString(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-M-d");
        }
    }
}
