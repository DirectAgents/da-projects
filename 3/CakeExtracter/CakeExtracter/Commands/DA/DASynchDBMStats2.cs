using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.IO;
using System.Linq;
using CakeExtracter.Common;
using CakeExtracter.Etl.TradingDesk.Extracters;
using CakeExtracter.Etl.TradingDesk.Extracters.SummaryCsvExtracters;
using CakeExtracter.Etl.TradingDesk.LoadersDA;
using DBM;
using DirectAgents.Domain.Concrete;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Commands
{
    ///The class represents a command that is used to retrieve statistics from the Google DBM portal
    [Export(typeof(ConsoleCommand))]
    public class DASynchDBMStats2 : ConsoleCommand
    {
        /// <summary>
        /// Command argument: Account ID in the database for which the command will be executed (default = all)
        /// </summary>
        public int? AccountId { get; set; }

        /// <summary>
        /// Command argument: Start date from which statistics will be extracted (default is 'daysAgo')
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Command argument: End date to which statistics will be extracted (default is yesterday)
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Command argument: The number of days ago to calculate the start date from which statistics will be retrieved,
        /// used if StartDate not specified (default = 14)
        /// </summary>
        public int? DaysAgoToStart { get; set; }

        private const int DefaultDaysAgo = 14;
        private const string AvailableAccountIdsConfigurationName = "DBM_StrategiesFromLineItems";
        private const string StrategyNameColMapping = "Line Item";
        private const string StrategyIdColMapping = "Line Item ID";

        private DBMUtility DbmUtility { get; set; }

        /// <summary>
        /// The constructor sets a command name and command arguments names, provides a description for them.
        /// </summary>
        public DASynchDBMStats2()
        {
            IsCommand("daSynchDBMStats2", "synch DBM Stats");
            HasOption<int>("a|accountId=", "Account Id (default = all)", c => AccountId = c);
            HasOption("s|startDate=", "Start Date (default is 'daysAgo')", c => StartDate = DateTime.Parse(c));
            HasOption("e|endDate=", "End Date (default is yesterday)", c => EndDate = DateTime.Parse(c));
            HasOption<int>("d|daysAgo=", $"Days Ago to start, if startDate not specified (default = {DefaultDaysAgo})", c => DaysAgoToStart = c);
        }

        /// <summary>
        /// The method runs the current command and extract and save statistics from the DBM portal based on the command arguments.
        /// </summary>
        /// <param name="remainingArguments"></param>
        /// <returns>Execution code</returns>
        public override int Execute(string[] remainingArguments)
        {
            Logger.LogToOneFile = true;
            SetupDbmUtility();

            var dateRange = GetDateRange();
            var accounts = GetAccounts();
            Logger.Info("DBM ETL. DateRange {0}.", dateRange);

            foreach (var account in accounts)
            {
                Logger.Info(account.Id, "DBM ETL. Account {0} - {1}", account.Id, account.Name);
                DoEtLs(account, dateRange);
                Logger.Info(account.Id, "Finished DBM ETL for account ({0}) {1}", account.Id, account.Name);
            }

            SaveTokens();
            return 0;
        }

        /// <summary>
        /// The method resets command arguments to defaults
        /// </summary>
        public override void ResetProperties()
        {
            AccountId = null;
            StartDate = null;
            EndDate = null;
            DaysAgoToStart = null;
        }

        private static PlatColMapping GetInitializedAccountColMapping(ExtAccount account)
        {
            var colMapping = account.Platform.PlatColMapping;
            colMapping.StrategyName = StrategyNameColMapping;
            colMapping.StrategyEid = StrategyIdColMapping;
            return colMapping;
        }

        private static void DoETL_DailyFromStrategyInDatabase(int accountId, DateRange dateRange)
        {
            var extractor = new DatabaseStrategyToDailySummaryExtracter(dateRange, accountId);
            var loader = new TDDailySummaryLoader(accountId);
            var extractorThread = extractor.Start();
            var loaderThread = loader.Start(extractor);
            extractorThread.Join();
            loaderThread.Join();
        }

        private static void DoETL_Strategy(int accountId, ColumnMapping colMapping, StreamReader streamReader, DateRange dateRange)
        {
            var extractor = new DbmStrategyCsvExtractor(dateRange, colMapping, streamReader);
            var loader = new TDStrategySummaryLoader(accountId);
            var extractorThread = extractor.Start();
            var loaderThread = loader.Start(extractor);
            extractorThread.Join();
            loaderThread.Join();
        }

        private void DoEtLs(ExtAccount account, DateRange dateRange)
        {
            var reportUrl = DbmUtility.GetURLForReport(account.ExternalId_int.Value);
            if (string.IsNullOrWhiteSpace(reportUrl))
            {
                return;
            }

            var colMapping = GetInitializedAccountColMapping(account);
            var streamReader = TDDailySummaryExtracter.CreateStreamReaderFromUrl(reportUrl);
            DoETL_Strategy(account.Id, colMapping, streamReader, dateRange);

            Logger.Info("Creating daily stats from strategy stats.");
            DoETL_DailyFromStrategyInDatabase(account.Id, dateRange);
        }

        // --- setup ---

        private void SetupDbmUtility()
        {
            DbmUtility = new DBMUtility(m => Logger.Info(m), m => Logger.Warn(m));
            GetTokens();
            DbmUtility.SetupService();
        }

        private void GetTokens()
        {
            // Get tokens, if any, from the database
            var tokenSets = Platform.GetPlatformTokens(Platform.Code_DBM);
            DbmUtility.TokenSets = tokenSets;
        }

        private void SaveTokens()
        {
            Platform.SavePlatformTokens(Platform.Code_DBM, DbmUtility.TokenSets);
        }

        private DateRange GetDateRange()
        {
            var today = DateTime.Today;
            var daysAgo = DaysAgoToStart ?? DefaultDaysAgo;
            var startDate = StartDate ?? today.AddDays(-daysAgo);
            var endDate = EndDate ?? today.AddDays(-1);
            var dateRange = new DateRange(startDate, endDate);
            return dateRange;
        }

        private IEnumerable<ExtAccount> GetAccounts()
        {
            var accountsDb = GetEnabledAccountsFromDatabase();
            var accountsConfig = GetEnabledAccountsFromConfig();
            var enabledAccounts = accountsDb.Where(x => accountsConfig.Contains(x.ExternalId) && x.ExternalId_int.HasValue).ToList();
            return enabledAccounts;
        }

        private IEnumerable<string> GetEnabledAccountsFromConfig()
        {
            var stringStrategiesFromLineItems = ConfigurationManager.AppSettings[AvailableAccountIdsConfigurationName] ?? string.Empty;
            var repsStrategiesFromLineItems = stringStrategiesFromLineItems.Split(',');
            return repsStrategiesFromLineItems;
        }

        private IEnumerable<ExtAccount> GetEnabledAccountsFromDatabase()
        {
            var repository = new PlatformAccountRepository();
            if (!AccountId.HasValue)
            {
                var accounts = repository.GetAccountsWithFilledExternalIdByPlatformCode(Platform.Code_DBM, false, true);
                return accounts;
            }

            var account = repository.GetAccountWithColumnMapping(AccountId.Value);
            return new[] { account };
        }
    }
}
