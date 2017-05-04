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
                        "TBD", this.dateRange.FromDate, this.dateRange.ToDate);
            try
            {
                var reportData = _afUtility.GetReportData(dateRange.FromDate, dateRange.ToDate, clientId);
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

    public class AdformRowConverter
    {
        private List<List<object>> rows;
        private Dictionary<string, int> columnLookup;

        //public AdformRowConverter(List<List<object>> rows, Dictionary<string, int> columnLookup)
        public AdformRowConverter(ReportData reportData)
        {
            this.rows = reportData.rows;
            this.columnLookup = reportData.CreateColumnLookup();
        }
        private void CheckColumnLookup()
        {
            var columnsNeeded = new string[] { "date", "cost", "impressions", "clicks", "conversions" };
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

        public DailySummary RowToDailySummary(List<object> row)
        {
            try
            {
                var daysum = new DailySummary
                {
                    Date = DateTime.Parse(row[columnLookup["date"]].ToString()),
                    Cost = Convert.ToDecimal(row[columnLookup["cost"]]),
                    Impressions = Convert.ToInt32(row[columnLookup["impressions"]]),
                    Clicks = Convert.ToInt32(row[columnLookup["clicks"]]),
                    PostViewConv = Convert.ToInt32(row[columnLookup["conversions"]])
                };
                return daysum;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }
    }
}
