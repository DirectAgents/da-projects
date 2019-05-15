using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using CakeExtracter.Bootstrappers;
using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Exceptions;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Models;
using CakeExtracter.Etl;
using CakeExtracter.Etl.YAM.CommonClassesExtensions;
using CakeExtracter.Etl.YAM.Extractors.ApiExtractors;
using CakeExtracter.Etl.YAM.Extractors.CsvExtractors;
using CakeExtracter.Etl.YAM.Loaders;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Concrete;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.YAM.Summaries;
using Yahoo;
using Yahoo.Exceptions;

namespace CakeExtracter.Commands.DA
{
    [Export(typeof(ConsoleCommand))]
    public class DaSynchYamStats : ConsoleCommand
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
            var cmd = new DaSynchYamStats
            {
                AccountId = accountId,
                StartDate = startDate,
                EndDate = endDate,
                StatsType = statsType
            };
            return cmd.Run();
        }

        public DaSynchYamStats()
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
            SetConfigurationVariables();
            var dateRange = CommandHelper.GetDateRange(StartDate, EndDate, DaysAgoToStart, DefaultDaysAgo);
            var statsType = new YamStatsType(StatsType);
            var accounts = GetAccounts();
            DoEtlsParallel(dateRange, statsType, accounts);
            return 0;
        }

        public override IEnumerable<CommandWithSchedule> GetUniqueBroadCommands(
            IEnumerable<CommandWithSchedule> commands)
        {
            var broadCommands = new List<CommandWithSchedule>();
            var commandsGroupedByAccountAndLevel = commands.GroupBy(x =>
            {
                var command = x.Command as DaSynchYamStats;
                return new {command?.AccountId, command?.StatsType};
            });
            foreach (var commandsGroup in commandsGroupedByAccountAndLevel)
            {
                var accountLevelBroadCommands = GetUniqueBroadAccountLevelCommands(commandsGroup);
                broadCommands.AddRange(accountLevelBroadCommands);
            }

            return broadCommands;
        }

        private void SetConfigurationVariables()
        {
            extIdsUsePixelParams = ConfigurationHelper.ExtractEnumerableFromConfig("YAMids_UsePixelParm");
            IntervalBetweenUnsuccessfulAndNewRequestInMinutes = int.Parse(ConfigurationManager.AppSettings["YamIntervalBetweenRequestsInMinutes"]);
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
            Logger.Info("YAM ETL. DateRange {0}.", dateRange);
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
            var yamUtility = new YamUtility(m => Logger.Info(account.Id, m), m => Logger.Warn(account.Id, m),
                exc => Logger.Error(account.Id, exc));
            yamUtility.SetWhichAlt(account.ExternalId);
            InitUtilityEvents(yamUtility);
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
            var csvExtractor = new YamCsvExtractor(account);
            var extractor = new YamDailySummaryExtractor(csvExtractor, yamUtility, dateRange, account, usePixelParameters);
            var loader = new YamDailySummaryLoader(account.Id);
            DoEtl<YamDailySummary, YamDailySummaryExtractor, YamDailySummaryLoader>(extractor, loader, account);
        }

        private void DoETL_Campaign(DateRange dateRange, ExtAccount account, YamUtility yamUtility, bool usePixelParameters)
        {
            var csvExtractor = new YamCsvExtractor(account);
            var extractor = new YamCampaignSummaryExtractor(csvExtractor, yamUtility, dateRange, account, usePixelParameters);
            var loader = new YamCampaignSummaryLoader(account.Id);
            DoEtl<YamCampaignSummary, YamCampaignSummaryExtractor, YamCampaignSummaryLoader>(extractor, loader, account);
        }

        private void DoETL_Line(DateRange dateRange, ExtAccount account, YamUtility yamUtility, bool usePixelParameters)
        {
            var csvExtractor = new YamCsvExtractor(account);
            var extractor = new YamLineSummaryExtractor(csvExtractor, yamUtility, dateRange, account, usePixelParameters);
            var loader = new YamLineSummaryLoader(account.Id);
            DoEtl<YamLineSummary, YamLineSummaryExtractor, YamLineSummaryLoader>(extractor, loader, account);
        }

        private void DoETL_Creative(DateRange dateRange, ExtAccount account, YamUtility yamUtility, bool usePixelParameters)
        {
            var csvExtractor = new YamCsvExtractor(account);
            var extractor = new YamCreativeSummaryExtractor(csvExtractor, yamUtility, dateRange, account, usePixelParameters);
            var loader = new YamCreativeSummaryLoader(account.Id);
            DoEtl<YamCreativeSummary, YamCreativeSummaryExtractor, YamCreativeSummaryLoader>(extractor, loader, account);
        }

        private void DoETL_Ad(DateRange dateRange, ExtAccount account, YamUtility yamUtility, bool usePixelParameters)
        {
            var csvExtractor = new YamCsvExtractor(account);
            var extractor = new YamAdSummaryExtractor(csvExtractor, yamUtility, dateRange, account, usePixelParameters);
            var loader = new YamAdSummaryLoader(account.Id);
            DoEtl<YamAdSummary, YamAdSummaryExtractor, YamAdSummaryLoader>(extractor, loader, account);
        }

        private void DoETL_Pixel(DateRange dateRange, ExtAccount account, YamUtility yamUtility, bool usePixelParameters)
        {
            var csvExtractor = new YamCsvExtractor(account);
            var extractor = new YamPixelSummaryExtractor(csvExtractor, yamUtility, dateRange, account, usePixelParameters);
            var loader = new YamPixelSummaryLoader(account.Id);
            DoEtl<YamPixelSummary, YamPixelSummaryExtractor, YamPixelSummaryLoader>(extractor, loader, account);
        }

        private void DoEtl<TSummary, TExtractor, TLoader>(TExtractor extractor, TLoader loader, ExtAccount account)
            where TSummary : BaseYamSummary, new()
            where TExtractor : BaseYamApiExtractor<TSummary>
            where TLoader : Loader<TSummary>
        {
            InitEtlEvents<TSummary, TExtractor, TLoader>(extractor, loader);
            CommandHelper.DoEtl(extractor, loader);
            CommandExecutionContext.Current?.AppendJobExecutionStateInHistory($"{extractor.SummariesDisplayName} - Finished", account.Id);
        }

        private void InitUtilityEvents(YamUtility utility)
        {
            utility.ProcessFailedReportGeneration += exception =>
                ScheduleNewCommandLaunch<DaSynchYamStats>(command =>
                    UpdateCommandParameters(command, exception));
        }

        private void InitEtlEvents<TSummary, TExtractor, TLoader>(TExtractor extractor, TLoader loader)
            where TSummary : BaseYamSummary, new()
            where TExtractor : BaseYamApiExtractor<TSummary>
            where TLoader : Loader<TSummary>
        {
            extractor.ProcessFailedExtraction += exception =>
                ScheduleNewCommandLaunch<DaSynchYamStats>(command =>
                    UpdateCommandParameters(command, exception));
            extractor.ProcessEtlFailedWithoutInformation += exception =>
                ScheduleNewCommandLaunch<DaSynchYamStats>(command =>
                    UpdateCommandParameters(command, exception));
            loader.ProcessEtlFailedWithoutInformation += exception =>
                ScheduleNewCommandLaunch<DaSynchYamStats>(command =>
                    UpdateCommandParameters(command, exception));
        }

        private void UpdateCommandParameters(DaSynchYamStats command, FailedReportGenerationException exception)
        {
            command.StartDate = exception.StartDate;
            command.EndDate = exception.EndDate;
            var repository = new PlatformAccountRepository();
            var dbAccount = repository.GetAccountByExternalId(exception.AccountId.ToString(), Platform.Code_YAM);
            command.AccountId = dbAccount.Id;
            var statsType = new YamStatsType
            {
                Ad = exception.ByAd,
                Campaign = exception.ByCampaign,
                Creative = exception.ByCreative,
                Daily = true,
                Line = exception.ByLine,
                Pixel = exception.ByPixel
            };
            command.StatsType = statsType.GetStatsTypeString();
        }

        private void UpdateCommandParameters(DaSynchYamStats command, FailedEtlException exception)
        {
            command.AccountId = exception.AccountId;
        }

        private List<CommandWithSchedule> GetUniqueBroadAccountLevelCommands(IEnumerable<CommandWithSchedule> commandsWithSchedule)
        {
            var accountLevelCommands = new List<Tuple<DaSynchYamStats, DateRange, CommandWithSchedule>>();
            foreach (var commandWithSchedule in commandsWithSchedule)
            {
                var command = (DaSynchYamStats)commandWithSchedule.Command;
                var commandDateRange = CommandHelper.GetDateRange(command.StartDate, command.EndDate, command.DaysAgoToStart, DefaultDaysAgo);
                var crossCommands = accountLevelCommands.Where(x => commandDateRange.IsCrossDateRange(x.Item2)).ToList();
                foreach (var crossCommand in crossCommands)
                {
                    commandDateRange = commandDateRange.MergeDateRange(crossCommand.Item2);
                    commandWithSchedule.ScheduledTime =
                        crossCommand.Item3.ScheduledTime > commandWithSchedule.ScheduledTime
                            ? crossCommand.Item3.ScheduledTime
                            : commandWithSchedule.ScheduledTime;
                    accountLevelCommands.Remove(crossCommand);
                }

                accountLevelCommands.Add(new Tuple<DaSynchYamStats, DateRange, CommandWithSchedule>(command, commandDateRange, commandWithSchedule));
            }

            var broadCommands = accountLevelCommands.Select(GetCommandWithCorrectDateRange).ToList();
            return broadCommands;
        }

        private CommandWithSchedule GetCommandWithCorrectDateRange(Tuple<DaSynchYamStats, DateRange, CommandWithSchedule> setting)
        {
            setting.Item1.StartDate = setting.Item2.FromDate;
            setting.Item1.EndDate = setting.Item2.ToDate;
            setting.Item1.DaysAgoToStart = null;
            setting.Item3.Command = setting.Item1;
            return setting.Item3;
        }
    }
}
