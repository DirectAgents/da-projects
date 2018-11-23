using CakeExtractor.SeleniumApplication.Drivers;
using CakeExtractor.SeleniumApplication.PageActions.AmazonPda;
using ManyConsole;
using OpenQA.Selenium;
using System.Threading;
using CakeExtracter.Etl.TradingDesk.Extracters;
using CakeExtracter.Etl.TradingDesk.LoadersDA;
using CakeExtractor.SeleniumApplication.Helpers;
using CakeExtractor.SeleniumApplication.Jobs.ExtractAmazonPda;

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

        public SyncAmazonPdaCommand()
        {
            IsCommand("SyncAmazonPdaCommand", "Synch Amazon PDA Stats");
        }

        public override int Run(string[] remainingArguments)
        {
            Initialize();
            OpenPage();
            ExtractAmazonPdaScheduler.Start(this);
            AlwaysSleep();
            return 0;
        }

        public void ExtractCampaigns()
        {
            pageActions.RefreshPage();
            pageActions.ExportCsv();
            var csvPath = FileManager.CombinePath(downloadDir, reportNameTemplate);
            DoEtl(csvPath);
            FileManager.CleanDirectory(downloadDir, reportNameTemplate);
        }

        private void Initialize()
        {
            InitializeSettings();
            driver = new ChromeWebDriver(downloadDir);
            pageActions = new AmazonPdaPageActions(driver, waitPageTimeoutInMinuts);
            FileManager.CreateDirectoryIfNotExist(downloadDir);
        }

        private void InitializeSettings()
        {
            signInUrl = Properties.Settings.Default.SignInPageUrl;
            campaignsUrl = Properties.Settings.Default.CampaignsPageUrl;
            email = Properties.Settings.Default.EMail;
            pass = Properties.Settings.Default.EMailPassword;
            reportNameTemplate = Properties.Settings.Default.FilesNameTemplate;
            downloadDir = FileManager.GetAssemblyRelativePath(Properties.Settings.Default.DownloadsDirectoryName);
            waitPageTimeoutInMinuts = Properties.Settings.Default.WaitPageTimeoutInMinuts;
        }

        private void OpenPage()
        {
            pageActions.NavigateToAmazonAdvertising(signInUrl);
            pageActions.LoginProcess(email, pass);
            pageActions.NavigateToCampaigns(campaignsUrl);
            pageActions.SetFiltersOnCampaigns();
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
