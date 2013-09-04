﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;
using Google.Api.Ads.AdWords.Lib;
using Google.Api.Ads.AdWords.Util.Reports;
using Google.Api.Ads.AdWords.v201306;

namespace CakeExtracter.Etl.SearchMarketing.Extracters
{
    public class AdWordsApiExtracter : Extracter<Dictionary<string, string>>
    {
        private readonly string reportFilePath = ConfigurationManager.AppSettings["AdWordsReportFilePath"];

        private readonly string clientCustomerId;
        private readonly DateTime beginDate;
        private readonly DateTime endDate;

        public AdWordsApiExtracter(string clientCustomerId, DateTime beginDate, DateTime endDate)
        {
            this.clientCustomerId = clientCustomerId;
            this.beginDate = beginDate;
            this.endDate = endDate;
        }

        protected override void Extract()
        {
            Logger.Info("Extracting SearchDailySummaries from AdWords API for {0} from {1} to {2}",
                this.clientCustomerId, this.beginDate.ToShortDateString(), this.endDate.ToShortDateString());

            DownloadAdWordsXmlReport();
            var reportRows = EnumerateAdWordsXmlReportRows(this.reportFilePath);
            Add(reportRows);
            End();
        }

        private void DownloadAdWordsXmlReport()
        {
            var fieldsArray = new string[]
            {
                "AccountDescriptiveName",
                "AccountCurrencyCode",
                "AccountId",
                "AccountTimeZoneId",
                "CampaignId",
                "CampaignName",
                "CampaignStatus",
                "Date",
                "AdNetworkType1",
                "Device",
                "ClickType",
                "Impressions",
                "Clicks",
                "Conversions",
                "Cost",
                "Ctr",
                "ConversionValue",
            };

            string duringString = string.Format("DURING {0},{1}", this.beginDate.ToString("yyyyMMdd"), this.endDate.ToString("yyyyMMdd"));

            string queryFormat = "SELECT {0} FROM CAMPAIGN_PERFORMANCE_REPORT WHERE Impressions > 0 AND CampaignStatus IN [ACTIVE, PAUSED] {1}";

            string query = string.Format(queryFormat, string.Join(",", fieldsArray), duringString);

            try
            {
                var user = new AdWordsUser();
                ((AdWordsAppConfig)user.Config).ClientCustomerId = this.clientCustomerId; //"999-213-1770" is RamJet
                var utilities = new ReportUtilities(user);
                utilities.ReportVersion = "v201306";
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
