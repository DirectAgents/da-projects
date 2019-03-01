using System.Collections.Generic;

namespace Adform.Entities.ReportEntities
{
    public class ReportData
    {
        public List<string> columnHeaders { get; set; }
        public object columns { get; set; }
        public List<List<object>> rows { get; set; }
        // totals?

        public Dictionary<string, int> CreateColumnLookup()
        {
            var columnLookup = new Dictionary<string, int>();
            for (var i = 0; i < columnHeaders.Count; i++)
            {
                columnLookup[columnHeaders[i]] = i;
            }

            return columnLookup;
        }
    }
}