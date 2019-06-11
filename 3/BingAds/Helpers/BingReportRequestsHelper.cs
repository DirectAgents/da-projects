using System;
using Microsoft.BingAds.V12.Reporting;

namespace BingAds.Helpers
{
    /// <summary>
    /// The utility to prepare parameters to request Bing reports.
    /// </summary>
    internal static class BingReportRequestsHelper
    {
        /// <summary>
        /// Returns parameters for campaign performance report.
        /// </summary>
        /// <param name="accountId">Account ID on the Bing portal.</param>
        /// <param name="startDate">Start date.</param>
        /// <param name="endDate">End date.</param>
        /// <returns>Prepared report request.</returns>
        public static ReportRequest GetCampaignPerformanceReportRequest(long accountId, DateTime startDate, DateTime endDate)
        {
            const string reportName = "Campaign Performance Report";
            var reportRequest = new CampaignPerformanceReportRequest
            {
                Aggregation = ReportAggregation.Daily,
                Time = GetReportTime(startDate, endDate),
                Scope = new AccountThroughCampaignReportScope
                {
                    AccountIds = new[] { accountId },
                    Campaigns = null,
                },
                Columns = new[]
                {
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
                    CampaignPerformanceReportColumn.CampaignName,
                },
            };
            InitCommonFieldsInReportRequest(reportRequest, reportName);
            return reportRequest;
        }

        /// <summary>
        /// Returns parameters for product dimension performance report.
        /// </summary>
        /// <param name="accountId">Account ID on the Bing portal.</param>
        /// <param name="startDate">Start date.</param>
        /// <param name="endDate">End date.</param>
        /// <returns>Prepared report request.</returns>
        public static ReportRequest GetProductDimensionReportRequest(long accountId, DateTime startDate, DateTime endDate)
        {
            const string reportName = "Product Dimension Performance Report";
            var reportRequest = new ProductDimensionPerformanceReportRequest
            {
                Aggregation = ReportAggregation.Daily,
                Time = GetReportTime(startDate, endDate),
                Scope = new AccountThroughAdGroupReportScope
                {
                    AccountIds = new[] { accountId },
                    AdGroups = null,
                    Campaigns = null,
                },
                Columns = new[]
                {
                    ProductDimensionPerformanceReportColumn.MerchantProductId,
                    ProductDimensionPerformanceReportColumn.TimePeriod,
                    ProductDimensionPerformanceReportColumn.Impressions,
                    ProductDimensionPerformanceReportColumn.Clicks,
                    ProductDimensionPerformanceReportColumn.Conversions,
                    ProductDimensionPerformanceReportColumn.Spend,
                    ProductDimensionPerformanceReportColumn.Revenue,
                    ProductDimensionPerformanceReportColumn.AccountName,
                    ProductDimensionPerformanceReportColumn.AccountNumber,
                    ProductDimensionPerformanceReportColumn.CampaignId,
                    ProductDimensionPerformanceReportColumn.CampaignName,
                },
            };
            InitCommonFieldsInReportRequest(reportRequest, reportName);
            return reportRequest;
        }

        /// <summary>
        /// Returns parameters for goals and funnels report.
        /// </summary>
        /// <param name="accountId">Account ID on the Bing portal.</param>
        /// <param name="startDate">Start date.</param>
        /// <param name="endDate">End date.</param>
        /// <returns>Prepared report request.</returns>
        public static ReportRequest GetGoalsReportRequest(long accountId, DateTime startDate, DateTime endDate)
        {
            const string reportName = "Goals And Funnels Report";
            var reportRequest = new GoalsAndFunnelsReportRequest
            {
                Aggregation = ReportAggregation.Daily,
                Time = GetReportTime(startDate, endDate),
                Scope = new AccountThroughAdGroupReportScope
                {
                    AccountIds = new[] { accountId },
                    AdGroups = null,
                    Campaigns = null,
                },
                Columns = new[]
                {
                    GoalsAndFunnelsReportColumn.TimePeriod,
                    GoalsAndFunnelsReportColumn.GoalId,
                    GoalsAndFunnelsReportColumn.Goal,
                    GoalsAndFunnelsReportColumn.Conversions,
                    GoalsAndFunnelsReportColumn.Revenue,
                    GoalsAndFunnelsReportColumn.AccountId,
                    GoalsAndFunnelsReportColumn.AccountName,
                    GoalsAndFunnelsReportColumn.AccountNumber,
                    GoalsAndFunnelsReportColumn.CampaignId,
                    GoalsAndFunnelsReportColumn.CampaignName,
                },
            };
            InitCommonFieldsInReportRequest(reportRequest, reportName);
            return reportRequest;
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
                CustomDateRangeEnd = ConvertToDate(endDate),
            };
            return time;
        }

        private static Date ConvertToDate(DateTime dateTime)
        {
            var date = new Date
            {
                Year = dateTime.Year,
                Month = dateTime.Month,
                Day = dateTime.Day,
            };
            return date;
        }
    }
}
