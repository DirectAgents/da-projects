using System;
using System.Collections.Generic;
using System.Linq;
using SeleniumDataBrowser.PageActions;
using SeleniumDataBrowser.Drivers;

namespace SeleniumDataBrowser.PDA.PageActions
{
    public class AmazonPdaPageActions : BaseAmazonPageActions
    {
        public AmazonPdaPageActions(int timeoutMinutes, Action<string> logInfo, Action<string> logError, Action<string> logWarning)
            : base(new ChromeWebDriver(string.Empty), timeoutMinutes, logInfo, logError, logWarning)
        {
        }

        public Dictionary<string, string> GetAvailableProfileUrls()
        {
            WaitElementClickable(AmazonPdaPageObjects.CurrentProfileButton, timeout);
            MoveToElementAndClick(AmazonPdaPageObjects.CurrentProfileButton);
            WaitElementClickable(AmazonPdaPageObjects.ProfilesMenu, timeout);
            var menuContainers = GetChildrenElements(AmazonPdaPageObjects.ProfilesMenu, AmazonPdaPageObjects.ProfilesMenuItemContainer);
            var menuItems = menuContainers.Select(x => GetChildElement(x, AmazonPdaPageObjects.ProfilesMenuItem));
            return menuItems.ToDictionary(x => x.Text.Trim(), x => x.GetAttribute(HrefAttribute));
        }
    }
}