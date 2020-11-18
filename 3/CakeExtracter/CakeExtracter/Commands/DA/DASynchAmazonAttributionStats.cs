using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

using Amazon;

using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement;
using CakeExtracter.Etl.Amazon.Extractors.AmazonApiExtractors;
using CakeExtracter.Etl.Amazon.Loaders;
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
            AmazonUtility.TokenSets = GetTokens();

            foreach (var account in accounts)
            {
                Logger.Info("Start ETL for Amazon Attribution account ({0}) {1}", account.Id, account.Name);
                CommandExecutionContext.Current.SetJobExecutionStateInHistory("Started", account.Id);
                DoEtl(account, dateRange);
                CommandExecutionContext.Current.SetJobExecutionStateInHistory("Finished", account.Id);
            }
            SaveTokens(AmazonUtility.TokenSets);
            return 0;
        }

        private void DoEtl(ExtAccount account, DateRange dateRange)
        {
            var amazonUtility = CreateUtility(account);
            AmazonTimeTracker.Instance.ExecuteWithTimeTracking(
                () =>
                    {
                        var extractor = new AmazonAttributionExtractor(amazonUtility, dateRange, account);
                        var loader = new AmazonAttributionLoader(account.Id);
                        CommandHelper.DoEtl(extractor, loader);
                    },
                account.Id,
                AmazonJobLevels.Attribution,
                AmazonJobOperations.Total);
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
            var amazonUtility = new AmazonUtility(m => Logger.Info(account.Id, m), m => Logger.Warn(account.Id, m));
            amazonUtility.SetWhichAlt(account.ExternalId);
            amazonUtility.SetApiEndpointUrl(account.Name);
            return amazonUtility;
        }

        private string[] GetTokens()
        {
            return Platform.GetPlatformTokens(Platform.Code_AttributionAmazon);
        }

        private void SaveTokens(string[] tokens)
        {
            Platform.SavePlatformTokens(Platform.Code_AttributionAmazon, tokens);
        }
    }
}
