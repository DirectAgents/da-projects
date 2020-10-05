using System.Collections.Generic;
using Amazon.Entities.Summaries;

using Newtonsoft.Json;

namespace Amazon.Entities.HelperEntities
{
    public class AmazonAttributionReport
    {
        public List<AmazonAttributionSummary> Reports { get; set; }

        public int Count { get; set; }

        public string CursorId { get; set; }
    }
}