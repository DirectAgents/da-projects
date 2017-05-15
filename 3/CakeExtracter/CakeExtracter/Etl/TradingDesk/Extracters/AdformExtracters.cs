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
                var parms = _afUtility.CreateReportParams(dateRange.FromDate, dateRange.ToDate, clientId, RTBonly: true);
                var basicStatsReportData = _afUtility.GetReportData(parms);
                parms = _afUtility.CreateReportParams(dateRange.FromDate, dateRange.ToDate, clientId, RTBonly: true, basicMetrics: false, convMetrics: true, byAdInteractionType: true);
                var convStatsReportData = _afUtility.GetReportData(parms);

                var daysums = EnumerateRows(basicStatsReportData, convStatsReportData);
                Add(daysums);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            End();
        }
        private IEnumerable<DailySummary> EnumerateRows(ReportData basicStatsReportData, ReportData convStatsReportData)
        {
            var rowConverter = new AdformRowConverter(basicStatsReportData, includeConvMetrics: false);
            var daysumDict = rowConverter.EnumerateDailySummaries().ToDictionary(x => x.Date);

            var convRowConverter = new AdformConvRowConverter(convStatsReportData);
            var convsums = convRowConverter.EnumerateAFConvSummaries();
            var convsumGroups = convsums.GroupBy(x => x.Date);
            // Steps:
            // loop through convsumGroups; get daysum or create blank one
            // then go through daysums that didn't have a convsumGroup
            foreach (var csGroup in convsumGroups)
            {
                DailySummary daysum;
                if (daysumDict.ContainsKey(csGroup.Key))
                    daysum = daysumDict[csGroup.Key];
                else
                    daysum = new DailySummary
                    {
                        Date = csGroup.Key
                    };
                var clickThroughs = csGroup.Where(x => x.AdInteractionType == "Click");
                daysum.PostClickConv = clickThroughs.Sum(x => x.Conversions);
                daysum.PostClickRev = clickThroughs.Sum(x => x.Sales);
                var viewThroughs = csGroup.Where(x => x.AdInteractionType == "Impression");
                daysum.PostViewConv = viewThroughs.Sum(x => x.Conversions);
                daysum.PostViewRev = viewThroughs.Sum(x => x.Sales);
                yield return daysum;
            }
            var convsumDates = convsumGroups.Select(x => x.Key).ToArray();
            var remainingDaySums = daysumDict.Values.Where(ds => !convsumDates.Contains(ds.Date));
            foreach (var daysum in remainingDaySums)
                yield return daysum;
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
                var parms = _afUtility.CreateReportParams(dateRange.FromDate, dateRange.ToDate, clientId, byLineItem: true, RTBonly: true);
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
            var rowConverter = new AdformRowConverter(reportData, includeConvMetrics: true);
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

    public class AdformTDadSummaryExtracter : AdformApiExtracter<TDadSummary>
    {
        public AdformTDadSummaryExtracter(AdformUtility adformUtility, DateRange dateRange, string clientId)
            : base(adformUtility, dateRange, clientId)
        { }

        protected override void Extract()
        {
            Logger.Info("Extracting TDadSummaries from Adform API for ({0}) from {1:d} to {2:d}",
                        this.clientId, this.dateRange.FromDate, this.dateRange.ToDate);
            try
            {
                var parms = _afUtility.CreateReportParams(dateRange.FromDate, dateRange.ToDate, clientId, byBanner: true, RTBonly: true);
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
        private IEnumerable<TDadSummary> EnumerateRows(ReportData reportData)
        {
            var rowConverter = new AdformRowConverter(reportData, includeConvMetrics: true);
            var sums = rowConverter.EnumerateTDadSummaries();

            var sGroups = sums.GroupBy(x => new { x.TDadName, x.Date });
            foreach (var sGroup in sGroups)
            {
                if (sGroup.Count() == 1)
                    yield return sGroup.First();
                else
                {
                    var sum = new TDadSummary
                    {
                        TDadName = sGroup.Key.TDadName,
                        Date = sGroup.Key.Date,
                        Cost = sGroup.Sum(x => x.Cost),
                        Impressions = sGroup.Sum(x => x.Impressions),
                        Clicks = sGroup.Sum(x => x.Clicks),
                        PostViewConv = sGroup.Sum(x => x.PostViewConv),
                        //PostViewRev = sGroup.Sum(x => x.PostViewRev),
                        //PostClickConv = sGroup.Sum(x => x.PostClickConv),
                        //PostClickRev = sGroup.Sum(x => x.PostClickRev)
                    };
                    yield return sum;
                }
            }
        }
    }

    public class AdformConvRowConverter
    {
        private List<List<object>> rows;
        private Dictionary<string, int> columnLookup;

        public AdformConvRowConverter(ReportData reportData)
        {
            this.rows = reportData.rows;
            this.columnLookup = reportData.CreateColumnLookup();
        }
        public IEnumerable<AFConvSummary> EnumerateAFConvSummaries()
        {
            foreach (var row in rows)
            {
                var convsum = RowToConvSummary(row);
                if (convsum != null)
                    yield return convsum;
            }
        }
        public AFConvSummary RowToConvSummary(List<object> row)
        {
            try
            {
                var convsum = new AFConvSummary();
                AssignConvSummaryProperties(convsum, row);
                return convsum;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }
        private void AssignConvSummaryProperties(AFConvSummary summary, List<object> row)
        {
            summary.AdInteractionType = Convert.ToString(row[columnLookup["adInteractionType"]]);
            if (summary.AdInteractionType.StartsWith("Recent "))
                summary.AdInteractionType = summary.AdInteractionType.Substring(7);
            summary.Date = DateTime.Parse(row[columnLookup["date"]].ToString());
            summary.Conversions = Convert.ToInt32(row[columnLookup["conversions"]]);
            summary.Sales = Convert.ToDecimal(row[columnLookup["sales"]]);
        }
    }

    public class AdformRowConverter
    {
        private List<List<object>> rows;
        private Dictionary<string, int> columnLookup;
        private bool includeConvMetrics;

        public AdformRowConverter(ReportData reportData, bool includeConvMetrics)
        {
            this.rows = reportData.rows;
            this.includeConvMetrics = includeConvMetrics;
            this.columnLookup = reportData.CreateColumnLookup();
        }
        private void CheckColumnLookup(bool byLineItem = false, bool byBanner = false)
        {
            var columnsNeeded = new List<string> { "date", "cost", "impressions", "clicks" };
            if (includeConvMetrics)
                columnsNeeded.AddRange(new string[] { "conversions", "sales" });
            if (byLineItem)
                columnsNeeded.Add("lineItem");
            if (byBanner)
                columnsNeeded.Add("banner");
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
        public IEnumerable<TDadSummary> EnumerateTDadSummaries()
        {
            CheckColumnLookup(byBanner: true);
            foreach (var row in rows)
            {
                var sum = RowToTDadSummary(row);
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
        public TDadSummary RowToTDadSummary(List<object> row)
        {
            try
            {
                var sum = new TDadSummary
                {
                    TDadName = Convert.ToString(row[columnLookup["banner"]])
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

        private void AssignDailySummaryProperties(DatedStatsSummary summary, List<object> row)
        {
            summary.Date = DateTime.Parse(row[columnLookup["date"]].ToString());
            summary.Cost = Convert.ToDecimal(row[columnLookup["cost"]]);
            summary.Impressions = Convert.ToInt32(row[columnLookup["impressions"]]);
            summary.Clicks = Convert.ToInt32(row[columnLookup["clicks"]]);
            if (includeConvMetrics)
            {
                summary.PostViewConv = Convert.ToInt32(row[columnLookup["conversions"]]);
                if (summary is DatedStatsSummaryWithRev)
                    ((DatedStatsSummaryWithRev)summary).PostViewRev = Convert.ToDecimal(row[columnLookup["sales"]]);
            }
        }
    }
}
