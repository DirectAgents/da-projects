using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using CakeExtracter.Bootstrappers;
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
        public static int RunStatic(int? accountId = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            AutoMapperBootstrapper.CheckRunSetup();
            var cmd = new DASynchFacebookStats
            {
                AccountId = accountId,
                StartDate = startDate,
                EndDate = endDate
            };
            return cmd.Run();
        }

        public int? AccountId { get; set; }
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
            HasOption<int>("a|accountId=", "Account Id (default = all)", c => AccountId = c);
            HasOption("s|startDate=", "Start Date (default is ... ago)", c => StartDate = DateTime.Parse(c));
            HasOption("e|endDate=", "End Date (default is yesterday)", c => EndDate = DateTime.Parse(c));
        }

        public override int Execute(string[] remainingArguments)
        {
            var today = DateTime.Today;
            var oneMonthAgo = today.AddMonths(-1); //TODO: chg to two months?
            var dateRange = new DateRange(StartDate ?? oneMonthAgo, EndDate ?? today.AddDays(-1));

            var fbUtility = new FacebookUtility(m => Logger.Info(m), m => Logger.Warn(m));

            var accounts = GetAccounts();
            foreach (var acct in accounts)
            {
                var extracter = new FacebookDailySummariesExtracter(dateRange, acct.ExternalId, fbUtility);
                var loader = new FacebookDailySummaryLoader(acct.Id);
                if (loader.ReadyToLoad)
                {
                    var extracterThread = extracter.Start();
                    var loaderThread = loader.Start(extracter);
                    extracterThread.Join();
                    loaderThread.Join();
                }
                else
                {
                    Logger.Warn("FacebookDailySummaryLoader (AccountId: {0}) not ready. Skipping ETL.", acct.Id);
                }
            }

            //var acctId = "act_10153287675738628"; // Crackle
            //var acctId = "act_101672655"; // Zeel consumer
            //var start = new DateTime(2015, 11, 1);
            //var end = new DateTime(2015, 11, 3);
            //var stats = fbUtility.GetDailyStats(acctId, start, end);

            return 0;
        }

        public IEnumerable<ExtAccount> GetAccounts()
        {
            string[] acctIdsArray = new string[] { };
            using (var db = new DATDContext())
            {
                var accounts = db.ExtAccounts.Where(a => a.Platform.Code == Platform.Code_FB);
                if (AccountId.HasValue)
                    accounts = accounts.Where(a => a.Id == AccountId.Value);

                return accounts.ToList().Where(a => !string.IsNullOrWhiteSpace(a.ExternalId));
            }
        }
    }
}
