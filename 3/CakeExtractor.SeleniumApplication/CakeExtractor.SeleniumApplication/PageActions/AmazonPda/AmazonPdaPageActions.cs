using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter;
using CakeExtractor.SeleniumApplication.Helpers;
using OpenQA.Selenium;
using CakeExtractor.SeleniumApplication.Models;

namespace CakeExtractor.SeleniumApplication.PageActions.AmazonPda
{
    public class AmazonPdaPageActions : BaseAmazonPageActions
    {
        private readonly TimeSpan timeoutThread = TimeSpan.FromSeconds(3);

        public AmazonPdaPageActions(IWebDriver driver, int timeoutMinutes) : base(driver,timeoutMinutes)
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

        public void SetFiltersOnCampaigns()
        {
            try
            {
                if (!IsElementPresent(AmazonPdaPageObjects.FilterResetButton))
                {
                    SelectFilters();

                    WaitLoading(AmazonPdaPageObjects.FilterLoader, timeout);
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Could not to set the filter: {e.Message}", e);
            }
        }

        public void ExportCsv()
        {
            try
            {
                WaitElementClickable(AmazonPdaPageObjects.ExportButton, timeout);
                ClickElement(AmazonPdaPageObjects.ExportButton);
                Wait(timeoutThread);
            }
            catch (Exception e)
            {
                throw new Exception($"Could not download csv export file: {e.Message}", e);
            }
        }

        public System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> GetCampaignsNameWebElementsList(
            By campaignsContainer, By campaignsNamesList)
        {
            return IsElementPresent(campaignsContainer)
                ? GetChildrenElements(campaignsContainer, campaignsNamesList)
                : null;
        }

        public string GetCampaignUrl(IWebElement campaignsName)
        {
            var url = "";
            var campaignNameElement = GetChildElement(campaignsName, AmazonPdaPageObjects.CampaignName);
            var linkElement = GetChildElement(campaignNameElement, AmazonPdaPageObjects.CampaignNameLink);

            if (linkElement != null)
            {
                url = linkElement.GetAttribute("href");
                Logger.Info($"Campaign URL found [{url}]");
            }

            return url;
        }

        public void ClickOnTab(By listElement, By itemElement)
        {
            try
            {
                ClickOnTabItem(listElement, itemElement);
            }
            catch (Exception e)
            {
                throw new Exception($"Could not click on tab [{itemElement}]: {e.Message}", e);
            }
        }

        public void GetCampaignSettingsInfo(CampaignInfo campaign)
        {
            try
            {
                Logger.Info("Retrieving campaign settings...");
                WaitElementClickable(AmazonPdaPageObjects.CampaignSettingsTable, timeout);

                var tableRows = GetTableRows(AmazonPdaPageObjects.CampaignSettingsTable);
                foreach (var row in tableRows)
                {
                    try
                    {
                        var rowTH = row.FindElement(By.TagName("th")).Text;
                        var rowTD = row.FindElement(By.TagName("td")).Text;
                        switch (rowTH)
                        {
                            case "Campaign name":
                                campaign.Name = rowTD;
                                break;
                            case "Campaign ID":
                                campaign.Id = rowTD;
                                break;
                            case "Type":
                                campaign.Type = rowTD;
                                break;
                            case "Status":
                                campaign.Status = rowTD;
                                break;
                            case "Duration":
                                campaign.Duration = rowTD;
                                break;
                            case "Targeting":
                                campaign.Targeting = rowTD;
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.Warn(e.Message);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(
                    $"Could not get the settings information about campaign [{campaign.Name}]: {e.Message}", e);
            }
        }

        public void GetCampaignReportsInfo(CampaignInfo campaign, string downloadPath, string templateFileName)
        {
            try
            {
                Logger.Info("Retrieving campaign report...");

                WaitElementClickable(AmazonPdaPageObjects.DownloadReportButton, timeout);
                ClickElement(AmazonPdaPageObjects.DownloadReportButton);
                WaitLoading(AmazonPdaPageObjects.DownloadingLoader, timeout, true);

                if (IsElementEnabledAndDisplayed(AmazonPdaPageObjects.AfterDownloadReportNoData))
                {
                    Logger.Warn("The report is not attached because the answer is 'No data'");
                    return;
                }

                if (IsElementEnabledAndDisplayed(AmazonPdaPageObjects.AfterDownloadReportFailed))
                {
                    Logger.Warn("The report is not attached because the answer is 'Download failed'");
                    return;
                }

                Wait(timeoutThread);
                AttachDownloadedReport(campaign, downloadPath, templateFileName);
            }
            catch (Exception e)
            {
                throw new Exception($"Could not get reports for campaign [{campaign.Name}]: {e.Message}", e);
            }
        }

        public void NavigateNextCampaignPage()
        {
            Logger.Info("Go to the next page...");
            ClickElement(AmazonPdaPageObjects.NavigateNextPageButton);
            WaitLoading(AmazonPdaPageObjects.FilterLoader, timeout);
        }

        private void AttachDownloadedReport(CampaignInfo campaign, string downloadPath, string templateFileName)
        {
            var files = FileManager.GetFilesFromPath(downloadPath, templateFileName, campaign.Name);
            if (files.Any())
            {
                campaign.ReportPath = files.First();
                Logger.Info("Report is attached: {0}", campaign.ReportPath);
            }
            else
            {
                var exc = new Exception("Report is not attached!");
                Logger.Error(exc);
            }
        }

        private void SelectFilters()
        {
            Logger.Info("Setting the filter...");
            WaitElementClickable(AmazonPdaPageObjects.FilterByButton, timeout);
            ClickElement(AmazonPdaPageObjects.FilterByButton);
            WaitElementClickable(AmazonPdaPageObjects.FilterTypeButton, timeout);
            ClickElement(AmazonPdaPageObjects.FilterTypeButton);
            WaitElementClickable(AmazonPdaPageObjects.FilterByValues, timeout);
            ClickElement(AmazonPdaPageObjects.FilterByValues);
            WaitElementClickable(AmazonPdaPageObjects.FilterPdaValues, timeout);
            ClickElement(AmazonPdaPageObjects.FilterPdaValues);
            ClickElement(AmazonPdaPageObjects.SaveSearchAndFilterButton);
        }
    }
}