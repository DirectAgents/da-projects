using System;
using System.Collections.Generic;
using EomApp1.Formss.PubRep1.Data;
using System.Globalization;

namespace EomApp1.Formss.PubRep1.Controls
{
    public partial class PubRepTemplate
    {
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
    }
}
