using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Facebook;
using DirectAgents.Domain.Entities.CPProg.Facebook.Ad;
using DirectAgents.Domain.Entities.CPProg.Facebook.AdSet;
using DirectAgents.Domain.Entities.CPProg.Facebook.Campaign;
using FacebookAPI;
using FacebookAPI.Entities;

namespace CakeExtracter.Etl.SocialMarketing.Extracters
{
    public class FacebookAdSummaryExtracter : FacebookApiExtracter<FbAdSummary>
    {
        private readonly FacebookAdMetadataProvider fbAdMetadataProvider;

        public FacebookAdSummaryExtracter(DateRange dateRange, ExtAccount account, FacebookInsightsDataProvider fbUtility,
            FacebookAdMetadataProvider fbAdMetadataProvider) : base(fbUtility, dateRange, account)
        {
            this.fbAdMetadataProvider = fbAdMetadataProvider;
        }

        protected override void Extract()
        {
            Logger.Info(accountId, "Extracting Ad Summaries from Facebook API for ({0}) from {1:d} to {2:d}",
                        this.fbAccountId, this.dateRange.Value.FromDate, this.dateRange.Value.ToDate);
            try
            {
                var fbSums = _fbUtility.GetDailyAdStats("act_" + fbAccountId, dateRange.Value.FromDate, dateRange.Value.ToDate);
                Logger.Info(accountId, "Started reading ad's metadata");
                var allAdsMetadata = fbAdMetadataProvider.GetAllAdsDataForAccount("act_" + fbAccountId, dateRange.Value.FromDate, dateRange.Value.ToDate);
                Logger.Info(accountId, "Finished reading ad's metadata");
                var fbAdSummaryItems = fbSums.Select(item => { return CreateTDadSummary(item, allAdsMetadata); });
                Add(fbAdSummaryItems);
            }
            catch (Exception ex)
            {
                Logger.Error(accountId, ex);
            }
            End();
        }

        private FbAdSummary CreateTDadSummary(FBSummary item, List<AdData> adMetadata)
        {
            var sum = new FbAdSummary
            {
                Date = item.Date,
                Ad = new FbAd
                {
                    Name = item.AdName,
                    ExternalId = item.AdId,
                    AccountId = accountId,
                    Campaign = new FbCampaign
                    {
                        AccountId = accountId,
                        Name = item.CampaignName,
                        ExternalId = item.CampaignId
                    },
                    AdSet = new FbAdSet
                    {
                        AccountId = accountId,
                        Name = item.AdSetName,
                        ExternalId = item.AdSetId
                    },
                    Creative = GetRelatedCreativeData(item, adMetadata),
                },
                Impressions = item.Impressions,
                AllClicks = item.AllClicks,
                Clicks = item.LinkClicks,
                PostClickConv = item.Conversions_click,
                PostViewConv = item.Conversions_view,
                Cost = item.Spend,
                Actions = GetActions(item)
            };
            sum.Ad.AdSet.Campaign = sum.Ad.Campaign;
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

        private FbCreative GetRelatedCreativeData(FBSummary item, List<AdData> allAdsMetadata)
        {
            var relatedAdMetadata = allAdsMetadata.FirstOrDefault(ad => ad.Id == item.AdId);
            if (relatedAdMetadata != null)
            {
                return new FbCreative
                {
                    AccountId = accountId,
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
