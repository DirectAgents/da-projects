using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;
using Google.Api.Ads.AdWords.Lib;
using Google.Api.Ads.AdWords.Util.Reports;
using Google.Api.Ads.AdWords.v201502;
using Google.Api.Ads.Common.Util.Reports;

namespace CakeExtracter.Etl.SearchMarketing.Extracters
{
    public class AdWordsApiExtracter : Extracter<Dictionary<string, string>>
    {
        const string VERSION = "v201502";
        private readonly string reportFilePath = ConfigurationManager.AppSettings["AdWordsReportFilePath"];

        private readonly string clientCustomerId;
        private readonly DateTime beginDate;
        private readonly DateTime endDate;
        private readonly bool includeClickType;

        public AdWordsApiExtracter(string clientCustomerId, CakeExtracter.Common.DateRange dateRange, bool includeClickType = false)
        {
            this.clientCustomerId = clientCustomerId;
            this.beginDate = dateRange.FromDate;
            this.endDate = dateRange.ToDate;
            this.includeClickType = includeClickType;
        }

        protected override void Extract()
        {
            Logger.Info("Extracting SearchDailySummaries from AdWords API for {0} from {1} to {2}",
                this.clientCustomerId, this.beginDate.ToShortDateString(), this.endDate.ToShortDateString());

            try
            {
                DownloadAdWordsXmlReport();
                var reportRows = EnumerateAdWordsXmlReportRows(this.reportFilePath);
                Add(reportRows);
            }
            catch (Exception ex)
            {
                Logger.Info("Extraction error: {0}", ex.Message);
            }
            End();
        }

        private void DownloadAdWordsXmlReport()
        {
            var fieldsList = new List<string>(new string[]
            {                             // "XML ATTRIBUTE"
                "AccountDescriptiveName", // account
                "AccountCurrencyCode", // currency
                "ExternalCustomerId",  // customerID
                "AccountTimeZoneId",   // timeZone
                "CampaignId",    // campaignID
                "CampaignName",  // campaign
                "CampaignStatus",// campaignStatus
                "Date",        // day
                "Impressions", // impressions
                "Clicks",      // clicks
                "ConvertedClicks", // convertedClicks
                "ConversionsManyPerClick", // conversions
                "Cost", // cost
                //"Ctr", REMOVE
                "ConversionValue" // totalConvValue
            });

            fieldsList.Add("AdNetworkType1"); // network
            fieldsList.Add("Device"); // device

            if (includeClickType)
            {
                fieldsList.Add("ClickType"); // clickType
            }

            var definition = new ReportDefinition
            {
                reportName = "CAMPAIGN_PERFORMANCE_REPORT",
                reportType = ReportDefinitionReportType.CAMPAIGN_PERFORMANCE_REPORT,
                downloadFormat = DownloadFormat.XML,
                dateRangeType = ReportDefinitionDateRangeType.CUSTOM_DATE,
                selector = new Selector
                {
                    dateRange = new DateRange {
                        min = this.beginDate.ToString("yyyyMMdd"),
                        max = this.endDate.ToString("yyyyMMdd")
                    },
                    fields = fieldsList.ToArray(),
                    predicates = new Predicate[] {
                        new Predicate {
                            field = "CampaignStatus",
                            @operator = PredicateOperator.IN,
                            values = new string[] { "ENABLED","PAUSED" }
                        }
                    }
                },
                includeZeroImpressions = false
            };

            try
            {
                var user = new AdWordsUser();
                ((AdWordsAppConfig)user.Config).ClientCustomerId = this.clientCustomerId; //"999-213-1770" is RamJet
                var utilities = new ReportUtilities(user, VERSION, definition);
                using (ReportResponse response = utilities.GetResponse())
                {
                    response.Save(this.reportFilePath);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to download AdWords report.", ex);
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
