using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Amazon.Enums;
using Amazon.Helpers;
using CakeExtracter;
using CakeExtracter.Common;
using CakeExtractor.SeleniumApplication.Drivers;
using CakeExtractor.SeleniumApplication.Helpers;
using CakeExtractor.SeleniumApplication.Models;
using CakeExtractor.SeleniumApplication.Models.CommonHelperModels;
using CakeExtractor.SeleniumApplication.Models.ConsoleManagerUtilityModels;
using CakeExtractor.SeleniumApplication.PageActions.AmazonPda;
using CakeExtractor.SeleniumApplication.Utilities;
using DirectAgents.Domain.Entities.CPProg;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using FileManager = CakeExtractor.SeleniumApplication.Helpers.FileManager;

namespace CakeExtractor.SeleniumApplication.SeleniumExtractors.AmazonPdaExtractors
{
    public class AmazonPdaExtractor
    {
        private static AmazonPdaPageActions pageActions;
        private static AuthorizationModel authorizationModel;
        private static string downloadDir;
        private static string reportNameTemplate;
        private static string campaignsUrl;

        private static Dictionary<string, string> availableProfileUrls;

        private readonly ExtAccount account;

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
        }

        public static void SetAvailableProfileUrls()
        {
            try
            {
                pageActions.NavigateToUrl(campaignsUrl, AmazonPdaPageObjects.FilterByButton);
                if (!pageActions.IsElementPresent(AmazonPdaPageObjects.FilterByButton) &&
                    pageActions.IsElementPresent(AmazonPdaPageObjects.LoginPassInput))
                {
                    // need to repeat the password
                    pageActions.LoginByPassword(authorizationModel.Password);
                }
                SetAvailableProfiles();
            }
            catch (Exception e)
            {
                throw new Exception($"Could not to get profile URLs: {e.Message}", e);
            }
        }

        private static void Initialize()
        {
            InitializeSettings();
            InitializeAuthorizationModel();
            InitializePageActions();
            FileManager.CreateDirectoryIfNotExist(downloadDir);
            FileManager.CreateDirectoryIfNotExist(authorizationModel.CookiesDir);
        }

        private static void InitializeSettings()
        {
            campaignsUrl = Properties.Settings.Default.CampaignsPageUrl;
            reportNameTemplate = Properties.Settings.Default.FilesNameTemplate;
            downloadDir = FileManager.GetAssemblyRelativePath(Properties.Settings.Default.DownloadsDirectoryName);
        }

        private static void InitializeAuthorizationModel()
        {
            var cookieDir = Properties.Settings.Default.CookiesDirectory;
            authorizationModel = new AuthorizationModel
            {
                Login = Properties.Settings.Default.EMail,
                Password = Properties.Settings.Default.EMailPassword,
                SignInUrl = Properties.Settings.Default.SignInPageUrl,
                CookiesDir = FileManager.GetAssemblyRelativePath(cookieDir)
            };
        }

        private static void InitializePageActions()
        {
            var driver = new ChromeWebDriver(downloadDir);
            var waitPageTimeoutInMinutes = Properties.Settings.Default.WaitPageTimeoutInMinuts;
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

        public void Extract(Action<List<string>> extractAction)
        {
            try
            {
                NavigateToPageCampaignInfo();
                var campaignUrlList = GetCampaignPageUrls();
                extractAction(campaignUrlList);
            }
            catch (Exception e)
            {
                Logger.Error(account.Id, new Exception($"Could not extract campaigns information: {e.Message}", e));
            }
            finally
            {
                FileManager.CleanDirectory(downloadDir, reportNameTemplate);
            }
        }

        public IEnumerable<AmazonCmApiCampaignSummary> ExtractCampaignApiTruncatedSummaries(DateRange dateRange)
        {
            var cmApiUtility = GetAmazonConsoleManagerUtility();
            var accountEntityId = GetCurrentProfileEntityId();
            var campaignsInfos = cmApiUtility.GetPdaCampaignsTruncatedSummaries(accountEntityId, dateRange);
            return campaignsInfos.ToList();
        }

        public IEnumerable<AmazonCmApiCampaignSummary> ExtractCampaignApiFullSummaries(DateRange dateRange)
        {
            var cmApiUtility = GetAmazonConsoleManagerUtility();
            var accountEntityId = GetCurrentProfileEntityId();
            var campaignsInfos = cmApiUtility.GetPdaCampaignsSummaries(accountEntityId, dateRange);
            var campaignType = AmazonApiHelper.GetCampaignTypeName(CampaignType.ProductDisplay);
            campaignsInfos.ForEach(x => x.Type = campaignType);
            return campaignsInfos.ToList();
        }

        public CampaignInfo ExtractCampaignInfo(string campaignUrl, DateRange dateRange)
        {
            var campaign = new CampaignInfo();
            pageActions.NavigateToUrl(campaignUrl, AmazonPdaPageObjects.CampaignTabContainer);
            GetCampaignSettingsInfo(campaign);
            if (IsCampaignValid(campaign, dateRange))
            {
                GetCampaignReportInfo(campaign);
            }

            return campaign;
        }

        private AmazonConsoleManagerUtility GetAmazonConsoleManagerUtility()
        {
            var cookies = pageActions.GetAllCookies();
            var cmApiUtility = new AmazonConsoleManagerUtility(cookies,
                x => Logger.Info(account.Id, x), x => Logger.Warn(account.Id, x));
            return cmApiUtility;
        }

        private void GetCampaignSettingsInfo(CampaignInfo campaign)
        {
            pageActions.ClickOnTab(AmazonPdaPageObjects.CampaignTabSet, AmazonPdaPageObjects.CampaignSettingsTab);
            pageActions.GetCampaignSettingsInfo(campaign);
            campaign.Type = AmazonApiHelper.GetCampaignTypeName(CampaignType.ProductDisplay);
        }

        private bool IsCampaignValid(CampaignInfo campaign, DateRange dateRange)
        {
            if (!IsValidReportDateRange(dateRange, campaign))
            {
                Logger.Warn(
                    "The report is not attached because it is in the invalid date range. Campaign duration - {0}, date range - {1}.",
                    campaign.Duration, dateRange);
                return false;
            }

            if (!IsValidCampaignName(account.Filter, campaign))
            {
                Logger.Warn(
                    "The report is not attached because the campaign has an incorrect name. Campaign name - {0}, account filter - {1}.",
                    campaign.Name, account.Filter);
                return false;
            }

            return true;
        }

        private void GetCampaignReportInfo(CampaignInfo campaign)
        {
            pageActions.ClickOnTab(AmazonPdaPageObjects.CampaignTabSet, AmazonPdaPageObjects.CampaignReportsTab);
            pageActions.GetCampaignReportsInfo(campaign, downloadDir, reportNameTemplate);
        }

        private bool IsValidReportDateRange(DateRange dateRange, CampaignInfo campaignInfo)
        {
            var durationDates = campaignInfo.Duration.Split('-');
            if (!DateTime.TryParse(durationDates[0], out var startDate))
            {
                startDate = DateTime.MinValue;
            }

            if (durationDates.Length == 1 || !DateTime.TryParse(durationDates[1], out var endDate))
            {
                endDate = DateTime.MaxValue;
            }

            return startDate <= dateRange.ToDate && endDate >= dateRange.FromDate;
        }

        private bool IsValidCampaignName(string campaignFilter, CampaignInfo campaignInfo)
        {
            return string.IsNullOrEmpty(campaignFilter) || campaignInfo.Name.Contains(campaignFilter);
        }

        private void NavigateToPageCampaignInfo()
        {
            try
            {
                var url = GetAvailableProfileUrl(account.Name);
                pageActions.NavigateToUrl(url);
                pageActions.SetFiltersOnCampaigns();
            }
            catch (Exception e)
            {
                throw new Exception($"Could not navigate to the page with campaigns information: {e.Message}", e);
            }
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
                throw new Exception($"The account {authorizationModel.Login} does not have the following profile - {profileName}");
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

        private List<string> GetCampaignPageUrls()
        {
            Logger.Info(account.Id, "Searching all available campaigns URLs...");
            var campaignUrlList = GetCampaignUrlList();
            Logger.Info(account.Id, "[{0}] campaign URLs received", campaignUrlList.Count);
            return campaignUrlList;
        }

        private List<string> GetCampaignUrlList()
        {
            var campaignAllUrlList = new List<string>();
            try
            {
                AddCampaignUrls(campaignAllUrlList);
            }
            catch (Exception exc)
            {
                Logger.Warn(account.Id, "Failed to get campaign URLs: {0}", exc.Message);
            }

            var realUrls = campaignAllUrlList.FindAll(url => !string.IsNullOrEmpty(url));
            Logger.Info(account.Id, "[{0}] urls has been retrieved from {1} elements", realUrls.Count,
                campaignAllUrlList.Count);
            return realUrls;
        }

        private void AddCampaignUrls(List<string> campaignAllUrlList)
        {
            AddCampaignUrls(campaignAllUrlList, 1);
            for (var i = 2; pageActions.IsElementEnabledAndDisplayed(AmazonPdaPageObjects.NavigateNextPageButton); i++)
            {
                pageActions.NavigateNextCampaignPage();
                AddCampaignUrls(campaignAllUrlList, i);
            }
        }

        private void AddCampaignUrls(List<string> campaignAllUrlList, int pageNumber)
        {
            Logger.Info(account.Id, "Number of the page is {0}", pageNumber);
            try
            {
                var campaignUrls = GetCampaignUrlsOnPage();
                campaignAllUrlList.AddRange(campaignUrls);
            }
            catch (Exception exc)
            {
                Logger.Warn(account.Id, "Page processing is failed: {0}", exc.Message);
            }
        }

        private IEnumerable<string> GetCampaignUrlsOnPage()
        {
            Logger.Info(account.Id, "Retrieving a list of campaign name web elements...");
            var campNameWebElementList = pageActions.GetCampaignsNameWebElementsList(
                AmazonPdaPageObjects.CampaignsNameContainer, AmazonPdaPageObjects.CampaignsNamesList);
            Logger.Info(account.Id, "[{0}] web elements received", campNameWebElementList?.Count ?? 0);
            return campNameWebElementList?.Select(pageActions.GetCampaignUrl) ?? new List<string> {""};
        }
    }
}