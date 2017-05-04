using System.Collections.Generic;

namespace Adform
{
    public class GetTokenResponse
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string token_type { get; set; }
    }

    public class ReportParams
    {
        public string[] dimensions { get; set; }
        public string[] metrics { get; set; }
        public ReportFilter filter { get; set; }
    }
    public class ReportFilter
    {
        public Dates date { get; set; }
        public int[] client { get; set; }
    }
    public class Dates
    {
        public string from { get; set; }
        public string to { get; set; }
    }

    public class ReportResponse
    {
        public ReportData reportData { get; set; }
        // totalRowCount?
        public string correlationCode { get; set; }
    }
    public class ReportData
    {
        public List<string> columnHeaders { get; set; }
        public object columns { get; set; }
        public List<List<object>> rows { get; set; }
        // totals?

        public Dictionary<string, int> CreateColumnLookup()
        {
            var columnLookup = new Dictionary<string, int>();
            for (int i = 0; i < this.columnHeaders.Count; i++)
                columnLookup[this.columnHeaders[i]] = i;
            return columnLookup;
        }
    }

    //public class AFSummary
    //{
    //    public int impressions { get; set; }
    //    public int clicks { get; set; }
    //}
}
