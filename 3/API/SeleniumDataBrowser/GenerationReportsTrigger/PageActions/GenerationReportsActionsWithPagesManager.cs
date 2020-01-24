using System.Collections.Generic;
using OpenQA.Selenium;
using SeleniumDataBrowser.Helpers;
using SeleniumDataBrowser.PDA.PageActions;

namespace SeleniumDataBrowser.GenerationReportsTrigger.PageActions
{
    public class GenerationReportsActionsWithPagesManager : AmazonPdaActionsWithPagesManager
    {
        public GenerationReportsActionsWithPagesManager(
            int timeoutMinutes, bool isHiddenBrowserWindow, SeleniumLogger logger)
            : base(timeoutMinutes, isHiddenBrowserWindow, logger)
        {
        }

        public void GenerateSearchTermReport(string reportProfileUrl)
        {
            NavigateToUrl(reportProfileUrl, GenerationReportsPageObjects.ReportsDashboard);
            SetupReportName();
            SetupSearchTermReportType();
            SetupLastMonthTimePeriod();
            SetupDailyDataUnit();
            ClickElement(GenerationReportsPageObjects.CreateReportButton);
        }

        private void SetupSearchTermReportType()
        {
            const string textOfSearchTermReportType = "Search term";
            SetupSpecifiedItemFromDropdownItemList(GetReportTypesFromDropdown, textOfSearchTermReportType);
        }

        private void SetupLastMonthTimePeriod()
        {
            const string textOfLastMonthTimePeriod = "Last month";
            SetupSpecifiedItemFromDropdownItemList(GetTimePeriodsFromDropdown, textOfLastMonthTimePeriod);
        }

        private void SetupDailyDataUnit()
        {
            const string textOfDailyDataUnit = "Daily";
            SetupSpecifiedItemFromDropdownItemList(GetDataUnitsFromDropdown, textOfDailyDataUnit);
        }

        private void SetupReportName()
        {
            const string reportName = "GenerationReportsTrigger_SP_SearchTerm_Report";
            SetupTextToInput(GenerationReportsPageObjects.ReportNameInput, reportName);
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
