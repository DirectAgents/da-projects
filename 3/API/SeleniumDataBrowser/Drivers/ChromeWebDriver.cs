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
        /// <param name="isHiddenBrowserWindow">Include hiding the browser window.</param>
        public ChromeWebDriver(string downloadDir, bool isHiddenBrowserWindow)
            : base(GetDriverService(isHiddenBrowserWindow), GetOptions(downloadDir, isHiddenBrowserWindow))
        {
        }

        private static ChromeOptions GetOptions(string downloadDir, bool isHiddenBrowserWindow)
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddUserProfilePreference("download.default_directory", downloadDir);
            chromeOptions.AddUserProfilePreference("intl.accept_languages", "nl");
            chromeOptions.AddUserProfilePreference("disable-popup-blocking", "true");
            if (isHiddenBrowserWindow)
            {
                chromeOptions.AddArgument("headless");
            }
            return chromeOptions;
        }

        private static ChromeDriverService GetDriverService(bool isHiddenBrowserWindow)
        {
            var driverService = ChromeDriverService.CreateDefaultService();
            if (isHiddenBrowserWindow)
            {
                driverService.HideCommandPromptWindow = true;
            }
            return driverService;
        }
    }
}
