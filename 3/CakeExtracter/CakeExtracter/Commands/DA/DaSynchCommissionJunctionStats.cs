using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Exceptions;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Models;
using CakeExtracter.Etl.TradingDesk.Extracters.CommissionJunctionExtractors;
using CakeExtracter.Etl.TradingDesk.LoadersDA.CommissionJunctionLoaders;
using CakeExtracter.Helpers;
using CommissionJunction.Enums;
using CommissionJunction.Exceptions;
using CommissionJunction.Utilities;
using DirectAgents.Domain.Concrete;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Commands.DA
{
    ///The class represents a command that is used to retrieve statistics from the Commission Junction portal
    [Export(typeof(ConsoleCommand))]
    public class DaSynchCommissionJunctionStats : ConsoleCommand
    {
        private const int DefaultDaysAgo = 31;

        private IEnumerable<string> lockingDateRangeAccountsExternalIds;
        private IEnumerable<string> postingDateRangeAccountsExternalIds;

        /// <summary>
        /// Command argument: Account ID in the database for which the command will be executed (default = all)
        /// </summary>
        public int? AccountId { get; set; }
        
        /// <summary>
        /// Command argument: Start date from which statistics will be extracted (default is 'daysAgo')
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Command argument: End date to which statistics will be extracted (default is yesterday)
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Command argument: The number of days ago to calculate the start date from which statistics will be retrieved,
        /// used if StartDate not specified (default = 31)
        /// </summary>
        public int? DaysAgoToStart { get; set; }

        protected override int IntervalBetweenUnsuccessfulAndNewRequestInMinutes { get; set; }

        /// <summary>
        /// The constructor sets a command name and command arguments names, provides a description for them.
        /// </summary>
        public DaSynchCommissionJunctionStats()
        {
            IsCommand("DaSynchCommissionJunctionStats", "Synch Commission Junction Stats");
            HasOption<int>("a|accountId=", "Account Id (default = all)", c => AccountId = c);
            HasOption("s|startDate=", "Start Date (default is 'daysAgo')", c => StartDate = DateTime.Parse(c));
            HasOption("e|endDate=", "End Date (default is yesterday)", c => EndDate = DateTime.Parse(c));
            HasOption<int>("d|daysAgo=", $"Days Ago to start, if startDate not specified (default = {DefaultDaysAgo})", c => DaysAgoToStart = c);
        }

        /// <summary>
        /// The method resets command arguments to defaults
        /// </summary>
        public override void ResetProperties()
        {
            AccountId = null;
            StartDate = null;
            EndDate = null;
            DaysAgoToStart = null;
        }

        /// <summary>
        /// The method runs the current command and extract and save statistics from the Commission Junction portal based on the command arguments.
        /// </summary>
        /// <param name="remainingArguments"></param>
        /// <returns>Execution code</returns>
        public override int Execute(string[] remainingArguments)
        {
            SetConfigurationVariables();
            var dateRange = CommandHelper.GetDateRange(StartDate, EndDate, DaysAgoToStart, DefaultDaysAgo);
            Logger.Info("Commission Junction ETL. DateRange {0}.", dateRange);
            var accounts = GetAccounts();
            Parallel.ForEach(accounts, account =>
            {
                Logger.Info(account.Id, "Commencing ETL for Commission Junction account ({0}) {1}", account.Id, account.Name);
                var utility = CreateUtility(account);
                var dateRangeType = GetDateRangeType(account);
                DoEtls(utility, dateRangeType, dateRange, account);
                Logger.Info(account.Id, "Finished ETL for Commission Junction account ({0}) {1}", account.Id, account.Name);
            });
            return 0;
        }

        public override IEnumerable<CommandWithSchedule> GetUniqueBroadCommands(
            IEnumerable<CommandWithSchedule> commands)
        {
            var broadCommands = new List<CommandWithSchedule>();
            var commandsGroupedByAccount = commands.GroupBy(x => (x.Command as DaSynchCommissionJunctionStats)?.AccountId);
            foreach (var commandsGroup in commandsGroupedByAccount)
            {
                var accountBroadCommands = GetUniqueBroadAccountCommands(commandsGroup);
                broadCommands.AddRange(accountBroadCommands);
            }

            return broadCommands;
        }

        private void SetConfigurationVariables()
        {
            IntervalBetweenUnsuccessfulAndNewRequestInMinutes = int.Parse(ConfigurationManager.AppSettings["CJIntervalBetweenRequestsInMinutes"]);
            lockingDateRangeAccountsExternalIds = ConfigurationHelper.ExtractEnumerableFromConfig("CJLockingDateRangeAccountsExternalIds");
            postingDateRangeAccountsExternalIds = ConfigurationHelper.ExtractEnumerableFromConfig("CJPostingDateRangeAccountsExternalIds");
        }

        private DateRangeType GetDateRangeType(ExtAccount account)
        {
            return lockingDateRangeAccountsExternalIds.Contains(account.ExternalId)
                ? DateRangeType.Locking
                : postingDateRangeAccountsExternalIds.Contains(account.ExternalId)
                    ? DateRangeType.Posting
                    : DateRangeType.Event;
        }

        private IEnumerable<ExtAccount> GetAccounts()
        {
            var repository = new PlatformAccountRepository();
            if (!AccountId.HasValue)
            {
                var accounts = repository.GetAccountsWithFilledExternalIdByPlatformCode(Platform.Code_CJ, false);
                return accounts;
            }
            var account = repository.GetAccount(AccountId.Value);
            return new[] { account };
        }

        private CjUtility CreateUtility(ExtAccount account)
        {
            var utility = new CjUtility(message => Logger.Info(account.Id, message),
                exc => Logger.Error(account.Id, exc), message => Logger.Warn(account.Id, message));
            utility.SetWhichAlt(account.ExternalId);
            InitUtilityEvents(utility);
            return utility;
        }

        private void DoEtls(CjUtility utility, DateRangeType dateRangeType, DateRange dateRange, ExtAccount account)
        {
            var cleaner = new CommissionJunctionAdvertiserCleaner();
            var extractor = new CommissionJunctionAdvertiserExtractor(utility, cleaner, dateRangeType, dateRange, account);
            var loader = new CommissionJunctionAdvertiserLoader(account.Id);
            InitEtlEvents(extractor, loader);
            CommandHelper.DoEtl(extractor, loader);
        }

        private void InitUtilityEvents(CjUtility utility)
        {
            utility.ProcessSkippedCommissions += exception =>
                ScheduleNewJobRequest<DaSynchCommissionJunctionStats>(command =>
                    UpdateCommandParameters(command, exception));
        }

        private void InitEtlEvents(CommissionJunctionAdvertiserExtractor extractor, CommissionJunctionAdvertiserLoader loader)
        {
            extractor.ProcessFailedExtraction += exception =>
                ScheduleNewJobRequest<DaSynchCommissionJunctionStats>(command =>
                    UpdateCommandParameters(command, exception));
            extractor.ProcessEtlFailedWithoutInformation += exception =>
                ScheduleNewJobRequest<DaSynchCommissionJunctionStats>(command =>
                    UpdateCommandParameters(command, exception));
            loader.ProcessEtlFailedWithoutInformation += exception =>
                ScheduleNewJobRequest<DaSynchCommissionJunctionStats>(command =>
                    UpdateCommandParameters(command, exception));
        }

        private void UpdateCommandParameters(DaSynchCommissionJunctionStats command, SkippedCommissionsException exception)
        {
            command.StartDate = exception.StartDate;
            command.EndDate = exception.EndDate;
            var repository = new PlatformAccountRepository();
            var dbAccount = repository.GetAccountByExternalId(exception.AccountId, Platform.Code_CJ);
            command.AccountId = dbAccount.Id;
        }

        private void UpdateCommandParameters(DaSynchCommissionJunctionStats command, FailedEtlException exception)
        {
            command.StartDate = exception.StartDate;
            command.EndDate = exception.EndDate;
            command.AccountId = exception.AccountId;
        }

        private void UpdateCommandParameters(DaSynchCommissionJunctionStats command, Exception exception)
        {
        }

        private List<CommandWithSchedule> GetUniqueBroadAccountCommands(IEnumerable<CommandWithSchedule> commandsWithSchedule)
        {
            var accountCommands = new List<Tuple<DaSynchCommissionJunctionStats, DateRange, CommandWithSchedule>>();
            foreach (var commandWithSchedule in commandsWithSchedule)
            {
                var command = (DaSynchCommissionJunctionStats) commandWithSchedule.Command;
                var commandDateRange = CommandHelper.GetDateRange(command.StartDate, command.EndDate, command.DaysAgoToStart, DefaultDaysAgo);
                var crossCommands = accountCommands.Where(x => commandDateRange.IsCrossDateRange(x.Item2)).ToList();
                foreach (var crossCommand in crossCommands)
                {
                    commandDateRange = commandDateRange.MergeDateRange(crossCommand.Item2);
                    commandWithSchedule.ScheduledTime =
                        crossCommand.Item3.ScheduledTime > commandWithSchedule.ScheduledTime
                            ? crossCommand.Item3.ScheduledTime
                            : commandWithSchedule.ScheduledTime;
                    accountCommands.Remove(crossCommand);
                }

                accountCommands.Add(new Tuple<DaSynchCommissionJunctionStats, DateRange, CommandWithSchedule>(command, commandDateRange, commandWithSchedule));
            }

            var broadCommands = accountCommands.Select(GetCommandWithCorrectDateRange).ToList();
            return broadCommands;
        }

        private CommandWithSchedule GetCommandWithCorrectDateRange(Tuple<DaSynchCommissionJunctionStats, DateRange, CommandWithSchedule> setting)
        {
            setting.Item1.StartDate = setting.Item2.FromDate;
            setting.Item1.EndDate = setting.Item2.ToDate;
            setting.Item1.DaysAgoToStart = null;
            setting.Item3.Command = setting.Item1;
            return setting.Item3;
        }
    }
}
