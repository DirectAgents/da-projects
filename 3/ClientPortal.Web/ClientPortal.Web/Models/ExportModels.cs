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
        public string ConversionId { get; set; }
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
        public int Clicks { get; set; }
        public int Leads { get; set; }
        public float ConversionPct { get; set; }
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

    public class SearchStatExportRow_Retail
    {
        public string Channel { get; set; }
        public string Title { get; set; }
        //public string Range { get; set; }

        public int Clicks { get; set; }
        public int Impressions { get; set; }
        public decimal CTR { get; set; }
        public decimal Spend { get; set; }
        public decimal CPC { get; set; }

        public int Orders { get; set; }
        public decimal Revenue { get; set; }
        public decimal Net { get; set; }
        public decimal CPO { get; set; }
        public decimal OrderRate { get; set; }
        public decimal RevenuePerOrder { get; set; }
        public int ROAS { get; set; }

        //public decimal OrdersPerDay { get; set; }
        public int Days { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Network { get; set; }
        public string Device { get; set; }
        public string ClickType { get; set; }
    }

    public class SearchStatExportRow_LeadGen
    {
        public string Channel { get; set; }
        public string Title { get; set; }
        //public string Range { get; set; }

        public int Clicks { get; set; }
        public int Impressions { get; set; }
        public decimal CTR { get; set; }
        public decimal Spend { get; set; }
        public decimal CPC { get; set; }

        public int Leads { get; set; }
        public int Calls { get; set; }
        public int TotalLeads { get; set; }
        public decimal CPL { get; set; }
        public decimal ConvRate { get; set; }

        //public decimal OrdersPerDay { get; set; }
        public int Days { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Network { get; set; }
        public string Device { get; set; }
        public string ClickType { get; set; }
    }
}