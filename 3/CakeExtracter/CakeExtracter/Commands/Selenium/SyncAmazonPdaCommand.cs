using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Exceptions;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Models;
using CakeExtracter.Etl;
using CakeExtracter.Etl.AmazonSelenium.PDA;
using CakeExtracter.Etl.AmazonSelenium.PDA.Configuration;
using CakeExtracter.Etl.AmazonSelenium.PDA.Extractors;
using CakeExtracter.Etl.TradingDesk.LoadersDA.AmazonLoaders;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Concrete;
using DirectAgents.Domain.Entities.CPProg;
using SeleniumDataBrowser.Helpers;
using SeleniumDataBrowser.PDA;
using Platform = DirectAgents.Domain.Entities.CPProg.Platform;

namespace CakeExtracter.Commands.Selenium
{
    /// <inheritdoc />
    /// <summary>
    /// The class represents a command that is used to retrieve statistics
    /// of Product Display Ads type from the Amazon Advertising Portal.
    /// </summary>
    [Export(typeof(ConsoleCommand))]
    public class SyncAmazonPdaCommand : ConsoleCommand
    {
        private const int DefaultDaysAgo = 41;

        private PdaDataProviderBuilder pdaDataProviderBuilder;

        /// <summary>
        /// Gets or sets the command argument: Account ID in the database
        /// for which the command will be executed (default = all).
        /// </summary>
        public int? AccountId { get; set; }

        /// <summary>
        /// Gets or sets the command argument: Start date
        /// from which statistics will be extracted (default is 'daysAgo').
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the command argument: End date
        /// to which statistics will be extracted (default is yesterday).
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets the command argument: The number of days ago to calculate the start date
        /// from which statistics will be retrieved, used if StartDate not specified (default = 31).
        /// </summary>
        public int? DaysAgoToStart { get; set; }

        /// <summary>
        /// Gets or sets the command argument: Type of statistics will be extracted (default = all).
        /// </summary>
        public string StatsType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to hide the browser window (default = false).
        /// </summary>
        public bool IsHidingBrowserWindow { get; set; }

        /// <inheritdoc cref="ConsoleCommand"/>/>
        /// <summary>
        /// Initializes a new instance of the <see cref="SyncAmazonPdaCommand" /> class.
        /// </summary>
        public SyncAmazonPdaCommand()
        {
            IsCommand("SyncAmazonPdaCommand", "Sync Amazon Product Display Ads Stats");
            HasOption<int>("a|accountId=", "Account Id (default = all)", c => AccountId = c);
            HasOption("s|startDate=", "Start Date (default is 'daysAgo')", c => StartDate = DateTime.Parse(c));
            HasOption("e|endDate=", "End Date (default is yesterday)", c => EndDate = DateTime.Parse(c));
            HasOption<int>("d|daysAgo=", $"Days Ago to start, if startDate not specified (default = {DefaultDaysAgo})", c => DaysAgoToStart = c);
            HasOption<string>("t|statsType=", "Stats Type (default: all)", c => StatsType = c);
            HasOption<bool>("h|hideWindow=", "Include hiding the browser window", c => IsHidingBrowserWindow = c);
        }

        /// <inheritdoc />
        /// <summary>
        /// The method resets command arguments to defaults.
        /// </summary>
        public override void ResetProperties()
        {
            AccountId = null;
            StartDate = null;
            EndDate = null;
            DaysAgoToStart = null;
            StatsType = null;
            IsHidingBrowserWindow = false;
        }

        /// <inheritdoc />
        /// <summary>
        /// The method runs the current command and extract and save statistics
        /// of Product Display Ads type from the Amazon Advertising Portal
        /// based on the command arguments.
        /// </summary>
        /// <param name="remainingArguments"></param>
        /// <returns>Execution code.</returns>
        public override int Execute(string[] remainingArguments)
        {
            IntervalBetweenUnsuccessfulAndNewRequestInMinutes =
                PdaCommandConfigurationManager.GetIntervalBetweenUnsuccessfulAndNewRequest();
            var pdaDataProvider = BuildPdaDataProvider();
            RunEtls(pdaDataProvider);
            return 0;
        }

        /// <inheritdoc />
        public override IEnumerable<CommandWithSchedule> GetUniqueBroadCommands(
            IEnumerable<CommandWithSchedule> commands)
        {
            var broadCommands = new List<CommandWithSchedule>();
            var commandsGroupedByAccountAndLevel = commands.GroupBy(x =>
            {
                var command = x.Command as SyncAmazonPdaCommand;
                return new { command?.AccountId, command?.StatsType };
            });
            foreach (var commandsGroup in commandsGroupedByAccountAndLevel)
            {
                var accountLevelBroadCommands = GetUniqueBroadAccountLevelCommands(commandsGroup);
                broadCommands.AddRange(accountLevelBroadCommands);
            }
            return broadCommands;
        }

        private PdaDataProvider BuildPdaDataProvider()
        {
            try
            {
                var loggerWithoutAccountId = GetLoggerWithoutAccountId();
                pdaDataProviderBuilder = new PdaDataProviderBuilder(IsHidingBrowserWindow);
                var pdaDataProvider = pdaDataProviderBuilder.BuildDataProvider(loggerWithoutAccountId);
                return pdaDataProvider;
            }
            catch (Exception e)
            {
                throw new Exception("Failed to build PDA data provider.", e);
            }
        }

        private void RunEtls(PdaDataProvider pdaDataProvider)
        {
            var dateRange = CommandHelper.GetDateRange(StartDate, EndDate, DaysAgoToStart, DefaultDaysAgo);
            Logger.Info($"Amazon ETL (PDA Campaigns). DateRange: {dateRange}.");
            var statsType = new StatsTypeAgg(StatsType);
            var accounts = GetAccounts();
            SetInfoAboutAllAccountsInHistory(accounts);
            foreach (var account in accounts)
            {
                DoEtls(account, dateRange, statsType, pdaDataProvider);
            }
            Logger.Info("Amazon ETL (PDA Campaigns) has been finished.");
        }

        private void DoEtls(ExtAccount account, DateRange dateRange, StatsTypeAgg statsType, PdaDataProvider pdaDataProvider)
        {
            Logger.Info(account.Id, $"Commencing ETL for Amazon account ({account.Id}) {account.Name}");
            ConfigureDataProviderForCurrentAccount(account);
            try
            {
                if (statsType.Daily)
                {
                    DoEtlDailyFromRequests(account, dateRange, pdaDataProvider);
                }
                if (statsType.Strategy)
                {
                    DoEtlStrategyFromRequests(account, dateRange, pdaDataProvider);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(account.Id, ex);
            }
            Logger.Info(account.Id, $"Finished ETL for Amazon account ({account.Id}) {account.Name}");
        }

        private void ConfigureDataProviderForCurrentAccount(ExtAccount account)
        {
            var loggerWithAccountId = GetLoggerWithAccountId(account.Id);
            pdaDataProviderBuilder.InitializeAmazonPdaUtility(account.Name, loggerWithAccountId);
        }

        private void DoEtlDailyFromRequests(ExtAccount account, DateRange dateRange, PdaDataProvider pdaDataProvider)
        {
            var extractor = new AmazonPdaDailyRequestExtractor(account, dateRange, pdaDataProvider);
            var loader = new AmazonPdaDailySummaryLoader(account.Id);
            InitEtlEvents<DailySummary, AmazonPdaDailyRequestExtractor, AmazonPdaDailySummaryLoader>(extractor, loader);
            CommandExecutionContext.Current?.SetJobExecutionStateInHistory($"{extractor.SummariesDisplayName} - Started", account.Id);
            CommandHelper.DoEtl(extractor, loader);
            CommandExecutionContext.Current?.SetJobExecutionStateInHistory($"{extractor.SummariesDisplayName} - Finished", account.Id);
        }

        private void DoEtlStrategyFromRequests(ExtAccount account, DateRange dateRange, PdaDataProvider pdaDataProvider)
        {
            var extractor = new AmazonPdaCampaignRequestExtractor(account, dateRange, pdaDataProvider);
            var loader = new AmazonCampaignSummaryLoader(account.Id);
            InitEtlEvents<StrategySummary, AmazonPdaCampaignRequestExtractor, AmazonCampaignSummaryLoader>(extractor, loader);
            CommandExecutionContext.Current?.SetJobExecutionStateInHistory($"{extractor.SummariesDisplayName} - Started", account.Id);
            CommandHelper.DoEtl(extractor, loader);
            CommandExecutionContext.Current?.SetJobExecutionStateInHistory($"{extractor.SummariesDisplayName} - Finished", account.Id);
        }

        private void InitEtlEvents<TSummary, TExtractor, TLoader>(TExtractor extractor, TLoader loader)
            where TSummary : DatedStatsSummary, new()
            where TExtractor : AmazonPdaExtractor<TSummary>
            where TLoader : Loader<TSummary>
        {
            extractor.ProcessEtlFailedWithoutInformation += exception =>
                ScheduleNewCommandLaunch<SyncAmazonPdaCommand>(command =>
                    UpdateCommandParameters(command, exception));
            loader.ProcessEtlFailedWithoutInformation += exception =>
                ScheduleNewCommandLaunch<SyncAmazonPdaCommand>(command =>
                    UpdateCommandParameters(command, exception));
        }

        private void UpdateCommandParameters(SyncAmazonPdaCommand command, FailedEtlException exception)
        {
            command.AccountId = exception.AccountId;
        }

        private IEnumerable<CommandWithSchedule> GetUniqueBroadAccountLevelCommands(IEnumerable<CommandWithSchedule> commandsWithSchedule)
        {
            var accountLevelCommands = new List<Tuple<SyncAmazonPdaCommand, DateRange, CommandWithSchedule>>();
            foreach (var commandWithSchedule in commandsWithSchedule)
            {
                var command = (SyncAmazonPdaCommand)commandWithSchedule.Command;
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
                accountLevelCommands.Add(
                    new Tuple<SyncAmazonPdaCommand, DateRange, CommandWithSchedule>(command, commandDateRange, commandWithSchedule));
            }
            var broadCommands = accountLevelCommands.Select(GetCommandWithCorrectDateRange).ToList();
            return broadCommands;
        }

        private CommandWithSchedule GetCommandWithCorrectDateRange(Tuple<SyncAmazonPdaCommand, DateRange, CommandWithSchedule> setting)
        {
            setting.Item1.StartDate = setting.Item2.FromDate;
            setting.Item1.EndDate = setting.Item2.ToDate;
            setting.Item1.DaysAgoToStart = null;
            setting.Item3.Command = setting.Item1;
            return setting.Item3;
        }

        private List<ExtAccount> GetAccounts()
        {
            var repository = new PlatformAccountRepository();
            if (!AccountId.HasValue)
            {
                var accounts = repository.GetAccountsWithFilledExternalIdByPlatformCode(Platform.Code_Amazon, false);
                return accounts.ToList();
            }
            var account = repository.GetAccount(AccountId.Value);
            return new List<ExtAccount> { account };
        }

        private SeleniumLogger GetLoggerWithoutAccountId()
        {
            return new SeleniumLogger(
                x => Logger.Info(x),
                Logger.Error,
                x => Logger.Warn(x));
        }

        private SeleniumLogger GetLoggerWithAccountId(int accountId)
        {
            return new SeleniumLogger(
                x => Logger.Info(accountId, x),
                exc => Logger.Error(accountId, exc),
                x => Logger.Warn(accountId, x));
        }

        private void SetInfoAboutAllAccountsInHistory(IEnumerable<ExtAccount> accounts)
        {
            const string firstAccountState = "Not started";
            foreach (var account in accounts)
            {
                CommandExecutionContext.Current.SetJobExecutionStateInHistory(firstAccountState, account.Id);
            }
        }
    }
}