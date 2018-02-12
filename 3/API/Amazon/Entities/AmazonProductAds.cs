using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Entities
{
    public class AmazonProductAds
    {
        public string AdId { get; set; }
        public string AdGroupId { get; set; }
        public Int64 CampaignId { get; set; }
        public string Asin { get; set; }
        public string State { get; set; }
    }
}
