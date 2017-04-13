using System;
using System.ComponentModel.Composition;
using System.Security.Cryptography.X509Certificates;
using Apple;
using CakeExtracter.Common;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Deserializers;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class TestAppleCommand : ConsoleCommand
    {
        public override void ResetProperties()
        {
        }

        public TestAppleCommand()
        {
            IsCommand("testApple");
        }

        public override int Execute(string[] remainingArguments)
        {
            Test1();
            return 0;
        }

        public void Test1()
        {
            var appleAdsUtility = new AppleAdsUtility();
            string orgId = "124790"; //80760-DA, 124790-Crackle
            var startDate = new DateTime(2017, 4, 11);
            var endDate = new DateTime(2017, 4, 12);
            var stats = appleAdsUtility.GetCampaignDailyStats(orgId, startDate, endDate);
        }

        public void Test()
        {
            var restClient = new RestClient("https://api.searchads.apple.com/api/v1/campaigns");
            restClient.AddHandler("application/json", new JsonDeserializer());

            var certificate = new X509Certificate2();
            certificate.Import(@"G:\work\sp\apple\Certificates\AppleCertificateDA.p12", "appleda1", X509KeyStorageFlags.DefaultKeySet);
            restClient.ClientCertificates = new X509CertificateCollection() { certificate };

            var request = new RestRequest();
            request.AddHeader("Authorization", "orgId=124790"); //80760-DA, 124790-Crackle

            var response = restClient.Execute<AppleResponse>(request);
        }

    }
    public class AppleResponse
    {
        public object data { get; set; }
        public Pagination pagination { get; set; }
        public object error { get; set; }
    }
    public class Pagination
    {
        public int totalResults { get; set; }
        public int startIndex { get; set; }
        public int itemsPerPage { get; set; }
    }
}
