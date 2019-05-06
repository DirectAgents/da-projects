using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using CakeExtracter.Bootstrappers;
using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement;
using CakeExtracter.Etl.YAM.Extractors.ApiExtractors;
using CakeExtracter.Etl.YAM.Helpers;
using CakeExtracter.Etl.YAM.Loaders;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Concrete;
using DirectAgents.Domain.Entities.CPProg;
using Yahoo;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class DASynchYAMStats : ConsoleCommand
    {
        private const int DefaultDaysAgo = 41;

        private List<string> extIdsUsePixelParams;
        private List<Action> etlList;

        public int? AccountId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? DaysAgoToStart { get; set; }
        public string StatsType { get; set; }
        public bool DisabledOnly { get; set; }

        public static int RunStatic(int? accountId = null, DateTime? startDate = null, DateTime? endDate = null, string statsType = null)
        {
            AutoMapperBootstrapper.CheckRunSetup();
            var cmd = new DASynchYAMStats
            {
                AccountId = accountId,
                StartDate = startDate,
                EndDate = endDate,
                StatsType = statsType
            };
            return cmd.Run();
        }

        public DASynchYAMStats()
        {
            IsCommand("daSynchYAMStats", "synch YAM Stats");
            HasOption<int>("a|accountId=", "Account Id (default = all)", c => AccountId = c);
            HasOption("s|startDate=", "Start Date (default is 'daysAgo')", c => StartDate = DateTime.Parse(c));
            HasOption("e|endDate=", "End Date (default is yesterday)", c => EndDate = DateTime.Parse(c));
            HasOption<int>("d|daysAgo=", $"Days Ago to start, if startDate not specified (default = {DefaultDaysAgo})", c => DaysAgoToStart = c);
            HasOption<string>("t|statsType=", "Stats Type (default: all)", c => StatsType = c);
            HasOption<bool>("x|disabledOnly=", "Include only disabled accounts (default = false)", c => DisabledOnly = c);
        }

        public override void ResetProperties()
        {
            AccountId = null;
            StartDate = null;
            EndDate = null;
            DaysAgoToStart = null;
            StatsType = null;
            DisabledOnly = false;
        }

        //TODO: if synching all accounts, can we make one API call to get everything?

        public override int Execute(string[] remainingArguments)
        {
            var dateRange = CommandHelper.GetDateRange(StartDate, EndDate, DaysAgoToStart, DefaultDaysAgo);
            Logger.Info("YAM ETL. DateRange {0}.", dateRange);

            var statsType = new YamStatsType(StatsType);
            extIdsUsePixelParams = ConfigurationHelper.ExtractEnumerableFromConfig("YAMids_UsePixelParm");
            var accounts = GetAccounts();
            DoEtlsParallel(dateRange, statsType, accounts);
            return 0;
        }

        private IEnumerable<ExtAccount> GetAccounts()
        {
            var repository = new PlatformAccountRepository();
            if (!AccountId.HasValue)
            {
                var accounts = repository.GetAccountsWithFilledExternalIdByPlatformCode(Platform.Code_YAM, DisabledOnly);
                return accounts;
            }
            var account = repository.GetAccount(AccountId.Value);
            return new[] { account };
        }

        private void DoEtlsParallel(DateRange dateRange, YamStatsType statsType, IEnumerable<ExtAccount> accounts)
        {
            YamUtility.TokenSets = GetTokens();
            etlList = new List<Action>();
            foreach (var account in accounts)
            {
                DoEtls(dateRange, statsType, account);
            }

            Parallel.Invoke(etlList.ToArray());
            SaveTokens();
        }

        private string[] GetTokens()
        {
            // Get tokens, if any, from the database
            return Platform.GetPlatformTokens(Platform.Code_YAM);
        }

        private void SaveTokens()
        {
            Platform.SavePlatformTokens(Platform.Code_YAM, YamUtility.TokenSets);
        }

        private void DoEtls(DateRange dateRange, YamStatsType statsType, ExtAccount account)
        {
            Logger.Info(account.Id, "Commencing ETL for YAM account ({0}) {1}", account.Id, account.Name);
            var yamUtility = CreateUtility(account);
            var usePixelParameters = extIdsUsePixelParams.Contains(account.ExternalId);
            AddEnabledEtl(statsType.Daily, account, () => DoETL_Daily(dateRange, account, yamUtility, usePixelParameters));
            AddEnabledEtl(statsType.Campaign, account, () => DoETL_Campaign(dateRange, account, yamUtility, usePixelParameters));
            AddEnabledEtl(statsType.Line, account, () => DoETL_Line(dateRange, account, yamUtility, usePixelParameters));
            AddEnabledEtl(statsType.Creative, account, () => DoETL_Creative(dateRange, account, yamUtility, usePixelParameters));
            AddEnabledEtl(statsType.Ad, account, () => DoETL_Ad(dateRange, account, yamUtility, usePixelParameters));
            AddEnabledEtl(statsType.Pixel, account, () => DoETL_Pixel(dateRange, account, yamUtility, usePixelParameters));
        }

        private YamUtility CreateUtility(ExtAccount account)
        {
            var yamUtility = new YamUtility(m => Logger.Info(account.Id, m), m => Logger.Warn(m),
                exc => Logger.Error(account.Id, exc));
            yamUtility.SetWhichAlt(account.ExternalId);
            return yamUtility;
        }

        private void AddEnabledEtl(bool etlEnabled, ExtAccount account, Action etlAction)
        {
            if (!etlEnabled)
            {
                return;
            }

            etlList.Add(() =>
            {
                try
                {
                    etlAction();
                }
                catch (Exception ex)
                {
                    Logger.Error(account.Id, ex);
                }
            });
        }

        private void DoETL_Daily(DateRange dateRange, ExtAccount account, YamUtility yamUtility, bool usePixelParameters)
        {
            var extractor = new YamDailySummaryExtractor(yamUtility, dateRange, account, usePixelParameters);
            var loader = new YamDailySummaryLoader(account.Id);
            CommandHelper.DoEtl(extractor, loader);
            CommandExecutionContext.Current?.AppendJobExecutionStateInHistory("YamDailySummaries - Finished", account.Id);
        }

        private void DoETL_Campaign(DateRange dateRange, ExtAccount account, YamUtility yamUtility, bool usePixelParameters)
        {
            var extractor = new YamCampaignSummaryExtractor(yamUtility, dateRange, account, usePixelParameters);
            var loader = new YamCampaignSummaryLoader(account.Id);
            CommandHelper.DoEtl(extractor, loader);
            CommandExecutionContext.Current?.AppendJobExecutionStateInHistory("YamCampaignSummaries - Finished", account.Id);
        }

        private void DoETL_Line(DateRange dateRange, ExtAccount account, YamUtility yamUtility, bool usePixelParameters)
        {
            var extractor = new YamLineSummaryExtractor(yamUtility, dateRange, account, usePixelParameters);
            var loader = new YamLineSummaryLoader(account.Id);
            CommandHelper.DoEtl(extractor, loader);
            CommandExecutionContext.Current?.AppendJobExecutionStateInHistory("YamLineSummaries - Finished", account.Id);
        }

        private void DoETL_Creative(DateRange dateRange, ExtAccount account, YamUtility yamUtility, bool usePixelParameters)
        {
            var extractor = new YamCreativeSummaryExtractor(yamUtility, dateRange, account, usePixelParameters);
            var loader = new YamCreativeSummaryLoader(account.Id);
            CommandHelper.DoEtl(extractor, loader);
            CommandExecutionContext.Current?.AppendJobExecutionStateInHistory("YamCreativeSummaries - Finished", account.Id);
        }

        private void DoETL_Ad(DateRange dateRange, ExtAccount account, YamUtility yamUtility, bool usePixelParameters)
        {
            var extractor = new YamAdSummaryExtractor(yamUtility, dateRange, account, usePixelParameters);
            var loader = new YamAdSummaryLoader(account.Id);
            CommandHelper.DoEtl(extractor, loader);
            CommandExecutionContext.Current?.AppendJobExecutionStateInHistory("YamAdSummaries - Finished", account.Id);
        }

        private void DoETL_Pixel(DateRange dateRange, ExtAccount account, YamUtility yamUtility, bool usePixelParameters)
        {
            var extractor = new YamPixelSummaryExtractor(yamUtility, dateRange, account, usePixelParameters);
            var loader = new YamPixelSummaryLoader(account.Id);
            CommandHelper.DoEtl(extractor, loader);
            CommandExecutionContext.Current?.AppendJobExecutionStateInHistory("YamPixelSummaries - Finished", account.Id);
        }
    }
}
