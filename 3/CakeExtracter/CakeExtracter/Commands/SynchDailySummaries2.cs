using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using CakeExtracter.Common;
using CakeExtracter.Etl.CakeMarketing.Extracters;
using CakeExtracter.Etl.CakeMarketing.Loaders;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class SynchDailySummaries2Command : ConsoleCommand
    {
        public int OfferId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int DaysAgoToStart { get; set; }
        public int DaysToInclude { get; set; }

        public override void ResetProperties()
        {
            OfferId = 0;
            StartDate = null;
            EndDate = null;
            DaysAgoToStart = 0;
            DaysToInclude = 0;
        }

        public SynchDailySummaries2Command()
        {
            IsCommand("synchDailySummaries2", "synch DailySummaries with one Cake API call per day");
            HasOption<int>("o|offerId=", "Offer Id (default = 0 / all offers)", c => OfferId = c);
            HasOption("s|startDate=", "Start Date (default is today)", c => StartDate = DateTime.Parse(c));
            HasOption("e|endDate=", "End Date (default is startDate)", c => EndDate = DateTime.Parse(c));
            HasOption<int>("d|daysAgo=", "Days Ago to start, if startDate not specified (default = 0, i.e. today)", c => DaysAgoToStart = c);
            HasOption<int>("i|daysToInclude=", "Days to include, if endDate not specified (default = 1)", c => DaysToInclude = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            if (DaysToInclude < 1) DaysToInclude = 1; // used if EndDate==null
            DateTime from = StartDate ?? DateTime.Today.AddDays(-DaysAgoToStart); // default: today
            DateTime to = EndDate ?? from.AddDays(DaysToInclude - 1); // default: whatever from is
            var dateRange = new DateRange(from, to);
            foreach (var date in dateRange.Dates)
            {
                //var existingDailySummaries = DailySummaries(date);
                var initialOffAffs = GetOffAffs(date);

                var extracter = new CampaignSummaryExtracter(date, offerId: OfferId);
                var loader = new CampaignSummaryLoader(date);
                var extracterThread = extracter.Start();
                var loaderThread = loader.Start(extracter);
                extracterThread.Join();
                loaderThread.Join();

                DeleteOldDailySummaries(date, initialOffAffs, loader.GetLoadedOffAffs());
            }
            return 0;
        }

        // For the specified date, get a list of the existing offerId/affId combinations that have a dailySummary in the db.
        public List<Tuple<int, int>> GetOffAffs(DateTime date)
        {
            using (var db = new ClientPortal.Data.Contexts.ClientPortalContext())
            {
                var daySums = db.DailySummaries.Where(ds => ds.Date == date).ToList();
                return daySums.Select(ds => new Tuple<int, int>(ds.OfferId, ds.AffiliateId)).ToList();
            }
        }

        public void DeleteOldDailySummaries(DateTime date, List<Tuple<int, int>> initialOffAffs, Dictionary<Tuple<int, int>, int> loadedOffAffs)
        {
            using (var db = new ClientPortal.Data.Contexts.ClientPortalContext())
            {
                foreach (var offAff in initialOffAffs)
                {
                    if (!loadedOffAffs.ContainsKey(offAff))
                    {
                        var ds = db.DailySummaries.Find(date, offAff.Item1, offAff.Item2);
                        if (ds != null)
                        {
                            Logger.Info("Deleting DailySummary for {0:d}, OffId {1}, AffId {2}",
                                        date, offAff.Item1, offAff.Item2);
                            db.DailySummaries.Remove(ds);
                        }
                    }
                }
                db.SaveChanges();
            }
        }

        // The plan:
        // - one cake call per day (CampaignSummaries)
        // - check for all off/aff combos beforehand...
        // - compare with what's there afterward.
        // - delete any that need to be deleted
    }
}
