using CakeExtracter.Common;
using CakeExtracter.Etl.SearchMarketing.Extracters;
using CakeExtracter.Etl.SearchMarketing.Loaders;
using System.ComponentModel.Composition;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class SynchSearchDailySummariesBingCsvCommand : ConsoleCommand
    {
        public string CsvFile { get; set; }
        public int SearchAccountId { get; set; }

        public override void ResetProperties()
        {
            CsvFile = null;
            SearchAccountId = 0;
        }

        public SynchSearchDailySummariesBingCsvCommand()
        {
            IsCommand("synchSearchDailySummariesBingCsv",
                      "synch SearchDailySummaries for Bing CSV Report");
            HasRequiredOption("f|csvFile=", "CSV File", c => CsvFile = c);
            HasRequiredOption<int>("a|searchAccountId=", "SearchAccount Id", c => SearchAccountId = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            var extracter = new BingCsvReportExtracter(CsvFile);
            var loader = new BingLoader(SearchAccountId);
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
            return 0;
        }

    }
}
