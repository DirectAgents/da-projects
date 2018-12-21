using System;
using System.Linq;
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

        public AmazonPdaPageActions(IWebDriver driver, int timeoutMinutes) : base(driver)
        {
            timeout = TimeSpan.FromMinutes(timeoutMinutes);
        }

        public void NavigateToUrl(string url, By waitingElement)
        {
            Logger.Info("Go to URL [{0}]...", url);
            NavigateToUrl(url, waitingElement, timeout);
        }

        public void LoginProcess(string email, string password)
        {
            Logger.Info("Login with e-mail [{0}]...", email);
            try
            {
                LoginWithEmailAndPassword(email, password);
            }
            catch (Exception e)
            {
                throw new Exception($"Login failed [{email}]: {e.Message}", e);
            }
        }

        public void LoginByPassword(string password)
        {
            Logger.Info("Need to repeat the password...");
            try
            {
                LoginWithPassword(password);
                WaitElementClickable(AmazonPdaPageObjects.FilterByButton, timeout);
            }
            catch (Exception e)
            {
                throw new Exception($"Could not to repeat password: {e.Message}", e);
            }
        }

        public string GetProfileUrl(string profileName)
        {
            string profileUrl = null;
            Logger.Info("Searching the URL of profile [{0}]...", profileName);
            try
            {
                ClickElement(AmazonPdaPageObjects.CurrentProfileButton);
                WaitElementClickable(AmazonPdaPageObjects.ProfilesMenu, timeout);
                var menuContainers = GetChildrenElements(AmazonPdaPageObjects.ProfilesMenu, AmazonPdaPageObjects.ProfilesMenuItemContainer);
                foreach (var menuContainer in menuContainers)
                {
                    var menuItem = GetChildElement(menuContainer, AmazonPdaPageObjects.ProfilesMenuItem);
                    if (string.Equals(menuItem.Text, profileName, StringComparison.OrdinalIgnoreCase))
                    {
                        profileUrl = menuItem.GetAttribute("href");
                        Logger.Info("Profile URL found [{0}]", profileUrl);
                        break;
                    }
                }
                return profileUrl;
            }
            catch (Exception e)
            {
                throw new Exception($"Could not to get URL of the profile [{profileName}]: {e.Message}", e);
            }
        }

        public void SetFiltersOnCampaigns()
        {
            try
            {
                if (!IsElementVisible(AmazonPdaPageObjects.FilterResetButton))
                {
                    SelectFilters();
                }
                WaitLoading(AmazonPdaPageObjects.FilterLoader, timeout);
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
                Logger.Info($"Campaign URL found [{url}]");
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
                throw new Exception($"Could not get the settings information about campaign [{campaign.Name}]: {e.Message}", e);
            }
        }

        public void GetCampaignReportsInfo(CampaignInfo campaign, string downloadPath, string templateFileName)
        {
            try
            {
                Logger.Info("Retrieving campaign report...");
                
                WaitElementClickable(AmazonPdaPageObjects.DownloadReportButton, timeout);
                ClickElement(AmazonPdaPageObjects.DownloadReportButton);
                WaitLoading(AmazonPdaPageObjects.DownloadingLoader, timeout);
                
                if (IsElementEnabledAndDisplayed(AmazonPdaPageObjects.AfterDownloadReportNoData))
                {
                    Logger.Warn("The report is not attached because the answer is 'No data'");
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
            ClickElement(AmazonPdaPageObjects.FilterByButton);
            WaitElementClickable(AmazonPdaPageObjects.FilterTypeButton, timeout);
            ClickElement(AmazonPdaPageObjects.FilterTypeButton);
            WaitElementClickable(AmazonPdaPageObjects.FilterByValues, timeout);
            ClickElement(AmazonPdaPageObjects.FilterByValues);
            WaitElementClickable(AmazonPdaPageObjects.FilterPdaValues, timeout);
            ClickElement(AmazonPdaPageObjects.FilterPdaValues);
            ClickElement(AmazonPdaPageObjects.SaveSearchAndFilterButton);
        }
            
        private void LoginWithEmailAndPassword(string email, string password)
        {
            EnterEmail(email);
            LoginWithPassword(password);
        }

        private void LoginWithPassword(string password)
        {
            EnterPassword(password);
            ClickElement(AmazonPdaPageObjects.RememberMeCheckBox);
            ClickElement(AmazonPdaPageObjects.LoginButton);
            WaitSecurityCodeIfNecessary();
        }

        private void EnterEmail(string email)
        {
            ClickElement(AmazonPdaPageObjects.LoginEmailInput);
            SendKeys(AmazonPdaPageObjects.LoginEmailInput, email);
        }

        private void EnterPassword(string password)
        {
            ClickElement(AmazonPdaPageObjects.LoginPassInput);
            SendKeys(AmazonPdaPageObjects.LoginPassInput, password);
        }

        private void WaitSecurityCodeIfNecessary()
        {
            if (!IsElementPresent(AmazonPdaPageObjects.CodeInput))
            {
                return;
            }

            WaitElementClickable(AmazonPdaPageObjects.CodeInput, timeout);
            WaitSecurityCode();
        }

        private void WaitSecurityCode()
        {
            Logger.Info("Waiting the code...");
            WaitElementClickable(AmazonPdaPageObjects.AccountButton, timeout);
        }
    }
}
