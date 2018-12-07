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

        public AmazonPDAExtractor(AmazonPdaPageActions pageActions, string downloadDir, string reportNameTemplate)
        {
            _pageActions = pageActions;
            _downloadDir = downloadDir;
            _reportNameTemplate = reportNameTemplate;
        }
        
        protected override void Extract()
        {
            try
            {
                Console.WriteLine("Searching all available campaigns URLs...");
                var campaignUrlList = GetCampaignUrlList();
                Console.WriteLine($"[{campaignUrlList.Count()}] campaign URLs received");

                Console.WriteLine("Retrieving information about campaigns...");
                var campaignsInfoList = GetCampaignsInfoList(campaignUrlList);
                Console.WriteLine($"Total number of campaign is [{campaignsInfoList.Count()}]");

                // working with campaignsInfoList...
                Console.WriteLine("Working with information about campaigns...");



                //...after working with campaignsInfoList...
                FileManager.CleanDirectory(_downloadDir, _reportNameTemplate);
            }
            catch (Exception e)
            {
                throw new Exception($"Could not extract campaigns information: {e.Message}", e);
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
                Console.WriteLine($"Number of the page is {++count}");
                if (count != 1)
                {
                    _pageActions.NavigateNextCampaignPage();
                }

                Console.WriteLine("Retrieving a list of campaign name web elements...");
                var campNameWebElementList = _pageActions.GetCampaignsNameWebElementsList(
                    AmazonPdaPageObjects.CampaignsContainer,
                    AmazonPdaPageObjects.CampaignsNamesList);
                Console.WriteLine($"[{campNameWebElementList.Count}] web elements received");

                foreach (var campNameWebElem in campNameWebElementList)
                {
                    campaignAllUrlList.Add(_pageActions.GetCampaignUrl(campNameWebElem));
                }
            } while (_pageActions.IsElementEnabled(AmazonPdaPageObjects.NavigateNextPageButton));

            Console.WriteLine($"[{campaignAllUrlList.Count}] elements has been processed");

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
                    Console.WriteLine($"Retrieving information about campaign [{++count}]...");
                    campaignsInfoList.Add(GetCampaignInfo(campaignUrl));
                    Console.WriteLine("Ok");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Warning: {e.Message}");
                }
            }
            return campaignsInfoList;
        }

    }
}
