using FacebookAPI.Entities;
using System;
using System.Collections.Generic;

namespace FacebookAPI.Converters
{
    internal abstract class FacebookSummaryConverter
    {
        string conversionActionType;
        string clickAttribution;
        string viewAttribution;

        public FacebookSummaryConverter(string conversionActionType,
            string clickAttribution, string viewAttribution)
        {
            this.conversionActionType = conversionActionType;
            this.clickAttribution = clickAttribution;
            this.viewAttribution = viewAttribution;
        }

        public abstract FBSummary ParseSummaryRow(dynamic row);

        protected FBSummary GetFacebokSummaryMetricsFromRow(dynamic row)
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
            return fbSum;
        }

        protected void ProcessAllActions(dynamic row, FBSummary fbSum)
        {
            var actionStats = row.actions;
            var actionVals = row.action_values;
            var videoStats1 = row.video_10_sec_watched_actions;
            var videoStats2 = row.video_p100_watched_actions;
            fbSum.Actions = new Dictionary<string, FBAction>();
            if (videoStats1 != null)
            {
                foreach (var videoStat in videoStats1)
                {
                    var action = new FBAction
                    {
                        ActionType = "video_10_sec_watched",
                        ClickAttrWindow = clickAttribution,
                        ViewAttrWindow = viewAttribution
                    };
                    SetNum_ClickView(action, videoStat);
                    fbSum.Actions[action.ActionType] = action;
                    break; // should be just one videoStat
                }
            }
            if (videoStats2 != null)
            {
                foreach (var videoStat in videoStats2)
                {
                    var action = new FBAction
                    {
                        ActionType = "video_p100_watched",
                        ClickAttrWindow = clickAttribution,
                        ViewAttrWindow = viewAttribution
                    };
                    SetNum_ClickView(action, videoStat);
                    fbSum.Actions[action.ActionType] = action;
                    break; // should be just one videoStat
                }
            }
            if (actionStats != null)
            {
                foreach (var stat in actionStats)
                {
                    var action = new FBAction
                    {
                        ActionType = stat.action_type,
                        ClickAttrWindow = clickAttribution,
                        ViewAttrWindow = viewAttribution
                    };
                    SetNum_ClickView(action, stat);
                    fbSum.Actions[action.ActionType] = action;
                    if (stat.action_type == conversionActionType)
                    {
                        fbSum.SetNumConvsFromAction(action); // for backward compatibility
                    }
                }
            }
            if (actionVals != null)
            {
                foreach (var stat in actionVals)
                {
                    if (!fbSum.Actions.ContainsKey(stat.action_type))
                        fbSum.Actions[stat.action_type] = new FBAction
                        {
                            ActionType = stat.action_type,
                            ClickAttrWindow = clickAttribution,
                            ViewAttrWindow = viewAttribution
                        };
                    FBAction action = fbSum.Actions[stat.action_type];
                    SetVal_ClickView(action, stat);
                    if (stat.action_type == conversionActionType)
                        fbSum.SetConValsFromAction(action); // for backward compatibility
                }
            }
        }

        protected void ProcessConversionValuesActions(dynamic row, FBSummary fbSum)
        {
            var actionStats = row.actions;
            var actionVals = row.action_values;
            fbSum.Actions = new Dictionary<string, FBAction>();
            if (actionStats != null)
            {
                foreach (var stat in actionStats)
                {
                    if (stat.action_type == conversionActionType)
                    {
                        var action = new FBAction
                        {
                            ActionType = stat.action_type,
                            ClickAttrWindow = clickAttribution,
                            ViewAttrWindow = viewAttribution
                        };
                        SetNum_ClickView(action, stat);
                        fbSum.SetNumConvsFromAction(action); // for backward compatibility
                        break;
                    }
                }
            }
            if (actionVals != null)
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

        protected void SetNum_ClickView(FBAction action, dynamic stat)
        {
            if (((IDictionary<String, object>)stat).ContainsKey(clickAttribution))
                action.Num_click = int.Parse(stat[clickAttribution]);
            if (((IDictionary<String, object>)stat).ContainsKey(viewAttribution))
                action.Num_view = int.Parse(stat[viewAttribution]);
        }

        protected void SetVal_ClickView(FBAction action, dynamic stat)
        {
            if (((IDictionary<String, object>)stat).ContainsKey(clickAttribution))
                action.Val_click = decimal.Parse(stat[clickAttribution]);
            if (((IDictionary<String, object>)stat).ContainsKey(viewAttribution))
                action.Val_view = decimal.Parse(stat[viewAttribution]);
        }
    }
}
