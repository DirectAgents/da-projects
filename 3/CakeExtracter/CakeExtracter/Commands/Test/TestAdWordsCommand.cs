using System;
using System.ComponentModel.Composition;
using System.Linq;
using CakeExtracter.Common;
using ClientPortal.Data.Contexts;

namespace CakeExtracter.Commands.Test
{
    [Export(typeof(ConsoleCommand))]
    public class TestAdWordsCommand : ConsoleCommand
    {
        public override void ResetProperties()
        {
        }

        public TestAdWordsCommand()
        {
            IsCommand("testAdWords", "test AdWords");
        }

        public override int Execute(string[] remainingArguments)
        {
            //FillStats();
            return 0;
        }

        public void FillStats()
        {
            var dateRange = new DateRange(new DateTime(2017, 9, 25), new DateTime(2017, 10, 2));

            using (var db = new ClientPortalContext())
            {
                var campIds = db.SearchCampaigns.Where(x => x.SearchAccountId == 161).Select(x => x.SearchCampaignId).ToArray(); // priv

                var sdsQuery = db.SearchDailySummaries.Where(x => campIds.Contains(x.SearchCampaignId));

                foreach (var date in dateRange.Dates)
                {
                    var sdsForDate = sdsQuery.Where(x => x.Date == date).Select(x => x.SearchCampaignId).Distinct().ToArray();
                    var missingCampIds = campIds.Where(x => !sdsForDate.Contains(x)).ToArray();
                    foreach (var campId in missingCampIds)
                    {
                        var sds = new SearchDailySummary
                        {
                            SearchCampaignId = campId,
                            Date = date,
                            Network = "S",
                            Device = "M"
                        };
                        db.SearchDailySummaries.Add(sds);
                    }
                    db.SaveChanges();
                }
            }
        }

    }
}
