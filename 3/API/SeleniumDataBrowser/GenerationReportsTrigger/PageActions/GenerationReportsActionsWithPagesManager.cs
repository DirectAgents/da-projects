using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using SeleniumDataBrowser.Helpers;
using SeleniumDataBrowser.PDA.PageActions;

namespace SeleniumDataBrowser.GenerationReportsTrigger.PageActions
{
    public class GenerationReportsActionsWithPagesManager : AmazonPdaActionsWithPagesManager
    {
        private const string AdvertisingPortalUrlForCanada = "https://advertising.amazon.ca";
        private const string TextOfSearchTermReportTypeForUS = "Search term";
        private const string TextOfSearchTermReportTypeForCanada = "Search terms";
        private const string TextOfLastMonthTimePeriodForUS = "Last month";
        private const string TextOfLastMonthTimePeriodForCanada = "Last Month";
        private const string TextOfDailyDataUnit = "Daily";
        private const string TextForReportName = "GenerationReportsTrigger_SP_SearchTerm_Report";
        private const string AdvertisingAccountUrlPattern = "https://advertising.amazon.com/home?entityId=";

        public GenerationReportsActionsWithPagesManager(
            int timeoutMinutes, bool isHiddenBrowserWindow, SeleniumLogger logger)
            : base(timeoutMinutes, isHiddenBrowserWindow, logger)
        {
        }

        public List<string> GetDefaultMarketplaceProfileUrls()
        {
            var entities = GetChildrenElements(
                GenerationReportsPageObjects.EntitiesPageMainContainer, GenerationReportsPageObjects.EntitiesPageRow);
            var someAccount = entities.FirstOrDefault(entity =>
                entity.GetAttribute(HrefAttribute).Contains(AdvertisingAccountUrlPattern));
            var accounts = entities.Where(entity =>
                entity.GetAttribute(HrefAttribute).Contains(AdvertisingAccountUrlPattern)).ToList();
            var defaultMarketplaceProfileUrls = accounts.Select(x => x.GetAttribute(HrefAttribute)).ToList();
            GoToSomeAccountHomePage(someAccount);
            return defaultMarketplaceProfileUrls;
        }

        public List<string> GetOtherMarketplaceProfileUrls()
        {
            WaitElementClickable(AmazonPdaPageObjects.CurrentProfileButton);
            MoveToElementAndClick(AmazonPdaPageObjects.CurrentProfileButton);
            WaitElementClickable(GenerationReportsPageObjects.OtherMarketplace);
            ClickElement(GenerationReportsPageObjects.OtherMarketplace);
            WaitElementClickable(AmazonPdaPageObjects.ProfilesMenu);
            var menuItems = GetChildrenElements(AmazonPdaPageObjects.ProfilesMenu, AmazonPdaPageObjects.ProfilesMenuItem);
            var otherMarketplaceProfileUrls = menuItems.Select(x => x.GetAttribute(HrefAttribute)).ToList();
            return otherMarketplaceProfileUrls;
        }

        public void GenerateSearchTermReport()
        {
            SetupSearchTermReportType();
            SetupLastMonthTimePeriod();
            SetupDailyDataUnit();
            SetupReportName();
            WaitElementClickable(GenerationReportsPageObjects.CreateReportButton);
            Logger.LogInfo("Generate report!");
            // ClickElement(GenerationReportsPageObjects.CreateReportButton);
        }

        public bool IsCurrentUrlForCanadaMarketplace()
        {
            var currentUrl = GetCurrentWindowUrl();
            return currentUrl.Contains(AdvertisingPortalUrlForCanada);
        }

        private void GoToSomeAccountHomePage(IWebElement someAccount)
        {
            ClickElement(someAccount);
            WaitElementClickable(AmazonPdaPageObjects.FilterByButton);
        }

        private void SetupSearchTermReportType()
        {
            var textOfSearchTermReportType = IsCurrentUrlForCanadaMarketplace()
                ? TextOfSearchTermReportTypeForCanada
                : TextOfSearchTermReportTypeForUS;
            Logger.LogInfo($"Setup the [{textOfSearchTermReportType}] report type...");
            SetupSpecifiedItemFromDropdownItemList(GetReportTypesFromDropdown, textOfSearchTermReportType);
        }

        private void SetupLastMonthTimePeriod()
        {
            var textOfLastMonthTimePeriod = IsCurrentUrlForCanadaMarketplace()
                ? TextOfLastMonthTimePeriodForCanada
                : TextOfLastMonthTimePeriodForUS;
            Logger.LogInfo($"Setup the [{textOfLastMonthTimePeriod}] time period...");
            SetupSpecifiedItemFromDropdownItemList(GetTimePeriodsFromDropdown, textOfLastMonthTimePeriod);
        }

        private void SetupDailyDataUnit()
        {
            Logger.LogInfo($"Setup the [{TextOfDailyDataUnit}] data unit...");
            SetupSpecifiedItemFromDropdownItemList(GetDataUnitsFromDropdown, TextOfDailyDataUnit);
        }

        private void SetupReportName()
        {
            Logger.LogInfo($"Setup the [{TextForReportName}] report name...");
            SetupTextToInput(GenerationReportsPageObjects.ReportNameInput, TextForReportName);
        }

        private IEnumerable<IWebElement> GetReportTypesFromDropdown()
        {
            return GetItemListFromSpecifiedDropdown(
                GenerationReportsPageObjects.ReportTypeDropdown,
                GenerationReportsPageObjects.ReportTypeItemList,
                GenerationReportsPageObjects.DropdownItem);
        }

        private IEnumerable<IWebElement> GetTimePeriodsFromDropdown()
        {
            return GetItemListFromSpecifiedDropdown(
                GenerationReportsPageObjects.TimePeriodDropdown,
                GenerationReportsPageObjects.TimePeriodItemList,
                GenerationReportsPageObjects.DropdownItem);
        }

        private IEnumerable<IWebElement> GetDataUnitsFromDropdown()
        {
            return GetItemListFromSpecifiedDropdown(
                GenerationReportsPageObjects.DataUnitDropdown,
                GenerationReportsPageObjects.DataUnitItemList,
                GenerationReportsPageObjects.DropdownItem);
        }
    }
}
