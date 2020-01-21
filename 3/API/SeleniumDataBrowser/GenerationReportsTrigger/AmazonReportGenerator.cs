using System;
using System.Collections.Generic;
using SeleniumDataBrowser.Helpers;
using SeleniumDataBrowser.PDA.Helpers;

namespace SeleniumDataBrowser.GenerationReportsTrigger
{
    public class AmazonReportGenerator //: ISeleniumProvider
    {
        private readonly PdaLoginManager loginProcessManager;
        private readonly PdaProfileUrlManager profileUrlManager;
        private readonly SeleniumLogger logger;

        private Dictionary<string, string> reportProfileUrls;

        public AmazonReportGenerator(
            PdaLoginManager loginProcessManager,
            PdaProfileUrlManager profileUrlManager,
            SeleniumLogger logger)
        {
            this.loginProcessManager = loginProcessManager;
            this.profileUrlManager = profileUrlManager;
            this.logger = logger;
        }

        /// <summary>
        /// Login to the Amazon Advertiser Portal.
        /// </summary>
        public void LoginToPortal()
        {
            loginProcessManager.LoginToPortal();
        }

        /// <summary>
        /// Sets the URLs for the available profiles to the "Reports" page of the Advertising Portal.
        /// </summary>
        public void SetAvailableProfileUrls()
        {
            try
            {
                reportProfileUrls = profileUrlManager.GetAvailableReportsProfileUrls();
                logger.LogInfo("The following profiles were found for the current account:");
                foreach (var reportProfileUrl in reportProfileUrls)
                {
                    logger.LogInfo($"{reportProfileUrl.Key} - {reportProfileUrl.Value}");
                }
            }
            catch (Exception e)
            {
                throw new Exception("Failed to set available profile URLs.", e);
            }
        }

        ///// <summary>
        ///// Sets current cookies that uses web driver for PDA data provider.
        ///// </summary>
        ///// <param name="pageActionsManager">Manager of actions with web pages.</param>
        //public void SetCookiesForDataProvider(AmazonPdaActionsWithPagesManager pageActionsManager)
        //{
        //    cookies = pageActionsManager.GetAllCookies();
        //}

        public void GenerateReports()
        {
            
        }
    }
}
