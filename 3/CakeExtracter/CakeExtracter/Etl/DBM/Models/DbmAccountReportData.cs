using System.Collections.Generic;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.DBM.Models
{
    public class DbmAccountReportData
    {
        public ExtAccount Account { get; set; }

        public List<DbmDailyReportData> DailyDataCollection { get; set; }
    }
}
