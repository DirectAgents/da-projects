using System;
using AdRoll.Entities;

namespace AdRoll.Clients
{
    public class AdvertisableReportClient : ApiClient
    {
        public AdvertisableReportClient()
            : base(1, "report", "advertisable")
        {
        }

        public DailySummaryReportResponse DailySummaries(AdvertisableReportRequest request)
        {
            var result = Execute<DailySummaryReportResponse>(request);
            return result;
        }
    }

    public class AdvertisableReportRequest : ApiRequest
    {
        public AdvertisableReportRequest()
        {
            start_date = DateTime.Today.AddDays(-1).ToString("MM-dd-yyyy");
            end_date = start_date;
            data_format = "date";
        }

        public string campaigns { get; set; }
        public string adgroups { get; set; }
        public string ads { get; set; }
        public string advertisables { get; set; }

        public string data_format { get; set; }

        //public int? past_days { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
    }
}
