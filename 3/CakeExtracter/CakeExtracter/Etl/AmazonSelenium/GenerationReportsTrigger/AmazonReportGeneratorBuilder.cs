using System;
using CakeExtracter.Etl.AmazonSelenium.Configuration;
using CakeExtracter.Etl.AmazonSelenium.PDA.Configuration;
using SeleniumDataBrowser.GenerationReportsTrigger;
using SeleniumDataBrowser.Helpers;
using SeleniumDataBrowser.Models;
using SeleniumDataBrowser.PDA.Helpers;
using SeleniumDataBrowser.PDA.PageActions;

namespace CakeExtracter.Etl.AmazonSelenium.GenerationReportsTrigger
{
    internal class AmazonReportGeneratorBuilder
    {
        private readonly int maxRetryAttempts;
        private readonly TimeSpan pauseBetweenAttempts;
        private readonly PdaCommandConfigurationManager configurationManager;
        private AmazonReportGenerator reportGenerator;
        private AuthorizationModel authorizationModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonReportGeneratorBuilder"/> class.
        /// </summary>
        public AmazonReportGeneratorBuilder(PdaCommandConfigurationManager configurationManager)
        {
            this.configurationManager = configurationManager;
            maxRetryAttempts = this.configurationManager.GetMaxRetryAttempts();
            pauseBetweenAttempts = this.configurationManager.GetPauseBetweenAttempts();
        }

        public AmazonReportGenerator BuildReportGenerator(SeleniumLogger logger, bool isHidingBrowserWindow)
        {
            authorizationModel = GetAuthorizationModel();
            var pageActionsManager = GetPageActionsManager(logger, isHidingBrowserWindow);
            var loginProcessManager = GetLoginProcessManager(pageActionsManager, logger);
            var pdaProfileUrlManager = GetProfileUrlManager(pageActionsManager, loginProcessManager);
            reportGenerator = new AmazonReportGenerator(loginProcessManager, pdaProfileUrlManager, logger);
            reportGenerator.LoginToPortal();
            reportGenerator.SetAvailableProfileUrls();
            return reportGenerator;
        }

        private AuthorizationModel GetAuthorizationModel()
        {
            try
            {
                var emailLogin = configurationManager.GetEMail();
                var emailPassword = configurationManager.GetEMailPassword();
                var cookieDirectoryName = configurationManager.GetCookiesDirectoryName();
                return new AuthorizationModel
                {
                    Login = emailLogin,
                    Password = emailPassword,
                    CookiesDir = cookieDirectoryName,
                };
            }
            catch (Exception e)
            {
                throw new Exception("Failed to initialize authorization settings.", e);
            }
        }

        private AmazonPdaActionsWithPagesManager GetPageActionsManager(SeleniumLogger logger, bool isHidingBrowserWindow)
        {
            try
            {
                var timeoutInMinutes = SeleniumCommandConfigurationManager.GetWaitPageTimeout();
                return new AmazonPdaActionsWithPagesManager(timeoutInMinutes, isHidingBrowserWindow, logger);
            }
            catch (Exception e)
            {
                throw new Exception("Failed to initialize page actions manager.", e);
            }
        }

        private PdaLoginManager GetLoginProcessManager(AmazonPdaActionsWithPagesManager pageActionsManager, SeleniumLogger logger)
        {
            try
            {
                return new PdaLoginManager(authorizationModel, pageActionsManager, logger);
            }
            catch (Exception e)
            {
                throw new Exception("Failed to initialize login process manager.", e);
            }
        }

        private PdaProfileUrlManager GetProfileUrlManager(
            AmazonPdaActionsWithPagesManager pageActionsManager, PdaLoginManager loginProcessManager)
        {
            try
            {
                return new PdaProfileUrlManager(pageActionsManager, loginProcessManager, maxRetryAttempts, pauseBetweenAttempts);
            }
            catch (Exception e)
            {
                throw new Exception("Failed to initialize profile URL manager.", e);
            }
        }
    }
}
