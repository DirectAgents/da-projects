using System.Collections.Generic;
using CakeExtracter.Etl.DBM.Extractors.Parsers.Models;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.DBM.Models
{
    /// <summary>
    /// Model for creative report data grouped by account
    /// </summary>
    public class DbmAccountCreativeReportData
    {
        /// <summary>
        /// External account
        /// </summary>
        public ExtAccount Account { get; set; }

        /// <summary>
        /// List of creative report rows
        /// </summary>
        public IEnumerable<DbmCreativeReportRow> CreativeReportRows { get; set; }
    }
}
