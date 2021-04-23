using System;
using System.ComponentModel.Composition;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Models;
using CakeExtracter.Etl.AmazonSelenium.Configuration;
using CakeExtracter.Etl.AmazonSelenium.VCD.Configuration;
using CakeExtracter.Helpers;
using CakeExtracter.Logging.TimeWatchers.Amazon;
using DirectAgents.Domain.Entities.CPProg;
using SeleniumDataBrowser.Helpers;
using SeleniumDataBrowser.Models;
using SeleniumDataBrowser.VCD;
using SeleniumDataBrowser.VCD.Models;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Extractors;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Loaders;
using SeleniumDataBrowser.VCD.Enums;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models.Products;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Extractors.VcdCustomReportsExtractionHelpers.ReportParsing.RowMaps;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models.Base;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models.Interface;

namespace CakeExtracter.Commands.Selenium
{
    /// <inheritdoc />
    /// <summary>
    /// The class represents a command that is used to retrieve statistics of Amazon Vendor Central statistics.
    /// </summary>
    [Export(typeof(ConsoleCommand))]
    public class SyncAmazonVcdCustomReportsCommand : ConsoleCommand
    {
        private const int DefaultDaysAgo = 60;

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

        public PeriodType RequestedPeriod { get; set; }

        public string ReportType { get; set; }

        /// <inheritdoc cref="ConsoleCommand" />
        /// <summary>
        /// Initializes a new instance of the <see cref="SyncAmazonVcdCommand" /> class.
        /// </summary>
        public SyncAmazonVcdCustomReportsCommand()
        {
            IsCommand("SyncAmazonVcdCustomReportsCommand", "Sync Vendor Central Stats");
            HasOption<int>("p|profileNumber=", "Execution profile number", c => ProfileNumber = c);
            HasOption("s|startDate=", "Start Date (default is 'daysAgo')", c => StartDate = DateTime.Parse(c));
            HasOption("e|endDate=", "End Date (default is yesterday)", c => EndDate = DateTime.Parse(c));
            HasOption<int>("d|daysAgo=", $"Days Ago to start, if startDate not specified (default = {DefaultDaysAgo})", c => DaysAgoToStart = c);
            HasOption<bool>("h|hideWindow=", "Include hiding the browser window", c => IsHidingBrowserWindow = c);
            HasOption<string>("t|periodType=", "", c => RequestedPeriod = Enum.TryParse<PeriodType>(c, true, out var result) ? result : PeriodType.DAILY);
            HasOption<string>("r|reportType=", "", c => ReportType = c);
            NoNeedToCreateRepeatRequests = true;
        }

        /// <inheritdoc />
        /// <summary>
        /// The method resets command arguments to defaults.
        /// </summary>
        public override void ResetProperties()
        {
            ProfileNumber = null;
            StartDate = null;
            EndDate = null;
            DaysAgoToStart = null;
            IsHidingBrowserWindow = false;
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
            IntervalBetweenUnsuccessfulAndNewRequestInMinutes =
                VcdCommandConfigurationManager.GetIntervalBetweenUnsuccessfulAndNewRequest();
            using (var vcdDataProvider = BuildVcdDataProvider())
            {
                RunEtls(vcdDataProvider);
            }

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

        private VcdDataProvider BuildVcdDataProvider()
        {
            try
            {
                VcdExecutionProfileManager.Current.SetExecutionProfileNumber(ProfileNumber);
                var authModel = GetAuthorizationModel();
                var loggerWithoutAccountId = GetLoggerWithoutAccountId();
                var waitPageTimeoutInMinutes = SeleniumCommandConfigurationManager.GetWaitPageTimeout();
                var vcdDataProvider = new VcdDataProvider(
                    waitPageTimeoutInMinutes,
                    IsHidingBrowserWindow,
                    loggerWithoutAccountId,
                    authModel);
                return vcdDataProvider;
            }
            catch (Exception e)
            {
                throw new Exception("Failed to build VCD data provider.", e);
            }
        }

        private AuthorizationModel GetAuthorizationModel()
        {
            try
            {
                return new AuthorizationModel
                {
                    Login = VcdExecutionProfileManager.Current.ProfileConfiguration.LoginEmail,
                    Password = VcdExecutionProfileManager.Current.ProfileConfiguration.LoginPassword,
                    SignInUrl = VcdExecutionProfileManager.Current.ProfileConfiguration.SignInUrl,
                    CookiesDir = VcdExecutionProfileManager.Current.ProfileConfiguration.CookiesDirectory,
                };
            }
            catch (Exception e)
            {
                throw new Exception("Failed to initialize authorization settings.", e);
            }
        }

        private void RunEtls(VcdDataProvider vcdDataProvider)
        {
            var vcdWorkflowHelper = new VcdWorkflowHelper(vcdDataProvider);
            var accounts = vcdWorkflowHelper.VcdAccountsInfo;
            SetInfoAboutAllAccountsInHistory(accounts.Keys.ToList());
            RunForAccounts(accounts, vcdDataProvider);
        }

        private void RunForAccounts(Dictionary<ExtAccount, VcdAccountInfo> accounts, VcdDataProvider vcdDataProvider)
        {
            var reportType = new AmazonCustomReportType(ReportType);
            foreach (var account in accounts)
            {
                try
                {
                    var dateRange = CommandHelper.GetDateRange(StartDate, EndDate, DaysAgoToStart, DefaultDaysAgo);
                    if (reportType.GeoSales)
                    {
                        DoGeographicSalesInsightsEtlForAccount(account, dateRange, vcdDataProvider);
                    }

                    if (reportType.NetPpm)
                    {
                        DoNetPpmEtlForAccount(account, dateRange, vcdDataProvider);
                    }

                    if (reportType.RepeatPurchase)
                    {
                        DoRepeatPurchaseBehaviorEtlForAccount(account, dateRange, vcdDataProvider);
                    }

                    if (reportType.MarketBasket)
                    {
                        DoMarketBasketAnalysisEtlForAccount(account, dateRange, vcdDataProvider);
                    }

                    if (reportType.ItemComparison)
                    {
                        DoItemComparisonEtlForAccount(account, dateRange, vcdDataProvider);
                    }

                    if (reportType.AlternativePurchase)
                    {
                        DoAlternativePurchaseEtlForAccount(account, dateRange, vcdDataProvider);
                    }

                    AmazonTimeTracker.Instance.LogTrackingData(account.Key.Id);
                }
                catch (Exception ex)
                {
                    Logger.Error(account.Key.Id, ex);
                }
            }
        }

        private void DoGeographicSalesInsightsEtlForAccount(KeyValuePair<ExtAccount, VcdAccountInfo> account, DateRange dateRange, VcdDataProvider vcdDataProvider)
        {
            Logger.Info(account.Key.Id, $"Amazon VCD, GeographicSalesInsights ETL for account {account.Key.Name} ({account.Key.Id}) started.");
            ConfigureDataProviderForCurrentAccount(account.Key, vcdDataProvider);
            var extractor = GetCustomReportExtractor<GeoSalesProduct, GeographicSalesInsightsProductsRowMap>(
                account,
                dateRange,
                RequestedPeriod,
                SeleniumDataBrowser.VCD.Enums.ReportType.geographicSalesInsights,
                vcdDataProvider);
            var loader = new GeographicSalesInsightsLoader(account.Key);

            CommandHelper.DoEtl(extractor, loader);
            Logger.Info(account.Key.Id, $"Amazon VCD, ETL for account {account.Key.Name} ({account.Key.Id}) finished.");
        }

        private void DoNetPpmEtlForAccount(KeyValuePair<ExtAccount, VcdAccountInfo> account, DateRange dateRange, VcdDataProvider vcdDataProvider)
        {
            Logger.Info(account.Key.Id, $"Amazon VCD, Net Ppm ETL for account {account.Key.Name} ({account.Key.Id}) started.");
            ConfigureDataProviderForCurrentAccount(account.Key, vcdDataProvider);
            var extractor = GetCustomReportExtractor<NetPpmProduct, NetPpmProductsRowMap>(
                account,
                dateRange,
                RequestedPeriod,
                SeleniumDataBrowser.VCD.Enums.ReportType.netPPM,
                vcdDataProvider);
            var loader = new NetPpmLoader(account.Key, RequestedPeriod);

            CommandHelper.DoEtl(extractor, loader);
            Logger.Info(account.Key.Id, $"Amazon VCD, Net PPM ETL for account {account.Key.Name} ({account.Key.Id}) finished.");
        }

        private void DoRepeatPurchaseBehaviorEtlForAccount(KeyValuePair<ExtAccount, VcdAccountInfo> account, DateRange dateRange, VcdDataProvider vcdDataProvider)
        {
            Logger.Info(account.Key.Id, $"Amazon VCD, Repeat Purchase Behavior ETL for account {account.Key.Name} ({account.Key.Id}) started.");
            var extractor = GetCustomReportExtractor<RepeatPurchaseBehaviorProduct, RepeatPurchaseBehaviorProductsRowMap>(
                account,
                dateRange,
                RequestedPeriod,
                SeleniumDataBrowser.VCD.Enums.ReportType.repeatPurchaseBehavior,
                vcdDataProvider);
            var loader = new RepeatPurchaseBehaviorLoader(account.Key, RequestedPeriod);

            CommandHelper.DoEtl(extractor, loader);
            Logger.Info(account.Key.Id, $"Amazon VCD, RepeatPurchaseBehavior ETL for account {account.Key.Name} ({account.Key.Id}) finished.");
        }

        private void DoMarketBasketAnalysisEtlForAccount(KeyValuePair<ExtAccount, VcdAccountInfo> account, DateRange dateRange, VcdDataProvider vcdDataProvider)
        {
            Logger.Info(account.Key.Id, $"Amazon VCD, MarketBasketAnalysis ETL for account {account.Key.Name} ({account.Key.Id}) started.");
            ConfigureDataProviderForCurrentAccount(account.Key, vcdDataProvider);
            var extractor = GetCustomReportExtractor<MarketBasketAnalysisProduct, MarketBasketAnalysisRowMap>(
                account,
                dateRange,
                RequestedPeriod,
                SeleniumDataBrowser.VCD.Enums.ReportType.marketBasketAnalysis,
                vcdDataProvider);
            var loader = new MarketBasketAnalysisLoader(account.Key);

            CommandHelper.DoEtl(extractor, loader);
            Logger.Info(account.Key.Id, $"Amazon VCD, ETL for account {account.Key.Name} ({account.Key.Id}) finished.");
        }

        private void DoAlternativePurchaseEtlForAccount(KeyValuePair<ExtAccount, VcdAccountInfo> account, DateRange dateRange, VcdDataProvider vcdDataProvider)
        {
            Logger.Info(account.Key.Id, $"Amazon VCD, AlternativePurchase ETL for account {account.Key.Name} ({account.Key.Id}) started.");
            ConfigureDataProviderForCurrentAccount(account.Key, vcdDataProvider);
            var extractor = GetCustomReportExtractor<AlternativePurchaseProduct, AlternativePurchaseRowMap>(
                account,
                dateRange,
                RequestedPeriod,
                SeleniumDataBrowser.VCD.Enums.ReportType.alternativePurchase,
                vcdDataProvider);
            var loader = new AlternativePurchaseLoader(account.Key);

            CommandHelper.DoEtl(extractor, loader);
            Logger.Info(account.Key.Id, $"Amazon VCD, ETL for account {account.Key.Name} ({account.Key.Id}) finished.");
        }

        private void DoItemComparisonEtlForAccount(KeyValuePair<ExtAccount, VcdAccountInfo> account, DateRange dateRange, VcdDataProvider vcdDataProvider)
        {
            Logger.Info(account.Key.Id, $"Amazon VCD, ItemComparison ETL for account {account.Key.Name} ({account.Key.Id}) started.");
            ConfigureDataProviderForCurrentAccount(account.Key, vcdDataProvider);
            var extractor = GetCustomReportExtractor<ItemComparisonProduct, ItemComparisonRowMap>(
                account,
                dateRange,
                RequestedPeriod,
                SeleniumDataBrowser.VCD.Enums.ReportType.itemComparison,
                vcdDataProvider);
            var loader = new ItemComparisonLoader(account.Key);

            CommandHelper.DoEtl(extractor, loader);
            Logger.Info(account.Key.Id, $"Amazon VCD, ETL for account {account.Key.Name} ({account.Key.Id}) finished.");
        }

        private VcdCustomReportExtractor<TProduct, TRowMap> GetCustomReportExtractor<TProduct, TRowMap>(
            KeyValuePair<ExtAccount, VcdAccountInfo> account, DateRange dateRange, PeriodType period, ReportType reportType, VcdDataProvider vcdDataProvider)
                where TProduct : VcdCustomProduct, ISumMetrics<TProduct>
                where TRowMap : BaseCustomReportRowMap<TProduct>
        {
            var reportParameters = new VcdCustomReportExtractorParameters<TProduct>
            {
                Account = account.Key,
                DataProvider = vcdDataProvider,
                AccountInfo = account.Value,
                Period = period,
                DateRange = dateRange,
                ReportType = reportType,
            };
            var extractor = new VcdCustomReportExtractor<TProduct, TRowMap>(reportParameters);
            return extractor;
        }

        private void SetInfoAboutAllAccountsInHistory(IEnumerable<ExtAccount> accounts)
        {
            const string firstAccountState = "Not started";
            foreach (var account in accounts)
            {
                CommandExecutionContext.Current.SetJobExecutionStateInHistory(firstAccountState, account.Id);
            }
        }

        private void ConfigureDataProviderForCurrentAccount(ExtAccount account, VcdDataProvider vcdDataProvider)
        {
            var loggerWithAccountId = GetLoggerWithAccountId(account.Id);
            vcdDataProvider.LoggerWithAccountId = loggerWithAccountId;
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
