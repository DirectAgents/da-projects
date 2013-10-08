using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using CsvHelper;

namespace ClientPortal.Web.Controllers
{
    internal class ControllerHelpers
    {
        internal static DateTime? ParseDate(string dateString, CultureInfo cultureInfo)
        {
            DateTime? date;
            ParseDate(dateString, cultureInfo, out date);
            return date;
        }

        // returns true iff parsed; a null or whitespace dateString qualifies as parsed, resulting in the out parameter being null
        internal static bool ParseDate(string dateString, CultureInfo cultureInfo, out DateTime? date)
        {
            date = null;
            if (String.IsNullOrWhiteSpace(dateString)) return true;

            DateTime parseDate;
            bool parsed = DateTime.TryParse(dateString, cultureInfo, DateTimeStyles.None, out parseDate);
            if (parsed)
                date = parseDate;

            return parsed;
        }

        // returns false if either of the dates couldn't be parsed
        internal static bool ParseDates(string startdate, string enddate, CultureInfo cultureInfo, out DateTime? start, out DateTime? end)
        {
            bool startParsed = ParseDate(startdate, cultureInfo, out start);
            bool endParsed = ParseDate(enddate, cultureInfo, out end);
            return (startParsed && endParsed);
        }

        internal static string DateStamp()
        {
            return DateTime.Now.ToString("yyyyMMdd");
        }

        internal static MemoryStream CsvStream<T>(IEnumerable<T> rows)
            where T : class
        {
            var output = new MemoryStream();
            var writer = new StreamWriter(output);

            writer.WriteLine("Exported by Direct Agents Client Portal");
            writer.WriteLine("on " + DateTime.Now.ToString());
            writer.WriteLine();

            var csv = new CsvWriter(writer);
            csv.WriteRecords<T>(rows);
            writer.Flush();
            output.Position = 0;
            return output;
        }

        internal static void SendEmail(string from, string[] to, string[] cc, string subject, string body, bool isHTML)
        {
            MailMessage message = new MailMessage
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = isHTML,
                From = new MailAddress(from),
            };
            foreach (var item in to.SelectMany(c => c.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)))
                message.To.Add(item);
            Array.ForEach(cc, c => message.CC.Add(c));
            SmtpClient SmtpMailer = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                Timeout = 50000,
                EnableSsl = true
            };
            SmtpMailer.Credentials = new System.Net.NetworkCredential("reporting@directagents.com", "1423qrwe");
            SmtpMailer.Send(message);
        }
    }

    internal class Dates
    {
        public DateTime Today { get; set; }
        public DateTime Yesterday { get; set; }
        public DateTime Latest { get; set; } // the most recent day with stats (e.g. yesterday)
        public DateTime FirstOfWeek { get; set; }
        public DateTime FirstOfLastWeek { get; set; }
        public DateTime LastOfLastWeek { get; set; }
        public DateTime FirstOfMonth { get; set; }
        public DateTime FirstOfLastMonth { get; set; }
        public DateTime LastOfLastMonth { get; set; }
        public DateTime FirstOfYear { get; set; }

        // will be the last day of last month if today's "day" is greater than the number of days in last month
        public DateTime OneMonthAgo { get; set; }

        // note: if (useYesterdayAsLatest==true) and it's the first of the month, we still act as if it's the last day of last month
        //       "first", "last", etc are relative to Latest
        public Dates(bool useYesterdayAsLatest)
        {
            Today = DateTime.Now;
            Yesterday = Today.AddDays(-1);
            Latest = useYesterdayAsLatest ? Yesterday : Today;

            FirstOfWeek = new DateTime(Latest.Year, Latest.Month, Latest.Day);
            while (FirstOfWeek.DayOfWeek != DayOfWeek.Sunday)
            {
                FirstOfWeek = FirstOfWeek.AddDays(-1);
            }
            FirstOfLastWeek = FirstOfWeek.AddDays(-7);
            LastOfLastWeek = FirstOfWeek.AddDays(-1);

            FirstOfMonth = new DateTime(Latest.Year, Latest.Month, 1);
            FirstOfLastMonth = FirstOfMonth.AddMonths(-1);
            LastOfLastMonth = FirstOfMonth.AddDays(-1);

            FirstOfYear = new DateTime(Latest.Year, 1, 1);
            OneMonthAgo = new DateTime(FirstOfLastMonth.Year, FirstOfLastMonth.Month, (Latest.Day < LastOfLastMonth.Day) ? Latest.Day : LastOfLastMonth.Day);
        }
    }
}