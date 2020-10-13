using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Models;
using CakeExtracter.Etl;
using CakeExtracter.Etl.DSP.Configuration;
using CakeExtracter.Etl.DSP.Exceptions;
using CakeExtracter.Etl.DSP.Extractors;
using CakeExtracter.Etl.DSP.Loaders;
using CakeExtracter.Etl.DSP.Models;
using DirectAgents.Domain.Concrete;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Commands.DA
{
    /// <summary>DSP stat syncer job command.</summary>
    [Export(typeof(ConsoleCommand))]
    public class DASyncAmazonDspStats : ConsoleCommand
    {
        private readonly AmazonDspExtractor extractor;
        private readonly AmazonDspConfigurationProvider configurationProvider;
        private readonly AmazonDspDataLoader loader;
        private readonly CommandsExecutionAccountsProvider accountsProvider;

        public int? AccountId { get; set; }

        /// <summary>Initializes a new instance of the <see cref="DASyncAmazonDspStats"/> class.
        /// Initialises cmd params values.</summary>
        public DASyncAmazonDspStats()
        {
            IsCommand("daSyncAmazonDspStats", "Synch Amazon DSP Stats");
            HasOption<int>("a|accountId=", "Account Id (default = all)", c => AccountId = c);
            configurationProvider = new AmazonDspConfigurationProvider();
            extractor = new AmazonDspExtractor(configurationProvider, AccountId);
            loader = new AmazonDspDataLoader();
            accountsProvider = new CommandsExecutionAccountsProvider(new PlatformAccountRepository());
        }

        /// <summary>Resets command properties.</summary>
        public override void ResetProperties()
        {
            AccountId = null;
        }

        /// <summary>Executes the command logic.</summary>
        /// <param name="remainingArguments">The remaining arguments.</param>
        /// <returns>Execution status.</returns>
        public override int Execute(string[] remainingArguments)
        {
            InitEtlEvents();
            ProcessDailyEtl();
            return 0;
        }

        /// <inheritdoc/>
        public override IEnumerable<CommandWithSchedule> GetUniqueBroadCommands(
            IEnumerable<CommandWithSchedule> commands)
        {
            var broadCommands = new List<CommandWithSchedule>();
            var groupedCommands = commands.GroupBy(x =>
            {
                var command = x.Command as DASyncAmazonDspStats;
                return new { command?.AccountId };
            });
            foreach (var commandsGroup in groupedCommands)
            {
                var accountLevelBroadCommands = GetUniqueBroadAccountCommands(commandsGroup);
                broadCommands.AddRange(accountLevelBroadCommands);
            }
            return broadCommands;
        }

        private IEnumerable<CommandWithSchedule> GetUniqueBroadAccountCommands(IEnumerable<CommandWithSchedule> commandsWithSchedule)
        {
            var accountCommands = new List<Tuple<DASyncAmazonDspStats, CommandWithSchedule>>();

            foreach (var commandWithSchedule in commandsWithSchedule)
            {
                var command = (DASyncAmazonDspStats)commandWithSchedule.Command;
                accountCommands.Add(
                    new Tuple<DASyncAmazonDspStats, CommandWithSchedule>(command, commandWithSchedule));
            }
            var broadCommands = accountCommands.Select(x => new CommandWithSchedule
            {
                Command = x.Item1,
                ScheduledTime = x.Item2.ScheduledTime,
            }).ToList();
            return broadCommands;
        }

        private void ProcessDailyEtl()
        {
            try
            {
                var accounts = accountsProvider.GetAccountsToProcess(Platform.Code_DspAmazon, AccountId);
                var reportData = ProcessDailyDataExtraction(accounts);
                if (reportData != null)
                {
                    loader.LoadData(reportData);
                }
                else
                {
                    Logger.Warn("DSP - Data loading canceled because report data extraction failed");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private List<AmazonDspAccountReportData> ProcessDailyDataExtraction(List<ExtAccount> accounts)
        {
            try
            {
                var dailyReportData = extractor.ExtractDailyData(accounts);
                return dailyReportData;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        private void InitEtlEvents()
        {
            extractor.ProcessFailedExtraction += exception =>
                ScheduleNewCommandLaunch<DASyncAmazonDspStats>(command =>
                    UpdateCommandParameters(command, exception));
            loader.ProcessFailedLoading += exception =>
                 ScheduleNewCommandLaunch<DASyncAmazonDspStats>(command =>
                    UpdateCommandParameters(command, exception));
        }

        private void UpdateCommandParameters(DASyncAmazonDspStats command, DspFailedEtlException exception)
        {
            command.AccountId = exception.AccountId;
        }
    }
}
