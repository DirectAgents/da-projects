using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Commands;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Facebook;
using DirectAgents.Domain.Entities.CPProg.Facebook.AdSet;
using DirectAgents.Domain.Entities.CPProg.Facebook.Campaign;
using FacebookAPI.Entities;
using FacebookAPI.Providers;

namespace CakeExtracter.Etl.Facebook.Extractors
{
    /// <inheritdoc />
    /// <summary>
    /// Facebook Adset summary extractor.
    /// </summary>
    public class FacebookAdSetSummaryExtractor : FacebookApiExtractor<FbAdSetSummary, FacebookInsightsDataProvider>
    {
        /// <inheritdoc cref="FacebookApiExtractor{T,TProvider}"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookAdSetSummaryExtractor"/> class.
        /// </summary>
        /// <param name="dateRange">The date range.</param>
        /// <param name="account">The account.</param>
        /// <param name="fbUtility">The fb utility.</param>
        public FacebookAdSetSummaryExtractor(DateRange dateRange, ExtAccount account, FacebookInsightsDataProvider fbUtility)
            : base(fbUtility, dateRange, account)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// The derived class implements this method, which calls Add() for each item
        /// extracted and then calls End() when complete.
        /// </summary>
        protected override void Extract()
        {
            Logger.Info(
                AccountId,
                $"Extracting AdSet Summaries from Facebook API for ({FbAccountId}) from {DateRange?.FromDate:d} to {DateRange?.ToDate:d}");
            try
            {
                var fbSums = FbUtility.GetDailyAdSetStats("act_" + FbAccountId, DateRange.Value.FromDate, DateRange.Value.ToDate);
                var fbAdSetSummaryItems = fbSums.Select(CreateFbAdsetSummary);
                Add(fbAdSetSummaryItems);
            }
            catch (Exception ex)
            {
                OnProcessFailedExtraction(
                    this.DateRange?.FromDate,
                    this.DateRange?.ToDate,
                    this.AccountId,
                    FbStatsTypeAgg.AdSetArg,
                    ex);
                Logger.Error(AccountId, ex);
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
                    AccountId = AccountId,
                    Campaign = new FbCampaign
                    {
                        AccountId = AccountId,
                        Name = item.CampaignName,
                        ExternalId = item.CampaignId,
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
                Actions = GetActions(item),
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
                    PostViewVal = fbAction.Val_view ?? 0,
                }).ToList();
            }
            return actions;
        }
    }
}
