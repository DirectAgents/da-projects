using System;
using System.Collections.Generic;
using System.Linq;
using SeleniumDataBrowser.PageActions;
using SeleniumDataBrowser.Drivers;
using OpenQA.Selenium;

namespace SeleniumDataBrowser.PDA.PageActions
{
    public class AmazonPdaPageActions : BaseAmazonPageActions
    {
        public AmazonPdaPageActions(int timeoutMinutes)
            : base(new ChromeWebDriver(string.Empty), timeoutMinutes)
        {
        }

        public Dictionary<string, string> GetAvailableProfileUrls()
        {
            WaitElementClickable(AmazonPdaPageObjects.CurrentProfileButton, Timeout);
            MoveToElementAndClick(AmazonPdaPageObjects.CurrentProfileButton);
            WaitElementClickable(AmazonPdaPageObjects.ProfilesMenu, Timeout);
            var menuContainers = GetChildrenElements(AmazonPdaPageObjects.ProfilesMenu, AmazonPdaPageObjects.ProfilesMenuItemContainer);
            var menuItems = menuContainers.Select(x => GetChildElement(x, AmazonPdaPageObjects.ProfilesMenuItem));
            return menuItems.ToDictionary(x => x.Text.Trim(), x => x.GetAttribute(HrefAttribute));
        }

        public void LoginByPassword(string password, By waitElement = null)
        {
            LogInfo("Need to repeat the password...");
            try
            {
                LoginWithPassword(password);
                if (waitElement != null)
                {
                    WaitElementClickable(waitElement, Timeout);
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Could not to repeat password: {e.Message}", e);
            }
        }
    }
}