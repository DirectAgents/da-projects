using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Amazon
{
    public class AwsFilesDownloader
    {
        private readonly string amazonReportBucketName = "amazonselfservicedsp";

        private readonly string awsAccessKey;

        private readonly string awsSecretKey;

        private AmazonS3Client client;

        private readonly Action<string> logInfo;

        private readonly Action<Exception> logError;

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
        /// <param name="date">Date to pull the reports.</param>
        /// <returns>The list of CSV contents from reports.</returns>
        public string GetLatestFileContentByFilePrefix(string filePrefix)
        {
            try
            {
                IList<S3Object> allObjects = GetAllObjects();
                var reportsObjects = allObjects.Where(report => report.Key.StartsWith(filePrefix));
                var latestReport = reportsObjects.OrderByDescending(r => r.LastModified).FirstOrDefault();
                if (latestReport == null)
                {
                    logInfo("No files with prefix was foud");
                    return string.Empty;
                }
                else
                {
                    return GetObjectContent(latestReport);
                }
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                logError(new Exception("S3 error occurred while downloading latest report", amazonS3Exception));
                return string.Empty;
            }
            catch (Exception e)
            {
                logError(e);
                return string.Empty;
            }
        }

        private string GetObjectContent(S3Object reportObject)
        {
            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = this.amazonReportBucketName,
                Key = reportObject.Key
            };
            using (GetObjectResponse response = client.GetObject(request))
            {
                 return GetS3ObjectContent(response);
            }
        }

        /// <summary>
        /// Gets a list of S3 Objects from the configured bucket.
        /// </summary>
        /// <returns>A list of S3 objects.</returns>
        private IList<S3Object> GetAllObjects()
        {
            var getObjectsRequest = new ListObjectsV2Request
            {
                BucketName = this.amazonReportBucketName
            };
            ListObjectsV2Response objectListResponse = client.ListObjectsV2(getObjectsRequest);
            return objectListResponse.S3Objects;
        }

        /// <summary>
        /// Gets the text content of the S3 object from the response.
        /// </summary>
        /// <param name="response">Response of AWS S3 service.</param>
        /// <returns>A text content of S3 object.</returns>
        private string GetS3ObjectContent(GetObjectResponse response)
        {
            using (Stream responseStream = response.ResponseStream)
            {
                return this.TryUnzipStream(responseStream);
            }
        }

        /// <summary>
        /// Tries to unzip the expected stream if possible.
        /// If the stream is an incorrect zip, the empty string is returned.
        /// </summary>
        /// <param name="stream">Stream to unzip.</param>
        /// <returns>Unzipped string contents of the stream.</returns>
        private string TryUnzipStream(Stream stream)
        {
            try
            {
                using (GZipStream gzip = new GZipStream(stream, CompressionMode.Decompress))
                {
                    using (StreamReader reader = new StreamReader(gzip))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (Exception e)
            {
                logError( new Exception("Unzipping error occured. Exception: " + e.ToString(), e));
                return string.Empty;
            }
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
