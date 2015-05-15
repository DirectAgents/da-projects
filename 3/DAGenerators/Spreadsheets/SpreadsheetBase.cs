using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using OfficeOpenXml;
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

        protected void LoadStats<T>(ExcelWorksheet ws, int startingRow, IEnumerable<T> stats, MetricsHolderBase metrics)
        {
            foreach (var metric in metrics.GetNonComputed(true))
            {
                LoadColumnFromStats(ws, startingRow, stats, metric);
            }
        }
        private void LoadColumnFromStats<T>(ExcelWorksheet ws, int startingRow, IEnumerable<T> stats, Metric metric)
        {
            var type = stats.First().GetType();
            ws.Cells[startingRow, metric.ColNum].LoadFromCollection(stats, false, TableStyles.None, BindingFlags.Default, new[] { type.GetProperty(metric.PropName) });
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
