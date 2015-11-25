using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using CakeExtracter.Common;
using CakeExtracter.Etl.SocialMarketing.Extracters;
using CakeExtracter.Etl.SocialMarketing.LoadersDA;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.TD;
using FacebookAPI;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class DASynchFacebookStats : ConsoleCommand
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public override void ResetProperties()
        {
            StartDate = null;
            EndDate = null;
        }

        public DASynchFacebookStats()
        {
            IsCommand("daSynchFacebookStats", "synch Facebook stats");
            HasOption("s|startDate=", "Start Date (default is ... ago)", c => StartDate = DateTime.Parse(c));
            HasOption("e|endDate=", "End Date (default is yesterday)", c => EndDate = DateTime.Parse(c));
        }

        public override int Execute(string[] remainingArguments)
        {
            var today = DateTime.Today;
            var oneMonthAgo = today.AddMonths(-1); //TODO: chg to two months?
            var dateRange = new DateRange(StartDate ?? oneMonthAgo, EndDate ?? today.AddDays(-1));

            var fbUtility = new FacebookUtility(m => Logger.Info(m), m => Logger.Warn(m));

            var extAccounts = GetExtAccounts();
            foreach (var extAcct in extAccounts)
            {
                var extracter = new FacebookDailySummariesExtracter(dateRange, extAcct.ExternalId, fbUtility);
                var loader = new FacebookDailySummaryLoader(extAcct.Id);
                if (loader.ReadyToLoad)
                {
                    var extracterThread = extracter.Start();
                    var loaderThread = loader.Start(extracter);
                    extracterThread.Join();
                    loaderThread.Join();
                }
                else
                {
                    Logger.Warn("FacebookDailySummaryLoader (ExtAcctId: {0}) not ready. Skipping ETL.", extAcct.Id);
                }
            }

            //var acctId = "act_10153287675738628"; // Crackle
            //var acctId = "act_101672655"; // Zeel consumer
            //var start = new DateTime(2015, 11, 1);
            //var end = new DateTime(2015, 11, 3);
            //var stats = fbUtility.GetDailyStats(acctId, start, end);

            return 0;
        }

        public IEnumerable<ExtAccount> GetExtAccounts()
        {
            string[] acctIdsArray = new string[] { };
            using (var db = new DATDContext())
            {
                var extAccounts = db.ExtAccounts.Where(a => a.Platform.Code == Platform.Code_FB);
                // if (some id) ...hasvalue ...extAccounts = extAccounts.Where()

                return extAccounts.ToList().Where(a => !string.IsNullOrWhiteSpace(a.ExternalId));
            }
        }
    }
}
