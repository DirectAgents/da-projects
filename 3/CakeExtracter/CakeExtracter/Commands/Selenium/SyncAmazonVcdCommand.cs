using System;
using System.ComponentModel.Composition;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Models;
using CakeExtracter.Etl.AmazonSelenium.VCD;
using CakeExtracter.Etl.AmazonSelenium.VCD.Configuration;
using CakeExtracter.Etl.AmazonSelenium.VCD.DataProviderBuilders;
using CakeExtracter.Etl.AmazonSelenium.VCD.Extractors;
using CakeExtracter.Etl.AmazonSelenium.VCD.Loaders;
using CakeExtracter.Etl.AmazonSelenium.VCD.Synchers;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Concrete;
using DirectAgents.Domain.Entities.CPProg;
using SeleniumDataBrowser.Helpers;
using SeleniumDataBrowser.VCD.DataProviders;

namespace CakeExtracter.Commands.Selenium
{
    /// <inheritdoc />
    /// <summary>
    /// The class represents a command that is used to retrieve statistics of Amazon Vendor Central statistics.
    /// </summary>
    [Export(typeof(ConsoleCommand))]
    public class SyncAmazonVcdCommand : ConsoleCommand
    {
        private const int DefaultDaysAgo = 60;

        private IVcdDataProviderBuilder vcdDataProviderBuilder;

        /// <summary>
        /// Gets or sets the command argument: Account ID in the database
        /// for which the command will be executed (default = all).
        /// </summary>
        public int? AccountId { get; set; }

        /// <summary>
        /// Gets or sets the command argument: a number of execution profile (default = 1).
        /// </summary>
        public int? ProfileNumber { get; set; }

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
        /// Gets or sets a value indicating whether to hide the browser window (default = false).
        /// </summary>
        public bool IsHidingBrowserWindow { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable manual mode of extracting (default = false).
        /// </summary>
        public bool IsExtractManual { get; set; }

        /// <inheritdoc cref="ConsoleCommand" />
        /// <summary>
        /// Initializes a new instance of the <see cref="SyncAmazonVcdCommand" /> class.
        /// </summary>
        public SyncAmazonVcdCommand()
        {
            IsCommand("SyncAmazonVcdCommand", "Sync Vendor Central Stats");
            HasOption<int>("a|accountId=", "Account Id (default = all)", c => AccountId = c);
            HasOption<int>("p|profileNumber=", "Execution profile number", c => ProfileNumber = c);
            HasOption("s|startDate=", "Start Date (default is 'daysAgo')", c => StartDate = DateTime.Parse(c));
            HasOption("e|endDate=", "End Date (default is yesterday)", c => EndDate = DateTime.Parse(c));
            HasOption<int>("d|daysAgo=", $"Days Ago to start, if startDate not specified (default = {DefaultDaysAgo})", c => DaysAgoToStart = c);
            HasOption<bool>("h|hideWindow=", "Include hiding the browser window (default is false)", c => IsHidingBrowserWindow = c);
            HasOption<bool>("m|manualExtract=", "Include manual mode of extracting (default is false)", c => IsExtractManual = c);
        }

        /// <inheritdoc />
        /// <summary>
        /// The method resets command arguments to defaults.
        /// </summary>
        public override void ResetProperties()
        {
            AccountId = null;
            ProfileNumber = null;
            StartDate = null;
            EndDate = null;
            DaysAgoToStart = null;
            IsHidingBrowserWindow = false;
            IsExtractManual = false;
        }

        /// <inheritdoc />
        /// <summary>
        /// The method runs the current command and extract and save statistics of Amazon Vendor Central statistics.
        /// based on the command arguments.
        /// </summary>
        /// <param name="remainingArguments"></param>
        /// <returns>Execution code.</returns>
        public override int Execute(string[] remainingArguments)
        {
            NoNeedToCreateRepeatRequests = IsExtractManual;
            IntervalBetweenUnsuccessfulAndNewRequestInMinutes =
                VcdCommandConfigurationManager.GetIntervalBetweenUnsuccessfulAndNewRequest();
            RunEtls();
            return 0;
        }

        /// <inheritdoc />
        public override IEnumerable<CommandWithSchedule> GetUniqueBroadCommands(IEnumerable<CommandWithSchedule> commands)
        {
            var broadCommands = new List<CommandWithSchedule>();
            var commandsGroupedByProfile = commands.GroupBy(x => (x.Command as SyncAmazonVcdCommand)?.ProfileNumber);
            foreach (var commandsGroup in commandsGroupedByProfile)
            {
                var profileLevelBroadCommands = GetUniqueBroadProfileLevelCommands(commandsGroup);
                broadCommands.AddRange(profileLevelBroadCommands);
            }
            return broadCommands;
        }

        private IVcdDataProvider BuildVcdDataProvider()
        {
            try
            {
                var loggerWithoutAccountId = GetLoggerWithoutAccountId();
                if (IsExtractManual)
                {
                    vcdDataProviderBuilder = new VcdManualDataProviderBuilder(AccountId);
                }
                else
                {
                    vcdDataProviderBuilder = new VcdDataProviderBuilder(ProfileNumber, IsHidingBrowserWindow);
                }
                var vcdDataProvider = vcdDataProviderBuilder.BuildDataProvider(loggerWithoutAccountId);
                return vcdDataProvider;
            }
            catch (Exception e)
            {
                throw new Exception("Failed to build VCD data provider.", e);
            }
        }

        private void RunEtls()
        {
            var vcdDataProvider = BuildVcdDataProvider();
            var accounts = vcdDataProviderBuilder.GetAccounts();
            SetInfoAboutAllAccountsInHistory(accounts);
            RunForAccounts(accounts, vcdDataProvider);
        }

        private void SetInfoAboutAllAccountsInHistory(IEnumerable<ExtAccount> accounts)
        {
            const string firstAccountState = "Not started";
            foreach (var account in accounts)
            {
                CommandExecutionContext.Current.SetJobExecutionStateInHistory(firstAccountState, account.Id);
            }
        }

        private void RunForAccounts(IEnumerable<ExtAccount> accounts, IVcdDataProvider vcdDataProvider)
        {
            var dateRange = CommandHelper.GetDateRange(StartDate, EndDate, DaysAgoToStart, DefaultDaysAgo);
            Logger.Info($"Amazon VCD ETL. DateRange {dateRange}.");
            foreach (var account in accounts)
            {
                try
                {
                    DoEtlForAccount(account, dateRange, vcdDataProvider);
                    SyncAccountDataToAnalyticTable(account.Id, dateRange);
                }
                catch (Exception ex)
                {
                    Logger.Error(account.Id, ex);
                }
            }
        }

        private void DoEtlForAccount(ExtAccount account, DateRange dateRange, IVcdDataProvider vcdDataProvider)
        {
            Logger.Info(account.Id, $"Amazon VCD, ETL for account {account.Name} ({account.Id}) started.");
            ConfigureDataProviderForCurrentAccount(account);
            var extractor = GetDailyDataExtractor(account, dateRange, vcdDataProvider);
            PrepareDailyDataLoader();
            var loader = new AmazonVcdLoader(account);
            InitEtlEvents(extractor, loader);
            CommandHelper.DoEtl(extractor, loader);
            Logger.Info(account.Id, $"Amazon VCD, ETL for account {account.Name} ({account.Id}) finished.");
        }

        private void ConfigureDataProviderForCurrentAccount(ExtAccount account)
        {
            var loggerWithAccountId = GetLoggerWithAccountId(account.Id);
            vcdDataProviderBuilder.InitializeReportDownloader(account, loggerWithAccountId);
        }

        private AmazonVcdExtractor GetDailyDataExtractor(ExtAccount account, DateRange dateRange, IVcdDataProvider vcdDataProvider)
        {
            var maxRetryAttemptsForExtractData = VcdCommandConfigurationManager.GetExtractDailyDataAttemptCount();
            var extractor = new AmazonVcdExtractor(account, dateRange, vcdDataProvider, maxRetryAttemptsForExtractData);
            return extractor;
        }

        private void PrepareDailyDataLoader()
        {
            try
            {
                AmazonVcdLoader.PrepareLoader();
            }
            catch (Exception e)
            {
                throw new Exception("Failed to prepare VCD loader.", e);
            }
        }

        private void InitEtlEvents(AmazonVcdExtractor extractor, AmazonVcdLoader loader)
        {
            extractor.ProcessEtlFailedWithoutInformation += exception =>
                ScheduleNewCommandLaunch<SyncAmazonVcdCommand>(command => { });
            loader.ProcessEtlFailedWithoutInformation += exception =>
                ScheduleNewCommandLaunch<SyncAmazonVcdCommand>(command => { });
        }

        private void SyncAccountDataToAnalyticTable(int accountId, DateRange dateRange)
        {
            try
            {
                CommandExecutionContext.Current.SetJobExecutionStateInHistory("Sync analytic table data.", accountId);
                Logger.Info(accountId, "Sync analytic table data.");
                var syncScriptPath = VcdCommandConfigurationManager.GetSyncScriptPath();
                var vcdTablesSyncher = new VcdAnalyticTablesSyncher(syncScriptPath);
                vcdTablesSyncher.SyncData(accountId, dateRange);
            }
            catch (Exception ex)
            {
                var exception = new Exception("Error occurred while sync VCD data to analytic table", ex);
                Logger.Error(accountId, exception);
                throw exception;
            }
        }

        private SeleniumLogger GetLoggerWithoutAccountId()
        {
            return new SeleniumLogger(
                x => Logger.Info(x), Logger.Error, x => Logger.Warn(x));
        }

        private SeleniumLogger GetLoggerWithAccountId(int accountId)
        {
            return new SeleniumLogger(
                x => Logger.Info(accountId, x),
                exc => Logger.Error(accountId, exc),
                x => Logger.Warn(accountId, x));
        }

        private IEnumerable<CommandWithSchedule> GetUniqueBroadProfileLevelCommands(IEnumerable<CommandWithSchedule> commandsWithSchedule)
        {
            var profileLevelCommands = new List<Tuple<SyncAmazonVcdCommand, DateRange, CommandWithSchedule>>();
            foreach (var commandWithSchedule in commandsWithSchedule)
            {
                var command = (SyncAmazonVcdCommand)commandWithSchedule.Command;
                var commandDateRange = CommandHelper.GetDateRange(command.StartDate, command.EndDate, command.DaysAgoToStart, DefaultDaysAgo);
                var crossCommands = profileLevelCommands.Where(x => commandDateRange.IsCrossDateRange(x.Item2)).ToList();
                foreach (var crossCommand in crossCommands)
                {
                    commandDateRange = commandDateRange.MergeDateRange(crossCommand.Item2);
                    commandWithSchedule.ScheduledTime =
                        crossCommand.Item3.ScheduledTime > commandWithSchedule.ScheduledTime
                            ? crossCommand.Item3.ScheduledTime
                            : commandWithSchedule.ScheduledTime;
                    profileLevelCommands.Remove(crossCommand);
                }
                profileLevelCommands.Add(new Tuple<SyncAmazonVcdCommand, DateRange, CommandWithSchedule>(command, commandDateRange, commandWithSchedule));
            }
            var broadCommands = profileLevelCommands.Select(GetCommandWithCorrectDateRange).ToList();
            return broadCommands;
        }

        private CommandWithSchedule GetCommandWithCorrectDateRange(Tuple<SyncAmazonVcdCommand, DateRange, CommandWithSchedule> setting)
        {
            setting.Item1.StartDate = setting.Item2.FromDate;
            setting.Item1.EndDate = setting.Item2.ToDate;
            setting.Item1.DaysAgoToStart = null;
            setting.Item3.Command = setting.Item1;
            return setting.Item3;
        }
    }
}