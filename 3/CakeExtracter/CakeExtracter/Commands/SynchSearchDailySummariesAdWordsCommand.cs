using System;
using System.ComponentModel.Composition;
using CakeExtracter.Common;
using CakeExtracter.Etl.SearchMarketing.Extracters;
using CakeExtracter.Etl.SearchMarketing.Loaders;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class SynchSearchDailySummariesAdWordsCommand : ConsoleCommand
    {
        public SynchSearchDailySummariesAdWordsCommand()
        {
            IsCommand("synchSearchDailySummariesAdWords", "synch SearchDailySummaries for Bing API Report");
            HasRequiredOption<string>("v|clientCustomerId=", "Client Customer Id", c => ClientCustomerId = c);
            HasRequiredOption<DateTime>("s|startDate=", "Start Date", c => StartDate = c);
            HasRequiredOption<DateTime>("e|endDate=", "End Date", c => EndDate = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            var extracter = new AdWordsApiExtracter(ClientCustomerId, StartDate, EndDate);
            var loader = new AdWordsApiLoader();
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
            return 0;
        }

        public string ClientCustomerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
