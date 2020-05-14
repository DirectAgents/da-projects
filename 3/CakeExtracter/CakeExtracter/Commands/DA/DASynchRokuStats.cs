using System;
using System.ComponentModel.Composition;

using CakeExtracter.Common;
using CakeExtracter.Etl.Roku.Extractors;
using CakeExtracter.Etl.Roku.Loaders;
using CakeExtracter.Helpers;

namespace CakeExtracter.Commands.DA
{
    /// <inheritdoc />
    /// The class represents a command that is used to retrieve
    /// statistics from the Roku Platform API
    [Export(typeof(ConsoleCommand))]
    public class DASynchRokuStats : ConsoleCommand
    {
        /// <summary>
        /// Gets or sets command argument: Start date from which statistics will be extracted (default is 'daysAgo').
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets command argument: End date to which statistics will be extracted (default is yesterday).
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets command argument: The number of days ago to calculate the start date from which statistics will be retrieved,
        /// used if StartDate not specified (default = 41).
        /// </summary>
        public int? DaysAgoToStart { get; set; }

        private const int DefaultDaysAgo = 41;

        /// <summary>
        /// Initializes a new instance of the <see cref="DASynchRokuStats"/> class.
        /// The constructor sets a command name and command arguments names, provides a description for them.
        /// </summary>
        public DASynchRokuStats()
        {
            IsCommand("daSynchRokuStats", "synch Roku Stats");
            HasOption("s|startDate=", "Start Date (default is 'daysAgo')", c => StartDate = DateTime.Parse(c));
            HasOption("e|endDate=", "End Date (default is yesterday)", c => EndDate = DateTime.Parse(c));
            HasOption<int>("d|daysAgo=", $"Days Ago to start, if startDate not specified (default = {DefaultDaysAgo})", c => DaysAgoToStart = c);
        }

        /// <summary>
        /// The method resets command arguments to defaults.
        /// </summary>
        public override void ResetProperties()
        {
            StartDate = null;
            EndDate = null;
            DaysAgoToStart = null;
        }

        /// <inheritdoc/>
        public override int Execute(string[] remainingArguments)
        {
            var dateRange = CommandHelper.GetDateRange(StartDate, EndDate, DaysAgoToStart, DefaultDaysAgo);
            DoEtl(dateRange);
            return 0;
        }

        private static void DoEtl(DateRange dateRange)
        {
            Logger.Info($"Roku ETL Started - DateRange: {dateRange}");
            var extractor = new RokuApiExtractor(dateRange);
            var loader = new RokuLoader();
            CommandHelper.DoEtl(extractor, loader);
            Logger.Info($"Roku ETL Finished - DateRange: {dateRange}");
        }
    }
}