using System;
using System.Collections.Generic;
using System.Linq;

namespace Amazon.Entities
{
    public enum SummaryMetricType
    {
        attributedConversions, attributedConversionsSameSKU, attributedSales, attributedSalesSameSKU, attributedUnitsOrdered
    }

    public class StatSummary
    {
        public decimal cost { get; set; }
        public int impressions { get; set; }
        public int clicks { get; set; }
        public int attributedConversions1d { get; set; }
        public int attributedConversions7d { get; set; }
        public int attributedConversions14d { get; set; }
        public int attributedConversions30d { get; set; }
        public int attributedConversions1dSameSKU { get; set; }
        public int attributedConversions7dSameSKU { get; set; }
        public int attributedConversions14dSameSKU { get; set; }
        public int attributedConversions30dSameSKU { get; set; }
        public decimal attributedSales1d { get; set; }
        public decimal attributedSales7d { get; set; }
        public decimal attributedSales14d { get; set; }
        public decimal attributedSales30d { get; set; }
        public decimal attributedSales1dSameSKU { get; set; }
        public decimal attributedSales7dSameSKU { get; set; }
        public decimal attributedSales14dSameSKU { get; set; }
        public decimal attributedSales30dSameSKU { get; set; }
        public int attributedUnitsOrdered1d { get; set; }
        public int attributedUnitsOrdered7d { get; set; }
        public int attributedUnitsOrdered14d { get; set; }
        public int attributedUnitsOrdered30d { get; set; }

        public virtual bool AllZeros()
        {
            return cost == 0 && impressions == 0 && clicks == 0 &&
                   attributedSales1d == 0.0M && attributedSales7d == 0.0M && attributedSales14d == 0.0M && attributedSales30d == 0.0M &&
                   attributedSales1dSameSKU == 0.0M && attributedSales7dSameSKU == 0.0M && attributedSales14dSameSKU == 0.0M && attributedSales30dSameSKU == 0.0M &&
                   attributedConversions1d == 0 && attributedConversions7d == 0 && attributedConversions14d == 0 && attributedConversions30d == 0 &&
                   attributedConversions1dSameSKU == 0 && attributedConversions7dSameSKU == 0 && attributedConversions14dSameSKU == 0 && attributedConversions30dSameSKU == 0 &&
                   attributedUnitsOrdered1d == 0 && attributedUnitsOrdered7d == 0 && attributedUnitsOrdered14d == 0 && attributedUnitsOrdered30d == 0;
        }
    }

    public class AmazonDailySummary : StatSummary
    {
        public Int64 campaignId { get; set; }
        public string campaignName { get; set; }
        public DateTime date { get; set; }
    }

    public class AmazonAdDailySummary : StatSummary
    {
        public string adId { get; set; }
        public string asin { get; set; }
    }

    public class AmazonKeywordDailySummary : AmazonDailySummary
    {
        public string KeywordId { get; set; }
        public string KeywordText { get; set; }
    }

    public class AmazonSearchTermDailySummary : StatSummary
    {
        public string KeywordId { get; set; }
        public string query { get; set; }
        public DateTime date { get; set; }
    }
}
