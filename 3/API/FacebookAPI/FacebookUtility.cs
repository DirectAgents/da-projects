using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Facebook;
using FacebookAPI.Entities;

namespace FacebookAPI
{
    public class FacebookUtility
    {
        public const int RowsReturnedAtATime = 25;
        public const string Pattern_ParenNums = @"^\((\d+)\)\s*";

        public int DaysPerCall_Campaign = 20;
        public int DaysPerCall_AdSet = 4;
        public int DaysPerCall_Ad = 4;

        public const string Conversion_ActionType_Default = "offsite_conversion";
        public const string Conversion_ActionType_MobileAppInstall = "mobile_app_install";
        public const string Conversion_ActionType_Purchase = "offsite_conversion.fb_pixel_purchase";
        public const string Conversion_ActionType_Registration = "offsite_conversion.fb_pixel_complete_registration";
        public string Conversion_ActionType = Conversion_ActionType_Default;

        public bool IncludeAllActions = false;

        private string PlatformFilter = null;
        public void SetFacebook() { PlatformFilter = "facebook"; }
        public void SetInstagram() { PlatformFilter = "instagram"; }
        public void SetAudienceNetwork() { PlatformFilter = "audience_network"; }
        public void SetAll() { PlatformFilter = null; }

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
        public IEnumerable<FBSummary> GetDailyCampaignStats2(string accountId, DateTime start, DateTime end)
        {
            int daysPerCall = DaysPerCall_Campaign;
            while (start <= end)
            {
                var tempEnd = start.AddDays(daysPerCall - 1);
                if (tempEnd > end)
                    tempEnd = end;
                var fbSummaries = GetFBSummaries(accountId, start, tempEnd, byCampaign: true);

                //NOTE: Forgot why this grouping was done (instead of using GetFBSummariesLoop).
                //      Apparently there could be two campaigns with the same name and we want to display them as one.

                var groups = fbSummaries.GroupBy(s => new { s.Date, s.CampaignName });
                foreach (var group in groups)
                {
                    var fbSum = new FBSummary
                    {
                        Date = group.Key.Date,
                        CampaignName = group.Key.CampaignName,
                        Spend = group.Sum(g => g.Spend),
                        Impressions = group.Sum(g => g.Impressions),
                        UniqueClicks = group.Sum(g => g.UniqueClicks),
                        LinkClicks = group.Sum(g => g.LinkClicks),
                        Conversions_28d_click = group.Sum(g => g.Conversions_28d_click),
                        Conversions_1d_view = group.Sum(g => g.Conversions_1d_view),
                        TotalActions = group.Sum(g => g.TotalActions),
                        ConVal_28d_click = group.Sum(g => g.ConVal_28d_click),
                        ConVal_1d_view = group.Sum(g => g.ConVal_1d_view)
                    };
                    yield return fbSum;
                }
                start = start.AddDays(daysPerCall);
            }
        }
        public IEnumerable<FBSummary> GetDailyAdSetStats(string accountId, DateTime start, DateTime end)
        {
            return GetFBSummariesLoop(accountId, start, end, byCampaign: true, byAdSet: true);
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

                fbSummaries = RemoveIds(fbSummaries);

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
                        Conversions_28d_click = group.Sum(g => g.Conversions_28d_click),
                        Conversions_1d_view = group.Sum(g => g.Conversions_1d_view),
                        TotalActions = group.Sum(g => g.TotalActions),
                        AdId = group.First().AdId
                    };
                    yield return fbSum;
                }
                start = start.AddDays(daysPerCall);
            }
        }
        public static IEnumerable<FBSummary> RemoveIds(IEnumerable<FBSummary> fbSummaries)
        {
            foreach (var fbSum in fbSummaries)
            {
                fbSum.AdName = Regex.Replace(fbSum.AdName, Pattern_ParenNums, "");
                yield return fbSum;
            }
        }

        public IEnumerable<FBSummary> GetFBSummariesLoop(string accountId, DateTime start, DateTime end, bool byCampaign = false, bool byAdSet = false, bool byAd = false)
        {
            int daysPerCall = 365; // default
            if (byCampaign)
                daysPerCall = DaysPerCall_Campaign;
            if (byAdSet)
                daysPerCall = DaysPerCall_AdSet;
            if (byAd)
                daysPerCall = DaysPerCall_Ad;

            while (start <= end)
            {
                var tempEnd = start.AddDays(daysPerCall - 1);
                if (tempEnd > end)
                    tempEnd = end;
                var fbSummaries = GetFBSummaries(accountId, start, tempEnd, byCampaign: byCampaign, byAdSet: byAdSet, byAd: byAd);
                foreach (var fbSum in fbSummaries)
                {
                    yield return fbSum;
                }
                start = start.AddDays(daysPerCall);
            }
        }
        public IEnumerable<FBSummary> GetFBSummaries(string accountId, DateTime start, DateTime end, bool byCampaign = false, bool byAdSet = false, bool byAd = false)
        {
            string by = byCampaign ? " by Campaign" : "";
            by += byAdSet ? " by AdSet" : "";
            by += byAd ? " by Ad" : "";
            LogInfo(string.Format("GetFBSummaries {0:d} - {1:d} ({2}{3})", start, end, accountId, by));

            var fbClient = CreateFBClient();
            var path = accountId + "/insights";

            var levelVal = "";
            var fieldsVal = "impressions,unique_clicks,inline_link_clicks,actions,action_values,total_actions,spend";
            if (byCampaign)
            {
                levelVal = "campaign";
                fieldsVal += ",campaign_id,campaign_name";
            }
            if (byAdSet)
            {
                levelVal = "adset";
                fieldsVal += ",adset_id,adset_name";
            }
            if (byAd)
            {
                levelVal = "ad";
                fieldsVal += ",ad_id,ad_name";
            }
            var afterVal = "";

            var filterVal = new[] { new { field = "publisher_platform", @operator = "IN", value = new[] { PlatformFilter } } };
            if (String.IsNullOrWhiteSpace(PlatformFilter))
                filterVal = null;

            bool moreData;
            do
            {
                moreData = false;
                var parms = new
                {
                    //filtering = new[] { new { field = "campaign.name", @operator = "EQUAL", value = "DA | Mobile App Installs (Android)" } },
                    //metadata = 1,
                    filtering = filterVal,
                    level = levelVal,
                    fields = fieldsVal,
                    action_breakdowns = "action_type", //,action_reaction
                    action_attribution_windows = "28d_click,1d_view",
                    time_range = new { since = DateString(start), until = DateString(end) },
                    time_increment = 1,
                    after = afterVal
                };
                dynamic retObj = null;
                int tryNumber = 0;
                int MAX_TRIES = 10;
                bool abort = false;
                do
                {
                    try
                    {
                        retObj = fbClient.Get(path, parms);
                        tryNumber = 0; // Mark as call succeeded (no exception)
                    }
                    catch (Exception ex)
                    {
                        LogError(ex.Message);
                        if (ex.Message.Contains("OAuthException"))
                            abort = true;
                        else
                        {
                            tryNumber++;
                            if (tryNumber < MAX_TRIES)
                            {
                                LogInfo("Waiting 90 seconds before trying again.");
                                Thread.Sleep(90000);
                            }
                        }
                    }
                } while (tryNumber > 0 && tryNumber < MAX_TRIES && !abort);
                if (abort)
                    throw new Exception("Aborting GetFBSummaries.");
                if (tryNumber >= MAX_TRIES)
                    throw new Exception(String.Format("Tried {0} times. Aborting GetFBSummaries.", tryNumber));

                if (retObj == null)
                    continue;

                decimal decParseVal;
                int intParseVal;
                if (retObj.data != null)
                {
                    foreach (var row in retObj.data)
                    {
                        var fbSum = new FBSummary
                        {
                            Date = DateTime.Parse(row.date_start),
                            //Spend = Decimal.Parse(row.spend),
                            //Impressions = int.Parse(row.impressions),
                            //UniqueClicks = int.Parse(row.unique_clicks),
                            //LinkClicks = int.Parse(row.inline_link_clicks),
                            //TotalActions = int.Parse(row.total_actions),
                            CampaignId = row.campaign_id,
                            CampaignName = row.campaign_name,
                            AdSetId = row.adset_id,
                            AdSetName = row.adset_name,
                            AdId = row.ad_id,
                            AdName = row.ad_name
                        };
                        if (Decimal.TryParse(row.spend, out decParseVal))
                            fbSum.Spend = decParseVal;
                        if (Int32.TryParse(row.impressions, out intParseVal))
                            fbSum.Impressions = intParseVal;
                        if (Int32.TryParse(row.unique_clicks, out intParseVal))
                            fbSum.UniqueClicks = intParseVal;
                        if (Int32.TryParse(row.inline_link_clicks, out intParseVal))
                            fbSum.LinkClicks = intParseVal;
                        if (Int32.TryParse(row.total_actions, out intParseVal))
                            fbSum.TotalActions = intParseVal;

                        var actionStats = row.actions;
                        if (actionStats != null)
                        {
                            if (IncludeAllActions)
                            {
                                fbSum.Actions = new List<FBAction>();
                                foreach (var stat in actionStats)
                                {
                                    var action = new FBAction
                                    {
                                        ActionType = stat.action_type
                                    };
                                    if (((IDictionary<String, object>)stat).ContainsKey("28d_click"))
                                        action.Num_28d_click = int.Parse(stat["28d_click"]);
                                    if (((IDictionary<String, object>)stat).ContainsKey("1d_view"))
                                        action.Num_1d_view = int.Parse(stat["1d_view"]);
                                    fbSum.Actions.Add(action);

                                    if (stat.action_type == Conversion_ActionType) // for backward compatibility
                                    {
                                        if (action.Num_28d_click.HasValue)
                                            fbSum.Conversions_28d_click = action.Num_28d_click.Value;
                                        if (action.Num_1d_view.HasValue)
                                            fbSum.Conversions_1d_view = action.Num_1d_view.Value;
                                    }
                                }
                            }
                            else
                            {
                                foreach (var stat in actionStats)
                                {
                                    if (stat.action_type == Conversion_ActionType)
                                    {
                                        if (((IDictionary<String, object>)stat).ContainsKey("28d_click"))
                                            fbSum.Conversions_28d_click = int.Parse(stat["28d_click"]);
                                        if (((IDictionary<String, object>)stat).ContainsKey("1d_view"))
                                            fbSum.Conversions_1d_view = int.Parse(stat["1d_view"]);
                                        break;
                                    }
                                }
                            }
                        }
                        var actionVals = row.action_values;
                        if (actionVals != null)
                        {
                            foreach (var entity in actionVals)
                            {
                                if (entity.action_type == Conversion_ActionType)
                                {
                                    if (((IDictionary<String, object>)entity).ContainsKey("28d_click"))
                                        fbSum.ConVal_28d_click = decimal.Parse(entity["28d_click"]);
                                    if (((IDictionary<String, object>)entity).ContainsKey("1d_view"))
                                        fbSum.ConVal_1d_view = decimal.Parse(entity["1d_view"]);
                                }
                            }
                        }
                        yield return fbSum;
                    }
                }
                moreData = (retObj.paging != null && retObj.paging.next != null);
                if (moreData)
                    afterVal = retObj.paging.cursors.after;
            } while (moreData);
        }

        //public IEnumerable<FBAdPreview> GetAdPreviews(string accountId, IEnumerable<string> fbAdIds)
        public IEnumerable<FBAdPreview> GetAdPreviews(IEnumerable<string> fbAdIds)
        {
            //TODO: create FacebookClient once, here. Then pass in to GetAdPreviewsAPI().

            foreach (var adId in fbAdIds)
            {
                var fbAdPreviews = GetAdPreviewsAPI(adId);

                foreach (var fbAdPreview in fbAdPreviews)
                {
                    yield return fbAdPreview;
                }
            }
        }
        
        // Reference: https://developers.facebook.com/docs/marketing-api/generatepreview/v2.6
        public IEnumerable<FBAdPreview> GetAdPreviewsAPI(string adId)
        {
            LogInfo(string.Format("GetAdPreviews (adId: {0})", adId));

            var fbClient = CreateFBClient();
            var path = adId + "/previews";

            var afterVal = "";
            bool moreData;
            do
            {
                moreData = false;
                var parms = new
                {
                    ad_format = "DESKTOP_FEED_STANDARD",
                    after = afterVal
                };
                dynamic retObj = null;
                int tryNumber = 0;
                do
                {
                    try
                    {
                        retObj = fbClient.Get(path, parms);
                        tryNumber = 0; // Mark as call succeeded (no exception)
                    }
                    catch (Exception ex)
                    {
                        LogError(ex.Message);
                        tryNumber++;
                        if (tryNumber < 10)
                        {
                            LogInfo("Waiting 90 seconds before trying again.");
                            Thread.Sleep(90000);
                        }
                    }
                } while (tryNumber > 0 && tryNumber < 10);
                if (tryNumber >= 10)
                    throw new Exception("Tried 10 times. Throwing exception.");

                if (retObj == null)
                    continue;

                if (retObj.data != null)
                {
                    foreach (var row in retObj.data)
                    {
                        var fbAdPreview = new FBAdPreview
                        {
                            AdId = adId,
                            BodyHTML = System.Net.WebUtility.HtmlDecode(row.body)
                        };
                        yield return fbAdPreview;
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
