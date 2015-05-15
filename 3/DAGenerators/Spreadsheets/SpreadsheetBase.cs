﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Table;

namespace DAGenerators.Spreadsheets
{
    public abstract class SpreadsheetBase
    {
        public ExcelPackage ExcelPackage { get; set; }
        private MemoryStream MemoryStream { get; set; }

        //public static string ContentType { get { return "application/vnd.ms-excel"; } }
        public static string ContentType { get { return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"; } }

        // NOTE: Be sure to call DisposeResources after calling this (i.e. after sending email)
        public Attachment GetAsAttachment(string filename)
        {
            this.MemoryStream = new MemoryStream();
            this.ExcelPackage.SaveAs(this.MemoryStream);
            this.MemoryStream.Seek(0, SeekOrigin.Begin);
            var attachment = new Attachment(this.MemoryStream, filename, ContentType);
            return attachment;
        }
        public MemoryStream GetAsMemoryStream()
        {
            this.MemoryStream = new MemoryStream();
            this.ExcelPackage.SaveAs(this.MemoryStream);
            this.MemoryStream.Position = 0;
            return this.MemoryStream;
        }

        public void DisposeResources()
        {
            if (this.MemoryStream != null)
                this.MemoryStream.Dispose();
            if (this.ExcelPackage != null)
                this.ExcelPackage.Dispose();
        }

        // --- Possibly for a separate class representing one tab (worksheet) ---

        // returns: # of rows added (in addition to blankRowsInTemplate); negative means blankRows deleted
        protected static int LoadStats<T>(MetricsHolderBase metrics, ExcelWorksheet ws, int startingRow, IEnumerable<T> stats, int blankRowsInTemplate)
        {
            // First, prepare the worksheet-area by adding/removing blank rows - if necessary
            int numRows = stats.Count();
            int blankRowsToInsert = numRows - blankRowsInTemplate;
            if (blankRowsToInsert < 0)
            {
                ws.DeleteRowZ(startingRow, -blankRowsToInsert);
            }
            if (numRows > 0)
            {
                if (blankRowsToInsert > 0)
                {
                    ws.InsertRowZ(startingRow + (blankRowsInTemplate > 0 ? 1 : 0), blankRowsToInsert, startingRow);

                    if (blankRowsInTemplate > 0)
                    {   // copy the formulas from the "blank row" to the newly inserted rows
                        for (int iRow = startingRow + 1; iRow < startingRow + 1 + blankRowsToInsert; iRow++)
                        {
                            ws.Cells[startingRow + ":" + startingRow].Copy(ws.Cells[iRow + ":" + iRow]);
                        }
                    }
                    //else
                    //{   // generate the formulas if there were no blank rows
                    //    for (int i = 0; i < numRows; i++)
                    //        LoadStatsRowFormulas(startingRow + i);
                    //}
                }

                // Now, load the stats
                foreach (var metric in metrics.GetNonComputed(true))
                {
                    LoadColumnFromStats(ws, startingRow, stats, metric);
                }
            }
            return blankRowsToInsert;
        }
        private static void LoadColumnFromStats<T>(ExcelWorksheet ws, int startingRow, IEnumerable<T> stats, Metric metric)
        {
            var type = stats.First().GetType();
            ws.Cells[startingRow, metric.ColNum].LoadFromCollection(stats, false, TableStyles.None, BindingFlags.Default, new[] { type.GetProperty(metric.PropName) });
        }

        protected static void CreateChart(ExcelWorksheet ws, int titleCol, Metric metric1, Metric metric2, int startRow_Stats, int numRows_Stats, int topRow, int leftCol, int chartWidth, int chartHeight, string typeWM, string chartNameSuffix)
        {
            var chart = ws.Drawings.AddChart("chart" + typeWM + chartNameSuffix, eChartType.ColumnClustered);
            chart.SetPosition(topRow, 0, leftCol, 0); // row & column are 0-based
            chart.SetSize(chartWidth, chartHeight);

            chart.Title.Text = typeWM + " " + metric1.DisplayName + " vs. " + metric2.DisplayName;
            chart.Title.Font.Bold = true;
            //chart.Title.Anchor = eTextAnchoringType.Bottom;
            //chart.Title.AnchorCtr = false;

            var series = chart.Series.Add(new ExcelAddress(startRow_Stats, metric1.ColNum, startRow_Stats + numRows_Stats - 1, metric1.ColNum).Address,
                                          new ExcelAddress(startRow_Stats, titleCol, startRow_Stats + numRows_Stats - 1, titleCol).Address);
            //series.HeaderAddress = new ExcelAddress(Row_StatsHeader, column1, Row_StatsHeader, column1);
            series.Header = metric1.DisplayName;

            var chartType2 = chart.PlotArea.ChartTypes.Add(eChartType.LineMarkers);
            chartType2.UseSecondaryAxis = true;
            chartType2.XAxis.Deleted = true;
            var series2 = chartType2.Series.Add(new ExcelAddress(startRow_Stats, metric2.ColNum, startRow_Stats + numRows_Stats - 1, metric2.ColNum).Address,
                                                new ExcelAddress(startRow_Stats, titleCol, startRow_Stats + numRows_Stats - 1, titleCol).Address);
            //series2.HeaderAddress = new ExcelAddress(Row_StatsHeader, column2, Row_StatsHeader, column2);
            series2.Header = metric2.DisplayName;

            chart.Legend.Position = eLegendPosition.Bottom;
        }
    }

    public class Metric
    {
        public Metric(int colNum, string displayName)
        {
            this.ColNum = colNum;
            this.DisplayName = displayName;
        }

        public int ColNum { get; set; }
        public string DisplayName { get; set; } // mainly used for charts
        public string PropName { get; set; }
    }

    public abstract class MetricsHolderBase
    {
        // Should return metrics that have 'propnames' - corresponding to properties of type T when loading
        public abstract IEnumerable<Metric> GetNonComputed(bool includeTitle);

        public abstract IEnumerable<Metric> GetComputed();

        public IEnumerable<Metric> GetAll(bool includeTitle)
        {
            return GetNonComputed(includeTitle).Concat(GetComputed());
        }
    }
}
