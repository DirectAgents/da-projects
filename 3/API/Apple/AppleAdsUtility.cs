using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Deserializers;

namespace Apple
{
    public class AppleAdsUtility
    {
        private string AppleBaseUrl { get; set; }
        private string AppleP12Location { get; set; }
        private string AppleP12Password { get; set; }

        // --- Logging ---
        private Action<string> _LogInfo;
        private Action<string> _LogError;

        private void LogInfo(string message)
        {
            if (_LogInfo == null)
                Console.WriteLine(message);
            else
                _LogInfo("[YAMUtility] " + message);
        }

        private void LogError(string message)
        {
            if (_LogError == null)
                Console.WriteLine(message);
            else
                _LogError("[YAMUtility] " + message);
        }

        // --- Constructors ---
        public AppleAdsUtility()
        {
            Setup();
        }
        public AppleAdsUtility(Action<string> logInfo, Action<string> logError)
            : this()
        {
            _LogInfo = logInfo;
            _LogError = logError;
        }
        private void Setup()
        {
            AppleBaseUrl = ConfigurationManager.AppSettings["AppleBaseUrl"];
            AppleP12Location = ConfigurationManager.AppSettings["AppleP12Location"];
            AppleP12Password = ConfigurationManager.AppSettings["AppleP12Password"];
        }

        private IRestResponse<T> ProcessRequest<T>(RestRequest restRequest) //, bool postNotGet = false)
            where T : new()
        {
            var restClient = new RestClient(AppleBaseUrl);
            restClient.AddHandler("application/json", new JsonDeserializer());

            var certificate = new X509Certificate2();
            certificate.Import(AppleP12Location, AppleP12Password, X509KeyStorageFlags.DefaultKeySet);
            restClient.ClientCertificates = new X509CertificateCollection() { certificate };

            var response = restClient.Execute<T>(restRequest);
            return response;
        }

        public void Test()
        {
            var request = new RestRequest("campaigns");
            request.AddHeader("Authorization", "orgId=124790"); //80760-DA, 124790-Crackle

            var response = ProcessRequest<AppleResponse>(request);
        }

        public IEnumerable<AppleStatGroup> GetCampaignDailyStats(string orgId, DateTime startDate, DateTime endDate)
        {
            var requestBody = new ReportRequest
            {
                startTime = startDate.ToString("yyyy-MM-dd"),
                endTime = endDate.ToString("yyyy-MM-dd"),
                granularity = "DAILY",
                selector = new Selector
                {
                    orderBy = new[] { new OrderBy { field = "campaignName", sortOrder = "ASCENDING" } }
                }
            };

            var request = new RestRequest("reports/campaigns", Method.POST);
            request.AddHeader("Authorization", "orgId=" + orgId);
            request.AddJsonBody(requestBody);
            var response = ProcessRequest<AppleReportResponse>(request);

            //TODO: error checking

            if (response != null && response.Data != null && response.Data.data != null &&
                response.Data.data.reportingDataResponse != null && response.Data.data.reportingDataResponse.row != null)
            {
                var reportRows = response.Data.data.reportingDataResponse.row;
                return reportRows;
            }
            else
                return null;
        }
    }
}
