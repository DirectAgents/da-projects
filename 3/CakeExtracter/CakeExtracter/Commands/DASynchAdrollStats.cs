using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using AdRoll;
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
        // if Eid not specified, will ask AdRoll for all Advertisables that have stats
        public static int RunStatic(string advertisableEid = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            var cmd = new DASynchAdrollStats
            {
                AdvertisableEid = advertisableEid,
                CheckActiveAdvertisables = string.IsNullOrWhiteSpace(advertisableEid),
                StartDate = startDate,
                EndDate = endDate
            };
            cmd.Run();
            cmd.OneStatPerAdvertisable = true;
            return cmd.Run();
        }

        public int? AdvertisableId { get; set; }
        public string AdvertisableEid { get; set; }
        public bool CheckActiveAdvertisables { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool OneStatPerAdvertisable { get; set; } // (per day)

        public override void ResetProperties()
        {
            AdvertisableId = null;
            AdvertisableEid = null;
            CheckActiveAdvertisables = false;
            StartDate = null;
            EndDate = null;
            OneStatPerAdvertisable = false;
        }

        public DASynchAdrollStats()
        {
            IsCommand("daSynchAdrollStats", "synch AdRoll Stats");
            HasOption("a|advertisableEid=", "Advertisable Eid (default = all)", c => AdvertisableEid = c);
            HasOption<int>("i|advertisableId=", "Advertisable Id (default = all)", c => AdvertisableId = c);
            HasOption("c|checkActive=", "Check AdRoll for Advertisables with stats (if none specified)", c => bool.Parse(c));
            HasOption("s|startDate=", "Start Date (default is one month ago)", c => StartDate = DateTime.Parse(c));
            HasOption("e|endDate=", "End Date (default is yesterday)", c => EndDate = DateTime.Parse(c));
            HasOption<bool>("o|oneStatPerAdv=", "One Stat per advertisable per day (default = false / one per ad)", c => OneStatPerAdvertisable = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            var today = DateTime.Today;
            var oneMonthAgo = today.AddMonths(-1);
            var dateRange = new DateRange(StartDate ?? oneMonthAgo, EndDate ?? today.AddDays(-1));

            var arUtility = new AdRollUtility(m => Logger.Info(m), m => Logger.Warn(m));

            IEnumerable<Advertisable> advertisables;
            if (CheckActiveAdvertisables && string.IsNullOrWhiteSpace(AdvertisableEid) && !AdvertisableId.HasValue)
                advertisables = GetAdvertisablesThatHaveStats(dateRange, arUtility);
            else
                advertisables = GetAdvertisables();

            if (OneStatPerAdvertisable)
                DoETL_AdvertisableLevel(dateRange, advertisables);
            else
                DoETL_AdLevel(dateRange, advertisables);

            return 0;
        }

        private void DoETL_AdvertisableLevel(DateRange dateRange, IEnumerable<Advertisable> advertisables, AdRollUtility arUtility = null)
        {
            foreach (var adv in advertisables)
            {
                var extracter = new AdrollDailySummariesExtracter(dateRange, adv.Eid, arUtility);
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
        }

        private void DoETL_AdLevel(DateRange dateRange, IEnumerable<Advertisable> advertisables, AdRollUtility arUtility = null)
        {
            foreach (var adv in advertisables)
            {
                var extracter = new AdrollAdDailySummariesExtracter(dateRange, adv.Eid, arUtility);
                var loader = new AdrollAdDailySummaryLoader(adv.Id);
                var extracterThread = extracter.Start();
                var loaderThread = loader.Start(extracter);
                extracterThread.Join();
                loaderThread.Join();
            }
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

        public IEnumerable<Advertisable> GetAdvertisablesThatHaveStats(DateRange dateRange, AdRollUtility arUtility)
        {
            IEnumerable<Advertisable> advertisables;
            using (var db = new DATDContext())
            {
                advertisables = db.Advertisables.ToList();
            }
            var dbAdvEids = advertisables.Select(a => a.Eid).ToArray();
            var advSums = arUtility.AdvertisableSummaries(dateRange.FromDate, dateRange.ToDate, dbAdvEids);
            var advEidsThatHaveStats = advSums.Where(s => !s.AllZeros(includeProspects: true)).Select(s => s.eid).ToArray();
            advertisables = advertisables.Where(a => advEidsThatHaveStats.Contains(a.Eid));
            return advertisables;
        }
    }
}
