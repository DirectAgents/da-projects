using CakeExtracter.Commands;
using CakeExtracter.Common;
using DirectAgents.Domain.Concrete;
using DirectAgents.Domain.Entities.CPProg;
using System;
using System.Collections.Generic;
using CakeExtracter;
using CakeExtracter.Etl;
using CakeExtracter.Etl.TradingDesk.Extracters;
using CakeExtracter.Etl.TradingDesk.LoadersDA.AmazonLoaders;
using CakeExtractor.SeleniumApplication.Configuration.Pda;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.AmazonPdaExtractors;
using Platform = DirectAgents.Domain.Entities.CPProg.Platform;

namespace CakeExtractor.SeleniumApplication.Commands
{
    internal class SyncAmazonPdaCommand : BaseAmazonSeleniumCommand
    {
        private readonly PdaCommandConfigurationManager configurationManager;
        
        public SyncAmazonPdaCommand()
        {
            configurationManager = new PdaCommandConfigurationManager();
        }

        public override string CommandName => "SyncAmazonPdaCommand";

        public override void PrepareCommandEnvironment(int executionProfileNumber)
        {
            AmazonPdaExtractor.PrepareExtractor();
        }

        public override void Run()
        {
            var statsType = new StatsTypeAgg(configurationManager.GetStatsTypeString());
            var dateRange = GetDateRange();
            var fromDatabase = configurationManager.GetFromDatabaseFlag();
            var fromRequest = configurationManager.GetFromRequestFlag();

            Logger.Info("Amazon ETL (PDA Campaigns). DateRange: {0}.", dateRange);
            AmazonPdaExtractor.SetAvailableProfileUrls();
            
            var accounts = GetAccounts(configurationManager.GetAccountId(), configurationManager.GetDisabledOnlyFlag());
            foreach (var account in accounts)
            {
                DoEtls(account, dateRange, statsType, fromDatabase, fromRequest);
            }

            Logger.Info("Amazon ETL (PDA Campaigns) has been finished.");
        }

        private static void DoEtls(ExtAccount account, DateRange dateRange, StatsTypeAgg statsType, bool fromDatabase, bool fromRequests)
        {
            Logger.Info(account.Id, "Commencing ETL for Amazon account ({0}) {1}", account.Id, account.Name);
            try
            {
                if (statsType.Daily && !fromDatabase)
                {
                    if (fromRequests)
                    {
                        DoEtlDailyFromRequests(account, dateRange);
                    }
                    else
                    {
                        DoEtlDaily(account, dateRange);
                    }
                }

                if (statsType.Strategy)
                {
                    if (fromRequests)
                    {
                        DoEtlStrategyFromRequests(account, dateRange);
                    }
                    else
                    {
                        DoEtlStrategy(account, dateRange);
                    }
                }

                if (statsType.Daily && fromDatabase)
                {
                    DoEtlDailyFromStrategyInDatabase(account, dateRange);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(account.Id, ex);
            }

            Logger.Info(account.Id, "Finished ETL for Amazon account ({0}) {1}", account.Id, account.Name);
        }

        private static void DoEtlDaily(ExtAccount account, DateRange dateRange)
        {
            var extractor = new AmazonPdaDailyExtractor(account, dateRange);
            var loader = new AmazonPdaDailySummaryLoader(account.Id);
            StartEtl(extractor, loader);
        }

        private static void DoEtlDailyFromRequests(ExtAccount account, DateRange dateRange)
        {
            var extractor = new AmazonPdaDailyRequestExtractor(account, dateRange);
            var loader = new AmazonPdaDailySummaryLoader(account.Id);
            StartEtl(extractor, loader);
        }

        private static void DoEtlDailyFromStrategyInDatabase(ExtAccount account, DateRange dateRange)
        {
            var extractor = new DatabaseStrategyToDailySummaryExtractor(dateRange, account.Id);
            var loader = new AmazonDailySummaryLoader(account.Id);
            StartEtl(extractor, loader);
        }

        private static void DoEtlStrategy(ExtAccount account, DateRange dateRange)
        {
            var extractor = new AmazonPdaCampaignExtractor(account, dateRange);
            var loader = new AmazonCampaignSummaryLoader(account.Id);
            StartEtl(extractor, loader);
        }

        private static void DoEtlStrategyFromRequests(ExtAccount account, DateRange dateRange)
        {
            var extractor = new AmazonPdaCampaignRequestExtractor(account, dateRange);
            var loader = new AmazonCampaignSummaryLoader(account.Id);
            StartEtl(extractor, loader);
        }

        private static void StartEtl<T>(Extracter<T> extractor, Loader<T> loader)
        {
            var extractorThread = extractor.Start();
            var loaderThread = loader.Start(extractor);
            extractorThread.Join();
            loaderThread.Join();
        }

        private DateRange GetDateRange()
        {
            var daysAgo = configurationManager.GetDaysAgo();
            var startDate = configurationManager.GetStartDate(daysAgo);
            var endDate = configurationManager.GetEndDate();
            var dateRange = new DateRange(startDate, endDate);
            return dateRange;
        }

        private IEnumerable<ExtAccount> GetAccounts(int? accountId, bool disabledOnly)
        {
            var repository = new PlatformAccountRepository();
            if (!accountId.HasValue)
            {
                var accounts = repository.GetAccountsWithFilledExternalIdByPlatformCode(Platform.Code_Amazon, disabledOnly);
                return accounts;
            }
            var account = repository.GetAccount(accountId.Value);
            return new[] { account };
        }
    }
}