using System;
using System.Collections.Generic;

namespace CakeExtracter.Etl.DSP.Models
{
    internal class AmazonDspDailyReportData
    {
        public DateTime Date { get; set; }

        public List<AdvertiserReport> Advertisers { get; set; }

        public List<OrderReport> Orders { get; set; }

        public List<LineItemReport> LineItems { get; set; }

        public List<CreativeReport> Creatives { get; set; }
    }
}
