using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;
using Google.Api.Ads.AdWords.Lib;
using Google.Api.Ads.AdWords.Util.Reports;
using Google.Api.Ads.AdWords.v201809;
using Google.Api.Ads.Common.Util.Reports;

namespace CakeExtracter.Etl.SearchMarketing.Extracters
{
    public class AdWordsApiExtracter : Extracter<Dictionary<string, string>>
    {
        private const string VERSION = "v201809";
        private readonly string reportFilePath = ConfigurationManager.AppSettings["AdWordsReportFilePath"];

        private readonly string clientCustomerId;
        private readonly DateTime beginDate;
        private readonly DateTime endDate;
        private readonly bool includeClickType;

        private readonly bool clickAssistConvStats;
        private readonly bool conversionTypeStats;

        public AdWordsApiExtracter(
            string clientCustomerId,
            Common.DateRange dateRange,
            bool includeClickType,
            bool clickAssistConvStats = false,
            bool conversionTypeStats = false) // choose zero or one of these
        {
            this.clientCustomerId = clientCustomerId;
            this.beginDate = dateRange.FromDate;
            this.endDate = dateRange.ToDate;
            this.includeClickType = includeClickType;

            // The first one that's true will be used; if none, it'll be a standard report
            this.clickAssistConvStats = clickAssistConvStats;
            this.conversionTypeStats = conversionTypeStats;
        }

        /// <inheritdoc/>
        protected override void Extract()
        {
            var extra = string.Empty;
            if (clickAssistConvStats || conversionTypeStats || includeClickType)
            {
                extra = $" [{(clickAssistConvStats ? "ClickAssistConvStats" : (conversionTypeStats ? "ConversionTypeStats" : ""))}{(includeClickType ? " w/ClickType" : "")}]";
            }

            Logger.Info(
                $"Extracting SearchDailySummaries{extra} from AdWords API for {clientCustomerId} from {beginDate.ToShortDateString()} to {endDate.ToShortDateString()}");

            try
            {
                string[] fields;
                if (clickAssistConvStats)
                {
                    fields = GetFields_ClickAssistedConversions();
                }
                else if (conversionTypeStats)
                {
                    fields = GetFields_ConversionTypeStats();
                }
                else
                {
                    fields = GetFields_StandardReport();
                }

                DownloadAdWordsXmlReport(fields);
                var reportRows = EnumerateAdWordsXmlReportRows(this.reportFilePath);
                Add(reportRows);
            }
            catch (Exception ex)
            {
                Logger.Warn("Extraction error: {0}", ex.Message);
            }
            End();
        }

        private string[] GetFields_StandardReport()
        {
            var fieldsList = new List<string>(new[]
            { // "XML ATTRIBUTE"
                "AccountDescriptiveName", // account
                "AccountCurrencyCode", // currency
                "ExternalCustomerId",  // customerID
                "AccountTimeZone",   // timeZone
                "CampaignId",    // campaignID
                "CampaignName",  // campaign
                "CampaignStatus",// campaignStatus (used for filtering)
                "AdvertisingChannelSubType", //advertisingSubChannel (used for filtering)
                "Date",        // day
                "Impressions", // impressions
                "Clicks",      // clicks
                // "ConvertedClicks", // convertedClicks
                "Conversions", // conversions
                "Cost", // cost
                "ConversionValue", // totalConvValue
                "AdNetworkType1", // network
                "Device", // device
                "ViewThroughConversions", // viewThroughConv
                
            });
            if (includeClickType)
            {
                fieldsList.Add("ClickType"); // clickType
            }
            else
            {
                fieldsList.AddRange(
                    new[]
                    {
                        "VideoQuartile100Rate",
                        "VideoQuartile75Rate",
                        "VideoQuartile50Rate",
                        "VideoQuartile25Rate",
                        "ActiveViewImpressions",
                        "VideoViews",
                    });
            }
            return fieldsList.ToArray();
        }

        private string[] GetFields_ClickAssistedConversions()
        {
            var fieldsList = new List<string>(new[]
            {
                "AccountDescriptiveName",
                "AccountCurrencyCode",
                "ExternalCustomerId",
                "CampaignId",
                "CampaignName",
                "Date",
                "AdNetworkType1",
                "ClickAssistedConversions", // clickAssistedConv
                "ClickAssistedConversionValue", // clickAssistedConvValue
            });
            if (includeClickType)
            {
                fieldsList.Add("ClickType"); // clickType
            }
            return fieldsList.ToArray();
        }

        private string[] GetFields_ConversionTypeStats()
        {
            var fieldsList = new List<string>(new[]
            {
                "AccountDescriptiveName",
                "AccountCurrencyCode",
                "ExternalCustomerId",
                "CampaignId",
                "CampaignName",
                "Date",
                "AdNetworkType1",
                "Device",
                "ConversionTypeName", // conversionName
                "Conversions",
                "ConversionValue",
                "AllConversions", // allConv
                "AllConversionValue", // allConvValue
                // "ViewThroughConversions" // viewThroughConv
            });

            // if (includeClickType)
            // {
            //    fieldsList.Add("ClickType"); // clickType
            // }
            return fieldsList.ToArray();
        }

        private void DownloadAdWordsXmlReport(string[] fieldsList)
        {
            var definition = new ReportDefinition
            {
                reportName = "CAMPAIGN_PERFORMANCE_REPORT",
                reportType = ReportDefinitionReportType.CAMPAIGN_PERFORMANCE_REPORT,
                downloadFormat = DownloadFormat.XML,
                dateRangeType = ReportDefinitionDateRangeType.CUSTOM_DATE,
                selector = new Selector
                {
                    dateRange = new DateRange
                    {
                        min = this.beginDate.ToString("yyyyMMdd"),
                        max = this.endDate.ToString("yyyyMMdd"),
                    },
                    fields = fieldsList,
                    predicates = new[]
                    {
                        new Predicate
                        {
                            field = "CampaignStatus",
                            @operator = PredicateOperator.IN,
                            values = new[] { "ENABLED", "PAUSED", "REMOVED" },
                        },
                        new Predicate
                        {
                            field = "AdvertisingChannelSubType",
                            @operator = PredicateOperator.NOT_IN,
                            values = new[] { "SEARCH_EXPRESS", "DISPLAY_EXPRESS" },
                        },

                        // For Megabus conversion fix. Also comment out Impressions, Clicks and Cost above
                        // new Predicate
                        // {
                        //    field = "ConversionTypeName",
                        //    @operator = PredicateOperator.NOT_EQUALS,
                        //    values = new string[] { "Transactions (us.megabus.com)" }
                        // }
                    },
                },
            };

            try
            {
                var user = new AdWordsUser();
                ((AdWordsAppConfig)user.Config).ClientCustomerId = this.clientCustomerId; // "999-213-1770" is RamJet
                var utilities = new ReportUtilities(user, VERSION, definition);
                using (ReportResponse response = utilities.GetResponse())
                {
                    response.Save(this.reportFilePath);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to download AdWords report: {ex}");
            }
        }

        private static IEnumerable<Dictionary<string, string>> EnumerateAdWordsXmlReportRows(string xmlFilePath)
        {
            using (var reader = XmlReader.Create(xmlFilePath))
            {
                var columnNames = new List<string>();
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            switch (reader.Name)
                            {
                                case "column":
                                    if (reader.MoveToAttribute("name"))
                                    {
                                        columnNames.Add(reader.Value);
                                    }
                                    break;
                                case "row":
                                    {
                                        var row = new Dictionary<string, string>();
                                        foreach (var columnName in columnNames)
                                        {
                                            if (reader.MoveToAttribute(columnName))
                                            {
                                                row.Add(reader.Name, reader.Value);
                                            }
                                            else
                                                throw new Exception("could not move to column " + columnName);
                                        }
                                        yield return row;
                                    }
                                    break;
                            }
                            break;
                    }
                }
            }
        }
    }
}
