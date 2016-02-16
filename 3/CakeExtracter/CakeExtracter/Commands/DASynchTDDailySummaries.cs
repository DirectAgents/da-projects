using System.ComponentModel.Composition;
using System.Data.Entity;
using System.IO;
using System.Linq;
using CakeExtracter.Bootstrappers;
using CakeExtracter.Common;
using CakeExtracter.Etl.TradingDesk.Extracters;
using CakeExtracter.Etl.TradingDesk.LoadersDA;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.TD;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class DASynchTDDailySummaries : ConsoleCommand
    {
        public static int RunStatic(int accountId, StreamReader streamReader, string statsType = null)
        {
            AutoMapperBootstrapper.CheckRunSetup();
            var cmd = new DASynchTDDailySummaries
            {
                AccountId = accountId,
                StreamReader = streamReader,
                StatsType = statsType
            };
            int result = cmd.Run();
            return result;
        }

        public int AccountId { get; set; }
        public StreamReader StreamReader { get; set; }
        public string FilePath { get; set; }
        public string StatsType { get; set; }

        public override void ResetProperties()
        {
            AccountId = 0;
            StreamReader = null;
            FilePath = null;
            StatsType = null;
        }

        public DASynchTDDailySummaries()
        {
            IsCommand("daSynchTDDailySummaries", "synch daily summaries via file upload");
            HasRequiredOption<int>("a|accountId=", "Account ID", c => AccountId = c);
            HasOption<string>("f|filePath=", "CSV filepath", c => FilePath = c);
            HasOption<string>("s|statsType=", "Stats Type (default: daily)", c => StatsType = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            var extAccount = GetExtAccount(AccountId);
            if (extAccount != null)
            {
                ColumnMapping mapping = extAccount.Platform.PlatColMapping;
                if (mapping == null)
                    mapping = ColumnMapping.CreateDefault();

                StatsType = (StatsType == null) ? "" : StatsType.ToLower(); // make it lowered and not null

                if (StatsType.StartsWith("strat"))
                    DoETL_Strategy(mapping);
                else if (StatsType.StartsWith("creat"))
                    DoETL_Creative(mapping);
                else if (StatsType.StartsWith("site"))
                    DoETL_Site(mapping);
                else
                    DoETL_Daily(mapping);
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
            var extracter = new TDSiteSummaryExtracter(mapping, streamReader: StreamReader, csvFilePath: FilePath);
            var loader = new TDSiteSummaryLoader(AccountId);
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
        }

        public static ExtAccount GetExtAccount(int acctId)
        {
            using (var db = new DATDContext())
            {
                return db.ExtAccounts
                    .Include(a => a.Platform.PlatColMapping)
                    .SingleOrDefault(a => a.Id == acctId);
            }
        }
    }
}
