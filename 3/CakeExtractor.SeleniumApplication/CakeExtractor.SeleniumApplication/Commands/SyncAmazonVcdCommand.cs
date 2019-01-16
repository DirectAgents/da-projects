using System;
using CakeExtractor.SeleniumApplication.Models.CommonHelperModels;
using ConsoleCommand = ManyConsole.ConsoleCommand;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD;
using DirectAgents.Domain.Entities.CPProg;
using CakeExtracter.Common;
using CakeExtractor.SeleniumApplication.Loaders.VCD;
using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Concrete;
using CakeExtracter;

namespace CakeExtractor.SeleniumApplication.Commands
{
    internal class SyncAmazonVcdCommand : ConsoleCommand
    {
        public int? AccountId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        private int DaysAgoToStart = 30;

        private int executionNumber;
        private JobScheduleModel scheduling;

        public SyncAmazonVcdCommand()
        {
            IsCommand("SyncAmazonVcdCommand", "Synch Amazon Vendor Central Data Stats");
            HasOption<int>("vcdA|vcdAccountId=", "Account Id (default = all)", c => AccountId = c);
            HasOption("vcdS|vcdStartDate=", "Start Date (default is from config or 'daysAgo')",
                c => StartDate = DateTime.Parse(c));
            HasOption("vcdE|vcdEndDate=", "End Date (default is from config or yesterday)", c => EndDate = DateTime.Parse(c));
            HasOption<int>("d|daysAgo=", "Days Ago to start, if startDate or end date not specified (default is 30})",
                c => DaysAgoToStart = c);
        }

        public override int Run(string[] remainingArguments)
        {
            var extractor = new AmazonVcdExtractor();
            var loader = new AmazonVcdLoader();
            extractor.PrepareExtractor();
            var accounts = GetAccounts();
            foreach (var account in accounts)
            {
                DoEtls(extractor, loader, account);
            }
            return 1;
        }

        private void DoEtls(AmazonVcdExtractor extractor, AmazonVcdLoader loader, ExtAccount account)
        {
            var daysToProcess = GetDaysToProcess();
            daysToProcess.ForEach(day =>
            {
                try
                {
                    Logger.Info("Amazon VCD, ETL for {0} started.", day);
                    var dailyVendorData = extractor.ExtractDailyData(day);
                    //loader.LoadDailyData(dailyVendorData, day, account);
                    Logger.Info("Amazon VCD, ETL for {0} finished.", day);
                }
                catch (Exception ex)
                {
                    Logger.Warn("Amazon VCD, ETL for {0} failed due to exception. Date skipped", day);
                    throw ex; //temporary for development purposes. should be removed in future
                }
            });
        }

        private List<DateTime> GetDaysToProcess()
        {
            var dateRangeToProcess = GetDateRangeToProcess();
            return GetDaysBetweenToDates(dateRangeToProcess.FromDate, dateRangeToProcess.ToDate).ToList();
        }

        private IEnumerable<DateTime> GetDaysBetweenToDates(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

        private DateRange GetDateRangeToProcess()
        {
            if (StartDate.HasValue && EndDate.HasValue)
            {
                return new DateRange(StartDate.Value, EndDate.Value);
            }
            else
            {
                var endDate = DateTime.Today.AddDays(-1);
                var startDate = DateTime.Today.AddDays(-DaysAgoToStart);
                return new DateRange(startDate, endDate);
            }
        }

        private IEnumerable<ExtAccount> GetAccounts()
        {
            var repository = new PlatformAccountRepository();
            if (!AccountId.HasValue)
            {
                throw new Exception("Account should be specified");
            }
            var account = repository.GetAccount(AccountId.Value);
            return new[] { account };
        }
    }
}