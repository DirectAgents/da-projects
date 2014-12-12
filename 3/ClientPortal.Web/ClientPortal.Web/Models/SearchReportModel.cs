using ClientPortal.Data.Contexts;

namespace ClientPortal.Web.Models
{
    public class SearchReportModel
    {
        public SearchProfile SearchProfile { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}