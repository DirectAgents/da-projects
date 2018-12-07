using System;
using CakeExtracter;
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
            //driver.Manage().Timeouts().ImplicitWait = timeout; // WARNING
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
                WaitElement(AmazonPdaPageObjects.CodeInput, timeout);
                FileManager.TmpConsoleLog("Waiting the code...");
                WaitElement(AmazonPdaPageObjects.AccountButton, timeout);
                FileManager.TmpConsoleLog("Ok");
            }
            catch (Exception e)
            {
                throw new Exception($"Login failed [{email}]: {e.Message}", e);
            }
        }

        public void SetFiltersOnCampaigns()
        {
            try
            {
                FileManager.TmpConsoleLog("Setting the filter...");
                ClickElement(AmazonPdaPageObjects.FilterByButton);
                WaitElement(AmazonPdaPageObjects.FilterTypeButton, timeout);
                ClickElement(AmazonPdaPageObjects.FilterTypeButton);
                WaitElement(AmazonPdaPageObjects.FilterByValues, timeout);
                ClickElement(AmazonPdaPageObjects.FilterByValues);
                WaitElement(AmazonPdaPageObjects.FilterPdaValues, timeout);
                ClickElement(AmazonPdaPageObjects.FilterPdaValues);
                ClickElement(AmazonPdaPageObjects.SaveSearchAndFilterButton);
                //Wait(timeoutThread);
                WaitElement(AmazonPdaPageObjects.CampaignsContainer, timeout);
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
                WaitElement(AmazonPdaPageObjects.ExportButton, timeout);
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
                WaitElement(tabElement, timeout);
                ClickElement(tabElement);
                WaitElement(tabContent, timeout);
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
                Wait(timeoutThread);
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

                Wait(timeoutThread);
                //ClickElement(AmazonPdaPageObjects.StartDate);
                //SendKeys(AmazonPdaPageObjects.StartDate, "12/07/2018");

                ClickElement(AmazonPdaPageObjects.DownloadReportButton);
                Wait(timeoutThread);
                //FileManager.CheckDirectoryLength(downloadPath);

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
            Wait(timeoutThread);            
            FileManager.TmpConsoleLog("Ok");
        }

        public string GetProfileUrl(string profileName)
        {
            string profileUrl = null;
            try
            {
                FileManager.TmpConsoleLog($"Searching the URL of profile [{profileName}]...");
                ClickElement(AmazonPdaPageObjects.CurrentProfileButton);
                WaitElement(AmazonPdaPageObjects.ProfilesMenu, timeout);

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
