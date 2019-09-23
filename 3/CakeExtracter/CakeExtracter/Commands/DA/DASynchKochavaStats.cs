using Amazon;
using CakeExtracter.Common;
using CakeExtracter.Etl;
using CakeExtracter.Etl.Kochava.Configuration;
using CakeExtracter.Etl.Kochava.Extractors;
using CakeExtracter.Etl.Kochava.Extractors.Parsers;
using CakeExtracter.Etl.Kochava.Loaders;
using DirectAgents.Domain.Concrete;
using DirectAgents.Domain.Entities.CPProg;
using System;
using System.ComponentModel.Composition;
using CakeExtracter.Common.Extractors.ArchiveExctractors;

namespace CakeExtracter.Commands.DA
{
    /// <summary>
    /// Command for Kochava ETL Job
    /// </summary>
    /// <seealso cref="CakeExtracter.Common.ConsoleCommand" />
    [Export(typeof(ConsoleCommand))]
    public class DASynchKochavaStats : ConsoleCommand
    {
        private readonly KochavaExtractor extractor;
        private readonly KochavaConfigurationProvider configurationProvider;
        private readonly KochavaLoader loader;
        private readonly CommandsExecutionAccountsProvider accountsProvider;

        public int? AccountId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DASynchKochavaStats"/> class.
        /// </summary>
        public DASynchKochavaStats()
        {
            IsCommand("dASynchKochavaStats", "Synch Kochava Stats");
            HasOption<int>("a|accountId=", "Account Id (default = all)", c => AccountId = c);
            configurationProvider = new KochavaConfigurationProvider();
            var awsFilesDownloader = new AwsFilesDownloader(configurationProvider.GetAwsAccessToken(),
                configurationProvider.GetAwsSecretValue(), m => Logger.Info(m), (ex) => Logger.Error(ex));
            extractor = new KochavaExtractor(configurationProvider, new KochavaReportParser(), awsFilesDownloader, new ZipArchiveExtractor());

            loader = new KochavaLoader(configurationProvider, new KochavaItemsDbService());
            accountsProvider = new CommandsExecutionAccountsProvider(new PlatformAccountRepository());
        }

        /// <summary>
        /// Executes the specified remaining arguments.
        /// </summary>
        /// <param name="remainingArguments">The remaining arguments.</param>
        /// <returns></returns>
        public override int Execute(string[] remainingArguments)
        {
            var accounts = accountsProvider.GetAccountsToProcess(Platform.Code_Kochava, AccountId);
            if (accounts != null && accounts.Count > 0)
            {
                accounts.ForEach(account =>
                {
                    ProcessDailyEtlForAccount(account);
                });
            }
            else
            {
                Logger.Warn("No Kochava accounts were found to process");
            }
            return 0;
        }

        /// <summary>
        /// Processes the daily etl for account.
        /// </summary>
        /// <param name="account">The account.</param>
        public void ProcessDailyEtlForAccount(ExtAccount account)
        {
            try
            {
                Logger.Info(account.Id, $"Started processing Kochava ETL for account - {account.Id}");
                var accountData = extractor.ExtractLatestAccountData(account);
                if (accountData != null && accountData.Count > 0)
                {
                    loader.LoadData(accountData, account);
                }
                else
                {
                    Logger.Warn(account.Id, $"Loading skipped because no data was extracted.");
                }
                Logger.Info(account.Id, $"Finished processing Kochava ETL for account - {account.Id}");
            }
            catch (Exception ex)
            {
                Logger.Error(account.Id, ex);
                Logger.Warn(account.Id, "Account processing skipped due to the exception");
            }
        }

        /// <summary>
        /// Resets the properties.
        /// </summary>
        public override void ResetProperties()
        {
            AccountId = null;
        }
    }
}
