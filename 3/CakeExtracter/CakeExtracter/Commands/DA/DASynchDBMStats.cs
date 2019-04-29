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
using CakeExtracter.Etl.DBM.Extractors.Parsers.Models;
using CakeExtracter.Etl.DBM.Extractors.Parsers.ParsingConverters;
using CakeExtracter.Etl.DBM.Models;
using CakeExtracter.Helpers;
using CsvHelper.Configuration;
using DBM;
using DBM.Helpers;
using DBM.Parsers.Models;
using DirectAgents.Domain.Concrete;
using DirectAgents.Domain.Entities.CPProg;
namespace CakeExtracter.Commands.DA
{
    ///The class represents a command that is used to retrieve statistics from the Google DBM portal
    [Export(typeof(ConsoleCommand))]
    public class DASynchDBMStats : ConsoleCommand
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

        /// <summary>
        /// Command argument: Store all reports from DBM portal in a separate folder
        /// </summary>
        public bool KeepReports { get; set; }
        public static string SavedReportFileName = "dbm_{0}.csv";
        public static string SavedReportsDirectoryName = "SavedReports";

        /// <summary>
        /// List of creative report identifiers specified on the configuration file
        /// </summary>
        public List<int> CreativeReportIds { get; set; }
        /// <summary>
        /// List of line item report identifiers specified on the configuration file
        /// </summary>
        public List<int> LineItemReportIds { get; set; }

        private DBMUtility DbmUtility { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// The constructor sets a command name and command arguments names, provides a description for them.
        /// </summary>
        public DASynchDBMStats()
        {
            IsCommand("DASynchDBMStats", "synch DBM Stats");
            HasOption<int>("a|accountId=", "Account Id (default = all)", c => AccountId = c);
            HasOption("s|startDate=", "Start Date (default is 'daysAgo')", c => StartDate = DateTime.Parse(c));
            HasOption("e|endDate=", "End Date (default is yesterday)", c => EndDate = DateTime.Parse(c));
            HasOption<int>("d|daysAgo=", $"Days Ago to start, if startDate not specified (default = {DefaultDaysAgo})", c => DaysAgoToStart = c);
            HasOption<bool>("k|keepReports=", "Store received DBM reports in a separate folder (default = false)", c => KeepReports = c);
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
            KeepReports = false;
        }

        private void SetConfigurationVariables()
        {
            LineItemReportIds = GetValuesFromConfig("DBM_LineItemReportIds", ConfigurationHelper.ExtractNumbersFromConfigValue);
            CreativeReportIds = GetValuesFromConfig("DBM_CreativeReportIds", ConfigurationHelper.ExtractNumbersFromConfigValue);
        }

        private static List<T> GetValuesFromConfig<T>(string parameterName, Func<string, IEnumerable<T>> extractFromConfigFunc)
        {
            var configValue = ConfigurationManager.AppSettings[parameterName];
            var values = string.IsNullOrEmpty(configValue) ? new List<T>() : extractFromConfigFunc(configValue);
            return values.ToList();
        }

        private void DoETLs(DateRange dateRange, IEnumerable<ExtAccount> accounts)
        {
            Logger.Info("DBM ETL. DateRange {0}.", dateRange);
            DoETLs_Creative(dateRange, accounts);
            DoETLs_LineItem(dateRange, accounts);
        }

        private void DoETLs_Creative(DateRange dateRange, IEnumerable<ExtAccount> accounts)
        {
            Logger.Info($"Start processing Creative reports (report count: {CreativeReportIds.Count})...");
            CreativeReportIds.ForEach(creativeReportId =>
            {
                try
                {
                    DoEtl_Creative(dateRange, accounts, creativeReportId);
                }
                catch (Exception e)
                {
                    Logger.Warn($"Could not process a report [report ID: {creativeReportId}]: {e.Message}");
                }
            });
            Logger.Info("Finished processing Creative reports");
        }

        private void DoETLs_LineItem(DateRange dateRange, IEnumerable<ExtAccount> accounts)
        {
            Logger.Info($"Start processing Line item reports (report count: {LineItemReportIds.Count})...");
            LineItemReportIds.ForEach(lineItemReportId =>
            {
                try
                {
                    DoEtl_LineItem(dateRange, accounts, lineItemReportId);
                }
                catch (Exception e)
                {
                    Logger.Warn($"Could not process a report [report ID: {lineItemReportId}]: {e}");
                }
            });
            Logger.Info("Finished processing Line item reports");
        }

        private void DoEtl_Creative(DateRange dateRange, IEnumerable<ExtAccount> accounts, int creativeReportId)
        {
            Logger.Info($"Start processing creative report [report ID: {creativeReportId}]...");
            var reportContent = GetReportContent(creativeReportId);
            var creativeReportRows = GetCreativeRows(dateRange, reportContent);
            var creativeSummariesGroups = GetCreativeSummariesGroupedByAccount(accounts, creativeReportRows);

            creativeSummariesGroups.ForEach(creativeReportData =>
            {
                DoEtl_CreativeForAccount(dateRange, creativeReportData);
            });
            Logger.Info($"Finished processing creative report [report ID: {creativeReportId}]");
        }

        private void DoEtl_LineItem(DateRange dateRange, IEnumerable<ExtAccount> accounts, int lineItemReportId)
        {
            Logger.Info($"Start processing line item report [report ID: {lineItemReportId}]...");

            var reportContent = GetReportContent(lineItemReportId);
            var lineItemReportRows = GetLineItemRows(dateRange, reportContent);
            var lineItemSummariesGroups = GetLineItemSummariesGroupedByAccount(accounts, lineItemReportRows);

            lineItemSummariesGroups.ForEach(lineItemReportData =>
            {
                DoEtl_LineItemForAccount(dateRange, lineItemReportData);
            });
            Logger.Info($"Finished processing line item report [report ID: {lineItemReportId}]");
        }

        private StreamReader GetReportContent(int reportId)
        {
            var reportUrl = GetReportUrl(reportId);
            var reportContent = GetReportContentFromUrl(reportUrl);
            return reportContent;
        }

        private string GetReportUrl(int reportId)
        {
            Logger.Info($"Retrieve a report location URL from the query ID [{reportId}]...");
            var reportUrl = DbmUtility.GetURLForReport(reportId);
            if (string.IsNullOrWhiteSpace(reportUrl))
            {
                throw new Exception("Could not retrieve the report location URL.");
            }
            Logger.Info($"The report location URL: {reportUrl}.");
            return reportUrl;
        }

        private StreamReader GetReportContentFromUrl(string reportUrl)
        {
            Logger.Info("Downloading a report...");
            var reportContent = DbmReportDownloader.GetStreamReaderFromUrl(reportUrl);
            if (reportContent == StreamReader.Null)
            {
                throw new Exception($"Failed downloading a report [report URL: {reportUrl}]");
            }
            if (KeepReports)
            {
                var reportContentToSave = DbmReportDownloader.GetStreamReaderFromUrl(reportUrl);
                SaveReport(reportContentToSave);
            }
            Logger.Info("The report downloaded successfully");
            return reportContent;
        }

        private static List<TDbmReportRow> GetReportRows<TDbmReportRow>(DateRange dateRange, StreamReader stream, CsvClassMap rowMap)
            where TDbmReportRow : DbmBaseReportRow
        {
            Logger.Info("Parsing a report...");
            var parser = new DbmReportCsvParser<TDbmReportRow>(dateRange, rowMap, streamReader: stream);
            var reportRows = parser.EnumerateRows().ToList();
            Logger.Info($"The report parsed successfully (row count: {reportRows.Count})");
            return reportRows;
        }

        private static IEnumerable<DbmCreativeReportRow> GetCreativeRows(DateRange dateRange, StreamReader reportContent)
        {
            var rowMap = new DbmCreativeReportEntityRowMap();
            var creativeReportRows = GetReportRows<DbmCreativeReportRow>(dateRange, reportContent, rowMap);
            return creativeReportRows;
        }

        private static IEnumerable<DbmLineItemReportRow> GetLineItemRows(DateRange dateRange, StreamReader reportContent)
        {
            var rowMap = new DbmLineItemReportEntityRowMap();
            var lineItemReportRows = GetReportRows<DbmLineItemReportRow>(dateRange, reportContent, rowMap);
            return lineItemReportRows;
        }

        private static List<DbmAccountLineItemReportData> GetLineItemSummariesGroupedByAccount(
            IEnumerable<ExtAccount> accounts, IEnumerable<DbmLineItemReportRow> lineItemReportRows)
        {
            Logger.Info("Composing report data...");
            var composer = new DbmReportDataComposer(accounts.ToList());
            var lineItemSummariesGroupedByAccount = composer.ComposeLineItemReportData(lineItemReportRows);
            Logger.Info($"Report data composed. Retrieved groups by account (group count: {lineItemSummariesGroupedByAccount.Count})");
            return lineItemSummariesGroupedByAccount;
        }

        private static List<DbmAccountCreativeReportData> GetCreativeSummariesGroupedByAccount(
            IEnumerable<ExtAccount> accounts, IEnumerable<DbmCreativeReportRow> creativeReportRows)
        {
            Logger.Info("Composing report data...");
            var composer = new DbmReportDataComposer(accounts.ToList());
            var creativeSummariesGroupedByAccount = composer.ComposeCreativeReportData(creativeReportRows);
            Logger.Info($"Report data composed. Retrieved groups by account (group count: {creativeSummariesGroupedByAccount.Count})");
            return creativeSummariesGroupedByAccount;
        }

        private static void DoEtl_LineItemForAccount(DateRange dateRange, DbmAccountLineItemReportData lineItemReportData)
        {
            var account = lineItemReportData.Account;
            Logger.Info(account.Id, "DBM ETL Line Item. Account ({0}) {1}", account.Id, account.Name);

            var extractor = new DbmLineItemExtractor(lineItemReportData);
            var loader = new Etl.DBM.Loaders.SummariesLoaders.DbmLineItemSummaryLoader(account.Id, dateRange);
            CommandHelper.DoEtl(extractor, loader);

            Logger.Info(account.Id, "Finished DBM ETL Line Item for account ({0}) {1}", account.Id, account.Name);
        }

        private static void DoEtl_CreativeForAccount(DateRange dateRange, DbmAccountCreativeReportData creativeReportData)
        {
            var account = creativeReportData.Account;
            Logger.Info(account.Id, "DBM ETL Creative. Account ({0}) {1}", account.Id, account.Name);

            var extractor = new DbmCreativeExtractor(creativeReportData);
            var loader = new Etl.DBM.Loaders.SummariesLoaders.DbmCreativeSummaryLoader(account.Id, dateRange);
            CommandHelper.DoEtl(extractor, loader);

            Logger.Info(account.Id, "Finished DBM ETL Creative for account ({0}) {1}", account.Id, account.Name);
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
            return new[] { account };
        }

        private static void SaveReport(StreamReader reportContent)
        {
            var reportFileName = string.Format(SavedReportFileName, DateTime.Now.ToString("yyyyMMdd_HHmmss"));
            try
            {
                FileManager.SaveToFileInExecutionFolder(SavedReportsDirectoryName, reportFileName, reportContent);
                Logger.Info($"Report content was saved to {reportFileName} file.");
            }
            catch (Exception e)
            {
                Logger.Error(new Exception($"Saving report failed ({reportFileName})", e));
            }
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
