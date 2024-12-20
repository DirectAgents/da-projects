﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Apple.Logger;
using RestSharp;
using RestSharp.Deserializers;

namespace Apple
{
    // Certificate Code comes from SearchAccount.ExternalId
    // For clients under DA, use "DA". For others, use the same 'code' as in the p12 certificate's filename.
    // Need to create a new certificate for non-DA clients. (Name the file "AppleCertificate[CODE].p12" and place in the 'AppleP12Location'.)
    // https://developer.apple.com/library/content/documentation/General/Conceptual/AppStoreSearchAdsAPIReference/API_Overview.html
    public class AppleAdsUtility
    {
        public const int RowsReturnedAtATime = 20;

        private string AppleBaseUrl { get; set; }

        private string AppleP12Location { get; set; }

        private string AppleP12Password { get; set; }

        // --- Logging ---
        private AppleLogger appleLogger;

        // --- Constructors ---
        public AppleAdsUtility()
        {
            Setup();
        }

        public AppleAdsUtility(Action<string> logWarn, Action<Exception> logError)
            : this()
        {
            appleLogger = new AppleLogger(logWarn, logError);
        }

        private void Setup()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            AppleBaseUrl = ConfigurationManager.AppSettings["AppleBaseUrl"];
            AppleP12Location = ConfigurationManager.AppSettings["AppleP12Location"];
            AppleP12Password = ConfigurationManager.AppSettings["AppleP12Password"];
        }

        private IRestResponse<T> ProcessRequest<T>(RestRequest restRequest, string certificateCode = "") //, bool postNotGet = false)
            where T : new()
        {
            var restClient = new RestClient(AppleBaseUrl);
            restClient.AddHandler("application/json", new JsonDeserializer());
            var certificate = CreateCertificate(certificateCode);
            restClient.ClientCertificates = new X509CertificateCollection() { certificate };
            var certificateExpirationDate = certificate.NotAfter;
            CheckСertificateValidity(certificateExpirationDate);
            var response = restClient.Execute<T>(restRequest);
            return response;
        }

        private X509Certificate2 CreateCertificate(string certificateCode)
        {
            var filename = String.Format(AppleP12Location, certificateCode);
            //LogInfo("certificate location: " + filename);
            var certificate = new X509Certificate2();
            certificate.Import(filename, AppleP12Password,
                X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);
            return certificate;
        }

        public void Test()
        {
            var request = new RestRequest("campaigns");
            request.AddHeader("Authorization", "orgId=124790"); //80760-DA, 124790-Crackle

            var response = ProcessRequest<AppleResponse>(request);
        }

        public IEnumerable<AppleStatGroup> GetCampaignDailyStats(DateTime startDate, DateTime endDate, string orgId, string certificateCode = "")
        {
            var requestBody = new ReportRequest
            {
                startTime = startDate.ToString("yyyy-MM-dd"),
                endTime = endDate.ToString("yyyy-MM-dd"),
                granularity = "DAILY",
                selector = new Selector
                {
                    orderBy = new[] { new Sel_OrderBy { field = "campaignName", sortOrder = "ASCENDING" } },
                    pagination = new Sel_Pagination { limit = RowsReturnedAtATime }
                }
            };
            bool done = false;
            while (!done)
            {
                var response = GetReport(requestBody, orgId, certificateCode);
                if (response != null && response.error != null)
                {
                    appleLogger.LogWarn("response.error: " + response.error);
                }
                done = true;
                if (response != null && response.data != null && response.data.reportingDataResponse != null && response.data.reportingDataResponse.row != null)
                {
                    var statGroups = response.data.reportingDataResponse.row;
                    foreach (var statGroup in statGroups)
                        yield return statGroup;
                    var pagination = response.pagination;
                    if (pagination.startIndex + pagination.itemsPerPage < pagination.totalResults)
                    {
                        done = false;
                        requestBody.selector.pagination.offset += pagination.itemsPerPage;
                    }
                }
            }
        }

        private AppleReportResponse GetReport(ReportRequest requestBody, string orgId, string certificateCode)
        {
            var request = new RestRequest("reports/campaigns", Method.POST);
            request.AddHeader("Authorization", "orgId=" + orgId);
            request.AddJsonBody(requestBody);
            var restResponse = ProcessRequest<AppleReportResponse>(request, certificateCode);

            if (restResponse != null)
            {
                return restResponse.Data;
            }
            else
            {
                return null;
            }
        }

        private void CheckСertificateValidity(DateTime certificateExpirationDateTime)
        {
            const int numberOfDaysForWarnings = 7;
            var startOfPotentialWarningPeriod = DateTime.Now;
            var finishOfPotentialWarningPeriod = startOfPotentialWarningPeriod.AddDays(numberOfDaysForWarnings);
            if (!DoesPeriodOfWarningsBegin(finishOfPotentialWarningPeriod, certificateExpirationDateTime))
            {
                return; 
            }
            WarningPeriodHasBegun(certificateExpirationDateTime);
        }

        private bool DoesPeriodOfWarningsBegin(DateTime endOfPotentialWarningPeriod, DateTime certificateExpirationDateTime)
        {
            return DateTime.Compare(endOfPotentialWarningPeriod, certificateExpirationDateTime) >= 0;
        }

        private void WarningPeriodHasBegun(DateTime certificateExpirationDateTime)
        {
            if (IsCertificateExpired(certificateExpirationDateTime))
            {
                appleLogger.LogError("Certificate has expired");
            }
            else
            {
                CertificateHasNotExpired(certificateExpirationDateTime);
            }
        }

        private void CertificateHasNotExpired(DateTime certificateExpirationDateTime)
        {
            var numberOfDaysBeforeCerExpires = GetNumberOfDaysBeforeCertificateExpires(certificateExpirationDateTime);
            appleLogger.LogWarn($"Certificate expires in {numberOfDaysBeforeCerExpires} days");
        }

        private bool IsCertificateExpired(DateTime certificateExpirationDateTime)
        {
            return DateTime.Compare(DateTime.Now, certificateExpirationDateTime) >= 0;
        }

        private int GetNumberOfDaysBeforeCertificateExpires(DateTime certificateExpirationDateTime)
        {
            var timeUntilCertificateExpiration = certificateExpirationDateTime - DateTime.Now;
            return Convert.ToInt32(timeUntilCertificateExpiration.TotalDays);
        }
    }
}
