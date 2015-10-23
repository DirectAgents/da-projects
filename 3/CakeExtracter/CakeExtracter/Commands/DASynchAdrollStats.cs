using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using AdRoll;
using CakeExtracter.Bootstrappers;
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
        public static int RunStatic(string advertisableEids = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            AutoMapperBootstrapper.CheckRunSetup();
            var cmd = new DASynchAdrollStats
            {
                AdvertisableEids = advertisableEids,
                CheckActiveAdvertisables = string.IsNullOrWhiteSpace(advertisableEids),
                StartDate = startDate,
                EndDate = endDate
            };
            cmd.Run();
            cmd.OneStatPerAdvertisable = true;
            return cmd.Run();
        }

        public int? AdvertisableId { get; set; }
        public string AdvertisableEids { get; set; } // if NullOrWhitespace, gets filled in during Run()
        public bool CheckActiveAdvertisables { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool OneStatPerAdvertisable { get; set; } // (per day)

        public override void ResetProperties()
        {
            AdvertisableId = null;
            AdvertisableEids = null;
            CheckActiveAdvertisables = false;
            StartDate = null;
            EndDate = null;
            OneStatPerAdvertisable = false;
        }

        public DASynchAdrollStats()
        {
            IsCommand("daSynchAdrollStats", "synch AdRoll Stats");
            HasOption("a|advertisableEids=", "Advertisable Eids (comma-separated) (default = all)", c => AdvertisableEids = c);
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
            if (CheckActiveAdvertisables && string.IsNullOrWhiteSpace(AdvertisableEids) && !AdvertisableId.HasValue)
                advertisables = GetAdvertisablesThatHaveStats(dateRange, arUtility);
            else
                advertisables = GetAdvertisables();

            if (string.IsNullOrWhiteSpace(AdvertisableEids))
                AdvertisableEids = String.Join(",", advertisables.Select(a => a.Eid));

            if (OneStatPerAdvertisable)
                DoETL_AdvertisableLevel(dateRange, advertisables);
            else
                DoETL_AdLevel(dateRange, advertisables);

            return 0;
        }

        private void DoETL_AdvertisableLevel(DateRange dateRange, IEnumerable<Advertisable> advertisables, AdRollUtility arUtility = null)
        {
            //TODO: If there are fewer dates than advertisables, can we loop through the dates and make one API call for each date?

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
            //TODO: Loop thru dateRange. For each date, make one API call.
            //      (Need to update Loader- pass in advertisables(?)... needed for creating new Ads in the DB... Items have Advertisable *name*, not Eid.)

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
            string[] advEidsArray = new string[] { };
            if (!string.IsNullOrWhiteSpace(this.AdvertisableEids))
                advEidsArray = this.AdvertisableEids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            using (var db = new DATDContext())
            {
                var advs = db.Advertisables.AsQueryable();
                if (advEidsArray.Any() && this.AdvertisableId.HasValue)
                {   // Handles if advs are specified by both Eid and Id
                    advs = advs.Where(a => advEidsArray.Contains(a.Eid) || a.Id == this.AdvertisableId.Value);
                }
                else if (advEidsArray.Any())
                {
                    advs = advs.Where(a => advEidsArray.Contains(a.Eid));
                }
                else if (this.AdvertisableId.HasValue)
                {
                    advs = advs.Where(a => a.Id == this.AdvertisableId.Value);
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
