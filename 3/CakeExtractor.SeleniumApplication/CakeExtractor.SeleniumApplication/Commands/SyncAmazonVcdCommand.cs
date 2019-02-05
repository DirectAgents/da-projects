using System;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD;
using CakeExtractor.SeleniumApplication.Loaders.VCD;
using CakeExtracter;
using CakeExtractor.SeleniumApplication.Configuration.Vcd;
using CakeExtractor.SeleniumApplication.Configuration.Models;

namespace CakeExtractor.SeleniumApplication.Commands
{
    internal class SyncAmazonVcdCommand : BaseAmazonSeleniumCommand
    {
        private AmazonVcdExtractor extractor;

        private AmazonVcdLoader loader;

        private VcdCommandConfigurationManager configurationManager;

        private VcdAccountsDataProvider accountsDataProvider;

        public SyncAmazonVcdCommand()
        {
            configurationManager = new VcdCommandConfigurationManager();
            extractor = new AmazonVcdExtractor(configurationManager);
            loader = new AmazonVcdLoader();
            accountsDataProvider = new VcdAccountsDataProvider();
        }

        public override string CommandName
        {
            get
            {
                return "SyncAmazonVcdCommand";
            }
        }

        public override void PrepareCommandEnvironment()
        {
            extractor.PrepareExtractor();
            loader.PrepareLoader();
        }

        public override void Run()
        {
            var accountsData = accountsDataProvider.GetAccountsDataToProcess(extractor);
            foreach (var accountData in accountsData)
            {
                Logger.Info("Amazon VCD, ETL for {0} account started.", accountData.Account.Id);
                DoEtlForAccount(extractor, loader, accountData);
                Logger.Info("Amazon VCD, ETL for {0} account finished.", accountData.Account.Id);
            }
        }

        private void DoEtlForAccount(AmazonVcdExtractor extractor, AmazonVcdLoader loader, AccountInfo accountInfo)
        {
            var daysToProcess = configurationManager.GetDaysToProcess();
            daysToProcess.ForEach(day =>
            {
                try
                {
                    Logger.Info("Amazon VCD, ETL for {0} started. Account {1} - {2}", day, accountInfo.Account.Name, accountInfo.Account.Id);
                    var dailyVendorData = extractor.ExtractDailyData(day, accountInfo);
                    loader.LoadDailyData(dailyVendorData, day, accountInfo.Account);
                    Logger.Info("Amazon VCD, ETL for {0} finished.  Account {1} - {2}", day, accountInfo.Account.Name, accountInfo.Account.Id);
                }
                catch (Exception ex)
                {
                    Logger.Warn("Amazon VCD, ETL for {0} failed due to exception. Date skipped", day);
                }
            });
        }
    }
}