using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Etl.AmazonSelenium.Drivers;
using CakeExtracter.Etl.AmazonSelenium.PageActions;

namespace CakeExtracter.Etl.AmazonSelenium.PDA.PageActions
{
    public class AmazonPdaPageActions : BaseAmazonPageActions
    {
        public AmazonPdaPageActions(int timeoutMinutes)
            : base(new ChromeWebDriver(string.Empty), timeoutMinutes)
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