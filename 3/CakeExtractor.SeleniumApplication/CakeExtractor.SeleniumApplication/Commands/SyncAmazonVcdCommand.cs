using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD;
using CakeExtractor.SeleniumApplication.Loaders.VCD;
using CakeExtracter;
using CakeExtracter.Helpers;
using CakeExtractor.SeleniumApplication.Configuration.Vcd;
using CakeExtractor.SeleniumApplication.Configuration.Models;

namespace CakeExtractor.SeleniumApplication.Commands
{
    internal class SyncAmazonVcdCommand : BaseAmazonSeleniumCommand
    {
        private readonly AmazonVcdExtractor extractor;
        private readonly VcdCommandConfigurationManager configurationManager;
        private readonly VcdAccountsDataProvider accountsDataProvider;

        public SyncAmazonVcdCommand()
        {
            configurationManager = new VcdCommandConfigurationManager();
            extractor = new AmazonVcdExtractor(configurationManager);
            accountsDataProvider = new VcdAccountsDataProvider();
        }

        public override string CommandName => "SyncAmazonVcdCommand";

        public override void PrepareCommandEnvironment()
        {
            extractor.PrepareExtractor();
            AmazonVcdLoader.PrepareLoader();
        }

        public override void Run()
        {
            SetDateRange();
            var accountsData = accountsDataProvider.GetAccountsDataToProcess(extractor);
            foreach (var accountData in accountsData)
            {
                DoEtlForAccount(extractor, accountData);
            }
        }

        private void SetDateRange()
        {
            var dateRange = configurationManager.GetDaysToProcess();
            extractor.DateRange = dateRange;
            Logger.Info($"Amazon VCD ETL. DateRange {dateRange}.");
        }

        private void DoEtlForAccount(AmazonVcdExtractor extractor, AccountInfo accountInfo)
        {
            Logger.Info($"Amazon VCD, ETL for account {accountInfo.Account.Name} ({accountInfo.Account.Id}) started.");
            extractor.AccountInfo = accountInfo;
            var loader = new AmazonVcdLoader(accountInfo.Account);
            CommandHelper.DoEtl(extractor, loader);
            Logger.Info($"Amazon VCD, ETL for account {accountInfo.Account.Name} ({accountInfo.Account.Id}) finished.");
        }
    }
}