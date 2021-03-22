using System;
using System.Collections.Generic;
using SeleniumDataBrowser.VCD.Helpers.ReportDownloading.Models;
using SeleniumDataBrowser.Helpers;
using System.Threading;

namespace SeleniumDataBrowser.VCD.Helpers.ReportDownloading.Constants
{
    /// <summary>
    /// Constants for preparing request body.
    /// </summary>
    internal class RequestBodyConstants
    {
        public const string RequestBodyFormat = "application/json";

        public const string ShippedRevenueReportLevel = "shippedRevenueLevel";

        public const string ShippedCogsLevel = "shippedCOGSLevel";

        public const string OrderedRevenueLevel = "orderedRevenueLevel";

        public const string GeographicSalesInsightLevel = "geographicSalesInsightLevel";

        public const string NetPPMLevel = "netPPMLevel";

        public const string RepeatPurchaseBehaviorLevel = "repeatPurchaseBehaviorLevel";

        public const string ShippedRevenueColumnId = "shippedrevenue";

        public const string ShippedCogsColumnId = "shippedcogs";

        public const string OrderedRevenueColumnId = "orderedrevenue";

        public const string HealthInventoryColumnId = "sellableonhandinventory";

        public const string CustomerReviewsColumnId = "numberofcustomerreviews";

        public const string GeographicSalesInsightColumnId = "shippedrevenue";

        public const string NetPPMColumnId = "netppmpercentoftotal";

        public const string RepeatPurchaseBehaviorColumnId = "orders";

        public const string WeeklyPeriod = "WEEKLY";

        public const string MonthlyPeriod = "MONTHLY";

        public const string YearlyPeriod = "YEARLY";

        public const string QuaterlyPeriod = "QUATERLY";

        /// <summary>
        /// Returns a list of sales diagnostic report parameters with values for request body.
        /// </summary>
        /// <param name="reportDate">Report date.</param>
        /// <param name="reportLevel">Report level.</param>
        /// <returns>List of report parameters.</returns>
        public static List<ReportParameter> GetSalesDiagnosticParameters(
            string reportDate,
            string reportLevel)
        {
            var isFullSetOfMetrics = reportLevel == ShippedRevenueReportLevel;
            var commonParams = GetCommonReportParameters(reportDate, reportDate, isFullSetOfMetrics);
            var isAllAsins = reportLevel == OrderedRevenueLevel;
            commonParams.AddRange(
                new List<ReportParameter>
                {
                    new ReportParameter
                    {
                        parameterId = "viewFilter",
                        values = new List<Value> { new Value { val = reportLevel } },
                    },
                    new ReportParameter
                    {
                        parameterId = "productView",
                        values = new List<Value> { new Value { val = isAllAsins ? "allAsins" : "kindleExcluded" } },
                    },
                    new ReportParameter
                    {
                        parameterId = "aggregationFilter",
                        values = new List<Value> { new Value { val = "ASINLevel" } },
                    },
                    new ReportParameter
                    {
                        parameterId = "distributorView",
                        values = new List<Value> { new Value { val = "manufacturer" } },
                    },
                });
            return commonParams;
        }

        /// <summary>
        /// Returns a list of inventory health report parameters with values for request body.
        /// </summary>
        /// <param name="reportDate">Report date.</param>
        /// <returns>List of report parameters.</returns>
        public static List<ReportParameter> GetInventoryHealthParameters(string reportDate)
        {
            var commonParams = GetCommonReportParameters(reportDate, reportDate, false);
            commonParams.AddRange(
                new List<ReportParameter>
                {
                    new ReportParameter
                    {
                        parameterId = "isFCAllowed",
                        values = new List<Value> { new Value { val = false } },
                    },
                    new ReportParameter
                    {
                        parameterId = "aggregationFilter",
                        values = new List<Value> { new Value { val = "ASINLevel" } },
                    },
                    new ReportParameter
                    {
                        parameterId = "distributorView",
                        values = new List<Value> { new Value { val = "manufacturer" } },
                    },
                });
            return commonParams;
        }

        /// <summary>
        /// Returns a list of customer reviews report parameters with values for request body.
        /// </summary>
        /// <param name="reportDate">Report date.</param>
        /// <returns>List of report parameters.</returns>
        public static List<ReportParameter> GetCustomerReviewsParameters(string reportDate)
        {
            const int sentimentNtileValue = 5;
            var commonParams = GetCommonReportParameters(reportDate, reportDate, false);
            commonParams.AddRange(
                new List<ReportParameter>
                {
                    new ReportParameter
                    {
                        parameterId = "sentimentASIN",
                        values = new List<Value> { new Value { val = string.Empty } },
                    },
                    new ReportParameter
                    {
                        parameterId = "sentimentNtile",
                        values = new List<Value> { new Value { val = sentimentNtileValue } },
                    },
                });
            return commonParams;
        }

        /// <summary>
        /// Returns a list of geographic sales insights report parameters with values for request body.
        /// </summary>
        /// <param name="reportDate">Report date.</param>
        /// <returns>List of report parameters.</returns>
        public static List<ReportParameter> GetGeographicSalesInsightsParameters(string reportDate)
        {
            var commonParams = GetCommonReportParameters(reportDate, reportDate, false);
            commonParams.AddRange(
                new List<ReportParameter> {
                new ReportParameter
                {
                    parameterId = "viewFilter",
                    values = new List<Value> {new Value { val = "shippedRevenueLevel" } },
                },
                new ReportParameter
                {
                    parameterId = "country",
                    values = new List<Value> { new Value { val = "US" } },
                },
                new ReportParameter
                {
                    parameterId = "state",
                    values = new List<Value> { new Value { val= "ALL" } },
                },

                new ReportParameter
                {
                    parameterId = "city",
                    values = new List<Value> { new Value { val = "ALL" } },
                },

                new ReportParameter
                {
                    parameterId = "zip",
                    values = new List<Value> { new Value { val = "ALL" } },
                },

                new ReportParameter
                {
                    parameterId = "aggregationFilter",
                    values = new List<Value> { new Value { val = "ASINLevel" } },
                },

                new ReportParameter
                {
                    parameterId = "countryCode",
                    values = new List<Value> { new Value { val = "US" } },
                },

                new ReportParameter
                {
                    parameterId = "parentASINVisibility",
                    values = new List<Value> { new Value { val = false } },
                },
                new ReportParameter
                {
                    parameterId = "eanVisibility",
                    values = new List<Value> { new Value { val = false } },
                },
            });
            return commonParams;
        }

        /// <summary>
        /// Returns a list of net ppm report parameters with values for request body.
        /// </summary>
        /// <param name="reportDate">Report date.</param>
        /// <returns>List of report parameters.</returns>
        public static List<ReportParameter> GetNetPpmParameters(string reportDate, string period)
        {
            string periodStartDay = "";
            string periodEndDay = "";
            DateTime lastPeriodDate;
            var culture = Thread.CurrentThread.CurrentCulture;

            switch (period)
            {
                case WeeklyPeriod:
                    lastPeriodDate = DateTime.Today.AddDays(-7);
                    var diff = lastPeriodDate.DayOfWeek - culture.DateTimeFormat.FirstDayOfWeek;
                    if (diff < 0)
                        diff += 7;
                    periodStartDay = lastPeriodDate.AddDays(-diff).Date.ToString("yyyyMMdd");
                    periodEndDay = lastPeriodDate.AddDays(-diff + 6).Date.ToString("yyyyMMdd");
                    break;
                case MonthlyPeriod:
                    lastPeriodDate = DateTime.Today.AddMonths(-1);
                    var firstDayOfMonth = new DateTime(lastPeriodDate.Year, lastPeriodDate.Month, 1);
                    var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                    periodStartDay = firstDayOfMonth.ToString("yyyyMMdd");
                    periodEndDay = lastDayOfMonth.ToString("yyyyMMdd");
                    break;
                case YearlyPeriod:
                    lastPeriodDate = DateTime.Today.AddYears(-1);
                    DateTime firstDayOfYear = new DateTime(lastPeriodDate.Year, 1, 1);
                    DateTime lastDayOfYear = new DateTime(lastPeriodDate.Year, 12, 31);
                    periodStartDay = firstDayOfYear.ToString("yyyyMMdd");
                    periodEndDay = lastDayOfYear.ToString("yyyyMMdd");
                    break;
            }
            var parameters = GetNetPpmReportParameters(period, periodStartDay, periodEndDay);
            return parameters;
        }

        /// <summary>
        /// Returns a list of repeat purchase behavior report parameters with values for request body.
        /// </summary>
        /// <param name="reportDate">Report date.</param>
        /// <returns>List of report parameters.</returns>
        public static List<ReportParameter> GetRepeatPurchaseBehaviorParameters(string reportDate, string period)
        {
            string periodEndDay = "";
            DateTime lastPeriodDate;
            var culture = Thread.CurrentThread.CurrentCulture;

            switch (period)
            {
                case MonthlyPeriod:
                    lastPeriodDate = DateTime.Today.AddMonths(-1);
                    var firstDayOfMonth = new DateTime(lastPeriodDate.Year, lastPeriodDate.Month, 1);
                    var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                    periodEndDay = lastDayOfMonth.ToString("yyyyMMdd");
                    break;
                case QuaterlyPeriod:
                    lastPeriodDate = DateTime.Today.AddMonths(-4);
                    int quarterNumber = (lastPeriodDate.Month - 1) / 3 + 1;
                    DateTime firstDayOfQuarter = new DateTime(lastPeriodDate.Year, (quarterNumber - 1) * 3 + 1, 1);
                    DateTime lastDayOfQuarter = firstDayOfQuarter.AddMonths(3).AddDays(-1);
                    periodEndDay = lastDayOfQuarter.ToString("yyyyMMdd");
                    break;
            }
            var parameters = GetRepeatPurchaseBehaviorReportParameters(period, periodEndDay);
            return parameters;
        }

        private static List<ReportParameter> GetNetPpmReportParameters(string period, string periodStartDay, string periodEndDay)
        {
            return new List<ReportParameter>
            {
                new ReportParameter
                {
                    parameterId = "asin",
                    values = new List<Value> { new Value { val = "ALL" } },
                },

                new ReportParameter
                {
                    parameterId = "aggregationFilter",
                    values = new List<Value> { new Value { val = "ASINLevel" } },
                },

                new ReportParameter
                {
                    parameterId = "period",
                    values = new List<Value> { new Value { val = period } },
                },

                new ReportParameter
                {
                    parameterId = "periodStartDay",
                    values = new List<Value> { new Value { val = periodStartDay } },
                },

                new ReportParameter
                {
                    parameterId = "periodEndDay",
                    values = new List<Value> { new Value { val = periodEndDay } },
                },

                new ReportParameter
                {
                    parameterId = "dataRefreshDate",
                    values = new List<Value> { new Value { val = "0000690138837" } },
                },
            };
        }

        private static List<ReportParameter> GetRepeatPurchaseBehaviorReportParameters(string period, string periodEndDay)
        {
            return new List<ReportParameter>
            {
                new ReportParameter
                {
                    parameterId = "asin",
                    values = new List<Value> { new Value { val = "ALL" } },
                },

                new ReportParameter
                {
                    parameterId = "aggregationFilter",
                    values = new List<Value> { new Value { val = "ASINLevel" } },
                },

                new ReportParameter
                {
                    parameterId = "period",
                    values = new List<Value> { new Value { val = period } },
                },

                new ReportParameter
                {
                    parameterId = "periodEndDay",
                    values = new List<Value> { new Value { val = periodEndDay } },
                },

                new ReportParameter
                {
                    parameterId = "dataRefreshDate",
                    values = new List<Value> { new Value { val = "0000437775652" } },
                },

                new ReportParameter
                {
                    parameterId = "categoryId",
                    values = new List<Value> { new Value { val = "ALL" } },
                },

                new ReportParameter
                {
                    parameterId = "subcategoryId",
                    values = new List<Value> { new Value { val = "ALL" } },
                },

                new ReportParameter
                {
                    parameterId = "brandId",
                    values = new List<Value> { new Value { val = "ALL" } },
                },

                new ReportParameter
                {
                    parameterId = "isbn13Visibility",
                    values = new List<Value> { new Value { val = false } },
                },

                new ReportParameter
                {
                    parameterId = "upcVisibility",
                    values = new List<Value> { new Value { val = false } },
                },

                new ReportParameter
                {
                    parameterId = "parentASINVisibility",
                    values = new List<Value> { new Value { val = false } },
                },

                new ReportParameter
                {
                    parameterId = "eanVisibility",
                    values = new List<Value> { new Value { val = false } },
                },

                new ReportParameter
                {
                    parameterId = "janVisibility",
                    values = new List<Value> { new Value { val = false } },
                },

                new ReportParameter
                {
                    parameterId = "brandVisibility",
                    values = new List<Value> { new Value { val = false } },
                },

                new ReportParameter
                {
                    parameterId = "brandCodeVisibility",
                    values = new List<Value> { new Value { val = false } },
                },

                new ReportParameter
                {
                    parameterId = "subcatVisibility",
                    values = new List<Value> { new Value { val = false } },
                },

                new ReportParameter
                {
                    parameterId = "catVisibility",
                    values = new List<Value> { new Value { val = false } },
                },

                new ReportParameter
                {
                    parameterId = "apparelSizeVisibility",
                    values = new List<Value> { new Value { val = false } },
                },

                new ReportParameter
                {
                    parameterId = "apparelSizeWidthVisibility",
                    values = new List<Value> { new Value { val = false } },
                },

                new ReportParameter
                {
                    parameterId = "authorVisibility",
                    values = new List<Value> { new Value { val = false } },
                },

                new ReportParameter
                {
                    parameterId = "bindingVisibility",
                    values = new List<Value> { new Value { val = false } },
                },

                new ReportParameter
                {
                    parameterId = "catalogNumberVisibility",
                    values = new List<Value> { new Value { val = false } },
                },

                new ReportParameter
                {
                    parameterId = "colorVisibility",
                    values = new List<Value> { new Value { val = false } },
                },

                new ReportParameter
                {
                    parameterId = "colorCountVisibility",
                    values = new List<Value> { new Value { val = false } },
                },

                new ReportParameter
                {
                    parameterId = "manufactureOnDemandVisibility",
                    values = new List<Value> { new Value { val = false } },
                },

                new ReportParameter
                {
                    parameterId = "listPriceVisibility",
                    values = new List<Value> { new Value { val = false } },
                },

                new ReportParameter
                {
                    parameterId = "modelStyleVisibility",
                    values = new List<Value> { new Value { val = false } },
                },

                new ReportParameter
                {
                    parameterId = "productGroupVisibility",
                    values = new List<Value> { new Value { val = false } },
                },

                new ReportParameter
                {
                    parameterId = "releaseDateVisibility",
                    values = new List<Value> { new Value { val = false } },
                },

            };
        }

        private static List<ReportParameter> GetCommonReportParameters(
            string startDate, string endDate, bool isFullSetOfMetrics)
        {
            return new List<ReportParameter>
            {
                new ReportParameter
                {
                    parameterId = "asin",
                    values = new List<Value> { new Value { val = "ALL" } },
                },
                new ReportParameter
                {
                    parameterId = "periodStartDay",

                    values = new List<Value> { new Value { val = startDate } },
                },
                new ReportParameter
                {
                    parameterId = "periodEndDay",

                    values = new List<Value> { new Value { val = endDate } },
                },
                new ReportParameter
                {
                    parameterId = "isPeriodToDate",
                    values = new List<Value> { new Value { val = false } },
                },
                new ReportParameter
                {
                    parameterId = "isCustomDateRange",
                    values = new List<Value> { new Value { val = "false" } },
                },
                new ReportParameter
                {
                    parameterId = "hideLastYear",
                    values = new List<Value> { new Value { val = false } },
                },
                new ReportParameter
                {
                    parameterId = "period",
                    values = new List<Value> { new Value { val = "DAILY" } },
                },
                new ReportParameter
                {
                    parameterId = "dataRefreshDate",

                    values = new List<Value> { new Value { val = "0000690138837" } },
                },
                new ReportParameter
                {
                    parameterId = "categoryId",
                    values = new List<Value> { new Value { val = "ALL" } },
                },
                new ReportParameter
                {
                    parameterId = "subcategoryId",
                    values = new List<Value> { new Value { val = "ALL" } },
                },
                new ReportParameter
                {
                    parameterId = "brandId",
                    values = new List<Value> { new Value { val = "ALL" } },
                },
                new ReportParameter
                {
                    parameterId = "isbn13Visibility",
                    values = new List<Value> { new Value { val = false } },
                },
                new ReportParameter
                {
                    parameterId = "upcVisibility",
                    values = new List<Value> { new Value { val = isFullSetOfMetrics } },
                },
                new ReportParameter
                {
                    parameterId = "janVisibility",
                    values = new List<Value> { new Value { val = false } },
                },
                new ReportParameter
                {
                    parameterId = "brandVisibility",
                    values = new List<Value> { new Value { val = isFullSetOfMetrics } },
                },
                new ReportParameter
                {
                    parameterId = "brandCodeVisibility",
                    values = new List<Value> { new Value { val = false } },
                },
                new ReportParameter
                {
                    parameterId = "subcatVisibility",
                    values = new List<Value> { new Value { val = false } },
                },
                new ReportParameter
                {
                    parameterId = "catVisibility",
                    values = new List<Value> { new Value { val = isFullSetOfMetrics } },
                },
                new ReportParameter
                {
                    parameterId = "apparelSizeVisibility",
                    values = new List<Value> { new Value { val = isFullSetOfMetrics } },
                },
                new ReportParameter
                {
                    parameterId = "apparelSizeWidthVisibility",
                    values = new List<Value> { new Value { val = isFullSetOfMetrics } },
                },
                new ReportParameter
                {
                    parameterId = "authorVisibility",
                    values = new List<Value> { new Value { val = false } },
                },
                new ReportParameter
                {
                    parameterId = "bindingVisibility",
                    values = new List<Value> { new Value { val = isFullSetOfMetrics } },
                },
                new ReportParameter
                {
                    parameterId = "catalogNumberVisibility",
                    values = new List<Value> { new Value { val = false } },
                },
                new ReportParameter
                {
                    parameterId = "colorVisibility",
                    values = new List<Value> { new Value { val = isFullSetOfMetrics } },
                },
                new ReportParameter
                {
                    parameterId = "colorCountVisibility",
                    values = new List<Value> { new Value { val = false } },
                },
                new ReportParameter
                {
                    parameterId = "manufactureOnDemandVisibility",
                    values = new List<Value> { new Value { val = false } },
                },
                new ReportParameter
                {
                    parameterId = "listPriceVisibility",
                    values = new List<Value> { new Value { val = false } },
                },
                new ReportParameter
                {
                    parameterId = "modelStyleVisibility",
                    values = new List<Value> { new Value { val = isFullSetOfMetrics } },
                },
                new ReportParameter
                {
                    parameterId = "productGroupVisibility",
                    values = new List<Value> { new Value { val = false } },
                },
                new ReportParameter
                {
                    parameterId = "releaseDateVisibility",
                    values = new List<Value> { new Value { val = isFullSetOfMetrics } },
                },
            };
        }

        /// <summary>
        /// Gets part of request body - report parameter of pagination.
        /// </summary>
        /// <param name="reportId">String identifier of kind of report.</param>
        /// <param name="pageIndex">Number of current page index.</param>
        /// <param name="pageSizeValue">Number of page size.</param>
        /// <returns>Report parameter of pagination.</returns>
        public static ReportPaginationWithOrderParameter GetReportPaginationWithOrderParameter(
            string reportId, int pageIndex, int pageSizeValue)
        {
            return new ReportPaginationWithOrderParameter
            {
                reportPagination = new ReportPagination
                {
                    pageIndex = pageIndex,
                    pageSize = pageSizeValue,
                },
                reportOrders = new List<ReportOrder>
                {
                    new ReportOrder
                    {
                        columnId = reportId,
                        ascending = false,
                    },
                },
            };
        }
    }
}
