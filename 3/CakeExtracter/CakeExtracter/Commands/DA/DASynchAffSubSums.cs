using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using CakeExtracter.Bootstrappers;
using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Models;
using CakeExtracter.Etl.CakeMarketing.DALoaders;
using CakeExtracter.Etl.CakeMarketing.Exceptions;
using CakeExtracter.Etl.CakeMarketing.Extracters;
using CakeExtracter.Helpers;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class DASynchAffSubSums : ConsoleCommand
    {
        public static int RunStatic(int? affiliateId = null, int? advertiserId = null, int? offerId = null, DateTime? startDate = null, DateTime? endDate = null, int? daysAgoToStart = null)
            //, bool clearFirst = false)
        {
            AutoMapperBootstrapper.CheckRunSetup();
            var cmd = new DASynchAffSubSums
            {
                AffiliateId = affiliateId,
                AdvertiserId = advertiserId,
                OfferId = offerId,
                StartDate = startDate,
                EndDate = endDate,
                DaysAgoToStart = daysAgoToStart
            };
            return cmd.Run();
        }

        public int? AffiliateId { get; set; }

        public int? AdvertiserId { get; set; }

        public int? OfferId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? DaysAgoToStart { get; set; }
        //public bool ClearFirst { get; set; }

        public override void ResetProperties()
        {
            AffiliateId = null;
            AdvertiserId = null;
            OfferId = null;
            StartDate = null;
            EndDate = null;
            DaysAgoToStart = null;
            //ClearFirst = false;
        }

        //Note: Uses CampSums to determine which Affs & Offers to loop thru (i.e. those with campsum stats)
        public DASynchAffSubSums()
        {
            IsCommand("daSynchAffSubSums", "synch stats by AffSub and Offer");
            HasOption<int>("f|affiliateId=", "Affiliate Id (default: all affiliates)", c => AffiliateId = c);
            HasOption<int>("a|advertiserId=", "Advertiser Id (default: all advertisers)", c => AdvertiserId = c);
            HasOption<int>("o|offerId=", "Offer Id (default: all offers)", c => OfferId = c);
            HasOption("s|startDate=", "Start Date (default: 'daysAgo')", c => StartDate = DateTime.Parse(c));
            HasOption("e|endDate=", "End Date (default: today)", c => EndDate = DateTime.Parse(c));
            HasOption<int>("d|daysAgo=", "Days Ago to start, if startDate not specified (default: -1 == first-of-month)", c => DaysAgoToStart = c);
            //HasOption<bool>("w|wipeFirst=", "Clear existing convs in the db first (default: false)", c => ClearFirst = c);
        }

        private DateRange GetDateRange()
        {
            return DASynchCampSums.GetDateRange(StartDate, EndDate, DaysAgoToStart, defaultDaysAgo: -1, useYesterday: false);
        }

        public override int Execute(string[] remainingArguments)
        {
            var dateRange = GetDateRange();
            Logger.Info("Cake AffSubSummaries ETL. DateRange {0}.", dateRange);

            var extracter = new SubIdSummariesExtracter(dateRange, affiliateId: AffiliateId, advertiserId: AdvertiserId, offerId: OfferId, getDailyStats: true);
            var loader = new DAAffSubSummaryLoader();
            InitEtlEvents(extracter, loader);
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
            return 0;
        }

        /// <inheritdoc />
        public override IEnumerable<CommandWithSchedule> GetUniqueBroadCommands(
            IEnumerable<CommandWithSchedule> commands)
        {
            var broadCommands = new List<CommandWithSchedule>();
            var groupedCommands = commands.GroupBy(x =>
            {
                var command = x.Command as DASynchAffSubSums;
                return new { command?.AdvertiserId, command?.OfferId, command?.AffiliateId };
            });
            foreach (var commandsGroup in groupedCommands)
            {
                var accountBroadCommands = GetUniqueBroadAccountCommands(commandsGroup);
                broadCommands.AddRange(accountBroadCommands);
            }

            return broadCommands;
        }

        private List<CommandWithSchedule> GetUniqueBroadAccountCommands(IEnumerable<CommandWithSchedule> commandsWithSchedule)
        {
            var accountCommands = new List<Tuple<DASynchAffSubSums, DateRange, CommandWithSchedule>>();
            foreach (var commandWithSchedule in commandsWithSchedule)
            {
                var command = (DASynchAffSubSums)commandWithSchedule.Command;
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

                accountCommands.Add(new Tuple<DASynchAffSubSums, DateRange, CommandWithSchedule>(command, commandDateRange, commandWithSchedule));
            }

            var broadCommands = accountCommands.Select(GetCommandWithCorrectDateRange).ToList();
            return broadCommands;
        }

        private CommandWithSchedule GetCommandWithCorrectDateRange(Tuple<DASynchAffSubSums, DateRange, CommandWithSchedule> setting)
        {
            setting.Item1.StartDate = setting.Item2.FromDate;
            setting.Item1.EndDate = setting.Item2.ToDate;
            setting.Item1.DaysAgoToStart = null;
            setting.Item3.Command = setting.Item1;
            return setting.Item3;
        }

        private void InitEtlEvents(SubIdSummariesExtracter extractor, DAAffSubSummaryLoader loader)
        {
            GeneralInitEtlEvents(extractor, loader);
            extractor.ProcessFailedExtraction += exception =>
                ScheduleNewCommandLaunch<DASynchAffSubSums>(command =>
                    UpdateCommandParameters(command, exception));
            loader.ProcessFailedLoading += exception =>
                ScheduleNewCommandLaunch<DASynchAffSubSums>(command =>
                    UpdateCommandParameters(command, exception));
        }

        private void GeneralInitEtlEvents(SubIdSummariesExtracter extractor, DAAffSubSummaryLoader loader)
        {
            extractor.ProcessEtlFailedWithoutInformation += exception =>
                ScheduleNewCommandLaunch<DASynchAffSubSums>(command => { });
            loader.ProcessEtlFailedWithoutInformation += exception =>
                ScheduleNewCommandLaunch<DASynchAffSubSums>(command => { });
        }

        private void UpdateCommandParameters(DASynchAffSubSums command, CakeAffSubSumsFailedEtlException exception)
        {
            command.StartDate = exception.StartDate;
            command.EndDate = exception.EndDate;
            command.AffiliateId = exception.AffiliateId;
            command.AdvertiserId = exception.AdvertiserId;
            command.OfferId = exception.OfferId;
        }
    }
}
