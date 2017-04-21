using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using CakeExtracter.Common;
using CakeExtracter.Etl.TradingDesk.Extracters;
using CakeExtracter.Etl.TradingDesk.LoadersDA;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using Yahoo;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class DASynchYAMStats : ConsoleCommand
    {
        public int? AccountId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string StatsType { get; set; }

        private YAMUtility yamUtility { get; set; }

        public override void ResetProperties()
        {
            AccountId = null;
            StartDate = null;
            EndDate = null;
            StatsType = null;
        }

        public DASynchYAMStats()
        {
            IsCommand("daSynchYAMStats", "synch YAM Stats");
            HasOption<int>("a|accountId=", "Account Id (default = all)", c => AccountId = c);
            HasOption("s|startDate=", "Start Date (default is seven days ago)", c => StartDate = DateTime.Parse(c));
            HasOption("e|endDate=", "End Date (default is yesterday)", c => EndDate = DateTime.Parse(c));
            HasOption<string>("t|statsType=", "Stats Type (default: all)", c => StatsType = c);
        }

        //TODO: if synching all accounts, can we make one API call to get everything?

        public override int Execute(string[] remainingArguments)
        {
            var today = DateTime.Today;
            var oneWeekAgo = today.AddDays(-7);
            var dateRange = new DateRange(StartDate ?? oneWeekAgo, EndDate ?? today.AddDays(-1));

            var statsType = new StatsTypeAgg(this.StatsType);
            SetupYAMUtility();

            var accounts = GetAccounts();
            foreach (var account in accounts)
            {
                if (statsType.Daily)
                    DoETL_Daily(dateRange, account);
                if (statsType.Strategy)
                    DoETL_Strategy(dateRange, account);
                if (statsType.Creative)
                    DoETL_Creative(dateRange, account);
            }
            SaveTokens();
            return 0;
        }
        private void SetupYAMUtility()
        {
            this.yamUtility = new YAMUtility(m => Logger.Info(m), m => Logger.Warn(m));
            using (var db = new ClientPortalProgContext())
            {
                var platYAM = db.Platforms.Single(x => x.Code == Platform.Code_YAM);
                if (String.IsNullOrWhiteSpace(platYAM.Tokens))
                {
                    yamUtility.GetAccessToken();
                }
                else
                {
                    var tokens = platYAM.Tokens.Split(new string[] { YAMUtility.TOKEN_DELIMITER }, StringSplitOptions.None);
                    yamUtility.AccessToken = tokens[0];
                    if (tokens.Length > 1)
                        yamUtility.RefreshToken = tokens[1];
                }
            }
        }
        private void SaveTokens()
        {
            using (var db = new ClientPortalProgContext())
            {
                var platYAM = db.Platforms.Single(x => x.Code == Platform.Code_YAM);
                platYAM.Tokens = yamUtility.AccessToken + YAMUtility.TOKEN_DELIMITER + yamUtility.RefreshToken;
                db.SaveChanges();
            }
        }

        private void DoETL_Daily(DateRange dateRange, ExtAccount account)
        {
            var extracter = new YAMDailySummaryExtracter(yamUtility, dateRange, account);
            var loader = new TDDailySummaryLoader(account.Id);
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
        }
        private void DoETL_Strategy(DateRange dateRange, ExtAccount account)
        {
            var extracter = new YAMStrategySummaryExtracter(yamUtility, dateRange, account);
            var loader = new TDStrategySummaryLoader(account.Id);
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
        }
        private void DoETL_Creative(DateRange dateRange, ExtAccount account)
        {
            var extracter = new YAMTDadSummaryExtracter(yamUtility, dateRange, account);
            var loader = new TDadSummaryLoader(account.Id);
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
        }

        private IEnumerable<ExtAccount> GetAccounts()
        {
            string[] acctIdsArray = new string[] { };
            using (var db = new ClientPortalProgContext())
            {
                var accounts = db.ExtAccounts.Include("Platform.PlatColMapping").Where(a => a.Platform.Code == Platform.Code_YAM);
                if (AccountId.HasValue)
                    accounts = accounts.Where(a => a.Id == AccountId.Value);

                return accounts.ToList().Where(a => !string.IsNullOrWhiteSpace(a.ExternalId));
            }
        }

    }
}
