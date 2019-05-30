﻿using System;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.BingAds;
using Microsoft.BingAds.V12.Reporting;

namespace BingAds
{
    public class BingUtility
    {
        public static ServiceClient<IReportingService> _service;

        private readonly string _customerID = ConfigurationManager.AppSettings["BingCustomerID"];
        private readonly string _developerToken = ConfigurationManager.AppSettings["BingApiToken"];
        private readonly string _clientId = ConfigurationManager.AppSettings["BingClientId"];
        private readonly string _refreshToken = ConfigurationManager.AppSettings["BingRefreshtoken"];

        private readonly string _userName = ConfigurationManager.AppSettings["BingApiUsername"];
        private readonly string _password = ConfigurationManager.AppSettings["BingApiPassword"];
        private readonly string _folder = ConfigurationManager.AppSettings["BingReportFolder"];
        private readonly string _filename = ConfigurationManager.AppSettings["BingReportFilename"];

        private readonly int[] criticalErrorCodes =
        {
            2004, // ReportingServiceNoCompleteDataAvailable
            2011, // ReportingServiceEndDateBeforeStartDate
        };

        private long CustomerID { get; set; }
        private string DeveloperToken { get; set; }
        private string UserName { get; set; }
        private string Password { get; set; }

        private string ClientId { get; set; }
        private string ClientSecret { get; set; }
        private string RefreshToken { get; set; }

        private void ResetCredentials()
        {
            CustomerID = Convert.ToInt64(_customerID);
            DeveloperToken = _developerToken;
            ClientId = _clientId;
            ClientSecret = "";
            RefreshToken = _refreshToken;
            UserName = _userName;
            Password = _password;
        }
        private void SetCredentials(long accountId)
        {
            ResetCredentials();

            string customerID = ConfigurationManager.AppSettings["BingCustomerID" + accountId];
            if (!String.IsNullOrWhiteSpace(customerID))
                CustomerID = Convert.ToInt64(customerID);
            string token = ConfigurationManager.AppSettings["BingApiToken" + accountId];
            if (!String.IsNullOrWhiteSpace(token))
                DeveloperToken = token;
            string username = ConfigurationManager.AppSettings["BingApiUsername" + accountId];
            if (!String.IsNullOrWhiteSpace(username))
                UserName = username;
            string password = ConfigurationManager.AppSettings["BingApiPassword" + accountId];
            if (!String.IsNullOrWhiteSpace(password))
                Password = password;

            string _clientId = ConfigurationManager.AppSettings["BingClientId" + accountId];
            if (!String.IsNullOrWhiteSpace(_clientId))
                ClientId = _clientId;
            string _clientSecret = ConfigurationManager.AppSettings["BingClientSecret" + accountId];
            if (!String.IsNullOrWhiteSpace(_clientSecret))
                ClientSecret = _clientSecret;
            string _refreshToken = ConfigurationManager.AppSettings["BingRefreshToken" + accountId];
            if (!String.IsNullOrWhiteSpace(_refreshToken))
                RefreshToken = _refreshToken;
        }
        private AuthorizationData GetAuthorizationData()
        {
            var authorizationData = new AuthorizationData
            {
                CustomerId = CustomerID,
                //AccountId: not needed?
                DeveloperToken = DeveloperToken
            };
            if (String.IsNullOrWhiteSpace(UserName) || UserName.Contains('@')) // is an email address (Microsoft account); can't use PasswordAuthentication
            {
                string redirString = ConfigurationManager.AppSettings["BingRedirectionUri"];
                var authorization = new OAuthWebAuthCodeGrant(ClientId, ClientSecret, new Uri(redirString));
                var task = authorization.RequestAccessAndRefreshTokensAsync(RefreshToken);
                task.Wait();
                // TODO: see if refreshtoken changed; if so, save the new one

                authorizationData.Authentication = authorization;
            }
            else
            {   // old style: BingAds username
                authorizationData.Authentication = new PasswordAuthentication(UserName, Password);
            }
            return authorizationData;
        }

        // --- Logging ---
        private Action<string> _LogInfo;
        private Action<string> _LogError;

        private void LogInfo(string message)
        {
            if (_LogInfo == null)
                Console.WriteLine(message);
            else
                _LogInfo("[BingAds.Reports] " + message);
        }

        private void LogError(string message)
        {
            if (_LogError == null)
                Console.WriteLine(message);
            else
                _LogError("[BingAds.Reports] " + message);
        }

        // --- Constructors ---
        public BingUtility()
        {
            ResetCredentials();
        }
        public BingUtility(Action<string> logInfo, Action<string> logError)
        {
            _LogInfo = logInfo;
            _LogError = logError;
            ResetCredentials();
        }

        // --- GetReport... ---

        // (returns filepath of csv)
        public string GetReport_DailySummaries(long accountId, DateTime startDate, DateTime endDate, bool forShoppingCampaigns = false)
        {
            var reportRequest = forShoppingCampaigns
                ? GetReportRequest_ProductDimension(accountId, startDate, endDate)
                : GetReportRequest_CampaignPerformance(accountId, startDate, endDate);

            return SendReportRequest(accountId, reportRequest);
        }
        public string GetReport_DailySummariesByGoal(long accountId, DateTime startDate, DateTime endDate)
        {
            var reportRequest = GetReportRequest_Goals(accountId, startDate, endDate);
            return SendReportRequest(accountId, reportRequest);
        }

        // --- private methods ---

        private string SendReportRequest(long accountId, ReportRequest reportRequest)
        {
            SetCredentials(accountId);
            var authorizationData = GetAuthorizationData();

            var task = GetReportAsync(authorizationData, reportRequest);
            task.Wait();
            return task.Result;
        }

        private static void InitCommonFieldsInReportRequest(ReportRequest reportRequest, string reportName)
        {
            reportRequest.Format = ReportFormat.Csv;
            reportRequest.ReportName = reportName;
            reportRequest.ReturnOnlyCompleteData = true;
        }

        private static ReportTime GetReportTime(DateTime startDate, DateTime endDate)
        {
            var time = new ReportTime
            {
                CustomDateRangeStart = ConvertToDate(startDate),
                CustomDateRangeEnd = ConvertToDate(endDate)
            };
            return time;
        }

        private static Date ConvertToDate(DateTime dateTime)
        {
            var date = new Date
            {
                Year = dateTime.Year,
                Month = dateTime.Month,
                Day = dateTime.Day
            };
            return date;
        }

        private ReportRequest GetReportRequest_CampaignPerformance(long accountId, DateTime startDate, DateTime endDate)
        {
            var reportRequest = new CampaignPerformanceReportRequest
            {
                Aggregation = ReportAggregation.Daily,
                Time = GetReportTime(startDate, endDate),
                Scope = new AccountThroughCampaignReportScope
                {
                    AccountIds = new[] { accountId },
                    //AdGroups = null,
                    Campaigns = null
                },
                Columns = new[] {
                    CampaignPerformanceReportColumn.TimePeriod,
                    CampaignPerformanceReportColumn.Impressions,
                    CampaignPerformanceReportColumn.Clicks,
                    CampaignPerformanceReportColumn.Conversions,
                    CampaignPerformanceReportColumn.Spend,
                    CampaignPerformanceReportColumn.Revenue,
                    CampaignPerformanceReportColumn.AccountId,
                    CampaignPerformanceReportColumn.AccountName,
                    CampaignPerformanceReportColumn.AccountNumber,
                    CampaignPerformanceReportColumn.CampaignId,
                    CampaignPerformanceReportColumn.CampaignName
                }
            };
            InitCommonFieldsInReportRequest(reportRequest, "Campaign Performance Report");
            return reportRequest;
        }

        private ReportRequest GetReportRequest_ProductDimension(long accountId, DateTime startDate, DateTime endDate)
        {
            var reportRequest = new ProductDimensionPerformanceReportRequest
            {
                Aggregation = ReportAggregation.Daily,
                Time = GetReportTime(startDate, endDate),
                Scope = new AccountThroughAdGroupReportScope
                {
                    AccountIds = new[] { accountId },
                    AdGroups = null,
                    Campaigns = null
                },
                Columns = new[] {
                    ProductDimensionPerformanceReportColumn.MerchantProductId,
                    ProductDimensionPerformanceReportColumn.TimePeriod,
                    ProductDimensionPerformanceReportColumn.Impressions,
                    ProductDimensionPerformanceReportColumn.Clicks,
                    ProductDimensionPerformanceReportColumn.Conversions,
                    ProductDimensionPerformanceReportColumn.Spend,
                    ProductDimensionPerformanceReportColumn.Revenue,
                    // No AccountId
                    ProductDimensionPerformanceReportColumn.AccountName,
                    ProductDimensionPerformanceReportColumn.AccountNumber,
                    ProductDimensionPerformanceReportColumn.CampaignId,
                    ProductDimensionPerformanceReportColumn.CampaignName
                }
            };
            InitCommonFieldsInReportRequest(reportRequest, "Product Dimension Performance Report");
            return reportRequest;
        }
        private ReportRequest GetReportRequest_Goals(long accountId, DateTime startDate, DateTime endDate)
        {
            var reportRequest = new GoalsAndFunnelsReportRequest
            {
                Aggregation = ReportAggregation.Daily,
                Time = GetReportTime(startDate, endDate),
                Scope = new AccountThroughAdGroupReportScope
                {
                    AccountIds = new[] { accountId },
                    AdGroups = null,
                    Campaigns = null
                },
                Columns = new[] {
                    GoalsAndFunnelsReportColumn.TimePeriod,
                    GoalsAndFunnelsReportColumn.GoalId,
                    GoalsAndFunnelsReportColumn.Goal,
                    GoalsAndFunnelsReportColumn.Conversions,
                    //GoalsAndFunnelsReportColumn.Spend,
                    GoalsAndFunnelsReportColumn.Revenue,
                    GoalsAndFunnelsReportColumn.AccountId,
                    GoalsAndFunnelsReportColumn.AccountName,
                    GoalsAndFunnelsReportColumn.AccountNumber,
                    GoalsAndFunnelsReportColumn.CampaignId,
                    GoalsAndFunnelsReportColumn.CampaignName
                }
            };
            InitCommonFieldsInReportRequest(reportRequest, "Goals And Funnels Report");
            return reportRequest;
        }

        // returns the filepath of the report (downloaded and unzipped)
        private async Task<string> GetReportAsync(AuthorizationData authorizationData, ReportRequest reportRequest)
        {
            string filepath = null;
            try
            {
                _service = new ServiceClient<IReportingService>(authorizationData);

                // SubmitGenerateReport helper method calls the corresponding Bing Ads service operation 
                // to request the report identifier. The identifier is used to check report generation status
                // before downloading the report. 

                var reportRequestId = await SubmitGenerateReportAsync(reportRequest);

                LogInfo("Report Request ID: " + reportRequestId);

                var waitTime = new TimeSpan(0, 0, 10);
                ReportRequestStatus reportRequestStatus = null;

                // Poll every X seconds.
                // If the call succeeds, stop polling. If the call or 
                // download fails, the call throws a fault.

                for (var i = 0; i < 6 * 10; i++) // 6 * # of minutes
                {
                    LogInfo($"Will check if the report is ready in {waitTime.Seconds} seconds...");
                    Thread.Sleep(waitTime);

                    // PollGenerateReport helper method calls the corresponding Bing Ads service operation 
                    // to get the report request status.
                    reportRequestStatus = await PollGenerateReportAsync(reportRequestId);

                    if (reportRequestStatus.Status == ReportRequestStatusType.Success ||
                        reportRequestStatus.Status == ReportRequestStatusType.Error)
                    {
                        break;
                    }
                    LogInfo("The report is not yet ready for download.");
                }

                if (reportRequestStatus != null)
                {
                    switch (reportRequestStatus.Status)
                    {
                        case ReportRequestStatusType.Success:
                            filepath = ExtractReportByLocalPath(reportRequestStatus.ReportDownloadUrl, reportRequest.Format, reportRequestId);
                            break;
                        case ReportRequestStatusType.Error:
                            LogError("The request failed. Try requesting the report later. If the request continues to fail, contact support.");
                            break;
                        // Pending
                        default:
                            LogError($"The request is taking longer than expected. Save the report ID ({reportRequestId}) and try again later.");
                            break;
                    }
                }
            }
            // Catch authentication exceptions
            catch (OAuthTokenRequestException ex)
            {
                LogError($"Couldn't get OAuth tokens. Error: {ex.Details.Error}. Description: {ex.Details.Description}");
            }
            // Catch Reporting service exceptions
            catch (FaultException<AdApiFaultDetail> ex)
            {
                LogError(string.Join("; ", ex.Detail.Errors.Select(error => $"{error.Code}: {error.Message}")));
            }
            catch (FaultException<ApiFaultDetail> ex)
            {
                var operationErrorsMessage = string.Join("; ", ex.Detail.OperationErrors.Select(error => $"{error.Code}: {error.Message}"));
                var batchErrorsMessage = string.Join("; ", ex.Detail.BatchErrors.Select(error => $"{error.Code}: {error.Message}"));

                if (ex.Detail.OperationErrors.Any(error => criticalErrorCodes.Contains(error.Code)))
                {
                    throw new Exception($"{operationErrorsMessage}\t{batchErrorsMessage}");
                }

                LogError(operationErrorsMessage);
                LogError(batchErrorsMessage);
            }
            catch (WebException ex)
            {
                var message = ex.Response != null
                    ? ex.Message + "\nHTTP status code: " + ((HttpWebResponse) ex.Response).StatusCode
                    : ex.Message;
                LogError(message);
            }
            catch (IOException ex)
            {
                LogError(ex.Message);
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }

            return filepath;
        }

        // Request the report and returns the ReportRequestId that can be used to check report
        // status and then used to download the report.
        private async Task<string> SubmitGenerateReportAsync(ReportRequest report)
        {
            var request = new SubmitGenerateReportRequest
            {
                ReportRequest = report
            };

            return (await _service.CallAsync((s, r) => s.SubmitGenerateReportAsync(r), request)).ReportRequestId;
        }

        // Checks the status of a report request. Returns a data object that contains both
        // report status and download URL. 
        private async Task<ReportRequestStatus> PollGenerateReportAsync(string reportId)
        {
            var request = new PollGenerateReportRequest
            {
                ReportRequestId = reportId
            };

            return (await _service.CallAsync((s, r) => s.PollGenerateReportAsync(r), request)).ReportRequestStatus;
        }

        private string ExtractReportByLocalPath(string reportDownloadUrl, ReportFormat? reportFormat, string reportRequestId)
        {
            if (string.IsNullOrEmpty(reportDownloadUrl))
            {
                LogInfo("Report URL is empty!");
                return null;
            }

            var zipFileLocation = _folder + "\\" + _filename;
            LogInfo($"Downloading from {reportDownloadUrl}.");
            DownloadFile(reportDownloadUrl, zipFileLocation);
            LogInfo($"The report was written to {zipFileLocation}.");
            ZipFile.ExtractToDirectory(zipFileLocation, _folder);

            //TODO: handle other formats
            return reportFormat == ReportFormat.Csv
                ? _folder + "\\" + reportRequestId + ".csv"
                : null;
        }

        static void DownloadFile(string reportDownloadUrl, string downloadPath)
        {
            var request = (HttpWebRequest)WebRequest.Create(reportDownloadUrl);
            var response = (HttpWebResponse)request.GetResponse();
            var fileInfo = new FileInfo(downloadPath);
            Stream responseStream = null;
            BinaryWriter binaryWriter = null;
            BinaryReader binaryReader = null;

            // If the folders in the specified path do not exist, create them.
            if (fileInfo.Directory != null && !fileInfo.Directory.Exists)
            {
                fileInfo.Directory.Create();
            }

            // Create the (ZIP) file.
            var fileStream = new FileStream(fileInfo.FullName, FileMode.Create);

            try
            {
                responseStream = response.GetResponseStream();
                binaryWriter = new BinaryWriter(fileStream);
                if (responseStream != null) binaryReader = new BinaryReader(responseStream);

                const int bufferSize = 100 * 1024;

                while (true)
                {
                    // Read report data from download URL.
                    if (binaryReader != null)
                    {
                        byte[] buffer = binaryReader.ReadBytes(bufferSize);

                        // Write report data to file.
                        binaryWriter.Write(buffer);

                        // If the end of the report is reached, break out of the loop.
                        if (buffer.Length != bufferSize)
                        {
                            break;
                        }
                    }
                }
            }
            finally
            {
                fileStream.Close();
                if (responseStream != null) responseStream.Close();
                if (binaryReader != null) binaryReader.Close();
                if (binaryWriter != null) binaryWriter.Close();
            }
        }

    }

}
