using System;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD;
using CakeExtractor.SeleniumApplication.Loaders.VCD;
using System.Collections.Generic;
using DirectAgents.Domain.Concrete;
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

        private PlatformAccountRepository accountsRepository;

        public SyncAmazonVcdCommand()
        {
            configurationManager = new VcdCommandConfigurationManager();
            extractor = new AmazonVcdExtractor(configurationManager);
            loader = new AmazonVcdLoader();
            accountsRepository = new PlatformAccountRepository();
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
            var accountsData = GetAccountsDataToProcess();
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
                    Logger.Info("Amazon VCD, ETL for {0} started.", day);
                    var dailyVendorData = extractor.ExtractDailyData(day, accountInfo);
                    loader.LoadDailyData(dailyVendorData, day, accountInfo.Account);
                    Logger.Info("Amazon VCD, ETL for {0} finished.", day);
                }
                catch (Exception ex)
                {
                    Logger.Warn("Amazon VCD, ETL for {0} failed due to exception. Date skipped", day);
                    throw ex; //temporary for development purposes. should be removed in future
                }
            });
        }

        // in current implementation multiple accounts processing is not supported
        // account data for processing defined in config
        // in future account for processing should be defined in database, vendorGroupId and mcId 
        // can be fetched from page using selenium (See UserInfoExtracter.cs) or also can be stored in database
        private List<AccountInfo> GetAccountsDataToProcess()
        {
            var account = accountsRepository.GetAccount(configurationManager.GetAccountId());
            return new List<AccountInfo>()
            {
               new AccountInfo
               {
                   Account = account,
                   McId = configurationManager.GetMcId(),
                   VendorGroupId = configurationManager.GetVendorGroupId()
               }
            };
        }
    }
}