using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Facebook;
using FacebookAPI.Entities;
using FacebookAPI.Enums;

namespace FacebookAPI
{
    public class FacebookUtility
    {
        public const int RowsReturnedAtATime = 25;
        public const string Pattern_ParenNums = @"^\((\d+)\)\s*";
        public const int InitialWaitMillisecs = 1500;
        public const int MaxRetries = 20; //??reduce??
        public const int SecondsToWaitIfLimitReached = 61;

        private static readonly Dictionary<PlatformFilter, string> PlatformFilterNames = new Dictionary<PlatformFilter, string>
        {
            {PlatformFilter.Facebook, "facebook"},
            {PlatformFilter.Instagram, "instagram"},
            {PlatformFilter.Audience, "audience_network"},
            {PlatformFilter.Messenger, "messenger"},
            {PlatformFilter.All, null}
        };

        //TODO: type MobileAppPurchase - app_custom_event.fb_mobile_purchase (for FUNimation)
        private static readonly Dictionary<ConversionActionType, string> ActionTypeNames = new Dictionary<ConversionActionType, string>
        {
            {ConversionActionType.MobileAppInstall, "mobile_app_install"},
            {ConversionActionType.Purchase, "offsite_conversion.fb_pixel_purchase"},
            {ConversionActionType.Registration, "offsite_conversion.fb_pixel_complete_registration"},
            {ConversionActionType.VideoPlay, "video_play"},
            {ConversionActionType.Default, "offsite_conversion"}
        };

        private static readonly Dictionary<Attribution, string> AttributionPostfixNames = new Dictionary<Attribution, string>
        {
            {Attribution.Click, "click"},
            {Attribution.View, "view"}
        };

        public int? DaysPerCall_Override = null;
        public int DaysPerCall_Campaign = 15;
        public int DaysPerCall_AdSet = 7;
        public int DaysPerCall_Ad = 3;

        public bool IncludeAllActions = false;

        private string campaignFilterOperator;
        private string campaignFilterValue;
        private string conversionActionType = ActionTypeNames[ConversionActionType.Default];
        private string platformFilter = PlatformFilterNames[PlatformFilter.All];
        private string clickAttribution = GetAttributionName(Attribution.Click, AttributionWindow.Days28);
        private string viewAttribution = GetAttributionName(Attribution.View, AttributionWindow.Day1);

        public void SetConversionActionType(ConversionActionType actionType)
        {
            conversionActionType = ActionTypeNames[actionType];
        }

        public void SetPlatformFilter(PlatformFilter filter)
        {
            platformFilter = PlatformFilterNames[filter];
        }

        public void SetCampaignFilter(string filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                campaignFilterValue = null;
            }
            else
            {
                campaignFilterValue = filter;
                campaignFilterOperator = "CONTAIN"; // for now; later, could implement NOT_CONTAIN, etc
            }
        }

        public void SetClickAttributionWindow(AttributionWindow window)
        {
            clickAttribution = GetAttributionName(Attribution.Click, window);
        }

        public void SetViewAttributionWindow(AttributionWindow window)
        {
            viewAttribution = GetAttributionName(Attribution.View, window);
        }

        private static string GetAttributionName(Attribution attribution, AttributionWindow window)
        {
            return $"{(int) window}d_{AttributionPostfixNames[attribution]}";
        }

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
            var fbClient = new FacebookClient(AccessToken) {Version = "v" + ApiVersion};
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
            int daysPerCall = DaysPerCall_Override ?? DaysPerCall_Campaign;
            while (start <= end)
            {
                var tempEnd = start.AddDays(daysPerCall - 1);
                if (tempEnd > end)
                    tempEnd = end;
                var clientParms = GetClientAndParms(accountId, start, tempEnd, byCampaign: true);
                var fbSummaries = GetFBSummaries(clientParms);

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
                        LinkClicks = group.Sum(g => g.LinkClicks),
                        AllClicks = group.Sum(g => g.AllClicks),
                        //UniqueClicks = group.Sum(g => g.UniqueClicks),
                        //TotalActions = group.Sum(g => g.TotalActions),
                        Conversions_click = group.Sum(g => g.Conversions_click),
                        Conversions_view = group.Sum(g => g.Conversions_view),
                        ConVal_click = group.Sum(g => g.ConVal_click),
                        ConVal_view = group.Sum(g => g.ConVal_view)
                    };
                    yield return fbSum;
                }
                start = start.AddDays(daysPerCall);
            }
        }
        public IEnumerable<FBSummary> GetDailyAdSetStats(string accountId, DateTime start, DateTime end)
        {
            return GetFBSummariesLoop(accountId, start, end, byCampaign: true, byAdSet: true, getArchived: true);
        }

        public IEnumerable<FBSummary> GetDailyAdStats(string accountId, DateTime start, DateTime end)
        {
            int daysPerCall = DaysPerCall_Override ?? DaysPerCall_Ad;
            var clientParmsList = new List<ClientAndParms>();
            while (start <= end)
            {
                var tempEnd = start.AddDays(daysPerCall - 1);
                if (tempEnd > end)
                    tempEnd = end;

                var clientParms = GetClientAndParms(accountId, start, tempEnd, byAd: true);
                GetRunId_withRetry(clientParms); // fire off the job

                clientParmsList.Add(clientParms);
                start = start.AddDays(daysPerCall);
            }
            Thread.Sleep(InitialWaitMillisecs);

            foreach (var clientParms in clientParmsList)
            {
                var fbSummaries = GetFBSummaries(clientParms);
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
                        LinkClicks = group.Sum(g => g.LinkClicks),
                        AllClicks = group.Sum(g => g.AllClicks),
                        //UniqueClicks = group.Sum(g => g.UniqueClicks),
                        //TotalActions = group.Sum(g => g.TotalActions),
                        Conversions_click = group.Sum(g => g.Conversions_click),
                        Conversions_view = group.Sum(g => g.Conversions_view),
                        AdId = group.First().AdId
                    };
                    yield return fbSum;
                }
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

        public IEnumerable<FBSummary> GetFBSummariesLoop(string accountId, DateTime start, DateTime end, bool byCampaign = false, bool byAdSet = false, bool byAd = false, bool getArchived = false)
        {
            int daysPerCall = 365; // default
            if (DaysPerCall_Override.HasValue)
                daysPerCall = DaysPerCall_Override.Value;
            else
            {
                if (byCampaign)
                    daysPerCall = DaysPerCall_Campaign;
                if (byAdSet)
                    daysPerCall = DaysPerCall_AdSet;
                if (byAd)
                    daysPerCall = DaysPerCall_Ad;
            }

            var clientParmsList = new List<ClientAndParms>();
            while (start <= end)
            {
                var tempEnd = start.AddDays(daysPerCall - 1);
                if (tempEnd > end)
                    tempEnd = end;

                var clientParms = GetClientAndParms(accountId, start, tempEnd, byCampaign: byCampaign, byAdSet: byAdSet, byAd: byAd);
                GetRunId_withRetry(clientParms); // fire off the job
                clientParmsList.Add(clientParms);

                if (getArchived)
                {
                    var clientParmsArchived = GetClientAndParms(accountId, start, tempEnd, byCampaign: byCampaign, byAdSet: byAdSet, byAd: byAd, getArchived: true);
                    GetRunId_withRetry(clientParmsArchived);
                    clientParmsList.Add(clientParmsArchived);
                }

                start = start.AddDays(daysPerCall);
            }
            Thread.Sleep(InitialWaitMillisecs);

            foreach (var clientParms in clientParmsList)
            {
                var fbSummaries = GetFBSummaries(clientParms);
                foreach (var fbSum in fbSummaries)
                {
                    yield return fbSum;
                }
            }
        }

        private ClientAndParms GetClientAndParms(string accountId, DateTime start, DateTime end, bool byCampaign = false, bool byAdSet = false, bool byAd = false, bool getArchived = false)
        {
            var levelVal = "";
            var fieldsVal = "spend,impressions,inline_link_clicks,clicks,actions,action_values"; // unique_clicks,total_actions
            if (IncludeAllActions)
            {
                fieldsVal += ",video_10_sec_watched_actions,video_p100_watched_actions"; // video fields
            }
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

            var filterList = new List<Filter>();
            if (!String.IsNullOrWhiteSpace(platformFilter))
                filterList.Add(new Filter { field = "publisher_platform", @operator = "IN", value = new[] { platformFilter } });
            if (!String.IsNullOrEmpty(campaignFilterValue))
                filterList.Add(new Filter { field = "campaign.name", @operator = campaignFilterOperator, value = campaignFilterValue });

            if (getArchived)
            {
                filterList.Add(new Filter{ field = $"{levelVal}.effective_status", @operator = "IN", value = new[] { "ARCHIVED" } });
            }

            // See https://developers.facebook.com/docs/marketing-api/ad-rules-getting-started/

            var parameters = new
            {
                //filtering = new[] { new { field = "campaign.name", @operator = "EQUAL", value = "DA | Mobile App Installs (Android)" } },
                //metadata = 1,
                filtering = filterList.ToArray(),
                level = levelVal,
                fields = fieldsVal,
                action_breakdowns = "action_type", //,action_reaction
                action_attribution_windows = clickAttribution + "," + viewAttribution, // e.g. "28d_click,1d_view",
                time_range = new { since = DateString(start), until = DateString(end) },
                time_increment = 1,
                //after = afterVal
            };

            string by = byCampaign ? " by Campaign" : "";
            by += byAdSet ? " by AdSet" : "";
            by += byAd ? " by Ad" : "";
            string logMessage = string.Format("GetFBSummaries {0:d} - {1:d} ({2}{3})", start, end, accountId, by);

            return new ClientAndParms
            {
                fbClient = CreateFBClient(),
                path = accountId + "/insights",
                parms = parameters,
                logMessage = logMessage
            };
        }

        private string GetRunId_withRetry(ClientAndParms clientParms)
        {
            int tryNumber = 0;
            string runId = null;
            do
            {
                try
                {
                    runId = clientParms.GetRunId();
                    tryNumber = 0; // mark as call succeeded (no exception)
                }
                //catch (FacebookOAuthException ex)
                catch (Exception ex)
                {
                    LogError(ex.Message);
                    int secondsToWait = 2;
                    if (ex.Message.Contains("request limit") || ex.Message.Contains("rate limit"))
                        secondsToWait = SecondsToWaitIfLimitReached;

                    tryNumber++;
                    if (tryNumber < MaxRetries)
                    {
                        LogInfo(String.Format("Waiting {0} seconds before trying again.", secondsToWait));
                        Thread.Sleep(secondsToWait * 1000);
                    }
                }
            } while (tryNumber > 0 && tryNumber < MaxRetries);
            if (tryNumber >= MaxRetries)
                throw new Exception(String.Format("Tried {0} times. Aborting GetRunId_withRetry.", tryNumber));

            return runId;
        }

        private IEnumerable<FBSummary> GetFBSummaries(ClientAndParms clientParms)
        {
            LogInfo(clientParms.logMessage);

            bool moreData = false;
            var afterVal = ""; // later, used for paging
            do
            {
                dynamic retObj = null;
                int tryNumber = 0;
                do
                {
                    try
                    {
                        var runId = clientParms.GetRunId();
                        if (!moreData) // skip this when getting page 2+
                        {
                            retObj = clientParms.fbClient.Get(runId); // check to see if job complete
                            int numCalls = 1;
                            int waitMillisecs = InitialWaitMillisecs;
                            while (retObj.async_status != "Job Completed" || retObj.async_percent_completion < 100)
                            {
                                waitMillisecs += 500;
                                Thread.Sleep(waitMillisecs);
                                retObj = clientParms.fbClient.Get(runId);
                                numCalls++;
                            }
                            LogInfo(String.Format("{0} call(s) to check if job completed", numCalls));
                        }
                        retObj = clientParms.fbClient.Get(runId + "/insights", new { after = afterVal }); // get the actual data
                        tryNumber = 0; // mark as call succeeded (no exception)
                    }
                    //catch (FacebookOAuthException ex)
                    catch (Exception ex)
                    {
                        LogError(ex.Message);
                        int secondsToWait = 2;
                        if (ex.Message.Contains("request limit") || ex.Message.Contains("rate limit"))
                            secondsToWait = SecondsToWaitIfLimitReached;

                        tryNumber++;
                        if (tryNumber < MaxRetries)
                        {
                            LogInfo(String.Format("Waiting {0} seconds before trying again.", secondsToWait));
                            Thread.Sleep(secondsToWait * 1000);
                        }
                    }
                } while (tryNumber > 0 && tryNumber < MaxRetries);
                if (tryNumber >= MaxRetries)
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
                            //LinkClicks = int.Parse(row.inline_link_clicks),
                            //AllClicks = int.Parse(row.clicks),
                            //UniqueClicks = int.Parse(row.unique_clicks),
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
                        if (Int32.TryParse(row.inline_link_clicks, out intParseVal))
                            fbSum.LinkClicks = intParseVal;
                        if (Int32.TryParse(row.clicks, out intParseVal))
                            fbSum.AllClicks = intParseVal;
                        //if (Int32.TryParse(row.unique_clicks, out intParseVal))
                        //    fbSum.UniqueClicks = intParseVal;
                        //if (Int32.TryParse(row.total_actions, out intParseVal))
                        //    fbSum.TotalActions = intParseVal;

                        var actionStats = row.actions;
                        var actionVals = row.action_values;
                        var videoStats1 = row.video_10_sec_watched_actions;
                        var videoStats2 = row.video_p100_watched_actions;
                        if (IncludeAllActions && (actionStats != null || actionVals != null || videoStats1 != null || videoStats2 != null))
                            fbSum.Actions = new Dictionary<string, FBAction>();

                        if (IncludeAllActions)
                        {
                            if (videoStats1 != null)
                            {
                                foreach (var videoStat in videoStats1)
                                {
                                    var action = new FBAction { ActionType = "video_10_sec_watched" };
                                    SetNum_ClickView(action, videoStat);
                                    fbSum.Actions[action.ActionType] = action;
                                    break; // should be just one videoStat
                                }
                            }
                            if (videoStats2 != null)
                            {
                                foreach (var videoStat in videoStats2)
                                {
                                    var action = new FBAction { ActionType = "video_p100_watched" };
                                    SetNum_ClickView(action, videoStat);
                                    fbSum.Actions[action.ActionType] = action;
                                    break; // should be just one videoStat
                                }
                            }
                        }

                        if (actionStats != null)
                        {
                            foreach (var stat in actionStats)
                            {
                                if (!IncludeAllActions && stat.action_type != conversionActionType)
                                    continue;

                                var action = new FBAction { ActionType = stat.action_type };
                                SetNum_ClickView(action, stat);
                                if (IncludeAllActions)
                                    fbSum.Actions[action.ActionType] = action;

                                if (stat.action_type == conversionActionType)
                                {
                                    fbSum.SetNumConvsFromAction(action); // for backward compatibility
                                    if (!IncludeAllActions)
                                        break;
                                }
                            }
                        }
                        if (actionVals != null)
                        {
                            if (IncludeAllActions)
                            {
                                foreach (var stat in actionVals)
                                {
                                    if (!fbSum.Actions.ContainsKey(stat.action_type))
                                        fbSum.Actions[stat.action_type] = new FBAction { ActionType = stat.action_type };
                                    FBAction action = fbSum.Actions[stat.action_type];
                                    SetVal_ClickView(action, stat);

                                    if (stat.action_type == conversionActionType)
                                        fbSum.SetConValsFromAction(action); // for backward compatibility
                                }
                            }
                            else
                            {
                                foreach (var stat in actionVals)
                                {
                                    if (stat.action_type == conversionActionType)
                                    {
                                        var action = new FBAction(); // temp holding object
                                        SetVal_ClickView(action, stat);
                                        fbSum.SetConValsFromAction(action);
                                        break;
                                    }
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

        private void SetNum_ClickView(FBAction action, dynamic stat)
        {
            if (((IDictionary<String, object>)stat).ContainsKey(clickAttribution))
                action.Num_click = int.Parse(stat[clickAttribution]);
            if (((IDictionary<String, object>)stat).ContainsKey(viewAttribution))
                action.Num_view = int.Parse(stat[viewAttribution]);
        }
        private void SetVal_ClickView(FBAction action, dynamic stat)
        {
            if (((IDictionary<String, object>)stat).ContainsKey(clickAttribution))
                action.Val_click = decimal.Parse(stat[clickAttribution]);
            if (((IDictionary<String, object>)stat).ContainsKey(viewAttribution))
                action.Val_view = decimal.Parse(stat[viewAttribution]);
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

        private class ClientAndParms
        {
            public FacebookClient fbClient;
            public string path;
            public object parms;
            public string logMessage;

            private string runId;
            public string GetRunId(int waitMillisecs = 0)
            {
                if (String.IsNullOrWhiteSpace(runId))
                {
                    dynamic retObj = fbClient.Post(path, parms); // initial async call

                    runId = retObj.report_run_id;
                    Thread.Sleep(waitMillisecs);
                }
                return runId;
            }
        }

        private class Filter
        {
            public string field;
            public string @operator;
            public object value;
        }
    }
}
