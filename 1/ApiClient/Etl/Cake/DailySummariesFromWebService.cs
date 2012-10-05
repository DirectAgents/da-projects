using System;
using System.Linq;
using ApiClient.Models.Cake;
using Common;

namespace ApiClient.Etl.Cake
{
    public class DailySummariesFromWebService : Source<DailySummary>
    {
        public override void DoExtract()
        {
            var dateRange = new DateRange(DateTime.Today.AddDays(-20), DateTime.Today.AddDays(1));
            var offers = CakeWebService.Offers();
            var pidArray = offers.offers.Select(c => c.offer_id).ToArray();
            foreach (var pid in pidArray)
            {
                var dailySummaries = CakeWebService.DailySummaries(pid, dateRange);
                AddItems(dailySummaries);
            }
            Done = true;
        }
    }
}
