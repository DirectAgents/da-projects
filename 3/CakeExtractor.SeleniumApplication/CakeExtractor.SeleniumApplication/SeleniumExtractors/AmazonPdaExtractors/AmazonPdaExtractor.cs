using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.Enums;
using Amazon.Helpers;
using CakeExtracter;
using CakeExtracter.Common;
using CakeExtractor.SeleniumApplication.Drivers;
using CakeExtractor.SeleniumApplication.Helpers;
using CakeExtractor.SeleniumApplication.Models;
using CakeExtractor.SeleniumApplication.PageActions.AmazonPda;
using DirectAgents.Domain.Entities.CPProg;
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

        private readonly ExtAccount account;

        static AmazonPdaExtractor()
        {
            Initialize();
        }

        public AmazonPdaExtractor(ExtAccount account)
        {
            this.account = account;
        }

        public static void PrepareExtractor()
        {
            authorizationModel.Cookies = CookieManager.GetCookiesFromFiles(authorizationModel.CookiesDir);
            var cookiesExist = authorizationModel.Cookies.Any();
            Logger.Info("Login into the portal{0} using cookies.", cookiesExist ? string.Empty : " without");

            if (cookiesExist)
            {
                LoginWithCookie(authorizationModel);
                return;
            }

            LoginWithoutCookie(authorizationModel);
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

        public CampaignInfo ExtractCampaignInfo(string campaignUrl, DateRange dateRange)
        {
            var campaign = new CampaignInfo();
            pageActions.NavigateToUrl(campaignUrl, AmazonPdaPageObjects.CampaignTabContainer);
            GetCampaignSettingsInfo(campaign);
            campaign.Type = AmazonApiHelper.GetCampaignTypeName(CampaignType.ProductDisplay);
            if (IsCampaignValid(campaign, dateRange))
            {
                GetCampaignReportInfo(campaign);
            }
            return campaign;
        }

        private void GetCampaignSettingsInfo(CampaignInfo campaign)
        {
            pageActions.NavigateToTab(AmazonPdaPageObjects.CampaignSettingsTab, AmazonPdaPageObjects.CampaignSettingsContent);
            pageActions.GetCampaignSettingsInfo(campaign);
        }

        private bool IsCampaignValid(CampaignInfo campaign, DateRange dateRange)
        {
            if (!IsValidReportDateRange(dateRange, campaign))
            {
                Logger.Warn("The report is not attached because it is in the invalid date range. Campaign duration - {0}, date range - {1}.",
                    campaign.Duration, dateRange);
                return false;
            }

            if (!IsValidCampaignName(account.Filter, campaign))
            {
                Logger.Warn("The report is not attached because the campaign has an incorrect name. Campaign name - {0}, account filter - {1}.",
                    campaign.Name, account.Filter);
                return false;
            }

            return true;
        }

        private void GetCampaignReportInfo(CampaignInfo campaign)
        {
            pageActions.NavigateToTab(AmazonPdaPageObjects.CampaignReportsTab, AmazonPdaPageObjects.CampaignReportsContent);
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
                NavigateToProfile();
                if (!pageActions.IsElementPresent(AmazonPdaPageObjects.FilterByButton) && pageActions.IsElementPresent(AmazonPdaPageObjects.LoginPassInput))
                {
                    // need to repeat the password
                    pageActions.LoginByPassword(authorizationModel.Password);
                }
                pageActions.SetFiltersOnCampaigns();
            }
            catch (Exception e)
            {
                throw new Exception($"Could not navigate to the page with campaigns information: {e.Message}", e);
            }
        }

        private void NavigateToProfile()
        {
            pageActions.NavigateToUrl(campaignsUrl, AmazonPdaPageObjects.FilterByButton);
            var url = pageActions.GetProfileUrl(account.Name);
            if (string.IsNullOrEmpty(url))
            {
                throw new Exception($"The account {authorizationModel.Login} does not have the following profile - {account.Name}");
            }
            pageActions.NavigateToUrl(url);
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
            Logger.Info(account.Id, "[{0}] urls has been retrieved from {1} elements", realUrls.Count, campaignAllUrlList.Count);
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
            var campaignUrls = campNameWebElementList.Select(pageActions.GetCampaignUrl);
            Logger.Info(account.Id, "[{0}] web elements received", campNameWebElementList.Count);
            return campaignUrls;
        }
    }
}