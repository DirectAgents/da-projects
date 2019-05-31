using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using CakeExtracter;
using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement;
using CakeExtracter.Helpers;
using CakeExtractor.SeleniumApplication.Configuration.Models;
using CakeExtractor.SeleniumApplication.Configuration.Vcd;
using CakeExtractor.SeleniumApplication.Loaders.VCD;
using CakeExtractor.SeleniumApplication.Models.CommonHelperModels;
using CakeExtractor.SeleniumApplication.PageActions.AmazonVcd;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD;
using CakeExtractor.SeleniumApplication.Synchers;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;

namespace CakeExtractor.SeleniumApplication.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class SyncAmazonVcdCommand : ConsoleCommand
    {
        public int ProfileNumber { get; set; }

        private VcdCommandConfigurationManager configurationManager;
        private VcdAccountsDataProvider accountsDataProvider;
        private AuthorizationModel authorizationModel;

        private static AmazonVcdPageActions pageActions;

        public SyncAmazonVcdCommand()
        {
            NoNeedToCreateRepeatRequests = true;
            IsAutoShutDownMechanismEnabled = false; // TODO : Enable shut down mechanism when selenium job will be refactored to common job.
            IsCommand("SyncAmazonVcdCommand", "Sync VCD Stats");
            HasOption<int>("p|profileNumber=", "Profile Number", c => ProfileNumber = c);
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
            accountsDataProvider = new VcdAccountsDataProvider();
            if (!AmazonVcdExtractor.IsInitialised)
            {
                AmazonVcdExtractor.PrepareExtractor(pageActions,authorizationModel);
            }
            AmazonVcdLoader.PrepareLoader();
            RunEtls();
            return 0;
        }

        public void RunEtls()
        {
            pageActions.RefreshSalesDiagnosticPage(authorizationModel);
            var dateRanges = configurationManager.GetDateRangesToProcess();
            var accountsData = accountsDataProvider.GetAccountsDataToProcess(pageActions);
            dateRanges.ForEach(d => RunForDateRange(d, accountsData));
        }

        private void RunForDateRange(DateRange dateRange, List<AccountInfo> accountsData)
        {
            Logger.Info($"\nAmazon VCD ETL. DateRange {dateRange}.");
            foreach (var accountData in accountsData)
            {
                DoEtlForAccount(accountData, dateRange);
                SyncAccountDataToAnalyticTable(accountData.Account.Id);
            }
        }

        private void DoEtlForAccount(AccountInfo accountInfo, DateRange dateRange)
        {
            Logger.Info(accountInfo.Account.Id, $"Amazon VCD, ETL for account {accountInfo.Account.Name} ({accountInfo.Account.Id}) started.");
            var extractor = new AmazonVcdExtractor(pageActions, authorizationModel);
            PrepareExtractorForAccount(extractor, accountInfo, dateRange);
            var loader = new AmazonVcdLoader(accountInfo.Account);
            CommandHelper.DoEtl(extractor, loader);
            Logger.Info(accountInfo.Account.Id, $"Amazon VCD, ETL for account {accountInfo.Account.Name} ({accountInfo.Account.Id}) finished.");
        }

        private void PrepareExtractorForAccount(AmazonVcdExtractor extractor, AccountInfo accountInfo, DateRange dateRange)
        {
            extractor.Initialize();
            extractor.AccountInfo = accountInfo;
            extractor.OpenAccountPage();
            extractor.DateRange = dateRange;
        }

        private void SyncAccountDataToAnalyticTable(int accountId)
        {
            try
            {
                CommandExecutionContext.Current.SetJobExecutionStateInHistory($"Sync analytic table data.", accountId);
                Logger.Info(accountId, "Sync analytic table data.");
                var vcdTablesSyncher = new VcdAnalyticTablesSyncher();
                vcdTablesSyncher.SyncData(accountId);
            }
            catch (Exception ex)
            {
                Logger.Error(accountId, new Exception("Error occurred while sync VCD data to analytic table", ex));
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