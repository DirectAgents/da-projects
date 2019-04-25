using System.Collections.Generic;
using CakeExtracter.Etl.DBM.Extractors.Parsers.Models;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.DBM.Models
{
    public class DbmAccountLineItemReportData
    {
        public ExtAccount Account { get; set; }

        public IEnumerable<DbmLineItemReportRow> LineItemReportRows { get; set; }
    }
}
