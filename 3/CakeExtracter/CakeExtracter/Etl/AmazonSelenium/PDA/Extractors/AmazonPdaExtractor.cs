using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Amazon.Enums;
using Amazon.Helpers;
using CakeExtracter.Common;
using CakeExtracter.Etl.AmazonSelenium.Drivers;
using CakeExtracter.Etl.AmazonSelenium.Helpers;
using CakeExtracter.Etl.AmazonSelenium.PDA.Configuration;
using CakeExtracter.Etl.AmazonSelenium.PDA.Exceptions;
using CakeExtracter.Etl.AmazonSelenium.PDA.Models;
using CakeExtracter.Etl.AmazonSelenium.PDA.Models.CommonHelperModels;
using CakeExtracter.Etl.AmazonSelenium.PDA.Models.ConsoleManagerUtilityModels;
using CakeExtracter.Etl.AmazonSelenium.PDA.PageActions;
using CakeExtracter.Etl.AmazonSelenium.PDA.Utilities;
using CakeExtracter.Etl.AmazonSelenium.PDA.Helpers;
using DirectAgents.Domain.Entities.CPProg;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Polly;
using FileManager = CakeExtracter.Etl.AmazonSelenium.Helpers.FileManager;

namespace CakeExtracter.Etl.AmazonSelenium.PDA.Extractors
{
    internal class AmazonPdaExtractor
    {
        private static AmazonPdaPageActions pageActions;
        private static AuthorizationModel authorizationModel;
        private static string downloadDir;
        private static string reportNameTemplate;
        private static Dictionary<string, string> availableProfileUrls;
        private static PdaConfigurationManager configurationManager;

        private readonly ExtAccount account;

        public static bool IsInitialised
        {
            get;
            private set;
        }

        static AmazonPdaExtractor()
        {
            Initialize();
        }

        public AmazonPdaExtractor(ExtAccount account)
        {
            this.account = account;
        }

        /// Login to the advertising portal and saving cookies
        public static void PrepareExtractor()
        {
            authorizationModel.Cookies = CookieManager.GetCookiesFromFiles(authorizationModel.CookiesDir);
            var cookiesExist = authorizationModel.Cookies.Any();

            if (cookiesExist)
            {
                Logger.Info("Login into the portal using cookies.");
                LoginWithCookie(authorizationModel);
                return;
            }
            Logger.Warn("Login into the portal without using cookies. Please enter an authorization code!");
            LoginWithoutCookie(authorizationModel);
            IsInitialised = true;
        }

        /// <summary>
        /// The method sets the URLs for the available profiles on main page of portal
        /// </summary>
        public static void SetAvailableProfileUrls()
        {
            try
            {
                GoToPortalMainPage();
                TrySetAvailableProfiles();
            }
            catch (Exception e)
            {
                throw new Exception($"Could not to get the profile URLs: {e.Message}", e);
            }
        }

        private static void Initialize()
        {
            SetConfigurationManager();
            InitializeSettings();
            InitializeAuthorizationModel();
            InitializePageActions();
            FileManager.CreateDirectoryIfNotExist(downloadDir);
            FileManager.CreateDirectoryIfNotExist(authorizationModel.CookiesDir);
        }

        private static void SetConfigurationManager()
        {
            configurationManager = new PdaConfigurationManager();
        }

        private static void InitializeSettings()
        {
            reportNameTemplate = configurationManager.GetFilesNameTemplate();
            downloadDir = FileManager.GetAssemblyRelativePath(configurationManager.GetDownloadsDirectoryName());
        }

        private static void InitializeAuthorizationModel()
        {
            var cookieDirName = configurationManager.GetCookiesDirectoryName();
            authorizationModel = new AuthorizationModel
            {
                Login = configurationManager.GetEMail(),
                Password = configurationManager.GetEMailPassword(),
                SignInUrl = configurationManager.GetSignInPageUrl(),
                CookiesDir = FileManager.GetAssemblyRelativePath(cookieDirName),
            };
        }

        private static void InitializePageActions()
        {
            var driver = new ChromeWebDriver(downloadDir);
            var waitPageTimeoutInMinutes = configurationManager.GetWaitPageTimeout();
            pageActions = new AmazonPdaPageActions(driver, waitPageTimeoutInMinutes);
        }

        private static void LoginWithoutCookie(AuthorizationModel authModel)
        {
            pageActions.NavigateToUrl(authModel.SignInUrl, AmazonPdaPageObjects.ForgotPassLink);
            pageActions.LoginProcess(authModel.Login, authModel.Password);
            var cookies = pageActions.GetAllCookies();
            CookieManager.SaveCookiesToFiles(cookies, authModel.CookiesDir);
        }

        private static void LoginWithCookie(AuthorizationModel authModel)
        {
            pageActions.NavigateToUrl(authModel.SignInUrl);
            foreach (var cookie in authModel.Cookies)
            {
                pageActions.SetCookie(cookie);
            }
        }

        private static void SetAvailableProfiles()
        {
            availableProfileUrls = pageActions.GetAvailableProfileUrls();
            Logger.Info("The following profiles were found for the current account:");
            availableProfileUrls.ForEach(x => Logger.Info($"{x.Key} - {x.Value}"));
        }

        public IEnumerable<AmazonCmApiCampaignSummary> ExtractCampaignApiFullSummaries(DateRange dateRange)
        {
            var accountEntityId = GetCurrentProfileEntityId();
            var cmApiUtility = GetAmazonConsoleManagerUtility();
            var campaignsInfos = cmApiUtility.GetPdaCampaignsSummaries(accountEntityId, dateRange);
            var campaignType = AmazonApiHelper.GetCampaignTypeName(CampaignType.ProductDisplay);
            campaignsInfos.ForEach(x => x.Type = campaignType);
            return campaignsInfos.ToList();
        }

        private AmazonConsoleManagerUtility GetAmazonConsoleManagerUtility()
        {
            var cookies = pageActions.GetAllCookies();
            var maxRetryAttempts = configurationManager.GetMaxRetryAttempts();
            var pauseBetweenAttempts = configurationManager.GetPauseBetweenAttempts();

            var cmApiUtility = new AmazonConsoleManagerUtility(
                cookies,
                maxRetryAttempts,
                pauseBetweenAttempts,
                x => Logger.Info(account.Id, x),
                x => Logger.Error(account.Id, new Exception(x)),
                x => Logger.Warn(account.Id, x));
            return cmApiUtility;
        }

        private string GetCurrentProfileEntityId()
        {
            var profileUrl = GetAvailableProfileUrl(account.Name);
            var accountEntityId = GetProfileEntityId(profileUrl);
            return accountEntityId;
        }

        private string GetAvailableProfileUrl(string profileName)
        {
            var name = profileName.Trim();
            var availableProfileUrl = availableProfileUrls.FirstOrDefault(x =>
                string.Equals(x.Key, name, StringComparison.OrdinalIgnoreCase));
            var url = availableProfileUrl.Value;
            if (string.IsNullOrEmpty(url))
            {
                // The current account does not have the following profile
                throw new AccountDoesNotHaveProfileException(authorizationModel.Login, account.Name);
            }
            return url;
        }

        private string GetProfileEntityId(string url)
        {
            var uri = new Uri(url);
            var queryParams = HttpUtility.ParseQueryString(uri.Query);
            var entityId = queryParams.Get(AmazonCmApiHelper.EntityIdArgName);
            return entityId;
        }

        private static void GoToPortalMainPage()
        {
            var campaignsUrl = configurationManager.GetCampaignsPageUrl();
            pageActions.NavigateToUrl(campaignsUrl, AmazonPdaPageObjects.FilterByButton);
            if (!pageActions.IsElementPresent(AmazonPdaPageObjects.FilterByButton) &&
                pageActions.IsElementPresent(AmazonPdaPageObjects.LoginPassInput))
            {
                // need to repeat the password
                pageActions.LoginByPassword(authorizationModel.Password, AmazonPdaPageObjects.FilterByButton);
            }
        }

        private static void TrySetAvailableProfiles()
        {
            const int maxRetryAttempts = 5;
            var pauseBetweenAttempts = TimeSpan.FromSeconds(3);
            Policy
                .Handle<Exception>()
                .WaitAndRetry(
                    maxRetryAttempts,
                    i => pauseBetweenAttempts,
                    (exception, timeSpan, retryCount, context) =>
                    {
                        var message = $"Waiting {timeSpan} before setting URLs";
                        LoggerHelper.LogWaiting(message, retryCount, x => Logger.Info(x));
                    })
                .Execute(SetAvailableProfiles);
        }
    }
}