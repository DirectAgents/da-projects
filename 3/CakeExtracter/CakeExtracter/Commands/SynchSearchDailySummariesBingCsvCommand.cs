﻿using CakeExtracter.Common;
using CakeExtracter.Etl.GoogleAdWords.Extracters;
using CakeExtracter.Etl.GoogleAdWords.Loaders;
using System.ComponentModel.Composition;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class SynchSearchDailySummariesBingCsvCommand : ConsoleCommand
    {
        public SynchSearchDailySummariesBingCsvCommand()
        {
            IsCommand("synchSearchDailySummariesBingCsv",
                      "synch SearchDailySummaries for Bing CSV Report");
            HasRequiredOption("f|csvFile=", "CSV File", c => CsvFile = c);
            HasRequiredOption<int>("v|advertiserId=", "Advertiser Id", c => AdvertiserId = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            var extracter = new BingCsvReportExtracter(CsvFile);
            var loader = new BingLoader(AdvertiserId);
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
            return 0;
        }

        public string CsvFile { get; set; }
        public int AdvertiserId { get; set; }
    }
}
