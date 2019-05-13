using CakeExtracter.Common;
using CakeExtracter.Etl.SocialMarketing.Extracters;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Facebook.Ad;
using DirectAgents.Domain.Entities.CPProg.Facebook.AdSet;
using DirectAgents.Domain.Entities.CPProg.Facebook.Campaign;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace CakeExtracter.Etl.SocialMarketing.MigrationExtracters
{
    //!!! Migration only extractor. Remove when Facebook V2 migration completed and verified.
    public class FacebookAdMigrationExtractor : Extracter<FbAdSummary>
    {
        protected readonly DateRange? dateRange;
        protected readonly int accountId;   // in our db
        protected readonly string fbAccountId; // fb account: aka "ad account"

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookAdSetSummaryExtracter"/> class.
        /// </summary>
        /// <param name="dateRange">The date range.</param>
        /// <param name="account">The account.</param>
        public FacebookAdMigrationExtractor(DateRange dateRange, ExtAccount account)
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
            Logger.Info(accountId, "Extracting AdSet Summaries from Facebook API for ({0}) from {1:d} to {2:d}",
                        this.fbAccountId, this.dateRange.Value.FromDate, this.dateRange.Value.ToDate);
            try
            {
                using (var db = new ClientPortalProgContext())
                {
                    var summaries = GetExistingAdSummaries(accountId, db);
                    var fbSummaries = summaries.Select(CreateFbAdSummary).ToList();
                    Add(fbSummaries);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(accountId, ex);
            }
            End();
        }

        private List<TDadSummary> GetExistingAdSummaries(int accountId, ClientPortalProgContext db)
        {
            var adSummaries = db.TDadSummaries.Where(sum => sum.TDad.AccountId == accountId).Include(s => s.TDad.AdSet.Strategy).ToList();
            return adSummaries;
        }

        private FbAdSummary CreateFbAdSummary(TDadSummary item)
        {
            var sum = new FbAdSummary
            {
                Date = item.Date,
                Ad = new FbAd
                {
                    Name = item.TDad.Name,
                    ExternalId = item.TDad.ExternalId,
                    AccountId = accountId,
                    AdSet = item.TDad?.AdSet != null ? new FbAdSet
                    {
                        AccountId = accountId,
                        Name = item.TDad.AdSet.Strategy.Name,
                        ExternalId = item.TDad.AdSet.Strategy.ExternalId,
                        Campaign = item.TDad?.AdSet?.Strategy != null ? new FbCampaign
                        {
                            AccountId = accountId,
                            Name = item.TDad.AdSet.Strategy.Name,
                            ExternalId = item.TDad.AdSet.Strategy.ExternalId
                        } : null,
                    } : null,
                },
                Impressions = item.Impressions,
                AllClicks = item.AllClicks,
                Clicks = item.Clicks,
                PostClickConv = item.PostClickConv,
                PostViewConv = item.PostViewConv,
                Cost = item.Cost,
                Actions = new List<FbAdAction>()
            };
            return sum;
        }
    }
}
