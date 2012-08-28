using System;
using System.Collections.Generic;
using System.Globalization;

namespace Eom.Common.PublisherReports
{
    public partial class PubRepTemplate
    {
        public PubRepTemplate(PubRepTemplateHtmlMode htmlMode)
        {
            this.HtmlMode = htmlMode;
        }

        public PublisherReportDataSet1.CampaignsPublisherReportDetailsDataTable Data { get; set; }

        public string Publisher { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        Dictionary<string, string> CurrMap = new Dictionary<string, string>
        {
            {"USD", "en-us"},
            {"GBP", "en-gb"},
            {"EUR", "de-de"},
            {"AUD", "en-AU"}
        };

        public string FormatCurrency(string currency, decimal amount)
        {
            return string.Format(CultureInfo.CreateSpecificCulture(CurrMap[currency]), "{0:C}", amount);
        }

        public string FormatDate(DateTime dateTime)
        {
            return String.Format("{0:M/d/yyyy}", dateTime);
        }

        public bool ShowPayInterface { get; set; }

        public PubRepTemplateHtmlMode HtmlMode { get; set; }
    }
}
