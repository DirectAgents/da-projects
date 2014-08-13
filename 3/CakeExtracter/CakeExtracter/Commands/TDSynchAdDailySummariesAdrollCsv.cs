using CakeExtracter.Common;
using CakeExtracter.Etl.TradingDesk.Extracters;
using CakeExtracter.Etl.TradingDesk.Loaders;
using System;
using System.ComponentModel.Composition;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class TDSynchAdDailySummariesAdrollCsv : ConsoleCommand
    {
        public string CsvFile { get; set; }
        public int AdId { get; set; }

        public override void ResetProperties()
        {
            CsvFile = null;
            AdId = -1;
        }

        public TDSynchAdDailySummariesAdrollCsv()
        {
            IsCommand("tdSynchAdDailySummariesAdrollCsv", "synch AdDailySummaries for AdRoll CSV Report");
            HasRequiredOption("f|csvFile=", "CSV File", c => CsvFile = c);
            HasRequiredOption<int>("i|adId=", "AdRollAd id", c => AdId = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            var extracter = new AdrollCsvExtracter(CsvFile);
            var loader = new AdrollAdDailySummaryLoader(AdId);
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
            return 0;
        }

    }
}
