using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Facebook;
using DirectAgents.Domain.Entities.CPProg.Facebook.AdSet;
using DirectAgents.Domain.Entities.CPProg.Facebook.Campaign;
using FacebookAPI;
using FacebookAPI.Entities;

namespace CakeExtracter.Etl.SocialMarketing.Extracters.V2
{
    /// <summary>
    /// Facebook Adset summary extractor.
    /// </summary>
    /// <seealso cref="CakeExtracter.Etl.SocialMarketing.Extracters.FacebookApiExtracter{DirectAgents.Domain.Entities.CPProg.Facebook.AdSet.FbAdSetSummary}" />
    public class FacebookAdSetSummaryExtracterV2 : FacebookApiExtracterV2<FbAdSetSummary>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookAdSetSummaryExtracter"/> class.
        /// </summary>
        /// <param name="dateRange">The date range.</param>
        /// <param name="account">The account.</param>
        /// <param name="fbUtility">The fb utility.</param>
        public FacebookAdSetSummaryExtracterV2(DateRange dateRange, ExtAccount account, FacebookInsightsDataProvider fbUtility)
            : base(fbUtility, dateRange, account)
        { }

        /// <summary>
        /// The derived class implements this method, which calls Add() for each item
        /// extracted and then calls End() when complete.
        /// </summary>
        protected override void Extract()
        {
            Logger.Info(accountId, "Extracting AdSet Summaries from Facebook API for ({0}) from {1:d} to {2:d}",
                        this.fbAccountId, this.dateRange.Value.FromDate, this.dateRange.Value.ToDate);
            try
            {
                var fbSums = _fbUtility.GetDailyAdSetStats("act_" + fbAccountId, dateRange.Value.FromDate, dateRange.Value.ToDate);
                var fbAdSetSummaryItems = fbSums.Select(CreateFbAdsetSummary);
                Add(fbAdSetSummaryItems);
            }
            catch (Exception ex)
            {
                Logger.Error(accountId, ex);
            }
            End();
        }

        private FbAdSetSummary CreateFbAdsetSummary(FBSummary item)
        {
            var sum = new FbAdSetSummary
            {
                Date = item.Date,
                AdSet = new FbAdSet
                {
                    Name = item.AdSetName,
                    ExternalId = item.AdSetId,
                    AccountId = accountId,
                    Campaign = new FbCampaign
                    {
                        AccountId = accountId,
                        Name = item.CampaignName,
                        ExternalId = item.CampaignId
                    },
                },
                Impressions = item.Impressions,
                AllClicks = item.AllClicks,
                Clicks = item.LinkClicks,
                PostClickConv = item.Conversions_click,
                PostViewConv = item.Conversions_view,
                PostClickRev = item.ConVal_click,
                PostViewRev = item.ConVal_view,
                Cost = item.Spend,
                Actions = GetActions(item)
            };
            return sum;
        }

        private List<FbAdSetAction> GetActions(FBSummary summaryItem)
        {
            var actions = new List<FbAdSetAction>();
            if (summaryItem.Actions != null)
            {
                actions = summaryItem.Actions.Values.Select(fbAction => new FbAdSetAction
                {
                    ActionType = new FbActionType
                    {
                        Code = fbAction.ActionType,
                    },
                    ClickAttrWindow = fbAction.ClickAttrWindow,
                    ViewAttrWindow = fbAction.ViewAttrWindow,
                    Date = summaryItem.Date,
                    PostClick = fbAction.Num_click ?? 0,
                    PostView = fbAction.Num_view ?? 0,
                    PostClickVal = fbAction.Val_click ?? 0,
                    PostViewVal = fbAction.Val_view ?? 0
                }).ToList();
            }
            return actions;
        }
    }
}
