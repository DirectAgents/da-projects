using Amazon;
using CakeExtracter.Common;
using CakeExtracter.Common.ArchiveExtractors;
using CakeExtracter.Etl;
using CakeExtracter.Etl.Kochava.Configuration;
using CakeExtracter.Etl.Kochava.Extractors;
using CakeExtracter.Etl.Kochava.Extractors.Parsers;
using CakeExtracter.Etl.Kochava.Loaders;
using DirectAgents.Domain.Concrete;
using DirectAgents.Domain.Entities.CPProg;
using System.ComponentModel.Composition;

namespace CakeExtracter.Commands.DA
{
    [Export(typeof(ConsoleCommand))]
    public class DASynchKochavaStats : ConsoleCommand
    {
        private readonly KochavaExtractor extractor;
        private readonly KochavaConfigurationProvider configurationProvider;
        private readonly KochavaLoader loader;
        private readonly CommandsExecutionAccountsProvider accountsProvider;

        private int? AccountId { get; set; }

        public DASynchKochavaStats()
        {
            IsCommand("dASynchKochavaStats", "Synch Kochava Stats");
            HasOption<int>("a|accountId=", "Account Id (default = all)", c => AccountId = c);
            configurationProvider = new KochavaConfigurationProvider();
            var awsFilesDownloader = new AwsFilesDownloader(configurationProvider.GetAwsAccessToken(),
                configurationProvider.GetAwsSecretValue(), m => Logger.Info(m), (ex) => Logger.Error(ex));
            extractor = new KochavaExtractor(configurationProvider, new KochavaReportParser(), awsFilesDownloader, new ZipArchiveExtractor());

            loader = new KochavaLoader(new KochavaCleaner());
            accountsProvider = new CommandsExecutionAccountsProvider(new PlatformAccountRepository());
        }

        public override int Execute(string[] remainingArguments)
        {
            var accounts = accountsProvider.GetAccountsToProcess(Platform.Code_Kochava, AccountId);
            accounts.ForEach(account =>
            {
                ProcessDailyEtlForAccount(account);
            });
            return 0;
        }

        public void ProcessDailyEtlForAccount(ExtAccount account)
        {
            var acccountData = extractor.ExtractLatestAccountData(account);
            loader.LoadData(acccountData, account);
        }

        public override void ResetProperties()
        {
            AccountId = null;
        }
    }
}
