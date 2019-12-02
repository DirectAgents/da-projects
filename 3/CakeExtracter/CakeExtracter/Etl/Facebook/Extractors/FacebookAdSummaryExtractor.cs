using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Facebook;
using DirectAgents.Domain.Entities.CPProg.Facebook.Ad;
using DirectAgents.Domain.Entities.CPProg.Facebook.AdSet;
using DirectAgents.Domain.Entities.CPProg.Facebook.Campaign;
using FacebookAPI.Entities;
using FacebookAPI.Entities.AdDataEntities;
using FacebookAPI.Providers;

namespace CakeExtracter.Etl.Facebook.Extractors
{
    /// <summary>
    /// Facebook ads summary extractor.
    /// </summary>
    public class FacebookAdSummaryExtractor : FacebookApiExtractor<FbAdSummary, FacebookInsightsDataProvider>
    {
        private readonly List<AdCreativeData> allAdsMetadata;

        /// <inheritdoc cref="FacebookApiExtractor{T,TProvider}"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookAdSummaryExtractor" /> class.
        /// </summary>
        /// <param name="dateRange">The date range.</param>
        /// <param name="account">The account.</param>
        /// <param name="fbUtility">The fb utility.</param>
        /// <param name="allAdsMetadata">All ads metadata.</param>
        public FacebookAdSummaryExtractor(
            DateRange dateRange, ExtAccount account, FacebookInsightsDataProvider fbUtility, List<AdCreativeData> allAdsMetadata)
            : base(fbUtility, dateRange, account)
        {
            this.allAdsMetadata = allAdsMetadata;
        }

        /// <inheritdoc />
        /// <summary>
        /// The derived class implements this method, which calls Add() for each item
        /// extracted and then calls End() when complete.
        /// </summary>
        protected override void Extract()
        {
            Logger.Info(AccountId, "Extracting Ad Summaries from Facebook API for ({0}) from {1:d} to {2:d}",
                        this.FbAccountId, this.DateRange.Value.FromDate, this.DateRange.Value.ToDate);
            try
            {
                var fbSums = FbUtility.GetDailyAdStats("act_" + FbAccountId, DateRange.Value.FromDate, DateRange.Value.ToDate);
                var fbAdSummaryItems = fbSums.Select(item => CreateFbAdSummary(item, allAdsMetadata));
                Add(fbAdSummaryItems);
            }
            catch (Exception ex)
            {
                Logger.Error(AccountId, ex);
            }
            End();
        }

        private FbAdSummary CreateFbAdSummary(FBSummary item, List<AdCreativeData> allAdMetadata)
        {
            var relatedAdMetadata = allAdMetadata.FirstOrDefault(ad => ad.Id == item.AdId);
            var sum = new FbAdSummary
            {
                Date = item.Date,
                Ad = new FbAd
                {
                    Name = item.AdName,
                    Status = relatedAdMetadata?.Status,
                    ExternalId = item.AdId,
                    AccountId = AccountId,
                    AdSet = new FbAdSet
                    {
                        AccountId = AccountId,
                        Name = item.AdSetName,
                        ExternalId = item.AdSetId,
                        Campaign = new FbCampaign
                        {
                            AccountId = AccountId,
                            Name = item.CampaignName,
                            ExternalId = item.CampaignId
                        },
                    },
                    Creative = GetRelatedCreativeData(item, relatedAdMetadata),
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

        private List<FbAdAction> GetActions(FBSummary summaryItem)
        {
            var actions = new List<FbAdAction>();
            if (summaryItem.Actions != null)
            {
                actions = summaryItem.Actions.Values.Select(fbAction => new FbAdAction
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

        private FbCreative GetRelatedCreativeData(FBSummary item, AdCreativeData relatedAdMetadata)
        {
            if (relatedAdMetadata != null)
            {
                return new FbCreative
                {
                    AccountId = AccountId,
                    Body = relatedAdMetadata.Creative.Body,
                    ExternalId = relatedAdMetadata.Creative.Id,
                    Title = relatedAdMetadata.Creative.Title,
                    ImageUrl = relatedAdMetadata.Creative.ImageUrl,
                    ThumbnailUrl = relatedAdMetadata.Creative.ThumbnailUrl,
                    Name = relatedAdMetadata.Creative.Name
                };
            }
            return null;
        }
    }
}
