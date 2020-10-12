using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Amazon;

using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Models;
using CakeExtracter.Etl;
using CakeExtracter.Etl.Amazon.Extractors.AmazonApiExtractors;
using CakeExtracter.Etl.Amazon.Loaders;
using CakeExtracter.Etl.AmazonAttribution.Exceptions;
using CakeExtracter.Etl.AmazonAttribution.Extractors;
using CakeExtracter.Etl.AmazonAttribution.Loaders;
using CakeExtracter.Helpers;
using CakeExtracter.Logging.TimeWatchers.Amazon;

using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Commands.DA
{
    /// <inheritdoc />
    [Export(typeof(ConsoleCommand))]
    public class DASynchAmazonAttributionStats : ConsoleCommand
    {
        private const int DefaultDaysAgo = 60;

        public virtual int? AccountId { get; set; }

        public virtual DateTime? StartDate { get; set; }

        public virtual DateTime? EndDate { get; set; }

        public virtual int? DaysAgoToStart { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DASynchAmazonAttributionStats"/> class.
        /// </summary>
        public DASynchAmazonAttributionStats()
        {
            IsCommand("daSynchAmazonAttributionStats", "Synch Amazon Attribution Stats");
            HasOption<int>("a|accountId=", "Account Id (default = all)", c => AccountId = c);
            HasOption("s|startDate=", "Start Date (default is 'daysAgo')", c => StartDate = DateTime.Parse(c));
            HasOption("e|endDate=", "End Date (default is yesterday)", c => EndDate = DateTime.Parse(c));
            HasOption<int>("d|daysAgo=", $"Days Ago to start, if startDate not specified (default = {DefaultDaysAgo})", c => DaysAgoToStart = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            var dateRange = CommandHelper.GetDateRange(StartDate, EndDate, DaysAgoToStart, DefaultDaysAgo);
            var accounts = GetAccounts();

            foreach (var account in accounts)
            {
                Logger.Info("Start ETL for Amazon Attribution account ({0}) {1}", account.Id, account.Name);
                CommandExecutionContext.Current.SetJobExecutionStateInHistory("Started", account.Id);
                DoEtl(account, dateRange);
                CommandExecutionContext.Current.SetJobExecutionStateInHistory("Finished", account.Id);
            }
            return 0;
        }

        /// <inheritdoc/>
        public override IEnumerable<CommandWithSchedule> GetUniqueBroadCommands(
            IEnumerable<CommandWithSchedule> commands)
        {
            var broadCommands = new List<CommandWithSchedule>();
            var commandsGroupedByAccountAndLevel = commands.GroupBy(x =>
            {
                var command = x.Command as DASynchAmazonAttributionStats;
                return new { command?.StartDate, command?.EndDate, command?.AccountId };
            });
            foreach (var commandsGroup in commandsGroupedByAccountAndLevel)
            {
                var accountLevelBroadCommands = GetUniqueBroadAccountCommands(commandsGroup);
                broadCommands.AddRange(accountLevelBroadCommands);
            }
            return broadCommands;
        }

        private void DoEtl(ExtAccount account, DateRange dateRange)
        {
            try
            {
                var amazonUtility = CreateUtility(account);
                AmazonTimeTracker.Instance.ExecuteWithTimeTracking(
                    () =>
                        {
                            var extractor = new AmazonAttributionExtractor(amazonUtility, dateRange, account);
                            var loader = new AmazonAttributionLoader(account.Id);
                            InitEtlEvents(extractor, loader);
                            CommandHelper.DoEtl(extractor, loader);
                        },
                    account.Id,
                    AmazonJobLevels.Attribution,
                    AmazonJobOperations.Total);
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        private IEnumerable<ExtAccount> GetAccounts()
        {
            var accountRepository = DIKernel.Get<IPlatformAccountRepository>();
            if (!AccountId.HasValue)
            {
                var accounts = accountRepository.GetAccountsWithFilledExternalIdByPlatformCode(Platform.Code_AttributionAmazon, false);
                return accounts;
            }

            var account = accountRepository.GetAccount(AccountId.Value);
            return new[] { account };
        }

        private AmazonUtility CreateUtility(ExtAccount account)
        {
            AmazonUtility.TokenSets = Platform.GetPlatformTokens(Platform.Code_AttributionAmazon);
            var amazonUtility = new AmazonUtility(m => Logger.Info(account.Id, m), m => Logger.Warn(account.Id, m));
            amazonUtility.SetWhichAlt(account.ExternalId);
            amazonUtility.SetApiEndpointUrl(account.Name);
            return amazonUtility;
        }

        private IEnumerable<CommandWithSchedule> GetUniqueBroadAccountCommands(IEnumerable<CommandWithSchedule> commandsWithSchedule)
        {
            var accountCommands =
                new List<Tuple<DASynchAmazonAttributionStats, CommandWithSchedule>>();
            foreach (var commandWithSchedule in commandsWithSchedule)
            {
                var command = (DASynchAmazonAttributionStats)commandWithSchedule.Command;
                accountCommands.Add(
                    new Tuple<DASynchAmazonAttributionStats, CommandWithSchedule>(command, commandWithSchedule));
            }
            var broadCommands = accountCommands.Select(x => new CommandWithSchedule
            {
                Command = x.Item1,
                ScheduledTime = x.Item2.ScheduledTime,
            }).ToList();
            return broadCommands;
        }

        private void InitEtlEvents(AmazonAttributionExtractor extractor, AmazonAttributionLoader loader)
        {
            GeneralInitEtlEvents(extractor, loader);
            extractor.ProcessFailedExtraction += exception =>
                ScheduleNewCommandLaunch<DASynchAmazonAttributionStats>(command =>
                    UpdateCommandParameters(command, exception));
            loader.ProcessFailedLoading += exception =>
                ScheduleNewCommandLaunch<DASynchAmazonAttributionStats>(command =>
                    UpdateCommandParameters(command, exception));
        }

        private void GeneralInitEtlEvents<T>(Extracter<T> extractor, Loader<T> loader)
        {
            extractor.ProcessEtlFailedWithoutInformation += exception =>
                ScheduleNewCommandLaunch<DASynchAmazonAttributionStats>(command => { });
            loader.ProcessEtlFailedWithoutInformation += exception =>
                ScheduleNewCommandLaunch<DASynchAmazonAttributionStats>(command => { });
        }

        private void UpdateCommandParameters(DASynchAmazonAttributionStats command, AttributionFailedEtlException exception)
        {
            command.StartDate = exception.StartDate;
            command.EndDate = exception.EndDate;
            command.AccountId = exception.AccountId;
        }
    }
}
