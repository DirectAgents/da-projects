using System;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.BingAds;
using Microsoft.BingAds.Reporting;

namespace BingAds
{
    public class BingUtility
    {
        public static ServiceClient<IReportingService> _service;

        private readonly string _customerID = ConfigurationManager.AppSettings["BingCustomerID"];
        private readonly string _developerToken = ConfigurationManager.AppSettings["BingApiToken"];
        private readonly string _userName = ConfigurationManager.AppSettings["BingApiUsername"];
        private readonly string _password = ConfigurationManager.AppSettings["BingApiPassword"];
        private readonly string _folder = ConfigurationManager.AppSettings["BingReportFolder"];
        private readonly string _filename = ConfigurationManager.AppSettings["BingReportFilename"];

        private long CustomerID { get; set; }
        private string DeveloperToken { get; set; }
        private string UserName { get; set; }
        private string Password { get; set; }

        private void ResetCredentials()
        {
            CustomerID = Convert.ToInt64(_customerID);
            DeveloperToken = _developerToken;
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
        }
        private AuthorizationData GetAuthorizationData()
        {
            var authentication = new PasswordAuthentication(UserName, Password);
            var authorizationData = new AuthorizationData
            {
                Authentication = authentication,
                CustomerId = CustomerID,
                //AccountId: not needed?
                DeveloperToken = DeveloperToken
            };
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

        // returns filepath of csv
        public string GetDailySummaries(long accountId, DateTime startDate, DateTime endDate)
        {
            SetCredentials(accountId);
            var authorizationData = GetAuthorizationData();
            var reportRequest = GetReportRequest_DailySums(accountId, startDate, endDate);

            var task = GetReportAsync(authorizationData, reportRequest);
            task.Wait();
            return task.Result;
        }

        private ReportRequest GetReportRequest_DailySums(long accountId, DateTime startDate, DateTime endDate)
        {
            var reportRequest = new ConversionPerformanceReportRequest
            {
                Format = ReportFormat.Csv,
                ReportName = "Conversion Report",
                ReturnOnlyCompleteData = true,
                Aggregation = NonHourlyReportAggregation.Daily,
                Scope = new AccountThroughAdGroupReportScope
                {
                    AccountIds = new[] { accountId },
                    AdGroups = null,
                    Campaigns = null
                },
                Time = new ReportTime
                {
                    CustomDateRangeStart = new Date
                    {
                        Year = startDate.Year,
                        Month = startDate.Month,
                        Day = startDate.Day
                    },
                    CustomDateRangeEnd = new Date
                    {
                        Year = endDate.Year,
                        Month = endDate.Month,
                        Day = endDate.Day
                    }
                },
                Columns = new[] {
                    ConversionPerformanceReportColumn.TimePeriod,
                    ConversionPerformanceReportColumn.Impressions,
                    ConversionPerformanceReportColumn.Clicks,
                    ConversionPerformanceReportColumn.Conversions,
                    ConversionPerformanceReportColumn.Spend,
                    ConversionPerformanceReportColumn.Revenue,
                    ConversionPerformanceReportColumn.AccountId,
                    ConversionPerformanceReportColumn.AccountName,
                    ConversionPerformanceReportColumn.AccountNumber,
                    ConversionPerformanceReportColumn.CampaignId,
                    ConversionPerformanceReportColumn.CampaignName
                }
            };
            return reportRequest;
        }

        // returns the filepath of the report (downloaded and unzipped)
        public async Task<string> GetReportAsync(AuthorizationData authorizationData, ReportRequest reportRequest)
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

                var waitTime = new TimeSpan(0, 0, 6);
                ReportRequestStatus reportRequestStatus = null;

                // Poll every X seconds.
                // If the call succeeds, stop polling. If the call or 
                // download fails, the call throws a fault.

                for (int i = 0; i < 10; i++)
                {
                    LogInfo(String.Format("Will check if the report is ready in {0} seconds...", waitTime.Seconds));
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
                    if (reportRequestStatus.Status == ReportRequestStatusType.Success)
                    {
                        string reportDownloadUrl = reportRequestStatus.ReportDownloadUrl;
                        string zipfileLocation = _folder + "\\" + _filename;

                        LogInfo(String.Format("Downloading from {0}.", reportDownloadUrl));
                        DownloadFile(reportDownloadUrl, zipfileLocation);
                        LogInfo(String.Format("The report was written to {0}.", zipfileLocation));
                        
                        ZipFile.ExtractToDirectory(zipfileLocation, _folder);

                        if (reportRequest.Format == ReportFormat.Csv)
                            filepath = _folder + "\\" + reportRequestId + ".csv";
                        //TODO: handle other formats
                    }
                    else if (reportRequestStatus.Status == ReportRequestStatusType.Error)
                    {
                        LogInfo("The request failed. Try requesting the report later. If the request continues to fail, contact support.");
                    }
                    else  // Pending
                    {
                        LogInfo(String.Format("The request is taking longer than expected. Save the report ID ({0}) and try again later.", reportRequestId));
                    }
                }
            }
            // Catch authentication exceptions
            catch (OAuthTokenRequestException ex)
            {
                LogInfo(string.Format("Couldn't get OAuth tokens. Error: {0}. Description: {1}", ex.Details.Error, ex.Details.Description));
            }
            // Catch Reporting service exceptions
            catch (FaultException<Microsoft.BingAds.Reporting.AdApiFaultDetail> ex)
            {
                LogInfo(string.Join("; ", ex.Detail.Errors.Select(error => string.Format("{0}: {1}", error.Code, error.Message))));
            }
            catch (FaultException<Microsoft.BingAds.Reporting.ApiFaultDetail> ex)
            {
                LogInfo(string.Join("; ", ex.Detail.OperationErrors.Select(error => string.Format("{0}: {1}", error.Code, error.Message))));
                LogInfo(string.Join("; ", ex.Detail.BatchErrors.Select(error => string.Format("{0}: {1}", error.Code, error.Message))));
            }
            catch (WebException ex)
            {
                LogInfo(ex.Message);

                if (ex.Response != null)
                    LogInfo("HTTP status code: " + ((HttpWebResponse)ex.Response).StatusCode);
            }
            catch (IOException ex)
            {
                LogInfo(ex.Message);
            }
            catch (Exception ex)
            {
                LogInfo(ex.Message);
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
