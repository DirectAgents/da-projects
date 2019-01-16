using CakeExtractor.SeleniumApplication.Models.Vcd;
using System.Collections.Generic;

namespace CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportDownloading.Constants
{
    internal class RequestBodyConstants
    {
        public const string ReportDatesVisibleFilterName = "Viewing";

        public const string StartDateReportParameter = "periodStartDay";

        public const string EndDateReportParameter = "periodEndDay";

        public const string ReportId = "salesDiagnosticDetail";

        public const string RequestBodyFormat = "application/json";

        public static Dictionary<string, List<string>> GetInitialVisibleFilters()
        {
            return new Dictionary<string, List<string>>()
                    {
                        { "Program", new List<string>{ "Amazon Retail" } },
                        { "Distributor View", new List<string>{ "Manufacturing" } },
                        { "Sales View", new List<string>{ "Shipped Revenue" } },
                        { "Category", new List<string>{ "All" } },
                        { "Subcategory", new List<string>{ "All" }},
                        { "Brand", new List<string>{ "All" }},
                        { "Search for ASINs or Keywords", new List<string>{ "All" }},
                        { "Reporting Range", new List<string>{ "Daily" }},
                        { ReportDatesVisibleFilterName, new List<string>{ ""} }, // Should be filled dynamically with report day date
                        { "View by", new List<string>{ "ASIN"} },
                        { "Add", new List<string>{ "Subcategory", "Category" } }
                    };
        }

        public static List<ReportParameter> GetReportParameters()
        {
            return new List<ReportParameter>
            {
                new ReportParameter
                {
                    parameterId = "distributorView",
                    values = new List<Value>{ new Value { val= "manufacturer" } }
                },
                new ReportParameter
                {
                    parameterId = "viewFilter",
                    values = new List<Value>{ new Value { val= "shippedRevenueLevel" } }
                },
                new ReportParameter
                {
                    parameterId = "asin",
                    values = new List<Value>{ new Value { val= "ALL" } }
                },
                 new ReportParameter
                {
                    parameterId = "aggregationFilter",
                    values = new List<Value>{ new Value { val= "ASINLevel" } }
                },
                new ReportParameter
                {
                    parameterId = StartDateReportParameter,
                    values = new List<Value>{ new Value { val= "" } } // Should be filled dynamically with report day date
                },
                new ReportParameter
                {
                    parameterId = EndDateReportParameter,
                    values = new List<Value>{ new Value { val= "" } } /// Should be filled dynamically with report day date
                },
                new ReportParameter
                {
                    parameterId = "isPeriodToDate",
                    values = new List<Value>{ new Value { val= false } }
                },
                new ReportParameter
                {
                    parameterId = "isCustomDateRange",
                    values = new List<Value>{ new Value { val= "false" } }
                },
                new ReportParameter
                {
                    parameterId = "hideLastYear",
                    values = new List<Value>{ new Value { val= false } }
                },
                new ReportParameter
                {
                    parameterId = "period",
                    values = new List<Value>{ new Value { val= "DAILY" } }
                },
                new ReportParameter
                {
                    parameterId = "dataRefreshDate",
                    values = new List<Value>{ new Value { val= "0001703861052" } }
                },
                new ReportParameter
                {
                    parameterId = "categoryId",
                    values = new List<Value>{ new Value { val= "ALL" } }
                },
                new ReportParameter
                {
                    parameterId = "subcategoryId",
                    values = new List<Value>{ new Value { val= "ALL" } }
                },
                new ReportParameter
                {
                    parameterId = "brandId",
                    values = new List<Value>{ new Value { val= "ALL" } }
                },
                new ReportParameter
                {
                    parameterId = "parentASINVisibility",
                    values = new List<Value>{ new Value { val= false } }
                },
                new ReportParameter
                {
                    parameterId = "eanVisibility",
                    values = new List<Value>{ new Value { val= false } }
                },
                new ReportParameter
                {
                    parameterId = "isbn13Visibility",
                    values = new List<Value>{ new Value { val= false } }
                },
                new ReportParameter
                {
                    parameterId = "upcVisibility",
                    values = new List<Value>{ new Value { val= false } }
                },
                new ReportParameter
                {
                    parameterId = "janVisibility",
                    values = new List<Value>{ new Value { val= false } }
                },
                new ReportParameter
                {
                    parameterId = "brandVisibility",
                    values = new List<Value>{ new Value { val= false } }
                },
                new ReportParameter
                {
                    parameterId = "brandCodeVisibility",
                    values = new List<Value>{ new Value { val= false } }
                },
                new ReportParameter
                {
                    parameterId = "catVisibility",
                    values = new List<Value>{ new Value { val= true } }
                },
                new ReportParameter
                {
                    parameterId = "apparelSizeVisibility",
                    values = new List<Value>{ new Value { val= false } }
                },
                new ReportParameter
                {
                    parameterId = "apparelSizeWidthVisibility",
                    values = new List<Value>{ new Value { val= false } }
                },
                new ReportParameter
                {
                    parameterId = "authorVisibility",
                    values = new List<Value>{ new Value { val= false } }
                },
                new ReportParameter
                {
                    parameterId =  "bindingVisibility",
                    values = new List<Value>{ new Value { val= false } }
                },
                new ReportParameter
                {
                    parameterId = "catalogNumberVisibility",
                    values = new List<Value>{ new Value { val= false } }
                },
                new ReportParameter
                {
                    parameterId = "colorVisibility",
                    values = new List<Value>{ new Value { val= false } }
                },
                new ReportParameter
                {
                    parameterId =  "colorCountVisibility",
                    values = new List<Value>{ new Value { val= false } }
                },
                new ReportParameter
                {
                    parameterId = "manufactureOnDemandVisibility",
                    values = new List<Value>{ new Value { val= false } }
                },
                new ReportParameter
                {
                    parameterId = "listPriceVisibility",
                    values = new List<Value>{ new Value { val= false } }
                },
                new ReportParameter
                {
                    parameterId = "modelStyleVisibility",
                    values = new List<Value>{ new Value { val= false } }
                },
                new ReportParameter
                {
                    parameterId = "productGroupVisibility",
                    values = new List<Value>{ new Value { val= false } }
                },
                new ReportParameter
                {
                    parameterId = "releaseDateVisibility",
                    values = new List<Value>{ new Value { val= false } }
                },
            };
        }
    }
}
