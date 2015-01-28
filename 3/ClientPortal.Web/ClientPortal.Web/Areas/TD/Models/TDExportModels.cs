using System;

namespace ClientPortal.Web.Areas.TD.Models
{
    public class RangeStatExportRow
    {
        public string Title { get; set; }
        //public string Range { get; set; }

        public int Impressions { get; set; }
        public int Clicks { get; set; }
        public double CTR { get; set; }

        public int Conversions { get; set; }
        public double ConvRate { get; set; }

        public decimal Spend { get; set; }

        public decimal eCPM { get; set; }
        public decimal eCPC { get; set; }
        public decimal eCPA { get; set; }

        public int Days { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}