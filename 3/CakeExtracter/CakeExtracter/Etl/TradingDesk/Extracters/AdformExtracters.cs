using System;
using System.Collections.Generic;
using System.Linq;
using Adform;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.Extracters
{
    public abstract class AdformApiExtracter<T> : Extracter<T>
    {
        protected readonly AdformUtility _afUtility;
        protected readonly DateRange dateRange;
        protected readonly int clientId;

        public AdformApiExtracter(AdformUtility adformUtility, DateRange dateRange, string clientId)
        {
            this._afUtility = adformUtility;
            this.dateRange = dateRange;
            this.clientId = Int32.Parse(clientId);
        }
    }

    public class AdformDailySummaryExtracter : AdformApiExtracter<DailySummary>
    {
        public AdformDailySummaryExtracter(AdformUtility adformUtility, DateRange dateRange, string clientId)
            : base(adformUtility, dateRange, clientId)
        { }

        protected override void Extract()
        {
            Logger.Info("Extracting DailySummaries from Adform API for ({0}) from {1:d} to {2:d}",
                        this.clientId, this.dateRange.FromDate, this.dateRange.ToDate);
            try
            {
                var parms = _afUtility.CreateReportParams(dateRange.FromDate, dateRange.ToDate, clientId);
                var reportData = _afUtility.GetReportData(parms);
                if (reportData != null)
                {
                    var daysums = EnumerateRows(reportData);
                    Add(daysums);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            End();
        }
        private IEnumerable<DailySummary> EnumerateRows(ReportData reportData)
        {
            var rowConverter = new AdformRowConverter(reportData);
            return rowConverter.EnumerateDailySummaries();
        }
    }

    public class AdformStrategySummaryExtracter : AdformApiExtracter<StrategySummary>
    {
        public AdformStrategySummaryExtracter(AdformUtility adformUtility, DateRange dateRange, string clientId)
            : base(adformUtility, dateRange, clientId)
        { }

        protected override void Extract()
        {
            Logger.Info("Extracting StrategySummaries from Adform API for ({0}) from {1:d} to {2:d}",
                        this.clientId, this.dateRange.FromDate, this.dateRange.ToDate);
            try
            {
                var parms = _afUtility.CreateReportParams(dateRange.FromDate, dateRange.ToDate, clientId, byLineItem: true);
                var reportData = _afUtility.GetReportData(parms);
                if (reportData != null)
                {
                    var sums = EnumerateRows(reportData);
                    Add(sums);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            End();
        }
        private IEnumerable<StrategySummary> EnumerateRows(ReportData reportData)
        {
            var rowConverter = new AdformRowConverter(reportData);
            var stratSums = rowConverter.EnumerateStrategySummaries();

            var sGroups = stratSums.GroupBy(x => new { x.StrategyName, x.Date });
            foreach (var sGroup in sGroups)
            {
                if (sGroup.Count() == 1)
                    yield return sGroup.First();
                else
                {
                    var sum = new StrategySummary
                    {
                        StrategyName = sGroup.Key.StrategyName,
                        Date = sGroup.Key.Date,
                        Cost = sGroup.Sum(x => x.Cost),
                        Impressions = sGroup.Sum(x => x.Impressions),
                        Clicks = sGroup.Sum(x => x.Clicks),
                        PostViewConv = sGroup.Sum(x => x.PostViewConv),
                        PostViewRev = sGroup.Sum(x => x.PostViewRev),
                        //PostClickConv = sGroup.Sum(x => x.PostClickConv),
                        //PostClickRev = sGroup.Sum(x => x.PostClickRev)
                    };
                    yield return sum;
                }
            }
        }
    }

    public class AdformRowConverter
    {
        private List<List<object>> rows;
        private Dictionary<string, int> columnLookup;

        public AdformRowConverter(ReportData reportData)
        {
            this.rows = reportData.rows;
            this.columnLookup = reportData.CreateColumnLookup();
        }
        private void CheckColumnLookup(bool byLineItem = false)
        {
            var columnsNeeded = new List<string> { "date", "cost", "impressions", "clicks", "conversions", "sales" };
            if (byLineItem)
                columnsNeeded.Add("lineItem");
            var missingColumns = columnsNeeded.Where(c => !columnLookup.ContainsKey(c));
            if (missingColumns.Any())
                throw new Exception("Missing columns in lookup: " + String.Join(", ", missingColumns));
        }

        public IEnumerable<DailySummary> EnumerateDailySummaries()
        {
            CheckColumnLookup();
            foreach (var row in rows)
            {
                var daysum = RowToDailySummary(row);
                if (daysum != null)
                    yield return daysum;
            }
        }
        public IEnumerable<StrategySummary> EnumerateStrategySummaries()
        {
            CheckColumnLookup(byLineItem: true);
            foreach (var row in rows)
            {
                var sum = RowToStrategySummary(row);
                if (sum != null)
                    yield return sum;
            }
        }

        public DailySummary RowToDailySummary(List<object> row)
        {
            try
            {
                var daysum = new DailySummary();
                AssignDailySummaryProperties(daysum, row);
                return daysum;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }
        public StrategySummary RowToStrategySummary(List<object> row)
        {
            try
            {
                var sum = new StrategySummary
                {
                    StrategyName = Convert.ToString(row[columnLookup["lineItem"]])
                };
                AssignDailySummaryProperties(sum, row);
                return sum;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        private void AssignDailySummaryProperties(DatedStatsSummaryWithRev summary, List<object> row)
        {
            summary.Date = DateTime.Parse(row[columnLookup["date"]].ToString());
            summary.Cost = Convert.ToDecimal(row[columnLookup["cost"]]);
            summary.Impressions = Convert.ToInt32(row[columnLookup["impressions"]]);
            summary.Clicks = Convert.ToInt32(row[columnLookup["clicks"]]);
            summary.PostViewConv = Convert.ToInt32(row[columnLookup["conversions"]]);
            summary.PostViewRev = Convert.ToDecimal(row[columnLookup["sales"]]);
        }
    }
}
