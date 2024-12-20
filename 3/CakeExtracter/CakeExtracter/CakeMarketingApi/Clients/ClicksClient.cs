﻿using CakeExtracter.CakeMarketingApi.Entities;

namespace CakeExtracter.CakeMarketingApi.Clients
{
    public class ClicksClient : ApiClient
    {
        public ClicksClient()
            : base(6, "reports", "Clicks")
        {
        }

        public ClickReportResponse Clicks(ClicksRequest request)
        {
            var result = TryGetResponse<ClickReportResponse>(request);
            return result;
        }
    }
}
