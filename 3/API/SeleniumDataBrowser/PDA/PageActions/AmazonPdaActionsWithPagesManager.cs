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
        protected const string HrefAttribute = "href";

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
            return menuItems.ToDictionary(x => x.Text.Trim(), x => x.GetAttribute(HrefAttribute));
        }
    }
}