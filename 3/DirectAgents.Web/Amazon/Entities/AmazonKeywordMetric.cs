using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Entities
{
    public class AmazonKeywordMetric
    {
        public Int64 KeywordId { get; set; }
        public decimal Cost { get; set; }
        public Int64 Clicks { get; set; }
        public int Impressions { get; set; }

    }
}
