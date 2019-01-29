using System;
using System.Collections.Generic;

namespace CakeExtracter.Etl.DSP.Models
{
    internal class AmazonDspDailyReportData
    {
        public DateTime Date { get; set; }

        public List<ReportAdvertiser> Advertisers { get; set; }

        public List<ReportOrder> Orders { get; set; }

        public List<ReportLineItem> LineItems { get; set; }

        public List<ReportCreative> Creatives { get; set; }
    }
}
