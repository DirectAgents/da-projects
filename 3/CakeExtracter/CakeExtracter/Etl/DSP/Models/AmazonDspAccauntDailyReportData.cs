using DirectAgents.Domain.Entities.CPProg;
using System.Collections.Generic;

namespace CakeExtracter.Etl.DSP.Models
{
    internal class AmazonDspAccauntReportData
    {
        public ExtAccount Account { get; set; }

        public List<AmazonDspDailyReportData> DailyDataCollection { get; set; }
    }
}
