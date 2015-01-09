using System;
using AdRoll.Entities;
using RestSharp.Deserializers;

namespace AdRoll.Clients
{
    public class AdReportClient : ApiClient
    {
        public AdReportClient()
            : base(1, "report", "ad")
        {
        }

        public AdSummaryReportResponse AdSummaries(AdReportRequest request)
        {
            var deserializer = new JsonDeserializer();
            var result = Execute<AdSummaryReportResponse>(request, deserializer);
            return result;
        }
    }

    public class AdReportRequest : ApiRequest
    {
        public AdReportRequest()
        {
            start_date = DateTime.Today.AddDays(-1).ToString("MM-dd-yyyy");
            end_date = start_date;
            data_format = "entity";
        }

        public string campaigns { get; set; }
        public string adgroups { get; set; }
        public string ads { get; set; }
        public string advertisables { get; set; }

        public string start_date { get; set; }
        public string end_date { get; set; }
        public string data_format { get; set; }
    }
}
