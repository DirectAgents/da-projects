﻿using System;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.IO;
using System.Linq;
using CakeExtracter.Bootstrappers;
using CakeExtracter.Common;
using CakeExtracter.Etl.TradingDesk.Extracters;
using CakeExtracter.Etl.TradingDesk.Extracters.SummaryCsvExtracters;
using CakeExtracter.Etl.TradingDesk.LoadersDA;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class DASynchTDDailySummaries : ConsoleCommand
    {
        public static int RunStatic(int accountId, StreamReader streamReader, string statsType = null, DateTime? statsDate = null)
        {
            AutoMapperBootstrapper.CheckRunSetup();
            var cmd = new DASynchTDDailySummaries
            {
                AccountId = accountId,
                StreamReader = streamReader,
                StatsType = statsType,
                StatsDate = statsDate
            };
            int result = cmd.Run();
            return result;
        }

        public int AccountId { get; set; }
        public StreamReader StreamReader { get; set; }
        public string FilePath { get; set; }
        public string StatsType { get; set; }
        public DateTime? StatsDate { get; set; } // optional

        public override void ResetProperties()
        {
            AccountId = 0;
            StreamReader = null;
            FilePath = null;
            StatsType = null;
            StatsDate = null;
        }

        public DASynchTDDailySummaries()
        {
            IsCommand("daSynchTDDailySummaries", "synch daily summaries via file upload");
            HasRequiredOption<int>("a|accountId=", "Account ID", c => AccountId = c);
            HasOption<string>("f|filePath=", "CSV filepath", c => FilePath = c);
            HasOption<string>("t|statsType=", "Stats Type (default: all)", c => StatsType = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            Logger.LogToOneFile = true;

            var account = GetAccount(AccountId);
            if (account != null)
            {
                ColumnMapping mapping = account.Platform.PlatColMapping;
                if (mapping == null)
                    mapping = ColumnMapping.CreateDefault();

                //TESTING!
                mapping.AdSetName = "Line Item";
                mapping.AdSetEid = "Line Item ID";
                //TODO: allow saving to db

                var statsType = new StatsTypeAgg(this.StatsType);

                if (statsType.Daily)
                    DoETL_Daily(mapping);
                if (statsType.Strategy)
                    DoETL_Strategy(mapping);
                if (statsType.AdSet)
                    DoETL_AdSet(mapping);
                if (statsType.Creative)
                    DoETL_Creative(mapping);
                if (statsType.Site)
                    DoETL_Site(mapping);
                if (statsType.Conv)
                    DoETL_Conv();
            }
            else
            {
                Logger.Warn("ExtAccount ({0}) not found. Skipping ETL.", AccountId);
            }
            return 0;
        }

        public void DoETL_Daily(ColumnMapping mapping)
        {
            var extracter = new TDDailySummaryExtracter(mapping, streamReader: StreamReader, csvFilePath: FilePath);
            var loader = new TDDailySummaryLoader(AccountId);
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
        }
        public void DoETL_Strategy(ColumnMapping mapping)
        {
            var extracter = new TDStrategySummaryExtracter(mapping, streamReader: StreamReader, csvFilePath: FilePath);
            var loader = new TDStrategySummaryLoader(AccountId);
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
        }
        public void DoETL_AdSet(ColumnMapping mapping)
        {
            var extracter = new TDAdSetSummaryExtracter(mapping, streamReader: StreamReader, csvFilePath: FilePath);
            var loader = new TDAdSetSummaryLoader(AccountId);
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
        }
        public void DoETL_Creative(ColumnMapping mapping)
        {
            var extracter = new TDadSummaryExtracter(mapping, streamReader: StreamReader, csvFilePath: FilePath);
            var loader = new TDadSummaryLoader(AccountId);
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
        }
        public void DoETL_Site(ColumnMapping mapping)
        {
            var extracter = new TDSiteSummaryExtracter(mapping, dateOverride: StatsDate, streamReader: StreamReader, csvFilePath: FilePath);
            var loader = new TDSiteSummaryLoader(AccountId);
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
        }
        public void DoETL_Conv()
        {
            var platCode = GetAccount(AccountId).Platform.Code;
            var extracter = new TDConvExtracter(csvFilePath: FilePath, streamReader: StreamReader, platCode: platCode);
            var loader = new TDConvLoader(AccountId, platCode);
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
        }


        public static ExtAccount GetAccount(int acctId)
        {
            using (var db = new ClientPortalProgContext())
            {
                return db.ExtAccounts
                    .Include(a => a.Platform.PlatColMapping)
                    .SingleOrDefault(a => a.Id == acctId);
            }
        }
    }
}
