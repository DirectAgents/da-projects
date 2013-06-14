using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientPortal.Web.Models
{
    public class OfferSummaryReportExportRow
    {
        public int OfferId { get; set; }
        public string Offer { get; set; }
        public string Format { get; set; }
        public decimal Price { get; set; }
        public int Clicks { get; set; }
        public int Leads { get; set; }
        public decimal Spend { get; set; }
        public string Culture { get; set; }
    }

    public class DailySummaryReportExportRow
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public int Impressions { get; set; }
        public int Clicks { get; set; }
        public int Leads { get; set; }
        public float ConversionPct { get; set; }
        public decimal Spend { get; set; }
        public decimal SpendPerClick { get; set; }
        public string Culture { get; set; }
    }

    public class ConversionReportExportRow
    {
        public int ConversionId { get; set; }
        public DateTime Date { get; set; }
        public int OfferId { get; set; }
        public string Offer { get; set; }
        public int SubId { get; set; }
        public decimal Price { get; set; }
        public string Culture { get; set; }
        public decimal ConVal { get; set; }
        public string TransactionId { get; set; }
    }

    public class AffiliateReportExportRow
    {
        public int SubId { get; set; }
        public int OfferId { get; set; }
        public string Offer { get; set; }
        public int Leads { get; set; }
        public decimal Price { get; set; }
        public string Culture { get; set; }
    }

    public class CPMReportExportRow
    {
        public int OfferId { get; set; }
        public string Offer { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal Spend { get; set; }
        public int AccountingStatusId { get; set; }
    }
}