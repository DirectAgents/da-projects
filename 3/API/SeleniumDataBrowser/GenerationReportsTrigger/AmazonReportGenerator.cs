using System;
using System.Collections.Generic;
using SeleniumDataBrowser.GenerationReportsTrigger.Helpers;
using SeleniumDataBrowser.GenerationReportsTrigger.PageActions;
using SeleniumDataBrowser.Helpers;
using SeleniumDataBrowser.PDA.Helpers;

namespace SeleniumDataBrowser.GenerationReportsTrigger
{
    public class AmazonReportGenerator
    {
        private readonly PdaLoginManager loginProcessManager;
        private readonly GenerationReportsProfileUrlManager profileUrlManager;
        private readonly GenerationReportsActionsWithPagesManager actionsWithPagesManager;
        private readonly SeleniumLogger logger;

        private Dictionary<string, string> reportProfileUrls;

        public AmazonReportGenerator(
            PdaLoginManager loginProcessManager,
            GenerationReportsProfileUrlManager profileUrlManager,
            GenerationReportsActionsWithPagesManager actionsWithPagesManager,
            SeleniumLogger logger)
        {
            this.loginProcessManager = loginProcessManager;
            this.profileUrlManager = profileUrlManager;
            this.actionsWithPagesManager = actionsWithPagesManager;
            this.logger = logger;
        }

        /// <summary>
        /// Login to the Amazon Advertiser Portal.
        /// </summary>
        public void LoginToPortal()
        {
            try
            {
                loginProcessManager.LoginToPortal();
            }
            catch (Exception e)
            {
                throw new Exception("Failed to login to Amazon Advertiser Portal.", e);
            }
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

        public void GenerateReports()
        {
            foreach (var reportProfile in reportProfileUrls)
            {
                actionsWithPagesManager.GenerateSearchTermReport(reportProfile.Value);
            }
        }
    }
}
