using BingAds.Reporting;
using System;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.ServiceModel;
using System.Threading;

namespace BingAds
{
    public class Reports
    {
        private readonly string _developerToken = ConfigurationManager.AppSettings["BingApiToken"];
        private readonly string _userName = ConfigurationManager.AppSettings["BingApiUsername"];
        private readonly string _password = ConfigurationManager.AppSettings["BingApiPassword"];
        private readonly string _folder = ConfigurationManager.AppSettings["BingReportFolder"];
        private readonly string _filename = ConfigurationManager.AppSettings["BingReportFilename"];

        private static ReportingServiceClient _service;

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
        public Reports()
        {
        }
        public Reports(Action<string> logInfo, Action<string> logError)
        {
            _LogInfo = logInfo;
            _LogError = logError;
        }

        // --- public methods ---
        public string GetKeywordPerformance(long accountId, long campaignId)
        {
            ReportRequest reportRequest = GetReportRequest_KeywordPerf(accountId, campaignId);
            var filepath = GetReport(reportRequest);
            return filepath;
        }
        public string GetDailySummaries(long accountId, DateTime startDate, DateTime endDate)
        {
            ReportRequest reportRequest = GetReportRequest_DailySums(accountId, startDate, endDate);
            var filepath = GetReport(reportRequest);
            return filepath;
        }

        // --- ReportRequest generators ---
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
        private ReportRequest GetReportRequest_KeywordPerf(long accountId, long campaignId)
        {
            var reportRequest = new KeywordPerformanceReportRequest
            {
                Format = ReportFormat.Csv,
                ReportName = "My Keyword Performance Report",
                ReturnOnlyCompleteData = true,
                Aggregation = ReportAggregation.Daily,
                Scope = new AccountThroughAdGroupReportScope
                {
                    AccountIds = null,
                    AdGroups = null,
                    Campaigns = new[]
                                {
                                    new CampaignReportScope
                                        {
                                            CampaignId = campaignId,
                                            ParentAccountId = accountId
                                        }
                                }
                },
                Time = new ReportTime
                {
                    //CustomDateRangeStart = new Date
                    //    {
                    //        Month = DateTime.Now.Month,
                    //        Day = DateTime.Now.Day,
                    //        Year = DateTime.Now.Year
                    //    },
                    //CustomDateRangeEnd = new Date
                    //    {
                    //    Month = 2,
                    //    Day = 15,
                    //    Year = DateTime.Now.Year + 1
                    //    },
                    PredefinedTime = ReportTimePeriod.Yesterday
                },
                Filter = new KeywordPerformanceReportFilter
                {
                    DeviceType = DeviceTypeReportFilter.Computer |
                                 DeviceTypeReportFilter.SmartPhone
                },
                Columns = new[]
                        {
                            KeywordPerformanceReportColumn.TimePeriod,
                            KeywordPerformanceReportColumn.AccountId,
                            KeywordPerformanceReportColumn.CampaignId,
                            KeywordPerformanceReportColumn.Keyword,
                            KeywordPerformanceReportColumn.KeywordId,
                            KeywordPerformanceReportColumn.DeviceType,
                            KeywordPerformanceReportColumn.BidMatchType,
                            KeywordPerformanceReportColumn.Clicks,
                            KeywordPerformanceReportColumn.Impressions,
                            KeywordPerformanceReportColumn.Ctr,
                            KeywordPerformanceReportColumn.AverageCpc,
                            KeywordPerformanceReportColumn.Spend,
                            KeywordPerformanceReportColumn.QualityScore
                        }
            };
            return reportRequest;
        }

        // --- the main method ---
        private string GetReport(ReportRequest reportRequest)
        {
            ReportRequestStatus response = null;
            string filepath = null;

            try
            {
                _service = new ReportingServiceClient();

                LogInfo("Requesting Report");
                var reportId = RequestReport(reportRequest);

                if (null != reportId)
                {
                    LogInfo("Report ID: " + reportId);

                    response = GetDownloadUrl(reportId);

                    if (null != response)
                    {
                        if (ReportRequestStatusType.Success == response.Status)
                        {
                            string zipfileLocation = _folder + "\\" + _filename;
                            DownloadReport(response.ReportDownloadUrl, zipfileLocation);
                            ZipFile.ExtractToDirectory(zipfileLocation, _folder);

                            if (reportRequest.Format == ReportFormat.Csv)
                                filepath = _folder + "\\" + reportId + ".csv";
                        }
                        else if (ReportRequestStatusType.Error == response.Status)
                        {
                            LogError("The request failed. Try requesting the report " +
                                "later.\nIf the request continues to fail, contact support.");
                        }
                        else  // Pending
                        {
                            LogError(String.Format("The request is taking longer than expected.\n " +
                                "Save the report ID ({0}) and try again later.", reportId));
                        }
                    }
                }

                _service.Close();
            }
            catch (CommunicationException e)
            {
                LogError("CommunicationException: " + e.Message);

                if (null != e.InnerException)
                {
                    LogError(e.InnerException.Message);
                }

                if (_service != null)
                {
                    _service.Abort();
                }
            }
            catch (TimeoutException e)
            {
                LogError("TimeoutException: " + e.Message);

                if (_service != null)
                {
                    _service.Abort();
                }
            }
            catch (WebException e)
            {
                LogError("Failed to access the report using the following URL: " + (response != null ? response.ReportDownloadUrl : ""));
                LogError("HTTP status code: " + ((HttpWebResponse)e.Response).StatusCode);
            }
            catch (IOException e)
            {
                LogError("There was an error reading the data from the HTTP response or writing the data to the report file.");
                LogError(e.Message);
            }
            catch (Exception e)
            {
                // Ignore fault exceptions that we already caught.

                if (!(e.InnerException is FaultException))
                {
                    LogError(e.Message);
                }

                if (_service != null)
                {
                    _service.Abort();
                }
            }
            return filepath;
        }

        // Called by GetReport...
        private string RequestReport(ReportRequest reportRequest)
        {
            var request = new SubmitGenerateReportRequest();
            SubmitGenerateReportResponse response;

            try
            {
                // Set the header information.

                request.DeveloperToken = _developerToken;
                request.UserName = _userName;
                request.Password = _password;

                // Set the request information.

                request.ReportRequest = reportRequest;

                response = _service.SubmitGenerateReport(request);
            }
            // Reporting service operations can throw AdApiFaultDetail.
            catch (FaultException<AdApiFaultDetail> fault)
            {
                // Log this fault.

                LogError("The operation failed with the following faults:");

                // If the AdApiError array is not null, the following are examples of error codes that may be found.
                foreach (AdApiError error in fault.Detail.Errors)
                {
                    LogError(String.Format("AdApiError. Code: {0}\nError Code: {1}\nMessage: {2}\n", error.Code, error.ErrorCode, error.Message));

                    switch (error.Code)
                    {
                        case 0:     // InternalError
                            break;
                        case 105:   // InvalidCredentials
                            break;
                        default:
                            LogError("Please see MSDN documentation for more details about the error code output above.");
                            break;
                    }
                }

                throw new Exception("", fault);
            }
            // Reporting service operations can throw ApiFaultDetail.
            catch (FaultException<ApiFaultDetail> fault)
            {
                // Log this fault.

                LogError("The operation failed with the following faults:");

                // If the BatchError array is not null, the following are examples of error codes that may be found.
                foreach (BatchError error in fault.Detail.BatchErrors)
                {
                    LogError(String.Format("BatchError at Index: {0}", error.Index));
                    LogError(String.Format("Code: {0}  Error Code: {1}  Message: {2}", error.Code, error.ErrorCode, error.Message));

                    switch (error.Code)
                    {
                        case 0:     // InternalError
                            break;
                        default:
                            LogError("Please see MSDN documentation for more details about the error code output above.");
                            break;
                    }
                }

                // If the OperationError array is not null, the following are examples of error codes that may be found.
                foreach (OperationError error in fault.Detail.OperationErrors)
                {
                    LogError(String.Format("OperationError. Code: {0}\nError Code: {1}\nMessage: {2}\n", error.Code, error.ErrorCode, error.Message));

                    switch (error.Code)
                    {
                        case 0:     // InternalError
                            break;
                        case 106:   // UserIsNotAuthorized
                            break;
                        case 2004:  // ReportingServiceNoCompleteDataAvaliable
                            break;
                        case 2007:  // ReportingServiceInvalidReportAggregation
                            break;
                        case 2008:  // ReportingServiceInvalidReportTimeSelection
                            break;
                        case 2009:  // ReportingServiceInvalidCustomDateRangeStart
                            break;
                        case 2010:  // ReportingServiceInvalidCustomDateRangeEnd
                            break;
                        case 2011:  // ReportingServiceEndDateBeforeStartDate
                            break;
                        case 2015:  // ReportingServiceRequiredColumnsNotSelected
                            break;
                        case 2016:  // ReportingServiceDuplicateColumns
                            break;
                        case 2017:  // ReportingServiceNoMeasureSelected
                            break;
                        case 2028:  // ReportingServiceInvalidAccountThruAdGroupReportScope
                            break;
                        case 2034:  // ReportingServiceInvalidTimePeriodColumnForSummaryReport
                            break;
                        default:
                            LogError("Please see MSDN documentation for more details about the error code output above.");
                            break;
                    }
                }

                throw new Exception("", fault);
            }
            return (null == response) ? null : response.ReportRequestId;
        }

        private ReportRequestStatus GetDownloadUrl(string reportId)
        {
            var request = new PollGenerateReportRequest();
            PollGenerateReportResponse response = null;
            var waitTime = new TimeSpan(0, 0, 15); //(0, 1, 0);  // Poll every 1 to 2 minutes for small reports.

            try
            {
                // Set the header information.

                request.DeveloperToken = _developerToken;
                request.UserName = _userName;
                request.Password = _password;

                // Set the request information.

                request.ReportRequestId = reportId;

                // Poll for status up to 12 times (or up to one hour).
                // If the call succeeds or fails, stop polling.

                for (int i = 0; i < 12; i++)
                {
                    Thread.Sleep(waitTime);

                    response = _service.PollGenerateReport(request);

                    if (ReportRequestStatusType.Success == response.ReportRequestStatus.Status ||
                        ReportRequestStatusType.Error == response.ReportRequestStatus.Status)
                    {
                        break;
                    }
                }
            }
            // Reporting service operations can throw AdApiFaultDetail.
            catch (FaultException<AdApiFaultDetail> fault)
            {
                // Log this fault.

                LogError("The operation failed with the following faults:");

                // If the AdApiError array is not null, the following are examples of error codes that may be found.
                foreach (AdApiError error in fault.Detail.Errors)
                {
                    LogError(String.Format("AdApiError. Code: {0}\nError Code: {1}\nMessage: {2}\n", error.Code, error.ErrorCode, error.Message));

                    switch (error.Code)
                    {
                        case 0:     // InternalError
                            break;
                        case 105:   // InvalidCredentials
                            break;
                        default:
                            LogError("Please see MSDN documentation for more details about the error code output above.");
                            break;
                    }
                }

                throw new Exception("", fault);
            }
            // Reporting service operations can throw ApiFaultDetail.
            catch (FaultException<ApiFaultDetail> fault)
            {
                // Log this fault.

                LogError("The operation failed with the following faults:");

                // If the BatchError array is not null, the following are examples of error codes that may be found.
                foreach (BatchError error in fault.Detail.BatchErrors)
                {
                    LogError(String.Format("BatchError at Index: {0}", error.Index));
                    LogError(String.Format("Code: {0}  Error Code: {1}  Message: {2}\n", error.Code, error.ErrorCode, error.Message));

                    switch (error.Code)
                    {
                        case 0:     // InternalError
                            break;
                        default:
                            LogError("Please see MSDN documentation for more details about the error code output above.");
                            break;
                    }
                }

                // If the OperationError array is not null, the following are examples of error codes that may be found.
                foreach (OperationError error in fault.Detail.OperationErrors)
                {
                    LogError("OperationError");
                    LogError(String.Format("Code: {0}  Error Code: {1}  Message: {2}\n", error.Code, error.ErrorCode, error.Message));

                    switch (error.Code)
                    {
                        case 0:     // InternalError
                            break;
                        case 106:   // UserIsNotAuthorized
                            break;
                        case 2100:  // ReportingServiceInvalidReportId
                            break;
                        default:
                            LogError("Please see MSDN documentation for more details about the error code output above.");
                            break;
                    }
                }

                throw new Exception("", fault);
            }

            return (null == response) ? null : response.ReportRequestStatus;
        }

        private static void DownloadReport(string reportDownloadUrl, string downloadPath)
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

            // Create the ZIP file.

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
