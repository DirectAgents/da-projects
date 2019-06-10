using OpenQA.Selenium.Chrome;

namespace SeleniumDataBrowser.Drivers
{
    /// <inheritdoc />
    /// <summary>
    /// Class for instance Selenium Chrome Web Driver.
    /// </summary>
    internal class ChromeWebDriver : ChromeDriver
    {
        /// <inheritdoc cref="ChromeDriver"/>/>
        /// <summary>
        /// Initializes a new instance of the <see cref="ChromeWebDriver" /> class.
        /// </summary>
        /// <param name="downloadDir">Path to the directory for files that will be download.</param>
        public ChromeWebDriver(string downloadDir)
            : base(GetDriverService(), GetOptions(downloadDir))
        {
        }

        private static ChromeOptions GetOptions(string downloadDir)
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddUserProfilePreference("download.default_directory", downloadDir);
            chromeOptions.AddUserProfilePreference("intl.accept_languages", "nl");
            chromeOptions.AddUserProfilePreference("disable-popup-blocking", "true");

            //chromeOptions.AddArgument("headless");

            return chromeOptions;
        }

        private static ChromeDriverService GetDriverService()
        {
            var driverService = ChromeDriverService.CreateDefaultService();

            //driverService.HideCommandPromptWindow = true;

            return driverService;
        }
    }
}
