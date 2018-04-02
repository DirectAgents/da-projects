using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using CakeExtracter.Common;
using Criteo;
using Criteo.CriteoAPI;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.Extracters
{
    public abstract class CriteoApiExtracter<T> : Extracter<T>
        where T : DatedStatsSummary
    {
        protected readonly string accountCode;
        protected readonly DateRange dateRange;
        protected readonly int timezoneOffset;

        protected CriteoUtility _criteoUtility;

        public CriteoApiExtracter(string accountCode, DateRange dateRange, int timezoneOffset)
        {
            this.accountCode = accountCode;
            this.dateRange = dateRange;
            this.timezoneOffset = timezoneOffset;
            _criteoUtility = new CriteoUtility(m => Logger.Info(m), m => Logger.Warn(m));
            _criteoUtility.SetCredentials(accountCode);
        }

        public campaign[] GetCampaigns()
        {
            return _criteoUtility.GetCampaigns();
        }
    }

    //NOTE: this modeled after ETL/SearchMarketing/Extracters/CriteoApiExtracter2
    //TODO: allow to pass in CriteoUtility

    public class CriteoStrategySummaryExtracter : CriteoApiExtracter<StrategySummary>
    {
        public CriteoStrategySummaryExtracter(string accountCode, DateRange dateRange, int timezoneOffset)
            : base(accountCode, dateRange, timezoneOffset)
        { }

        protected override void Extract()
        {
            Logger.Info("Extracting StrategySummaries from Criteo API for {0} from {1:d} to {2:d}",
                        this.accountCode, this.dateRange.FromDate, this.dateRange.ToDate);

            var adjustedBeginDate = this.dateRange.FromDate;
            var adjustedEndDate = this.dateRange.ToDate;
            if (this.timezoneOffset < 0)
                adjustedEndDate = adjustedEndDate.AddDays(1);
            else if (this.timezoneOffset > 0)
                adjustedBeginDate = adjustedBeginDate.AddDays(-1);

            var reportUrl = _criteoUtility.GetCampaignReport(adjustedBeginDate, adjustedEndDate, true);

            var dailySummaries = EnumerateRows(reportUrl);
            Add(dailySummaries);
            End();
        }

        private IEnumerable<StrategySummary> EnumerateRows(string reportUrl)
        {
            IEnumerable<StrategySummary> hourlySummaries = EnumerateHourlyXmlReportRows(reportUrl).ToList();
            hourlySummaries = hourlySummaries.Where(s => s.Date >= this.dateRange.FromDate && s.Date <= this.dateRange.ToDate);
            var dailyGroups = hourlySummaries.GroupBy(s => new { s.StrategyEid, s.Date });
            foreach (var group in dailyGroups)
            {
                var sum = new StrategySummary
                {
                    StrategyEid = group.Key.StrategyEid,
                    Date = group.Key.Date
                };
                sum.SetStats(group);
                yield return sum;
            }
        }

        private IEnumerable<StrategySummary> EnumerateHourlyXmlReportRows(string reportUrl)
        {
            using (var reader = XmlReader.Create(reportUrl))
            {
                var columnNames = new List<string>(new[] { "campaignID", "dateTime" });
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
                                        var sum = new StrategySummary
                                        {
                                            StrategyEid = row["campaignID"],
                                            Date = DateTime.Parse(row["dateTime"]).AddHours(timezoneOffset).Date,
                                            Cost = decimal.Parse(row["cost"]),
                                            Impressions = int.Parse(row["impressions"]),
                                            Clicks = int.Parse(row["click"]),
                                            PostClickConv = int.Parse(row["sales"]),
                                            PostClickRev = decimal.Parse(row["orderValue"]),
                                        };
                                        yield return sum;
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
