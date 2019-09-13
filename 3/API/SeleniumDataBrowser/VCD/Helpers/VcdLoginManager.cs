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
    public class VcdLoginManager
    {
        private const string SignInPageUrl = "https://vendorcentral.amazon.com/gp/vendor/sign-in";

        private readonly AuthorizationModel authorizationModel;
        private readonly AmazonVcdActionsWithPagesManager pageActionManager;
        private readonly SeleniumLogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="VcdLoginManager"/> class.
        /// </summary>
        /// <param name="authorizationModel">Authorization settings.</param>
        /// <param name="pageActionManager">Manager of page actions.</param>
        /// <param name="logger">Selenium data browser logger.</param>
        public VcdLoginManager(
            AuthorizationModel authorizationModel,
            AmazonVcdActionsWithPagesManager pageActionManager,
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
        public static bool NeedRepeatPassword(AmazonLoginActionsWithPagesManager pageActions)
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
        public static void RepeatPassword(
            AmazonLoginActionsWithPagesManager pageActions, AuthorizationModel authModel)
        {
            pageActions.LoginWithPassword(authModel.Password);
        }

        /// <summary>
        /// Login to the Amazon Advertiser Portal and saving cookies.
        /// </summary>
        public void LoginToAmazonPortal()
        {
            pageActionManager.NavigateToSalesDiagnosticPage();
            if (IsLoginProcessNeeded())
            {
                Login();
            }
        }

        private bool IsLoginProcessNeeded()
        {
            var currentUrl = pageActionManager.GetCurrentWindowUrl();
            return currentUrl.IndexOf(authorizationModel.SignInUrl, StringComparison.Ordinal) <= 0;
        }

        private void LoginWithoutCookie()
        {
            pageActionManager.NavigateToUrl(authorizationModel.SignInUrl, AmazonLoginPageObjects.ForgotPassLink);
            pageActionManager.LoginProcess(authorizationModel.Login, authorizationModel.Password);
            var cookies = pageActionManager.GetAllCookies();
            SeleniumCookieHelper.SaveCookiesToFiles(cookies, authorizationModel.CookiesDir);
        }

        private void LoginWithCookie()
        {
            pageActionManager.NavigateToUrl(authorizationModel.SignInUrl);
            foreach (var cookie in authorizationModel.Cookies)
            {
                pageActionManager.SetCookie(cookie);
            }
        }

        private void Login()
        {
            authorizationModel.Cookies = SeleniumCookieHelper.GetCookiesFromFiles(authorizationModel.CookiesDir);
            var cookiesExist = authorizationModel.Cookies.Any();
            logger.LogInfo($"Login into the portal{(cookiesExist ? string.Empty : " without")} using cookies.");
            if (cookiesExist)
            {
                LoginWithCookie();
            }
            else
            {
                logger.LogWarning("Login into the portal without using cookies. Please, enter an authorization code!");
                LoginWithoutCookie();
            }
            pageActionManager.RefreshSalesDiagnosticPage(authorizationModel);
        }
    }
}