using System.Collections.Generic;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD;
using CakeExtractor.SeleniumApplication.Loaders.VCD;
using CakeExtracter;
using CakeExtracter.Common;
using CakeExtracter.Helpers;
using CakeExtractor.SeleniumApplication.Configuration.Vcd;
using CakeExtractor.SeleniumApplication.Configuration.Models;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using CakeExtractor.SeleniumApplication.Synchers;
using System;
using CakeExtractor.SeleniumApplication.PageActions.AmazonVcd;
using CakeExtractor.SeleniumApplication.Models.CommonHelperModels;

namespace CakeExtractor.SeleniumApplication.Commands
{
    internal class SyncAmazonVcdCommand : BaseAmazonSeleniumCommand
    {
        private AmazonVcdExtractor extractor;
        private VcdCommandConfigurationManager configurationManager;
        private VcdAccountsDataProvider accountsDataProvider;
        private AmazonVcdPageActions pageActions;
        private AuthorizationModel authorizationModel;

        public SyncAmazonVcdCommand()
        {
        }

        public override string CommandName => "SyncAmazonVcdCommand";

        public override void PrepareCommandEnvironment(int? executionProfileNumber)
        {
            VcdExecutionProfileManger.Current.SetExecutionProfileNumber(executionProfileNumber);
            InitializeAuthorizationModel();
            configurationManager = new VcdCommandConfigurationManager();
            pageActions = new AmazonVcdPageActions();
            extractor = new AmazonVcdExtractor(configurationManager, pageActions, authorizationModel);
            accountsDataProvider = new VcdAccountsDataProvider();
            extractor.PrepareExtractor();
            AmazonVcdLoader.PrepareLoader();
        }

        public override void Run()
        {
            pageActions.RefreshSalesDiagnosticPage(authorizationModel);
            var dateRanges = configurationManager.GetDateRangesToProcess();
            var accountsData = accountsDataProvider.GetAccountsDataToProcess(extractor);
            dateRanges.ForEach(d => RunForDateRange(d, accountsData));
        }

        private void RunForDateRange(DateRange dateRange, List<AccountInfo> accountsData)
        {
            extractor.DateRange = dateRange;
            Logger.Info($"\nAmazon VCD ETL. DateRange {dateRange}.");
            foreach (var accountData in accountsData)
            {
                DoEtlForAccount(accountData);
                SyncAccountDataToAnalyticTable(accountData.Account.Id);
            }
        }

        private void DoEtlForAccount(AccountInfo accountInfo)
        {
            Logger.Info(accountInfo.Account.Id, $"Amazon VCD, ETL for account {accountInfo.Account.Name} ({accountInfo.Account.Id}) started.");
            PrepareExtractorForAccount(accountInfo);
            var loader = new AmazonVcdLoader(accountInfo.Account);
            CommandHelper.DoEtl(extractor, loader);
            Logger.Info(accountInfo.Account.Id, $"Amazon VCD, ETL for account {accountInfo.Account.Name} ({accountInfo.Account.Id}) finished.");
        }

        private void PrepareExtractorForAccount(AccountInfo accountInfo)
        {
            extractor.Initialize();
            extractor.AccountInfo = accountInfo;
            extractor.OpenAccountPage();
        }

        private void SyncAccountDataToAnalyticTable(int accountId)
        {
            try
            {
                var vcdTablesSyncher = new VcdAnalyticTablesSyncher();
                vcdTablesSyncher.SyncData(accountId);
            }
            catch (Exception ex)
            {
                Logger.Error(accountId, new Exception("Error occured while sync vcd data to analytic table", ex));
            }
        }

        private void InitializeAuthorizationModel()
        {
            authorizationModel = new AuthorizationModel
            {
                Login = VcdExecutionProfileManger.Current.ProfileConfiguration.LoginEmail,
                Password = VcdExecutionProfileManger.Current.ProfileConfiguration.LoginPassword,
                SignInUrl = VcdExecutionProfileManger.Current.ProfileConfiguration.SignInUrl,
                CookiesDir = VcdExecutionProfileManger.Current.ProfileConfiguration.CookiesDirectory
            };
        }
    }
}