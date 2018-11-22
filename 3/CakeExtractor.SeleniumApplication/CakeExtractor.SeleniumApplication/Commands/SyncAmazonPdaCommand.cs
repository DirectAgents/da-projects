using CakeExtractor.SeleniumApplication.Drivers;
using CakeExtractor.SeleniumApplication.PageActions.AmazonPda;
using ManyConsole;
using OpenQA.Selenium;
using System.Threading;
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
            FileManager.ParsingCsvFiles(downloadDir, reportNameTemplate);
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
            waitPageTimeoutInMinuts = int.TryParse(Properties.Settings.Default.WaitPageTimeoutInMinuts, out var i) ? i : 30;
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
    }
}
