﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement;
using CakeExtracter.Etl.SocialMarketing;
using CakeExtracter.Etl.SocialMarketing.Extractors.V2;
using CakeExtracter.Etl.SocialMarketing.LoadersDAV2;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using FacebookAPI;

namespace CakeExtracter.Commands
{
    /// <summary>
    /// Facebook stats updating command(Included creatives expansion)
    /// </summary>
    /// <seealso cref="CakeExtracter.Common.ConsoleCommand" />
    [Export(typeof(ConsoleCommand))]
    public class DASynchFacebookStatsV2 : ConsoleCommand
    {
        private const int DefaultDaysAgo = 41;

        private const int ProcessingChunkSize = 11;

        private FacebookInsightsDataProviderBuilder insightsDataProviderBuilder;

        public int? AccountId { get; set; }

        public int? CampaignId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? DaysAgoToStart { get; set; }

        public string StatsType { get; set; }

        public bool DisabledOnly { get; set; }

        public int? MinAccountId { get; set; }

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
        }

        public DASynchFacebookStatsV2()
        {
            IsCommand("daSynchFacebookStatsV2", "synch Facebook stats");
            HasOption<int>("a|accountId=", "Account Id (default = all)", c => AccountId = c);
            HasOption<int>("c|campaignId=", "Campaign Id (optional)", c => CampaignId = c);
            HasOption("s|startDate=", "Start Date (default is 'daysAgo')", c => StartDate = DateTime.Parse(c));
            HasOption("e|endDate=", "End Date (default is yesterday)", c => EndDate = DateTime.Parse(c));
            HasOption<int>("d|daysAgo=", $"Days Ago to start, if startDate not specified (default = {DefaultDaysAgo})", c => DaysAgoToStart = c);
            HasOption<string>("t|statsType=", "Stats Type (default: all)", c => StatsType = c);
            HasOption<bool>("x|disabledOnly=", "Include only disabled accounts (default = false)", c => DisabledOnly = c);
            HasOption<int>("m|minAccountId=", "Include this and all higher accountIds (optional)", c => MinAccountId = c);
            insightsDataProviderBuilder = new FacebookInsightsDataProviderBuilder();
        }

        public override int Execute(string[] remainingArguments)
        {
            var dateRange = CommandHelper.GetDateRange(StartDate, EndDate, DaysAgoToStart, DefaultDaysAgo);
            Logger.Info("Facebook ETL. DateRange {0}.", dateRange);

            var statsType = new StatsTypeAgg(StatsType);
            var accounts = GetAccounts();

            var fbAdMetadataProvider = new FacebookAdMetadataProvider(null, null);
            var metadataExtractor = new FacebookAdMetadataExtractorV2(fbAdMetadataProvider);
            Parallel.ForEach(accounts, (account) =>
            {
                CommandExecutionContext.Current.SetJobExecutionStateInHistory("Started", account.Id);
                var acctDateRange = new DateRange(dateRange.FromDate, dateRange.ToDate);
                if (account.Campaign != null) // check/adjust daterange - if acct assigned to a campaign/advertiser
                {
                    if (!StartDate.HasValue && account.Campaign.Advertiser.StartDate.HasValue
                        && acctDateRange.FromDate < account.Campaign.Advertiser.StartDate.Value)
                        acctDateRange.FromDate = account.Campaign.Advertiser.StartDate.Value;
                    if (!EndDate.HasValue && account.Campaign.Advertiser.EndDate.HasValue
                        && acctDateRange.ToDate > account.Campaign.Advertiser.EndDate.Value)
                        acctDateRange.ToDate = account.Campaign.Advertiser.EndDate.Value;
                }
                Logger.Info(account.Id, "Facebook ETL. Account {0} - {1}. DateRange {2}.", account.Id, account.Name, acctDateRange);
                if (acctDateRange.ToDate < acctDateRange.FromDate)
                {
                    Logger.Info(account.Id, "Finished Facebook ETL. Account {0} - {1}. DateRange {2}.", account.Id, account.Name, acctDateRange);
                    CommandExecutionContext.Current.SetJobExecutionStateInHistory("Finished", account.Id);
                    return;
                }
                var fbUtility = insightsDataProviderBuilder.BuildInsightsDataProvider(account);
                int? numDailyItems = null;
                if (statsType.Daily)
                    numDailyItems = DoETL_Daily(acctDateRange, account, fbUtility);

                var Accts_DailyOnly = !AccountId.HasValue || statsType.All
                    ? ConfigurationHelper.ExtractEnumerableFromConfig("FB_DailyStatsOnly").ToArray()
                    : new string[] { };
                if (Accts_DailyOnly.Contains(account.ExternalId))
                {
                    Logger.Info(account.Id, "Finished Facebook ETL. Account {0} - {1}. DateRange {2}.", account.Id, account.Name, acctDateRange);
                    CommandExecutionContext.Current.SetJobExecutionStateInHistory("Finished", account.Id);
                    return;
                }

                // Skip strategy & adset stats if there were no dailies
                if (statsType.Strategy && (numDailyItems == null || numDailyItems.Value > 0))
                    DoETL_Strategy(acctDateRange, account, fbUtility);
                if (statsType.AdSet && (numDailyItems == null || numDailyItems.Value > 0))
                    DoETL_AdSet(acctDateRange, account, fbUtility);
                if (statsType.Creative && (numDailyItems == null || numDailyItems.Value > 0))
                    DoETL_Creative(acctDateRange, account, fbUtility, metadataExtractor);
                Logger.Info(account.Id, "Finished Facebook ETL. Account {0} - {1}. DateRange {2}.", account.Id, account.Name, acctDateRange);
                CommandExecutionContext.Current.SetJobExecutionStateInHistory("Finished", account.Id);
            });
            return 0;
        }

        private int DoETL_Daily(DateRange dateRange, ExtAccount account, FacebookInsightsDataProvider fbUtility)
        {
            CommandExecutionContext.Current.SetJobExecutionStateInHistory("Daily level.", account.Id);
            var dateRangesToProcess = dateRange.GetDaysChunks(ProcessingChunkSize).ToList();
            int addedCount = 0;
            dateRangesToProcess.ForEach(rangeToProcess =>
            {
                var extractor = new FacebookDailySummaryExtractorV2(rangeToProcess, account, fbUtility);
                var loader = new FacebookDailySummaryLoaderV2(account.Id, rangeToProcess);
                CommandHelper.DoEtl(extractor, loader);
                addedCount += extractor.Added;
            });
            return addedCount;
        }

        private void DoETL_Strategy(DateRange dateRange, ExtAccount account, FacebookInsightsDataProvider fbUtility)
        {
            CommandExecutionContext.Current.SetJobExecutionStateInHistory("Strategy level.", account.Id);
            var dateRangesToProcess = dateRange.GetDaysChunks(ProcessingChunkSize).ToList();
            dateRangesToProcess.ForEach(rangeToProcess =>
            {
                var extractor = new FacebookCampaignSummaryExtractorV2(rangeToProcess, account, fbUtility);
                var loader = new FacebookCampaignSummaryLoaderV2(account.Id, rangeToProcess);
                CommandHelper.DoEtl(extractor, loader);
            });
        }

        private void DoETL_AdSet(DateRange dateRange, ExtAccount account, FacebookInsightsDataProvider fbUtility)
        {
            CommandExecutionContext.Current.SetJobExecutionStateInHistory("Adset level.", account.Id);
            var dateRangesToProcess = dateRange.GetDaysChunks(ProcessingChunkSize).ToList();
            dateRangesToProcess.ForEach(rangeToProcess =>
            {
                var extractor = new FacebookAdSetSummaryExtractorV2(rangeToProcess, account, fbUtility);
                var loader = new FacebookAdSetSummaryLoaderV2(account.Id, rangeToProcess);
                CommandHelper.DoEtl(extractor, loader);
            });
        }

        private void DoETL_Creative(DateRange dateRange, ExtAccount account, FacebookInsightsDataProvider fbUtility, FacebookAdMetadataExtractorV2 metadataExtractor)
        {
            CommandExecutionContext.Current.SetJobExecutionStateInHistory("Ad level.", account.Id);
            var dateRangesToProcess = dateRange.GetDaysChunks(ProcessingChunkSize).ToList();
            Logger.Info(account.Id, "Started reading ad's metadata");

            // It's not possible currently fetch facebook creatives data from the insights endpoint.
            var allAdsMetadata = metadataExtractor.GetAdCreativesData(account);
            Logger.Info(account.Id, "Finished reading ad's metadata");
            dateRangesToProcess.ForEach(rangeToProcess =>
            {
                var extractor = new FacebookAdSummaryExtractorV2(rangeToProcess, account, fbUtility, allAdsMetadata);
                var loader = new FacebookAdSummaryLoaderV2(account.Id, rangeToProcess);
                CommandHelper.DoEtl(extractor, loader);
            });
        }

        private IEnumerable<ExtAccount> GetAccounts()
        {
            using (var db = new ClientPortalProgContext())
            {
                var accounts = db.ExtAccounts.Include("Network").Include("Campaign.Advertiser").Where(a => a.Platform.Code == Platform.Code_FB);
                if (CampaignId.HasValue || AccountId.HasValue)
                {
                    if (CampaignId.HasValue)
                        accounts = accounts.Where(a => a.CampaignId == CampaignId.Value);
                    if (AccountId.HasValue)
                        accounts = accounts.Where(a => a.Id == AccountId.Value);
                }
                else if (!DisabledOnly)
                    accounts = accounts.Where(a => !a.Disabled); //all accounts that aren't disabled

                if (DisabledOnly)
                    accounts = accounts.Where(a => a.Disabled);
                if (MinAccountId.HasValue)
                    accounts = accounts.Where(a => a.Id >= MinAccountId.Value);

                return accounts.OrderBy(a => a.Id).ToList().Where(a => !string.IsNullOrWhiteSpace(a.ExternalId));
            }
        }
    }
}
