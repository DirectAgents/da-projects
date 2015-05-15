﻿using OfficeOpenXml;
using System.IO;
using System.Net.Mail;

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
    }

    public class Metric
    {
        public Metric(int colNum, string displayName, bool isComputed = false, bool show = true)
        {
            this.ColNum = colNum;
            this.DisplayName = displayName;
            //this.IsComputed = isComputed;
            //this.Show = show;
        }

        public int ColNum { get; set; }
        public string DisplayName { get; set; } // mainly used for charts
        //public bool IsComputed { get; set; }
        //public bool Show { get; set; }
        public string PropName { get; set; }
    }
}
