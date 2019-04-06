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
using System.ComponentModel.Composition;

namespace CakeExtractor.SeleniumApplication.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class SyncAmazonVcdCommand : ConsoleCommand
    {
        public int ProfileNumber { get; set; }

        private AmazonVcdExtractor extractor;
        private VcdCommandConfigurationManager configurationManager;
        private VcdAccountsDataProvider accountsDataProvider;
        private AuthorizationModel authorizationModel;

        private static AmazonVcdPageActions pageActions;

        public SyncAmazonVcdCommand()
        {
            IsCommand("SyncAmazonVcdCommand", "Sync VCD Stats");
            HasOption<int>("p|profileNumber=", "Profile Number", c => ProfileNumber = c);
        }

        public override void ResetProperties()
        {
            ProfileNumber = 0;
        }

        public override int Execute(string[] remainingArguments)
        {
            VcdExecutionProfileManger.Current.SetExecutionProfileNumber(ProfileNumber);
            if (pageActions == null)
            {
                pageActions = new AmazonVcdPageActions();
            }
            configurationManager = new VcdCommandConfigurationManager();
            InitializeAuthorizationModel();
            extractor = new AmazonVcdExtractor(pageActions, authorizationModel);
            accountsDataProvider = new VcdAccountsDataProvider();
            if (!AmazonVcdExtractor.IsInitialised)
            {
                extractor.PrepareExtractor();
            }
            AmazonVcdLoader.PrepareLoader();
            RunEtl();
            return 0;
        }

        public void RunEtl()
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