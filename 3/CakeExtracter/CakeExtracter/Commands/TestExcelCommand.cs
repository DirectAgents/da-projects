using CakeExtracter.Common;
using CakeExtracter.Reports;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

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
            Test();
            return 0;
        }

        private void Test()
        {
            using (ExcelPackage p = new ExcelPackage())
            {
                p.Workbook.Properties.Title = "Whatever";

                p.Workbook.Worksheets.Add("sample worksheet");
                var ws = p.Workbook.Worksheets[1];
                ws.Cells.Style.Font.Size = 11;
                ws.Cells.Style.Font.Name = "Calibri";

                ws.Cells[1, 1].Value = "hello";

                //SaveToFile(p);
                SendViaEmail(p);
            }
        }

        private void SaveToFile(ExcelPackage p)
        {
            Byte[] bin = p.GetAsByteArray();
            string path = "c:\\users\\kslesinsky\\downloads\\test123.xlsx";
            File.WriteAllBytes(path, bin);
        }

        private void SendViaEmail(ExcelPackage p)
        {
            var sendTo = "kevin@directagents.com";

            var gmailUsername = ConfigurationManager.AppSettings["GmailEmailer_Username"];
            var gmailPassword = ConfigurationManager.AppSettings["GmailEmailer_Password"];
            var emailer = new GmailEmailer(new NetworkCredential(gmailUsername, gmailPassword));

            var plainView = AlternateView.CreateAlternateViewFromString("this is the plain view", null, "text/plain");
            var htmlView = AlternateView.CreateAlternateViewFromString("this is the <b>html</b> view", null, "text/html");

            using (var ms = new MemoryStream())
            {
                p.SaveAs(ms);
                ms.Seek(0, SeekOrigin.Begin);
                Attachment attachment = new Attachment(ms, "report.xlsx", "application/vnd.ms-excel");

                emailer.SendEmail("ignored@directagents.com", new[] { sendTo }, null, "test excel attachment", new[] { plainView, htmlView }, attachment);
            }
        }
    }
}
