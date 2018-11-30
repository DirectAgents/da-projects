using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper.Configuration;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.Extracters.SummaryCsvExtracters
{
    public class TDSiteSummaryExtracter : SummaryCsvExtracter<SiteSummary>
    {
        private readonly DateTime? dateOverride;

        public TDSiteSummaryExtracter(ColumnMapping columnMapping, DateTime? dateOverride, StreamReader streamReader = null, string csvFilePath = null)
            : base("SiteSummaries", columnMapping, streamReader, csvFilePath)
        {
            this.dateOverride = dateOverride;
        }

        protected override void AddPropertiesMaps(CsvClassMap classMap, Type classType, ColumnMapping colMap)
        {
            base.AddPropertiesMaps(classMap, classType, colMap);
            CheckAddPropertyMap(classMap, classType, "SiteName", colMap.SiteName);
            CheckAddPropertyMap(classMap, classType, "Date", colMap.Month);
        }

        protected override IEnumerable<SiteSummary> GroupAndEnumerate(List<SiteSummary> csvRows)
        {
            // NOTE: assume that if the Date field is assigned, it's set to the 1st of each month
            if (dateOverride.HasValue)
            {
                foreach (var row in csvRows)
                {
                    row.Date = dateOverride.Value;
                }
            }
            var groupedRows = csvRows.GroupBy(r => new { r.Date, r.SiteName });
            var sums = groupedRows.Select(g => InitSummary(g, g.Key.Date, g.Key.SiteName));
            return sums.ToList();
        }

        private SiteSummary InitSummary(IEnumerable<SiteSummary> sums, DateTime date, string siteName)
        {
            var sum = new SiteSummary
            {
                Date = date,
                SiteName = siteName
            };
            sum.SetStats(sums);
            return sum;
        }
    }
}
