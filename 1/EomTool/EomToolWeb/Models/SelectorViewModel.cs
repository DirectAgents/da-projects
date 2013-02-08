using EomTool.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace EomToolWeb.Models
{
    public class SelectorViewModel
    {
        public string[] AccountingPeriods { get; set; }
        public string AccountingPeriod { get; set; }
        public int? AccountingStatus { get; set; }

        public IQueryable<PublisherPayout> PayoutsQueryable { get; set; }
        public IEnumerable<IGrouping<string, PublisherPayout>> PubGroups { get; set; }
        public IEnumerable<PublisherPayout> PubPayouts { get; set; }
    }
}