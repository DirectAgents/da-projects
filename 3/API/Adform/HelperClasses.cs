
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
        public string date { get; set; }
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
        public object rows { get; set; }
        // totals?
    }
}
