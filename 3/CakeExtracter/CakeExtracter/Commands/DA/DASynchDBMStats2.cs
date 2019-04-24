using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.IO;
using System.Linq;
using CakeExtracter.Common;
using CakeExtracter.Etl.DBM.Composer;
using CakeExtracter.Etl.DBM.Downloader;
using CakeExtracter.Etl.DBM.Extractors;
using CakeExtracter.Etl.DBM.Extractors.Parsers;
using CakeExtracter.Etl.DBM.Extractors.Parsers.ParsingConverters;
using CakeExtracter.Etl.DBM.Models;
using CakeExtracter.Helpers;
using CsvHelper.Configuration;
using DBM;
using DBM.Parsers.Models;
using DirectAgents.Domain.Concrete;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Commands
{
    ///The class represents a command that is used to retrieve statistics from the Google DBM portal
    [Export(typeof(ConsoleCommand))]
    public class DASynchDBMStats2 : ConsoleCommand
    {
        private const int DefaultDaysAgo = 14;


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

        public IEnumerable<int> CreativeReportIds { get; set; }
        public IEnumerable<int> LineItemReportIds { get; set; }
        
        private DBMUtility DbmUtility { get; set; }
        
        /// <inheritdoc />
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

        /// <inheritdoc />
        /// <summary>
        /// The method runs the current command and extract and save statistics from the DBM portal based on the command arguments.
        /// </summary>
        /// <param name="remainingArguments"></param>
        /// <returns>Execution code</returns>
        public override int Execute(string[] remainingArguments)
        {
            SetConfigurationVariables();
            SetupDbmUtility();

            var dateRange = CommandHelper.GetDateRange(StartDate, EndDate, DaysAgoToStart, DefaultDaysAgo);
            Logger.Info("DBM ETL. DateRange {0}.", dateRange);

            var accounts = GetAccounts();
            DoETLs(dateRange, accounts);
            
            SaveTokens();
            return 0;
        }

        /// <inheritdoc />
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

        private void SetConfigurationVariables()
        {
            LineItemReportIds = GetValuesFromConfig("DBM_LineItemReportIds", ConfigurationHelper.ExtractNumbersFromConfigValue);
            CreativeReportIds = GetValuesFromConfig("DBM_CreativeReportIds", ConfigurationHelper.ExtractNumbersFromConfigValue);
        }

        private static IEnumerable<T> GetValuesFromConfig<T>(string parameterName, Func<string, IEnumerable<T>> extractFromConfigFunc)
        {
            var configValue = ConfigurationManager.AppSettings[parameterName];
            var values = extractFromConfigFunc(configValue);
            return values;
        }

        private void DoETLs(DateRange dateRange, IEnumerable<ExtAccount> accounts)
        {
            foreach (var creativeReportId in CreativeReportIds)
            {
                DoEtl_Creative(dateRange, accounts, creativeReportId);
            }
        }

        private void DoEtl_Creative(DateRange dateRange, IEnumerable<ExtAccount> accounts, int creativeReportId)
        {
            var reportUrl = DbmUtility.GetURLForReport(creativeReportId);
            if (string.IsNullOrWhiteSpace(reportUrl))
            {
                return;
            }

            var reportContent = DbmReportDownloader.GetStreamReaderFromUrl(reportUrl);
            var creativeReportRows = GetCreativeRowsFromReportContent(dateRange, reportContent);
            
            var composer = new DbmReportDataComposer(accounts.ToList());
            var creativeSummariesGroupedByAccount = composer.Compose(creativeReportRows);

            creativeSummariesGroupedByAccount.ForEach(creativeReportData =>
            {
                DoEtl_Creative(dateRange, creativeReportData);
            });
        }

        private IEnumerable<DbmCreativeReportRow> GetCreativeRowsFromReportContent(DateRange dateRange, StreamReader reportContent)
        {
            var rowMap = new DbmCreativeReportEntityRowMap();
            var creativeReportRows = GetReportRows<DbmCreativeReportRow>(dateRange, reportContent, rowMap);
            return creativeReportRows;
        }

        private static IEnumerable<TDbmReportRow> GetReportRows<TDbmReportRow>(DateRange dateRange, StreamReader stream, CsvClassMap rowMap)
         where TDbmReportRow : DbmBaseReportRow
        {
            const string path = "Reports\\TEST_NEW_Creative_20190424_052740_604983784_2523313339.csv";
            //var parser = new DbmReportCsvParser<TDbmReportRow>(dateRange, rowMap, streamReader: stream);
            var parser = new DbmReportCsvParser<TDbmReportRow>(dateRange, rowMap, csvFilePath: path);
            var creativeReportRows = parser.EnumerateRows();
            return creativeReportRows;
        }

        private static void DoEtl_Creative(DateRange dateRange, DbmAccountReportData creativeReportData)
        {
            var account = creativeReportData.Account;
            Logger.Info(account.Id, "DBM ETL. Account ({0}) {1}", account.Id, account.Name);

            var extractor = new DbmCreativeExtractor(creativeReportData);
            var loader = new Etl.DBM.Loaders.SummariesLoaders.DbmCreativeSummaryLoader(creativeReportData.Account.Id, dateRange);
            CommandHelper.DoEtl(extractor, loader);

            Logger.Info(account.Id, "Finished DBM ETL for account ({0}) {1}", account.Id, account.Name);
        }

        private IEnumerable<ExtAccount> GetAccounts()
        {
            var repository = new PlatformAccountRepository();
            if (!AccountId.HasValue)
            {
                var accounts = repository.GetAccountsWithFilledExternalIdByPlatformCode(Platform.Code_DBM, false);
                return accounts;
            }

            var account = repository.GetAccount(AccountId.Value);
            return new[] {account};
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
    }
}
