using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EomTool.Domain.Entities
{
    public class PublisherSummary
    {
        public int affid { get; set; }
        public string PublisherName { get; set; }
        public string Currency { get; set; }
        public decimal PayoutTotal { get; set; }
    }
}
