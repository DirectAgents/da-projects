using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Etl;
using CakeExtractor.SeleniumApplication.Helpers;
using CakeExtractor.SeleniumApplication.Models;
using CakeExtractor.SeleniumApplication.PageActions.AmazonPda;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtractor.SeleniumApplication.SeleniumExtractors
{
    public class AmazonPDAExtractor: Extracter<StrategySummary>
    {
        private readonly AmazonPdaPageActions pageActions;
        private readonly string downloadDir;
        private readonly string reportNameTemplate;
        private readonly string campaignsUrl;
        private readonly string profileText;
        private readonly int countExecute;
        private readonly string accountPassword;

        public AmazonPDAExtractor(AmazonPdaPageActions pageActions, string downloadDir,
            string reportNameTemplate, string campaignsUrl, string profileText, 
            int countExecute, string accountPassword)
        {
            this.pageActions = pageActions;
            this.downloadDir = downloadDir;
            this.reportNameTemplate = reportNameTemplate;
            this.campaignsUrl = campaignsUrl;
            this.profileText = profileText;
            this.countExecute = countExecute;
            this.accountPassword = accountPassword;
        }

        protected override void Extract()
        {
            NavigateToPageCampaignInfo();
            try
            {
                FileManager.TmpConsoleLog("Searching all available campaigns URLs...");
                var campaignUrlList = GetCampaignUrlList();
                FileManager.TmpConsoleLog($"[{campaignUrlList.Count()}] campaign URLs received");

                FileManager.TmpConsoleLog("Retrieving information about campaigns...");
                var campaignsInfoList = GetCampaignsInfoList(campaignUrlList);
                FileManager.TmpConsoleLog($"Total number of campaign is [{campaignsInfoList.Count()}]");

                // working with campaignsInfoList...
                FileManager.TmpConsoleLog("Working with information about campaigns...");



                //...after working with campaignsInfoList...
                FileManager.CleanDirectory(downloadDir, reportNameTemplate);
            }
            catch (Exception e)
            {
                throw new Exception($"Could not extract campaigns information: {e.Message}", e);
            }
        }

        private void NavigateToPageCampaignInfo()
        {
            try
            {
                pageActions.NavigateToUrl(campaignsUrl, AmazonPdaPageObjects.FilterByButton);

                FileManager.TmpConsoleLog($"Number of executions: {countExecute}");
                if (countExecute <= 1)
                {
                    NavigateToProfile();
                    pageActions.SetFiltersOnCampaigns();
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Could not navigate to the page with campaigns information: {e.Message}", e);
            }
        }

        private void NavigateToProfile()
        {
            var profileUrl = pageActions.GetProfileUrl(profileText);
            if (string.IsNullOrEmpty(profileUrl))
                return;
            
            pageActions.NavigateToUrl(profileUrl);
            if (pageActions.IsElementPresent(AmazonPdaPageObjects.FilterByButton))
                return;

            if (pageActions.IsElementPresent(AmazonPdaPageObjects.LoginPassInput))
            {
                // need to repeat the password
                pageActions.LoginByPassword(accountPassword);
            }
        }

        private CampaignInfo GetCampaignInfo(string campaignUrl)
        {
            var campaign = new CampaignInfo();
            try
            {
                pageActions.NavigateToUrl(campaignUrl, AmazonPdaPageObjects.CampaignTabContainer);

                pageActions.NavigateToTab(AmazonPdaPageObjects.CampaignSettingsTab,
                    AmazonPdaPageObjects.CampaignSettingsContent);
                pageActions.GetCampaignSettingsInfo(campaign);

                pageActions.NavigateToTab(AmazonPdaPageObjects.CampaignReportsTab,
                    AmazonPdaPageObjects.CampaignReportsContent);
                pageActions.GetCampaignReportsInfo(campaign, downloadDir, reportNameTemplate);

                return campaign;
            }
            catch (Exception e)
            {
                throw new Exception($"Could not extract campaign information [name: {campaign.Name}]: {e.Message}", e);
            }
        }

        private IEnumerable<string> GetCampaignUrlList()
        {
            var campaignAllUrlList = new List<string>();
            var count = 0;            
            do
            {
                FileManager.TmpConsoleLog($"Number of the page is {++count}");
                if (count != 1)
                {
                    pageActions.NavigateNextCampaignPage();
                }

                FileManager.TmpConsoleLog("Retrieving a list of campaign name web elements...");
                var campNameWebElementList = pageActions.GetCampaignsNameWebElementsList(
                    AmazonPdaPageObjects.CampaignsNameContainer,
                    AmazonPdaPageObjects.CampaignsNamesList);
                FileManager.TmpConsoleLog($"[{campNameWebElementList.Count}] web elements received");

                foreach (var campNameWebElem in campNameWebElementList)
                {
                    campaignAllUrlList.Add(pageActions.GetCampaignUrl(campNameWebElem));
                }
            } while (pageActions.IsElementEnabledAndDisplayed(AmazonPdaPageObjects.NavigateNextPageButton));

            FileManager.TmpConsoleLog($"[{campaignAllUrlList.Count}] elements has been processed");

            return campaignAllUrlList.FindAll(url => !string.IsNullOrEmpty(url));
        }

        private IEnumerable<CampaignInfo> GetCampaignsInfoList(IEnumerable<string> campaignUrlList)
        {
            var campaignsInfoList = new List<CampaignInfo>();
            var count = 0;

            foreach (var campaignUrl in campaignUrlList)
            {
                try
                {
                    FileManager.TmpConsoleLog($"Retrieving information about campaign [{++count}]...");

                    var campaignInfoItem = GetCampaignInfo(campaignUrl);
                    campaignsInfoList.Add(campaignInfoItem);

                    FileManager.TmpConsoleLog("Ok");
                }
                catch (Exception e)
                {
                    FileManager.TmpConsoleLog($"Warning: {e.Message}");
                }
            }
            return campaignsInfoList;
        }

    }
}
