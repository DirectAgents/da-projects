using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using CakeExtracter.Common;
using CakeExtracter.Etl.SocialMarketing.LoadersDA;
using CakeExtracter.Etl.SocialMarketing.MigrationExtracters;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;

using Microsoft.Practices.EnterpriseLibrary.Common.Utility;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class DASynchFacebookMigration : ConsoleCommand
    {
        public int? AccountId { get; set; }
        public int? CampaignId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? DaysAgoToStart { get; set; }
        public string StatsType { get; set; }
        public bool DisabledOnly { get; set; }
        public int? MinAccountId { get; set; }
        public int? DaysPerCall { get; set; }
        public int? ClickWindow { get; set; }
        public int? ViewWindow { get; set; }
        private const int processingChunkSize = 100;

        public override void ResetProperties()
        {
            AccountId = null;
            CampaignId = null;
            StartDate = null;
            EndDate = null;
            DaysAgoToStart = null;
            StatsType = null;
            DisabledOnly = false;
            MinAccountId = null;
            DaysPerCall = null;
            ClickWindow = null;
            ViewWindow = null;
        }

        public DASynchFacebookMigration()
        {
            IsCommand("daSynchFacebookMigration", "synch Facebook stats");
        }

        public override int Execute(string[] remainingArguments)
        {
            var dateRange = new DateRange(DateTime.Now.AddYears(-4), DateTime.Now);
            var accounts = GetAccounts();
            accounts.ForEach((account) =>
            {
                Logger.Info($"Processing account {account.Id} - {account.Name}");
                DoETL_Daily(dateRange, account);
                DoETL_Strategy(dateRange, account);
                DoETL_AdSet(dateRange, account);
                DoETL_Creative(dateRange, account);
                Logger.Info($"Finished account {account.Id} - {account.Name}");
            });
            return 0;
        }

        private int DoETL_Daily(DateRange dateRange, ExtAccount account)
        {
            var extractor = new FacebookDailyMigrationExtractor(dateRange, account);
            var loader = new FacebookDailySummaryLoader(account.Id, dateRange);
            CommandHelper.DoEtl(extractor, loader);
            return extractor.Added;
        }

        private void DoETL_Strategy(DateRange dateRange, ExtAccount account)
        {
            var extractor = new FacebookCampaignMigrationExtractor(dateRange, account);
            var loader = new FacebookCampaignSummaryLoader(account.Id, dateRange);
            CommandHelper.DoEtl(extractor, loader);
        }

        private void DoETL_AdSet(DateRange dateRange, ExtAccount account)
        {
            var dateRangesToProcess = dateRange.GetDaysChunks(processingChunkSize).ToList();
            dateRangesToProcess.ForEach(rangeToProcess =>
            {
                var extractor = new FacebookAdSetMigrationExtractor(rangeToProcess, account);
                var loader = new FacebookAdSetSummaryLoader(account.Id, rangeToProcess);
                CommandHelper.DoEtl(extractor, loader);
            });
        }

        private void DoETL_Creative(DateRange dateRange, ExtAccount account)
        {
            var extractor = new FacebookAdMigrationExtractor(dateRange, account);
            var loader = new FacebookAdSummaryLoader(account.Id, dateRange);
            CommandHelper.DoEtl(extractor, loader);
        }

        private IEnumerable<ExtAccount> GetAccounts()
        {
            using (var db = new ClientPortalProgContext())
            {
                var accounts = db.ExtAccounts.Include("Network").Include("Campaign.Advertiser").Where(a => a.Platform.Code == Platform.Code_FB);
                return accounts.OrderBy(a => a.Id).ToList().Where(a => !string.IsNullOrWhiteSpace(a.ExternalId));
            }
        }
    }
}
