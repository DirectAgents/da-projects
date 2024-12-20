﻿using CakeExtracter.CakeMarketingApi.Entities;

namespace CakeExtracter.CakeMarketingApi.Clients
{
    public class DailySummariesClient : ApiClient
    {
        public DailySummariesClient()
            : base(1, "reports", "DailySummaryExport")
        {
        }

        public ArrayOfDailySummary DailySummaries(DailySummariesRequest request)
        {
            var result = TryGetResponse<ArrayOfDailySummary>(request);
            return result;
        }
    }
}