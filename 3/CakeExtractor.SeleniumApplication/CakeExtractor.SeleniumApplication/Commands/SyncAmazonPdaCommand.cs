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
using CakeExtractor.SeleniumApplication.SeleniumExtractors;

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
        private string cookiesDir;
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
            var extracter = new AmazonPDAExtractor(pageActions, downloadDir, reportNameTemplate);

            var loader = new AmazonCampaignSummaryLoader(-1); //accountId ??
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
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
                FileManager.CreateDirectoryIfNotExist("Cookies");
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
                cookiesDir = FileManager.GetAssemblyRelativePath("Cookies");
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
                var allCookies = FileManager.GetCookiesFromFiles(cookiesDir);
                if (!allCookies.Any())
                {
                    pageActions.NavigateToUrl(signInUrl, AmazonPdaPageObjects.ForgotPassLink);
                    pageActions.LoginProcess(email, pass);

                    var cookies = pageActions.GetAllCookies();
                    FileManager.SaveCookiesToFiles(cookies, cookiesDir);
                }
                else
                {
                    pageActions.NavigateToUrlWithoutWaiting(signInUrl);
                    foreach (var cookie in allCookies)
                    {
                        pageActions.SetCookie(cookie);
                    }
                }

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
