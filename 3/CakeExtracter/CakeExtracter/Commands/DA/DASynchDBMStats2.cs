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
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class DASynchDBMStats2 : ConsoleCommand
    {
        public int? AccountId { get; set; }

        private DBMUtility dbmUtility { get; set; }

        public override void ResetProperties()
        {
            AccountId = null;
        }

        public DASynchDBMStats2()
        {
            IsCommand("daSynchDBMStats2", "synch DBM Stats");
            HasOption<int>("a|accountId=", "Account Id (default = all)", c => AccountId = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            Logger.LogToOneFile = true;
            SetupDBMUtility();

            var string_StrategiesFromLineItems = ConfigurationManager.AppSettings["DBM_StrategiesFromLineItems"] ?? "";
            var Reps_StrategiesFromLineItems = string_StrategiesFromLineItems.Split(new char[] { ',' });

            var today = DateTime.Today;
            var yesterday = today.AddDays(-1);
            var twoWeeksAgo = today.AddDays(-14);

            var accounts = GetAccounts();
            foreach (var account in accounts)
            {
                if (Reps_StrategiesFromLineItems.Contains(account.ExternalId))
                {
                    int? reportId = account.ExternalId_int;
                    if (!reportId.HasValue)
                        continue;

                    Logger.Info("DBM ETL. Account {0} - {1}", account.Id, account.Name);

                    var colMapping = account.Platform.PlatColMapping;
                    colMapping.StrategyName = "Line Item";
                    colMapping.StrategyEid = "Line Item ID";

                    var reportUrl = dbmUtility.GetURLForReport(reportId.Value);
                    if (!string.IsNullOrWhiteSpace(reportUrl))
                    {
                        var streamReader = TDDailySummaryExtracter.CreateStreamReaderFromUrl(reportUrl);
                        DoETL_Strategy(account.Id, colMapping, streamReader);

                        //do stratsum to dailysum etl (for what dates??? - last 2 weeks / or earliest/latest dates from loader)
                        var dateRange = new DateRange(twoWeeksAgo, yesterday);
                        Logger.Info("Creating daily stats from strategy stats - going 2 weeks back.");
                        DoETL_DailyFromStrategyInDatabase(account.Id, dateRange);
                    }
                }
            }
            SaveTokens();

            return 0;
        }

        private void DoETL_DailyFromStrategyInDatabase(int accountId, DateRange dateRange)
        {
            var extracter = new DatabaseStrategyToDailySummaryExtracter(dateRange, accountId);
            var loader = new TDDailySummaryLoader(accountId);
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
        }

        public void DoETL_Strategy(int accountId, ColumnMapping colMapping, StreamReader streamReader)
        {
            var extractor = new DbmStrategyCsvExtractor(colMapping, streamReader);
            var loader = new TDStrategySummaryLoader(accountId);
            var extractorThread = extractor.Start();
            var loaderThread = loader.Start(extractor);
            extractorThread.Join();
            loaderThread.Join();
        }

        // --- setup ---

        private void SetupDBMUtility()
        {
            this.dbmUtility = new DBMUtility(m => Logger.Info(m), m => Logger.Warn(m));
            GetTokens();
            dbmUtility.SetupService();
        }
        private void GetTokens()
        {
            // Get tokens, if any, from the database
            string[] tokenSets = Platform.GetPlatformTokens(Platform.Code_DBM);
            dbmUtility.TokenSets = tokenSets;
        }

        private void SaveTokens()
        {
            Platform.SavePlatformTokens(Platform.Code_DBM, dbmUtility.TokenSets);
        }

        private IEnumerable<ExtAccount> GetAccounts()
        {
            using (var db = new ClientPortalProgContext())
            {
                var accounts = db.ExtAccounts.Include("Platform.PlatColMapping").Where(a => a.Platform.Code == Platform.Code_DBM);
                if (AccountId.HasValue)
                    accounts = accounts.Where(x => x.Id == AccountId.Value);

                //return accounts.ToList().Where(a => !string.IsNullOrWhiteSpace(a.ExternalId));
                return accounts.ToList();
            }
        }
    }
}
