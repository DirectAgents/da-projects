
using Yahoo.Constants.Enums;

namespace Yahoo.Models
{
    internal class ReportPayload
    {
        public ReportOption reportOption { get; set; }

        public int intervalTypeId { get; set; }

        public int dateTypeId { get; set; }

        public string startDate { get; set; }

        public string endDate { get; set; }
    }
}
