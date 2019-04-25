using CakeExtracter.Common;
using CakeExtracter.Etl.SocialMarketing.Extracters;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Facebook.Daily;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CakeExtracter.Etl.SocialMarketing.MigrationExtracters
{
    //!!! Migration only extractor. Remove when Facebook V2 migration completed and verified.
    public class FacebookDailyMigrationExtractor : Extracter<FbDailySummary>
    {
        protected readonly DateRange? dateRange;
        protected readonly int accountId;   // in our db
        protected readonly string fbAccountId; // fb account: aka "ad account"

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookAdSetSummaryExtracter"/> class.
        /// </summary>
        /// <param name="dateRange">The date range.</param>
        /// <param name="account">The account.</param>
        public FacebookDailyMigrationExtractor(DateRange dateRange, ExtAccount account)
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
                    var summaries = GetExistingDailySummaries(accountId, db);
                    var fbSummaries = summaries.Select(CreateFbDailySummary).ToList();
                    Add(fbSummaries);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(accountId, ex);
            }
            End();
        }

        private List<DailySummary> GetExistingDailySummaries(int accountId, ClientPortalProgContext db)
        {
            var dailySummaries = db.DailySummaries.Where(sum => sum.AccountId == accountId).ToList();
            return dailySummaries;
        }

        private FbDailySummary CreateFbDailySummary(DailySummary item)
        {
            var sum = new FbDailySummary
            {
                Date = item.Date,
                AccountId = accountId,
                Impressions = item.Impressions,
                AllClicks = item.AllClicks,
                Clicks = item.Clicks,
                PostClickConv = item.PostClickConv,
                PostViewConv = item.PostViewConv,
                PostClickRev = item.PostClickRev,
                PostViewRev = item.PostViewRev,
                Cost = item.Cost
            };
            return sum;
        }
    }
}
