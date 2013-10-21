using CakeExtracter.Common;
using CakeExtracter.Etl.SearchMarketing.Extracters;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class SynchSearchDailySummariesAnalyticsCommand : ConsoleCommand
    {
        public int AdvertiserId { get; set; }
        public string ClientCustomerId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public override void ResetProperties()
        {
            AdvertiserId = 0;
            ClientCustomerId = null;
            StartDate = null;
            EndDate = null;
        }

        public SynchSearchDailySummariesAnalyticsCommand()
        {
            IsCommand("synchSearchDailySummariesAnalytics", "synch SearchDailySummaries for Google Analytics");
            HasOption<int>("aid|advertiserId=", "Advertiser Id (default is 0, meaning all search advertisers, unless Client Customer Id specified)", c => AdvertiserId = c);
            HasOption<string>("v|clientCustomerId=", "Client Customer Id", c => ClientCustomerId = c);
            HasOption<DateTime>("s|startDate=", "Start Date (default is one month ago)", c => StartDate = c);
            HasOption<DateTime>("e|endDate=", "End Date (default is yesterday)", c => EndDate = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            var oneMonthAgo = DateTime.Today.AddMonths(-1);
            var yesterday = DateTime.Today.AddDays(-1);
            var dateRange = new DateRange(StartDate ?? oneMonthAgo, EndDate ?? yesterday);

            foreach (var clientId in SynchSearchDailySummariesAdWordsCommand.GetClientCustomerIds(this.AdvertiserId, this.ClientCustomerId))
            {
                var extracter = new AnalyticsApiExtracter(clientId, dateRange);
                //var loader = new AdWordsApiLoader();
                var extracterThread = extracter.Start();
                //var loaderThread = loader.Start(extracter);
                extracterThread.Join();
                //loaderThread.Join();
            }
            return 0;
        }
    }
}
