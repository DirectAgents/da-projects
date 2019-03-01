using Adform.Entities.RequestEntities;

namespace Adform.Entities.ReportEntities
{
    public class ReportParams
    {
        public string[] dimensions { get; set; }
        public string[] metrics { get; set; }
        public object filter { get; set; }
        public Paging paging { get; set; }
        public bool includeRowCount { get; set; }
    }
}