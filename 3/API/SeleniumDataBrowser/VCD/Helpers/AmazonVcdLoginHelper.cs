using System;
using System.Linq;
using SeleniumDataBrowser.PageActions;
using SeleniumDataBrowser.Helpers;
using SeleniumDataBrowser.Models;
using SeleniumDataBrowser.VCD.PageActions;

namespace SeleniumDataBrowser.VCD.Helpers
{
    /// <summary>
    /// Helper for VCD login process.
    /// </summary>
    public class AmazonVcdLoginHelper
    {
        private const string SignInPageUrl = "https://www.amazon.com/ap/signin";

        private readonly AuthorizationModel authorizationModel;
        private readonly AmazonVcdPageActions pageActionManager;
        private readonly SeleniumLogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonVcdLoginHelper"/> class.
        /// </summary>
        /// <param name="authorizationModel">Authorization settings.</param>
        /// <param name="pageActionManager">Manager of page actions.</param>
        /// <param name="logger">Selenium data browser logger.</param>
        public AmazonVcdLoginHelper(
            AuthorizationModel authorizationModel,
            AmazonVcdPageActions pageActionManager,
            SeleniumLogger logger)
        {
            this.authorizationModel = authorizationModel;
            this.pageActionManager = pageActionManager;
            this.logger = logger;
        }

        /// <summary>
        /// Specifies whether a password should be repeat.
        /// </summary>
        /// <param name="pageActions">Manager of page actions.</param>
        /// <returns>True / False.</returns>
        public static bool NeedRepeatPassword(BaseAmazonPageActions pageActions)
        {
            var currentUrl = pageActions.GetCurrentWindowUrl();
            var isCurrentUrlContainSignInUrl = currentUrl.Contains(SignInPageUrl);
            var isPasswordTextBoxExistOnCurrentPage =
                pageActions.IsElementPresent(AmazonLoginPageObjects.LoginPassInput);
            return isCurrentUrlContainSignInUrl && isPasswordTextBoxExistOnCurrentPage;
        }

        /// <summary>
        /// Repeats login with password.
        /// </summary>
        /// <param name="pageActions">Manager of page actions.</param>
        /// <param name="authModel">Authorization settings.</param>
        public static void RepeatPassword(BaseAmazonPageActions pageActions, AuthorizationModel authModel)
        {
            pageActions.LoginWithPassword(authModel.Password);
        }

        /// <summary>
        /// Login to the Amazon Advertiser Portal and saving cookies.
        /// </summary>
        public void LoginToAmazonPortal()
        {
            pageActionManager.NavigateToSalesDiagnosticPage();
            if (IsLoginProcessNeeded(authorizationModel, pageActionManager))
            {
                Login(authorizationModel, pageActionManager);
            }
        }

        private static bool IsLoginProcessNeeded(AuthorizationModel authorizationModel, AmazonVcdPageActions pageManager)
        {
            var currentUrl = pageManager.GetCurrentWindowUrl();
            return currentUrl.IndexOf(authorizationModel.SignInUrl, StringComparison.Ordinal) <= 0;
        }

        private static void LoginWithoutCookie(AuthorizationModel authModel, BaseAmazonPageActions pageManager)
        {
            pageManager.NavigateToUrl(authModel.SignInUrl, AmazonLoginPageObjects.ForgotPassLink);
            pageManager.LoginProcess(authModel.Login, authModel.Password);
            var cookies = pageManager.GetAllCookies();
            CookieManager.SaveCookiesToFiles(cookies, authModel.CookiesDir);
        }

        private static void LoginWithCookie(AuthorizationModel authModel, BaseAmazonPageActions pageManager)
        {
            pageManager.NavigateToUrl(authModel.SignInUrl);
            foreach (var cookie in authModel.Cookies)
            {
                pageManager.SetCookie(cookie);
            }
        }

        private void Login(AuthorizationModel authorizationModel, AmazonVcdPageActions pageManager)
        {
            authorizationModel.Cookies = CookieManager.GetCookiesFromFiles(authorizationModel.CookiesDir);
            var cookiesExist = authorizationModel.Cookies.Any();
            logger.LogInfo($"Login into the portal{(cookiesExist ? string.Empty : " without")} using cookies.");
            if (cookiesExist)
            {
                LoginWithCookie(authorizationModel, pageManager);
            }
            else
            {
                logger.LogWarning("Login into the portal without using cookies. Please choose 'Amazon DSP console (formerly Amazon Advertising Platform)' and enter an authorization code!");
                LoginWithoutCookie(authorizationModel, pageManager);
            }
            pageManager.RefreshSalesDiagnosticPage(authorizationModel);
        }

    }
}
