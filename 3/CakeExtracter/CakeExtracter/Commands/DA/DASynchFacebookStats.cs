using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using CakeExtracter.Bootstrappers;
using CakeExtracter.Common;
using CakeExtracter.Etl.SocialMarketing.Extracters;
using CakeExtracter.Etl.SocialMarketing.LoadersDA;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using FacebookAPI;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class DASynchFacebookStats : ConsoleCommand
    {
        public static int RunStatic(int? accountId = null, DateTime? startDate = null, DateTime? endDate = null, string statsType = null)
        {
            AutoMapperBootstrapper.CheckRunSetup();
            var cmd = new DASynchFacebookStats
            {
                AccountId = accountId,
                StartDate = startDate,
                EndDate = endDate,
                StatsType = statsType
            };
            return cmd.Run();
        }

        public int? AccountId { get; set; }
        public int? CampaignId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string StatsType { get; set; }

        public override void ResetProperties()
        {
            AccountId = null;
            CampaignId = null;
            StartDate = null;
            EndDate = null;
            StatsType = null;
        }

        public DASynchFacebookStats()
        {
            IsCommand("daSynchFacebookStats", "synch Facebook stats");
            HasOption<int>("a|accountId=", "Account Id (default = all)", c => AccountId = c);
            HasOption<int>("c|campaignId=", "Campaign Id (optional)", c => CampaignId = c);
            HasOption("s|startDate=", "Start Date (default is seven days ago)", c => StartDate = DateTime.Parse(c));
            HasOption("e|endDate=", "End Date (default is yesterday)", c => EndDate = DateTime.Parse(c));
            HasOption<string>("t|statsType=", "Stats Type (default: all)", c => StatsType = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            var today = DateTime.Today;
            var oneWeekAgo = today.AddDays(-7);
            var dateRange = new DateRange(StartDate ?? oneWeekAgo, EndDate ?? today.AddDays(-1));

            var fbUtility = new FacebookUtility(m => Logger.Info(m), m => Logger.Warn(m));
            var statsType = new StatsTypeAgg(this.StatsType);

            var string_ConvAsMobAppInst = ConfigurationManager.AppSettings["FB_ConversionsAsMobileAppInstalls"] ?? "";
            var Accts_ConvAsMobAppInst = string_ConvAsMobAppInst.Split(new char[] { ',' });
            var string_ConvAsPurch = ConfigurationManager.AppSettings["FB_ConversionsAsPurchases"] ?? "";
            var Accts_ConvAsPurch = string_ConvAsPurch.Split(new char[] { ',' });
            var string_ConvAsReg = ConfigurationManager.AppSettings["FB_ConversionsAsRegistrations"] ?? "";
            var Accts_ConvAsReg = string_ConvAsReg.Split(new char[] { ',' });

            var Accts_DailyOnly = new string[] { };
            if (!AccountId.HasValue || statsType.All)
            {
                var string_DailyOnly = ConfigurationManager.AppSettings["FB_DailyStatsOnly"] ?? "";
                Accts_DailyOnly = string_DailyOnly.Split(new char[] { ',' });
            }   // Used when synching all accounts AND/OR all stats types...
            // So if an account is marked as "daily only", you can only load other stats by specifying the accountId and statsType
            // TODO? remove this since we now handle exceptions in the extracter?

            var accounts = GetAccounts();
            foreach (var acct in accounts)
            {
                fbUtility.SetAll();
                if (acct.Network != null)
                {
                    string network = Regex.Replace(acct.Network.Name, @"\s+", "").ToUpper();
                    if (network == "FACEBOOK")
                        fbUtility.SetFacebook();
                    else if (network == "INSTAGRAM")
                        fbUtility.SetInstagram();
                    else if (network == "AUDIENCENETWORK")
                        fbUtility.SetAudienceNetwork();
                }

                if (Accts_ConvAsMobAppInst.Contains(acct.ExternalId))
                    fbUtility.Conversion_ActionType = FacebookUtility.Conversion_ActionType_MobileAppInstall;
                else if (Accts_ConvAsPurch.Contains(acct.ExternalId))
                    fbUtility.Conversion_ActionType = FacebookUtility.Conversion_ActionType_Purchase;
                else if (Accts_ConvAsReg.Contains(acct.ExternalId))
                    fbUtility.Conversion_ActionType = FacebookUtility.Conversion_ActionType_Registration;
                else
                    fbUtility.Conversion_ActionType = FacebookUtility.Conversion_ActionType_Default;

                if (statsType.Daily)
                    DoETL_Daily(dateRange, acct, fbUtility);

                if (Accts_DailyOnly.Contains(acct.ExternalId))
                    continue;

                if (statsType.Strategy)
                    DoETL_Strategy(dateRange, acct, fbUtility);
                if (statsType.AdSet)
                    DoETL_AdSet(dateRange, acct, fbUtility);
                if (statsType.Creative)
                    DoETL_Creative(dateRange, acct, fbUtility);
                //if (statsType.Site)
                //    DoETL_Site(dateRange, acct, fbUtility);
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
        public void DoETL_AdSet(DateRange dateRange, ExtAccount account, FacebookUtility fbUtility = null)
        {
            var extracter = new FacebookAdSetSummaryExtracter(dateRange, account.ExternalId, fbUtility);
            var loader = new FacebookAdSetSummaryLoader(account.Id);
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
            using (var db = new ClientPortalProgContext())
            {
                var accounts = db.ExtAccounts.Include("Network").Where(a => a.Platform.Code == Platform.Code_FB);
                if (CampaignId.HasValue)
                    accounts = accounts.Where(a => a.CampaignId == CampaignId.Value);
                if (AccountId.HasValue)
                    accounts = accounts.Where(a => a.Id == AccountId.Value);

                return accounts.ToList().Where(a => !string.IsNullOrWhiteSpace(a.ExternalId));
            }
        }
    }
}
