using System;
using CakeExtractor.SeleniumApplication.Models.CommonHelperModels;
using ConsoleCommand = ManyConsole.ConsoleCommand;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD;

namespace CakeExtractor.SeleniumApplication.Commands
{
    internal class SyncAmazonVcdCommand : ConsoleCommand
    {
        public int? AccountId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? DaysAgoToStart { get; set; }
        public string StatsType { get; set; }
        public bool DisabledOnly { get; set; }
        public bool FromDatabase { get; set; }

        private const int DefaultDaysAgoValue = 41;

        private int executionNumber;
        private JobScheduleModel scheduling;

        public SyncAmazonVcdCommand()
        {
            IsCommand("SyncAmazonVcdCommand", "Synch Amazon Vendor Central Data Stats");
            HasOption<int>("a|accountId=", "Account Id (default = all)", c => AccountId = c);
            HasOption("s|startDate=", "Start Date (default is from config or 'daysAgo')",
                c => StartDate = DateTime.Parse(c));
            HasOption("e|endDate=", "End Date (default is from config or yesterday)", c => EndDate = DateTime.Parse(c));
            HasOption<int>("d|daysAgo=",
                $"Days Ago to start, if startDate not specified (default is from config or {DefaultDaysAgoValue})",
                c => DaysAgoToStart = c);
            HasOption<string>("t|statsType=", "Stats Type (default: all)", c => StatsType = c);
            HasOption<bool>("x|disabledOnly=", "Include only disabled accounts (default = false)",
                c => DisabledOnly = c);
            HasOption<bool>("z|fromDatabase=", "Retrieve from database instead of API (where implemented - Daily)",
                c => FromDatabase = c);
        }

        public override int Run(string[] remainingArguments)
        {
            var extractor = new AmazonVcdExtractor();
            extractor.PrepareExtractor();
            return 1;
        }
    }
}