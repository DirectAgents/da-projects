using System;
using CakeExtractor.SeleniumApplication.Helpers;
using OpenQA.Selenium;
using CakeExtractor.SeleniumApplication.Models;

namespace CakeExtractor.SeleniumApplication.PageActions.AmazonPda
{
    public class AmazonPdaPageActions : BasePageActions
    {
        private readonly TimeSpan timeout;
        private readonly TimeSpan timeoutThread = TimeSpan.FromSeconds(3);

        public AmazonPdaPageActions(IWebDriver driver, int timeoutMinuts) : base(driver)
        {
            timeout = TimeSpan.FromMinutes(timeoutMinuts);
        }

        public void NavigateToUrl(string url, By waitingElement)
        {
            FileManager.TmpConsoleLog($"Go to URL [{url}]...");
            NavigateToUrl(url, waitingElement, timeout);
            FileManager.TmpConsoleLog("Ok");
        }

        public void LoginProcess(string email, string password)
        {
            try
            {
                FileManager.TmpConsoleLog($"Login with e-mail [{email}]...");
                ClickElement(AmazonPdaPageObjects.LoginEmailInput);
                SendKeys(AmazonPdaPageObjects.LoginEmailInput, email);
                ClickElement(AmazonPdaPageObjects.LoginPassInput);
                SendKeys(AmazonPdaPageObjects.LoginPassInput, password);
                ClickElement(AmazonPdaPageObjects.RememberMeCheckBox);
                ClickElement(AmazonPdaPageObjects.LoginButton);                
                WaitElementClickable(AmazonPdaPageObjects.CodeInput, timeout);
                FileManager.TmpConsoleLog("Waiting the code...");
                WaitElementClickable(AmazonPdaPageObjects.AccountButton, timeout);
                FileManager.TmpConsoleLog("Ok");
            }
            catch (Exception e)
            {
                throw new Exception($"Login failed [{email}]: {e.Message}", e);
            }
        }

        public void LoginByPassword(string password)
        {
            try
            {
                FileManager.TmpConsoleLog("Need to repeat the password...");
                ClickElement(AmazonPdaPageObjects.LoginPassInput);
                SendKeys(AmazonPdaPageObjects.LoginPassInput, password);
                ClickElement(AmazonPdaPageObjects.RememberMeCheckBox);
                ClickElement(AmazonPdaPageObjects.LoginButton);
                if (IsElementPresent(AmazonPdaPageObjects.CodeInput))
                {
                    WaitElementClickable(AmazonPdaPageObjects.CodeInput, timeout);
                    FileManager.TmpConsoleLog("Waiting the code...");
                    WaitElementClickable(AmazonPdaPageObjects.AccountButton, timeout);
                }
                WaitElementClickable(AmazonPdaPageObjects.FilterByButton, timeout);
                FileManager.TmpConsoleLog("Ok");
            }
            catch (Exception e)
            {
                throw new Exception($"Could not to repeat password: {e.Message}", e);
            }
        }

        public void SetFiltersOnCampaigns()
        {
            try
            {
                FileManager.TmpConsoleLog("Setting the filter...");
                ClickElement(AmazonPdaPageObjects.FilterByButton);
                WaitElementClickable(AmazonPdaPageObjects.FilterTypeButton, timeout);
                ClickElement(AmazonPdaPageObjects.FilterTypeButton);
                WaitElementClickable(AmazonPdaPageObjects.FilterByValues, timeout);
                ClickElement(AmazonPdaPageObjects.FilterByValues);
                WaitElementClickable(AmazonPdaPageObjects.FilterPdaValues, timeout);
                ClickElement(AmazonPdaPageObjects.FilterPdaValues);
                ClickElement(AmazonPdaPageObjects.SaveSearchAndFilterButton);

                WaitLoading(AmazonPdaPageObjects.FilterLoader, timeout);
                FileManager.TmpConsoleLog("Ok");
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

        public System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> GetCampaignsNameWebElementsList(By campaignsContainer, By campaignsNamesList)
        {
            return GetChildrenElements(campaignsContainer, campaignsNamesList);
        }

        public string GetCampaignUrl(IWebElement campaignsName)
        {
            var url = "";
            var campaignNameElement = GetChildElement(campaignsName, AmazonPdaPageObjects.CampaignName);            
            var linkElement = GetChildElement(campaignNameElement, AmazonPdaPageObjects.CampaignNameLink);

            if (linkElement != null)
            {
                url = linkElement.GetAttribute("href");
                FileManager.TmpConsoleLog($"Campaign URL found [{url}]");
            }            
            return url;
        }
        
        public void NavigateToTab(By tabElement, By tabContent)
        {
            try
            {
                WaitElementClickable(tabElement, timeout);
                ClickElement(tabElement);
                WaitElementClickable(tabContent, timeout);
            }
            catch (Exception e)
            {
                throw new Exception($"Could not navigate to tab [{tabElement}]: {e.Message}", e);
            }
        }

        public void GetCampaignSettingsInfo(CampaignInfo campaign)
        {
            try
            {
                FileManager.TmpConsoleLog("Retrieving campaign settings...");
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
                        FileManager.TmpConsoleLog($"Warning: {e.Message}");
                    }
                }
                FileManager.TmpConsoleLog("Ok");
            }
            catch (Exception e)
            {
                throw new Exception($"Could not get the settings information about campaign [{campaign.Name}]: {e.Message}", e);
            }
        }

        public void GetCampaignReportsInfo(CampaignInfo campaign, string downloadPath, string templateFileName)
        {
            try
            {
                FileManager.TmpConsoleLog("Retrieving campaign report...");
                
                WaitElementClickable(AmazonPdaPageObjects.DownloadReportButton, timeout);
                ClickElement(AmazonPdaPageObjects.DownloadReportButton);
                WaitLoading(AmazonPdaPageObjects.DownloadingLoader, timeout);
                
                if (IsElementEnabledAndDisplayed(AmazonPdaPageObjects.AfterDownloadReportNoData))
                { 
                    FileManager.TmpConsoleLog("Warning: the report is not attached because the answer is 'No data'");
                }
                
                var files = FileManager.GetFilesFromPath(downloadPath, templateFileName, campaign.Name);
                foreach (var file in files)
                {
                    campaign.ReportPath = file;
                    FileManager.TmpConsoleLog("Ok. Report is attached");
                    break;
                }
                if (string.IsNullOrEmpty(campaign.ReportPath))
                {
                    FileManager.TmpConsoleLog("Warning: Report is not attached");
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Could not get reports for campaign [{campaign.Name}]: {e.Message}", e);
            }
        }

        public void NavigateNextCampaignPage()
        {
            FileManager.TmpConsoleLog("Go to the next page...");
            ClickElement(AmazonPdaPageObjects.NavigateNextPageButton);
            
            WaitLoading(AmazonPdaPageObjects.FilterLoader, timeout);
            FileManager.TmpConsoleLog("Ok");
        }

        public string GetProfileUrl(string profileName)
        {
            string profileUrl = null;
            try
            {
                FileManager.TmpConsoleLog($"Searching the URL of profile [{profileName}]...");
                ClickElement(AmazonPdaPageObjects.CurrentProfileButton);
                WaitElementClickable(AmazonPdaPageObjects.ProfilesMenu, timeout);

                var menuContainers = GetChildrenElements(AmazonPdaPageObjects.ProfilesMenu,
                    AmazonPdaPageObjects.ProfilesMenuItemContainer);
                foreach (var menuContainer in menuContainers)
                {
                    var menuItem = GetChildElement(menuContainer, AmazonPdaPageObjects.ProfilesMenuItem);
                    if (menuItem.Text != profileName) continue;                    
                    profileUrl = menuItem.GetAttribute("href");
                    FileManager.TmpConsoleLog($"Profile URL found [{profileUrl}]");
                    break;
                }
                return profileUrl;
            }
            catch (Exception e)
            {
                throw new Exception($"Could not to get URL of the profile [{profileName}]: {e.Message}", e);
            }            
        }
    }
}
