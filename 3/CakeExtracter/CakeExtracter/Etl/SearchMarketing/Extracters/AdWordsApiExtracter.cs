using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;
using Google.Api.Ads.AdWords.Lib;
using Google.Api.Ads.AdWords.Util.Reports;
using Google.Api.Ads.AdWords.v201406;

namespace CakeExtracter.Etl.SearchMarketing.Extracters
{
    public class AdWordsApiExtracter : Extracter<Dictionary<string, string>>
    {
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
            {
                "AccountDescriptiveName",
                "AccountCurrencyCode",
                "ExternalCustomerId",
                "AccountTimeZoneId",
                "CampaignId",
                "CampaignName",
                "CampaignStatus",
                "Date",
                "Impressions",

                "Clicks",
                "Conversions",
                "Cost",
                "Ctr",
                "ConversionValue"
            });

            fieldsList.Add("AdNetworkType1");
            fieldsList.Add("Device");

            if (includeClickType)
            {
                fieldsList.Add("ClickType");
            }

            string duringString = string.Format("DURING {0},{1}", this.beginDate.ToString("yyyyMMdd"), this.endDate.ToString("yyyyMMdd"));

            string queryFormat = "SELECT {0} FROM CAMPAIGN_PERFORMANCE_REPORT WHERE Impressions > 0 AND CampaignStatus IN [ENABLED, PAUSED] {1}";

            string query = string.Format(queryFormat, string.Join(",", fieldsList), duringString);

            try
            {
                var user = new AdWordsUser();
                ((AdWordsAppConfig)user.Config).ClientCustomerId = this.clientCustomerId; //"999-213-1770" is RamJet
                var utilities = new ReportUtilities(user);
                utilities.ReportVersion = "v201406";
                utilities.DownloadClientReport(query, DownloadFormat.XML.ToString(), this.reportFilePath);
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
