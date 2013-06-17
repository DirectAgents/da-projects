using System;
using System.Linq;
using ApiClient.Models.Cake;
using Common;
using System.Threading.Tasks;

namespace ApiClient.Etl.Cake
{
    public class DailySummariesFromWebService : Source<DailySummary>
    {
        private readonly double numberDays;

        public DailySummariesFromWebService(int numberDays)
        {
            this.numberDays = -numberDays;
        }

        public override void DoExtract()
        {
            var dateRange = new DateRange(DateTime.Today.AddDays(this.numberDays), DateTime.Today.AddDays(1));
            var offers = CakeWebService.Offers();
            var pidArray = offers.offers.Select(c => c.offer_id).ToArray();
            Parallel.ForEach(pidArray, pid =>
            {
                var dailySummaries = CakeWebService.DailySummaries(pid, dateRange);
                AddItems(dailySummaries);
            });
            Done = true;
        }
    }
}
