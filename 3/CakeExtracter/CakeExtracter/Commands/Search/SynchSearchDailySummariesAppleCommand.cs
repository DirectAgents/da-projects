﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Apple;
using CakeExtracter.Bootstrappers;
using CakeExtracter.Common;
using CakeExtracter.Etl.SearchMarketing.Extracters;
using CakeExtracter.Etl.SearchMarketing.Loaders;
using ClientPortal.Data.Contexts;

namespace CakeExtracter.Commands.Search
{
    [Export(typeof(ConsoleCommand))]
    public class SynchSearchDailySummariesAppleCommand : ConsoleCommand
    {
        public int? SearchProfileId { get; set; }
        public string ClientId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? DaysAgoToStart { get; set; }

        public override void ResetProperties()
        {
            SearchProfileId = null;
            ClientId = null;
            StartDate = null;
            EndDate = null;
            DaysAgoToStart = null;
        }

        public SynchSearchDailySummariesAppleCommand()
        {
            IsCommand("synchSearchDailySummariesApple", "synch SearchDailySummaries for Apple");
            HasOption<int>("p|searchProfileId=", "SearchProfile Id (default = all)", c => SearchProfileId = c);
            HasOption<string>("v|clientId=", "Client Id", c => ClientId = c);
            HasOption<DateTime>("s|startDate=", "Start Date (optional)", c => StartDate = c);
            HasOption<DateTime>("e|endDate=", "End Date (default is yesterday)", c => EndDate = c);
            HasOption<int>("d|daysAgo=", "Days Ago to start, if startDate not specified (default = 62)", c => DaysAgoToStart = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            if (!DaysAgoToStart.HasValue)
                DaysAgoToStart = 62; // used if StartDate==null
            var today = DateTime.Today;
            var yesterday = today.AddDays(-1);
            var dateRange = new DateRange(StartDate ?? today.AddDays(-DaysAgoToStart.Value), EndDate ?? yesterday);

            var appleAdsUtility = new AppleAdsUtility(m => Logger.Info(m), m => Logger.Warn(m));
            var searchAccounts = GetSearchAccounts();
            foreach (var searchAccount in searchAccounts)
            {
                DateTime startDate = dateRange.FromDate;
                if (searchAccount.MinSynchDate.HasValue && (startDate < searchAccount.MinSynchDate.Value))
                    startDate = searchAccount.MinSynchDate.Value;
                var revisedDateRange = new DateRange(startDate, dateRange.ToDate);

                var extracter = new AppleApiExtracter(appleAdsUtility, revisedDateRange, searchAccount.AccountCode);
                var loader = new AppleApiLoader(searchAccount.SearchAccountId);
                var extracterThread = extracter.Start();
                var loaderThread = loader.Start(extracter);
                extracterThread.Join();
                loaderThread.Join();
            }
            return 0;
        }

        public IEnumerable<SearchAccount> GetSearchAccounts()
        {
            return SynchSearchDailySummariesAdWordsCommand.GetSearchAccounts("Apple", this.SearchProfileId, this.ClientId);
        }
    }

}
