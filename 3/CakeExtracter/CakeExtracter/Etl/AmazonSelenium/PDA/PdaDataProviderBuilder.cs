﻿using System;
using System.Collections.Generic;
using CakeExtracter.Etl.AmazonSelenium.Configuration;
using CakeExtracter.Etl.AmazonSelenium.PDA.Configuration;
using SeleniumDataBrowser.Helpers;
using SeleniumDataBrowser.Models;
using SeleniumDataBrowser.PDA;
using SeleniumDataBrowser.PDA.Helpers;
using SeleniumDataBrowser.PDA.PageActions;

namespace CakeExtracter.Etl.AmazonSelenium.PDA
{
    /// <summary>
    /// Builder of the PDA Data Provider.
    /// </summary>
    internal class PdaDataProviderBuilder
    {
        private readonly bool isHidingBrowserWindow;
        private readonly int maxRetryAttempts;
        private readonly TimeSpan pauseBetweenAttempts;
        private AuthorizationModel authorizationModel;
        private PdaDataProvider pdaDataProvider;
        private Dictionary<string, string> availableProfileUrls;

        /// <summary>
        /// Initializes a new instance of the <see cref="PdaDataProviderBuilder"/> class.
        /// </summary>
        /// <param name="isHidingBrowserWindow">Include hiding the browser window.</param>
        public PdaDataProviderBuilder(bool isHidingBrowserWindow)
        {
            this.isHidingBrowserWindow = isHidingBrowserWindow;
            maxRetryAttempts = PdaCommandConfigurationManager.GetMaxRetryAttempts();
            pauseBetweenAttempts = PdaCommandConfigurationManager.GetPauseBetweenAttempts();
        }

        /// <summary>
        /// Builds and gets ready instance of the PDA Data Provider.
        /// </summary>
        /// <param name="loggerWithoutAccountId">Logger without info about account ID.</param>
        /// <returns>Instance of the PDA Data Provider.</returns>
        public PdaDataProvider BuildDataProvider(SeleniumLogger loggerWithoutAccountId)
        {
            authorizationModel = GetAuthorizationModel();
            var pageActionsManager = GetPageActionsManager(loggerWithoutAccountId);
            var loginProcessManager = GetLoginProcessManager(pageActionsManager, loggerWithoutAccountId);
            var pdaProfileUrlManager = GetProfileUrlManager(pageActionsManager, loginProcessManager);
            pdaDataProvider = PdaDataProvider.GetPdaDataProviderInstance(loginProcessManager, pdaProfileUrlManager);
            pdaDataProvider.LoginToPortal();
            pdaDataProvider.SetCookiesForDataProvider(pageActionsManager);
            SetAvailableProfileUrls();
            return pdaDataProvider;
        }

        /// <summary>
        /// Creates the Amazon PDA Utility for the current account and sets it for the PDA data provider instance.
        /// </summary>
        /// <param name="accountName">Name of account.</param>
        /// <param name="loggerWithAccountId">Logger with info about account ID.</param>
        public void InitializeAmazonPdaUtility(string accountName, SeleniumLogger loggerWithAccountId)
        {
            try
            {
                var amazonPdaUtility = new AmazonConsoleManagerUtility(
                    accountName,
                    authorizationModel,
                    availableProfileUrls,
                    maxRetryAttempts,
                    pauseBetweenAttempts,
                    loggerWithAccountId);
                pdaDataProvider.SetAmazonPdaUtilityCurrentForDataProvider(amazonPdaUtility);
            }
            catch (Exception e)
            {
                throw new Exception("Failed to create Amazon PDA Utility.", e);
            }
        }

        private void SetAvailableProfileUrls()
        {
            availableProfileUrls = pdaDataProvider.GetAvailableProfileUrls();
            Logger.Info("The following profiles were found for the current account:");
            foreach (var availableProfileUrl in availableProfileUrls)
            {
                Logger.Info($"{availableProfileUrl.Key} - {availableProfileUrl.Value}");
            }
        }

        private AuthorizationModel GetAuthorizationModel()
        {
            try
            {
                var emailLogin = PdaCommandConfigurationManager.GetEMail();
                var emailPassword = PdaCommandConfigurationManager.GetEMailPassword();
                var cookieDirectoryName = PdaCommandConfigurationManager.GetCookiesDirectoryName();
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

        private AmazonPdaActionsWithPagesManager GetPageActionsManager(SeleniumLogger loggerWithoutAccountId)
        {
            try
            {
                var timeoutInMinutes = SeleniumCommandConfigurationManager.GetWaitPageTimeout();
                return new AmazonPdaActionsWithPagesManager(
                    timeoutInMinutes,
                    isHidingBrowserWindow,
                    loggerWithoutAccountId);
            }
            catch (Exception e)
            {
                throw new Exception("Failed to initialize page actions manager.", e);
            }
        }

        private PdaLoginManager GetLoginProcessManager(
            AmazonPdaActionsWithPagesManager pageActionsManager, SeleniumLogger loggerWithoutAccountId)
        {
            try
            {
                return new PdaLoginManager(authorizationModel, pageActionsManager, loggerWithoutAccountId);
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