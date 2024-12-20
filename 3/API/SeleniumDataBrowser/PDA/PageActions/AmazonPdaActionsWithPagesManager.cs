﻿using System;
using System.Collections.Generic;
using System.Linq;
using SeleniumDataBrowser.PageActions;
using SeleniumDataBrowser.Drivers;
using SeleniumDataBrowser.Helpers;

namespace SeleniumDataBrowser.PDA.PageActions
{
    /// <inheritdoc cref="AmazonLoginActionsWithPagesManager"/>
    /// <summary>
    /// Class for managing actions with web-pages of Amazon Advertiser Portal for Product Display Ads.
    /// </summary>
    public class AmazonPdaActionsWithPagesManager : AmazonLoginActionsWithPagesManager
    {
        private const string HrefAttribute = "href";

        /// <inheritdoc cref="AmazonLoginActionsWithPagesManager"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonPdaActionsWithPagesManager" /> class.
        /// </summary>
        /// <param name="timeoutMinutes">Number of minutes for waiting of elements.</param>
        /// <param name="isHiddenBrowserWindow">Include hiding the browser window.</param>
        /// /// <param name="logger">Selenium data browser logger.</param>
        public AmazonPdaActionsWithPagesManager(int timeoutMinutes, bool isHiddenBrowserWindow, SeleniumLogger logger)
            : base(new ChromeWebDriver(string.Empty, isHiddenBrowserWindow), timeoutMinutes, logger)
        {
        }

        /// <summary>
        /// Retrieves URLs of profiles which available in profile menu on the page.
        /// </summary>
        /// <returns>Dictionary of profile URLs which available in profile menu on the page.</returns>
        public Dictionary<string, string> GetAvailableProfileUrls()
        {
            WaitElementClickable(AmazonPdaPageObjects.CurrentProfileButton);
            MoveToElementAndClick(AmazonPdaPageObjects.CurrentProfileButton);
            WaitElementClickable(AmazonPdaPageObjects.ProfilesMenu);
            var menuItems = GetChildrenElements(AmazonPdaPageObjects.ProfilesMenu, AmazonPdaPageObjects.ProfilesMenuItem);
            return menuItems.ToDictionary(x => GetProfileName(x.Text), x => x.GetAttribute(HrefAttribute));
        }

        private string GetProfileName(string unchangedMenuItemText)
        {
            const int startIndex = 0;
            var menuItemText = unchangedMenuItemText.Trim();
            var newLineIndex = menuItemText.IndexOf("\r\n", StringComparison.Ordinal);
            return newLineIndex < 0
                ? menuItemText
                : menuItemText.Substring(startIndex, newLineIndex);
        }
    }
}