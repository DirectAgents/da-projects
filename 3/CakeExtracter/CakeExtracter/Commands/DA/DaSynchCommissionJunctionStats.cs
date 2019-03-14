using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using CakeExtracter.Common;
using CakeExtracter.Etl.TradingDesk.Extracters.CommissionJunctionExtractors;
using CakeExtracter.Etl.TradingDesk.LoadersDA.CommissionJunctionLoaders;
using CakeExtracter.Helpers;
using CommissionJunction.Utilities;
using DirectAgents.Domain.Concrete;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Commands.DA
{
    ///The class represents a command that is used to retrieve statistics from the Commission Junction portal
    [Export(typeof(ConsoleCommand))]
    public class DaSynchCommissionJunctionStats : ConsoleCommand
    {
        private const int DefaultDaysAgo = 31;

        /// <summary>
        /// Command argument: Account ID in the database for which the command will be executed (default = all)
        /// </summary>
        public int? AccountId { get; set; }
        
        /// <summary>
        /// Command argument: Start date from which statistics will be extracted (default is 'daysAgo')
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Command argument: End date to which statistics will be extracted (default is yesterday)
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Command argument: The number of days ago to calculate the start date from which statistics will be retrieved,
        /// used if StartDate not specified (default = 31)
        /// </summary>
        public int? DaysAgoToStart { get; set; }

        /// <summary>
        /// The constructor sets a command name and command arguments names, provides a description for them.
        /// </summary>
        public DaSynchCommissionJunctionStats()
        {
            IsCommand("DaSynchCommissionJunctionStats", "Synch Commission Junction Stats");
            HasOption<int>("a|accountId=", "Account Id (default = all)", c => AccountId = c);
            HasOption("s|startDate=", "Start Date (default is 'daysAgo')", c => StartDate = DateTime.Parse(c));
            HasOption("e|endDate=", "End Date (default is yesterday)", c => EndDate = DateTime.Parse(c));
            HasOption<int>("d|daysAgo=", $"Days Ago to start, if startDate not specified (default = {DefaultDaysAgo})", c => DaysAgoToStart = c);
        }

        /// <summary>
        /// The method resets command arguments to defaults
        /// </summary>
        public override void ResetProperties()
        {
            AccountId = null;
            StartDate = null;
            EndDate = null;
            DaysAgoToStart = null;
        }

        /// <summary>
        /// The method runs the current command and extract and save statistics from the Commission Junction portal based on the command arguments.
        /// </summary>
        /// <param name="remainingArguments"></param>
        /// <returns>Execution code</returns>
        public override int Execute(string[] remainingArguments)
        {
            var dateRange = CommandHelper.GetDateRange(StartDate, EndDate, DaysAgoToStart, DefaultDaysAgo);
            Logger.Info("Commission Junction ETL. DateRange {0}.", dateRange);
            var accounts = GetAccounts();
            Parallel.ForEach(accounts, account =>
            {
                Logger.Info(account.Id, "Commencing ETL for Commission Junction account ({0}) {1}", account.Id, account.Name);
                var utility = CreateUtility(account);
                DoEtls(utility, dateRange, account);
                Logger.Info(account.Id, "Finished ETL for Commission Junction account ({0}) {1}", account.Id, account.Name);
            });
            return 0;
        }

        private CjUtility CreateUtility(ExtAccount account)
        {
            var utility = new CjUtility(message => Logger.Info(account.Id, message),
                exc => Logger.Error(account.Id, exc), message => Logger.Warn(account.Id, message));
            utility.SetWhichAlt(account.ExternalId);
            return utility;
        }

        private void DoEtls(CjUtility utility, DateRange dateRange, ExtAccount account)
        {
            var extractor = new CommissionJunctionAdvertiserExtractor(utility, dateRange, account);
            var loader = new CommissionJunctionAdvertiserLoader();
            CommandHelper.DoEtl(extractor, loader);
        }

        private IEnumerable<ExtAccount> GetAccounts()
        {
            var repository = new PlatformAccountRepository();
            if (!AccountId.HasValue)
            {
                var accounts = repository.GetAccountsWithFilledExternalIdByPlatformCode(Platform.Code_CJ, false);
                return accounts;
            }
            var account = repository.GetAccount(AccountId.Value);
            return new[] { account };
        }
    }
}
