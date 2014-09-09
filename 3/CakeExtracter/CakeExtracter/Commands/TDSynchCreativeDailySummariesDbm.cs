using CakeExtracter.Common;
using CakeExtracter.Etl.TradingDesk.Extracters;
using CakeExtracter.Etl.TradingDesk.Loaders;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Storage.v1;
using System;
using System.ComponentModel.Composition;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class TDSynchCreativeDailySummariesDbm : ConsoleCommand
    {
        public override void ResetProperties()
        {
        }

        public TDSynchCreativeDailySummariesDbm()
        {
            IsCommand("tdSynchCreativeDailySummariesDbm", "synch CreativeDailySummaries for DBM Report");
        }

        public override int Execute(string[] remainingArguments)
        {
            var streamReader = GetReport_StreamReader();

            var extracter = new DbmCsvExtracter(streamReader, true);
            var loader = new DbmCreativeDailySummaryLoader();
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
            return 0;
        }

        private static StreamReader GetReport_StreamReader()
        {
            string serviceEmail = ConfigurationManager.AppSettings["GoogleAPI_ServiceEmail"];
            string certPath = ConfigurationManager.AppSettings["GoogleAPI_Certificate"];
            var certificate = new X509Certificate2(certPath, "notasecret", X509KeyStorageFlags.Exportable);

            var credential = new ServiceAccountCredential(
                new ServiceAccountCredential.Initializer(serviceEmail)
                {
                    Scopes = new[] { StorageService.Scope.DevstorageReadOnly }
                }.FromCertificate(certificate));

            var service = new StorageService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "DA Client Portal"
            });

            string bucketName = "151075984680687222131410283081521_report"; // Betterment_creative

            var request = service.Objects.List(bucketName);
            var results = request.Execute();

            string dateString = DateTime.Today.ToString("yyyy-MM-dd");
            var reportObject = results.Items.Where(i => i.Name.Contains(dateString)).FirstOrDefault();

            HttpWebRequest req = createRequest(reportObject.MediaLink, credential);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

            // Handle redirects manully to ensure that the Authorization header is present if
            // our request is redirected.
            if (resp.StatusCode == HttpStatusCode.TemporaryRedirect)
            {
                req = createRequest(resp.Headers["Location"], credential);
                resp = (HttpWebResponse)req.GetResponse();
            }

            Stream stream = resp.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            return reader;
        }

        /// <summary>
        /// Generate a HttpWebRequest for the given URL with the appropriate OAuth2 authorization
        /// header applied.  The HttpWebRequest object returned has its AllowAutoRedirect option
        /// disabled to allow us to manually handle redirects.
        /// </summary>
        private static HttpWebRequest createRequest(string url, ServiceAccountCredential credential)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("Authorization", "Bearer " + credential.Token.AccessToken);
            request.AllowAutoRedirect = false;
            return request;
        }
    }
}
