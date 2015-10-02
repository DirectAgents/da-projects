using System;
using System.Collections.Generic;
using AdRoll.Entities;

namespace AdRoll.Clients
{
    #region advertisable

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
        public EntityReportResponse Summaries(AdvertisableReportRequest request)
        {
            var result = Execute<EntityReportResponse>(request);
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

    public class DailySummaryReportResponse
    {
        public List<AdrollDailySummary> results { get; set; }
    }
    public class EntityReportResponse
    {
        public List<AdvertisableSummary> results { get; set; }
    }

    #endregion
    #region ad

    public class AdReportClient : ApiClient
    {
        public AdReportClient()
            : base(1, "report", "ad")
        {
        }

        public AdSummaryReportResponse AdSummaries(AdReportRequest request)
        {
            var result = Execute<AdSummaryReportResponse>(request);
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

    public class AdSummaryReportResponse
    {
        public List<AdSummary> results { get; set; }
    }

    #endregion
}
