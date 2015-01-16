using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using CakeExtracter.Common;
using CakeExtracter.Etl.TradingDesk.Extracters;
using CakeExtracter.Etl.TradingDesk.Loaders;
using ClientPortal.Data.Entities.TD;
using ClientPortal.Data.Entities.TD.AdRoll;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class TDSynchAdDailySummariesAdroll : ConsoleCommand
    {
        public int? AdRollProfileId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public override void ResetProperties()
        {
            AdRollProfileId = null;
            StartDate = null;
            EndDate = null;
        }

        public TDSynchAdDailySummariesAdroll()
        {
            IsCommand("tdSynchAdDailySummariesAdroll", "synch AdDailySummaries for AdRoll");
            HasOption<int>("p|adrollProfileId=", "AdRollProfile id (default = all)", c => AdRollProfileId = c);
            HasOption<DateTime>("s|startDate=", "Start Date (default is 2 days ago)", c => StartDate = c);
            HasOption<DateTime>("e|endDate=", "End Date (default is yesterday)", c => EndDate = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            var twoDaysAgo = DateTime.Today.AddDays(-2);
            var yesterday = DateTime.Today.AddDays(-1);
            var dateRange = new DateRange(StartDate ?? twoDaysAgo, EndDate ?? yesterday);

            var profiles = GetAdRollProfiles();
            foreach (var profile in profiles)
            {
                var extracter = new AdrollApiExtracter(dateRange, profile.Eid);
                var loader = new AdrollAdDailySummaryLoader2(profile.Id);
                var extracterThread = extracter.Start();
                var loaderThread = loader.Start(extracter);
                extracterThread.Join();
                loaderThread.Join();
            }
            return 0;
        }

        public IEnumerable<AdRollProfile> GetAdRollProfiles()
        {
            using (var db = new TDContext())
            {
                var profiles = db.AdRollProfiles.AsQueryable();
                if (this.AdRollProfileId.HasValue)
                {
                    profiles = profiles.Where(p => p.Id == AdRollProfileId.Value);
                }
                return profiles.ToList();
            }
        }
    }
}
