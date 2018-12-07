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
        private AmazonPdaPageActions _pageActions;
        private string _downloadDir;
        private string _reportNameTemplate;
        private string _campaignsUrl;
        private string _profileText;
        private int _countExecute;

        public AmazonPDAExtractor(AmazonPdaPageActions pageActions, string downloadDir, 
            string reportNameTemplate, string campaignsUrl, string profileText, int countExecute)
        {
            _pageActions = pageActions;
            _downloadDir = downloadDir;
            _reportNameTemplate = reportNameTemplate;
            _campaignsUrl = campaignsUrl;
            _profileText = profileText;
            _countExecute = countExecute;
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
                FileManager.CleanDirectory(_downloadDir, _reportNameTemplate);
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
                _pageActions.NavigateToUrl(_campaignsUrl, AmazonPdaPageObjects.FilterByButton);

                FileManager.TmpConsoleLog($"Number of executions: {_countExecute}");
                if (_countExecute <= 1)
                {
                    var profileUrl = _pageActions.GetProfileUrl(_profileText);
                    if (!string.IsNullOrEmpty(profileUrl))
                    {
                        _pageActions.NavigateToUrl(profileUrl, AmazonPdaPageObjects.FilterByButton);
                    }

                    _pageActions.SetFiltersOnCampaigns();
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Could not navigate to the page with campaigns information: {e.Message}", e);
            }
        }

        private CampaignInfo GetCampaignInfo(string campaignUrl)
        {
            var campaign = new CampaignInfo();
            try
            {
                _pageActions.NavigateToUrl(campaignUrl, AmazonPdaPageObjects.CampaignTabContainer);

                _pageActions.NavigateToTab(AmazonPdaPageObjects.CampaignSettingsTab,
                    AmazonPdaPageObjects.CampaignSettingsContent);
                _pageActions.GetCampaignSettingsInfo(campaign);

                _pageActions.NavigateToTab(AmazonPdaPageObjects.CampaignReportsTab,
                    AmazonPdaPageObjects.CampaignReportsContent);
                _pageActions.GetCampaignReportsInfo(campaign, _downloadDir, _reportNameTemplate);

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
                    _pageActions.NavigateNextCampaignPage();
                }

                FileManager.TmpConsoleLog("Retrieving a list of campaign name web elements...");
                var campNameWebElementList = _pageActions.GetCampaignsNameWebElementsList(
                    AmazonPdaPageObjects.CampaignsContainer,
                    AmazonPdaPageObjects.CampaignsNamesList);
                FileManager.TmpConsoleLog($"[{campNameWebElementList.Count}] web elements received");

                foreach (var campNameWebElem in campNameWebElementList)
                {
                    campaignAllUrlList.Add(_pageActions.GetCampaignUrl(campNameWebElem));
                }
            } while (_pageActions.IsElementEnabledAndDisplayed(AmazonPdaPageObjects.NavigateNextPageButton));

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
                    campaignsInfoList.Add(GetCampaignInfo(campaignUrl));
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
