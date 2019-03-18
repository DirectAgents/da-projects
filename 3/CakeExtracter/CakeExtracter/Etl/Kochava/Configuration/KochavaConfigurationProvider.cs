using System.Configuration;

namespace CakeExtracter.Etl.Kochava.Configuration
{
    /// <summary>
    /// Kochava job configuration provider
    /// </summary>
    internal class KochavaConfigurationProvider
    {
        private const string AwsAccessKeyConfigurationKey = "KochavaAwsAccessKey";

        private const string AwsAccessSecretConfigurationKey = "KochavaAwsAccessSecret";

        private const string AwsBucketNameConfigurationKey = "KochavaAwsBucketName";

        /// <summary>
        /// Initializes a new instance of the <see cref="KochavaConfigurationProvider"/> class.
        /// </summary>
        public KochavaConfigurationProvider()
        {
        }

        /// <summary>
        /// Gets the aws access s3 token for accessing report files.
        /// </summary>
        /// <returns></returns>
        public string GetAwsAccessToken()
        {
            return ConfigurationManager.AppSettings[AwsAccessKeyConfigurationKey];
        }

        /// <summary>
        /// Gets the aws s3 secret value for accessing report files.
        /// </summary>
        /// <returns></returns>
        public string GetAwsSecretValue()
        {
            return ConfigurationManager.AppSettings[AwsAccessSecretConfigurationKey];
        }

        /// <summary>
        /// Gets the name of the aws s3 bucket.
        /// </summary>
        /// <returns></returns>
        public string GetAwsBucketName()
        {
            return ConfigurationManager.AppSettings[AwsBucketNameConfigurationKey];
        }
    }
}
