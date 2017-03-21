using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Etl.TradingDesk.LoadersDA;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using FacebookAPI;
using FacebookAPI.Entities;

namespace CakeExtracter.Etl.SocialMarketing.LoadersDA
{
    public class FacebookAdSetSummaryLoader : Loader<FBSummary>
    {
        private TDAdSetSummaryLoader adsetSummaryLoader;

        public FacebookAdSetSummaryLoader(int accountId)
        {
            this.adsetSummaryLoader = new TDAdSetSummaryLoader(accountId);
        }

        protected override int Load(List<FBSummary> items)
        {
            var dbItems = items.Select(i => CreateAdSetSummary(i)).ToList();
            adsetSummaryLoader.AddUpdateDependentStrategies(dbItems);
            adsetSummaryLoader.AddUpdateDependentAdSets(dbItems);
            adsetSummaryLoader.AssignAdSetIdToItems(dbItems);
            var count = adsetSummaryLoader.UpsertDailySummaries(dbItems);
            return count;
        }

        public static AdSetSummary CreateAdSetSummary(FBSummary item)
        {
            var sum = new AdSetSummary
            {
                Date = item.Date,
                AdSetName = item.AdSetName,
                AdSetEid = item.AdSetId,
                StrategyName = item.CampaignName,
                StrategyEid = item.CampaignId,
                Impressions = item.Impressions,
                //Clicks = item.UniqueClicks,
                Clicks = item.LinkClicks,
                PostClickConv = item.Conversions_28d_click,
                PostViewConv = item.Conversions_1d_view,
                PostClickRev = item.ConVal_28d_click,
                PostViewRev = item.ConVal_1d_view,
                Cost = item.Spend
            };
            return sum;
        }
    }
}
