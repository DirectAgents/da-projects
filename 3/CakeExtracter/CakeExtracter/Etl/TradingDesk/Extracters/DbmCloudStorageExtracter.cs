using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using CakeExtracter.Common;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Storage.v1;

namespace CakeExtracter.Etl.TradingDesk.Extracters
{
    public class DbmCloudStorageExtracter : Extracter<DbmRowBase>
    {
        private readonly DateRange dateRange;
        private readonly IEnumerable<string> bucketNames;

        public DbmCloudStorageExtracter(DateRange dateRange, IEnumerable<string> bucketNames)
        {
            this.dateRange = dateRange;
            this.bucketNames = bucketNames;
        }

        protected override void Extract()
        {
            Logger.Info("Extracting DailySummaries reports from {0} buckets - from {1:d} to {2:d}", bucketNames.Count(), dateRange.FromDate, dateRange.ToDate);
            var items = EnumerateRows();
            Add(items);
            End();
        }

        private IEnumerable<DbmRowBase> EnumerateRows()
        {
            var credential = CreateCredential();
            var service = CreateStorageService(credential);

            foreach (var bucketName in bucketNames)
            {
                var request = service.Objects.List(bucketName);
                var bucketObjects = request.Execute();

                foreach (var date in dateRange.Dates)
                {
                    string dateString = date.ToString("yyyy-MM-dd");
                    var reportObject = bucketObjects.Items.Where(i => i.Name.Contains(dateString)).FirstOrDefault();

                    HttpWebRequest req = CreateRequest(reportObject.MediaLink, credential);
                    HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

                    // Handle redirects manully to ensure that the Authorization header is present if
                    // our request is redirected.
                    if (resp.StatusCode == HttpStatusCode.TemporaryRedirect)
                    {
                        req = CreateRequest(resp.Headers["Location"], credential);
                        resp = (HttpWebResponse)req.GetResponse();
                    }

                    bool byCreative = true;
                    Stream stream = resp.GetResponseStream();
                    using (var reader = new StreamReader(stream))
                    {
                        foreach (var row in DbmCsvExtracter.EnumerateRowsStatic(reader, byCreative))
                            yield return row;
                    }
                }
            }
        }

        public static ServiceAccountCredential CreateCredential()
        {
            string serviceEmail = ConfigurationManager.AppSettings["GoogleAPI_ServiceEmail"];
            string certPath = ConfigurationManager.AppSettings["GoogleAPI_Certificate"];
            var certificate = new X509Certificate2(certPath, "notasecret", X509KeyStorageFlags.Exportable);

            var credential = new ServiceAccountCredential(
                new ServiceAccountCredential.Initializer(serviceEmail)
                {
                    Scopes = new[] { StorageService.Scope.DevstorageReadOnly }
                }.FromCertificate(certificate));

            return credential;
        }
        public static StorageService CreateStorageService(ServiceAccountCredential credential)
        {
            var service = new StorageService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "DA Client Portal"
            });
            return service;
        }

        /// <summary>
        /// Generate a HttpWebRequest for the given URL with the appropriate OAuth2 authorization
        /// header applied.  The HttpWebRequest object returned has its AllowAutoRedirect option
        /// disabled to allow us to manually handle redirects.
        /// </summary>
        public static HttpWebRequest CreateRequest(string url, ServiceAccountCredential credential)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("Authorization", "Bearer " + credential.Token.AccessToken);
            request.AllowAutoRedirect = false;
            return request;
        }
    }
}
