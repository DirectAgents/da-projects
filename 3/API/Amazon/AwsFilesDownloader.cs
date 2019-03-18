using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Amazon
{
    /// <summary>
    /// Aws s3 bucket content downloading helper.
    /// </summary>
    public class AwsFilesDownloader
    {
        private readonly string awsAccessKey;

        private readonly string awsSecretKey;

        private AmazonS3Client client;

        private readonly Action<string> logInfo;

        private readonly Action<Exception> logError;

        /// <summary>
        /// Initializes a new instance of the <see cref="AwsFilesDownloader"/> class.
        /// </summary>
        /// <param name="accessKey">The access key.</param>
        /// <param name="secretKey">The secret key.</param>
        /// <param name="logInfo">The log information.</param>
        /// <param name="logError">The log error.</param>
        public AwsFilesDownloader(string accessKey, string secretKey, Action<string> logInfo, Action<Exception> logError)
        {
            awsAccessKey = accessKey;
            awsSecretKey = secretKey;
            client = PrepareClient();
            this.logInfo = logInfo;
            this.logError = logError;
        }

        /// <summary>
        /// Gets report texts from CSV reports in the bucket.
        /// </summary>
        /// <param name="filePrefix">The file prefix.</param>
        /// <param name="bucketName">Name of the bucket.</param>
        /// <returns></returns>
        public Stream GetLatestFileContentByFilePrefix(string filePrefix, string bucketName)
        {
            try
            {
                IList<S3Object> allObjects = GetAllObjects(bucketName);
                var reportsObjects = allObjects.Where(report => report.Key.StartsWith(filePrefix));
                var latestReport = reportsObjects.OrderByDescending(r => r.LastModified).FirstOrDefault();
                if (latestReport == null)
                {
                    logInfo("No files with prefix was foud");
                    return null;
                }
                else
                {
                    return GetObjectContent(latestReport, bucketName);
                }
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                logError(new Exception("S3 error occurred while downloading latest report", amazonS3Exception));
                return null;
            }
            catch (Exception e)
            {
                logError(e);
                return null;
            }
        }

        private Stream GetObjectContent(S3Object reportObject, string bucketName)
        {
            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = reportObject.Key
            };
            GetObjectResponse response = client.GetObject(request);
            return response.ResponseStream;
        }

        /// <summary>
        /// Gets a list of S3 Objects from the configured bucket.
        /// </summary>
        /// <returns>A list of S3 objects.</returns>
        private IList<S3Object> GetAllObjects(string bucketName)
        {
            var getObjectsRequest = new ListObjectsV2Request
            {
                BucketName = bucketName
            };
            ListObjectsV2Response objectListResponse = client.ListObjectsV2(getObjectsRequest);
            return objectListResponse.S3Objects;
        }

        /// <summary>
        /// Creates a S3 client from configuration.
        /// </summary>
        /// <returns>S3 client.</returns>
        private AmazonS3Client PrepareClient()
        {
            var credentials = new BasicAWSCredentials(awsAccessKey, awsSecretKey);
            return new AmazonS3Client(credentials, RegionEndpoint.USEast1);
        }
    }
}
