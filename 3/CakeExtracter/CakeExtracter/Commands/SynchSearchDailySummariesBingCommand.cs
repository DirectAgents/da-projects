using CakeExtracter.Common;
using CakeExtracter.Etl.SearchMarketing.Extracters;
using CakeExtracter.Etl.SearchMarketing.Loaders;
using System;
using System.ComponentModel.Composition;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class SynchSearchDailySummariesBingCommand : ConsoleCommand
    {
        public SynchSearchDailySummariesBingCommand()
        {
            IsCommand("synchSearchDailySummariesBing",
                      "synch SearchDailySummaries for Bing API Report");
            HasRequiredOption<int>("v|advertiserId=", "Advertiser Id", c => AdvertiserId = c);
            HasRequiredOption<int>("a|accountId=", "Account Id", c => AccountId = c);
            HasRequiredOption<DateTime>("s|startDate=", "Start Date", c => StartDate = c);
            HasRequiredOption<DateTime>("e|endDate=", "End Date", c => EndDate = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            var extracter = new BingDailySummaryExtracter(AccountId, StartDate, EndDate);
            var loader = new BingLoader(AdvertiserId);
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
            return 0;
        }

        public int AdvertiserId { get; set; }
        public int AccountId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
