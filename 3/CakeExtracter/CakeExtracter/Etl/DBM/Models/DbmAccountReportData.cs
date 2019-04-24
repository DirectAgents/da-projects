using System.Collections.Generic;
using DBM.Parsers.Models;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.DBM.Models
{
    public class DbmAccountReportData
    {
        public ExtAccount Account { get; set; }

        //public List<DbmDailyReportData> DailyDataCollection { get; set; }
        public IEnumerable<DbmCreativeReportRow> Data { get; set; }
    }
}
