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
        private int _countExecute;

        public SyncAmazonPdaCommand()
        {
            IsCommand("SyncAmazonPdaCommand", "Synch Amazon PDA Stats");
        }

        public override int Run(string[] remainingArguments)
        {            
            try
            {
                Initialize();

                LoginWithCookies();
                
                ExtractAmazonPdaScheduler.Start(this); 
                
                AlwaysSleep();
            }
            catch (Exception e)
            {
                FileManager.TmpConsoleLog($"Error: {e.Message}{Environment.NewLine}{e.StackTrace}");
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
            _countExecute++;
            var extracter = new AmazonPDAExtractor(pageActions, downloadDir, reportNameTemplate, campaignsUrl, profileText, _countExecute);

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
                FileManager.TmpConsoleLog($"[{DateTime.Now}] Start initialize...");
                InitializeSettings();
                try
                {
                    FileManager.TmpConsoleLog("Create driver...");
                    driver = new ChromeWebDriver(downloadDir/*, waitPageTimeoutInMinuts*/);
                    FileManager.TmpConsoleLog("Ok");
                }
                catch (Exception e)
                {
                    throw new Exception($"Failed to initialize chrome driver (parameters: [{waitPageTimeoutInMinuts}], [{downloadDir}]): {e.Message}", e);
                }
                pageActions = new AmazonPdaPageActions(driver, waitPageTimeoutInMinuts);
                FileManager.CreateDirectoryIfNotExist(downloadDir);
                FileManager.CreateDirectoryIfNotExist("Cookies");
                _countExecute = 0;

                FileManager.TmpConsoleLog("Ok");
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
                FileManager.TmpConsoleLog("Start initialize settings...");
                signInUrl = Properties.Settings.Default.SignInPageUrl;
                campaignsUrl = Properties.Settings.Default.CampaignsPageUrl;
                email = Properties.Settings.Default.EMail;
                pass = Properties.Settings.Default.EMailPassword;
                reportNameTemplate = Properties.Settings.Default.FilesNameTemplate;
                downloadDir = FileManager.GetAssemblyRelativePath(Properties.Settings.Default.DownloadsDirectoryName);
                cookiesDir = FileManager.GetAssemblyRelativePath("Cookies");
                waitPageTimeoutInMinuts = Properties.Settings.Default.WaitPageTimeoutInMinuts;                
                profileText = Properties.Settings.Default.ProfileText;
                FileManager.TmpConsoleLog("Ok");
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to initialize settings: {e.Message}", e);
            }
        }

        private void LoginWithCookies()
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
            }
            catch (Exception e)
            {
                throw new Exception($"Could not to login with cookies: {e.Message}", e);
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
