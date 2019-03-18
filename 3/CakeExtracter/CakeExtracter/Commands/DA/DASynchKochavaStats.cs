using CakeExtracter.Common;
using CakeExtracter.Etl;
using CakeExtracter.Etl.DSP.Configuration;
using CakeExtracter.Etl.Kochava.Configuration;
using CakeExtracter.Etl.Kochava.Extractors;
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
            extractor = new KochavaExtractor(configurationProvider);
            loader = new KochavaLoader();
            accountsProvider = new CommandsExecutionAccountsProvider(new PlatformAccountRepository());
        }

        public override int Execute(string[] remainingArguments)
        {
            var accounts = accountsProvider.GetAccountsToProcess(Platform.Code_DspAmazon, AccountId);
            return 0;
        }

        public void ProcessDailyEtlForAccount(ExtAccount account)
        {
            var acccountData = extractor.ExtractLatestAccountData(account);
        }

        public override void ResetProperties()
        {
            throw new System.NotImplementedException();
        }
    }
}
