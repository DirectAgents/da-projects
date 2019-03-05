using System.Configuration;

namespace CakeExtracter.Etl.DSP.Configuration
{
    internal class AmazonDspConfigurationProvider
    {
        private const string AmazonDspAwsAccessKeyConfigurationKey = "AmazonDspAwsAccessKey";

        private const string AmazonDspAwsAccessSecretConfigurationKey = "AmazonDspAwsAccessSecret";

        private const string AmazonDspAwsAccessReportNameConfigurationKey = "AmazonDspAwsAccessReportName";

        public AmazonDspConfigurationProvider()
        {
        }

        public string GetAwsAccessToken()
        {
            return ConfigurationManager.AppSettings[AmazonDspAwsAccessKeyConfigurationKey];
        }

        public string GetAwsSecretValue()
        {
            return ConfigurationManager.AppSettings[AmazonDspAwsAccessSecretConfigurationKey];
        }

        public string GetDspReportName()
        {
            return ConfigurationManager.AppSettings[AmazonDspAwsAccessReportNameConfigurationKey];
        }
    }
}
