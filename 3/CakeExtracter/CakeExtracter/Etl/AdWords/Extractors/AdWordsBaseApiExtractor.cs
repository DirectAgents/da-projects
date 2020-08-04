using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;
using CakeExtracter.Etl.AdWords.Exceptions;
using Google.Api.Ads.AdWords.Lib;
using Google.Api.Ads.AdWords.Util.Reports;
using Google.Api.Ads.AdWords.v201809;
using Google.Api.Ads.Common.Util.Reports;

namespace CakeExtracter.Etl.AdWords.Extractors
{
    /// <summary>
    /// Base AdWords API extractor.
    /// </summary>
    internal abstract class AdWordsBaseApiExtractor : Extracter<Dictionary<string, string>>
    {
        private const string Version = "v201809";

        private const string DateFormat = "yyyyMMdd";

        private readonly string reportFilePath = ConfigurationManager.AppSettings["AdWordsReportFilePath"];

        private readonly string clientCustomerId;

        private readonly DateTime beginDate;

        private readonly DateTime endDate;

        /// <summary>
        /// Action for exception of failed extraction.
        /// </summary>
        public event Action<AdwordsFailedEtlException> ProcessFailedExtraction;

        /// <summary>
        /// Gets exact extracted stats type.
        /// </summary>
        protected abstract string StatsType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdWordsBaseApiExtractor"/> class.
        /// </summary>
        /// <param name="clientCustomerId">Account Code for the extracting data.</param>
        /// <param name="dateRange">Date range for the extracting data.</param>
        public AdWordsBaseApiExtractor(string clientCustomerId, Common.DateRange dateRange)
        {
            this.clientCustomerId = clientCustomerId;
            beginDate = dateRange.FromDate;
            endDate = dateRange.ToDate;
        }

        /// <inheritdoc/>
        protected override void Extract()
        {
            try
            {
                Logger.Info($"Extracting SearchDailySummaries[{StatsType}] from AdWords API for {clientCustomerId} " +
                            $"from {beginDate.ToShortDateString()} to {endDate.ToShortDateString()}");

                var fields = GetReportFields();
                DownloadXmlReport(fields);
                var reportRows = EnumerateXmlReportRows(reportFilePath);
                Add(reportRows);
            }
            catch (Exception ex)
            {
                ProcessFailedStatsExtraction(ex);
            }
            finally
            {
                End();
            }
        }

        /// <summary>
        /// Gets report fiealds depending on the stats type.
        /// </summary>
        /// <returns>Returns array of report fields.</returns>
        protected abstract string[] GetReportFields();

        private void DownloadXmlReport(string[] fieldsList)
        {
            try
            {
                var definition = CreateReportDefenition(fieldsList);
                var user = new AdWordsUser();
                ((AdWordsAppConfig)user.Config).ClientCustomerId = clientCustomerId;
                var utilities = new ReportUtilities(user, Version, definition);

                using (ReportResponse response = utilities.GetResponse())
                {
                    response.Save(reportFilePath);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to download AdWords report: {ex}");
            }
        }

        private ReportDefinition CreateReportDefenition(string[] fieldsList)
        {
            return new ReportDefinition
            {
                reportName = "CAMPAIGN_PERFORMANCE_REPORT",
                reportType = ReportDefinitionReportType.CAMPAIGN_PERFORMANCE_REPORT,
                downloadFormat = DownloadFormat.XML,
                dateRangeType = ReportDefinitionDateRangeType.CUSTOM_DATE,
                selector = new Selector
                {
                    dateRange = new DateRange
                    {
                        min = beginDate.ToString(DateFormat),
                        max = endDate.ToString(DateFormat),
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
                    },
                },
            };
        }

        // TODO: Refactor the legacy logic of the xml parsing using Linq to Xml.
        private static IEnumerable<Dictionary<string, string>> EnumerateXmlReportRows(string xmlFilePath)
        {
            using (var reader = XmlReader.Create(xmlFilePath, new XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit }))
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
                                            {
                                                throw new Exception("could not move to column " + columnName);
                                            }
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

        private void ProcessFailedStatsExtraction(Exception e)
        {
            Logger.Warn("Extraction error: {0}", e.Message);
            var exception = new AdwordsFailedEtlException(beginDate, endDate, clientCustomerId, StatsType, e);
            ProcessFailedExtraction?.Invoke(exception);
        }
    }
}
