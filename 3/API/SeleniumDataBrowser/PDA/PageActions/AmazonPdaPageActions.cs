using System.Collections.Generic;
using System.Linq;
using SeleniumDataBrowser.PageActions;
using SeleniumDataBrowser.Drivers;

namespace SeleniumDataBrowser.PDA.PageActions
{
    /// <inheritdoc cref="BaseAmazonPageActions"/>
    /// <summary>
    /// Class for page actions of Amazon Advertiser Portal for Product Display Ads.
    /// </summary>
    public class AmazonPdaPageActions : BaseAmazonPageActions
    {
        private const string HrefAttribute = "href";

        /// <inheritdoc cref="BaseAmazonPageActions"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonPdaPageActions" /> class.
        /// </summary>
        /// <param name="timeoutMinutes">Number of minutes for waiting of elements.</param>
        /// <param name="isHiddenBrowserWindow">Include hiding the browser window.</param>
        public AmazonPdaPageActions(int timeoutMinutes, bool isHiddenBrowserWindow)
            : base(new ChromeWebDriver(string.Empty, isHiddenBrowserWindow), timeoutMinutes)
        {
        }

        /// <summary>
        /// Retrieves URLs of profiles which available in profile menu on the page.
        /// </summary>
        /// <returns>Dictionary of profile URLs which available in profile menu on the page.</returns>
        public Dictionary<string, string> GetAvailableProfileUrls()
        {
            WaitElementClickable(AmazonPdaPageObjects.CurrentProfileButton, Timeout);
            MoveToElementAndClick(AmazonPdaPageObjects.CurrentProfileButton);
            WaitElementClickable(AmazonPdaPageObjects.ProfilesMenu, Timeout);
            var menuContainers = GetChildrenElements(AmazonPdaPageObjects.ProfilesMenu, AmazonPdaPageObjects.ProfilesMenuItemContainer);
            var menuItems = menuContainers.Select(x => GetChildElement(x, AmazonPdaPageObjects.ProfilesMenuItem));
            return menuItems.ToDictionary(x => x.Text.Trim(), x => x.GetAttribute(HrefAttribute));
        }
    }
}