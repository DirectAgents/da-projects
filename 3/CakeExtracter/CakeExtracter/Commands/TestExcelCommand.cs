using CakeExtracter.Common;
using CakeExtracter.Reports;
using OfficeOpenXml;
using Spreadsheets;
using System;
using System.ComponentModel.Composition;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class TestExcelCommand : ConsoleCommand
    {
        public override void ResetProperties()
        {
        }

        public TestExcelCommand()
        {
            IsCommand("testExcel");
        }

        public override int Execute(string[] remainingArguments)
        {
            //Test();
            //TestWithSpreadsheets();
            //TestWithLoad();
            TestWithTemplate();
            return 0;
        }

        private void Test()
        {
            using (ExcelPackage p = new ExcelPackage())
            {
                p.Workbook.Properties.Title = "Whatever";

                p.Workbook.Worksheets.Add("sample worksheet");
                var ws = p.Workbook.Worksheets[1];
                ws.Cells[1, 1].Value = "hello";

                //SaveToFile(p);
                SendViaEmail(p);
            }
        }
        private void TestWithSpreadsheets()
        {
            var spreadsheet = new TestSpreadSheet();

            SaveToFile(spreadsheet.ExcelPackage);
            //var attachment = spreadsheet.GetAsAttachment("report123.xlsx");
            //SendViaEmail(attachment);
            spreadsheet.DisposeResources();
        }
        private void TestWithLoad()
        {
            string filename = ConfigurationManager.AppSettings["ExcelTemplate_SearchPPC"]; // SearchPPCtemplate.xlsx
            var file = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename));
            using (var p = new ExcelPackage(file))
            {
                var worksheet = p.Workbook.Worksheets[1];
            }
        }
        private void TestWithTemplate()
        {
            string filename = ConfigurationManager.AppSettings["ExcelTemplate_SearchPPC"]; // SearchPPCtemplate.xlsx
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);

            var spreadsheet = new SearchReportPPC(path);
            var attachment = spreadsheet.GetAsAttachment("reportABC.xlsx");
            SendViaEmail(attachment);

            spreadsheet.DisposeResources();
        }

        private void SaveToFile(ExcelPackage p)
        {
            Byte[] bin = p.GetAsByteArray();
            string path = "c:\\users\\kslesinsky\\downloads\\test123.xlsx";
            File.WriteAllBytes(path, bin);
        }

        private void SendViaEmail(ExcelPackage p)
        {
            using (var ms = new MemoryStream())
            {
                p.SaveAs(ms);
                ms.Seek(0, SeekOrigin.Begin);
                var attachment = new Attachment(ms, "report.xlsx", "application/vnd.ms-excel");
                SendViaEmail(attachment);
            }
        }

        private void SendViaEmail(Attachment attachment)
        {
            var sendTo = "kevin@directagents.com";
            var gmailUsername = ConfigurationManager.AppSettings["GmailEmailer_Username"];
            var gmailPassword = ConfigurationManager.AppSettings["GmailEmailer_Password"];
            var emailer = new GmailEmailer(new NetworkCredential(gmailUsername, gmailPassword));

            var plainView = AlternateView.CreateAlternateViewFromString("this is the plain view", null, "text/plain");
            var htmlView = AlternateView.CreateAlternateViewFromString("this is the <b>html</b> view", null, "text/html");

            emailer.SendEmail("ignored@directagents.com", new[] { sendTo }, null, "test excel attachment", new[] { plainView, htmlView }, attachment);
        }
    }
}
