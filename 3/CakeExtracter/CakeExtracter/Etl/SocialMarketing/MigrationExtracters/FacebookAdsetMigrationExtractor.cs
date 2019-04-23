using CakeExtracter.Common;
using CakeExtracter.Etl.SocialMarketing.Extracters;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Facebook;
using DirectAgents.Domain.Entities.CPProg.Facebook.AdSet;
using DirectAgents.Domain.Entities.CPProg.Facebook.Campaign;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CakeExtracter.Etl.SocialMarketing.MigrationExtracters
{
    public class FacebookAdSetMigrationExtractor : Extracter<FbAdSetSummary>
    {
        protected readonly DateRange dateRange;
        protected readonly int accountId;   // in our db
        protected readonly string fbAccountId; // fb account: aka "ad account"

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookAdSetSummaryExtracter"/> class.
        /// </summary>
        /// <param name="dateRange">The date range.</param>
        /// <param name="account">The account.</param>
        /// <param name="fbUtility">The fb utility.</param>
        public FacebookAdSetMigrationExtractor(DateRange dateRange, ExtAccount account)
        {
            this.dateRange = dateRange;
            this.accountId = account.Id;
            this.fbAccountId = account.ExternalId;
        }

        /// <summary>
        /// The derived class implements this method, which calls Add() for each item
        /// extracted and then calls End() when complete.
        /// </summary>
        protected override void Extract()
        {
            Logger.Info(accountId, "Extracting AdSet Summaries from Facebook Database.",
                        this.fbAccountId, this.dateRange.FromDate, this.dateRange.ToDate);
            try
            {
                using (var db = new ClientPortalProgContext())
                {
                    var summaries = GetExistingAdsetSummaries(accountId, db);
                    var allActions = GetExistingAdsetActions(accountId, db);
                    var fbSummaries = summaries.Select(x => CreateFbAdsetSummary(x, allActions)).ToList();
                    Add(fbSummaries);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(accountId, ex);
            }
            End();
        }

        private List<AdSetSummary> GetExistingAdsetSummaries(int accountId, ClientPortalProgContext db)
        {
            var adsetSummaries = db.AdSetSummaries.Where(sum => sum.AdSet.AccountId == accountId
                && sum.Date >= dateRange.FromDate && sum.Date <= dateRange.ToDate).ToList();
            return adsetSummaries;
        }

        private Dictionary<int, List<AdSetAction>> GetExistingAdsetActions(int accountId, ClientPortalProgContext db)
        {
            var adsetActions = db.AdSetActions.Where(sum => sum.AdSet.AccountId == accountId 
                && sum.Date >= dateRange.FromDate && sum.Date <= dateRange.ToDate).GroupBy(act => act.AdSet.Id).ToDictionary(gr => gr.Key, gr => gr.ToList());
            return adsetActions;
        }

        private FbAdSetSummary CreateFbAdsetSummary(AdSetSummary item, Dictionary<int, List<AdSetAction>> allActions)
        {
            var sum = new FbAdSetSummary
            {
                Date = item.Date,
                AdSet = new FbAdSet
                {
                    Name = item.AdSet.Name,
                    ExternalId = item.AdSet.ExternalId,
                    AccountId = accountId,
                    Campaign = new FbCampaign
                    {
                        AccountId = accountId,
                        Name = item.AdSet.Strategy.Name,
                        ExternalId = item.AdSet.Strategy.ExternalId
                    },
                },
                Impressions = item.Impressions,
                AllClicks = item.AllClicks,
                Clicks = item.Clicks,
                PostClickConv = item.PostClickConv,
                PostViewConv = item.PostViewConv,
                PostClickRev = item.PostClickRev,
                PostViewRev = item.PostViewRev,
                Cost = item.Cost,
                Actions = allActions.ContainsKey(item.AdSet.Id) ? GetActions(allActions[item.AdSetId], item) : new List<FbAdSetAction>()
            };
            return sum;
        }

        private List<FbAdSetAction> GetActions(List<AdSetAction> allActions, AdSetSummary adsetSummary)
        {
            var adsetActions = allActions.Where(act => act.Date == adsetSummary.Date && act.AdSetId == adsetSummary.AdSet.Id).ToList();
            var fbActions = adsetActions.Select(action => new FbAdSetAction
            {
                ActionType = new FbActionType
                {
                    Code = action.ActionType.Code,
                },
                Date = action.Date,
                PostClick = action.PostClick,
                PostView = action.PostView,
                PostClickVal = action.PostClickVal,
                PostViewVal = action.PostViewVal
            }).ToList();
            return fbActions;
        }
    }
}
