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
        public static int RunStatic(int accountId, StreamReader streamReader)
        {
            AutoMapperBootstrapper.CheckRunSetup();
            var cmd = new DASynchTDDailySummaries
            {
                AccountId = accountId,
                StreamReader = streamReader
            };
            int result = cmd.Run();
            return result;
        }

        public int AccountId { get; set; }
        public StreamReader StreamReader { get; set; }
        public string FilePath { get; set; }

        public override void ResetProperties()
        {
            AccountId = 0;
            StreamReader = null;
            FilePath = null;
        }

        public DASynchTDDailySummaries()
        {
            IsCommand("daSynchTDDailySummaries", "synch daily summaries via file upload");
            HasRequiredOption<int>("a|accountId=", "Account ID", c => AccountId = c);
            HasOption<string>("f|filePath=", "CSV filepath", c => FilePath = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            //FilePath = @"G:\work\tradingdesk\yam\12-09-2015_bevel.csv";
            //int acctId = 1110;
            var extAccount = GetExtAccount(AccountId);
            if (extAccount != null)
            {
                //var mapping = new PlatColMapping
                //{
                //    //PlatformId = 
                //    Date = "Day",
                //    Cost = "Inventory Cost",
                //    Impressions = "Impressions",
                //    Clicks = "Clicks",
                //    PostClickConv = "Conversions",
                //    PostViewConv = null
                //};
                ColumnMapping mapping = extAccount.Platform.PlatColMapping;
                if (mapping == null)
                    mapping = ColumnMapping.CreateDefault();

                var extracter = new TDDailySummaryExtracter(mapping, streamReader: StreamReader, csvFilePath: FilePath);
                var loader = new TDDailySummaryLoader(AccountId);
                var extracterThread = extracter.Start();
                var loaderThread = loader.Start(extracter);
                extracterThread.Join();
                loaderThread.Join();
            }
            else
            {
                Logger.Warn("ExtAccount ({0}) not found. Skipping ETL.", AccountId);
            }
            return 0;
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
