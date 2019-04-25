using System.Collections.Generic;
using DBM.Parsers.Models;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.DBM.Models
{
    public class DbmAccountCreativeReportData
    {
        public ExtAccount Account { get; set; }

        public IEnumerable<DbmCreativeReportRow> CreativeReportRows { get; set; }
    }
}
