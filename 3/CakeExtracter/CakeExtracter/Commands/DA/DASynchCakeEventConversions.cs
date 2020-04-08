using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using CakeExtracter.Bootstrappers;
using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Models;
using CakeExtracter.Etl.CakeMarketing.Cleaners;
using CakeExtracter.Etl.CakeMarketing.DALoaders;
using CakeExtracter.Etl.CakeMarketing.Exceptions;
using CakeExtracter.Etl.CakeMarketing.Extracters;
using CakeExtracter.Helpers;

namespace CakeExtracter.Commands.DA
{
    [Export(typeof(ConsoleCommand))]
    public class DASynchCakeEventConversions : ConsoleCommand
    {
        public static int RunStatic(int? advertiserId = null, int? offerId = null, DateTime? startDate = null, DateTime? endDate = null, int? daysAgoToStart = null, bool clearFirst = false)
        {
            AutoMapperBootstrapper.CheckRunSetup();
            var cmd = new DASynchCakeEventConversions
            {
                AdvertiserId = advertiserId,
                OfferId = offerId,
                StartDate = startDate,
                EndDate = endDate,
                DaysAgoToStart = daysAgoToStart,
                ClearFirst = clearFirst,
            };
            return cmd.Run();
        }

        public int? AdvertiserId { get; set; }

        public int? OfferId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? DaysAgoToStart { get; set; }

        public bool ClearFirst { get; set; }

        public override void ResetProperties()
        {
            AdvertiserId = null;
            OfferId = null;
            StartDate = null;
            EndDate = null;
            DaysAgoToStart = null;
            ClearFirst = false;
        }

        // e.g. daSynchCakeEventConversions -a=806 -s=9/17/18 -e=10/1/18

        public DASynchCakeEventConversions()
        {
            IsCommand("daSynchCakeEventConversions", "synch EventConversions");
            HasOption<int>("a|advertiserId=", "Advertiser Id (default: 0 / all advertisers)", c => AdvertiserId = c);
            HasOption<int>("o|offerId=", "Offer Id (default: 0 / all offers)", c => OfferId = c);
            HasOption("s|startDate=", "Start Date (default: 'daysAgo')", c => StartDate = DateTime.Parse(c));
            HasOption("e|endDate=", "End Date (default: today)", c => EndDate = DateTime.Parse(c));
            HasOption<int>("d|daysAgo=", "Days Ago to start, if startDate not specified (default: 0(today); use -1 for first-of-month)", c => DaysAgoToStart = c);
            HasOption<bool>("w|wipeFirst=", "Clear existing convs in the db first (default: false)", c => ClearFirst = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            var dateRange = GetDateRange();
            Logger.Info("Cake EventConversions ETL. DateRange {0}.", dateRange);
            if (ClearFirst)
            {
                CleanEventConversions(dateRange);
            }
            var extractor = new EventConversionExtracter(dateRange, AdvertiserId ?? 0, OfferId ?? 0);
            var loader = new DAEventConversionLoader();
            InitEtlEvents(extractor, loader);
            CommandHelper.DoEtl(extractor, loader);
            return 0;
        }

        /// <inheritdoc />
        public override IEnumerable<CommandWithSchedule> GetUniqueBroadCommands(
            IEnumerable<CommandWithSchedule> commands)
        {
            var broadCommands = new List<CommandWithSchedule>();
            var commandsGroupedByAdvertiserAndOffer = commands.GroupBy(x =>
            {
                var command = x.Command as DASynchCakeEventConversions;
                return new { command?.AdvertiserId, command?.OfferId };
            });
            foreach (var commandsGroup in commandsGroupedByAdvertiserAndOffer)
            {
                var accountBroadCommands = GetUniqueBroadAccountCommands(commandsGroup);
                broadCommands.AddRange(accountBroadCommands);
            }

            return broadCommands;
        }

        private DateRange GetDateRange()
        {
            return DASynchCampSums.GetDateRange(StartDate, EndDate, DaysAgoToStart, defaultDaysAgo: 0, useYesterday: false);
        }

        private void CleanEventConversions(DateRange dateRange)
        {
            var cleaner = new DaEventConversionCleaner(AdvertiserId, OfferId, dateRange);
            cleaner.ClearEventConversions();
        }

        private List<CommandWithSchedule> GetUniqueBroadAccountCommands(IEnumerable<CommandWithSchedule> commandsWithSchedule)
        {
            var accountCommands = new List<Tuple<DASynchCakeEventConversions, DateRange, CommandWithSchedule>>();
            foreach (var commandWithSchedule in commandsWithSchedule)
            {
                var command = (DASynchCakeEventConversions)commandWithSchedule.Command;
                var commandDateRange = CommandHelper.GetDateRange(command.StartDate, command.EndDate, command.DaysAgoToStart, 0);
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

                accountCommands.Add(new Tuple<DASynchCakeEventConversions, DateRange, CommandWithSchedule>(command, commandDateRange, commandWithSchedule));
            }

            var broadCommands = accountCommands.Select(GetCommandWithCorrectDateRange).ToList();
            return broadCommands;
        }

        private CommandWithSchedule GetCommandWithCorrectDateRange(Tuple<DASynchCakeEventConversions, DateRange, CommandWithSchedule> setting)
        {
            setting.Item1.StartDate = setting.Item2.FromDate;
            setting.Item1.EndDate = setting.Item2.ToDate;
            setting.Item1.DaysAgoToStart = null;
            setting.Item3.Command = setting.Item1;
            return setting.Item3;
        }

        private void InitEtlEvents(EventConversionExtracter extractor, DAEventConversionLoader loader)
        {
            GeneralInitEtlEvents(extractor, loader);
            extractor.ProcessFailedExtraction += exception =>
                ScheduleNewCommandLaunch<DASynchCakeEventConversions>(command =>
                    UpdateCommandParameters(command, exception));
            loader.ProcessFailedLoading += exception =>
                ScheduleNewCommandLaunch<DASynchCakeEventConversions>(command =>
                    UpdateCommandParameters(command, exception));
        }

        private void GeneralInitEtlEvents(EventConversionExtracter extractor, DAEventConversionLoader loader)
        {
            extractor.ProcessEtlFailedWithoutInformation += exception =>
                ScheduleNewCommandLaunch<DASynchCakeEventConversions>(command => { });
            loader.ProcessEtlFailedWithoutInformation += exception =>
                ScheduleNewCommandLaunch<DASynchCakeEventConversions>(command => { });
        }

        private void UpdateCommandParameters(DASynchCakeEventConversions command, CakeEventConversionsFailedEtlException exception)
        {
            command.StartDate = exception.StartDate;
            command.EndDate = exception.EndDate;
            command.AdvertiserId = exception.AdvertiserId;
            command.OfferId = exception.OfferId;
        }
    }
}