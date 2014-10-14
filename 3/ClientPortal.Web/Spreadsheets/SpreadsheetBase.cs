using OfficeOpenXml;
using System.IO;
using System.Net.Mail;

namespace Spreadsheets
{
    public abstract class SpreadsheetBase
    {
        public ExcelPackage ExcelPackage { get; set; }
        public MemoryStream MemoryStream { get; set; }

        // NOTE: Be sure to call DisposeResources after calling this (i.e. after sending email)
        public Attachment GetAsAttachment(string filename)
        {
            this.MemoryStream = new MemoryStream();
            this.ExcelPackage.SaveAs(this.MemoryStream);
            this.MemoryStream.Seek(0, SeekOrigin.Begin);
            var attachment = new Attachment(this.MemoryStream, filename, "application/vnd.ms-excel");
            return attachment;
        }

        public void DisposeResources()
        {
            if (this.MemoryStream != null)
                this.MemoryStream.Dispose();
            if (this.ExcelPackage != null)
                this.ExcelPackage.Dispose();
        }
    }
}
