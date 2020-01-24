using System;
using System.ComponentModel.Composition;
using CakeExtracter.Common;
using CakeExtracter.Etl.AmazonSelenium.GenerationReportsTrigger;
using CakeExtracter.Etl.AmazonSelenium.PDA.Configuration;
using SeleniumDataBrowser.GenerationReportsTrigger;
using SeleniumDataBrowser.Helpers;

namespace CakeExtracter.Commands.Selenium
{
    /// <inheritdoc />
    /// <summary>
    /// The class represents a command that is used to creation reports on the Amazon Portal.
    /// </summary>
    [Export(typeof(ConsoleCommand))]
    public class AmazonGenerationReportsTrigger : ConsoleCommand
    {
        private PdaCommandConfigurationManager configurationManager;

        /// <summary>
        /// Gets or sets a value indicating whether to hide the browser window (default = false).
        /// </summary>
        public bool IsHidingBrowserWindow { get; set; }

        /// <inheritdoc cref="ConsoleCommand"/>/>
        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonGenerationReportsTrigger"/> class.
        /// </summary>
        public AmazonGenerationReportsTrigger()
        {
            IsCommand("AmazonGenerationReportsTrigger", "Command for generation reports of the Amazon Advertising Portal");
            HasOption<bool>("h|hideWindow=", "Include hiding the browser window", c => IsHidingBrowserWindow = c);
        }

        /// <inheritdoc />
        /// <summary>
        /// The method resets command arguments to defaults.
        /// </summary>
        public override void ResetProperties()
        {
            IsHidingBrowserWindow = false;
        }

        /// <inheritdoc />
        /// <summary>
        /// The method runs the current command
        /// and creates reports on the Amazon Advertising Portal
        /// based on the command arguments.
        /// </summary>
        /// <param name="remainingArguments"></param>
        /// <returns>Execution code.</returns>
        public override int Execute(string[] remainingArguments)
        {
            SetCommandConfigurationManager();
            var reportGenerator = BuildReportGenerator();
            Logger.Info("Amazon Generation reports.");
            reportGenerator.GenerateReports();
            Logger.Info("Amazon Reports Creation has been finished.");
            return 0;
        }

        private void SetCommandConfigurationManager()
        {
            const string cookiesDirectoryNameConfigurationKey = "GenerationReports_CookiesDirectoryName";
            const string emailConfigurationKey = "GenerationReports_EMail";
            const string emailPasswordConfigurationKey = "GenerationReports_EMailPassword";
            const string maxRetryAttemptsConfigurationKey = "GenerationReports_MaxRetryAttempts";
            const string pauseBetweenAttemptsConfigurationKey = "GenerationReports_PauseBetweenAttemptsInSeconds";
            const string intervalBetweenUnsuccessfulAndNewRequestsConfigurationKey =
                "GenerationReports_IntervalBetweenUnsuccessfulAndNewRequestsInMinutes";
            configurationManager = new PdaCommandConfigurationManager(
                cookiesDirectoryNameConfigurationKey,
                emailConfigurationKey,
                emailPasswordConfigurationKey,
                maxRetryAttemptsConfigurationKey,
                pauseBetweenAttemptsConfigurationKey,
                intervalBetweenUnsuccessfulAndNewRequestsConfigurationKey);
        }

        private AmazonReportGenerator BuildReportGenerator()
        {
            try
            {
                var logger = new SeleniumLogger(x => Logger.Info(x), Logger.Error, x => Logger.Warn(x));
                var reportGeneratorBuilder = new AmazonReportGeneratorBuilder(configurationManager);
                var reportGenerator = reportGeneratorBuilder.BuildReportGenerator(logger, IsHidingBrowserWindow);
                return reportGenerator;
            }
            catch (Exception e)
            {
                throw new Exception("Failed to build PDA data provider.", e);
            }
        }
    }
}
