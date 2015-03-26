﻿using System;
using System.ComponentModel.Composition;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using CakeExtracter.Common;
using Google.Apis.Auth.OAuth2;
using Google.Apis.DoubleClickBidManager.v1;
using Google.Apis.Services;
using Google.Apis.Storage.v1;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class TestDbmCommand : ConsoleCommand
    {
        public int Mode { get; set; }
        public bool Download { get; set; }

        public override void ResetProperties()
        {
            Mode = 0;
            Download = false;
        }

        public TestDbmCommand()
        {
            IsCommand("testDbm");
            HasOption<int>("m|mode=", "Mode", c => Mode = c);
            HasOption<bool>("d|download=", "Download(true/false)", c => Download = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            if (Mode == 1)
                Test1();
            else
                Test0();
            return 0;
        }

        // Try DBM API
        public void Test0()
        {
            string serviceEmail = ConfigurationManager.AppSettings["GoogleAPI_ServiceEmail"];
            string certPath = ConfigurationManager.AppSettings["GoogleAPI_Certificate"];
            var certificate = new X509Certificate2(certPath, "notasecret", X509KeyStorageFlags.Exportable);
            var credential = new ServiceAccountCredential(
                new ServiceAccountCredential.Initializer(serviceEmail)
                {
                    Scopes = new[] { "https://www.googleapis.com/auth/doubleclickbidmanager" }
                }.FromCertificate(certificate));
            var service = new DoubleClickBidManagerService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "DA Client Portal"
            });
            var resource = new QueriesResource(service);
            var request = resource.Listqueries();
            var response = request.Execute();
        }

        public void Test1()
        {
            try
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

                //string bucketObjectName = "entity/20130430.0.Browser.json";
                //string bucketName = "gdbm-479-320231"; //"uspto-pair"; (ok) //"gdbm-public"; *forbidden*
                //string bucketName = "151075984680687222131403708138869_report";
                //string bucketName = "099700104058925463911409777269032_report";
                //string bucketName = "151075984680687222131409855651304_report"; // test123 (2:34)
                //string bucketName = "151075984680687222131409861541653_report"; // ui_created
                string bucketName = "151075984680687222131410283081521_report"; // Betterment_creative

                ListBucket(service, credential);

                //var request = service.Objects.List(bucketName);
                //var results = request.Execute();

                //var listRequest = service.BucketAccessControls.List(bucketName);
                //var bucketAccessControls = listRequest.Execute();
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception caught: " + e.Message);
                Console.Write(e.StackTrace);
            }
        }
        private void ListBucket(StorageService service, ServiceAccountCredential credential)
        {
            string bucketName = "151075984680687222131410283081521_report"; // Betterment_creative

            //var request = service.Buckets.Get(bucketName);
            var request = service.Objects.List(bucketName);
            var results = request.Execute();
            Logger.Info("Found {0} objects in the bucket.", results.Items.Count);

            if (this.Download)
                TestDownload(credential, results);
        }
        private void TestDownload(ServiceAccountCredential credential, Google.Apis.Storage.v1.Data.Objects results)
        {
            string dateString = DateTime.Today.ToString("yyyy-MM-dd");
            var reportObject = results.Items.Where(i => i.Name.Contains(dateString)).FirstOrDefault();

            //var x = service.Objects.Get(bucketName, reportObject.Name).Execute();
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
            String data = reader.ReadToEnd();
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
