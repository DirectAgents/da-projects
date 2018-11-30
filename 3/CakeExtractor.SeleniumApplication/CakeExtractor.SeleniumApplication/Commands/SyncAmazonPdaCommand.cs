using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtractor.SeleniumApplication.Drivers;
using CakeExtractor.SeleniumApplication.PageActions.AmazonPda;
using ManyConsole;
using OpenQA.Selenium;
using System.Threading;
using CakeExtracter;
using CakeExtracter.Etl.TradingDesk.Extracters;
using CakeExtracter.Etl.TradingDesk.LoadersDA;
using CakeExtractor.SeleniumApplication.Helpers;
using CakeExtractor.SeleniumApplication.Jobs.ExtractAmazonPda;
using CakeExtractor.SeleniumApplication.Models;

namespace CakeExtractor.SeleniumApplication.Commands
{
    internal class SyncAmazonPdaCommand : ConsoleCommand
    {
        private IWebDriver driver;
        private AmazonPdaPageActions pageActions;

        private string signInUrl;
        private string campaignsUrl;
        private string email;
        private string pass;
        private string downloadDir;
        private string reportNameTemplate;
        private int waitPageTimeoutInMinuts;
        private string profileText;

        public SyncAmazonPdaCommand()
        {
            IsCommand("SyncAmazonPdaCommand", "Synch Amazon PDA Stats");
        }

        public override int Run(string[] remainingArguments)
        {            
            try
            {
                Initialize();
                OpenPage();
                ExtractAmazonPdaScheduler.Start(this);
                
                AlwaysSleep();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}{Environment.NewLine}{e.StackTrace}");
            }            
            return 0;
        }

        public void ExtractCampaignsFromExportCsv() //not use
        {
            pageActions.RefreshPage();
            pageActions.ExportCsv();
            var csvPath = FileManager.CombinePath(downloadDir, reportNameTemplate);
            DoEtl(csvPath);
            FileManager.CleanDirectory(downloadDir, reportNameTemplate);
        }

        public void ExtractCampaignsInfo()
        {
            try
            {
                var campaignAllUrlList = new List<string>();
                var campaignNotEmptyUrlList = new List<string>();
                var count = 0;
                
                Console.WriteLine("Searching all available campaigns URLs...");
                do
                {
                    Console.WriteLine($"Number of the page is {++count}");
                    if (count != 1)
                    {
                        pageActions.NavigateNextCampaignPage();
                    }

                    Console.WriteLine("Retrieving a list of campaign name web elements...");
                    var campNameWebElementList = pageActions.GetCampaignsNameWebElementsList(
                        AmazonPdaPageObjects.CampaignsContainer,
                        AmazonPdaPageObjects.CampaignsNamesList);
                    Console.WriteLine($"[{campNameWebElementList.Count}] web elements received");

                    foreach (var campNameWebElem in campNameWebElementList)
                    {
                        campaignAllUrlList.Add(pageActions.GetCampaignUrl(campNameWebElem));
                    }
                } while (pageActions.IsElementEnabled(AmazonPdaPageObjects.NavigateNextPageButton));
                
                campaignNotEmptyUrlList = campaignAllUrlList.FindAll(url => !string.IsNullOrEmpty(url));
                Console.WriteLine($"[{campaignAllUrlList.Count}] elements has been processed. [{campaignNotEmptyUrlList.Count}] campaign URLs received");

                Console.WriteLine("Retrieving information about campaigns...");
                var campaignsInfoList = new List<Campaign>();
                count = 0;

                foreach (var campaignUrl in campaignNotEmptyUrlList)
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
                Console.WriteLine($"Total number of campaign is [{campaignsInfoList.Count}]");

                // working with campaignsInfoList...
                Console.WriteLine("Working with information about campaigns...");



                //...after working with campaignsInfoList...
                FileManager.CleanDirectory(downloadDir, reportNameTemplate);
            }
            catch (Exception e)
            {
                throw new Exception($"Could not extract campaigns information: {e.Message}", e);
            }
        }

        private Campaign GetCampaignInfo(string campaignUrl)
        {
            var campaign = new Campaign();            
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

        private void Initialize()
        {            
            try
            {
                Console.WriteLine("Start initialize...");
                InitializeSettings();
                try
                {
                    Console.WriteLine("Create driver...");
                    driver = new ChromeWebDriver(downloadDir/*, waitPageTimeoutInMinuts*/);
                    Console.WriteLine("Ok");
                }
                catch (Exception e)
                {
                    throw new Exception($"Failed to initialize chrome driver (parameters: [{waitPageTimeoutInMinuts}], [{downloadDir}]): {e.Message}", e);
                }
                pageActions = new AmazonPdaPageActions(driver, waitPageTimeoutInMinuts);
                FileManager.CreateDirectoryIfNotExist(downloadDir);
                Console.WriteLine("Ok");
            }
            catch (Exception e)
            {
                throw new Exception($"Initialization error: {e.Message}", e);
            }
        }

        private void InitializeSettings()
        {
            try
            {
                Console.WriteLine("Start initialize settings...");
                signInUrl = Properties.Settings.Default.SignInPageUrl;
                campaignsUrl = Properties.Settings.Default.CampaignsPageUrl;
                email = Properties.Settings.Default.EMail;
                pass = Properties.Settings.Default.EMailPassword;
                reportNameTemplate = Properties.Settings.Default.FilesNameTemplate;
                downloadDir = FileManager.GetAssemblyRelativePath(Properties.Settings.Default.DownloadsDirectoryName);
                waitPageTimeoutInMinuts = Properties.Settings.Default.WaitPageTimeoutInMinuts;                
                profileText = Properties.Settings.Default.ProfileText;
                Console.WriteLine("Ok");
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to initialize settings: {e.Message}", e);
            }
        }

        private void OpenPage()
        {
            try
            {
                pageActions.NavigateToUrl(signInUrl, AmazonPdaPageObjects.ForgotPassLink);
                pageActions.LoginProcess(email, pass);
                pageActions.NavigateToUrl(campaignsUrl, AmazonPdaPageObjects.FilterByButton);
                var profileUrl = pageActions.GetProfileUrl(profileText);
                if (!string.IsNullOrEmpty(profileUrl))
                {
                    pageActions.NavigateToUrl(profileUrl, AmazonPdaPageObjects.FilterByButton);
                }
                pageActions.SetFiltersOnCampaigns();
            }
            catch (Exception e)
            {
                throw new Exception($"Could not navigate to the page with campaigns information: {e.Message}", e);
            }
        }

        private void AlwaysSleep()
        {
            while (true)
            {
                Thread.Sleep(int.MaxValue);
            }
        }

        private void DoEtl(string csvPath)
        {
            var extracter = new AmazonCampaignCsvExtractor(csvPath);
            var loader = new AmazonCampaignSummaryLoader(-1); //accountId ??
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
        }
    }
}
