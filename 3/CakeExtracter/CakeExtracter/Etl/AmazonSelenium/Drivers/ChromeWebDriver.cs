using OpenQA.Selenium.Chrome;

namespace CakeExtracter.Etl.AmazonSelenium.Drivers
{
    internal class ChromeWebDriver : ChromeDriver
    {
        public ChromeWebDriver(string downloadDir) 
            : base(GetOptions(downloadDir))
        {
        }

        private static ChromeOptions GetOptions(string downloadDir)
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddUserProfilePreference("download.default_directory", downloadDir);
            chromeOptions.AddUserProfilePreference("intl.accept_languages", "nl");
            chromeOptions.AddUserProfilePreference("disable-popup-blocking", "true");
            return chromeOptions;
        }
    }
}
