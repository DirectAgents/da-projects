namespace Adform.Entities.ReportEntities
{
    public class ReportResponse
    {
        public ReportData reportData { get; set; }
        public int totalRowCount { get; set; }
        public string correlationCode { get; set; }
    }
}