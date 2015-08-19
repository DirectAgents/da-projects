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
        public bool OneStatPerAdvertisable { get; set; } // (per day)

        public override void ResetProperties()
        {
            AdvertisableId = null;
            StartDate = null;
            EndDate = null;
            OneStatPerAdvertisable = false;
        }

        public DASynchAdrollStats()
        {
            IsCommand("daSynchAdrollStats", "synch AdRoll Stats");
            HasOption<int>("a|advertisableId=", "Advertisable Id (default = all)", c => AdvertisableId = c);
            HasOption("s|startDate=", "Start Date (default is one week ago)", c => StartDate = DateTime.Parse(c));
            HasOption("e|endDate=", "End Date (default is today)", c => EndDate = DateTime.Parse(c));
            HasOption<bool>("o|oneStatPerAdv=", "One Stat per advertisable per day (default = false / one per ad)", c => OneStatPerAdvertisable = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            var today = DateTime.Today;
            var sevenDaysAgo = today.AddDays(-7);
            var dateRange = new DateRange(StartDate ?? sevenDaysAgo, EndDate ?? today);

            var advertisables = GetAdvertisables();
            foreach (var adv in advertisables)
            {
                if (OneStatPerAdvertisable)
                    DoETL_AdvertisableLevel(dateRange, adv);
                else
                    DoETL_AdLevel(dateRange, adv);
            }
            return 0;
        }

        private void DoETL_AdvertisableLevel(DateRange dateRange, Advertisable adv)
        {
            var extracter = new AdrollDailySummariesExtracter(dateRange, adv.Eid);
            var loader = new AdrollDailySummaryLoader(adv.Eid);
            if (loader.FoundAccount())
            {
                var extracterThread = extracter.Start();
                var loaderThread = loader.Start(extracter);
                extracterThread.Join();
                loaderThread.Join();
            }
            else
                Logger.Warn("AdRoll Account did not exist for Advertisable with Eid {0}. Cannot do ETL.", adv.Eid);
        }
        private void DoETL_AdLevel(DateRange dateRange, Advertisable adv)
        {
            var extracter = new AdrollAdDailySummariesExtracter(dateRange, adv.Eid);
            var loader = new AdrollAdDailySummaryLoader(adv.Id);
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
        }

        public IEnumerable<Advertisable> GetAdvertisables()
        {
            using (var db = new DATDContext())
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
