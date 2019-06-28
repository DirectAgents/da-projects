using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using SeleniumDataBrowser.PDA.Helpers;
using SeleniumDataBrowser.PDA.Models;
using SeleniumDataBrowser.PDA.PageActions;

namespace SeleniumDataBrowser.PDA
{
    /// <summary>
    /// Data provider for Selenium Product Display Ads.
    /// </summary>
    public class PdaDataProvider
    {
        private static PdaDataProvider pdaDataProviderInstance;

        private readonly PdaProfileUrlManager pdaProfileUrlManager;
        private readonly PdaLoginManager loginProcessManager;
        private IEnumerable<Cookie> cookies;
        private AmazonConsoleManagerUtility currentAmazonPdaUtility;

        private PdaDataProvider(
            PdaLoginManager loginProcessManager,
            PdaProfileUrlManager pdaProfileUrlManager)
        {
            this.loginProcessManager = loginProcessManager;
            this.pdaProfileUrlManager = pdaProfileUrlManager;
        }

        /// <summary>
        /// Gets single instance (new or current) of PDA data provider.
        /// </summary>
        /// <param name="loginProcessManager">Manager of login process.</param>
        /// <param name="pdaProfileUrlManager">Manager of profile URLs.</param>
        /// <returns>Instance (new or current) of PDA data provider.</returns>
        public static PdaDataProvider GetPdaDataProviderInstance(
            PdaLoginManager loginProcessManager, PdaProfileUrlManager pdaProfileUrlManager)
        {
            if (pdaDataProviderInstance == null)
            {
                pdaDataProviderInstance = new PdaDataProvider(loginProcessManager, pdaProfileUrlManager);
            }
            return pdaDataProviderInstance;
        }

        /// <summary>
        /// Sets the specified amazon PDA utility current for the PDA data provider.
        /// </summary>
        /// <param name="amazonPdaUtility">Amazon PDA utility.</param>
        public void SetAmazonPdaUtilityCurrentForDataProvider(
            AmazonConsoleManagerUtility amazonPdaUtility)
        {
            currentAmazonPdaUtility = amazonPdaUtility;
            currentAmazonPdaUtility.SetCookiesForUtility(cookies);
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
        /// Returns summaries of PDA campaigns for specified dates.
        /// </summary>
        /// <param name="extractionDates">Dates for which summaries will be extracted.</param>
        /// <returns>Summaries of PDA campaigns.</returns>
        public IEnumerable<AmazonCmApiCampaignSummary> GetPdaCampaignsSummaries(
            IEnumerable<DateTime> extractionDates)
        {
            return currentAmazonPdaUtility.GetPdaCampaignsSummaries(extractionDates);
        }

        /// <summary>
        /// Gets the URLs for the available campaign profiles on main page of the portal.
        /// </summary>
        /// <returns>Dictionary of available profile URLs.</returns>
        public Dictionary<string, string> GetAvailableProfileUrls()
        {
            try
            {
                return pdaProfileUrlManager.GetAvailableProfileUrls();
            }
            catch (Exception e)
            {
                throw new Exception("Failed to set available profile URLs.", e);
            }
        }

        /// <summary>
        /// Sets current cookies that uses web driver for PDA data provider.
        /// </summary>
        /// <param name="pageActionsManager">Manager of actions with web pages.</param>
        public void SetCookiesForDataProvider(AmazonPdaActionsWithPagesManager pageActionsManager)
        {
            cookies = pageActionsManager.GetAllCookies();
        }
    }
}