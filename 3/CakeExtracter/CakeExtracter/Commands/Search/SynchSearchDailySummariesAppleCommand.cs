using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Apple;
using CakeExtracter.Bootstrappers;
using CakeExtracter.Common;
using CakeExtracter.Etl.SearchMarketing.Extracters;
using CakeExtracter.Etl.SearchMarketing.Loaders;
using CakeExtracter.Helpers;
using ClientPortal.Data.Contexts;

namespace CakeExtracter.Commands.Search
{
    /// <inheritdoc />
    /// The class represents a command that is used to retrieve
    /// statistics from the Apple Search Ads Campaign Management API
    [Export(typeof(ConsoleCommand))]
    public class SynchSearchDailySummariesAppleCommand : ConsoleCommand
    {
        private const int DefaultDaysAgo = 41;

        /// <summary>
        /// Gets or sets a command argument:
        /// Database identifier of search profile for which the command will be executed (default = all).
        /// </summary>
        public int? SearchProfileId { get; set; }

        /// <summary>
        /// Gets or sets a command argument:
        /// Database account code of search account (like external Id)
        /// for which the command will be executed (default = all).
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets a command argument:
        /// Start date from which statistics will be extracted (default is 'daysAgo').
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets a command argument:
        /// End date to which statistics will be extracted (default is yesterday).
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets a command argument:
        /// The number of days ago to calculate the start date from which statistics will be retrieved,
        /// used if StartDate not specified (default = 31).
        /// </summary>
        public int? DaysAgoToStart { get; set; }

        /// <summary>
        /// The static method used for sync from Admin Portal.
        /// </summary>
        /// <param name="searchProfileId">Id of search profile.</param>
        /// <param name="clientId">Account code of search account.</param>
        /// <param name="start">Start date.</param>
        /// <param name="end">End date.</param>
        /// <param name="daysAgoToStart">Days ago.</param>
        /// <returns>Execution code.</returns>
        public static int RunStatic(int? searchProfileId = null, string clientId = null, DateTime? start = null, DateTime? end = null, int? daysAgoToStart = null)
        {
            AutoMapperBootstrapper.CheckRunSetup();
            var cmd = new SynchSearchDailySummariesAppleCommand
            {
                SearchProfileId = searchProfileId,
                ClientId = clientId,
                StartDate = start,
                EndDate = end,
                DaysAgoToStart = daysAgoToStart,
            };
            return cmd.Run();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchSearchDailySummariesAppleCommand"/> class.
        /// </summary>
        public SynchSearchDailySummariesAppleCommand()
        {
            IsCommand("synchSearchDailySummariesApple", "synch SearchDailySummaries for Apple");
            HasOption<int>("p|searchProfileId=", "SearchProfile Id (default = all)", c => SearchProfileId = c);
            HasOption<string>("v|clientId=", "Client Id", c => ClientId = c);
            HasOption<DateTime>("s|startDate=", "Start Date (optional)", c => StartDate = c);
            HasOption<DateTime>("e|endDate=", "End Date (default is yesterday)", c => EndDate = c);
            HasOption<int>("d|daysAgo=", $"Days Ago to start, if startDate not specified (default = {DefaultDaysAgo})", c => DaysAgoToStart = c);
        }

        /// <inheritdoc />
        /// <summary>
        /// The method resets command arguments to defaults.
        /// </summary>
        public override void ResetProperties()
        {
            SearchProfileId = null;
            ClientId = null;
            StartDate = null;
            EndDate = null;
            DaysAgoToStart = null;
        }

        /// <inheritdoc />
        /// <summary>
        /// The method runs the current command and extract and save statistics from
        /// the Apple Search Ads Campaign Management API based on the command arguments.
        /// </summary>
        /// <param name="remainingArguments"></param>
        /// <returns>Execution code.</returns>
        public override int Execute(string[] remainingArguments)
        {
            try
            {
                var dateRange = CommandHelper.GetDateRange(StartDate, EndDate, DaysAgoToStart, DefaultDaysAgo);
                Logger.Info("Apple ETL. DateRange {0}.", dateRange);

                var appleAdsUtility = new AppleAdsUtility(m => Logger.Info(m), m => Logger.Warn(m));
                var searchAccounts = GetSearchAccounts();
                Logger.Info("Returned from GetSearchAccounts().");

                foreach (var searchAccount in searchAccounts)
                {
                    var revisedDateRange = ReviseDateRange(dateRange, searchAccount);
                    var extractor = new AppleApiExtracter(appleAdsUtility, revisedDateRange, searchAccount.AccountCode, searchAccount.ExternalId);
                    var loader = new AppleApiLoader(searchAccount.SearchAccountId);
                    CommandHelper.DoEtl(extractor, loader);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return 0;
        }

        private static DateRange ReviseDateRange(DateRange dateRange, SearchAccount searchAccount)
        {
            var startDate = dateRange.FromDate;
            if (searchAccount.MinSynchDate.HasValue && startDate < searchAccount.MinSynchDate.Value)
            {
                startDate = searchAccount.MinSynchDate.Value;
            }

            var revisedDateRange = new DateRange(startDate, dateRange.ToDate);
            return revisedDateRange;
        }

        private IEnumerable<SearchAccount> GetSearchAccounts()
        {
            return SynchSearchDailySummariesAdWordsCommand.GetSearchAccounts("Apple", this.SearchProfileId, this.ClientId);
        }
    }
}