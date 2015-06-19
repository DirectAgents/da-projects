using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using CakeExtracter.Common;
using CakeExtracter.Etl.TradingDesk.Extracters;
using CakeExtracter.Etl.TradingDesk.LoadersDA;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.AdRoll;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class DASynchAdrollStats : ConsoleCommand
    {
        public int? AdvertisableId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public override void ResetProperties()
        {
            AdvertisableId = null;
            StartDate = null;
            EndDate = null;
        }

        public DASynchAdrollStats()
        {
            IsCommand("daSynchAdrollStats", "synch AdRoll Stats");
            HasOption<int>("a|advertisableId=", "Advertisable Id (default = all)", c => AdvertisableId = c);
            HasOption("s|startDate=", "Start Date (default is one week ago)", c => StartDate = DateTime.Parse(c));
            HasOption("e|endDate=", "End Date (default is today)", c => EndDate = DateTime.Parse(c));
        }

        public override int Execute(string[] remainingArguments)
        {
            var today = DateTime.Today;
            var sevenDaysAgo = today.AddDays(-7);
            var dateRange = new DateRange(StartDate ?? sevenDaysAgo, EndDate ?? today);

            var advertisables = GetAdvertisables();
            foreach (var adv in advertisables)
            {
                var extracter = new AdrollDailySummariesExtracter(dateRange, adv.Eid);
                var loader = new AdrollAdvertisableStatsLoader(adv.Id);
                var extracterThread = extracter.Start();
                var loaderThread = loader.Start(extracter);
                extracterThread.Join();
                loaderThread.Join();
            }
            return 0;
        }

        public IEnumerable<Advertisable> GetAdvertisables()
        {
            using (var db = new DAContext())
            {
                var advs = db.Advertisables.AsQueryable();
                if (this.AdvertisableId.HasValue)
                {
                    advs = advs.Where(a => a.Id == AdvertisableId.Value);
                }
                return advs.ToList();
            }
        }
    }
}
