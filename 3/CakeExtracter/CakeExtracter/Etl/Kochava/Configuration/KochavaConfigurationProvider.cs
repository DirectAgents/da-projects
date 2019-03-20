using System.Configuration;

namespace CakeExtracter.Etl.Kochava.Configuration
{
    /// <summary>
    /// Kochava job configuration provider
    /// </summary>
    public class KochavaConfigurationProvider
    {
        private const string AwsAccessKeyConfigurationKey = "KochavaAwsAccessKey";

        private const string AwsAccessSecretConfigurationKey = "KochavaAwsAccessSecret";

        private const string AwsBucketNameConfigurationKey = "KochavaAwsBucketName";

        private const string KochavaReportPeriodConfigurationKey = "KochavaReportPeriod";

        private const int DefaultReportPeriod = 10;

        /// <summary>
        /// Initializes a new instance of the <see cref="KochavaConfigurationProvider"/> class.
        /// </summary>
        public KochavaConfigurationProvider()
        {
        }

        /// <summary>
        /// Gets the aws access s3 token for accessing report files.
        /// </summary>
        /// <returns>Aws access token.</returns>
        public virtual string GetAwsAccessToken()
        {
            return ConfigurationManager.AppSettings[AwsAccessKeyConfigurationKey];
        }

        /// <summary>
        /// Gets the aws s3 secret value for accessing report files.
        /// </summary>
        /// <returns>Aws secret value.</returns>
        public virtual string GetAwsSecretValue()
        {
            return ConfigurationManager.AppSettings[AwsAccessSecretConfigurationKey];
        }

        /// <summary>
        /// Gets the name of the aws s3 bucket from configuration.
        /// </summary>
        /// <returns>Name of aws s3 bucket.</returns>
        public virtual string GetAwsBucketName()
        {
            return ConfigurationManager.AppSettings[AwsBucketNameConfigurationKey];
        }

        /// <summary>
        /// Gets the report period in days from configuration.
        /// </summary>
        /// <returns>Report period in days.</returns>
        public virtual int GetReportPeriodInDays()
        {
            try
            {
                var reportPeriod = int.Parse(ConfigurationManager.AppSettings[KochavaReportPeriodConfigurationKey]);
                return reportPeriod;
            }
            catch
            {
                Logger.Warn($"Error occured while retreiving report period. Default value {DefaultReportPeriod} will be used by default.");
                return DefaultReportPeriod;
            }
        }
    }
}
