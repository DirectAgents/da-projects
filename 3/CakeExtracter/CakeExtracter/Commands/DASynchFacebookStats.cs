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
        public string StatsType { get; set; }

        public override void ResetProperties()
        {
            StartDate = null;
            EndDate = null;
            StatsType = null;
        }

        public DASynchFacebookStats()
        {
            IsCommand("daSynchFacebookStats", "synch Facebook stats");
            HasOption<int>("a|accountId=", "Account Id (default = all)", c => AccountId = c);
            HasOption("s|startDate=", "Start Date (default is one month ago)", c => StartDate = DateTime.Parse(c));
            HasOption("e|endDate=", "End Date (default is yesterday)", c => EndDate = DateTime.Parse(c));
            HasOption<string>("t|statsType=", "Stats Type (default: daily)", c => StatsType = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            var today = DateTime.Today;
            var oneMonthAgo = today.AddMonths(-1);
            var dateRange = new DateRange(StartDate ?? oneMonthAgo, EndDate ?? today.AddDays(-1));

            StatsType = (StatsType == null) ? "" : StatsType.ToLower(); // make it lowered and not null
            var fbUtility = new FacebookUtility(m => Logger.Info(m), m => Logger.Warn(m));

            var accounts = GetAccounts();
            foreach (var acct in accounts)
            {
                if (StatsType.StartsWith("strat"))
                    DoETL_Strategy(dateRange, acct, fbUtility);
                else if (StatsType.StartsWith("creat"))
                    DoETL_Creative(dateRange, acct, fbUtility);
                //else if (StatsType.StartsWith("site"))
                //    DoETL_Site(dateRange, acct, fbUtility);
                else
                    DoETL_Daily(dateRange, acct, fbUtility);
            }

            return 0;
        }

        public void DoETL_Daily(DateRange dateRange, ExtAccount account, FacebookUtility fbUtility = null)
        {
            var extracter = new FacebookDailySummaryExtracter(dateRange, account.ExternalId, fbUtility);
            var loader = new FacebookDailySummaryLoader(account.Id);
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
        }
        public void DoETL_Strategy(DateRange dateRange, ExtAccount account, FacebookUtility fbUtility = null)
        {
            var extracter = new FacebookCampaignSummaryExtracter(dateRange, account.ExternalId, fbUtility);
            var loader = new FacebookCampaignSummaryLoader(account.Id);
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
        }
        public void DoETL_Creative(DateRange dateRange, ExtAccount account, FacebookUtility fbUtility = null)
        {
            var extracter = new FacebookAdSummaryExtracter(dateRange, account.ExternalId, fbUtility);
            var loader = new FacebookAdSummaryLoader(account.Id);
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
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
