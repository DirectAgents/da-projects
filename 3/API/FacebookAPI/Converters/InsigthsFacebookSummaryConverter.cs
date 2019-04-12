using FacebookAPI.Entities;
using System;
using System.Collections.Generic;

namespace FacebookAPI.Converters
{
    internal class InsigthsFacebookSummaryConverter
    {
        bool includeAllActions;
        string conversionActionType;
        string clickAttribution;
        string viewAttribution;

        public InsigthsFacebookSummaryConverter(bool includeAllActions, string conversionActionType, 
            string clickAttribution, string viewAttribution)
        {
        }

        public FBSummary GetFacebookSummaryFromRow(dynamic row)
        {
            decimal decParseVal;
            int intParseVal;
            var fbSum = new FBSummary
            {
                Date = DateTime.Parse(row.date_start),
                CampaignId = row.campaign_id,
                CampaignName = row.campaign_name,
                AdSetId = row.adset_id,
                AdSetName = row.adset_name,
                AdId = row.ad_id,
                AdName = row.ad_name
            };
            if (decimal.TryParse(row.spend, out decParseVal))
                fbSum.Spend = decParseVal;
            if (int.TryParse(row.impressions, out intParseVal))
                fbSum.Impressions = intParseVal;
            if (int.TryParse(row.inline_link_clicks, out intParseVal))
                fbSum.LinkClicks = intParseVal;
            if (int.TryParse(row.clicks, out intParseVal))
                fbSum.AllClicks = intParseVal;
            //if (Int32.TryParse(row.unique_clicks, out intParseVal))
            //    fbSum.UniqueClicks = intParseVal;
            //if (Int32.TryParse(row.total_actions, out intParseVal))
            //    fbSum.TotalActions = intParseVal;

            var actionStats = row.actions;
            var actionVals = row.action_values;
            var videoStats1 = row.video_10_sec_watched_actions;
            var videoStats2 = row.video_p100_watched_actions;
            if (includeAllActions && (actionStats != null || actionVals != null || videoStats1 != null || videoStats2 != null))
                fbSum.Actions = new Dictionary<string, FBAction>();

            if (includeAllActions)
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
                    if (!includeAllActions && stat.action_type != conversionActionType)
                        continue;

                    var action = new FBAction { ActionType = stat.action_type };
                    SetNum_ClickView(action, stat);
                    if (includeAllActions)
                        fbSum.Actions[action.ActionType] = action;

                    if (stat.action_type == conversionActionType)
                    {
                        fbSum.SetNumConvsFromAction(action); // for backward compatibility
                        if (!includeAllActions)
                            break;
                    }
                }
            }
            if (actionVals != null)
            {
                if (includeAllActions)
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
            return fbSum;
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
    }
}
