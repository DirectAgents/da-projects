using CakeExtracter.Common;
using CakeExtracter.Etl.SearchMarketing.Extracters;
using CakeExtracter.Etl.SearchMarketing.Loaders;
using ClientPortal.Data.Contexts;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class SynchSearchCallDailySummariesCommand : ConsoleCommand
    {
        public int? SearchProfileId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public override void ResetProperties()
        {
            SearchProfileId = null;
            StartDate = null;
            EndDate = null;
        }

        public SynchSearchCallDailySummariesCommand()
        {
            IsCommand("synchSearchCallDailySummaries", "synch CallDailySummaries");
            HasOption<int>("p|searchProfileId=", "SearchProfile Id (default = all)", c => SearchProfileId = c);
            HasOption<DateTime>("s|startDate=", "Start Date (default is yesterday)", c => StartDate = c);
            HasOption<DateTime>("e|endDate=", "End Date (default is yesterday)", c => EndDate = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            var yesterday = DateTime.Today.AddDays(-1);
            var dateRange = new DateRange(StartDate ?? yesterday, EndDate ?? yesterday);

            foreach (var searchProfile in GetSearchProfiles())
            {
                var extracter = new LocalConnexApiExtracter(dateRange, searchProfile.LCaccid, searchProfile.CallMinSeconds);
                var loader = new CallDailySummaryLoader(searchProfile);
                var extracterThread = extracter.Start();
                var loaderThread = loader.Start(extracter);
                extracterThread.Join();
                loaderThread.Join();
            }
            return 0;
        }

        public IEnumerable<SearchProfile> GetSearchProfiles()
        {
            using (var db = new ClientPortalContext())
            {
                var searchProfiles = db.SearchProfiles.AsQueryable();
                if (this.SearchProfileId.HasValue)
                    searchProfiles = searchProfiles.Where(sp => sp.SearchProfileId == this.SearchProfileId.Value);
                return searchProfiles.ToList();
            }
        }

    }
}
