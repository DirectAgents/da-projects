using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Linq;
using CakeExtracter.Common;
using CakeExtracter.Etl.DBM.Extractors;
using CakeExtracter.Etl.DBM.Loaders.SummariesLoaders;
using CakeExtracter.Helpers;
using DBM;
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
                DoETL_Creative(dateRange, accounts, creativeReportId);
            });
            Logger.Info("Finished processing Creative reports");
        }

        private void DoETL_Creative(DateRange dateRange, IEnumerable<ExtAccount> accounts, int creativeReportId)
        {
            try
            {
                var extractor = new DbmCreativeExtractor(DbmUtility, dateRange, accounts, creativeReportId, KeepReports);
                var summaries = extractor.Extract();

                var loader = new DbmCreativeSummaryLoader(dateRange);
                loader.Load(summaries);
            }
            catch (Exception e)
            {
                Logger.Warn($"Could not process a report [report ID: {creativeReportId}]: {e.Message}");
            }
        }

        private void DoETLs_LineItem(DateRange dateRange, IEnumerable<ExtAccount> accounts)
        {
            Logger.Info($"Start processing Line item reports (report count: {LineItemReportIds.Count})...");
            LineItemReportIds.ForEach(lineItemReportId =>
            {
                DoETL_LineItem(dateRange, accounts, lineItemReportId);
            });
            Logger.Info("Finished processing Line item reports");
        }

        private void DoETL_LineItem(DateRange dateRange, IEnumerable<ExtAccount> accounts, int lineItemReportId)
        {
            try
            {
                var extractor = new DbmLineItemExtractor(DbmUtility, dateRange, accounts, lineItemReportId, KeepReports);
                var summaries = extractor.Extract();

                var loader = new DbmLineItemSummaryLoader(dateRange);
                loader.Load(summaries);
            }
            catch (Exception e)
            {
                Logger.Warn($"Could not process a report [report ID: {lineItemReportId}]: {e.Message}");
            }
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
