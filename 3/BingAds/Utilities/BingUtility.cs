using System;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using BingAds.Helpers;
using BingAds.Models;
using Microsoft.BingAds;
using Microsoft.BingAds.V12.Reporting;

namespace BingAds.Utilities
{
    /// <summary>
    /// Bing utility to send API requests to receive reports.
    /// </summary>
    public class BingUtility
    {
        private const string LoggerMessagePrefix = "[BingAds.Reports]";

        private static ServiceClient<IReportingService> service;

        private readonly int[] criticalErrorCodes =
        {
            2004, // ReportingServiceNoCompleteDataAvailable
            2011, // ReportingServiceEndDateBeforeStartDate
        };

        private readonly string folder = ConfigurationManager.AppSettings["BingReportFolder"];
        private readonly string filename = ConfigurationManager.AppSettings["BingReportFilename"];

        private readonly Action<string> logInfo;
        private readonly Action<string> logError;

        /// <summary>
        /// Gets or sets refresh tokens for Bing accounts.
        /// </summary>
        public static string[] RefreshTokens
        {
            get => ClientCredentialsProvider.RefreshTokens;
            set => ClientCredentialsProvider.RefreshTokens = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BingUtility"/> class.
        /// </summary>
        /// <param name="logInfo">Action that logs infos.</param>
        /// <param name="logError">Action that logs errors.</param>
        public BingUtility(Action<string> logInfo, Action<string> logError)
        {
            this.logInfo = logInfo;
            this.logError = logError;
        }

        /// <summary>
        /// Returns a metrics report for daily summaries.
        /// </summary>
        /// <param name="accountId">Account ID on the Bing portal.</param>
        /// <param name="startDate">Start date.</param>
        /// <param name="endDate">End date.</param>
        /// <param name="forShoppingCampaigns">The flag indicates which report type should be extracted.</param>
        /// <returns>The path to the CSV report file.</returns>
        public string GetDailySummariesReport(long accountId, DateTime startDate, DateTime endDate, bool forShoppingCampaigns = false)
        {
            var reportRequest = forShoppingCampaigns
                ? BingReportRequestsHelper.GetProductDimensionReportRequest(accountId, startDate, endDate)
                : BingReportRequestsHelper.GetCampaignPerformanceReportRequest(accountId, startDate, endDate);
            return SendReportRequest(accountId, reportRequest);
        }

        /// <summary>
        /// Returns a metrics report for goal summaries.
        /// </summary>
        /// <param name="accountId">Account ID on the Bing portal.</param>
        /// <param name="startDate">Start date.</param>
        /// <param name="endDate">End date.</param>
        /// <returns>The path to the CSV report file.</returns>
        public string GetDailySummariesByGoalReport(long accountId, DateTime startDate, DateTime endDate)
        {
            var reportRequest = BingReportRequestsHelper.GetGoalsReportRequest(accountId, startDate, endDate);
            return SendReportRequest(accountId, reportRequest);
        }

        private void LogInfo(string message)
        {
            var messageToLog = $"{LoggerMessagePrefix} {message}";
            if (logInfo == null)
            {
                Console.WriteLine(messageToLog);
            }
            else
            {
                logInfo(messageToLog);
            }
        }

        private void LogError(string message)
        {
            var messageToLog = $"{LoggerMessagePrefix} {message}";
            if (logError == null)
            {
                Console.WriteLine(messageToLog);
            }
            else
            {
                logError(messageToLog);
            }
        }

        private string SendReportRequest(long accountId, ReportRequest reportRequest)
        {
            var task = SendReportRequestAsync(accountId, reportRequest);
            task.Wait();
            return task.Result;
        }

        private async Task<string> SendReportRequestAsync(long accountId, ReportRequest reportRequest)
        {
            var credentials = ClientCredentialsProvider.GetClientCredentials(accountId);
            var authorizationData = await GetAuthorizationData(credentials);
            return await GetReportAsync(authorizationData, reportRequest);
        }

        private async Task<AuthorizationData> GetAuthorizationData(ClientCredentialsInfo credentials)
        {
            var authorizationData = new AuthorizationData
            {
                CustomerId = credentials.CustomerId,
                DeveloperToken = credentials.DeveloperToken,
                Authentication =
                    string.IsNullOrWhiteSpace(credentials.UserName) || credentials.UserName.Contains('@') // is an email address (Microsoft account); can't use PasswordAuthentication
                        ? await GetTokensAuthentication(credentials)
                        : new PasswordAuthentication(credentials.UserName, credentials.Password), // old style: BingAds username
            };
            return authorizationData;
        }

        private async Task<Authentication> GetTokensAuthentication(ClientCredentialsInfo credentials)
        {
            var redirectUrl = new Uri(ClientCredentialsProvider.RedirectUrl);
            var authorization = new OAuthWebAuthCodeGrant(credentials.ClientId, credentials.ClientSecret, redirectUrl);
            await authorization.RequestAccessAndRefreshTokensAsync(credentials.RefreshToken);
            ClientCredentialsProvider.UpdateRefreshToken(credentials, authorization.OAuthTokens.RefreshToken);
            return authorization;
        }

        // returns the filepath of the report (downloaded and unzipped)
        private async Task<string> GetReportAsync(AuthorizationData authorizationData, ReportRequest reportRequest)
        {
            string filepath = null;
            try
            {
                service = new ServiceClient<IReportingService>(authorizationData);

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
                    ? ex.Message + "\nHTTP status code: " + ((HttpWebResponse)ex.Response).StatusCode
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

            return (await service.CallAsync((s, r) => s.SubmitGenerateReportAsync(r), request)).ReportRequestId;
        }

        // Checks the status of a report request. Returns a data object that contains both
        // report status and download URL. 
        private async Task<ReportRequestStatus> PollGenerateReportAsync(string reportId)
        {
            var request = new PollGenerateReportRequest
            {
                ReportRequestId = reportId
            };

            return (await service.CallAsync((s, r) => s.PollGenerateReportAsync(r), request)).ReportRequestStatus;
        }

        private string ExtractReportByLocalPath(string reportDownloadUrl, ReportFormat? reportFormat, string reportRequestId)
        {
            if (string.IsNullOrEmpty(reportDownloadUrl))
            {
                LogInfo("Report URL is empty!");
                return null;
            }

            var zipFileLocation = folder + "\\" + filename;
            LogInfo($"Downloading from {reportDownloadUrl}.");
            DownloadFile(reportDownloadUrl, zipFileLocation);
            LogInfo($"The report was written to {zipFileLocation}.");
            ZipFile.ExtractToDirectory(zipFileLocation, folder);

            //TODO: handle other formats
            return reportFormat == ReportFormat.Csv
                ? folder + "\\" + reportRequestId + ".csv"
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
