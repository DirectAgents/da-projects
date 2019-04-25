using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Etl.TradingDesk.LoadersDA;
using DirectAgents.Domain.Entities.CPProg;
using FacebookAPI.Entities;

namespace CakeExtracter.Etl.SocialMarketing.LoadersDA
{
    [Obsolete]
    public class FacebookAdSummaryLoader : Loader<FBSummary>
    {
        private readonly TDadSummaryLoader tdAdSummaryLoader;
        //private Dictionary<string, int> tdAdIdLookupByFBAdId = new Dictionary<string, int>();

        public FacebookAdSummaryLoader(int accountId)
            : base(accountId)
        {
            //this.BatchSize = // the extracter groups the summaries by Date+AdName before yielding, so just use the default batch size
            tdAdSummaryLoader = new TDadSummaryLoader(accountId);
        }

        protected override int Load(List<FBSummary> items)
        {
            var tDadItems = items.Select(CreateTDadSummary).ToList();
            tdAdSummaryLoader.AddUpdateDependentTDads(tDadItems);
            tdAdSummaryLoader.AssignTDadIdToItems(tDadItems);
            var count = tdAdSummaryLoader.UpsertDailySummaries(tDadItems);
            return count;
        }

        public static TDadSummary CreateTDadSummary(FBSummary item)
        {
            var sum = new TDadSummary
            {
                Date = item.Date,
                TDad = new TDad
                {
                    Name = item.AdName,
                    ExternalId = item.AdId
                },
                Impressions = item.Impressions,
                AllClicks = item.AllClicks,
                Clicks = item.LinkClicks,
                PostClickConv = item.Conversions_click,
                PostViewConv = item.Conversions_view,
                Cost = item.Spend
            };
            return sum;
        }
    }
}
