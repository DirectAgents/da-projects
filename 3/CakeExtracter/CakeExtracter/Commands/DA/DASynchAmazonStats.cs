﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Amazon;
using CakeExtracter.Bootstrappers;
using CakeExtracter.Common;
using CakeExtracter.Etl.TradingDesk.Extracters;
using CakeExtracter.Etl.TradingDesk.LoadersDA;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class DASynchAmazonStats : ConsoleCommand
    {
        public static int RunStatic(int? accountId = null, DateTime? startDate = null, 
            DateTime? endDate = null, string statsType = null)
        {
            AutoMapperBootstrapper.CheckRunSetup();
            var cmd = new DASynchAmazonStats
            {
                AccountId = accountId,
                //CampaignId = campaignId,
                StartDate = startDate,
                EndDate = endDate,
                StatsType = statsType
            };
            return cmd.Run();
        }

        public int? AccountId { get; set; }
        //public int? CampaignId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? DaysAgoToStart { get; set; }
        public string StatsType { get; set; }
        public bool DisabledOnly { get; set; }

        private AmazonUtility AmazonUtility { get; set; }

        public override void ResetProperties()
        {
            AccountId = null;
            //CampaignId = null;
            StartDate = null;
            EndDate = null;
            DaysAgoToStart = null;
            StatsType = null;
            DisabledOnly = false;
        }

        public DASynchAmazonStats()
        {
            IsCommand("DASynchAmazonStats", "Synch Amazon Stats");
            HasOption<int>("a|accountId=", "Account Id (default = all)", c => AccountId = c);
            //HasOption<int>("c|campaignId=", "Campaign Id ", c => CampaignId = c);
            HasOption("s|startDate=", "Start Date (default is 'daysAgo')", c => StartDate = DateTime.Parse(c));
            HasOption("e|endDate=", "End Date (default is yesterday)", c => EndDate = DateTime.Parse(c));
            HasOption<int>("d|daysAgo=", "Days Ago to start, if startDate not specified (default = 41)", c => DaysAgoToStart = c);
            HasOption<string>("t|statsType=", "Stats Type (default: all)", c => StatsType = c);
            HasOption<bool>("x|disabledOnly=", "Include only disabled accounts (default = false)", c => DisabledOnly = c);
        }


        public override int Execute(string[] remainingArguments)
        {
            if (!DaysAgoToStart.HasValue)
                DaysAgoToStart = 41; // used if StartDate==null
            var today = DateTime.Today;
            var yesterday = today.AddDays(-1);
            var dateRange = new DateRange(StartDate ?? today.AddDays(-DaysAgoToStart.Value), EndDate ?? yesterday);
            Logger.Info("Amazon ETL. DateRange {0}.", dateRange);

            var statsType = new StatsTypeAgg(this.StatsType);
            SetupAmazonUtility();

            var accounts = GetAccounts();
            foreach (var account in accounts)
            {
                Logger.Info("Commencing ETL for Amazon account ({0}) {1}", account.Id, account.Name);
                //AmazonUtility.SetWhichAlt(account.ExternalId);
                try
                {
                    if (statsType.Daily)
                        DoETL_Daily(dateRange, account);
                    if (statsType.Strategy)
                        DoETL_Strategy(dateRange, account);
                    if (statsType.AdSet)
                        DoETL_AdSet(dateRange, account);
                    if (statsType.Creative)
                        DoETL_Creative(dateRange, account);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
            }

            SaveTokens();
            return 0;
        }

        private void SetupAmazonUtility()
        {
            this.AmazonUtility = new AmazonUtility(m => Logger.Info(m), m => Logger.Warn(m));
            GetTokens();
        }
        private void GetTokens()
        {
            // Get tokens, if any, from the database
            string[] tokens = Platform.GetPlatformTokens(Platform.Code_Amazon);
            AmazonUtility.AccessToken = tokens[0];

        }
        private void SaveTokens()
        {
            Platform.SavePlatformTokens(Platform.Code_Amazon, AmazonUtility.AccessToken);
        }

        private void DoETL_Daily(DateRange dateRange, ExtAccount account)
        {
            var extracter = new AmazonDailySummaryExtracter(AmazonUtility, dateRange, account.ExternalId);
            var loader = new TDDailySummaryLoader(account.Id);

            
            var extracterThread = extracter.Start();            
            var loaderThread = loader.Start(extracter);


            extracterThread.Join();
            loaderThread.Join();
        }
        private void DoETL_Strategy(DateRange dateRange, ExtAccount account)
        {
            var extracter = new AmazonStrategySummaryExtracter(AmazonUtility, dateRange, account.ExternalId);
            var loader = new TDStrategySummaryLoader(account.Id);
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
        }
        private void DoETL_AdSet(DateRange dateRange, ExtAccount account)
        {
            var extracter = new AmazonAdSetSummaryExtracter(AmazonUtility, dateRange, account.ExternalId);
            var loader = new TDAdSetSummaryLoader(account.Id);
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
        }
        private void DoETL_Creative(DateRange dateRange, ExtAccount account)
        {
            var extracter = new AmazonTDadSummaryExtracter(AmazonUtility, dateRange, account.ExternalId);
            var loader = new TDadSummaryLoader(account.Id);
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
        }

        private IEnumerable<ExtAccount> GetAccounts()
        {
            using (var db = new ClientPortalProgContext())
            {
                //var accounts = db.ExtAccounts.Include("Platform.PlatColMapping").Where(a => a.Platform.Code == Platform.Code_Amazon);
                var accounts = db.ExtAccounts.Where(a => a.Platform.Code == Platform.Code_Amazon);
                if (AccountId.HasValue)
                    accounts = accounts.Where(a => a.Id == AccountId.Value);
                else if (!DisabledOnly)
                    accounts = accounts.Where(a => !a.Disabled);

                if (DisabledOnly)
                    accounts = accounts.Where(a => a.Disabled);

                return accounts.ToList().Where(a => !string.IsNullOrWhiteSpace(a.ExternalId));
            }
        }

    }
}
