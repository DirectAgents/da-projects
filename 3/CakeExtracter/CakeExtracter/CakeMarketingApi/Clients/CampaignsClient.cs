﻿using CakeExtracter.CakeMarketingApi.Entities;

namespace CakeExtracter.CakeMarketingApi.Clients
{
    public class CampaignsClient : ApiClient
    {
        public CampaignsClient()
            : base(6, "export", "Campaigns")
        {
        }

        public CampaignExportResponse Campaigns(CampaignsRequest request)
        {
            var result = TryGetResponse<CampaignExportResponse>(request);
            return result;
        }
    }
}
