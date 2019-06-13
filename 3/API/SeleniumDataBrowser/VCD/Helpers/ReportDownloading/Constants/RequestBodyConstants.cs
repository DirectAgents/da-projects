using System;
using System.Collections.Generic;
using SeleniumDataBrowser.VCD.Helpers.ReportDownloading.Models;
using SeleniumDataBrowser.Helpers;

namespace SeleniumDataBrowser.VCD.Helpers.ReportDownloading.Constants
{
    internal class RequestBodyConstants
    {
        public const string ReportId = "salesDiagnosticDetail";

        public const string RequestBodyFormat = "application/json";

        public const string ShippedRevenueReportLevel = "shippedRevenueLevel";

        public const string ShippedCogsLevel = "shippedCOGSLevel";

        public const string OrderedRevenueLevel = "orderedRevenueLevel";

        public const string ShippedRevenueSalesView = "Shipped Revenue";

        public const string ShippedCogsSalesView = "Shipped COGS";

        public const string OrderedRevenueView = "Ordered Revenue";

        public static Dictionary<string, List<string>> GetInitialVisibleFilters(string reportDates, string salesViewName)
        {
            return new Dictionary<string, List<string>>()
            {
                {"Program", new List<string> {"Amazon Retail"}},
                {"Distributor View", new List<string> {"Manufacturing"}},
                {"Sales View", new List<string> {salesViewName}},
                {"Category", new List<string> {"All"}},
                {"Subcategory", new List<string> {"All"}},
                {"Brand", new List<string> {"All"}},
                {"Search for ASINs or Keywords", new List<string> {"All"}},
                {"Reporting Range", new List<string> {"Daily"}},
                {"Viewing", new List<string> {reportDates}},
                {"View by", new List<string> {"ASIN"}},
                {
                    "Add",
                    new List<string>
                    {
                        "Subcategory", "Category", "Parent ASIN", "EAN", "UPC", "Brand", "Apparel Size",
                        "Apparel Size Width", "Binding", "Color", "Model / Style Number", "Release Date"
                    }
                }
            };
        }

        public static List<ReportParameter> GetReportParameters(string startDate, string endDate, string reportLevel)
        {
            var isFullSetOfMetrics = reportLevel == ShippedRevenueReportLevel; 
            return new List<ReportParameter>
            {
                new ReportParameter
                {
                    parameterId = "distributorView",
                    values = new List<Value>{ new Value { val = "manufacturer" } }
                },
                new ReportParameter
                {
                    parameterId = "viewFilter",
                    values = new List<Value>{ new Value { val = reportLevel } }
                },
                new ReportParameter
                {
                    parameterId = "productView",
                    values = new List<Value>{ new Value { val = "allAsins" } }
                },
                new ReportParameter
                {
                    parameterId = "asin",
                    values = new List<Value>{ new Value { val = "ALL" } }
                },
                 new ReportParameter
                {
                    parameterId = "aggregationFilter",
                    values = new List<Value>{ new Value { val = "ASINLevel" } }
                },
                new ReportParameter
                {
                    parameterId = "periodStartDay",
                    values = new List<Value>{ new Value { val = startDate } } // Should be filled dynamically with report day date
                },
                new ReportParameter
                {
                    parameterId = "periodEndDay",
                    values = new List<Value>{ new Value { val = endDate } } // Should be filled dynamically with report day date
                },
                new ReportParameter
                {
                    parameterId = "isPeriodToDate",
                    values = new List<Value>{ new Value { val = false } }
                },
                new ReportParameter
                {
                    parameterId = "isCustomDateRange",
                    values = new List<Value>{ new Value { val = "false" } }
                },
                new ReportParameter
                {
                    parameterId = "hideLastYear",
                    values = new List<Value>{ new Value { val = false } }
                },
                new ReportParameter
                {
                    parameterId = "period",
                    values = new List<Value>{ new Value { val = "DAILY" } }
                },
                new ReportParameter
                {
                    parameterId = "dataRefreshDate",
                    values = new List<Value>{ new Value { val = TimeHelper.ConvertToUnixTimestamp(DateTime.Now).ToString() } } // value for retrieving the latest data
                },
                new ReportParameter
                {
                    parameterId = "categoryId",
                    values = new List<Value>{ new Value { val = "ALL" } }
                },
                new ReportParameter
                {
                    parameterId = "subcategoryId",
                    values = new List<Value>{ new Value { val = "ALL" } }
                },
                new ReportParameter
                {
                    parameterId = "brandId",
                    values = new List<Value>{ new Value { val = "ALL" } }
                },
                new ReportParameter
                {
                    parameterId = "parentASINVisibility",
                    values = new List<Value>{ new Value { val = isFullSetOfMetrics } }
                },
                new ReportParameter
                {
                    parameterId = "eanVisibility",
                    values = new List<Value>{ new Value { val = isFullSetOfMetrics } }
                },
                new ReportParameter
                {
                    parameterId = "isbn13Visibility",
                    values = new List<Value>{ new Value { val = false } }
                },
                new ReportParameter
                {
                    parameterId = "upcVisibility",
                    values = new List<Value>{ new Value { val = isFullSetOfMetrics } }
                },
                new ReportParameter
                {
                    parameterId = "janVisibility",
                    values = new List<Value>{ new Value { val = false } }
                },
                new ReportParameter
                {
                    parameterId = "brandVisibility",
                    values = new List<Value>{ new Value { val = isFullSetOfMetrics } }
                },
                new ReportParameter
                {
                    parameterId = "brandCodeVisibility",
                    values = new List<Value>{ new Value { val = false } }
                },
                new ReportParameter
                {
                    parameterId = "catVisibility",
                    values = new List<Value>{ new Value { val = isFullSetOfMetrics } }
                },
                new ReportParameter
                {
                    parameterId = "apparelSizeVisibility",
                    values = new List<Value>{ new Value { val = isFullSetOfMetrics } }
                },
                new ReportParameter
                {
                    parameterId = "apparelSizeWidthVisibility",
                    values = new List<Value>{ new Value { val = isFullSetOfMetrics } }
                },
                new ReportParameter
                {
                    parameterId = "authorVisibility",
                    values = new List<Value>{ new Value { val = false } }
                },
                new ReportParameter
                {
                    parameterId =  "bindingVisibility",
                    values = new List<Value>{ new Value { val = isFullSetOfMetrics } }
                },
                new ReportParameter
                {
                    parameterId = "catalogNumberVisibility",
                    values = new List<Value>{ new Value { val = false } }
                },
                new ReportParameter
                {
                    parameterId = "colorVisibility",
                    values = new List<Value>{ new Value { val = isFullSetOfMetrics } }
                },
                new ReportParameter
                {
                    parameterId =  "colorCountVisibility",
                    values = new List<Value>{ new Value { val = false } }
                },
                new ReportParameter
                {
                    parameterId = "manufactureOnDemandVisibility",
                    values = new List<Value>{ new Value { val = false } }
                },
                new ReportParameter
                {
                    parameterId = "listPriceVisibility",
                    values = new List<Value>{ new Value { val = false } }
                },
                new ReportParameter
                {
                    parameterId = "modelStyleVisibility",
                    values = new List<Value>{ new Value { val = isFullSetOfMetrics } }
                },
                new ReportParameter
                {
                    parameterId = "productGroupVisibility",
                    values = new List<Value>{ new Value { val = false } }
                },
                new ReportParameter
                {
                    parameterId = "releaseDateVisibility",
                    values = new List<Value>{ new Value { val = isFullSetOfMetrics } }
                },
            };
        }
    }
}
