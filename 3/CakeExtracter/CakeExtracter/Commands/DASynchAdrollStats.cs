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
        public static int RunStatic(string advertisableEid, DateTime? startDate = null, DateTime? endDate = null)
        {
            var cmd = new DASynchAdrollStats
            {
                AdvertisableEid = advertisableEid,
                StartDate = startDate,
                EndDate = endDate
            };
            cmd.Run();
            cmd.OneStatPerAdvertisable = true;
            return cmd.Run();
        }

        public int? AdvertisableId { get; set; }
        public string AdvertisableEid { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool OneStatPerAdvertisable { get; set; } // (per day)

        public override void ResetProperties()
        {
            AdvertisableId = null;
            AdvertisableEid = null;
            StartDate = null;
            EndDate = null;
            OneStatPerAdvertisable = false;
        }

        public DASynchAdrollStats()
        {
            IsCommand("daSynchAdrollStats", "synch AdRoll Stats");
            HasOption("a|advertisableEid=", "Advertisable Eid (default = all)", c => AdvertisableEid = c);
            HasOption<int>("i|advertisableId=", "Advertisable Id (default = all)", c => AdvertisableId = c);
            HasOption("s|startDate=", "Start Date (default is one week ago)", c => StartDate = DateTime.Parse(c));
            HasOption("e|endDate=", "End Date (default is yesterday)", c => EndDate = DateTime.Parse(c));
            HasOption<bool>("o|oneStatPerAdv=", "One Stat per advertisable per day (default = false / one per ad)", c => OneStatPerAdvertisable = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            var yesterday = DateTime.Today.AddDays(-1);
            var sevenDaysAgo = yesterday.AddDays(-6);
            var dateRange = new DateRange(StartDate ?? sevenDaysAgo, EndDate ?? yesterday);

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
                if (!string.IsNullOrWhiteSpace(this.AdvertisableEid) && this.AdvertisableId.HasValue)
                {   // Handles if two different advs are specified
                    advs = advs.Where(a => a.Eid == AdvertisableEid || a.Id == AdvertisableId.Value);
                }
                else if (!String.IsNullOrWhiteSpace(this.AdvertisableEid))
                {
                    advs = advs.Where(a => a.Eid == AdvertisableEid);
                }
                else if (this.AdvertisableId.HasValue)
                {
                    advs = advs.Where(a => a.Id == AdvertisableId.Value);
                }
                return advs.ToList();
            }
        }
    }
}
